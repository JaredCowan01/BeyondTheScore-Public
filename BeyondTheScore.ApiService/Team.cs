namespace BeyondTheScore.ApiService;

public class Team
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedUtc { get; set; }

    public virtual ICollection<AttackSequence> AttackSequences { get; set; } = new List<AttackSequence>();

    public virtual ICollection<Match> MatchAwayTeams { get; set; } = new List<Match>();

    public virtual ICollection<MatchEvent> MatchEvents { get; set; } = new List<MatchEvent>();

    public virtual ICollection<Match> MatchHomeTeams { get; set; } = new List<Match>();

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();

    public virtual ICollection<Possession> Possessions { get; set; } = new List<Possession>();
}
