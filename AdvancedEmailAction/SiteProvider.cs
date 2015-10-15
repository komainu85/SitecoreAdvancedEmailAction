using System;
using Sitecore.Data.Items;
using Sitecore.Sites;
using Sitecore.Web;

namespace MikeRobbins.AdvancedEmailAction
{
    public class SiteProvider
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