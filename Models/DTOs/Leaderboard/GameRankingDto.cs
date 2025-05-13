namespace MaxsMusicQuiz.Backend.Models.DTOs.Leaderboard
{
    public class GameRankingDto
    {
        public int GameId { get; set; }
        public string GameTheme { get; set; }
        public int Rank { get; set; }
        public int Score { get; set; }
    }
}