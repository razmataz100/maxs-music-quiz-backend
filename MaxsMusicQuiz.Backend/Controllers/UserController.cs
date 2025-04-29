using MaxsMusicQuiz.Backend.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using MaxsMusicQuiz.Backend.Services.Interfaces;
using MaxsMusicQuiz.WebApi.Models.DTOs;

namespace MaxsMusicQuiz.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                await userService.CreateUserAsync(registerRequest.Username, registerRequest.Password, registerRequest.Email);
                return Ok(new { message = "User created successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetRequest request)
        {
            var result = await userService.SendPasswordResetEmailAsync(request.Email);
            if (result)
            {
                return Ok("Password reset link has been sent to your email.");
            }

            return BadRequest("No user found with that email address.");
        }

        [HttpPost("reset-password-confirmation")]
        public async Task<IActionResult> ResetPasswordConfirmation([FromBody] PasswordResetConfirmationRequest request)
        {
            var result = await userService.ResetPasswordAsync(request.Token, request.NewPassword);
            if (result)
            {
                return Ok("Your password has been reset successfully.");
            }

            return BadRequest("Invalid or expired reset token.");
        }
    }
}