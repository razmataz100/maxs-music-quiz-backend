namespace MaxsMusicQuiz.Backend.Repositories.Interfaces;

public interface IGameRepository
{
    Task<QuizGame> CreateAsync(QuizGame quizGame);
    Task<IEnumerable<QuizGame>> GetAllAsync();
    Task<QuizGame> GetByIdAsync(int gameId);
    Task UpdateAsync(QuizGame quizGame);
    Task<QuizGame> GetByJoinCodeAsync(string joinCode);
    Task<bool> IsUserInGameAsync(int gameId, int userId);
    Task AddUserToGameAsync(int gameId, int userId);
    Task<QuizGame> GetGameByUserIdAsync(int userId);
    Task RemoveQuizGameUserAsync(int gameId, int userId); 
}