namespace BeyondTheScore.ApiService;

public class PersonalisedCommentary
{
    public long Id { get; set; }

    public int MatchId { get; set; }

    public string TargetAudience { get; set; } = null!;

    public string LanguageCode { get; set; } = null!;

    public int Minute { get; set; }

    public string Commentary { get; set; } = null!;

    public string GeneratedBy { get; set; } = null!;

    public DateTime CreatedUtc { get; set; }

    public virtual Match Match { get; set; } = null!;
}
