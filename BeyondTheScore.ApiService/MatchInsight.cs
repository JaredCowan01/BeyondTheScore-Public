namespace BeyondTheScore.ApiService;

public class MatchInsight
{
    public long Id { get; set; }

    public int MatchId { get; set; }

    public int Minute { get; set; }

    public string AgentName { get; set; } = null!;

    public string InsightType { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Explanation { get; set; } = null!;

    public decimal? ConfidenceScore { get; set; }

    public DateTime CreatedUtc { get; set; }

    public virtual Match Match { get; set; } = null!;
}
