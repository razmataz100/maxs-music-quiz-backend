using MaxsMusicQuiz.Backend.Models.Entities;
using MaxsMusicQuiz.Backend.Models.Enums;

public class QuizGame
{
    public int Id { get; set; }
    public string Theme { get; set; } = string.Empty;
    public string PlaylistUrl { get; set; } = string.Empty;
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public ICollection<QuizGameUser> QuizGameUsers { get; set; } = new List<QuizGameUser>();
    public string? JoinCode { get; set; }
    public QuizGameStatus Status { get; set; } = QuizGameStatus.Created;
}