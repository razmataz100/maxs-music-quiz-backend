namespace MaxsMusicQuiz.Backend.Models.DTOs
{


    public class UserRankingDto
    {
        public int GlobalRank { get; set; }
        public int TotalScore { get; set; }
        public double AverageScore { get; set; }
        public int GamesCompleted { get; set; }
        public List<GameRankingDto> GameRankings { get; set; } = new List<GameRankingDto>();
    }

    public class GameRankingDto
    {
        public int GameId { get; set; }
        public string GameTheme { get; set; }
        public int Rank { get; set; }
        public int Score { get; set; }
    }
}