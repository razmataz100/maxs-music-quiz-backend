namespace MaxsMusicQuiz.Backend.Data.Entities;

public class User
{
    public int Id { get; set; }
    public string Role { get; set; } 
    public string Nickname { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string ProfilePictureUrl { get; set; }
    public List<QuizGameUser> QuizGames { get; set; }
}
