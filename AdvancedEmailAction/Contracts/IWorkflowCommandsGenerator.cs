using Sitecore.Data.Items;
using Sitecore.Workflows;

namespace MikeRobbins.AdvancedEmailAction.Contracts
{
    public interface IWorkflowCommandsGenerator
    {
        string CreateWorkflowCommandLinks(Item workflowItem, WorkflowState state, string hostName);
    }
}