using System;
using System.Collections.Generic;
using MikeRobbins.AdvancedEmailAction.Entities;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Workflows;
using Sitecore.Workflows.Simple;

namespace MikeRobbins.AdvancedEmailAction.Repositories
{
    public class WorkflowRepository
    {
        private readonly Database _database = Sitecore.Data.Database.GetDatabase("master");

        public List<WorkflowHistoryItem> GetWorkflowHistory(Item itemInWorkflow, Item emailAction)
        {
            var workflowHistory = new List<WorkflowHistoryItem>();

            var context = _database;

            var workflowItemHistory = itemInWorkflow.State.GetWorkflow().GetHistory(itemInWorkflow);

            foreach (WorkflowEvent workflowEvent in workflowItemHistory)
            {
                Item previousWorkflowState = context.GetItem(workflowEvent.OldState);
                WorkflowState newWorkflowState = itemInWorkflow.State.GetWorkflow().GetState(workflowEvent.NewState);

                workflowHistory.Add(new WorkflowHistoryItem()
                {
                    Updated = workflowEvent.Date,
                    Username = workflowEvent.User,
                    PreviousState = previousWorkflowState?.DisplayName,
                    WorkflowState = newWorkflowState,
                    Comments = workflowEvent.CommentFields["comments"]
                });
            }

            return workflowHistory;
        }

        public WorkflowHistoryItem GetWorkflowHistoryForItem(Item item, string commentsToAdd, Item emailAction)
        {
            var history = new WorkflowHistoryItem()
            {
                Updated = DateTime.Now,
                Username = Sitecore.Context.GetUserName(),
                PreviousState = item.State.GetWorkflowState().DisplayName,
                WorkflowState = GetWorkflowStateForItem(item, emailAction),
                Comments = commentsToAdd
            };

            return history;
        }

        public string GetItemWorkflowName(Item workflowItem)
        {
            Sitecore.Data.Database context = _database;

            IWorkflow itemWorkflow = context.WorkflowProvider.GetWorkflow(workflowItem);
            string itemWorkflowName = itemWorkflow.Appearance.DisplayName;

            return itemWorkflowName;
        }

        public WorkflowState GetWorkflowStateForItem(Item item, Item emailAction)
        {
            var command = emailAction.Parent;

            var nextStateId = command["Next state"];

            var itemWorkflow = _database.WorkflowProvider.GetWorkflow(item);

            return itemWorkflow.GetState(nextStateId.ToString());
        }
    }
}