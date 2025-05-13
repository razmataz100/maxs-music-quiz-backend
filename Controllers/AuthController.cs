using MaxsMusicQuiz.Backend.Models.DTOs;
using MaxsMusicQuiz.Backend.Models.DTOs.Auth;
using MaxsMusicQuiz.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaxsMusicQuiz.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ITokenService tokenService, IUserService userService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await userService.ValidateUserAsync(request.Username, request.Password);

        if (user != null)
        {
            var token = tokenService.GenerateToken(user.Username, user.Id, user.Role.ToString());
            var response = new LoginResponse
            {
                Username = user.Username,
                UserId = user.Id,
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(1),
                UserRole = user.Role.ToString()
            };

            return Ok(response);
        }

        return Unauthorized();
    }
}