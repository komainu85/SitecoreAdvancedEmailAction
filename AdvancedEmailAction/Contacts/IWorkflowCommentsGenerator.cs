using Sitecore.Collections;

namespace MikeRobbins.AdvancedEmailAction.Contacts
{
    public interface IWorkflowCommentsGenerator
    {
        string CreateWorkflowComments(StringDictionary comments);
    }
}