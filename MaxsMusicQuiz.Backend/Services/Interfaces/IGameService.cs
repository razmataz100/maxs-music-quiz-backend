using MaxsMusicQuiz.WebApi.Models.DTOs;

namespace MaxsMusicQuiz.Backend.Services
{
    public interface IGameService
    {
        QuizGame CreateGame(CreateGameRequest createGameRequest);
    }
}