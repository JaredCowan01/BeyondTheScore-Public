namespace BeyondTheScore.ApiService;

public class Match
{
    public int Id { get; set; }

    public string MatchIdentifier { get; set; } = null!;

    public int HomeTeamId { get; set; }

    public int AwayTeamId { get; set; }

    public DateTime? MatchDate { get; set; }

    public DateTime CreatedUtc { get; set; }

    public virtual ICollection<AttackSequence> AttackSequences { get; set; } = new List<AttackSequence>();

    public virtual Team AwayTeam { get; set; } = null!;

    public virtual Team HomeTeam { get; set; } = null!;

    public virtual ICollection<MatchEvent> MatchEvents { get; set; } = new List<MatchEvent>();

    public virtual ICollection<MatchInsight> MatchInsights { get; set; } = new List<MatchInsight>();

    public virtual ICollection<PersonalisedCommentary> PersonalisedCommentaries { get; set; } = new List<PersonalisedCommentary>();

    public virtual ICollection<Possession> Possessions { get; set; } = new List<Possession>();
}
