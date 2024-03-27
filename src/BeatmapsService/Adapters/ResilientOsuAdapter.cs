using BeatmapsService.Models.Osu;
using Polly;
using Polly.Retry;

namespace BeatmapsService.Adapters;

public class ResilientOsuAdapter(IOsuAdapter osuAdapter) : IOsuAdapter
{
    public async Task<OAuthResponse> AuthenticateAsync(int clientId, string clientSecret, CancellationToken cancellationToken = default)
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
            async token => await osuAdapter.AuthenticateAsync(clientId, clientSecret, token),
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
            async token => await osuAdapter.FindBeatmapByIdAsync(beatmapId, accessToken, token),
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
            async token => await osuAdapter.FindBeatmapsetByIdAsync(beatmapsetId, accessToken, token),
            cancellationToken);

        return response;
    }

    public async Task<List<SearchBeatmapset>> SearchBeatmapsetsAsync(
        string? query,
        int? mode,
        string? status,
        int page,
        string accessToken,
        CancellationToken cancellationToken = default)
    {
        var resiliencePipeline = new ResiliencePipelineBuilder<List<SearchBeatmapset>>()
            .AddRetry(new RetryStrategyOptions<List<SearchBeatmapset>>
            {
                MaxRetryAttempts = 3,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                Delay = TimeSpan.FromSeconds(1),
                ShouldHandle = new PredicateBuilder<List<SearchBeatmapset>>()
                    .Handle<HttpRequestException>()
            })
            .Build();

        var response = await resiliencePipeline.ExecuteAsync(
            async token => await osuAdapter.SearchBeatmapsetsAsync(
                query,
                mode,
                status,
                page,
                accessToken,
                token),
            cancellationToken);

        return response;
    }
}