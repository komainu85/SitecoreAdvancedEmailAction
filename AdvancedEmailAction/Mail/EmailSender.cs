using System.Net.Mail;

namespace MikeRobbins.AdvancedEmailAction.Mail
{
    public class EmailSender
    {
        public void SendEmail(MailMessage mailMessage)
        {
            new SmtpClient().Send(mailMessage);
        }
    }
}