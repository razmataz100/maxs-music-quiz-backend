using MaxsMusicQuiz.Backend.Models.DTOs;
using MaxsMusicQuiz.Backend.Models.DTOs.Leaderboard;

namespace MaxsMusicQuiz.Backend.Services.Interfaces
{
    public interface ILeaderboardService
    {
        Task<List<LeaderboardEntryDto>> GetTotalScoreLeaderboardAsync(int limit, int offset);
        Task<List<LeaderboardEntryDto>> GetAverageScoreLeaderboardAsync(int limit, int offset);
        Task<List<LeaderboardEntryDto>> GetGamesCompletedLeaderboardAsync(int limit, int offset);
        Task<List<LeaderboardEntryDto>> GetGameLeaderboardAsync(int gameId, int limit, int offset);
        Task<List<LeaderboardEntryDto>> GetWeeklyLeaderboardAsync(int limit);
        Task<List<LeaderboardEntryDto>> GetMonthlyLeaderboardAsync(int limit);
        Task<UserRankingDto> GetUserRankingAsync(string userId);
    }
}