using BeatmapsService.Extensions;
using BeatmapsService.Models.Cheesegull;
using BeatmapsService.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BeatmapsService.Controllers;

[Route("api/search")]
[ApiController]
public class BeatmapsetSearchController(IOsuService osuService) : ControllerBase
{
    [HttpGet]
    public async Task<Results<Ok<IEnumerable<CheesegullBeatmapset>>, NotFound, ProblemHttpResult>> SearchBeatmapsets(
        [FromQuery] string? query,
        [FromQuery] int? mode,
        [FromQuery] int? status,
        [FromQuery(Name = "amount")] int? pageSize,
        [FromQuery(Name = "offset")] int? page,
        CancellationToken cancellationToken)
    {
        // ensure no more than 101 sets can be returned at once
        // this is intentionally 101 as osu!direct will seek for more maps if it knows there are more than 100 available
        pageSize = Math.Min(pageSize ?? 50, 101);
        page = Math.Max((page ?? 0) + 1, 1);

        var beatmapsets = await osuService.SearchBeatmapsetsAsync(
            query,
            mode,
            status,
            pageSize.Value,
            page.Value,
            cancellationToken);

        return TypedResults.Ok(beatmapsets.Select(s => s.ToCheesegullBeatmapset()));
    }
}