using Sitecore.Data.Items;
using Sitecore.Web;

namespace MikeRobbins.AdvancedEmailAction.Contracts
{
    public interface ISiteProvider
    {
        SiteInfo GetSiteFromSiteItem(Item item);
    }
}