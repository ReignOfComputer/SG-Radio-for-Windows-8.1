using SG_Radio_for_8._1.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.System;
using Windows.UI.Xaml.Controls;
using SQLite;
using Windows.ApplicationModel.Store;
using Windows.UI.Popups;
using Windows.UI.StartScreen;

namespace SG_Radio_for_8._1
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            HubPage.startArgument = e.Arguments;

            Frame rootFrame = Window.Current.Content as Frame;
            
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                    }
                }

                Window.Current.Content = rootFrame;
            }
            if (rootFrame.Content == null)
            {
                if (!rootFrame.Navigate(typeof(HubPage)))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            Window.Current.Activate();

            SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
        }

        void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            var about = new SettingsCommand("about", "About", (handler) =>
            {
                Windows.UI.Xaml.Controls.SettingsFlyout settings = new Windows.UI.Xaml.Controls.SettingsFlyout();
                settings.HeaderBackground = new SolidColorBrush(Colors.Red);
                settings.Background = new SolidColorBrush(Colors.White);
                settings.Title = "About";
                settings.Width = 600;
                settings.Content = new AboutUserControl();
                settings.Show();
            });

            args.Request.ApplicationCommands.Add(about);

            var settingsFlyout = new SettingsCommand("settings", "Settings", (handler) =>
            {
                Windows.UI.Xaml.Controls.SettingsFlyout settings = new Windows.UI.Xaml.Controls.SettingsFlyout();
                settings.HeaderBackground = new SolidColorBrush(Colors.Red);
                settings.Background = new SolidColorBrush(Colors.White);
                settings.Title = "Settings";
                settings.Width = 600;
                settings.Content = new SettingsUserControl();
                settings.Show();
            });

            args.Request.ApplicationCommands.Add(settingsFlyout);

            var emaildev = new SettingsCommand("email dev", "Email Dev", (handler) =>
            {
                sendEmail();
            });
            args.Request.ApplicationCommands.Add(emaildev);

            var privacypolicy = new SettingsCommand("privacy policy", "Privacy Policy", (handler) =>
            {
                Windows.UI.Xaml.Controls.SettingsFlyout settings = new Windows.UI.Xaml.Controls.SettingsFlyout();
                settings.HeaderBackground = new SolidColorBrush(Colors.Red);
                settings.Background = new SolidColorBrush(Colors.White);
                settings.Title = "Privacy Policy";
                settings.Width = 600;
                settings.Content = new PrivacyPolicyUserControl();
                settings.Show();
            });

            args.Request.ApplicationCommands.Add(privacypolicy);

            var donate = new SettingsCommand("donate", "Donate", (handler) =>
            {
                donateYay();
            });

            LicenseInformation licenseInformation = CurrentApp.LicenseInformation;
            var productLicense = licenseInformation.ProductLicenses["Donate"];
            if (!productLicense.IsActive)
            {
                args.Request.ApplicationCommands.Add(donate);
            }
        }

        async void donateYay()
        {
            if (!CurrentApp.LicenseInformation.ProductLicenses["Donate"].IsActive)
            {
                try
                {
                    await CurrentApp.RequestProductPurchaseAsync("Donate");
                    if (CurrentApp.LicenseInformation.ProductLicenses["Donate"].IsActive)
                    {
                        try
                        {
                            var messageDialog = new MessageDialog("Thank you for your support!", "Purchase completed");
                            messageDialog.Commands.Add(new UICommand("Close", (command) =>
                            {
                            }));
                            messageDialog.DefaultCommandIndex = 0;
                            await messageDialog.ShowAsync();
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e);
                        }
                    }
                }
                catch (Exception)
                {
                    buyException();
                }
            }
            else
            {
                try
                {
                    var messageDialog = new MessageDialog("You've already supported me today.", "Notice");
                    messageDialog.Commands.Add(new UICommand("Close", (command) =>
                    {
                    }));
                    messageDialog.DefaultCommandIndex = 0;
                    await messageDialog.ShowAsync();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
        }

        async void buyException()
        {
            try
            {
                var messageDialog = new MessageDialog("The purchase failed or was cancelled.", "Error");
                messageDialog.Commands.Add(new UICommand("Close", (command) =>
                {
                }));
                messageDialog.DefaultCommandIndex = 0;
                await messageDialog.ShowAsync();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        async void sendEmail()
        {
            await Launcher.LaunchUriAsync(new Uri("mailto://roc@reignofcomputer.com?subject=SG Radio for Windows 8", UriKind.Absolute));
        }

        private readonly string[] titleId = { "4000", "4001", "4002", "4003", "4004", "4005", "4006", "4007", "4008", "4009", "4010", "4011", "4012", "4013", "4014",
            "5000", "5001", "5002", "5003", "5004", "5005", "5006", "5007", "5008", "5009", "5010", "5011", "5012" };

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().Clear();

            foreach (string titleId in titleId)
            {
                if (SecondaryTile.Exists(titleId))
                {
                    Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile(titleId).Clear();
                }
            }

            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

        #region SQLDatabase
        private static string SQLDBPath = null;

        public static void LoadSQLTableData()
        {
            if (SQLDBPath != null)
                return;

            SQLDBPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db_sgradio.sqlite");
            using (SQLiteConnection db = new SQLiteConnection(SQLDBPath))
            {
                db.CreateTable<StarredDataSQL>();
                db.CreateTable<FavDataSQL>();
            }
        }

        public static void Insert_StarredItem(StarredDataSQL starred)
        {
            if (SQLDBPath == null)
                throw new Exception("SQL is not init yet, please call 'LoadSQLTableData()'.");

            using (SQLiteConnection db = new SQLiteConnection(SQLDBPath))
            {
                SQLiteCommand cmd = db.CreateCommand(string.Format("SELECT * from StarredDataSQL WHERE ID='{0}' LIMIT 1", starred.ID));
                List<StarredDataSQL> result = cmd.ExecuteQuery<StarredDataSQL>();

                if (result.Count == 0)
                {
                    db.RunInTransaction(() =>
                    {
                        db.Insert(starred);
                    });
                }
            }
        }

        public static void Remove_StarredItem(StarredDataSQL starred)
        {
            using (var db = new SQLite.SQLiteConnection(SQLDBPath))
            {
                SQLiteCommand cmd = db.CreateCommand(string.Format("DELETE from StarredDataSQL WHERE StarredTitle = \"{0}\"", starred.StarredTitle));
                List<StarredDataSQL> result = cmd.ExecuteQuery<StarredDataSQL>();
            }
        }

        public static void RemoveAll_StarredItem()
        {
            using (var db = new SQLite.SQLiteConnection(SQLDBPath))
            {
                SQLiteCommand cmd = db.CreateCommand("DELETE from StarredDataSQL");
                List<StarredDataSQL> result = cmd.ExecuteQuery<StarredDataSQL>();
            }
        }

        public static List<StarredDataSQL> GetAllSQL_StarredItems()
        {
            if (SQLDBPath == null)
                throw new Exception("SQL is not init yet, please call 'LoadSQLTableData()'.");

            using (var db = new SQLite.SQLiteConnection(SQLDBPath))
            {
                return db.Table<StarredDataSQL>().ToList();
            }
        }

        public static void Insert_FavoriteItem(FavDataSQL fav)
        {
            if (SQLDBPath == null)
                throw new Exception("SQL is not init yet, please call 'LoadSQLTableData()'.");

            using (SQLiteConnection db = new SQLiteConnection(SQLDBPath))
            {
                SQLiteCommand cmd = db.CreateCommand(string.Format("SELECT * from FavDataSQL WHERE FavID='{0}' LIMIT 1", fav.FavID));
                List<FavDataSQL> result = cmd.ExecuteQuery<FavDataSQL>();

                if (result.Count == 0)
                {
                    db.RunInTransaction(() =>
                    {
                        db.Insert(fav);
                    });
                }
            }
        }

        public static void Remove_FavoriteItem(FavDataSQL starred)
        {
            using (var db = new SQLite.SQLiteConnection(SQLDBPath))
            {
                SQLiteCommand cmd = db.CreateCommand(string.Format("DELETE from FavDataSQL WHERE FavID = {0}", starred.FavID));
                List<FavDataSQL> result = cmd.ExecuteQuery<FavDataSQL>();
            }
        }

        public static void RemoveAll_FavoriteItem()
        {
            using (var db = new SQLite.SQLiteConnection(SQLDBPath))
            {
                SQLiteCommand cmd = db.CreateCommand("DELETE from FavDataSQL");
                List<FavDataSQL> result = cmd.ExecuteQuery<FavDataSQL>();
            }
        }

        public static List<FavDataSQL> GetAllSQL_FavoriteItems()
        {
            if (SQLDBPath == null)
                throw new Exception("SQL is not init yet, please call 'LoadSQLTableData()'.");

            using (var db = new SQLite.SQLiteConnection(SQLDBPath))
            {
                return db.Table<FavDataSQL>().ToList();
            }
        }
        #endregion
    }
}
