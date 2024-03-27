using System.Text.Json.Serialization;

namespace BeatmapsService.Models.Osu;

public class BeatmapsetExtended : BeatmapsetExtendedBase
{
    [JsonPropertyName("genre")]
    public required NamedEntity Genre { get; init; }
    
    [JsonPropertyName("language")]
    public required NamedEntity Language { get; init; }
}