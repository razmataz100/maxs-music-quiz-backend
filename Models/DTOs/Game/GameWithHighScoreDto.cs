namespace MaxsMusicQuiz.Backend.Models.DTOs.Game;

public class GameWithHighScoreDto
{
    public int GameId { get; set; }
    public string Theme { get; set; }
    public int? QuestionsAnswered { get; set; }
    public int? CorrectAnswers { get; set; }
    public string? Username { get; set; }
    public int? HighScore { get; set; }
    public int? HighScoreQuestions { get; set; } 
    public string? HighScoreUsername { get; set; }
    public string? HighScoreProfilePictureUrl { get; set; }
    public DateTime? HighScoreDate { get; set; } 
    public DateTime? UserHighScoreDate { get; set; }
    public int? UserBestScore { get; set; }
    public int? UserBestScoreQuestions { get; set; }
}