using System.Net.Mail;

namespace MikeRobbins.AdvancedEmailAction.Repositories
{
    public class MailMessageRespository
    {
        public MailMessage CreateMailMessage(string from, string to, string subject, string message)
        {
            var mailMessage = new MailMessage(@from, to)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            return mailMessage;
        }
    }
}