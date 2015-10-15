using System;
using System.Collections.Generic;
using System.Text;
using MikeRobbins.AdvancedEmailAction.Entities;
using MikeRobbins.AdvancedEmailAction.Repositories;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Workflows;

namespace MikeRobbins.AdvancedEmailAction
{
    public class WorkflowHistoryGenerator
    {
        private readonly WorkflowRepository _workflowRepository = new WorkflowRepository();
        private readonly WorkflowHistory _workflowHistory = new WorkflowHistory();
        
        public WorkflowHistoryItem CreateWorkflowHistoryForItem(Item emailAction, Item workflowItem, string comments)
        {
            var correctState = _workflowRepository.GetWorkflowStateForItem(workflowItem, emailAction);

            WorkflowHistoryItem workflowHistoryItem = new WorkflowHistoryItem
            {
                ItemPath = workflowItem.Paths.FullPath,
                ItemLanguage = workflowItem.Language.GetDisplayName(),
                Version = workflowItem.Version.Number,
                DisplayName = workflowItem.DisplayName,
                Updated = DateTime.Now,
                WorkflowState = correctState,
                WorkflowName = _workflowRepository.GetItemWorkflowName(workflowItem),
                Username = Sitecore.Context.GetUserName(),
                Comments = comments
            };

            return workflowHistoryItem;
        }

        public string ReplaceVariables(string bodyText, WorkflowHistoryItem workflowHistoryItem, string workflowHistory, string commands)
        {
            bodyText = bodyText.Replace("$itempath$", workflowHistoryItem.ItemPath);
            bodyText = bodyText.Replace("$itemlanguage$", workflowHistoryItem.ItemLanguage);
            bodyText = bodyText.Replace("$itemversion$", workflowHistoryItem.Version.ToString());
            bodyText = bodyText.Replace("$itemtitle$", workflowHistoryItem.DisplayName);
            bodyText = bodyText.Replace("$itemdatetime$", workflowHistoryItem.Updated.ToString("dd MMMM yyyy, HH:mm:ss"));
            bodyText = bodyText.Replace("$itemworkflowstate$", workflowHistoryItem.WorkflowState.DisplayName);
            bodyText = bodyText.Replace("$itemworkflowname$", workflowHistoryItem.WorkflowName);
            bodyText = bodyText.Replace("$itemuser$", workflowHistoryItem.Username);
            bodyText = bodyText.Replace("$itemcomment$", workflowHistoryItem.Comments);
            bodyText = bodyText.Replace("$commands$", commands);

            bodyText = bodyText.Replace("$workflowhistorytable$", workflowHistory);

            return bodyText;
        }

        public string GetWorkflowTableData(Item emailAction, WorkflowHistoryItem workflowHistoryItem, Item workflowItem)
        {
            List<ItemWorkflowHistory> itemWorkflowHistories = _workflowRepository.GetWorkflowHistory(workflowItem, emailAction);

            itemWorkflowHistories.Add(_workflowRepository.GetWorkflowHistoryForItem(workflowItem, workflowHistoryItem.Comments, emailAction));

            return _workflowHistory.GenerateWorkflowTableData(itemWorkflowHistories);
        }

        public string GetCommandLinks(Item workflowItem, WorkflowState state, string hostName)
        {
            var sb = new StringBuilder();

            var workflow = Sitecore.Data.Database.GetDatabase("master").WorkflowProvider.GetWorkflow(workflowItem);

            var commands = workflow.GetCommands(state.StateID);

            sb.Append("<ul>");

            foreach (var command in commands)
            {
                var submit = Tools.GetContentEditorLink(ContentEditorMode.Submit, workflowItem, hostName, new ID(command.CommandID));
                var submitComment = Tools.GetContentEditorLink(ContentEditorMode.Submit, workflowItem, hostName, new ID(command.CommandID));

                sb.Append("<li><a href=\"" + submit + "\">" + command.DisplayName + "</a> or <a href=\"" + submitComment + "\">" + command.DisplayName + " & comment</a></li>");
            }

            string editLink = Tools.GetContentEditorLink(ContentEditorMode.Editor, workflowItem, hostName, ID.NewID);
            string previewLink = Tools.GetContentEditorLink(ContentEditorMode.Preview, workflowItem, hostName, ID.NewID);


            sb.Append("<li><a href=\"" + editLink + "\">Edit</li>");
            sb.Append("<li><a href=\"" + previewLink + "\">Preview</li>");
            sb.Append("<li><a href=\"" + "http://" + Sitecore.Sites.SiteContext.GetSite("BusinessCompanion").TargetHostName + "/sitecore/shell/Applications/Workbox/Default.aspx" + "\"/>Workbox</li>");

            sb.Append("</ul>");

            return sb.ToString();
        }
    }
}