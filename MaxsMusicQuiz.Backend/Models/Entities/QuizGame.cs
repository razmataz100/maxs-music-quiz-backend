using MaxsMusicQuiz.Backend.Data.Entities;

public class QuizGame
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int TeamAScore { get; set; }
    public int TeamBScore { get; set; }

    // Navigation property to the join table
    public List<QuizGameUser> Users { get; set; }
}