using System.Net.Mail;

namespace MikeRobbins.AdvancedEmailAction.Contracts
{
    public interface IEmailSender
    {
        void SendEmail(MailMessage mailMessage);
    }
}