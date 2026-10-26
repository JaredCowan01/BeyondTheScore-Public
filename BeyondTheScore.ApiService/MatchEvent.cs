namespace BeyondTheScore.ApiService;

public class MatchEvent
{
    public long Id { get; set; }

    public int EventId { get; set; }

    public int MatchId { get; set; }

    public int TeamId { get; set; }

    public int PlayerId { get; set; }

    public int Minute { get; set; }

    public int Second { get; set; }

    public string EventType { get; set; } = null!;

    public string? Outcome { get; set; }

    public decimal? Xcoord { get; set; }

    public decimal? Ycoord { get; set; }

    public decimal? EndXcoord { get; set; }

    public decimal? EndYcoord { get; set; }

    public int? PossessionId { get; set; }

    public int? AttackSequenceId { get; set; }

    public int? PressureLevel { get; set; }

    public decimal? PassDistance { get; set; }

    public decimal? PassDifficulty { get; set; }

    public decimal? PlayerSpeed { get; set; }

    public decimal? BallSpeed { get; set; }

    public decimal? ExpectedGoals { get; set; }

    public int? ScoreHome { get; set; }

    public int? ScoreAway { get; set; }

    public int? SignificanceScore { get; set; }

    public bool? NarrativeTrigger { get; set; }

    public decimal? MomentumDelta { get; set; }

    public string? StoryArc { get; set; }

    public DateTime CreatedUtc { get; set; }

    public virtual Match Match { get; set; } = null!;

    public virtual Player Player { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;
}
