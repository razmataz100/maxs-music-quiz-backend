namespace MaxsMusicQuiz.Backend.Models.Entities;

public class QuizGame
{
    public int Id { get; set; }
    public string Theme { get; set; }
    public string PlaylistUrl { get; set; }
    public ICollection<QuizQuestion> Questions { get; set; }
    public ICollection<GameHistory> GameHistories { get; set; }
}