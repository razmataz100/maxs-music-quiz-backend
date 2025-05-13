using MaxsMusicQuiz.Backend.Models.DTOs;
using MaxsMusicQuiz.Backend.Models.DTOs.User;
using MaxsMusicQuiz.Backend.Models.Entities;
using MaxsMusicQuiz.Backend.Models.Enums;
using MaxsMusicQuiz.Backend.Repositories.Interfaces;
using MaxsMusicQuiz.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MaxsMusicQuiz.Backend.Services;

public class UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
    : IUserService
{
    private static readonly Dictionary<string, (string Email, DateTime ExpiresAt)> ResetTokens = new();
    private readonly string _uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profiles");
    
    public async Task<User?> ValidateUserAsync(string username, string password)
    {
        var user = await userRepository.GetUserByUsernameAsync(username);
        if (user != null && passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Success)
        {
            return user;
        }
        return null;
    }

    public async Task CreateUserAsync(string username, string password, string email)
    {
        var existingUserByUsername = await userRepository.GetUserByUsernameAsync(username);
        if (existingUserByUsername != null)
        {
            throw new InvalidOperationException("Username is already taken.");
        }

        var existingUserByEmail = await userRepository.GetUserByEmailAsync(email);
        if (existingUserByEmail != null)
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        var user = new User
        {
            Username = username,
            Email = email,
            Role = UserRole.User 
        };

        user.PasswordHash = passwordHasher.HashPassword(user, password);

        await userRepository.AddAsync(user);
    }

    public async Task<bool> SendPasswordResetEmailAsync(string email)
    {
        var user = await userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        var token = Guid.NewGuid().ToString();
        ResetTokens[token] = (email, DateTime.UtcNow.AddMinutes(20));

        var sendGridApiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        if (string.IsNullOrEmpty(sendGridApiKey))
        {
            return false;
        }

        var client = new SendGridClient(sendGridApiKey);
        var from = new EmailAddress("rasmussandsjo@gmail.com", "Max's Music Quiz");
        var subject = "Reset Your Password";
        var to = new EmailAddress(email);
        var plainText = $"Hi {user.Username},\n\nHere is your password reset token: {token}\n\nThis token will expire in 20 minutes.";
        var htmlText = $"<p>Hi {user.Username},</p><p>Here is your password reset token:</p><h2>{token}</h2><p>This token will expire in 20 minutes.</p>";

        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainText, htmlText);

        var response = await client.SendEmailAsync(msg);

        return response.StatusCode == System.Net.HttpStatusCode.Accepted;
    }

    public async Task<bool> ResetPasswordAsync(string token, string newPassword)
    {
        if (!ResetTokens.TryGetValue(token, out var entry))
        {
            return false;
        }

        if (DateTime.UtcNow > entry.ExpiresAt)
        {
            ResetTokens.Remove(token);
            return false;
        }

        var user = await userRepository.GetUserByEmailAsync(entry.Email);
        if (user == null)
        {
            return false;
        }

        user.PasswordHash = passwordHasher.HashPassword(user, newPassword);
        await userRepository.UpdateAsync(user);

        ResetTokens.Remove(token);
        return true;
    }
    
    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await userRepository.GetByIdAsync(userId);
    }
    
    public async Task<string> SaveProfilePictureAsync(string userId, IFormFile file)
    {
        if (!Directory.Exists(_uploadsPath))
            Directory.CreateDirectory(_uploadsPath);
    
        var fileName = $"{userId}_{DateTime.Now.Ticks}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(_uploadsPath, fileName);
    
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
    
        var imageUrl = $"/uploads/profiles/{fileName}";
        
        var user = await userRepository.GetByIdAsync(int.Parse(userId));
        user.ProfilePictureUrl = imageUrl;
        await userRepository.UpdateAsync(user);
    
        return imageUrl;
    }

    public async Task<string> GetProfilePictureUrlAsync(string userId)
    {
        var user = await userRepository.GetByIdAsync(int.Parse(userId));
        return user?.ProfilePictureUrl;
    }
    
    public async Task<UserInfoResponse> GetUserInfoAsync(string userId)
    {
        var user = await userRepository.GetByIdAsync(int.Parse(userId));
    
        if (user == null)
            return null;
    
        return new UserInfoResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role.ToString(),
            ProfilePictureUrl = user.ProfilePictureUrl,
            TotalScore = user.TotalScore,
            TotalQuestionsAnswered = user.TotalQuestionsAnswered
        };
    }
    
    public async Task<bool> UpdateUserInfoAsync(string userId, UpdateUserRequest request)
    {
        var user = await userRepository.GetByIdAsync(int.Parse(userId));
    
        if (user == null)
            return false;
        
        if (user.Username != request.Username)
        {
            var existingUser = await userRepository.GetUserByUsernameAsync(request.Username);
            if (existingUser != null)
                throw new InvalidOperationException("Username is already taken.");
        }
        
        if (user.Email != request.Email)
        {
            var existingUser = await userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
                throw new InvalidOperationException("Email is already registered.");
        }
        
        user.Username = request.Username;
        user.Email = request.Email;
    
        await userRepository.UpdateAsync(user);
        return true;
    }
}