using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.Trees;
using Umbraco.Cms.Core.Notifications;

namespace punkOptimise.Notifications
{
    public class PunkOptimiseTreeHandler : INotificationHandler<MenuRenderingNotification>
    {
        public void Handle(MenuRenderingNotification notification)
        {
            switch (notification.TreeAlias)
            {
                case Constants.Trees.Media:
                    if (notification.NodeId != "0" &&
                        notification.NodeId != Constants.System.RecycleBinMedia.ToString() &&
                        notification.NodeId != Constants.System.Root.ToString())
                    {

                        var optimise = new MenuItem("optimiseNode", "punkOptimise");
                        optimise.AdditionalData.Add("actionView", "/App_Plugins/punkOptimise/backoffice/punkOptimise.html");
                        optimise.Icon = "scan color-deep-purple";
                        notification.Menu.Items.Add(optimise);
                    }
                    break;
            }
        }
    }
}
