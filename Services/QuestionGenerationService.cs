

using MaxsMusicQuiz.Backend.Services.Interfaces;

namespace MaxsMusicQuiz.Backend.Services;

public class QuestionGenerationService : IQuestionGenerationService
{
    private readonly Random _random = new Random();

    public List<QuizQuestion> GenerateArtistQuestions(PlaylistResponse playlist, int questionCount, int quizGameId)
    {
        var questions = new List<QuizQuestion>();
        
        // Filter tracks to only those with preview URLs
        var validTracks = playlist.Tracks.Items
            .Where(i => i.Track != null && 
                        !string.IsNullOrEmpty(i.Track.PreviewUrl) && 
                        i.Track.Artists != null && 
                        i.Track.Artists.Any())
            .ToList();
        
        if (!validTracks.Any())
        {
            return questions;
        }
        
        // Get all unique artists from the playlist
        var allArtists = validTracks
            .SelectMany(item => item.Track.Artists)
            .Select(a => a.Name)
            .Distinct()
            .ToList();
        
        // Make sure we have at least 4 artists for multiple choice
        if (allArtists.Count < 4)
        {
            allArtists.AddRange(new[] { 
                "Unknown Artist", 
                "Various Artists", 
                "The Band", 
                "The Orchestra" 
            });
            allArtists = allArtists.Distinct().ToList();
        }
        
        // Get up to the requested number of questions
        int numQuestions = Math.Min(questionCount, validTracks.Count);
        
        // Randomly select tracks
        var selectedTracks = validTracks
            .OrderBy(x => _random.Next())
            .Take(numQuestions)
            .ToList();
        
        // Create a question for each track
        foreach (var trackItem in selectedTracks)
        {
            var track = trackItem.Track;
            var correctArtist = track.Artists.First().Name;
            
            var question = new QuizQuestion
            {
                QuizGameId = quizGameId,
                Text = "Who is the artist of this song?",
                AudioUrl = track.PreviewUrl,
                QuestionType = 1, // GuessArtist
                TimeLimit = 20,
                Answers = new List<QuizAnswer>()
            };
            
            // Add correct answer
            question.Answers.Add(new QuizAnswer
            {
                Text = correctArtist,
                DisplayOrder = 0
            });
            
            // Get 3 random wrong artists
            var wrongArtists = allArtists
                .Where(a => a != correctArtist)
                .OrderBy(x => _random.Next())
                .Take(3)
                .ToList();
            
            // Add wrong answers
            for (int i = 0; i < wrongArtists.Count; i++)
            {
                question.Answers.Add(new QuizAnswer
                {
                    Text = wrongArtists[i],
                    DisplayOrder = i + 1
                });
            }
            
            // Shuffle answers
            var shuffledAnswers = question.Answers.OrderBy(x => _random.Next()).ToList();
            
            // Update display order and find correct answer index
            for (int i = 0; i < shuffledAnswers.Count; i++)
            {
                shuffledAnswers[i].DisplayOrder = i;
                if (shuffledAnswers[i].Text == correctArtist)
                {
                    question.CorrectAnswerIndex = i;
                }
            }
            
            question.Answers = shuffledAnswers;
            questions.Add(question);
        }
        
        return questions;
    }
}