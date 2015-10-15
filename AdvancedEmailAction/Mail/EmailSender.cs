using System.Net.Mail;
using MikeRobbins.AdvancedEmailAction.Contacts;

namespace MikeRobbins.AdvancedEmailAction.Mail
{
    public class EmailSender : IEmailSender
    {
        public void SendEmail(MailMessage mailMessage)
        {
            new SmtpClient().Send(mailMessage);
        }
    }
}