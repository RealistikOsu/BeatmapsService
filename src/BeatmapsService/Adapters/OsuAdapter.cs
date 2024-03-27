using System.Net;
using System.Net.Http.Headers;
using BeatmapsService.Models.Osu;
using Microsoft.AspNetCore.WebUtilities;

namespace BeatmapsService.Adapters;

public class OsuAdapter(IHttpClientFactory httpClientFactory) : IOsuAdapter
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient();

    public async Task<OAuthResponse> AuthenticateAsync(int clientId, string clientSecret, CancellationToken cancellationToken = default)
    {
        var formData = new Dictionary<string, string>()
        {
            { "client_id", clientId.ToString() },
            { "client_secret", clientSecret },
            { "grant_type", "client_credentials" },
            { "scope", "public" },
        };

        var response =
            await _httpClient.PostAsync(
                "https://osu.ppy.sh/oauth/token",
                new FormUrlEncodedContent(formData),
                cancellationToken);

        response.EnsureSuccessStatusCode();

        var oauthResponse = await response.Content.ReadFromJsonAsync<OAuthResponse>(cancellationToken);
        ArgumentNullException.ThrowIfNull(oauthResponse);

        return oauthResponse;
    }

    public async Task<BeatmapExtended?> FindBeatmapByIdAsync(int beatmapId, string accessToken, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri($"https://osu.ppy.sh/api/v2/beatmaps/{beatmapId}"),
            Headers =
            {
                Authorization = new AuthenticationHeaderValue("Bearer", accessToken)
            },
            Method = HttpMethod.Get,
        };

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode is HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var beatmapResponse = await response.Content.ReadFromJsonAsync<BeatmapExtended>(cancellationToken);
        return beatmapResponse;
    }

    public async Task<BeatmapsetExtended?> FindBeatmapsetByIdAsync(
        int beatmapsetId,
        string accessToken,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri($"https://osu.ppy.sh/api/v2/beatmapsets/{beatmapsetId}"),
            Headers =
            {
                Authorization = new AuthenticationHeaderValue("Bearer", accessToken)
            },
            Method = HttpMethod.Get,
        };

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode is HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var beatmapsetResponse = await response.Content.ReadFromJsonAsync<BeatmapsetExtended>(cancellationToken);
        return beatmapsetResponse;
    }

    public async Task<List<SearchBeatmapset>> SearchBeatmapsetsAsync(
        string? query,
        int? mode,
        string? status,
        int page,
        string accessToken,
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string?>
        {
            { "page", page.ToString() },
        };

        if (query is not null)
            parameters["q"] = query;

        if (mode is not null)
            parameters["m"] = mode.ToString();

        if (status is not null)
            parameters["s"] = status;
        
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri(QueryHelpers.AddQueryString("https://osu.ppy.sh/api/v2/beatmapsets/search", parameters)),
            Headers =
            {
                Authorization = new AuthenticationHeaderValue("Bearer", accessToken)
            },
            Method = HttpMethod.Get,
        };
        
        var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var beatmapsetsResponse = await response.Content.ReadFromJsonAsync<SearchBeatmapsetResponse>(cancellationToken);
        ArgumentNullException.ThrowIfNull(beatmapsetsResponse);

        return beatmapsetsResponse.Beatmapsets;
    }
}