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
                WorkflowHistoryItem workflowHistoryItem = _workflowHistoryGenerator.CreateWorkflowHistoryForItem(emailActionItem, args.DataItem, args.CommentFields["comments"]);

                var workflowTableData = _workflowHistoryGenerator.GetWorkflowTableData(emailActionItem, workflowHistoryItem, args.DataItem);

                var commands = _workflowHistoryGenerator.GetCommandLinks(args.DataItem, workflowHistoryItem.WorkflowState, HostName = emailActionItem["Host name"]);

                bodyText = _workflowHistoryGenerator.ReplaceVariables(bodyText, workflowHistoryItem, workflowTableData, commands);
            }

            return bodyText;
        }

        private Item GetEmailAction()
        {
            ProcessorItem processorItem = workflowPipelineArgs.ProcessorItem;
            return processorItem?.InnerItem;
        }
    }
}
