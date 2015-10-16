using System;
using MikeRobbins.AdvancedEmailAction.Contracts;
using MikeRobbins.AdvancedEmailAction.Entities;
using Sitecore.Collections;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Workflows.Simple;
using StructureMap;

namespace MikeRobbins.AdvancedEmailAction
{
    public class EmailCommand
    {
        private readonly Container _container = new Container(new IoC.Registry());

        private readonly IMailMessageRespository _mailMessageRespository;
        private readonly IEmailSender _emailSender;
        private readonly IWorkflowHistoryGenerator _workflowHistoryGenerator;
        private readonly IWorkflowRepository _workflowRepository;

        public EmailCommand()
        {
            _mailMessageRespository = _container.GetInstance<IMailMessageRespository>();
            _emailSender = _container.GetInstance<IEmailSender>();
            _workflowHistoryGenerator = _container.GetInstance<IWorkflowHistoryGenerator>();
            _workflowRepository = _container.GetInstance<IWorkflowRepository>();
        }

        public void Process(WorkflowPipelineArgs args)
        { 
            Assert.ArgumentNotNull((object)args, "args");
            Item emailActionItem = GetEmailAction(args);

            if (emailActionItem == null)
            {
                return;
            }

            string from = emailActionItem["From"];
            string to = emailActionItem["To"];
            string subject = emailActionItem["Subject"];

            string body = GetBodyText(emailActionItem, "Body", args);

            var mailMessage = _mailMessageRespository.CreateMailMessage(from, to, subject, body);

            _emailSender.SendEmail(mailMessage);
        }

        private string GetBodyText(Item emailActionItem, string field, WorkflowPipelineArgs args)
        {
            string bodyText = emailActionItem[field];

            if (!string.IsNullOrEmpty(bodyText))
            {
                WorkflowHistoryItem itemInWorkflow = CreateWorkflowHistoryForItem(emailActionItem, args.DataItem, args.CommentFields);

                bodyText = _workflowHistoryGenerator.CreateItemWorkflowHistoryHtml(bodyText, itemInWorkflow, emailActionItem,args.DataItem);
            }

            return bodyText;
        }

        public WorkflowHistoryItem CreateWorkflowHistoryForItem(Item emailAction, Item workflowItem, StringDictionary comments)
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

        private Item GetEmailAction(WorkflowPipelineArgs workflowPipelineArgs)
        {
            ProcessorItem processorItem = workflowPipelineArgs.ProcessorItem;
            return processorItem?.InnerItem;
        }
    }
}
