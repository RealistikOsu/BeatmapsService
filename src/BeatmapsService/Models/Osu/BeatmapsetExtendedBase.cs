using System.Text.Json.Serialization;

namespace BeatmapsService.Models.Osu;

public class BeatmapsetExtendedBase : BeatmapsetBase
{
    [JsonPropertyName("beatmaps")]
    public BeatmapExtended[] Beatmaps { get; init; } = Array.Empty<BeatmapExtended>();

    [JsonPropertyName("ranked_date")]
    public DateTimeOffset? RankedAt { get; init; }
    
    [JsonPropertyName("tags")]
    public required string Tags { get; init; }
    
    [JsonPropertyName("ranked")]
    public required int Ranked { get; init; }
    
    [JsonPropertyName("last_updated")]
    public required DateTimeOffset LastUpdated { get; init; }

}