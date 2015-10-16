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
            string htmlWorkflowTable = "<table><tr><th style='text-align: left; padding: 10px;'>Date</th><th style='text-align: left; padding: 10px;'>User</th><th style='text-align: left; padding: 10px;'>Previous State</th><th style='text-align: left; padding: 10px;'>Current State</th><th style='text-align: left; padding: 10px;'>Comments</th></tr>";

            foreach (WorkflowHistoryItem workflowItem in workflowHistory)
            {
                var comments = _workflowCommentsGenerator.CreateWorkflowComments(workflowItem.Comments);

                htmlWorkflowTable += "<tr>";
                htmlWorkflowTable += "<td style='text-align: left; padding: 10px;'>" + workflowItem.Updated.ToString("dd MMMM yyyy, HH:mm:ss") + "</td>";
                htmlWorkflowTable += "<td style='text-align: left; padding: 10px;'>" + workflowItem.Username + "</td>";
                htmlWorkflowTable += "<td style='text-align: left; padding: 10px;'>" + workflowItem.PreviousState + "</td>";
                htmlWorkflowTable += "<td style='text-align: left; padding: 10px;'>" + workflowItem.WorkflowState.DisplayName + "</td>";
                htmlWorkflowTable += "<td style='text-align: left; padding: 10px;'>" + comments + "</td>";
                htmlWorkflowTable += "</tr>";
            }

            htmlWorkflowTable += "</table>";

            return htmlWorkflowTable;
        }
    }
}