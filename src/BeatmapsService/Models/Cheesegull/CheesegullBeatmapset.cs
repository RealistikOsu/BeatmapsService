using System.Text.Json.Serialization;

namespace BeatmapsService.Models.Cheesegull;

public class CheesegullBeatmapset
{
    [JsonPropertyName("SetID")]
    public required int Id { get; init; }
    
    [JsonPropertyName("ChildrenBeatmaps")]
    public required CheesegullBeatmap[] ChildrenBeatmaps { get; init; }
    
    [JsonPropertyName("RankedStatus")]
    public required int RankedStatus { get; init; }
    
    [JsonPropertyName("ApprovedDate")]
    public required DateTimeOffset ApprovedDate { get; init; }
    
    [JsonPropertyName("LastUpdate")]
    public required DateTimeOffset LastUpdate { get; init; }
    
    [JsonPropertyName("LastChecked")]
    public required DateTimeOffset LastChecked { get; init; }
    
    [JsonPropertyName("Artist")]
    public required string Artist { get; init; }
    
    [JsonPropertyName("Title")]
    public required string Title { get; init; }
    
    [JsonPropertyName("Creator")]
    public required string Creator { get; init; }
    
    [JsonPropertyName("Source")]
    public required string Source { get; init; }
    
    [JsonPropertyName("Tags")]
    public required string Tags { get; init; }
    
    [JsonPropertyName("HasVideo")]
    public required bool HasVideo { get; init; }
    
    [JsonPropertyName("Genre")]
    public required int? Genre { get; init; }
    
    [JsonPropertyName("Language")]
    public required int? Language { get; init; }
    
    [JsonPropertyName("Favourites")]
    public required int Favourites { get; init; }
}