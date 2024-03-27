using BeatmapsService.Models.Osu;

namespace BeatmapsService.Adapters;

public interface IOsuAdapter
{
    Task<OAuthResponse> AuthenticateAsync(int clientId, string clientSecret, CancellationToken cancellationToken = default);
    Task<BeatmapExtended?> FindBeatmapByIdAsync(int beatmapId, string accessToken, CancellationToken cancellationToken = default);

    Task<BeatmapsetExtended?> FindBeatmapsetByIdAsync(
        int beatmapsetId,
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<List<SearchBeatmapset>> SearchBeatmapsetsAsync(
        string? query,
        int? mode,
        string? status,
        int page,
        string accessToken,
        CancellationToken cancellationToken = default);
}