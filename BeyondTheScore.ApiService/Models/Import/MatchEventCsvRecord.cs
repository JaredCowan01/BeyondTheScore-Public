using CsvHelper.Configuration.Attributes;

namespace BeyondTheScore.ApiService.Models.Import;

public class MatchEventCsvRecord
{
    [Name("event_id")]
    public int EventId { get; set; }

    [Name("minute")]
    public int Minute { get; set; }

    [Name("second")]
    public int Second { get; set; }

    [Name("match_id")]
    public string MatchId { get; set; } = string.Empty;

    [Name("home_team")]
    public string HomeTeam { get; set; } = string.Empty;

    [Name("away_team")]
    public string AwayTeam { get; set; } = string.Empty;

    [Name("event_type")]
    public string EventType { get; set; } = string.Empty;

    [Name("team")]
    public string Team { get; set; } = string.Empty;

    [Name("player_name")]
    public string PlayerName { get; set; } = string.Empty;

    [Name("x_coord")]
    public decimal XCoord { get; set; }

    [Name("y_coord")]
    public decimal YCoord { get; set; }

    [Name("end_x_coord")]
    public decimal EndXCoord { get; set; }

    [Name("end_y_coord")]
    public decimal EndYCoord { get; set; }

    [Name("outcome")]
    public string? Outcome { get; set; }

    [Name("possession_id")]
    public int? PossessionId { get; set; }

    [Name("attack_sequence_id")]
    public int? AttackSequenceId { get; set; }

    [Name("pressure_level")]
    public int? PressureLevel { get; set; }

    [Name("pass_distance")]
    public decimal? PassDistance { get; set; }

    [Name("pass_difficulty")]
    public decimal? PassDifficulty { get; set; }

    [Name("player_speed")]
    public decimal? PlayerSpeed { get; set; }

    [Name("ball_speed")]
    public decimal? BallSpeed { get; set; }

    [Name("expected_goals")]
    public decimal? ExpectedGoals { get; set; }

    [Name("score_home")]
    public int ScoreHome { get; set; }

    [Name("score_away")]
    public int ScoreAway { get; set; }

    [Name("significance_score")]
    public int? SignificanceScore { get; set; }

    [Name("narrative_trigger")]
    public int? NarrativeTrigger { get; set; }

    [Name("momentum_delta")]
    public decimal? MomentumDelta { get; set; }

    [Name("story_arc")]
    public string? StoryArc { get; set; }
}