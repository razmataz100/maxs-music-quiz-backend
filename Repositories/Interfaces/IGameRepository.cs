using MaxsMusicQuiz.Backend.Models.DTOs;
using MaxsMusicQuiz.Backend.Models.DTOs.Game;
using MaxsMusicQuiz.Backend.Models.Entities;

namespace MaxsMusicQuiz.Backend.Repositories.Interfaces;

public interface IGameRepository
{
    Task<QuizGame> CreateAsync(QuizGame quizGame);
    Task<QuizGame> GetByIdAsync(int gameId);
    Task AddGameHistoryAsync(GameHistory gameHistory);
    Task<IEnumerable<GameWithHighScoreDto>> GetAllWithHighScoresAsync(int userId);
    Task DeleteGameAsync(int gameId);
    Task<bool> ExistsByThemeAsync(string theme);
    Task<IEnumerable<GameHistory>> GetGameHistoryAsync(int gameId, int limit);
}