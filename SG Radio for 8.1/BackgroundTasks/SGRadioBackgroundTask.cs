using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.Web.Syndication;

namespace BackgroundTasks
{
    public sealed class SGRadioBackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().Clear();

            if (SecondaryTile.Exists("1000"))
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile("1000").Clear();
            }
            if (SecondaryTile.Exists("1001"))
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile("1001").Clear();
            }
            if (SecondaryTile.Exists("1002"))
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile("1002").Clear();
            }
            if (SecondaryTile.Exists("1003"))
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile("1003").Clear();
            }
            if (SecondaryTile.Exists("1004"))
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile("1004").Clear();
            }
            if (SecondaryTile.Exists("1005"))
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile("1005").Clear();
            }
            if (SecondaryTile.Exists("1006"))
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile("1006").Clear();
            }
            if (SecondaryTile.Exists("1007"))
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile("1007").Clear();
            }
            if (SecondaryTile.Exists("1008"))
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile("1008").Clear();
            }
            if (SecondaryTile.Exists("1009"))
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile("1009").Clear();
            }
            if (SecondaryTile.Exists("1010"))
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile("1010").Clear();
            }
            if (SecondaryTile.Exists("1011"))
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile("1011").Clear();
            }
            if (SecondaryTile.Exists("2000"))
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile("2000").Clear();
            }
            if (SecondaryTile.Exists("2001"))
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile("2001").Clear();
            }
            if (SecondaryTile.Exists("2002"))
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile("2002").Clear();
            }
            if (SecondaryTile.Exists("2003"))
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile("2003").Clear();
            }
            if (SecondaryTile.Exists("2004"))
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile("2004").Clear();
            }
            if (SecondaryTile.Exists("2005"))
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile("2005").Clear();
            }
            if (SecondaryTile.Exists("2006"))
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile("2006").Clear();
            }
            if (SecondaryTile.Exists("2007"))
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile("2007").Clear();
            }
            if (SecondaryTile.Exists("2008"))
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile("2008").Clear();
            }

            deferral.Complete();
        }
    }
}
