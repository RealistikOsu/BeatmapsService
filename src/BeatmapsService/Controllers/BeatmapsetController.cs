using BeatmapsService.Extensions;
using BeatmapsService.Models.Cheesegull;
using BeatmapsService.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BeatmapsService.Controllers;

[Route("api/s")]
[ApiController]
public class BeatmapsetController(IOsuService osuService) : ControllerBase
{
    [HttpGet("{beatmapsetId}")]
    public async Task<Results<Ok<CheesegullBeatmapset>, NotFound, ProblemHttpResult>> FindBeatmapsetById(
        [FromRoute] int beatmapsetId,
        CancellationToken cancellationToken)
    {
        var beatmapset = await osuService.FindBeatmapsetByIdAsync(beatmapsetId, cancellationToken);
        if (beatmapset is null)
            return TypedResults.NotFound();
        
        return TypedResults.Ok(beatmapset.ToCheesegullBeatmapset());
    }
}