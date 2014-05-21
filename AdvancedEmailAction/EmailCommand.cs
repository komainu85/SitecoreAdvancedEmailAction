using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Security;
using MikeRobbins.AdvancedEmailAction.Comparers;
using MikeRobbins.AdvancedEmailAction.Entities;
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
        public string HostName { get; set; }

        public WorkflowPipelineArgs workflowPipelineArgs;

        private string GetText(Item commandItem, string field, WorkflowPipelineArgs args)
        {
            string item = commandItem[field];
            return item.Length <= 0 ? string.Empty : this.ReplaceVariables(item, args);
        }

        public void Process(WorkflowPipelineArgs args)
        {
            workflowPipelineArgs = args;

            Assert.ArgumentNotNull((object)args, "args");
            Item emailActionItem = GetEmailAction();

            if (emailActionItem == null)
            {
                return;
            }

            string fullPath = emailActionItem.Paths.FullPath;
            string from = this.GetText(emailActionItem, "from", args);
            string to = GetDestinationAddress(emailActionItem);
            string subject = this.GetText(emailActionItem, "subject", args);
            string message = this.GetText(emailActionItem, "message", args);
            HostName = this.GetText(emailActionItem, "Host Name", args);

            Error.Assert(to.Length > 0, "The 'To' field is not specified in the mail action item: " + fullPath);
            Error.Assert(from.Length > 0, "The 'From' field is not specified in the mail action item: " + fullPath);
            Error.Assert(subject.Length > 0, "The 'Subject' field is not specified in the mail action item: " + fullPath);
            Error.Assert(subject.Length > 0, "The 'Host Name' field is not specified in the mail action item: " + fullPath);

            var mailMessage = new MailMessage(from, to)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            var item = args.DataItem as Item;

            var itemUserEmails = GetItemSelectedEmails(item);

            if (itemUserEmails.Any())
            {
                foreach (var itemUser in itemUserEmails.Where(x => !string.IsNullOrEmpty(x)).Distinct(new EmailComparer()))
                {
                    mailMessage.To.Add(itemUser);
                }
            }

            new SmtpClient().Send(mailMessage);
        }

        private Item GetEmailAction()
        {
            ProcessorItem processorItem = workflowPipelineArgs.ProcessorItem;
            if (processorItem == null)
            {
                return null;
            }
            return processorItem.InnerItem;
        }

        protected virtual string GetDestinationAddress(Item innerItem)
        {
            return this.GetText(innerItem, "to", workflowPipelineArgs).Trim();
        }

        private List<ItemWorkflowHistory> GetWorkflowHistory(Item workflowItem)
        {
            var workflowHistory = new List<ItemWorkflowHistory>();

            var context = Sitecore.Data.Database.GetDatabase("master");

            var workflowItemHistory = workflowItem.State.GetWorkflow().GetHistory(workflowItem);

            foreach (WorkflowEvent workflowEvent in workflowItemHistory)
            {
                Item iItemPreviousState = context.GetItem(workflowEvent.OldState);
                Item iItemCurrentState = context.GetItem(workflowEvent.NewState);
                String itemPreviousState = (iItemPreviousState != null) ? iItemPreviousState.DisplayName : string.Empty;
                String itemCurrentState = (iItemCurrentState != null) ? iItemCurrentState.DisplayName : string.Empty;

                workflowHistory.Add(new ItemWorkflowHistory()
                {
                    ItemDateTime = workflowEvent.Date,
                    User = workflowEvent.User,
                    PreviousState = itemPreviousState,
                    CurrentState = itemCurrentState,
                    Comment = workflowEvent.Text
                });
            }

            workflowHistory.Add(GetCurrentWorkflowHistoryItem());

            return workflowHistory;
        }

        private ItemWorkflowHistory GetCurrentWorkflowHistoryItem()
        {

            var history = new ItemWorkflowHistory()
            {
                ItemDateTime = DateTime.Now,
                User = Sitecore.Context.GetUserName(),
                PreviousState = workflowPipelineArgs.DataItem.State.GetWorkflowState().DisplayName,
                CurrentState = GetCorrectWorkflowState().DisplayName,
                Comment = workflowPipelineArgs.Comments
            };

            return history;
        }

        private String GetItemWorkflowName(Item workflowItem)
        {
            Sitecore.Data.Database context = Sitecore.Data.Database.GetDatabase("master");

            IWorkflow itemWorkflow = context.WorkflowProvider.GetWorkflow(workflowItem);
            string itemWorkflowName = itemWorkflow.Appearance.DisplayName;

            return itemWorkflowName;
        }

        private string GenerateWorkflowTableData(List<ItemWorkflowHistory> workflowHistory)
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

        private string ReplaceVariables(string text, WorkflowPipelineArgs args)
        {
            Item workflowItem = args.DataItem;

            var correctState = GetCorrectWorkflowState();

            text = text.Replace("$itempath$", workflowItem.Paths.FullPath);
            text = text.Replace("$itemlanguage$", workflowItem.Language.GetDisplayName());
            text = text.Replace("$itemversion$", workflowItem.Version.ToString());

            text = text.Replace("$itemtitle$", workflowItem.DisplayName);
            text = text.Replace("$itemdatetime$", System.DateTime.Now.ToString("dd MMMM yyyy, HH:mm:ss"));
            text = text.Replace("$itemworkflowstate$", correctState.DisplayName);

            text = text.Replace("$itemworkflowname$", GetItemWorkflowName(workflowItem));
            text = text.Replace("$itemuser$", Sitecore.Context.GetUserName());
            text = text.Replace("$itemcomment$", args.Comments);

            var commands = GetCommandLinks(workflowItem, correctState);

            text = text.Replace("$commands$", commands);
            text = text.Replace("$workflowhistorytable$", GenerateWorkflowTableData(GetWorkflowHistory(workflowItem)));

            return text;
        }

        private string GetCommandLinks(Item workflowItem, WorkflowState state)
        {
            var sb = new StringBuilder();

            var workflow = Sitecore.Data.Database.GetDatabase("master").WorkflowProvider.GetWorkflow(workflowItem);

            var commands = workflow.GetCommands(state.StateID);

            sb.Append("<ul>");

            foreach (var command in commands)
            {
                var submit = Tools.GetContentEditorLink(ContentEditorMode.Submit, workflowItem, HostName, new ID(command.CommandID));
                var submitComment = Tools.GetContentEditorLink(ContentEditorMode.Submit, workflowItem,HostName, new ID(command.CommandID));

                sb.Append("<li><a href=\"" + submit + "\">" + command.DisplayName + "</a> or <a href=\"" + submitComment + "\">" + command.DisplayName + " & comment</a></li>");
            }

            string editLink = Tools.GetContentEditorLink(ContentEditorMode.Editor, workflowItem, HostName, ID.NewID);
            string previewLink = Tools.GetContentEditorLink(ContentEditorMode.Preview, workflowItem,HostName, ID.NewID);


            sb.Append("<li><a href=\"" + editLink + "\">Edit</li>");
            sb.Append("<li><a href=\"" + previewLink + "\">Preview</li>");
            sb.Append("<li><a href=\"" + "http://" + Sitecore.Sites.SiteContext.GetSite("BusinessCompanion").TargetHostName + "/sitecore/shell/Applications/Workbox/Default.aspx" + "\"/>Workbox</li>");

            sb.Append("</ul>");

            return sb.ToString();
        }

        private List<string> GetItemSelectedEmails(Item item)
        {
            var monitor = item["Workflow Monitor"];
            var editor = item["Workflow Editor"];

            var monitorUser = GetUserAccount(monitor);
            var editorUser = GetUserAccount(editor);


            var emails = new List<string>();

            if (monitorUser != null)
            {
                emails.Add(monitorUser.Email);
            }

            if (editorUser != null)
            {
                emails.Add(editorUser.Email);
            }

            return emails;
        }

        private MembershipUser GetUserAccount(string username)
        {
            return Membership.GetUser(username);
        }

        private WorkflowState GetCorrectWorkflowState()
        {
            var emailAction = GetEmailAction();

            var command = emailAction.Parent;

            var nextStateId = command["Next state"];

            var itemWorkflow = Sitecore.Data.Database.GetDatabase("master").WorkflowProvider.GetWorkflow(workflowPipelineArgs.DataItem);

            return itemWorkflow.GetState(nextStateId.ToString());
        }
    }
}
