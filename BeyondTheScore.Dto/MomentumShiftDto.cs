namespace BeyondTheScore.Dto;

public class MomentumShiftDto
{
    public int Minute { get; set; }

    public decimal MomentumDelta { get; set; }

    public string Team { get; set; } = string.Empty;
}