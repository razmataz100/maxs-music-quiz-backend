namespace MaxsMusicQuiz.Backend.Services.Interfaces;

public interface IUserService
{
    Task<User?> ValidateUserAsync(string username, string password);
    Task CreateUserAsync(string username, string password, string email);
    Task<bool> SendPasswordResetEmailAsync(string email);
    Task<bool> ResetPasswordAsync(string token, string newPassword);
    Task<User?> GetUserByIdAsync(int userId);
    
}