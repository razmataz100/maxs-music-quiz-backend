namespace MaxsMusicQuiz.Backend.Models.DTOs.Game;

public class EndGameRequest
{
    public int UserId { get; set; }
    public int CorrectAnswers { get; set; }
    public int QuestionsAnswered { get; set; }
    
}