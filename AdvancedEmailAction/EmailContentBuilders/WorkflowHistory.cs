using System.Collections.Generic;
using MikeRobbins.AdvancedEmailAction.Contacts;
using MikeRobbins.AdvancedEmailAction.Entities;

namespace MikeRobbins.AdvancedEmailAction.EmailContentBuilders
{
    public class WorkflowHistory : IWorkflowHistory
    {
        public string GenerateWorkflowTableData(List<WorkflowHistoryItem> workflowHistory)
        {
            string htmlWorkflowTable = "<table><tr><th style='text-align: left; padding: 10px;'>Date</th><th style='text-align: left; padding: 10px;'>User</th><th style='text-align: left; padding: 10px;'>Previous State</th><th style='text-align: left; padding: 10px;'>Current State</th><th style='text-align: left; padding: 10px;'>Comment</th></tr>";
            foreach (WorkflowHistoryItem workflowItem in workflowHistory)
            {
                htmlWorkflowTable += "<tr>";
                htmlWorkflowTable += "<td style='text-align: left; padding: 10px;'>" + workflowItem.Updated.ToString("dd MMMM yyyy, HH:mm:ss") + "</td>";
                htmlWorkflowTable += "<td style='text-align: left; padding: 10px;'>" + workflowItem.Username + "</td>";
                htmlWorkflowTable += "<td style='text-align: left; padding: 10px;'>" + workflowItem.PreviousState + "</td>";
                htmlWorkflowTable += "<td style='text-align: left; padding: 10px;'>" + workflowItem.WorkflowState.DisplayName + "</td>";
                htmlWorkflowTable += "<td style='text-align: left; padding: 10px;'>" + workflowItem.Comments + "</td>";
                htmlWorkflowTable += "</tr>";
            }
            htmlWorkflowTable += "</table>";

            return htmlWorkflowTable;
        }
    }
}