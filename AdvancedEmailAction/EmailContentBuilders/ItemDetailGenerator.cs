﻿using System.Collections.Generic;
using System.Text;
using MikeRobbins.AdvancedEmailAction.Contacts;
using MikeRobbins.AdvancedEmailAction.Entities;
using MikeRobbins.AdvancedEmailAction.Providers;
using MikeRobbins.AdvancedEmailAction.Repositories;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Workflows;
using SiteProvider = MikeRobbins.AdvancedEmailAction.Providers.SiteProvider;

namespace MikeRobbins.AdvancedEmailAction.EmailContentBuilders
{
    public class ItemDetailGenerator : IWorkflowHistoryGenerator
    {
        private readonly IWorkflowRepository _workflowRepository;
        private readonly IWorkflowHistory _workflowHistory;
        private readonly IContentEditorUrlBuilder _contentEditorUrlBuilder;
        private readonly ISiteProvider _siteProvider;
        private readonly IWorkflowCommentsGenerator _workflowCommentsGenerator;

        public ItemDetailGenerator(IWorkflowRepository workflowRepository, IWorkflowHistory workflowHistory, IContentEditorUrlBuilder contentEditorUrlBuilder, ISiteProvider siteProvider, IWorkflowCommentsGenerator workflowCommentsGenerator)
        {
            _workflowRepository = workflowRepository;
            _workflowHistory = workflowHistory;
            _contentEditorUrlBuilder = contentEditorUrlBuilder;
            _siteProvider = siteProvider;
            _workflowCommentsGenerator = workflowCommentsGenerator;
        }

        public string CreateItemWorkflowHistoryHtml(string bodyText, WorkflowHistoryItem workflowHistoryItem, Item emailActionItem, Item workflowItem)
        {
            var workflowTableData = GetWorkflowHistoryTableData(emailActionItem, workflowHistoryItem, workflowItem);

            var commands = GetWorkflowCommandLinks(workflowItem, workflowHistoryItem.WorkflowState, emailActionItem["Host name"]);

            var comments = _workflowCommentsGenerator.CreateWorkflowComments(workflowHistoryItem.Comments);

            return ReplaceHtmlVariables(bodyText, workflowHistoryItem, workflowTableData, commands, comments);
        }

        public string ReplaceHtmlVariables(string bodyText, WorkflowHistoryItem workflowHistoryItem, string workflowHistory, string commands, string comments)
        {
            bodyText = bodyText.Replace("$itempath$", workflowHistoryItem.ItemPath);
            bodyText = bodyText.Replace("$itemlanguage$", workflowHistoryItem.ItemLanguage);
            bodyText = bodyText.Replace("$itemversion$", workflowHistoryItem.Version.ToString());
            bodyText = bodyText.Replace("$itemtitle$", workflowHistoryItem.DisplayName);
            bodyText = bodyText.Replace("$itemdatetime$", workflowHistoryItem.Updated.ToString("dd MMMM yyyy, HH:mm:ss"));
            bodyText = bodyText.Replace("$itemworkflowstate$", workflowHistoryItem.WorkflowState.DisplayName);
            bodyText = bodyText.Replace("$itemworkflowname$", workflowHistoryItem.WorkflowName);
            bodyText = bodyText.Replace("$itemuser$", workflowHistoryItem.Username);
            bodyText = bodyText.Replace("$itemcomment$", comments);
            bodyText = bodyText.Replace("$commands$", commands);
            bodyText = bodyText.Replace("$workflowhistorytable$", workflowHistory);

            return bodyText;
        }

        public string GetWorkflowHistoryTableData(Item emailAction, WorkflowHistoryItem workflowHistoryItem, Item workflowItem)
        {
            List<WorkflowHistoryItem> itemWorkflowHistories = _workflowRepository.GetWorkflowHistory(workflowItem, emailAction);

            itemWorkflowHistories.Add(_workflowRepository.GetWorkflowHistoryForItem(workflowItem, workflowHistoryItem.Comments, emailAction));

            return _workflowHistory.GenerateWorkflowTableData(itemWorkflowHistories);
        }

        public string GetWorkflowCommandLinks(Item workflowItem, WorkflowState state, string hostName)
        {
            var sb = new StringBuilder();

            var workflow = workflowItem.Database.WorkflowProvider.GetWorkflow(workflowItem);

            var commands = workflow.GetCommands(state.StateID);

            sb.Append("<ul>");

            foreach (var command in commands)
            {
                var submit = _contentEditorUrlBuilder.GetContentEditorLink(ContentEditorMode.Submit, workflowItem, hostName, new ID(command.CommandID));
                var submitComment = _contentEditorUrlBuilder.GetContentEditorLink(ContentEditorMode.Submit, workflowItem, hostName, new ID(command.CommandID));

                sb.Append("<li><a href=\"" + submit + "\">" + command.DisplayName + "</a> or <a href=\"" + submitComment + "\">" + command.DisplayName + " & comment</a></li>");
            }

            string editLink = _contentEditorUrlBuilder.GetContentEditorLink(ContentEditorMode.Editor, workflowItem, hostName, ID.NewID);
            string previewLink = _contentEditorUrlBuilder.GetContentEditorLink(ContentEditorMode.Preview, workflowItem, hostName, ID.NewID);

            sb.Append("<li><a href=\"" + editLink + "\">Edit</li>");
            sb.Append("<li><a href=\"" + previewLink + "\">Preview</li>");
            sb.Append("<li><a href=\"" + "http://" + _siteProvider.GetSiteFromSiteItem(workflowItem).TargetHostName + "/sitecore/shell/Applications/Workbox/Default.aspx" + "\"/>Workbox</li>");

            sb.Append("</ul>");

            return sb.ToString();
        }
    }
}