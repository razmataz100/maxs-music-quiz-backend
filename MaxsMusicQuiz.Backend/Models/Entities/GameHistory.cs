namespace MaxsMusicQuiz.Backend.Models.Entities;

public class GameHistory
{
    public int Id { get; set; }
    public int QuizGameId { get; set; }
    public QuizGame QuizGame { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int CorrectAnswers { get; set; }
    public int QuestionsAnswered { get; set; }
    public DateTime PlayedAt { get; set; }
}