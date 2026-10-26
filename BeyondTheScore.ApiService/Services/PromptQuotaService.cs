using BeyondTheScore.ApiService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeyondTheScore.ApiService.Services;

public class PromptQuotaService : IPromptQuotaService
{
    private readonly FootballDbContext _context;
    private const int MaxPrompts = 10;

    public PromptQuotaService(FootballDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CanUserPromptAsync(string auth0UserId)
    {
        UserPromptUsage? usage = await _context.UserPromptUsages
            .FirstOrDefaultAsync(u => u.Auth0UserId == auth0UserId);

        if (usage == null)
        {
            return true;
        }
        
        return usage.PromptCount < MaxPrompts;
    }

    public async Task IncrementPromptCountAsync(string auth0UserId)
    {
        UserPromptUsage? usage = await _context.UserPromptUsages
            .FirstOrDefaultAsync(u => u.Auth0UserId == auth0UserId);

        if (usage == null)
        {
            usage = new UserPromptUsage { Auth0UserId = auth0UserId, PromptCount = 1 };
            _context.UserPromptUsages.Add(usage);
        }
        else
        {
            usage.PromptCount++;
        }

        await _context.SaveChangesAsync();
    }
}