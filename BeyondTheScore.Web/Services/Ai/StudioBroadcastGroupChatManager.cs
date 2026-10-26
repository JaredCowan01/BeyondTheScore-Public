using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents.Orchestration.GroupChat;
using Microsoft.SemanticKernel.ChatCompletion;

namespace BeyondTheScore.Web.Services.Ai;

public class StudioBroadcastGroupChatManager : RoundRobinGroupChatManager
{
    public override ValueTask<GroupChatManagerResult<string>> SelectNextAgent(ChatHistory history, GroupChatTeam team, CancellationToken cancellationToken = default)
    {
        if (history.Count <= 1)
        {
            return ValueTask.FromResult(new GroupChatManagerResult<string>("Jimmy")
            {
                Reason = "Initial turn: Jimmy introduces the topic."
            });
        }

        ChatMessageContent? lastMessage = history.LastOrDefault();
        string content = lastMessage?.Content ?? string.Empty;

        if (!string.Equals(lastMessage?.AuthorName, "Jimmy", StringComparison.OrdinalIgnoreCase))
        {
            return base.SelectNextAgent(history, team, cancellationToken);
        }
           
        string? targetAgent = content switch
        {
            var c when c.Contains("Sam", StringComparison.OrdinalIgnoreCase) => "StattoSam",
            var c when c.Contains("Macca", StringComparison.OrdinalIgnoreCase) => "Macca",
            var c when c.Contains("Sophie", StringComparison.OrdinalIgnoreCase) => "Sophie",
            var c when c.Contains("Rico", StringComparison.OrdinalIgnoreCase) => "Rico",
            _ => null
        };

        if (targetAgent != null)
        {
            return ValueTask.FromResult(new GroupChatManagerResult<string>(targetAgent)
            {
                Reason = $"Direct routing: Jimmy called upon {targetAgent}."
            });
        }

        return base.SelectNextAgent(history, team, cancellationToken);
    }
}