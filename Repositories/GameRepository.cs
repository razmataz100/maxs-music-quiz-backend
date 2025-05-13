using MaxsMusicQuiz.Backend.Data;
using MaxsMusicQuiz.Backend.Models.DTOs;
using MaxsMusicQuiz.Backend.Models.DTOs.Game;
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

        public async Task<IEnumerable<GameWithHighScoreDto>> GetAllWithHighScoresAsync(int userId)
        {
            var games = await context.QuizGames
                .Include(g => g.GameHistories)
                .ThenInclude(gh => gh.User)
                .ToListAsync();
            
            var username = await context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Username)
                .FirstOrDefaultAsync();
            
            return games.Select(game => 
            {
                var userHistories = game.GameHistories.Where(gh => gh.UserId == userId).ToList();
                var bestGameHistory = game.GameHistories.OrderByDescending(gh => gh.CorrectAnswers).FirstOrDefault();
                var userBestHistory = userHistories.OrderByDescending(gh => gh.CorrectAnswers).FirstOrDefault();

                return new GameWithHighScoreDto
                {
                    GameId = game.Id,
                    Theme = game.Theme,
                    QuestionsAnswered = userHistories.Sum(gh => gh.QuestionsAnswered),
                    CorrectAnswers = userHistories.Sum(gh => gh.CorrectAnswers),
                    Username = username,
                    HighScore = bestGameHistory?.CorrectAnswers,
                    HighScoreQuestions = bestGameHistory?.QuestionsAnswered,
                    HighScoreUsername = bestGameHistory?.User.Username,
                    HighScoreProfilePictureUrl = bestGameHistory?.User.ProfilePictureUrl,
                    HighScoreDate = bestGameHistory?.PlayedAt,
                    UserBestScore = userBestHistory?.CorrectAnswers,
                    UserBestScoreQuestions = userBestHistory?.QuestionsAnswered,
                    UserHighScoreDate = userBestHistory?.PlayedAt
                };
            }).ToList();
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
        
        public async Task<bool> ExistsByThemeAsync(string theme)
        {
            return await context.QuizGames
                .AnyAsync(g => g.Theme.ToLower() == theme.ToLower());
        }
        
        public async Task<IEnumerable<GameHistory>> GetGameHistoryAsync(int gameId, int limit)
        {
            return await context.GameHistories
                .Include(gh => gh.QuizGame)
                .Include(gh => gh.User)
                .Where(gh => gh.QuizGameId == gameId)
                .OrderByDescending(gh => gh.PlayedAt)
                .Take(limit)
                .ToListAsync();
        }
    }
}