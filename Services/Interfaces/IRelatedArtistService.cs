namespace MaxsMusicQuiz.Backend.Services.Interfaces;

public interface IRelatedArtistService
{
    Task<List<string>> GetRelatedArtistsAsync(string artistName);
}