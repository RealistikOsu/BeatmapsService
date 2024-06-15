using System.ComponentModel.DataAnnotations;

namespace BeatmapsService;

public class BeatmapOptions
{
    [Required]
    public required string ServiceHost { get; init; }

    [Required]
    public required int ServicePort { get; init; }

    [Required]
    public required int ClientId { get; init; }

    [Required]
    public required string ClientSecret { get; init; }

    [Required]
    public required string RedisConnectionString { get; init; }
}