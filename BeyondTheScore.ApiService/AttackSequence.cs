namespace BeyondTheScore.ApiService;

public class AttackSequence
{
    public int Id { get; set; }

    public int MatchId { get; set; }

    public int SequenceNumber { get; set; }

    public int TeamId { get; set; }

    public int? StartMinute { get; set; }

    public int? EndMinute { get; set; }

    public decimal? ExpectedGoals { get; set; }

    public string? Outcome { get; set; }

    public virtual Match Match { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;
}
