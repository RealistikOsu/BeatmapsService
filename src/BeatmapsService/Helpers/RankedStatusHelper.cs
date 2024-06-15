namespace BeatmapsService.Helpers;

public static class RankedStatusHelper
{
    public static string ConvertRankedStatus(int? status)
    {
        return status switch
        {
            -2 => "graveyard",
            -1 => "wip",
            0 => "pending",
            1 => "ranked",
            2 => "approved",
            3 => "qualified",
            4 => "loved",
            null => "any",
            _ => throw new InvalidOperationException($"Ranked status {status} is invalid"),
        };
    }

    public static string GetRankedStatusSort(string status)
    {
        return status switch
        {
            "graveyard" or "wip" or "pending" or "any" => "updated_desc",
            "ranked" or "approved" or "qualified" or "loved" => "ranked_desc",
            _ => throw new InvalidOperationException($"Ranked status {status} is invalid"),
        };
    }
}