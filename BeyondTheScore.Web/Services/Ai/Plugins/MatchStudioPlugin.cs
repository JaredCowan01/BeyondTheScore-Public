using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace BeyondTheScore.Web.Services.Ai.Plugins;

public class MatchStudioPlugin
{
    private readonly string _matchDataJson;
    private readonly string _audienceTone;
    private readonly string _finalMatchSummary;

    public MatchStudioPlugin(string matchDataJson, string audienceTone, string finalMatchSummary)
    {
        _matchDataJson = matchDataJson;
        _audienceTone = audienceTone;
        _finalMatchSummary = finalMatchSummary;
    }

    [KernelFunction, Description("Fetches the live match events and key moment log data in JSON format.")]
    public string GetLiveMatchEvents() => _matchDataJson;

    [KernelFunction, Description("Gets the current target audience tone and presentation preference.")]
    public string GetAudienceTone() => _audienceTone;
    
    [KernelFunction, Description("Gets the final match stats. Only use this when the producer asks you to wrap up at the end of the game.")]
    public string GetFinalStats() => _finalMatchSummary;
}