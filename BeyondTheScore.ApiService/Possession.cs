namespace BeyondTheScore.ApiService;

public class Possession
{
    public int Id { get; set; }

    public int MatchId { get; set; }

    public int PossessionNumber { get; set; }

    public int TeamId { get; set; }

    public int? StartMinute { get; set; }

    public int? EndMinute { get; set; }

    public int? EventCount { get; set; }

    public virtual Match Match { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;
}
