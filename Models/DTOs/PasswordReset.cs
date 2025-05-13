namespace MaxsMusicQuiz.Backend.Models.DTOs;

public class PasswordResetRequest
{
    public string Email { get; set; }
}

public class PasswordResetConfirmationRequest
{
    public string Token { get; set; }
    public string NewPassword { get; set; }
}
