using MaxsMusicQuiz.Backend.Models.DTOs;
using MaxsMusicQuiz.Backend.Models.Entities;

namespace MaxsMusicQuiz.Backend.Repositories.Interfaces;

public interface IGameRepository
{
    Task<QuizGame> CreateAsync(QuizGame quizGame);
    Task<IEnumerable<QuizGame>> GetAllAsync();
    Task<QuizGame> GetByIdAsync(int gameId);
    Task UpdateAsync(QuizGame quizGame);
    Task AddQuestionsAsync(IEnumerable<QuizQuestion> questions);
    Task DeleteQuestionsForGameAsync(int gameId);
    Task AddGameHistoryAsync(GameHistory gameHistory);
    Task<IEnumerable<GameWithHighScore>> GetAllWithHighScoresAsync(int userId);
    Task DeleteGameAsync(int gameId);
    Task<IEnumerable<QuizQuestion>> GetQuestionsByGameIdAsync(int gameId);
}