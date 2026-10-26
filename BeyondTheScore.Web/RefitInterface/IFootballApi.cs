using BeyondTheScore.Dto;
using Refit;

namespace BeyondTheScore.Web.RefitInterface;

public interface IFootballAnalyticsApi
{
    [Get("/api/match/{matchId}/state")]
    Task<ApiResponse<MatchStateDto>> GetMatchState(int matchId);
    
    [Get("/api/prompt/allow")]
    Task<ApiResponse<EmptyResponse>> CanUserPromptAsync();
    
    [Post("/api/prompt/increment")]
    Task<ApiResponse<EmptyResponse>> IncrementPromptCount();
}

public class EmptyResponse
{
}