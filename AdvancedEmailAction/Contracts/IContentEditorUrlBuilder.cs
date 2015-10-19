using MikeRobbins.AdvancedEmailAction.Enums;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace MikeRobbins.AdvancedEmailAction.Contracts
{
    public interface IContentEditorUrlBuilder
    {
        string GetContentEditorLink(ContentEditorMode contentEditorMode, Item item, string hostName, ID commandId);
    }
}