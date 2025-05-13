namespace MaxsMusicQuiz.Backend.Models.DTOs;

public class QuizQuestion
{
    public string QuestionText { get; set; }
    public List<string> AnswerChoices { get; set; }
    public string CorrectAnswer { get; set; }
}