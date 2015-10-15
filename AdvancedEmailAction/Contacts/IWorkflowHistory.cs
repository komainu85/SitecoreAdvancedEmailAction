using System.Collections.Generic;
using MikeRobbins.AdvancedEmailAction.Entities;

namespace MikeRobbins.AdvancedEmailAction.Contacts
{
    public interface IWorkflowHistory
    {
        string GenerateWorkflowTableData(List<WorkflowHistoryItem> workflowHistory);
    }
}