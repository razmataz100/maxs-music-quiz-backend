using MaxsMusicQuiz.Backend.Models.Entities;

namespace MaxsMusicQuiz.Backend.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int userId);
    Task<User?> GetUserByUsernameAsync(string username);
    Task AddAsync(User user);
    Task<User?> GetUserByEmailAsync(string email);
    Task UpdateAsync(User user);
    
}
