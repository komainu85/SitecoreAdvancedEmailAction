using System.Net.Mail;

namespace MikeRobbins.AdvancedEmailAction.Contracts
{
    public interface IMailMessageRespository
    {
        MailMessage CreateMailMessage(string from, string to, string subject, string message);
    }
}