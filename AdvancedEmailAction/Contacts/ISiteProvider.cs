using Sitecore.Data.Items;
using Sitecore.Web;

namespace MikeRobbins.AdvancedEmailAction.Providers
{
    public interface ISiteProvider
    {
        SiteInfo GetSiteFromSiteItem(Item item);
    }
}