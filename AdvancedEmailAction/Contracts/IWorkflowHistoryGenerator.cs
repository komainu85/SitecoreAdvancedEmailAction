using MikeRobbins.AdvancedEmailAction.Entities;
using Sitecore.Data.Items;

namespace MikeRobbins.AdvancedEmailAction.Contracts
{
    public interface IWorkflowHistoryGenerator
    {
        string CreateItemWorkflowHistoryHtml(string bodyText, WorkflowHistoryItem workflowHistoryItem, Item emailActionItem, Item workflowItem);
        string ReplaceHtmlVariables(string bodyText, WorkflowHistoryItem workflowHistoryItem, string workflowHistory, string commands, string comments);
        string GetWorkflowHistoryTableData(Item emailAction, WorkflowHistoryItem workflowHistoryItem, Item workflowItem);
    }
}