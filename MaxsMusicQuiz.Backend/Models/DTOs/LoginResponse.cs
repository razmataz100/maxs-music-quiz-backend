namespace MaxsMusicQuiz.Backend.Models.DTOs;

public class LoginResponse
{
    public string Username { get; set; }
    public int UserId { get; set; }
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}