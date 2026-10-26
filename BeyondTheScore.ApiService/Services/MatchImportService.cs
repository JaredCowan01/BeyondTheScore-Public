using CsvHelper;
using System.Globalization;
using BeyondTheScore.ApiService.Interfaces;
using BeyondTheScore.ApiService.Models.Import;
using Microsoft.EntityFrameworkCore;

namespace BeyondTheScore.ApiService.Services;

public class MatchImportService : IMatchImportService
{
    private readonly FootballDbContext _dbContext;

    // ReSharper disable once ConvertToPrimaryConstructor
    public MatchImportService(FootballDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ImportResult> ImportAsync(Stream csvStream)
    {
        using var reader = new StreamReader(csvStream);

        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        List<MatchEventCsvRecord> records = csv.GetRecords<MatchEventCsvRecord>().ToList();

        if (records.Count == 0)
        {
            throw new Exception("CSV contains no records.");
        }

        ImportResult result = new ImportResult();

        MatchEventCsvRecord first = records.First();

        Team homeTeam = await GetOrCreateTeam(first.HomeTeam);

        Team awayTeam = await GetOrCreateTeam(first.AwayTeam);

        Match? match = await _dbContext.Matches.FirstOrDefaultAsync(x => x.MatchIdentifier == first.MatchId);

        if (match is null)
        {
            match = new Match
            {
                MatchIdentifier = first.MatchId,
                HomeTeamId = homeTeam.Id,
                AwayTeamId = awayTeam.Id
            };

            _dbContext.Matches.Add(match);

            await _dbContext.SaveChangesAsync();
        }

        Dictionary<string, Player> playerCache =
            await _dbContext.Players.Include(x => x.Team)
                .ToDictionaryAsync(x => $"{x.Team.Name}|{x.Name}");

        List<MatchEvent> eventEntities = [];

        foreach (MatchEventCsvRecord record in records)
        {
            Team team = record.Team == homeTeam.Name ? homeTeam : awayTeam;

            string playerKey = $"{team.Name}|{record.PlayerName}";

            if (!playerCache.TryGetValue(playerKey, out var player))
            {
                player = new Player
                {
                    Name = record.PlayerName,
                    TeamId = team.Id
                };

                _dbContext.Players.Add(player);

                await _dbContext.SaveChangesAsync();

                playerCache[playerKey] = player;
                result.PlayersCreated++;
            }

            eventEntities.Add(new MatchEvent
            {
                EventId = record.EventId,
                MatchId = match.Id,
                TeamId = team.Id,
                PlayerId = player.Id,
                Minute = record.Minute,
                Second = record.Second,
                EventType = record.EventType,
                Outcome = record.Outcome,
                Xcoord = record.XCoord,
                Ycoord = record.YCoord,
                EndXcoord = record.EndXCoord,
                EndYcoord = record.EndYCoord,
                PossessionId = record.PossessionId,
                AttackSequenceId = record.AttackSequenceId,
                PressureLevel = record.PressureLevel,
                PassDistance = record.PassDistance,
                PassDifficulty = record.PassDifficulty,
                PlayerSpeed = record.PlayerSpeed,
                BallSpeed = record.BallSpeed,
                ExpectedGoals = record.ExpectedGoals,
                ScoreHome = record.ScoreHome,
                ScoreAway = record.ScoreAway,
                SignificanceScore = record.SignificanceScore,
                NarrativeTrigger = record.NarrativeTrigger == 1,
                MomentumDelta = record.MomentumDelta,
                StoryArc = record.StoryArc
            });
        }

        await _dbContext.MatchEvents.AddRangeAsync(eventEntities);

        await _dbContext.SaveChangesAsync();

        result.EventsImported = eventEntities.Count;

        return result;
    }

    private async Task<Team> GetOrCreateTeam(string teamName)
    {
        Team? team = await _dbContext.Teams.FirstOrDefaultAsync(x => x.Name == teamName);

        if (team != null)
        {
            return team;
        }

        team = new Team
        {
            Name = teamName
        };

        _dbContext.Teams.Add(team);

        await _dbContext.SaveChangesAsync();

        return team;
    }
}