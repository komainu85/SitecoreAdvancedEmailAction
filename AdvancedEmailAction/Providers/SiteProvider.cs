using System;
using Sitecore.Data.Items;
using Sitecore.Web;

namespace MikeRobbins.AdvancedEmailAction.Providers
{
    public class SiteProvider : ISiteProvider
    {
        public SiteInfo GetSiteFromSiteItem(Item item)
        {
            SiteInfo site = null;

            var siteInfoList = Sitecore.Configuration.Factory.GetSiteInfoList();

            foreach (SiteInfo siteInfo in siteInfoList)
            {
                if (siteInfo.Domain != "sitecore" && !siteInfo.PhysicalFolder.Contains("sitecore") && item.Paths.FullPath.StartsWith(siteInfo.RootPath + siteInfo.StartItem, StringComparison.InvariantCultureIgnoreCase))
                {
                    site = siteInfo;
                }
            }

            return site;
        }
    }
}