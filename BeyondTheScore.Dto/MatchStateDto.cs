namespace BeyondTheScore.Dto;

public class MatchStateDto
{
    public MatchSummaryDto Summary { get; set; }

    public TeamStatsDto HomeTeam { get; set; }

    public TeamStatsDto AwayTeam { get; set; }

    public List<PlayerStatsDto> TopPlayers { get; set; }

    public List<KeyMomentDto> KeyMoments { get; set; }

    public List<MomentumShiftDto> MomentumShifts { get; set; }

    public List<AttackSequenceDto> DangerousAttacks { get; set; }
}