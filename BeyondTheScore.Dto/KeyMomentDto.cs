namespace BeyondTheScore.Dto;

public class KeyMomentDto
{
    public int Minute { get; set; }

    public string PlayerName { get; set; } = string.Empty;

    public string EventType { get; set; } = string.Empty;

    public int SignificanceScore { get; set; }

    public string StoryArc { get; set; } = string.Empty;

    public string Team { get; set; } = string.Empty;
}