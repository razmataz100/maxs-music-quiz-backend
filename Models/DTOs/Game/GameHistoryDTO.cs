namespace MaxsMusicQuiz.Backend.Models.DTOs.Game
{
    public class GameHistoryDTO
    {
        public int GameId { get; set; }
        public string Theme { get; set; }
        public string PlayedAt { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public string Username { get; set; }
        public bool IsCurrentUser { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}