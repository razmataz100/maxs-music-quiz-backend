using MaxsMusicQuiz.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaxsMusicQuiz.Backend.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class SpotifyController(ISpotifyService spotifyService) : ControllerBase
{
    [HttpGet("quiz-tracks/{gameId}")]
    public async Task<IActionResult> GetQuizTracks([FromRoute] int gameId)
    {
        var result = await spotifyService.GetQuizQuestionsAsync(gameId);
        return Ok(result);
    }
}