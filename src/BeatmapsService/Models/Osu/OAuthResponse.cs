﻿using System.Text.Json.Serialization;

namespace BeatmapsService.Models.Osu;

public class OAuthResponse
{
    [JsonPropertyName("token_type")]
    public required string TokenType { get; init; }
    
    [JsonPropertyName("expires_in")]
    public required int ExpiresIn { get; init; }
    
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; init; }
}