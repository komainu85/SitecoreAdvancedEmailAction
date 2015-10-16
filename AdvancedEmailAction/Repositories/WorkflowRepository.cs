using System;
using System.Collections.Generic;
using MikeRobbins.AdvancedEmailAction.Contracts;
using MikeRobbins.AdvancedEmailAction.Entities;
using Sitecore.Collections;
using Sitecore.Data.Items;
using Sitecore.Workflows;

namespace MikeRobbins.AdvancedEmailAction.Repositories
{
    public class WorkflowRepository : IWorkflowRepository
    {
        public List<WorkflowHistoryItem> GetWorkflowHistory(Item itemInWorkflow)
        {
            var workflowHistory = new List<WorkflowHistoryItem>();

            var workflowItemHistory = itemInWorkflow.State.GetWorkflow().GetHistory(itemInWorkflow);

            foreach (WorkflowEvent workflowEvent in workflowItemHistory)
            {
                Item previousWorkflowState = itemInWorkflow.Database.GetItem(workflowEvent.OldState);
                WorkflowState newWorkflowState = itemInWorkflow.State.GetWorkflow().GetState(workflowEvent.NewState);

                workflowHistory.Add(new WorkflowHistoryItem()
                {
                    Updated = workflowEvent.Date,
                    Username = workflowEvent.User,
                    PreviousState = previousWorkflowState?.DisplayName,
                    WorkflowState = newWorkflowState,
                    Comments = workflowEvent.CommentFields
                });
            }

            return workflowHistory;
        }

        public WorkflowHistoryItem GetWorkflowHistoryForItem(Item item, StringDictionary comments, Item emailAction)
        {
            var history = new WorkflowHistoryItem()
            {
                Updated = DateTime.Now,
                Username = Sitecore.Context.GetUserName(),
                PreviousState = item.State.GetWorkflowState().DisplayName,
                WorkflowState = GetWorkflowStateForItem(item, emailAction),
                Comments = comments
            };

            return history;
        }

        public string GetItemWorkflowName(Item workflowItem)
        {
            IWorkflow itemWorkflow = workflowItem.Database.WorkflowProvider.GetWorkflow(workflowItem);
            string itemWorkflowName = itemWorkflow.Appearance.DisplayName;

            return itemWorkflowName;
        }

        public WorkflowState GetWorkflowStateForItem(Item item, Item emailAction)
        {
            var command = emailAction.Parent;

            var nextStateId = command["Next state"];

            var itemWorkflow = item.Database.WorkflowProvider.GetWorkflow(item);

            return itemWorkflow.GetState(nextStateId.ToString());
        }
    }
}