using System.Diagnostics.CodeAnalysis;
using BeatmapsService.Adapters;
using BeatmapsService.Helpers;
using BeatmapsService.Models.Osu;
using Microsoft.Extensions.Options;

namespace BeatmapsService.Services;

public class OsuService(IOsuAdapter osuAdapter, IOptions<BeatmapOptions> beatmapOptions, ILogger<OsuService> logger) : IOsuService
{
    private const int MaxRequestCountPerMinute = 100;
    private static readonly TimeSpan BackoffTime = TimeSpan.FromSeconds(5);

    private string? _accessToken;
    private DateTimeOffset? _expiresAt;

    private DateTimeOffset? _requestsStart;
    private int _requestCount;

    [MemberNotNull(nameof(_accessToken))]
    private async Task Authenticate(CancellationToken cancellationToken = default)
    {
        if (_accessToken is not null && _expiresAt is not null && DateTimeOffset.UtcNow < _expiresAt)
            return;

        var oauthResponse = await osuAdapter.AuthenticateAsync(
            beatmapOptions.Value.ClientId, 
            beatmapOptions.Value.ClientSecret,
            cancellationToken);

        _accessToken = oauthResponse.AccessToken;
        _expiresAt = DateTimeOffset.UtcNow.AddSeconds(oauthResponse.ExpiresIn);
    }

    private async Task WaitForReady(CancellationToken cancellationToken = default)
    {
        if (_requestsStart is null)
        {
            _requestsStart = DateTimeOffset.UtcNow;
            _requestCount = 0;
        }

        if (_requestCount > MaxRequestCountPerMinute)
        {
            logger.LogWarning("Backing off due to hitting {@RequestCount} in the last minute", _requestCount);
            await Task.Delay(BackoffTime, cancellationToken);
        }

        if ((DateTimeOffset.UtcNow - _requestsStart) > TimeSpan.FromMinutes(1))
        {
            _requestsStart = DateTimeOffset.UtcNow;
            _requestCount = 0;
        }
        
        _requestCount += 1;
    }

    public async Task<BeatmapExtended?> FindBeatmapByIdAsync(int beatmapId, CancellationToken cancellationToken = default)
    {
        await Authenticate(cancellationToken);
        await WaitForReady(cancellationToken);

        var beatmap = await osuAdapter.FindBeatmapByIdAsync(
            beatmapId,
            _accessToken,
            cancellationToken);
        
        if (beatmap is not null)
            logger.LogInformation("Served beatmap ID {@BeatmapId} from API v2", beatmapId);

        return beatmap;
    }

    public async Task<BeatmapsetExtended?> FindBeatmapsetByIdAsync(int beatmapsetId, CancellationToken cancellationToken = default)
    {
        await Authenticate(cancellationToken);
        await WaitForReady(cancellationToken);

        var beatmapset = await osuAdapter.FindBeatmapsetByIdAsync(
            beatmapsetId,
            _accessToken,
            cancellationToken);
        
        if (beatmapset is not null)
            logger.LogInformation("Served beatmapset ID {@BeatmapsetId} from API v2", beatmapsetId);

        return beatmapset;
    }

    public async Task<List<SearchBeatmapset>> SearchBeatmapsetsAsync(
        string? query,
        int? mode,
        int? status,
        int pageSize,
        int page,
        CancellationToken cancellationToken = default)
    {
        await Authenticate(cancellationToken);

        var rankedStatus = status is not null ? RankedHelper.ConvertRankedStatus(status.Value) : null;

        var currentPage = page;
        var pagesRequired = (int)Math.Ceiling(pageSize / 50d);

        var beatmapsets = new List<SearchBeatmapset>();

        // osu! api can do 50 per page at MAX.
        // this ensures page sizes greater than 50 will still work
        while (pagesRequired > 0)
        {
            await WaitForReady(cancellationToken);
            
            var pageBeatmapsets = await osuAdapter.SearchBeatmapsetsAsync(
                query,
                mode,
                rankedStatus,
                currentPage,
                _accessToken,
                cancellationToken);

            beatmapsets.AddRange(pageBeatmapsets);

            // if there's less than 50 beatmapsets, we know that there's no point advancing further
            if (pageBeatmapsets.Count < 50)
                break;

            currentPage += 1;
            pagesRequired -= 1;
        }
        
        logger.LogInformation("Served beatmapset search with {@BeatmapsetCount} items from API", beatmapsets.Count);

        return beatmapsets.Take(pageSize).ToList();
    }
}