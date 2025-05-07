using MaxsMusicQuiz.Backend.Data.Contexts;
using MaxsMusicQuiz.Backend.Models.DTOs;
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
                       .Include(g => g.Questions)
                       .FirstOrDefaultAsync(g => g.Id == gameId)
                   ?? throw new InvalidOperationException("Game not found");
        }

        public async Task UpdateAsync(QuizGame quizGame)
        {
            context.QuizGames.Update(quizGame);
            await context.SaveChangesAsync();
        }

        public async Task AddQuestionsAsync(IEnumerable<QuizQuestion> questions)
        {
            await context.QuizQuestions.AddRangeAsync(questions);
            await context.SaveChangesAsync();
        }

        public async Task DeleteQuestionsForGameAsync(int gameId)
        {
            var questions = await context.QuizQuestions
                .Where(q => q.QuizGameId == gameId)
                .ToListAsync();

            if (questions.Any())
            {
                context.QuizQuestions.RemoveRange(questions);
                await context.SaveChangesAsync();
            }
        }

        public async Task AddGameHistoryAsync(GameHistory gameHistory)
        {
            await context.GameHistories.AddAsync(gameHistory);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<GameWithHighScore>> GetAllWithHighScoresAsync(int userId)
        {
            return await context.QuizGames
                .Select(game => new GameWithHighScore
                {
                    GameId = game.Id,
                    Theme = game.Theme,
                    QuestionsAnswered = game.GameHistories
                        .Where(gh => gh.UserId == userId)
                        .Sum(gh => gh.QuestionsAnswered),
                    CorrectAnswers = game.GameHistories
                        .Where(gh => gh.UserId == userId)
                        .Sum(gh => gh.CorrectAnswers),
                    Username = context.Users
                        .Where(u => u.Id == userId)
                        .Select(u => u.Username)
                        .FirstOrDefault(),
                    // Add these fields:
                    HighScore = game.GameHistories
                        .OrderByDescending(gh => gh.CorrectAnswers)
                        .Select(gh => (int?)gh.CorrectAnswers)
                        .FirstOrDefault(),
                    HighScoreUsername = game.GameHistories
                        .OrderByDescending(gh => gh.CorrectAnswers)
                        .Select(gh => gh.User.Username)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }

        public async Task DeleteGameAsync(int gameId)
        {
            var gameHistories = await context.GameHistories
                .Where(gh => gh.QuizGameId == gameId)
                .ToListAsync();

            if (gameHistories.Any())
            {
                context.GameHistories.RemoveRange(gameHistories);
            }
            
            var questions = await context.QuizQuestions
                .Where(q => q.QuizGameId == gameId)
                .ToListAsync();

            if (questions.Any())
            {
                context.QuizQuestions.RemoveRange(questions);
            }
            
            var game = await context.QuizGames.FindAsync(gameId);
            if (game != null)
            {
                context.QuizGames.Remove(game);
            }
            else
            {
                throw new InvalidOperationException($"Game with ID {gameId} not found");
            }
            
            await context.SaveChangesAsync();
        }
        
        public async Task<IEnumerable<QuizQuestion>> GetQuestionsByGameIdAsync(int gameId)
        {
            return await context.QuizQuestions
                .Where(q => q.QuizGameId == gameId)
                .ToListAsync();
        }
    }
}