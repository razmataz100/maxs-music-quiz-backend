using System.ComponentModel.DataAnnotations;
using MaxsMusicQuiz.Backend.Models.Enums;

namespace MaxsMusicQuiz.Backend.Models.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    public UserRole Role { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string Email { get; set; }
    public int TotalScore { get; set; } = 0;
    public int TotalQuestionsAnswered { get; set; } = 0;
    public ICollection<GameHistory> GameHistories { get; set; }
}