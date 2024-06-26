﻿using BeatmapsService.Models.Cheesegull;
using BeatmapsService.Models.Osu;

namespace BeatmapsService.Extensions;

public static class CheesegullExtensions
{
    public static CheesegullBeatmap ToCheesegullBeatmap<T>(this T beatmap) where T : BeatmapExtended
    {
        return new CheesegullBeatmap
        {
            Id = beatmap.Id,
            BeatmapsetId = beatmap.BeatmapsetId,
            Version = beatmap.Version,
            Checksum = beatmap.Checksum,
            Mode = beatmap.ModeInt,
            Bpm = beatmap.Bpm ?? 0m,
            ApproachRate = beatmap.ApproachRate,
            OverallDifficulty = beatmap.OverallDifficulty,
            CircleSize = beatmap.CircleSize,
            HealthPoints = beatmap.HealthPoints,
            TotalLength = beatmap.TotalLength,
            HitLength = beatmap.HitLength,
            PlayCount = beatmap.PlayCount,
            PassCount = beatmap.PassCount,
            MaxCombo = beatmap.MaxCombo ?? 0,
            DifficultyRating = beatmap.DifficultyRating,
        };
    }
    
    public static CheesegullBeatmapset ToCheesegullBeatmapset<T>(this T beatmapset) where T : BeatmapsetExtendedBase
    {
        return new CheesegullBeatmapset
        {
            Id = beatmapset.Id,
            ChildrenBeatmaps = beatmapset.Beatmaps.Select(b => b.ToCheesegullBeatmap()).ToArray(),
            RankedStatus = beatmapset.Ranked,
            ApprovedDate = beatmapset.RankedAt ?? DateTimeOffset.MinValue,
            LastUpdate = beatmapset.RankedAt ?? beatmapset.LastUpdated,
            LastChecked = DateTimeOffset.UtcNow, // TODO: use cache expiry
            Artist = beatmapset.Artist,
            Title = beatmapset.Title,
            Creator = beatmapset.Creator,
            Source = beatmapset.Source,
            Tags = beatmapset.Tags,
            HasVideo = beatmapset.Video,
            Genre = null, // highly unfortunate
            Language = null, // highly unfortunate
            Favourites = beatmapset.FavouriteCount,
        };
    }
    
    public static CheesegullBeatmapset ToCheesegullBeatmapset(this BeatmapsetExtended beatmapset)
    {
        return new CheesegullBeatmapset
        {
            Id = beatmapset.Id,
            ChildrenBeatmaps = beatmapset.Beatmaps.Select(b => b.ToCheesegullBeatmap()).ToArray(),
            RankedStatus = beatmapset.Ranked,
            ApprovedDate = beatmapset.RankedAt ?? DateTimeOffset.MinValue,
            LastUpdate = beatmapset.RankedAt ?? beatmapset.LastUpdated,
            LastChecked = DateTimeOffset.UtcNow, // TODO: use cache expiry
            Artist = beatmapset.Artist,
            Title = beatmapset.Title,
            Creator = beatmapset.Creator,
            Source = beatmapset.Source,
            Tags = beatmapset.Tags,
            HasVideo = beatmapset.Video,
            Genre = beatmapset.Genre.Id,
            Language = beatmapset.Language.Id,
            Favourites = beatmapset.FavouriteCount,
        };
    }
}