using System.Net.Mail;

namespace MikeRobbins.AdvancedEmailAction.Contacts
{
    public interface IMailMessageRespository
    {
        MailMessage CreateMailMessage(string from, string to, string subject, string message);
    }
}