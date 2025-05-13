namespace MaxsMusicQuiz.Backend.Models.DTOs.Leaderboard
{
    public class LeaderboardEntryDto
    {
        public int Rank { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string ProfilePictureUrl { get; set; }
        public int Score { get; set; }
    }
}