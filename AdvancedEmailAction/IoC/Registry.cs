using MikeRobbins.AdvancedEmailAction.Contracts;
using MikeRobbins.AdvancedEmailAction.EmailContentBuilders;
using MikeRobbins.AdvancedEmailAction.Mail;
using MikeRobbins.AdvancedEmailAction.Providers;
using MikeRobbins.AdvancedEmailAction.Repositories;

namespace MikeRobbins.AdvancedEmailAction.IoC
{
    public class Registry : StructureMap.Configuration.DSL.Registry
    {
        public Registry()
        {
            For<IContentEditorUrlBuilder>().Use<ContentEditorUrlBuilder>();
            For<IEmailSender>().Use<EmailSender>();
            For<IMailMessageRespository>().Use<MailMessageRespository>();
            For<IWorkflowHistory>().Use<WorkflowHistoryGenerator>();
            For<IWorkflowHistoryGenerator>().Use<ItemDetailGenerator>();
            For<IWorkflowRepository>().Use<WorkflowRepository>();
            For<ISiteProvider>().Use<SiteProvider>();
            For<IWorkflowCommentsGenerator>().Use<WorkflowCommentsGenerator>();
            For<IWorkflowCommandsGenerator>().Use<WorkflowCommandsGenerator>();
        }
    }
}
