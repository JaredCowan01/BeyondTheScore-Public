using BeyondTheScore.ApiService.Interfaces;
using BeyondTheScore.Dto;
using Microsoft.EntityFrameworkCore;

namespace BeyondTheScore.ApiService.Services;

public class MatchAnalyticsService : IMatchAnalyticsService
{
    private readonly FootballDbContext _db;

    public MatchAnalyticsService(FootballDbContext db)
    {
        _db = db;
    }

    public async Task<MatchStateDto> GetMatchState(int matchId)
    {
        return new MatchStateDto
        {
            Summary = await GetSummary(matchId),
            HomeTeam = await GetTeamStats(matchId, true),
            AwayTeam = await GetTeamStats(matchId, false),
            TopPlayers = await GetTopPlayers(matchId),
            KeyMoments = await GetKeyMoments(matchId),
            MomentumShifts = await GetMomentumShifts(matchId),
            DangerousAttacks = await GetDangerousAttackSequences(matchId)
        };
    }

    private async Task<MatchSummaryDto> GetSummary(int matchId)
    {
        Match match = await _db.Matches
            .Include(x => x.HomeTeam)
            .Include(x => x.AwayTeam)
            .FirstAsync(x => x.Id == matchId);

        MatchEvent latestEvent = await _db.MatchEvents
            .Where(x => x.MatchId == matchId)
            .OrderByDescending(x => x.Minute)
            .ThenByDescending(x => x.Second)
            .FirstAsync();

        return new MatchSummaryDto
        {
            MatchId = match.MatchIdentifier,
            HomeTeam = match.HomeTeam.Name,
            AwayTeam = match.AwayTeam.Name,
            ScoreHome = latestEvent.ScoreHome ?? 0,
            ScoreAway = latestEvent.ScoreAway ?? 0,
            CurrentMinute = latestEvent.Minute
        };
    }

    private async Task<TeamStatsDto> GetTeamStats(int matchId, bool isHomeTeam)
    {
        Match match = await _db.Matches
            .Include(x => x.HomeTeam)
            .Include(x => x.AwayTeam)
            .FirstAsync(x => x.Id == matchId);

        int teamId = isHomeTeam
            ? match.HomeTeamId
            : match.AwayTeamId;

        string teamName = isHomeTeam
            ? match.HomeTeam.Name
            : match.AwayTeam.Name;

        var events = _db.MatchEvents
            .Where(x =>
                x.MatchId == matchId &&
                x.TeamId == teamId);

        var totalPasses =
            await events.CountAsync(
                x => x.EventType == "Pass");

        var successfulPasses =
            await events.CountAsync(
                x =>
                    x.EventType == "Pass" &&
                    x.Outcome == "Successful");

        return new TeamStatsDto
        {
            Team = teamName,

            TotalPasses = totalPasses,

            SuccessfulPasses = successfulPasses,

            PassAccuracy =
                totalPasses == 0
                    ? 0
                    : Math.Round(
                        successfulPasses * 100m /
                        totalPasses,
                        2),

            TotalShots =
                await events.CountAsync(
                    x => x.EventType == "Shot"),

            Goals =
                await events.CountAsync(
                    x => x.EventType == "Goal"),

            ExpectedGoals =
                await events.SumAsync(
                    x => x.ExpectedGoals ?? 0),

            AveragePressure =
                await events.AverageAsync(
                    x => (decimal?)
                        x.PressureLevel) ?? 0,

            AverageBallSpeed =
                await events.AverageAsync(
                    x => x.BallSpeed) ?? 0
        };
    }

    private async Task<List<PlayerStatsDto>>
        GetTopPlayers(int matchId)
    {
        return await _db.MatchEvents
            .Where(x => x.MatchId == matchId)
            .GroupBy(x =>
                new
                {
                    x.PlayerId,
                    x.Player.Name
                })
            .Select(g =>
                new PlayerStatsDto
                {
                    PlayerName =
                        g.Key.Name,

                    Passes =
                        g.Count(x =>
                            x.EventType == "Pass"),

                    Shots =
                        g.Count(x =>
                            x.EventType == "Shot"),

                    Goals =
                        g.Count(x =>
                            x.EventType == "Goal"),

                    AverageSpeed =
                        g.Average(x =>
                            x.PlayerSpeed ?? 0),

                    Xg =
                        g.Sum(x =>
                            x.ExpectedGoals ?? 0)
                })
            .OrderByDescending(x => x.Goals)
            .ThenByDescending(x => x.Xg)
            .Take(10)
            .ToListAsync();
    }

    private async Task<List<KeyMomentDto>>
        GetKeyMoments(int matchId)
    {
        return await _db.MatchEvents
            .Include(x => x.Team)
            .Where(x =>
                x.MatchId == matchId &&
                (
                    x.EventType == "Goal" ||
                    x.SignificanceScore >= 70
                ))
            .OrderBy(x => x.Minute)
            .ThenBy(x => x.Second)
            .Select(x =>
                new KeyMomentDto
                {
                    Minute = x.Minute,

                    PlayerName =
                        x.Player.Name,

                    EventType =
                        x.EventType,

                    SignificanceScore =
                        x.SignificanceScore ?? 0,

                    StoryArc =
                        x.StoryArc ?? string.Empty,
                        
                        Team = x.Team.Name
                })
            .ToListAsync();
    }

    private async Task<List<MomentumShiftDto>>
        GetMomentumShifts(int matchId)
    {
        return await _db.MatchEvents
            .Where(x =>
                x.MatchId == matchId &&
                Math.Abs(
                    x.MomentumDelta ?? 0) >= 7)
            .OrderBy(x => x.Minute)
            .Select(x =>
                new MomentumShiftDto
                {
                    Minute = x.Minute,

                    MomentumDelta =
                        x.MomentumDelta ?? 0,

                    Team =
                        x.Team.Name
                })
            .ToListAsync();
    }

    private async Task<List<AttackSequenceDto>>
        GetDangerousAttackSequences(
            int matchId)
    {
        return await _db.MatchEvents
            .Where(x =>
                x.MatchId == matchId &&
                x.AttackSequenceId != null)
            .GroupBy(x =>
                x.AttackSequenceId)
            .Select(g =>
                new AttackSequenceDto
                {
                    SequenceId =
                        g.Key ?? 0,

                    Xg =
                        g.Sum(x =>
                            x.ExpectedGoals ?? 0),

                    ShotCount =
                        g.Count(x =>
                            x.EventType == "Shot"),

                    EventCount =
                        g.Count()
                })
            .OrderByDescending(x => x.Xg)
            .Take(5)
            .ToListAsync();
    }
}