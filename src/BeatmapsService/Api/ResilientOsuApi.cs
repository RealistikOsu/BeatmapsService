using BeatmapsService.Models.Osu;
using Polly;
using Polly.Retry;

namespace BeatmapsService.Api;

public class ResilientOsuApi(IOsuApi osuApi) : IOsuApi
{
    public async Task<OAuthResponse> AuthenticateAsync(OAuthRequest request, CancellationToken cancellationToken = default)
    {
        var resiliencePipeline = new ResiliencePipelineBuilder<OAuthResponse>()
            .AddRetry(new RetryStrategyOptions<OAuthResponse>
            {
                MaxRetryAttempts = 3,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                Delay = TimeSpan.FromSeconds(1),
                ShouldHandle = new PredicateBuilder<OAuthResponse>()
                    .Handle<HttpRequestException>()
            })
            .Build();

        var response = await resiliencePipeline.ExecuteAsync(
            async token => await osuApi.AuthenticateAsync(request, token),
            cancellationToken);

        return response;
    }

    public async Task<BeatmapExtended?> FindBeatmapByIdAsync(int beatmapId, string accessToken, CancellationToken cancellationToken = default)
    {
        var resiliencePipeline = new ResiliencePipelineBuilder<BeatmapExtended?>()
            .AddRetry(new RetryStrategyOptions<BeatmapExtended?>
            {
                MaxRetryAttempts = 3,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                Delay = TimeSpan.FromSeconds(1),
                ShouldHandle = new PredicateBuilder<BeatmapExtended?>()
                    .Handle<HttpRequestException>()
            })
            .Build();

        var response = await resiliencePipeline.ExecuteAsync(
            async token => await osuApi.FindBeatmapByIdAsync(beatmapId, accessToken, token),
            cancellationToken);
        
        return response;
    }

    public async Task<BeatmapsetExtended?> FindBeatmapsetByIdAsync(int beatmapsetId, string accessToken, CancellationToken cancellationToken = default)
    {
        var resiliencePipeline = new ResiliencePipelineBuilder<BeatmapsetExtended?>()
            .AddRetry(new RetryStrategyOptions<BeatmapsetExtended?>
            {
                MaxRetryAttempts = 3,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                Delay = TimeSpan.FromSeconds(1),
                ShouldHandle = new PredicateBuilder<BeatmapsetExtended?>()
                    .Handle<HttpRequestException>()
            })
            .Build();

        var response = await resiliencePipeline.ExecuteAsync(
            async token => await osuApi.FindBeatmapsetByIdAsync(beatmapsetId, accessToken, token),
            cancellationToken);

        return response;
    }

    public async Task<SearchBeatmapsetResponse> SearchBeatmapsetsAsync(
        string? query,
        int? mode,
        string? status,
        string? sort,
        int page,
        string accessToken,
        CancellationToken cancellationToken = default)
    {
        var resiliencePipeline = new ResiliencePipelineBuilder<SearchBeatmapsetResponse>()
            .AddRetry(new RetryStrategyOptions<SearchBeatmapsetResponse>
            {
                MaxRetryAttempts = 3,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                Delay = TimeSpan.FromSeconds(1),
                ShouldHandle = new PredicateBuilder<SearchBeatmapsetResponse>()
                    .Handle<HttpRequestException>()
            })
            .Build();

        var response = await resiliencePipeline.ExecuteAsync(
            async token => await osuApi.SearchBeatmapsetsAsync(
                query,
                mode,
                status,
                sort,
                page,
                accessToken,
                token),
            cancellationToken);

        return response;
    }
}