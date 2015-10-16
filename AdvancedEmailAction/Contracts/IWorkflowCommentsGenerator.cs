using Sitecore.Collections;

namespace MikeRobbins.AdvancedEmailAction.Contracts
{
    public interface IWorkflowCommentsGenerator
    {
        string CreateWorkflowComments(StringDictionary comments);
    }
}