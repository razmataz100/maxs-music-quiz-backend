using MaxsMusicQuiz.Backend.Models.DTOs;
using MaxsMusicQuiz.Backend.Models.DTOs.User;
using MaxsMusicQuiz.Backend.Models.Entities;

namespace MaxsMusicQuiz.Backend.Services.Interfaces;

public interface IUserService
{
    Task<User?> ValidateUserAsync(string username, string password);
    Task CreateUserAsync(string username, string password, string email);
    Task<bool> SendPasswordResetEmailAsync(string email);
    Task<bool> ResetPasswordAsync(string token, string newPassword);
    Task<string> SaveProfilePictureAsync(string userId, IFormFile file);
    Task<string> GetProfilePictureUrlAsync(string userId);
    Task<UserInfoResponse> GetUserInfoAsync(string userId);
    Task<bool> UpdateUserInfoAsync(string userId, UpdateUserRequest request);
}