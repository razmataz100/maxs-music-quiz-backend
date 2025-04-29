using MaxsMusicQuiz.Backend.Extensions;
using MaxsMusicQuiz.Backend.Services.Interfaces;
using MaxsMusicQuiz.WebApi.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaxsMusicQuiz.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GameController(IGameService gameService) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameRequest? createGameRequest)
        {
            if (createGameRequest == null)
                return BadRequest("Invalid game data.");

            var quizGame = await gameService.CreateGameAsync(createGameRequest);

            return Ok(quizGame);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllGames()
        {
            var quizGames = await gameService.GetAllGamesAsync();
            return Ok(quizGames);
        }

        [HttpPost("initiate/{gameId}")]
        public async Task<IActionResult> InitiateGame(int gameId)
        {
            try
            {
                var game = await gameService.InitiateGameAsync(gameId);
                return Ok(new { gameId = game.Id, joinCode = game.JoinCode, status = game.Status });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("game/{joinCode}/users")]
        public async Task<IActionResult> GetUsersInGame(string joinCode)
        {
            var game = await gameService.GetGameByJoinCodeAsync(joinCode);
            if (game == null)
            {
                return NotFound("Game not found.");
            }

            var users = game.QuizGameUsers.Select(qgu => new
            {
                userId = qgu.UserId,
                username = qgu.User.Username
            });

            return Ok(users);
        }

        [HttpPost("join/{joinCode}")]
        public async Task<IActionResult> JoinGame(string joinCode)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var game = await gameService.JoinGameAsync(joinCode, userId);
                
                
                return Ok(new
                {
                    gameId = game.Id,
                    status = game.Status,
                    joinCode = game.JoinCode,
                    userId = userId
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("leave/{joinCode}")]
        public async Task<IActionResult> LeaveGame(string joinCode)
        {
            try
            {
                var userId = HttpContext.GetUserId();
        
                var result = await gameService.LeaveGameAsync(joinCode, userId);

                if (!result)
                {
                    return BadRequest("Unable to leave the game. The game might not exist or you're not a member.");
                }

                return Ok(new
                {
                    message = "Successfully left the game",
                    userId = userId,
                    joinCode = joinCode
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while leaving the game");
            }
        }
    }
}