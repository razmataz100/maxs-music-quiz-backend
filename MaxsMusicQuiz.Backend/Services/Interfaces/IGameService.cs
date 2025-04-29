using MaxsMusicQuiz.WebApi.Models.DTOs;

namespace MaxsMusicQuiz.Backend.Services.Interfaces
{
    public interface IGameService
    {
        Task<QuizGame> CreateGameAsync(CreateGameRequest createGameRequest);
        Task<IEnumerable<QuizGame>> GetAllGamesAsync();
        Task<QuizGame> InitiateGameAsync(int gameId);
        Task<QuizGame> JoinGameAsync(string joinCode, int userId);
        Task<QuizGame?> GetGameByJoinCodeAsync(string joinCode);
        Task<bool> LeaveGameAsync(string joinCode, int userId);
    }
}