namespace MaxsMusicQuiz.Backend.Models.Entities;

public class QuizGameUser
{
    public int QuizGameId { get; set; }
    public QuizGame QuizGame { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    
}