using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MikeRobbins.AdvancedEmailAction.EmailContentBuilders;
using MikeRobbins.AdvancedEmailAction.Mail;
using MikeRobbins.AdvancedEmailAction.Repositories;

namespace MikeRobbins.AdvancedEmailAction.IoC
{
    public class Registry : StructureMap.Configuration.DSL.Registry
    {
        public Registry()
        {
            For<Contacts.IContentEditorUrlBuilder>().Use<ContentEditorUrlBuilder>();
            For<Contacts.IEmailSender>().Use<EmailSender>();
            For<Contacts.IMailMessageRespository>().Use<MailMessageRespository>();
            For<Contacts.IWorkflowHistory>().Use<WorkflowHistory>();
            For<Contacts.IWorkflowHistoryGenerator>().Use<WorkflowHistoryGenerator>();
            For<Contacts.IWorkflowRepository>().Use<WorkflowRepository>();
        }
    }
}
