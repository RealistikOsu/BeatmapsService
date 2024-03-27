namespace BeatmapsService.Helpers;

public static class RankedHelper
{
    public static string ConvertRankedStatus(int status)
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
            _ => throw new InvalidOperationException($"Ranked status {status} is invalid"),
        };
    }
}