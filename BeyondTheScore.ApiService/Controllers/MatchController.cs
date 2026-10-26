using BeyondTheScore.ApiService.Interfaces;
using BeyondTheScore.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeyondTheScore.ApiService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MatchController : ControllerBase
{
    private readonly IMatchAnalyticsService _matchAnalyticsService;

    public MatchController(IMatchAnalyticsService matchAnalyticsService)
    {
        _matchAnalyticsService = matchAnalyticsService;
    }

    [HttpGet("{matchId}/state")]
    [ProducesResponseType(typeof(MatchStateDto), 200)]
    public async Task<ActionResult<MatchStateDto>> GetMatchState(int matchId)
    {
        var result =
            await _matchAnalyticsService.GetMatchState(matchId);

        return Ok(result);
    }
}