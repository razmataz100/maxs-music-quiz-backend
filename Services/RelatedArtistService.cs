using System.Net.Http;
using System.Text.Json;
using MaxsMusicQuiz.Backend.Services.Interfaces;

public class RelatedArtistService : IRelatedArtistService
{
    private readonly HttpClient _httpClient;
    private readonly string _lastFmApiKey = "751b08fab7968dc95e4cf62d9529346e"; // Replace with your actual API key

    public RelatedArtistService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<List<string>> GetRelatedArtistsAsync(string artistName)
    {
        var url = $"http://ws.audioscrobbler.com/2.0/?method=artist.getsimilar" +
                  $"&artist={Uri.EscapeDataString(artistName)}&api_key={_lastFmApiKey}&format=json&limit=3";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        var related = new List<string>();
        foreach (var artist in json.RootElement
                     .GetProperty("similarartists")
                     .GetProperty("artist")
                     .EnumerateArray())
        {
            if (artist.TryGetProperty("name", out var name))
            {
                related.Add(name.GetString()!);
            }
        }

        return related;
    }
}