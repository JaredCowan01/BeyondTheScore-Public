namespace BeyondTheScore.ApiService;

public class Player
{
    public int Id { get; set; }

    public int TeamId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedUtc { get; set; }

    public virtual ICollection<MatchEvent> MatchEvents { get; set; } = new List<MatchEvent>();

    public virtual Team Team { get; set; } = null!;
}
