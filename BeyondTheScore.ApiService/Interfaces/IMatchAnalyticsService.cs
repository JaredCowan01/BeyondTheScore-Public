using BeyondTheScore.Dto;

namespace BeyondTheScore.ApiService.Interfaces;

public interface IMatchAnalyticsService
{
    Task<MatchStateDto> GetMatchState(int matchId);
}