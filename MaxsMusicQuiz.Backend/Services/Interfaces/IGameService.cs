using MaxsMusicQuiz.Backend.Models.DTOs;
using MaxsMusicQuiz.Backend.Models.Entities;
using MaxsMusicQuiz.WebApi.Models.DTOs;

namespace MaxsMusicQuiz.Backend.Services.Interfaces
{
    public interface IGameService
    {
        Task<QuizGame> CreateGameAsync(CreateGameRequest createGameRequest);
        Task EndGameAsync(int gameId, int userId, int score, int questionsAnswered);
        Task<IEnumerable<GameWithHighScore>> GetAllGamesWithHighScoresAsync(int userId);
        Task DeleteGameAsync(int gameId);
        Task<IEnumerable<QuizQuestion>> GetGameQuestionsAsync(int gameId);

    }
}