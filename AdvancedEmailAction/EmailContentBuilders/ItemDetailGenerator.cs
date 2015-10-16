using System.Collections.Generic;
using MikeRobbins.AdvancedEmailAction.Contracts;
using MikeRobbins.AdvancedEmailAction.Entities;
using Sitecore.Data.Items;
using Sitecore.Web;

namespace MikeRobbins.AdvancedEmailAction.EmailContentBuilders
{
    public class ItemDetailGenerator : IWorkflowHistoryGenerator
    {
        private readonly IWorkflowRepository _workflowRepository;
        private readonly IWorkflowHistory _workflowHistory;
        private readonly IWorkflowCommentsGenerator _workflowCommentsGenerator;
        private readonly IWorkflowCommandsGenerator _workflowCommandsGenerator;
        private readonly ISiteProvider _siteProvider;

        public ItemDetailGenerator(IWorkflowRepository workflowRepository, IWorkflowHistory workflowHistory, IWorkflowCommentsGenerator workflowCommentsGenerator, IWorkflowCommandsGenerator workflowCommandsGenerator, ISiteProvider siteProvider)
        {
            _workflowRepository = workflowRepository;
            _workflowHistory = workflowHistory;
            _workflowCommentsGenerator = workflowCommentsGenerator;
            _workflowCommandsGenerator = workflowCommandsGenerator;
            _siteProvider = siteProvider;
        }

        public string CreateItemWorkflowHistoryHtml(string bodyText, WorkflowHistoryItem workflowHistoryItem, Item emailActionItem, Item workflowItem)
        {
            SiteInfo siteInfo = _siteProvider.GetSiteFromSiteItem(workflowItem);

            var workflowTableData = GetWorkflowHistoryTableData(emailActionItem, workflowHistoryItem, workflowItem);

            var commands = _workflowCommandsGenerator.CreateWorkflowCommandLinks(workflowItem, workflowHistoryItem.WorkflowState, emailActionItem["Host name"]);

            var comments = _workflowCommentsGenerator.CreateWorkflowComments(workflowHistoryItem.Comments);

            return ReplaceHtmlVariables(bodyText, workflowHistoryItem, workflowTableData, commands, comments, siteInfo?.Name);
        }

        public string ReplaceHtmlVariables(string bodyText, WorkflowHistoryItem workflowHistoryItem, string workflowHistory, string commands, string comments, string siteName)
        {
            bodyText = bodyText.Replace("$siteName$", siteName);   
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

            return _workflowHistory.CreateWorkflowHistoryTable(itemWorkflowHistories);
        }
    }
}