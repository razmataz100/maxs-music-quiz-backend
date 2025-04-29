using MaxsMusicQuiz.Backend.Data.Contexts;
using MaxsMusicQuiz.Backend.Models.Entities;
using MaxsMusicQuiz.Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MaxsMusicQuiz.Backend.Repositories
{
    public class GameRepository(ApplicationDbContext context) : IGameRepository
    {
        public async Task<QuizGame> CreateAsync(QuizGame quizGame)
        {
            await context.QuizGames.AddAsync(quizGame);
            await context.SaveChangesAsync();
            return quizGame;
        }
        
        public async Task<IEnumerable<QuizGame>> GetAllAsync()
        {
            return await context.QuizGames.ToListAsync();
        }
        
        public async Task<QuizGame> GetByIdAsync(int gameId) 
        {
            return await context.QuizGames
                       .FirstOrDefaultAsync(g => g.Id == gameId) 
                   ?? throw new InvalidOperationException("Game not found");
        }
        
        public async Task UpdateAsync(QuizGame quizGame)
        {
            context.QuizGames.Update(quizGame);
            await context.SaveChangesAsync();
        }
        
        public async Task<QuizGame> GetByJoinCodeAsync(string joinCode)
        {
            return await context.QuizGames
                       .Include(qg => qg.QuizGameUsers)
                       .ThenInclude(qgu => qgu.User)
                       .FirstOrDefaultAsync(qg => qg.JoinCode == joinCode) 
                   ?? throw new InvalidOperationException("QuizGame with the specified join code was not found.");
        }

        public async Task<bool> IsUserInGameAsync(int gameId, int userId)
        {
            return await context.QuizGameUsers.AnyAsync(qgu => qgu.QuizGameId == gameId && qgu.UserId == userId);
        }

        public async Task AddUserToGameAsync(int gameId, int userId)
        {
            var quizGameUser = new QuizGameUser
            {
                QuizGameId = gameId,
                UserId = userId
            };
            await context.QuizGameUsers.AddAsync(quizGameUser);
            await context.SaveChangesAsync();
        }
        
        public async Task<QuizGame> GetGameByUserIdAsync(int userId)
        {
            var game = await context.QuizGames
                .FirstOrDefaultAsync(g => g.QuizGameUsers.Any(qgu => qgu.UserId == userId));

            if (game == null)
            {
                throw new InvalidOperationException("No game found for the specified user ID.");
            }

            return game;
        }
        
        public async Task RemoveQuizGameUserAsync(int gameId, int userId)
        {
            var quizGameUser = await context.QuizGameUsers
                .FirstOrDefaultAsync(qgu => qgu.QuizGameId == gameId && qgu.UserId == userId);

            if (quizGameUser != null)
            {
                context.QuizGameUsers.Remove(quizGameUser);
                await context.SaveChangesAsync();
            }
        }
    }
}