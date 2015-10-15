using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using MikeRobbins.AdvancedEmailAction.Comparers;

namespace MikeRobbins.AdvancedEmailAction
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