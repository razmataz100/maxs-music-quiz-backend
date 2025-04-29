using MaxsMusicQuiz.Backend.Models.Entities;
using MaxsMusicQuiz.Backend.Models.Enums;

public class User
{
    public int Id { get; set; }
    public UserRole Role { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string Email { get; set; }
    public ICollection<QuizGameUser> QuizGameUsers { get; set; }
}