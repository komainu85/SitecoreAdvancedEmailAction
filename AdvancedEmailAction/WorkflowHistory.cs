using System.Collections.Generic;
using MikeRobbins.AdvancedEmailAction.Entities;

namespace MikeRobbins.AdvancedEmailAction
{
    public class WorkflowHistory
    {
        public string GenerateWorkflowTableData(List<ItemWorkflowHistory> workflowHistory)
        {
            string htmlWorkflowTable = "<table><tr><th style='text-align: left; padding: 10px;'>Date</th><th style='text-align: left; padding: 10px;'>User</th><th style='text-align: left; padding: 10px;'>Previous State</th><th style='text-align: left; padding: 10px;'>Current State</th><th style='text-align: left; padding: 10px;'>Comment</th></tr>";
            foreach (ItemWorkflowHistory workflowItem in workflowHistory)
            {
                htmlWorkflowTable += "<tr>";
                htmlWorkflowTable += "<td style='text-align: left; padding: 10px;'>" + workflowItem.ItemDateTime.ToString("dd MMMM yyyy, HH:mm:ss") + "</td>";
                htmlWorkflowTable += "<td style='text-align: left; padding: 10px;'>" + workflowItem.User + "</td>";
                htmlWorkflowTable += "<td style='text-align: left; padding: 10px;'>" + workflowItem.PreviousState + "</td>";
                htmlWorkflowTable += "<td style='text-align: left; padding: 10px;'>" + workflowItem.CurrentState + "</td>";
                htmlWorkflowTable += "<td style='text-align: left; padding: 10px;'>" + workflowItem.Comment + "</td>";
                htmlWorkflowTable += "</tr>";
            }
            htmlWorkflowTable += "</table>";

            return htmlWorkflowTable;
        }
    }
}