namespace MaxsMusicQuiz.Backend.Models.DTOs;

public class GameWithHighScore
{
    public int GameId { get; set; }
    public string Theme { get; set; }
    public int? QuestionsAnswered { get; set; }
    public int? CorrectAnswers { get; set; }
    public string? Username { get; set; }
    public int? HighScore { get; set; }
    public string? HighScoreUsername { get; set; }
}