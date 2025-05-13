using MaxsMusicQuiz.Backend.Data;
using MaxsMusicQuiz.Backend.Models.Entities;
using MaxsMusicQuiz.Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace MaxsMusicQuiz.Backend.Repositories;

public class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetByIdAsync(int userId)
    {
        return await dbContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
    }

    public async Task AddAsync(User user)
    {
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        Console.WriteLine($"Email parameter: {email}");
        return await dbContext.Users
            .SingleOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task UpdateAsync(User user)
    {
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();
    }
}