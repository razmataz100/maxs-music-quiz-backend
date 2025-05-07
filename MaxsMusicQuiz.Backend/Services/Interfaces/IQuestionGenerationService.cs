namespace MaxsMusicQuiz.Backend.Services.Interfaces;

public interface IQuestionGenerationService
{
    List<QuizQuestion> GenerateArtistQuestions(PlaylistResponse playlist, int questionCount, int quizGameId);
}