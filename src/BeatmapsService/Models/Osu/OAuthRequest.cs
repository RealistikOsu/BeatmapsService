using System.Text.Json.Serialization;
using Refit;

namespace BeatmapsService.Models.Osu;

public class OAuthRequest
{
    [AliasAs("client_id")]
    public required string ClientId { get; init; }
    
    [AliasAs("client_secret")]
    public required string ClientSecret { get; init; }

    [AliasAs("grant_type")]
    public string GrantType => "client_credentials";

    [AliasAs("scope")]
    public string Scope => "public";
}