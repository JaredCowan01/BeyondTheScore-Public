namespace BeyondTheScore.Dto;

public class PlayerStatsDto
{
    public string PlayerName { get; set; } = string.Empty;

    public int Passes { get; set; }

    public int Shots { get; set; }

    public int Goals { get; set; }

    public decimal AverageSpeed { get; set; }

    public decimal Xg { get; set; }
}