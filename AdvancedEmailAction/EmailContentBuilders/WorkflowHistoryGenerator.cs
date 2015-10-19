using System.Collections.Generic;
using MikeRobbins.AdvancedEmailAction.Contracts;
using MikeRobbins.AdvancedEmailAction.Entities;

namespace MikeRobbins.AdvancedEmailAction.EmailContentBuilders
{
    public class WorkflowHistoryGenerator : IWorkflowHistory
    {
        private readonly IWorkflowCommentsGenerator _workflowCommentsGenerator;

        public WorkflowHistoryGenerator(IWorkflowCommentsGenerator workflowCommentsGenerator)
        {
            _workflowCommentsGenerator = workflowCommentsGenerator;
        }

        public string CreateWorkflowHistoryTable(List<WorkflowHistoryItem> workflowHistory)
        {
            string row = "";

            foreach (WorkflowHistoryItem workflowItem in workflowHistory)
            {
                var comments = _workflowCommentsGenerator.CreateWorkflowComments(workflowItem.Comments);

                row += "<tr>";
                row += "<td style='padding: 10px;'>" + workflowItem.Updated.ToString("dd MMMM yyyy, HH:mm:ss") + "</td>";
                row += "<td style='padding: 10px;'>" + workflowItem.Username + "</td>";
                row += "<td style='padding: 10px;'>" + workflowItem.PreviousState + "</td>";
                row += "<td style='padding: 10px;'>" + workflowItem.WorkflowState.DisplayName + "</td>";
                row += "<td style='padding: 10px;'>" + comments + "</td>";
                row += "</tr>";
            }
            
            return row;
        }
    }
}