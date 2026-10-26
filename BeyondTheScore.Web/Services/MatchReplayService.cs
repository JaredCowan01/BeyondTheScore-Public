using BeyondTheScore.Dto;
using BeyondTheScore.Web.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace BeyondTheScore.Web.Services;

public class MatchReplayService : BackgroundService
{
    private readonly IHubContext<MatchHub> _hub;
    private readonly MatchStateDto _match;

    // ReSharper disable once ConvertToPrimaryConstructor
    public MatchReplayService(IHubContext<MatchHub> hub, MatchStateDto match)
    {
        _hub = hub;
        _match = match;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        List<KeyMomentDto> keyMoments = _match.KeyMoments.OrderBy(x => x.Minute).ToList();

        int previousMinute = 0;

        foreach (KeyMomentDto keyMoment in keyMoments)
        {
            int waitTime = (keyMoment.Minute - previousMinute) * 1000;

            await Task.Delay(waitTime, ct);

            await _hub.Clients.All.SendAsync("MatchEvent", keyMoment, ct);

            previousMinute = keyMoment.Minute;
        }
    }
}