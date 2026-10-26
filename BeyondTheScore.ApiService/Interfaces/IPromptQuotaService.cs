namespace BeyondTheScore.ApiService.Interfaces;

public interface IPromptQuotaService
{
    Task<bool> CanUserPromptAsync(string auth0UserId);
    Task IncrementPromptCountAsync(string auth0UserId);
}