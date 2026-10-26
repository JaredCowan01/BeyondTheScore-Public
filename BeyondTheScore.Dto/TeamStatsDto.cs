namespace BeyondTheScore.Dto;

public class TeamStatsDto
{
    public string Team { get; set; } = string.Empty;

    public int TotalPasses { get; set; }

    public int SuccessfulPasses { get; set; }

    public decimal PassAccuracy { get; set; }

    public int TotalShots { get; set; }

    public int Goals { get; set; }

    public decimal ExpectedGoals { get; set; }

    public decimal AveragePressure { get; set; }

    public decimal AverageBallSpeed { get; set; }
}