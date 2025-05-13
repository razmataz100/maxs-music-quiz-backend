namespace MaxsMusicQuiz.Backend.Models.DTOs.User
{
    public class UserInfoResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string ProfilePictureUrl { get; set; }
        public int TotalScore { get; set; }
        public int TotalQuestionsAnswered { get; set; }
        public double AverageScore => TotalQuestionsAnswered > 0 
            ? Math.Round((double)TotalScore / TotalQuestionsAnswered, 2) 
            : 0;
    }
}