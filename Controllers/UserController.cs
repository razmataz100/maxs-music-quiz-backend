using System.Security.Claims;
using MaxsMusicQuiz.Backend.Models.DTOs;
using MaxsMusicQuiz.Backend.Models.DTOs.Auth;
using MaxsMusicQuiz.Backend.Models.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using MaxsMusicQuiz.Backend.Services.Interfaces;

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
        
        [HttpPost("upload-profile-picture")]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { error = "No file uploaded" });
            
            var userId = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
        
            try
            {
                var imageUrl = await userService.SaveProfilePictureAsync(userId, file);
                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("profile-picture")]
        public async Task<IActionResult> GetProfilePicture()
        {
            var userId = User.FindFirst("userId")?.Value;
            Console.WriteLine($"userId: {userId}");

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var imageUrl = await userService.GetProfilePictureUrlAsync(userId);
            Console.WriteLine($"imageUrl from DB/service: {imageUrl}");

            if (string.IsNullOrEmpty(imageUrl))
                return NotFound();
            
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            Console.WriteLine($"Resolved filePath: {filePath}");

            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine("File does NOT exist.");
                return NotFound();
            }

            Console.WriteLine("File found. Returning image.");

            var contentType = "image/jpeg";
            var extension = Path.GetExtension(filePath).ToLowerInvariant();

            switch (extension)
            {
                case ".png": contentType = "image/png"; break;
                case ".gif": contentType = "image/gif"; break;
                case ".webp": contentType = "image/webp"; break;
            }

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, contentType);
        }
        
        [HttpGet("user-info")]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = User.FindFirst("userId")?.Value;
    
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
    
            var userInfo = await userService.GetUserInfoAsync(userId);
    
            if (userInfo == null)
                return NotFound();
    
            return Ok(userInfo);
        }
        
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserRequest request)
        {
            var userId = User.FindFirst("userId")?.Value;
    
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
    
            try
            {
                var success = await userService.UpdateUserInfoAsync(userId, request);
        
                if (!success)
                    return NotFound();
                
                var userInfo = await userService.GetUserInfoAsync(userId);
                return Ok(userInfo);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}