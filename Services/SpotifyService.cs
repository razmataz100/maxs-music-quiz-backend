using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MaxsMusicQuiz.Backend.Models.DTOs;
using MaxsMusicQuiz.Backend.Models.Entities;
using MaxsMusicQuiz.Backend.Repositories.Interfaces;
using MaxsMusicQuiz.Backend.Services.Interfaces;

namespace MaxsMusicQuiz.Backend.Services;

public class SpotifyService(IHttpClientFactory httpClientFactory) : ISpotifyService
{
public async Task<List<QuizQuestion>> GenerateQuestionsFromPlaylist(string playlistUrl, int questionCount)
{
    var playlistId = ExtractPlaylistId(playlistUrl);
    var accessToken = await GetAccessTokenAsync(); 
    var client = httpClientFactory.CreateClient();
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

    var url = $"https://api.spotify.com/v1/playlists/{playlistId}/tracks";
    var response = await client.GetAsync(url);
    response.EnsureSuccessStatusCode();

    var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
    var questions = new List<QuizQuestion>();
    var allTracks = json.RootElement.GetProperty("items").EnumerateArray().ToList();
    var random = new Random();
    var shuffledTracks = allTracks.OrderBy(x => random.Next()).ToList();
    var tracksToUse = shuffledTracks.Take(questionCount).ToList();

    foreach (var item in tracksToUse)
    {
        var track = item.GetProperty("track");
        var trackName = track.GetProperty("name").GetString();
        var artistName = track.GetProperty("artists")[0].GetProperty("name").GetString();
        var releaseDate = track.GetProperty("album").GetProperty("release_date").GetString();
        var releaseYear = releaseDate[..4];
        var spotifyUrl = track.GetProperty("external_urls").GetProperty("spotify").GetString();
        var trackId = spotifyUrl.Split('/').Last();
        
        var question = new QuizQuestion
        {
            SongName = trackName,
            ArtistName = artistName,
            SpotifyTrackId = trackId,
            QuestionText = $"In which year was the song '{trackName}' released?",
            CorrectAnswer = releaseYear,
            AnswerChoices = new List<string> { releaseYear }
        };
        
        var falseYears = GenerateFalseYears(int.Parse(releaseYear));
        question.AnswerChoices.AddRange(falseYears);
        
        question.AnswerChoices = question.AnswerChoices.OrderBy(a => random.Next()).ToList();

        questions.Add(question); 
    }

    return questions;
}
    
    private string ExtractPlaylistId(string url)
    {
        var uri = new Uri(url);
        var segments = uri.Segments;
        return segments.Last().TrimEnd('/');
    }

    private async Task<string> GetAccessTokenAsync()
    {
        var clientId = "3223371638164f47b7b7a7c2f11f7c85";
        var clientSecret = "22948b50833347e088df8fb7d84715f4";
        var client = httpClientFactory.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
        var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
        request.Content = new FormUrlEncodedContent(new[] {
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return content.RootElement.GetProperty("access_token").GetString()!;
    }
    
    private List<string> GenerateFalseYears(int correctYear)
    {
        var random = new Random();
        var years = new HashSet<int> { correctYear };

        while (years.Count < 4)
        {
            var offset = random.Next(-10, 11);
            var year = correctYear + offset;
            if (year >= 1950 && year <= DateTime.Now.Year)
            {
                years.Add(year);
            }
        }

        return years.Where(y => y != correctYear).Select(y => y.ToString()).ToList();
    }

}
