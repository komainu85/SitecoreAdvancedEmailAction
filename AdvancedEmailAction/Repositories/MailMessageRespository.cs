using System.Net.Mail;
using MikeRobbins.AdvancedEmailAction.Contracts;

namespace MikeRobbins.AdvancedEmailAction.Repositories
{
    public class MailMessageRespository : IMailMessageRespository
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