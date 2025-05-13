using MaxsMusicQuiz.Backend.Data;
using MaxsMusicQuiz.Backend.Models.DTOs.Leaderboard;
using MaxsMusicQuiz.Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MaxsMusicQuiz.Backend.Repositories
{
    public class LeaderboardRepository(ApplicationDbContext dbContext) : ILeaderboardRepository
    {
        public async Task<List<LeaderboardEntryDto>> GetTotalScoreLeaderboardAsync(int limit, int offset)
        {
            var users = await dbContext.Users
                .OrderByDescending(u => u.TotalScore)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
            var leaderboard = users.Select((u, index) => new LeaderboardEntryDto
            {
                Rank = offset + index + 1,
                UserId = u.Id,
                Username = u.Username,
                ProfilePictureUrl = u.ProfilePictureUrl,
                Score = u.TotalScore
            }).ToList();

            return leaderboard;
        }

        public async Task<List<LeaderboardEntryDto>> GetAverageScoreLeaderboardAsync(int limit, int offset)
        {
            var users = await dbContext.Users
                .Where(u => u.TotalQuestionsAnswered > 0)
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.ProfilePictureUrl,
                    u.TotalScore,
                    u.TotalQuestionsAnswered,
                    AverageScore = (double)u.TotalScore / u.TotalQuestionsAnswered
                })
                .OrderByDescending(u => u.AverageScore)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            return users.Select((u, index) => new LeaderboardEntryDto
            {
                Rank = offset + index + 1,
                UserId = u.Id,
                Username = u.Username,
                ProfilePictureUrl = u.ProfilePictureUrl,
                Score = (int)Math.Round(u.AverageScore * 100)
            }).ToList();
        }

        public async Task<List<LeaderboardEntryDto>> GetGamesCompletedLeaderboardAsync(int limit, int offset)
        {
            var users = await dbContext.Users
                .OrderByDescending(u => u.GameHistories.Count)
                .Skip(offset)
                .Take(limit)
                .Select(u => new 
                {
                    u.Id,
                    u.Username,
                    u.ProfilePictureUrl,
                    GamesCount = u.GameHistories.Count
                })
                .ToListAsync();
            return users.Select((u, index) => new LeaderboardEntryDto
            {
                Rank = offset + index + 1,
                UserId = u.Id,
                Username = u.Username,
                ProfilePictureUrl = u.ProfilePictureUrl,
                Score = u.GamesCount
            }).ToList();
        }

        public async Task<List<LeaderboardEntryDto>> GetGameLeaderboardAsync(int gameId, int limit, int offset)
        {
            var leaderboard = await dbContext.GameHistories
                .Where(gh => gh.QuizGameId == gameId)
                .OrderByDescending(gh => gh.CorrectAnswers)
                .Skip(offset)
                .Take(limit)
                .Select((gh, index) => new LeaderboardEntryDto
                {
                    Rank = offset + index + 1,
                    UserId = gh.UserId,
                    Username = gh.User.Username,
                    ProfilePictureUrl = gh.User.ProfilePictureUrl,
                    Score = gh.CorrectAnswers
                })
                .ToListAsync();

            return leaderboard;
        }

        public async Task<List<LeaderboardEntryDto>> GetTimeBasedLeaderboardAsync(DateTime startDate, int limit)
        {
            var leaderboard = await dbContext.GameHistories
                .Where(gh => gh.PlayedAt >= startDate)
                .GroupBy(gh => gh.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    Username = g.First().User.Username,
                    ProfilePictureUrl = g.First().User.ProfilePictureUrl,
                    TotalScore = g.Sum(gh => gh.CorrectAnswers)
                })
                .OrderByDescending(u => u.TotalScore)
                .Take(limit)
                .ToListAsync();

            return leaderboard.Select((item, index) => new LeaderboardEntryDto
            {
                Rank = index + 1,
                UserId = item.UserId,
                Username = item.Username,
                ProfilePictureUrl = item.ProfilePictureUrl,
                Score = item.TotalScore
            }).ToList();
        }

        public async Task<int> GetUserGlobalRankAsync(int userId)
        {
            var user = await dbContext.Users.FindAsync(userId);
            
            if (user == null)
                return 0;

            var globalRank = await dbContext.Users
                .CountAsync(u => u.TotalScore > user.TotalScore) + 1;

            return globalRank;
        }

        public async Task<List<GameRankingDto>> GetUserGameRankingsAsync(int userId)
        {
            var gameRankings = new List<GameRankingDto>();
            var userGames = await dbContext.GameHistories
                .Where(gh => gh.UserId == userId)
                .GroupBy(gh => gh.QuizGameId)
                .Select(g => new
                {
                    GameId = g.Key,
                    GameTheme = g.First().QuizGame.Theme,
                    Score = g.Max(gh => gh.CorrectAnswers)
                })
                .ToListAsync();

            foreach (var game in userGames)
            {
                var rank = await dbContext.GameHistories
                    .Where(gh => gh.QuizGameId == game.GameId)
                    .GroupBy(gh => gh.UserId)
                    .Select(g => g.Max(gh => gh.CorrectAnswers))
                    .CountAsync(score => score > game.Score) + 1;

                gameRankings.Add(new GameRankingDto
                {
                    GameId = game.GameId,
                    GameTheme = game.GameTheme,
                    Rank = rank,
                    Score = game.Score
                });
            }

            return gameRankings;
        }

        public async Task<int> GetUserGamesCompletedCountAsync(int userId)
        {
            return await dbContext.GameHistories
                .Where(gh => gh.UserId == userId)
                .CountAsync();
        }
    }
}