using System.Collections.Generic;
using MikeRobbins.AdvancedEmailAction.Entities;

namespace MikeRobbins.AdvancedEmailAction.Contracts
{
    public interface IWorkflowHistory
    {
        string CreateWorkflowHistoryTable(List<WorkflowHistoryItem> workflowHistory);
    }
}