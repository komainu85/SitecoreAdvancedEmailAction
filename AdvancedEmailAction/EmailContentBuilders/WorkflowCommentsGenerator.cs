using System.Text;
using MikeRobbins.AdvancedEmailAction.Contacts;
using Sitecore.Collections;

namespace MikeRobbins.AdvancedEmailAction.EmailContentBuilders
{
    public class WorkflowCommentsGenerator : IWorkflowCommentsGenerator
    {
        public string CreateWorkflowComments(StringDictionary comments)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var comment in comments)
            {
                sb.Append("<span style='font-weight:bold;'>" + comment.Key  +"</span> ");
                sb.Append(comment.Value + "</br>");
            }

            return sb.ToString();
        }
    }
}