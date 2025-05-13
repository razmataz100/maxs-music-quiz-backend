using MaxsMusicQuiz.Backend.Models.DTOs;
using MaxsMusicQuiz.Backend.Models.DTOs.Game;
using MaxsMusicQuiz.Backend.Models.Entities;

namespace MaxsMusicQuiz.Backend.Services.Interfaces
{
    public interface IGameService
    {
        Task<QuizGame> CreateGameAsync(CreateGameRequest createGameRequest);
        Task EndGameAsync(int gameId, int userId, int score, int questionsAnswered);
        Task<IEnumerable<GameWithHighScoreDto>> GetAllGamesWithHighScoresAsync(int userId);
        Task DeleteGameAsync(int gameId);
        Task<IEnumerable<QuizQuestion>> GetGameQuestionsAsync(int gameId);
        Task<IEnumerable<GameHistoryDTO>> GetGameHistoryAsync(int gameId, int userId, int limit);

    }
}