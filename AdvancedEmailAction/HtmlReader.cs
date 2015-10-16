using System;
using System.IO;
using System.Web;
using MikeRobbins.AdvancedEmailAction.Contacts;

namespace MikeRobbins.AdvancedEmailAction
{
    public class HtmlReader : IHtmlReader
    {
        public string ReadHtmlFromDisk(EmailTemplateType emailTemplateType)
        {
            string html = "";

            string emailTemplatePath = HttpContext.Current.Server.MapPath("~/EmailTemplate/" + emailTemplateType.ToString() + ".html");

            try
            {
                using (StreamReader sr = new StreamReader(emailTemplatePath))
                {
                    html = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return html;
        }
    }
}