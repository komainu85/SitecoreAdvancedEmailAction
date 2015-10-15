using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Security;
using MikeRobbins.AdvancedEmailAction.Comparers;
using MikeRobbins.AdvancedEmailAction.Entities;
using MikeRobbins.AdvancedEmailAction.Repositories;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Feeds.Sections;
using Sitecore.Workflows;
using Sitecore.Workflows.Simple;

namespace MikeRobbins.AdvancedEmailAction
{
    public class EmailCommand
    {
        private readonly MailMessageRespository _mailMessageRespository = new MailMessageRespository();
        private readonly EmailSender _emailSender = new EmailSender();
        private readonly WorkflowHistoryGenerator _workflowHistoryGenerator = new WorkflowHistoryGenerator();
        private readonly WorkflowRepository _workflowRepository = new WorkflowRepository();

        public WorkflowPipelineArgs workflowPipelineArgs;

        public void Process(WorkflowPipelineArgs args)
        {
            workflowPipelineArgs = args;

            Assert.ArgumentNotNull((object)args, "args");
            Item emailActionItem = GetEmailAction();

            if (emailActionItem == null)
            {
                return;
            }

            string from = emailActionItem["from"];
            string to = emailActionItem["to"];
            string subject = emailActionItem["subject"];

            string message = GetBodyText(emailActionItem, "message", args);

            var mailMessage = _mailMessageRespository.CreateMailMessage(from, to, subject, message);

            _emailSender.SendEmail(mailMessage);
        }

        private string GetBodyText(Item emailActionItem, string field, WorkflowPipelineArgs args)
        {
            string bodyText = emailActionItem[field];

            if (!string.IsNullOrEmpty(bodyText))
            {
                WorkflowHistoryItem itemInWorkflow = CreateWorkflowHistoryForItem(emailActionItem, args.DataItem, args.CommentFields["comments"]);

                bodyText = _workflowHistoryGenerator.CreateWorkflowHistoryHtml(bodyText, itemInWorkflow, emailActionItem,args.DataItem);
            }

            return bodyText;
        }

        public WorkflowHistoryItem CreateWorkflowHistoryForItem(Item emailAction, Item workflowItem, string comments)
        {
            var correctState = _workflowRepository.GetWorkflowStateForItem(workflowItem, emailAction);

            WorkflowHistoryItem itemInWorkflow = new WorkflowHistoryItem
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

            return itemInWorkflow;
        }

        private Item GetEmailAction()
        {
            ProcessorItem processorItem = workflowPipelineArgs.ProcessorItem;
            return processorItem?.InnerItem;
        }
    }
}
