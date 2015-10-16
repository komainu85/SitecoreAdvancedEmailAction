using System.Text;
using MikeRobbins.AdvancedEmailAction.Contracts;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Workflows;

namespace MikeRobbins.AdvancedEmailAction.EmailContentBuilders
{
    public class WorkflowCommandsGenerator : IWorkflowCommandsGenerator
    {
        private readonly IContentEditorUrlBuilder _contentEditorUrlBuilder;

        public WorkflowCommandsGenerator( IContentEditorUrlBuilder contentEditorUrlBuilder)
        {
            _contentEditorUrlBuilder = contentEditorUrlBuilder;
        }

        public string CreateWorkflowCommandLinks(Item workflowItem, WorkflowState state, string hostName)
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
            sb.Append("<li><a href=\"" + "http://" + hostName + "/sitecore/shell/Applications/Workbox/Default.aspx" + "\"/>Workbox</li>");

            sb.Append("</ul>");

            return sb.ToString();
        }
    }
}