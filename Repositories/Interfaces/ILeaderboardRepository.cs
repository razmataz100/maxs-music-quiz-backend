using MaxsMusicQuiz.Backend.Models.DTOs.Leaderboard;

namespace MaxsMusicQuiz.Backend.Repositories.Interfaces
{
    public interface ILeaderboardRepository
    {
        Task<List<LeaderboardEntryDto>> GetTotalScoreLeaderboardAsync(int limit, int offset);
        Task<List<LeaderboardEntryDto>> GetAverageScoreLeaderboardAsync(int limit, int offset);
        Task<List<LeaderboardEntryDto>> GetGamesCompletedLeaderboardAsync(int limit, int offset);
        Task<List<LeaderboardEntryDto>> GetGameLeaderboardAsync(int gameId, int limit, int offset);
        Task<List<LeaderboardEntryDto>> GetTimeBasedLeaderboardAsync(DateTime startDate, int limit);
        Task<int> GetUserGlobalRankAsync(int userId);
        Task<List<GameRankingDto>> GetUserGameRankingsAsync(int userId);
        Task<int> GetUserGamesCompletedCountAsync(int userId);
    }
}