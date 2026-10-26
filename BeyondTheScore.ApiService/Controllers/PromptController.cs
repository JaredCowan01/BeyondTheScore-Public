using BeyondTheScore.ApiService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeyondTheScore.ApiService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PromptController : ControllerBase
{
    private readonly IPromptQuotaService _promptQuotaService;

    public PromptController(IPromptQuotaService promptQuotaService)
    {
        _promptQuotaService = promptQuotaService;
    }

    [HttpGet("allow")]
    public async Task<ActionResult<bool>> CanUserPrompt()
    {
        string? auth0UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(auth0UserId))
        {
            return Unauthorized("Invalid token claims.");
        }

        bool result = await _promptQuotaService.CanUserPromptAsync(auth0UserId);

        return result switch
        {
            true => Ok(),
            _ => Forbid()
        };
    }

    [HttpPost("increment")]
    public async Task<ActionResult> IncrementPromptCount()
    {
        string? auth0UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(auth0UserId))
        {
            return Unauthorized("Invalid token claims.");
        }

        try
        {
            await _promptQuotaService.IncrementPromptCountAsync(auth0UserId);

            return Ok();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
}