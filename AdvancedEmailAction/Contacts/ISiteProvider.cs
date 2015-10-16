using Sitecore.Data.Items;
using Sitecore.Web;

namespace MikeRobbins.AdvancedEmailAction.Contacts
{
    public interface ISiteProvider
    {
        SiteInfo GetSiteFromSiteItem(Item item);
    }
}