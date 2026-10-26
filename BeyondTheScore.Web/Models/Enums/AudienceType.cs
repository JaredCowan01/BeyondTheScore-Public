using System.ComponentModel;

namespace BeyondTheScore.Web.Models.Enums;

public enum AudienceType
{
    [Description("Matchday fan. Simple, accessible, and exciting commentary focused on key moments, momentum, and storylines.")]
    MatchDay,

    [Description("Season ticket holder. Passionate fan perspective with football knowledge, richer context, player insights, and familiar football terminology.")]
    SeasonTicket,

    [Description("Statto. Analytical, data-driven, and tactically focused with advanced metrics, trends, and deeper explanations.")]
    Statto
}