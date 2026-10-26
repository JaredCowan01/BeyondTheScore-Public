using BeyondTheScore.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BeyondTheScore.Web.SignalR;

public class MatchHub : Hub
{
    public async Task BroadcastMatchEvent(KeyMomentDto matchEvent)
    {
        await Clients.All.SendAsync("MatchEvent", matchEvent);
    }

    public async Task BroadcastMomentumShift(MomentumShiftDto momentumShift)
    {
        await Clients.All.SendAsync("MomentumShift", momentumShift);
    }
    
    public async Task StartMatchSimulation(
        IEnumerable<KeyMomentDto> events,
        IEnumerable<MomentumShiftDto> momentumShifts,
        int delayMilliseconds = 3000)
    {
        List<KeyMomentDto> orderedEvents = events
            .OrderBy(matchEvent => matchEvent.Minute)
            .ToList();

        List<MomentumShiftDto> orderedMomentumShifts = momentumShifts
            .OrderBy(momentumShift => momentumShift.Minute)
            .ToList();

        IEnumerable<int> simulationMinutes = orderedEvents
            .Select(matchEvent => matchEvent.Minute)
            .Concat(orderedMomentumShifts.Select(momentumShift => momentumShift.Minute))
            .Distinct()
            .Order();

        foreach (int minute in simulationMinutes)
        {
            foreach (MomentumShiftDto momentumShift in orderedMomentumShifts.Where(momentumShift => momentumShift.Minute == minute))
            {
                await Clients.All.SendAsync("MomentumShift", momentumShift);
            }

            foreach (KeyMomentDto matchEvent in orderedEvents.Where(matchEvent => matchEvent.Minute == minute))
            {
                await Clients.All.SendAsync("MatchEvent", matchEvent);
            }

            await Task.Delay(delayMilliseconds);
        }
    }
}