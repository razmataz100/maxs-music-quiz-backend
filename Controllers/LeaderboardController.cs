using MaxsMusicQuiz.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaxsMusicQuiz.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeaderboardController(ILeaderboardService leaderboardService) : ControllerBase
    {
        [HttpGet("global/total-score")]
        public async Task<IActionResult> GetTotalScoreLeaderboard([FromQuery] int limit = 10, [FromQuery] int offset = 0)
        {
            var leaderboard = await leaderboardService.GetTotalScoreLeaderboardAsync(limit, offset);
            return Ok(leaderboard);
        }

        [HttpGet("global/average-score")]
        public async Task<IActionResult> GetAverageScoreLeaderboard([FromQuery] int limit = 10, [FromQuery] int offset = 0)
        {
            var leaderboard = await leaderboardService.GetAverageScoreLeaderboardAsync(limit, offset);
            return Ok(leaderboard);
        }

        [HttpGet("global/games-completed")]
        public async Task<IActionResult> GetGamesCompletedLeaderboard([FromQuery] int limit = 10, [FromQuery] int offset = 0)
        {
            var leaderboard = await leaderboardService.GetGamesCompletedLeaderboardAsync(limit, offset);
            return Ok(leaderboard);
        }

        [HttpGet("game/{gameId}")]
        public async Task<IActionResult> GetGameLeaderboard(int gameId, [FromQuery] int limit = 10, [FromQuery] int offset = 0)
        {
            var leaderboard = await leaderboardService.GetGameLeaderboardAsync(gameId, limit, offset);
            return Ok(leaderboard);
        }

        [HttpGet("weekly")]
        public async Task<IActionResult> GetWeeklyLeaderboard([FromQuery] int limit = 10)
        {
            var leaderboard = await leaderboardService.GetWeeklyLeaderboardAsync(limit);
            return Ok(leaderboard);
        }

        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyLeaderboard([FromQuery] int limit = 10)
        {
            var leaderboard = await leaderboardService.GetMonthlyLeaderboardAsync(limit);
            return Ok(leaderboard);
        }

        [HttpGet("user/ranking")]
        public async Task<IActionResult> GetUserRanking()
        {
            var userId = User.FindFirst("userId")?.Value;
            
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
                
            var ranking = await leaderboardService.GetUserRankingAsync(userId);
            
            if (ranking == null)
                return NotFound();
                
            return Ok(ranking);
        }
    }
}