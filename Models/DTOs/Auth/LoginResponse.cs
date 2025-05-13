namespace MaxsMusicQuiz.Backend.Models.DTOs.Auth;

public class LoginResponse
{
    public string Username { get; set; }
    public int UserId { get; set; }
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public string UserRole { get; set; }
}