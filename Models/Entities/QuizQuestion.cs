using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MaxsMusicQuiz.Backend.Models.Entities
{
    public class QuizQuestion
    {
        [Key]
        public int Id { get; set; }
        public string SongName { get; set; }
        public string ArtistName { get; set; } 
        
        public string SpotifyTrackId { get; set; } 
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public List<string> AnswerChoices { get; set; } = new List<string>();
        
        public int QuizGameId { get; set; }
        
        [JsonIgnore]
        public QuizGame QuizGame { get; set; }
    }
}