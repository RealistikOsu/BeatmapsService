using BeatmapsService.Models.Osu;

namespace BeatmapsService.Services;

public interface IOsuService
{
    Task<BeatmapExtended?> FindBeatmapByIdAsync(int beatmapId, CancellationToken cancellationToken = default);
    Task<BeatmapsetExtended?> FindBeatmapsetByIdAsync(int beatmapsetId, CancellationToken cancellationToken = default);

    Task<List<SearchBeatmapset>> SearchBeatmapsetsAsync(
        string? query,
        int? mode,
        int? status,
        int pageSize,
        int page,
        CancellationToken cancellationToken = default);
}