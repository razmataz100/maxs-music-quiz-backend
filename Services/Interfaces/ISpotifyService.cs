using MaxsMusicQuiz.Backend.Models.Entities;

namespace MaxsMusicQuiz.Backend.Services.Interfaces;

public interface ISpotifyService
{
    Task<List<QuizQuestion>> GenerateQuestionsFromPlaylist(string playlistUrl, int questionCount);
}