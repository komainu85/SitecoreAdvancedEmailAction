using System.Net.Mail;

namespace MikeRobbins.AdvancedEmailAction.Contacts
{
    public interface IEmailSender
    {
        void SendEmail(MailMessage mailMessage);
    }
}