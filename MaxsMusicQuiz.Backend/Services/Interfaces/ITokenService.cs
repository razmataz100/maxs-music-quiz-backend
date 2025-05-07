namespace MaxsMusicQuiz.Backend.Services.Interfaces;

public interface ITokenService
{
    string GenerateToken(string username, int userId, string userRole);
}