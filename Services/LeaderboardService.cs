
using MaxsMusicQuiz.Backend.Models.DTOs;
using MaxsMusicQuiz.Backend.Models.DTOs.Leaderboard;
using MaxsMusicQuiz.Backend.Repositories.Interfaces;
using MaxsMusicQuiz.Backend.Services.Interfaces;

namespace MaxsMusicQuiz.Backend.Services
{
    public class LeaderboardService(
        ILeaderboardRepository leaderboardRepository,
        IUserRepository userRepository)
        : ILeaderboardService
    {
        public async Task<List<LeaderboardEntryDto>> GetTotalScoreLeaderboardAsync(int limit, int offset)
        {
            return await leaderboardRepository.GetTotalScoreLeaderboardAsync(limit, offset);
        }

        public async Task<List<LeaderboardEntryDto>> GetAverageScoreLeaderboardAsync(int limit, int offset)
        {
            return await leaderboardRepository.GetAverageScoreLeaderboardAsync(limit, offset);
        }

        public async Task<List<LeaderboardEntryDto>> GetGamesCompletedLeaderboardAsync(int limit, int offset)
        {
            return await leaderboardRepository.GetGamesCompletedLeaderboardAsync(limit, offset);
        }

        public async Task<List<LeaderboardEntryDto>> GetGameLeaderboardAsync(int gameId, int limit, int offset)
        {
            return await leaderboardRepository.GetGameLeaderboardAsync(gameId, limit, offset);
        }

        public async Task<List<LeaderboardEntryDto>> GetWeeklyLeaderboardAsync(int limit)
        {
            var weekAgo = DateTime.UtcNow.AddDays(-7);
            return await leaderboardRepository.GetTimeBasedLeaderboardAsync(weekAgo, limit);
        }

        public async Task<List<LeaderboardEntryDto>> GetMonthlyLeaderboardAsync(int limit)
        {
            var monthAgo = DateTime.UtcNow.AddDays(-30);
            return await leaderboardRepository.GetTimeBasedLeaderboardAsync(monthAgo, limit);
        }

        public async Task<UserRankingDto> GetUserRankingAsync(string userId)
        {
            var parsedUserId = int.Parse(userId);
            var user = await userRepository.GetByIdAsync(parsedUserId);
            
            if (user == null)
                return null;

            var globalRank = await leaderboardRepository.GetUserGlobalRankAsync(parsedUserId);
            var gameRankings = await leaderboardRepository.GetUserGameRankingsAsync(parsedUserId);
            var gamesCompleted = await leaderboardRepository.GetUserGamesCompletedCountAsync(parsedUserId);

            return new UserRankingDto
            {
                GlobalRank = globalRank,
                TotalScore = user.TotalScore,
                AverageScore = user.TotalQuestionsAnswered > 0 
                    ? (double)user.TotalScore / user.TotalQuestionsAnswered 
                    : 0,
                GamesCompleted = gamesCompleted,
                GameRankings = gameRankings
            };
        }
    }
}