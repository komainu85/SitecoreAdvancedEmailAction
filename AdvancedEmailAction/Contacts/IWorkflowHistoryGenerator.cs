using MikeRobbins.AdvancedEmailAction.Entities;
using Sitecore.Data.Items;
using Sitecore.Workflows;

namespace MikeRobbins.AdvancedEmailAction.Contacts
{
    public interface IWorkflowHistoryGenerator
    {
        string CreateWorkflowHistoryHtml(string bodyText, WorkflowHistoryItem workflowHistoryItem, Item emailActionItem, Item workflowItem);
        string ReplaceVariables(string bodyText, WorkflowHistoryItem workflowHistoryItem, string workflowHistory, string commands);
        string GetWorkflowTableData(Item emailAction, WorkflowHistoryItem workflowHistoryItem, Item workflowItem);
        string GetCommandLinks(Item workflowItem, WorkflowState state, string hostName);
    }
}