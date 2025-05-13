using MaxsMusicQuiz.Backend.Models.DTOs;
using MaxsMusicQuiz.Backend.Models.DTOs.Game;
using MaxsMusicQuiz.Backend.Models.Entities;
using MaxsMusicQuiz.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaxsMusicQuiz.Backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class GameController(IGameService gameService) : ControllerBase
    {
        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameRequest? createGameRequest)
        {
            if (createGameRequest == null)
                return BadRequest("Invalid game data.");

            try
            {
                var quizGame = await gameService.CreateGameAsync(createGameRequest);
                return Ok(quizGame);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.StartsWith("Game with theme"))
                    return Conflict(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("all/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetAllGames(int userId)
        {
            try
            {
                var gamesWithHighScores = await gameService.GetAllGamesWithHighScoresAsync(userId);
                return Ok(gamesWithHighScores);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("{gameId}/end-game")]
        [Authorize]
        public async Task<IActionResult> EndGame(int gameId, [FromBody] EndGameRequest endGameRequest)
        {
            if (endGameRequest == null)
                return BadRequest("Invalid game data.");

            try
            {
                await gameService.EndGameAsync(gameId, endGameRequest.UserId, endGameRequest.CorrectAnswers,
                    endGameRequest.QuestionsAnswered);
                return Ok(new { message = "Game history saved successfully" });
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("{gameId}/start-game")]
        [Authorize]
        public async Task<IActionResult> StartGame(int gameId)
        {
            try
            {
                var questions = await gameService.GetGameQuestionsAsync(gameId);

                if (questions == null || !questions.Any())
                    return NotFound("No questions could be generated for this game.");

                return Ok(questions);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{gameId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteGame(int gameId)
        {
            try
            {
                await gameService.DeleteGameAsync(gameId);
                return Ok(new { message = $"Game with ID {gameId} was successfully deleted" });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{gameId}/history/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetGameHistory(int gameId, int userId, [FromQuery] int limit = 3)
        {
            try
            {
                var history = await gameService.GetGameHistoryAsync(gameId, userId, limit);
                return Ok(history);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}