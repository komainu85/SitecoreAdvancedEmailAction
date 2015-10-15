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

        public List<ItemWorkflowHistory> GetWorkflowHistory(Item workflowItem, Item emailAction)
        {
            var workflowHistory = new List<ItemWorkflowHistory>();

            var context = _database;

            var workflowItemHistory = workflowItem.State.GetWorkflow().GetHistory(workflowItem);

            foreach (WorkflowEvent workflowEvent in workflowItemHistory)
            {
                Item iItemPreviousState = context.GetItem(workflowEvent.OldState);
                Item iItemCurrentState = context.GetItem(workflowEvent.NewState);
                string itemPreviousState = (iItemPreviousState != null) ? iItemPreviousState.DisplayName : string.Empty;
                string itemCurrentState = (iItemCurrentState != null) ? iItemCurrentState.DisplayName : string.Empty;

                workflowHistory.Add(new ItemWorkflowHistory()
                {
                    ItemDateTime = workflowEvent.Date,
                    User = workflowEvent.User,
                    PreviousState = itemPreviousState,
                    CurrentState = itemCurrentState,
                    Comment = workflowEvent.Text
                });
            }

            return workflowHistory;
        }

        public ItemWorkflowHistory GetWorkflowHistoryForItem(Item item, string commentsToAdd, Item emailAction)
        {
            var history = new ItemWorkflowHistory()
            {
                ItemDateTime = DateTime.Now,
                User = Sitecore.Context.GetUserName(),
                PreviousState = item.State.GetWorkflowState().DisplayName,
                CurrentState = GetWorkflowStateForItem(item, emailAction).DisplayName,
                Comment = commentsToAdd
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