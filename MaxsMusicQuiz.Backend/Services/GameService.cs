using MaxsMusicQuiz.Backend.Models.DTOs;
using MaxsMusicQuiz.Backend.Models.Entities;
using MaxsMusicQuiz.Backend.Models.Enums;
using MaxsMusicQuiz.Backend.Repositories.Interfaces;
using MaxsMusicQuiz.Backend.Services.Interfaces;
using MaxsMusicQuiz.WebApi.Models.DTOs;

namespace MaxsMusicQuiz.Backend.Services
{
    public class GameService(
        IGameRepository gameRepository,
        IUserRepository userRepository,
        ISpotifyService spotifyService) : IGameService
    {
        public async Task<QuizGame> CreateGameAsync(CreateGameRequest createGameRequest)
        {
            var quizGame = new QuizGame
            {
                Theme = createGameRequest.Theme,
                PlaylistUrl = createGameRequest.PlaylistUrl
            };
            
            quizGame.Questions = await spotifyService.GenerateQuestionsFromPlaylist(createGameRequest.PlaylistUrl, 10);
            
            var createdGame = await gameRepository.CreateAsync(quizGame);

            return createdGame;
        }
        
        public async Task EndGameAsync(int gameId, int userId, int score, int questionsAnswered)
        {
            var game = await gameRepository.GetByIdAsync(gameId);
            if (game == null)
                throw new ArgumentException("Game not found");
            
            var gameHistory = new GameHistory
            {
                QuizGameId = gameId,
                UserId = userId,
                CorrectAnswers = score,
                QuestionsAnswered = questionsAnswered,
                PlayedAt = DateTime.UtcNow
            };

            await gameRepository.AddGameHistoryAsync(gameHistory);
            
            var user = await userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                user.TotalScore += score;
                user.TotalQuestionsAnswered += questionsAnswered;
                await userRepository.UpdateAsync(user);
            }
        }

        public async Task<IEnumerable<QuizQuestion>> GetGameQuestionsAsync(int gameId)
        {
            var game = await gameRepository.GetByIdAsync(gameId);

            if (game == null)
                throw new ArgumentException("Game not found");

            return game.Questions;
        }

        public async Task<IEnumerable<GameWithHighScore>> GetAllGamesWithHighScoresAsync(int userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            return await gameRepository.GetAllWithHighScoresAsync(userId);
        }
        
        public async Task DeleteGameAsync(int gameId)
        {
            try
            {
                await gameRepository.DeleteGameAsync(gameId);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Failed to delete game: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the game: {ex.Message}", ex);
            }
        }
    }
}