namespace MaxsMusicQuiz.Backend.Models.DTOs.Auth
{
    public class PasswordResetConfirmationRequest
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}