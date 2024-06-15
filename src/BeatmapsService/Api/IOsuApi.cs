using BeatmapsService.Models.Osu;
using Refit;

namespace BeatmapsService.Api;

public interface IOsuApi
{
    [Post("/oauth/token")]
    Task<OAuthResponse> AuthenticateAsync(
        [Body(BodySerializationMethod.UrlEncoded)] OAuthRequest request,
        CancellationToken cancellationToken = default);

    [Get("/api/v2/beatmaps/{beatmapId}")]
    Task<BeatmapExtended?> FindBeatmapByIdAsync(
        int beatmapId,
        [Authorize] string accessToken,
        CancellationToken cancellationToken = default);
    
    [Get("/api/v2/beatmapsets/{beatmapsetId}")]
    Task<BeatmapsetExtended?> FindBeatmapsetByIdAsync(
        int beatmapsetId,
        [Authorize] string accessToken,
        CancellationToken cancellationToken = default);

    [Get("/api/v2/beatmapsets/search")]
    Task<SearchBeatmapsetResponse> SearchBeatmapsetsAsync(
        [AliasAs("q")] string? query,
        [AliasAs("m")] int? mode,
        [AliasAs("s")] string? status,
        string? sort,
        int page,
        [Authorize] string accessToken,
        CancellationToken cancellationToken = default);
}