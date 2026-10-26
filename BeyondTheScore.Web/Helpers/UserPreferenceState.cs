using BeyondTheScore.Web.Models.Enums;

namespace BeyondTheScore.Web.Helpers;

public class UserPreferenceState
{
    public AudienceType Audience { get; private set; } = AudienceType.MatchDay;

    public event Action? OnChange;

    public void SetAudience(AudienceType audience)
    {
        Audience = audience;
        OnChange?.Invoke();
    }
}