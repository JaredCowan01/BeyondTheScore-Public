namespace BeyondTheScore.Dto;

public class MatchSummaryDto
{
    public string MatchId { get; set; } = string.Empty;

    public string HomeTeam { get; set; } = string.Empty;

    public string AwayTeam { get; set; } = string.Empty;

    public int ScoreHome { get; set; }

    public int ScoreAway { get; set; }

    public int CurrentMinute { get; set; }
}