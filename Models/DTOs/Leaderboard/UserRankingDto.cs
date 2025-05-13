namespace MaxsMusicQuiz.Backend.Models.DTOs.Leaderboard
{
    public class UserRankingDto
    {
        public int GlobalRank { get; set; }
        public int TotalScore { get; set; }
        public double AverageScore { get; set; }
        public int GamesCompleted { get; set; }
        public List<GameRankingDto> GameRankings { get; set; } = new List<GameRankingDto>();
    }
}