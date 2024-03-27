using BeatmapsService.Caching;
using BeatmapsService.Helpers;
using BeatmapsService.Models.Osu;
using Microsoft.Extensions.Caching.Distributed;

namespace BeatmapsService.Services;

public class CachingOsuService(IOsuService osuService, ICache cache, ILogger<CachingOsuService> logger) : IOsuService
{
    private const string BeatmapKey = "beatmaps";
    private const string BeatmapsetKey = "beatmapsets";

    public async Task<BeatmapExtended?> FindBeatmapByIdAsync(int beatmapId, CancellationToken cancellationToken = default)
    {
        var (item, created) = await cache.GetOrCreateAsync(
            $"{BeatmapKey}/{beatmapId}",
            async options =>
            {
                var response = await osuService.FindBeatmapByIdAsync(beatmapId, cancellationToken);
                if (response is null)
                    return null;

                options.SetAbsoluteExpiration(BeatmapHelper.GetCacheExpiry(response.Ranked));

                return response;
            },
            cancellationToken);
        
        if (!created)
            logger.LogInformation("Served beatmap ID {@BeatmapId} from cache", beatmapId);

        return item;
    }

    public async Task<BeatmapsetExtended?> FindBeatmapsetByIdAsync(int beatmapsetId, CancellationToken cancellationToken = default)
    {
        var (item, created) = await cache.GetOrCreateAsync(
            $"{BeatmapsetKey}/{beatmapsetId}",
            async options =>
            {
                var response = await osuService.FindBeatmapsetByIdAsync(beatmapsetId, cancellationToken);
                if (response is null)
                    return null;

                options.SetAbsoluteExpiration(BeatmapHelper.GetCacheExpiry(response.Ranked));

                return response;
            },
            cancellationToken);
        
        if (!created)
            logger.LogInformation("Served beatmapset ID {@BeatmapsetId} from cache", beatmapsetId);
        
        return item;
    }

    // TODO: figure out if there's a reasonable way to cache this
    public Task<List<SearchBeatmapset>> SearchBeatmapsetsAsync(
        string? query,
        int? mode,
        int? status,
        int pageSize,
        int page,
        CancellationToken cancellationToken = default)
    {
        return osuService.SearchBeatmapsetsAsync(query, mode, status, pageSize, page, cancellationToken);
    }
}