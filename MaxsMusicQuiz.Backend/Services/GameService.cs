using MaxsMusicQuiz.Backend.Models.Entities;
using MaxsMusicQuiz.Backend.Models.Enums;
using MaxsMusicQuiz.Backend.Repositories.Interfaces;
using MaxsMusicQuiz.Backend.Services.Interfaces;
using MaxsMusicQuiz.WebApi.Models.DTOs;

namespace MaxsMusicQuiz.Backend.Services
{
    public class GameService(IGameRepository gameRepository, IUserRepository userRepository) : IGameService
    {
        public async Task<QuizGame> CreateGameAsync(CreateGameRequest createGameRequest)
        {
            var quizGame = new QuizGame
            {
                Theme = createGameRequest.Theme,
                PlaylistUrl = createGameRequest.PlaylistUrl,
                StartTime = null,
                EndTime = null,
                QuizGameUsers = new List<QuizGameUser>()
            };

            return await gameRepository.CreateAsync(quizGame);
        }

        public async Task<IEnumerable<QuizGame>> GetAllGamesAsync()
        {
            return await gameRepository.GetAllAsync();
        }

        public async Task<QuizGame?> GetGameByIdAsync(string gameId)
        {
            if (!int.TryParse(gameId, out var id))
            {
                throw new ArgumentException("Invalid game ID format.");
            }

            return await gameRepository.GetByIdAsync(id);
        }

        public async Task<QuizGame> InitiateGameAsync(int gameId)
        {
            var game = await gameRepository.GetByIdAsync(gameId);

            if (game == null)
                throw new InvalidOperationException("Game not found.");

            if (game.Status != QuizGameStatus.Created)
                throw new InvalidOperationException("Game already initiated or not in a creatable state.");

            game.JoinCode = GenerateJoinCode();
            game.Status = QuizGameStatus.Waiting;
            await gameRepository.UpdateAsync(game);

            return game;
        }

        private string GenerateJoinCode()
        {
            return Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
        }

        public async Task<QuizGame> JoinGameAsync(string joinCode, int userId)
        {
            var game = await gameRepository.GetByJoinCodeAsync(joinCode);

            if (game == null)
                throw new InvalidOperationException("Game not found or invalid join code.");

            if (game.Status != QuizGameStatus.Waiting)
                throw new InvalidOperationException("Game is not in a state that allows joining.");

            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            if (await gameRepository.IsUserInGameAsync(game.Id, user.Id))
                throw new InvalidOperationException("User is already in the game.");

            await gameRepository.AddUserToGameAsync(game.Id, user.Id);

            return await gameRepository.GetByJoinCodeAsync(joinCode);
        }

        public async Task RemovePlayerFromGameAsync(int gameId, int userId)
        {
            await gameRepository.RemoveQuizGameUserAsync(gameId, userId);
        }

        public async Task<string> GetGameIdForUserAsync(int userId)
        {
            var game = await gameRepository.GetGameByUserIdAsync(userId);
            return game?.Id.ToString() ?? "";
        }

        public async Task<QuizGame?> GetGameByJoinCodeAsync(string joinCode)
        {
            return await gameRepository.GetByJoinCodeAsync(joinCode);
        }

        public async Task<bool> LeaveGameAsync(string joinCode, int userId)
        {
            // Find the game by join code
            var game = await gameRepository.GetByJoinCodeAsync(joinCode);

            if (game == null)
            {
                return false;
            }

            // Remove the user from the game
            await gameRepository.RemoveQuizGameUserAsync(game.Id, userId);

            return true;
        }
    }
}
