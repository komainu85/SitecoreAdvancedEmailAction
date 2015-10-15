using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MikeRobbins.AdvancedEmailAction.Contacts;
using MikeRobbins.AdvancedEmailAction.Entities;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Sites;

namespace MikeRobbins.AdvancedEmailAction
{
    public class ContentEditorUrlBuilder : IContentEditorUrlBuilder
    {
        public string GetContentEditorLink(ContentEditorMode contentEditorMode, Item item, string hostName, ID commandId)
        {
            switch (contentEditorMode)
            {
                case ContentEditorMode.Editor:
                    return ("http://" + hostName.Replace("http://", "") + "/sitecore/shell/Applications/Content%20editor.aspx?fo=" + item.ID.ToString() + "&id=" + item.ID.ToString() + "&la=" + item.Language.Name + "&v=" + item.Version.Number.ToString() + "&sc_bw=1");

                case ContentEditorMode.Preview:
                    return ("http://" + hostName.Replace("http://", "") + "/sitecore/shell/feeds/action.aspx?c=Preview&id=" + item.ID.ToString() + "&la=" + item.Language.Name + "&v=" + item.Version.Number.ToString());

                case ContentEditorMode.Submit:
                    return ("http://" + hostName.Replace("http://", "") + "/sitecore/shell/feeds/action.aspx?c=Workflow&id=" + item.ID.ToString() + "&la=" + item.Language.Name + "&v=" + item.Version.Number.ToString() + "&cmd=" + commandId.ToString());

                case ContentEditorMode.SubmitComment:
                    return ("http://" + hostName.Replace("http://","") + "/sitecore/shell/feeds/action.aspx?c=Workflow&id=" + item.ID.ToString() + "&la=" + item.Language.Name + "&v=" + item.Version.Number.ToString() + "&cmd=" + commandId.ToString() + "&nc=1");
            }
            return "";
        }
    }
}
