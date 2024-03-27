using System.Text.Json.Serialization;

namespace BeatmapsService.Models.Osu;

public class SearchBeatmapsetResponse
{
    [JsonPropertyName("beatmapsets")]
    public required List<SearchBeatmapset> Beatmapsets { get; init; }
}