namespace MikeRobbins.AdvancedEmailAction.Contacts
{
    public interface IHtmlReader
    {
        string ReadHtmlFromDisk(EmailTemplateType emailTemplateType);
    }
}