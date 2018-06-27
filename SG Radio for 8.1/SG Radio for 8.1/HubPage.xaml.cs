using SG_Radio_for_8._1.Common;
using SG_Radio_for_8._1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Windows.ApplicationModel.DataTransfer;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Media;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using NotificationsExtensions.TileContent;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SG_Radio_for_8._1
{
    public sealed partial class HubPage : Page
    {
        #region Declarations
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private string playingId;
        private string globalId;
        private string holdThisSource;
        private string playingImg;
        private string selectedFav, selectedTime, selectedImage;
        public static string startArgument;
        private bool appBarSelected = false;
        private int countdown;
        private DispatcherTimer anitimer;
        private ItemClickEventArgs itemClick;
        private DispatcherTimer timer = new DispatcherTimer();
        private DispatcherTimer timerStandby = new DispatcherTimer();
        private DispatcherTimer timerShutdown = new DispatcherTimer();
        private IAsyncOperation<IUICommand> asyncCommand;
        private SystemMediaTransportControls systemControls;

        private int stationType = 0; // 0 = Mediacorp, 1 = SPH, 2 = BBC, 3 = So Drama, 4 = Others

        // Mediacorp
        private readonly string[] station0 = { "4002", "4005", "4006", "4007", "4009", "5001", "5002", "5004", "5006", "5007", "5008" };

        // SPH
        private readonly string[] station1 = { "4001", "4003", "4004", "5003", "5005" };

        // BBC
        private readonly string[] station2 = { "4000" };

        // So Drama
        private readonly string[] station3 = { "4008", "5000" };

        // Others
        private readonly string[] station4 = { "4010", "4011", "4012", "4013", "4014", "5009", "5010", "5011", "5012" };
        #endregion

        #region Page Setup
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public HubPage()
        {
            this.InitializeComponent();
            RequestedTheme = ElementTheme.Dark;
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;

            DataTransferManager.GetForCurrentView().DataRequested += OnDataRequested;

            BottomAppBar.Opened += BottomAppBar_Opened;
            BottomAppBar.Closed += BottomAppBar_Closed;

            systemControls = SystemMediaTransportControls.GetForCurrentView();
            systemControls.IsPlayEnabled = true;
            systemControls.IsStopEnabled = true;
            systemControls.IsPauseEnabled = true;
            systemControls.ButtonPressed += SystemControls_ButtonPressed;
            App.LoadSQLTableData();
            initializeFavoritesHub();
            initializeStarredHub();
            checkForFirstRun();
            startMusic();

            this.Loaded += HubPage_Loaded;
        }

        void HubPage_Loaded(object sender, RoutedEventArgs e)
        {
            anitimer = new DispatcherTimer();
            anitimer.Tick += anitimer_Tick;
            anitimer.Interval = TimeSpan.FromSeconds(2);
            anitimer.Start();
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.ApplicationSettings.SettingsPane.Show();
        }

        void anitimer_Tick(object sender, object e)
        {
            ellipsisMove.Begin();
            EllipsisGrid.Visibility = Visibility.Visible;
            ellipsisMove2.Begin();
            anitimer.Stop();
        }

        void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            string input;
            SystemMediaTransportControls.GetForCurrentView().DisplayUpdater.Type = MediaPlaybackType.Music;
            if (SystemMediaTransportControls.GetForCurrentView().DisplayUpdater.MusicProperties.Title != "")
            {
                input = @"I'm listening to " + SystemMediaTransportControls.GetForCurrentView().DisplayUpdater.MusicProperties.Title + " on " + SystemMediaTransportControls.GetForCurrentView().DisplayUpdater.MusicProperties.Artist + " using SG Radio! <br>https://apps.microsoft.com/webpdp/app/sg-radio/81dd2ed1-8b0a-4017-a6d8-cb7b28837c2a";
            }
            else
            {
                input = @"I'm listening to Singapore's Radio Stations using SG Radio! <br>https://apps.microsoft.com/webpdp/app/sg-radio/81dd2ed1-8b0a-4017-a6d8-cb7b28837c2a";
            }

            DataRequest request = args.Request;

            request.Data.Properties.Title = "SG Radio for Windows Store";
            request.Data.Properties.Description = string.Format(input);

            var formatted = HtmlFormatHelper.CreateHtmlFormat(input);
            request.Data.SetHtmlFormat(formatted);
        }

        private void checkForFirstRun()
        {
            if (!ApplicationData.Current.RoamingSettings.Values.ContainsKey("FirstRunV4004"))
            {
                var messageDialog = new MessageDialog("Welcome to SG Radio!\n\nChangelog:\n- Whoa, it's been over 3 years since the last update! Were you an active user of SG Radio? Let me know how I did by rating the app!\n- FIXED: All the streams that have been migrated\n- FIXED: All the rebranding for the stations\n- ADDED: Various new stations\n- ADDED: Settings Charms button in AppBar for Windows 10 users\n- CHANGED: Optimized how streaming works, much less lag now\n- CHANGED: Song name now appears before the artist instead of the other way around\n- CHANGED: Current playing station and track are now neatly centered\n- CHANGED: Removed redundant Refresh button\n- WARNING: All favorites and pinned tiles need to be reset after this major update\n- HOTFIX [4.0.0.1]: Users with versions before V4 would face issues with favorites and starred tracks after updating - this update wipes the database to fix that\n- CHANGED [4.0.0.2]: Microsoft apparently changed their policies (probably in a bid to piss off more developers and get them off their platform), so here's a quick change to appease them regarding donation links\n- FIXED [4.0.0.3]: SPH migrated some of their streams, thanks everyone who reported.\n- FIXED [4.0.0.4]: Whoops, updated the streams but not the favorites. SPH Favorites should be working again.", "SG Radio Update: v4.0.0.4");
                messageDialog.Commands.Add(new UICommand("Close", (command) =>
                {
                }));
                messageDialog.Commands.Add(new UICommand("Rate App", (command) =>
                {
                    rateApp();
                }));
                messageDialog.Commands.Add(new UICommand("Email Dev", (command) =>
                {
                    sendEmail();
                }));
                messageDialog.DefaultCommandIndex = 0;

                if (asyncCommand != null)
                {
                    asyncCommand.Cancel();
                }

                asyncCommand = messageDialog.ShowAsync();

                ApplicationData.Current.RoamingSettings.Values["FirstRunV4004"] = true;
            }
        }

        private async void sendEmail()
        {
            await Launcher.LaunchUriAsync(new Uri("mailto://roc@reignofcomputer.com?subject=SG Radio for Windows Store", UriKind.Absolute));
        }

        private async void rateApp()
        {
            var storeURI = new Uri("ms-windows-store:PDP?PFN=20694ReignOfComputer.SGRadio_whkbyfjgnjkag");
            await Launcher.LaunchUriAsync(storeURI);
        }

        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var SGRADIODataGroups = await SGRADIODataSource.GetGroupsAsync();
            this.DefaultViewModel["GroupsSGRADIO"] = SGRADIODataGroups;
        }

        private void initializeStarredHub()
        {
            var starredSQL = App.GetAllSQL_StarredItems();
            List<StarredData> starredList = new List<StarredData>();
            if (starredSQL.Count == 0)
            {
                starredList.Add(new StarredData("ms-appx:///Images/Absence.png", "No Starred Tracks", ""));

            }
            else
            {
                for (int i = 0; i < starredSQL.Count; i++)
                {
                    starredList.Add(new StarredData(starredSQL[i].StarredImage, starredSQL[i].StarredTitle, starredSQL[i].StarredTime));
                }
            }

            StarredHub.DataContext = starredList;
        }

        private void initializeFavoritesHub()
        {
            var favoritesSQL = App.GetAllSQL_FavoriteItems();
            List<FavData> dataList = new List<FavData>();
            if (favoritesSQL.Count == 0)
            {
                dataList.Add(new FavData("ms-appx:///Images/Absence.png", "No Favourites"));
            }
            else
            {
                for (int i = 0; i < favoritesSQL.Count; i++)
                {
                    dataList.Add(new FavData(Constants.getImage(favoritesSQL[i].FavID), Constants.getName(favoritesSQL[i].FavID)));
                }
            }
            FavHub.DataContext = dataList;
        }

        private void SetLiveTile()
        {
            string TileName = playingId;
            var itemImg = playingImg;

            var LiveTile = @"<tile>
                                <visual version=""1"">
                                  <binding template=""TileWideSmallImageAndText01"">
                                    <text id=""1"">" + systemControls.DisplayUpdater.MusicProperties.Title + @"</text>
                                    <image id=""1"" src=""" + itemImg + @"""/>
                                  </binding>
                                  <binding template=""TileSquarePeekImageAndText02"">
                                    <text id=""2"">" + systemControls.DisplayUpdater.MusicProperties.Title + @"</text>
                                    <image id=""1"" src=""" + itemImg + @"""/>
                                  </binding>
                                </visual>
                              </tile>";

            try
            {
                if (SecondaryTile.Exists(TileName))
                {
                    TileUpdater updater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(TileName);
                    updater.EnableNotificationQueue(false);
                    updater.Clear();

                    ITileWide310x150ImageAndText01 tileContent = TileContentFactory.CreateTileWide310x150ImageAndText01();
                    tileContent.TextCaptionWrap.Text = systemControls.DisplayUpdater.MusicProperties.Title;
                    tileContent.Image.Src = itemImg;
                    tileContent.BaseUri = playingImg;

                    ITileSquare150x150PeekImageAndText01 squareContent = TileContentFactory.CreateTileSquare150x150PeekImageAndText01();
                    squareContent.TextHeading.Text = systemControls.DisplayUpdater.MusicProperties.Title;
                    squareContent.Image.Src = itemImg;
                    tileContent.Square150x150Content = squareContent;

                    updater.Update(tileContent.CreateNotification());
                }

                XmlDocument tileXml = new XmlDocument();
                tileXml.LoadXml(LiveTile);
                var tileNotification = new TileNotification(tileXml);
                TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            }
            catch (Exception)
            {
            }
        }

        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            mediaElement1.Stop();

            itemClick = e;
            progressring.IsActive = true;
            var itemId = ((SGRADIODataItem)e.ClickedItem).UniqueId;
            var itemName = ((SGRADIODataItem)e.ClickedItem).Title;
            var itemImage = ((SGRADIODataItem)e.ClickedItem).ImagePath;
            var itemContent = ((SGRADIODataItem)e.ClickedItem).Content;

            if (station0.Contains(itemId))
                stationType = 0;
            else if (station1.Contains(itemId))
                stationType = 1;
            else if (station2.Contains(itemId))
                stationType = 2;
            else if (station3.Contains(itemId))
                stationType = 3;
            else
                stationType = 4;

            globalId = itemId;
            playingId = itemId;
            playingImg = itemImage;
            mediaElement1.Source = new Uri(itemContent, UriKind.Absolute);
            holdThisSource = @itemContent;
            SystemMediaTransportControlsDisplayUpdater updater = systemControls.DisplayUpdater;
            updater.Type = MediaPlaybackType.Music;
            updater.MusicProperties.Artist = "SG Radio - " + itemName;
            updater.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(itemImage, UriKind.Absolute));
            updater.Update();
            efrTick(1, 0);
            efrTicker();
            mediaElement1.Play();
            systemControls.PlaybackStatus = MediaPlaybackStatus.Playing;
            toggleAppBar(true);
        }

        private void ItemView_ItemClickFav(object sender, ItemClickEventArgs e)
        {
            mediaElement1.Stop();

            var itemName = ((FavData)e.ClickedItem).FavTitle;
            if (itemName == "No Favourites")
            {
                return;
            }
            itemClick = e;
            progressring.IsActive = true;
            var itemId = Constants.getIdFromName(itemName);
            var itemImage = ((FavData)e.ClickedItem).FavImage;
            var itemContent = Constants.getStream(itemId);

            if (station0.Contains(itemId))
                stationType = 0;
            else if (station1.Contains(itemId))
                stationType = 1;
            else if (station2.Contains(itemId))
                stationType = 2;
            else if (station3.Contains(itemId))
                stationType = 3;
            else
                stationType = 4;

            globalId = itemId;
            playingId = itemId;
            playingImg = itemImage;
            mediaElement1.Source = new Uri(itemContent, UriKind.Absolute);
            holdThisSource = @itemContent;
            SystemMediaTransportControlsDisplayUpdater updater = systemControls.DisplayUpdater;
            updater.Type = MediaPlaybackType.Music;
            updater.MusicProperties.Artist = "SG Radio - " + itemName;
            updater.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(itemImage, UriKind.Absolute));
            updater.Update();
            efrTick(1, 0);
            efrTicker();
            mediaElement1.Play();
            systemControls.PlaybackStatus = MediaPlaybackStatus.Playing;
            toggleAppBar(true);
        }
        #endregion

        #region Standby
        private void startStandby()
        {
            BottomAppBar.IsOpen = false;
            BottomAppBar.IsEnabled = false;
            timerStandby.Interval = TimeSpan.FromSeconds(1);
            timerStandby.Tick += standby_Tick;
            timerStandby.Start();
        }

        private void standby_Tick(object sender, object e)
        {
            standbyTime.Text = DateTime.Now.ToString("hh:mm tt");
            standbyStation.Text = currStation.Text;
            standbyTrack.Text = currTrack.Text;
        }

        private void standbyClose(object sender, RoutedEventArgs e)
        {
            standbyGrid.Visibility = Visibility.Collapsed;
            BottomAppBar.IsEnabled = true;
            timerStandby.Stop();
        }

        public SolidColorBrush ColorStringToBrush(string name)
        {
            var property = typeof(Colors).GetRuntimeProperty(name);
            if (property != null)
            {
                return new SolidColorBrush((Color)property.GetValue(null));
            }
            else
            {
                return null;
            }
        }

        private void standbyView_Click(object sender, RoutedEventArgs e)
        {
            startStandby();

            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("bgColor"))
            {
                standbyGrid.Background = ColorStringToBrush(ApplicationData.Current.RoamingSettings.Values["bgColor"].ToString());

                if (ApplicationData.Current.RoamingSettings.Values["bgColor"].ToString() == "White")
                {
                    backButton.RequestedTheme = ElementTheme.Light;
                }
                else
                {
                    backButton.RequestedTheme = ElementTheme.Dark;
                }
            }

            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("txtColor"))
            {
                standbyTime.Foreground = ColorStringToBrush(ApplicationData.Current.RoamingSettings.Values["txtColor"].ToString());
                standbyStation.Foreground = ColorStringToBrush(ApplicationData.Current.RoamingSettings.Values["txtColor"].ToString());
                standbyTrack.Foreground = ColorStringToBrush(ApplicationData.Current.RoamingSettings.Values["txtColor"].ToString());
            }

            standbyGrid.Visibility = Visibility.Visible;
        }
        #endregion

        #region Media Control
        private void startMusic()
        {
            if (startArgument == "")
            {
                return;
            }

            mediaElement1.Stop();

            progressring.IsActive = true;
            var itemName = Constants.getName(startArgument);
            var itemId = startArgument;
            var itemImage = Constants.getImage(itemId);
            var itemContent = Constants.getStream(itemId);

            globalId = itemId;
            playingId = itemId;
            playingImg = itemImage;

            if (station0.Contains(itemId))
                stationType = 0;
            else if (station1.Contains(itemId))
                stationType = 1;
            else if (station2.Contains(itemId))
                stationType = 2;
            else if (station3.Contains(itemId))
                stationType = 3;
            else
                stationType = 4;

            mediaElement1.Source = new Uri(itemContent, UriKind.Absolute);
            holdThisSource = @Constants.getStream(startArgument);
            SystemMediaTransportControlsDisplayUpdater updater = systemControls.DisplayUpdater;
            updater.Type = MediaPlaybackType.Music;
            updater.MusicProperties.Artist = "SG Radio - " + itemName;
            updater.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(itemImage, UriKind.Absolute));
            updater.Update();
            efrTick(1, 0);
            efrTicker();
            mediaElement1.Play();
            systemControls.PlaybackStatus = MediaPlaybackStatus.Playing;
            toggleAppBar(true);
        }

        async void SystemControls_ButtonPressed(SystemMediaTransportControls sender,
    SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                switch (args.Button)
                {
                    case SystemMediaTransportControlsButton.Play:
                        if (mediaElement1.CurrentState != MediaElementState.Playing)
                        {
                            restartSource();
                        }
                        else
                        {
                            completeClosure();
                        }
                        break;
                    case SystemMediaTransportControlsButton.Pause:
                    case SystemMediaTransportControlsButton.Stop:
                        completeClosure();
                        break;
                    default:
                        break;
                }
            });
        }

        private void mediaElement1_MediaFailed_1(object sender, ExceptionRoutedEventArgs e)
        {
            NoPlayback();
        }

        private void mediaElement1_MediaOpened_1(object sender, RoutedEventArgs e)
        {
            progressring.IsActive = false;
            statusGrid.Visibility = Visibility.Visible;
        }

        private void mediaElement1_MediaEnded_1(object sender, RoutedEventArgs e)
        {
            completeClosure();
        }

        private void NoPlayback()
        {
            completeClosure();

            var messageDialog = new MessageDialog("Check your Internet Connection or Re-Download the App.", "Media Playback Failed: Station Unreachable");
            messageDialog.Commands.Add(new UICommand("Close", (command) =>
            {
            }));
            messageDialog.DefaultCommandIndex = 0;

            if (asyncCommand != null)
            {
                asyncCommand.Cancel();
            }

            asyncCommand = messageDialog.ShowAsync();
        }

        private readonly string[] titleId = { "4000", "4001", "4002", "4003", "4004", "4005", "4006", "4007", "4008", "4009", "4010", "4011", "4012", "4013", "4014",
            "5000", "5001", "5002", "5003", "5004", "5005", "5006", "5007", "5008", "5009", "5010", "5011", "5012" };

        private async void completeClosure()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                mediaElement1.Stop();
                mediaElement1.Source = null;
                progressring.IsActive = false;
                statusGrid.Visibility = Visibility.Collapsed;
                toggleAppBar(false);
                TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                timer.Stop();

                foreach (string titleId in titleId)
                {
                    if (SecondaryTile.Exists(titleId))
                    {
                        TileUpdateManager.CreateTileUpdaterForSecondaryTile(titleId).Clear();
                    }
                }

                systemControls.PlaybackStatus = MediaPlaybackStatus.Stopped;
            });
        }

        private async void restartSource()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                mediaElement1.Source = new Uri(holdThisSource, UriKind.Absolute);
                mediaElement1.Play();
                progressring.IsActive = true;
                timer.Start();
                toggleAppBar(true);
                systemControls.PlaybackStatus = MediaPlaybackStatus.Playing;
            });
        }

        private void VolumeChangedEvent(object sender, RangeBaseValueChangedEventArgs e)
        {
            mediaElement1.Volume = sliderVolume.Value / 100;
        }
        #endregion

        #region Title Fetch
        private void efrTick(int type, int appbarMode)
        {
            int efrTickStationType = 0;

            if (appbarMode == 0)
                efrTickStationType = stationType;
            else
            {
                if (station0.Contains(globalId))
                    efrTickStationType = 0;
                else if (station1.Contains(globalId))
                    efrTickStationType = 1;
                else if (station2.Contains(globalId))
                    efrTickStationType = 2;
                else if (station3.Contains(globalId))
                    efrTickStationType = 3;
                else
                    efrTickStationType = 4;
            }

            if (efrTickStationType == 0)
                mediacorpTitle(type, appbarMode);
            else if (efrTickStationType == 1)
                sphTitle(type, appbarMode);
            else if (efrTickStationType == 2)
                bbcTitle(type, appbarMode);
            else if (efrTickStationType == 3)
                soDramaTitle(type, appbarMode);
            else
                othersTitle(type, appbarMode);
        }

        void efrTicker()
        {
            timer.Tick += (s, o) =>
            {
                efrTick(1, 0);
            };
            timer.Interval = new TimeSpan(0, 0, 30);
            bool enabled = timer.IsEnabled;
            timer.Start();
        }

        private async void mediacorpTitle(int type, int appbarMode)
        {
            var uri = new Uri("http://liveradio.toggle.sg/api/playouthistory?stationId=905fm");
            var client = new HttpClient();
            string finalId = "4000";
            if (appbarMode == 0)
            {
                uri = new Uri(Constants.getTitle(playingId));
                finalId = playingId;
            }

            else if (appbarMode == 1)
            {
                uri = new Uri(Constants.getTitle(globalId));
                finalId = globalId;
            }

            string response = "";
            var rootArtist = "";
            var rootTrack = "";

            try
            {
                response = await client.GetStringAsync(uri);
                List<PlayListItem> playList = JsonConvert.DeserializeObject<List<PlayListItem>>(response);

                PlayListItem firstItem = playList.First();
                rootTrack = firstItem.Song.Track;
                rootArtist = firstItem.Song.Artist;
            }
            catch (Exception)
            {
                rootArtist = "Not Available";
            }

            if (appbarMode == 0)
            {
                SystemMediaTransportControlsDisplayUpdater updater = systemControls.DisplayUpdater;
                updater.MusicProperties.Title = rootTrack + " - " + rootArtist;
                updater.Update();
            }

            if (type == 0)
            {
                var messageDialog = new MessageDialog(rootTrack + " - " + rootArtist, "SG Radio - " + Constants.getName(finalId) + ": Now Playing");
                messageDialog.Commands.Add(new UICommand("Close", (command) =>
                {
                }));
                messageDialog.Commands.Add(new UICommand("Copy to Clipboard", (command) =>
                {
                    DataPackage package = new DataPackage();
                    package.SetText(rootTrack + " - " + rootArtist);
                    Clipboard.SetContent(package);
                }));
                messageDialog.Commands.Add(new UICommand("Star", (command) =>
                {
                    App.Insert_StarredItem((new StarredDataSQL()
                    {
                        StarredTitle = rootTrack + " - " + rootArtist,
                        StarredImage = Constants.getImage(finalId),
                        StarredTime = DateTime.Now.ToString()
                    }));
                    initializeStarredHub();
                }));
                messageDialog.DefaultCommandIndex = 0;
                if (asyncCommand != null)
                {
                    asyncCommand.Cancel();
                }

                asyncCommand = messageDialog.ShowAsync();
            }
            else if (type == 1)
            {
                currStation.Text = "Current Station: " + Constants.getName(finalId);
                currTrack.Text = "Current Track: " + rootTrack + " - " + rootArtist;
                ToolTipService.SetToolTip(currTrack, rootTrack + " - " + rootArtist);
                SetLiveTile();
            }
        }

        private async void sphTitle(int type, int appbarMode)
        {
            var uri = new Uri("http://np.tritondigital.com/public/nowplaying?mountName=MONEY_893&numberToFetch=1");
            var client = new HttpClient();
            string finalId = "4000";

            if (appbarMode == 0)
            {
                switch (playingId)
                {
                    case "4001":
                        uri = new Uri("http://np.tritondigital.com/public/nowplaying?mountName=MONEY_893&numberToFetch=1");
                        break;
                    case "4003":
                        uri = new Uri("http://np.tritondigital.com/public/nowplaying?mountName=ONE_FM_913&numberToFetch=1");
                        break;
                    case "4004":
                        uri = new Uri("http://np.tritondigital.com/public/nowplaying?mountName=KISS_92&numberToFetch=1");
                        break;
                    case "5003":
                        uri = new Uri("http://np.tritondigital.com/public/nowplaying?mountName=HAO_963&numberToFetch=1");
                        break;
                    case "5005":
                        uri = new Uri("http://np.tritondigital.com/public/nowplaying?mountName=UFM_1003&numberToFetch=1");
                        break;
                    default:
                        break;
                }
                finalId = playingId;
            }
            else if (appbarMode == 1)
            {
                switch (globalId)
                {
                    case "4001":
                        uri = new Uri("http://np.tritondigital.com/public/nowplaying?mountName=MONEY_893&numberToFetch=1");
                        break;
                    case "4003":
                        uri = new Uri("http://np.tritondigital.com/public/nowplaying?mountName=ONE_FM_913&numberToFetch=1");
                        break;
                    case "4004":
                        uri = new Uri("http://np.tritondigital.com/public/nowplaying?mountName=KISS_92&numberToFetch=1");
                        break;
                    case "5003":
                        uri = new Uri("http://np.tritondigital.com/public/nowplaying?mountName=HAO_963&numberToFetch=1");
                        break;
                    case "5005":
                        uri = new Uri("http://np.tritondigital.com/public/nowplaying?mountName=UFM_1003&numberToFetch=1");
                        break;
                    default:
                        break;
                }
                finalId = globalId;
            }

            var rootTrack = "";
            var rootArtist = "";

            try
            {
                string response = await client.GetStringAsync(uri);
                XmlDocument doc1 = new XmlDocument();
                doc1.LoadXml(response);
                rootTrack = doc1.LastChild.FirstChild.ChildNodes[2].InnerText;
                rootArtist = doc1.LastChild.FirstChild.ChildNodes[3].InnerText;
            }
            catch (Exception)
            {

            }

            if (appbarMode == 0)
            {
                SystemMediaTransportControlsDisplayUpdater updater = systemControls.DisplayUpdater;
                updater.MusicProperties.Title = rootTrack + " - " + rootArtist;
                updater.Update();
            }

            if (type == 0)
            {
                var messageDialog = new MessageDialog(rootTrack + " - " + rootArtist, "SG Radio - " + Constants.getName(finalId) + ": Now Playing");
                messageDialog.Commands.Add(new UICommand("Close", (command) =>
                {
                }));
                messageDialog.Commands.Add(new UICommand("Copy to Clipboard", (command) =>
                {
                    DataPackage package = new DataPackage();
                    package.SetText(rootTrack + " - " + rootArtist);
                    Clipboard.SetContent(package);
                }));
                messageDialog.Commands.Add(new UICommand("Star", (command) =>
                {
                    App.Insert_StarredItem((new StarredDataSQL()
                    {
                        StarredTitle = rootTrack + " - " + rootArtist,
                        StarredImage = Constants.getImage(finalId),
                        StarredTime = DateTime.Now.ToString()
                    }));
                    initializeStarredHub();
                }));
                messageDialog.DefaultCommandIndex = 0;
                if (asyncCommand != null)
                {
                    asyncCommand.Cancel();
                }

                asyncCommand = messageDialog.ShowAsync();
            }
            else if (type == 1)
            {
                currStation.Text = "Current Station: " + Constants.getName(finalId);
                currTrack.Text = "Current Track: " + rootTrack + " - " + rootArtist;
                ToolTipService.SetToolTip(currTrack, rootTrack + " - " + rootArtist);
                SetLiveTile();
            }
        }

        private async void soDramaTitle(int type, int appbarMode)
        {
            var uri = new Uri("https://www.power98.com.sg/wp-admin/admin-ajax.php?action=orangePlaylist-json");
            var client = new HttpClient();
            string finalId = "4008";

            if (appbarMode == 0)
            {
                switch (playingId)
                {
                    case "4008":
                        uri = new Uri("https://www.power98.com.sg/wp-admin/admin-ajax.php?action=orangePlaylist-json");
                        break;
                    case "5000":
                        uri = new Uri("https://www.883jia.com.sg/wp-admin/admin-ajax.php?action=orangePlaylist-json");
                        break;
                    default:
                        break;
                }
                finalId = playingId;
            }
            else if (appbarMode == 1)
            {
                switch (globalId)
                {
                    case "4008":
                        uri = new Uri("https://www.power98.com.sg/wp-admin/admin-ajax.php?action=orangePlaylist-json");
                        break;
                    case "5000":
                        uri = new Uri("https://www.883jia.com.sg/wp-admin/admin-ajax.php?action=orangePlaylist-json");
                        break;
                    default:
                        break;
                }
                finalId = globalId;
            }

            var rootTrack = "";
            var rootArtist = "";
            string response = "";

            try
            {
                response = await client.GetStringAsync(uri);
                dynamic data = JArray.Parse(response);
                rootTrack = data[0].cue_title;
                rootArtist = data[0].track_artist_name;
            }
            catch (Exception)
            {
                rootArtist = "Not available";
            }

            if (appbarMode == 0)
            {
                SystemMediaTransportControlsDisplayUpdater updater = systemControls.DisplayUpdater;
                updater.MusicProperties.Title = rootTrack + " - " + rootArtist;
                updater.Update();
            }

            if (type == 0)
            {
                var messageDialog = new MessageDialog(rootTrack + " - " + rootArtist, "SG Radio - " + Constants.getName(finalId) + ": Now Playing");
                messageDialog.Commands.Add(new UICommand("Close", (command) =>
                {
                }));
                messageDialog.Commands.Add(new UICommand("Copy to Clipboard", (command) =>
                {
                    DataPackage package = new DataPackage();
                    package.SetText(rootTrack + " - " + rootArtist);
                    Clipboard.SetContent(package);
                }));
                messageDialog.Commands.Add(new UICommand("Star", (command) =>
                {
                    App.Insert_StarredItem((new StarredDataSQL()
                    {
                        StarredTitle = rootTrack + " - " + rootArtist,
                        StarredImage = Constants.getImage(finalId),
                        StarredTime = DateTime.Now.ToString()
                    }));
                    initializeStarredHub();
                }));
                messageDialog.DefaultCommandIndex = 0;
                if (asyncCommand != null)
                {
                    asyncCommand.Cancel();
                }

                asyncCommand = messageDialog.ShowAsync();
            }
            else if (type == 1)
            {
                currStation.Text = "Current Station: " + Constants.getName(finalId);
                currTrack.Text = "Current Track: " + rootTrack + " - " + rootArtist;
                ToolTipService.SetToolTip(currTrack, rootTrack + " - " + rootArtist);
                SetLiveTile();
            }
        }

        private async void bbcTitle(int type, int appbarMode)
        {
            var uri = new Uri("http://polling.bbc.co.uk/modules/onairpanel/include/bbc_world_service.jsonp");
            var client = new HttpClient();
            string response = "";
            string titleFinish = "";

            try
            {
                response = await client.GetStringAsync(uri);
                var titleStart = response.IndexOf("On Now : ");
                titleStart = titleStart + 9;
                var testString = response.Substring(titleStart, response.Length - titleStart);
                var closureStart = testString.IndexOf("\\\">") + 3;
                var closureEnd = testString.IndexOf("<\\/a>");
                titleFinish = testString.Substring(closureStart, closureEnd - closureStart);
            }
            catch (Exception)
            {
                titleFinish = "Not available";
            }

            if (appbarMode == 0)
            {
                SystemMediaTransportControlsDisplayUpdater updater = systemControls.DisplayUpdater;
                updater.MusicProperties.Title = titleFinish;
                updater.Update();
            }

            if (type == 0)
            {
                var messageDialog = new MessageDialog(titleFinish, "SG Radio - BBC World Service" + ": Now Playing");
                messageDialog.Commands.Add(new UICommand("Close", (command) =>
                {
                }));
                messageDialog.Commands.Add(new UICommand("Copy to Clipboard", (command) =>
                {
                    DataPackage package = new DataPackage();
                    package.SetText(titleFinish);
                    Clipboard.SetContent(package);
                }));
                messageDialog.Commands.Add(new UICommand("Star", (command) =>
                {
                    App.Insert_StarredItem((new StarredDataSQL()
                    {
                        StarredTitle = titleFinish,
                        StarredImage = Constants.getImage("4000"),
                        StarredTime = DateTime.Now.ToString()
                    }));
                    initializeStarredHub();
                }));
                messageDialog.DefaultCommandIndex = 0;
                if (asyncCommand != null)
                {
                    asyncCommand.Cancel();
                }

                asyncCommand = messageDialog.ShowAsync();
            }
            else if (type == 1)
            {
                currStation.Text = "Current Station: BBC World Service";
                currTrack.Text = "Current Track: " + titleFinish;
                ToolTipService.SetToolTip(currTrack, titleFinish);
                SetLiveTile();
            }
        }

        private async void othersTitle(int type, int appbarMode)
        {
            var uri = new Uri("http://onlineradiobox.com/json/sg/973fm/playlist/0");
            var client = new HttpClient();
            string finalId = "4010";

            string response = "";
            var currPlaying = "";

            if (appbarMode == 0)
                finalId = playingId;
            else if (appbarMode == 1)
                finalId = globalId;

            switch (finalId)
            {
                case "4010":
                    uri = new Uri("http://onlineradiobox.com/json/sg/973fm/playlist/0");
                    try
                    {
                        response = await client.GetStringAsync(uri);
                        RootObject playList = JsonConvert.DeserializeObject<RootObject>(response);
                        Playlist firstItem = playList.playlist.First();
                        currPlaying = firstItem.name;
                    }
                    catch (Exception)
                    {

                    }
                    break;
                case "4011":
                    uri = new Uri("https://nowplaying.audiospace.co/210/currentlyPlaying");
                    try
                    {
                        response = await client.GetStringAsync(uri);
                        dynamic data = JObject.Parse(response);
                        currPlaying = data.song + " - " + data.artist;
                    }
                    catch (Exception)
                    {

                    }
                    break;
                case "4012":
                    currPlaying = "Bible Witness Web Radio";
                    break;
                case "4013":
                    currPlaying = "Hitz.fm";
                    break;
                case "4014":
                    uri = new Uri("https://quasar.shoutca.st/external/rpc.php?callback=&m=recenttracks.get&username=muhammadizar&rid=muhammadizar&limit=1");
                    try
                    {
                        response = await client.GetStringAsync(uri);
                        dynamic data = JObject.Parse(response);
                        currPlaying = data.data[0][0].title + " - " + data.data[0][0].artist;
                    }
                    catch (Exception)
                    {

                    }
                    break;
                case "5009":
                    uri = new Uri("http://www.kiismedia.com/radio/index.php?c=Melody");
                    try
                    {
                        response = await client.GetStringAsync(uri);
                        dynamic data = JObject.Parse(response);
                        currPlaying = data.title + " - " + data.artist;
                    }
                    catch (Exception)
                    {

                    }
                    break;
                case "5010":
                    uri = new Uri("https://quasar.shoutca.st/external/rpc.php?callback=&m=recenttracks.get&username=muhammadizar&rid=muhammadizar&limit=1");
                    try
                    {
                        response = await client.GetStringAsync(uri);
                        dynamic data = JObject.Parse(response);
                        currPlaying = data.data[0][0].title + " - " + data.data[0][0].artist;
                    }
                    catch (Exception)
                    {

                    }
                    break;
                case "5011":
                    uri = new Uri("https://quasar.shoutca.st/external/rpc.php?callback=&m=recenttracks.get&username=muhammadizar&rid=muhammadizar&limit=1");
                    try
                    {
                        response = await client.GetStringAsync(uri);
                        dynamic data = JObject.Parse(response);
                        currPlaying = data.data[0][0].title + " - " + data.data[0][0].artist;
                    }
                    catch (Exception)
                    {

                    }
                    break;
                case "5012":
                    currPlaying = "Desi Dance";
                    break;
                default:
                    break;
            }

            if (appbarMode == 0)
            {
                SystemMediaTransportControlsDisplayUpdater updater = systemControls.DisplayUpdater;
                updater.MusicProperties.Title = currPlaying;
                updater.Update();
            }

            if (type == 0)
            {
                var messageDialog = new MessageDialog(currPlaying, "SG Radio - " + Constants.getName(finalId) + ": Now Playing");
                messageDialog.Commands.Add(new UICommand("Close", (command) =>
                {
                }));
                messageDialog.Commands.Add(new UICommand("Copy to Clipboard", (command) =>
                {
                    DataPackage package = new DataPackage();
                    package.SetText(currPlaying);
                    Clipboard.SetContent(package);
                }));
                messageDialog.Commands.Add(new UICommand("Star", (command) =>
                {
                    App.Insert_StarredItem((new StarredDataSQL()
                    {
                        StarredTitle = currPlaying,
                        StarredImage = Constants.getImage(finalId),
                        StarredTime = DateTime.Now.ToString()
                    }));
                    initializeStarredHub();
                }));
                messageDialog.DefaultCommandIndex = 0;
                if (asyncCommand != null)
                {
                    asyncCommand.Cancel();
                }

                asyncCommand = messageDialog.ShowAsync();
            }
            else if (type == 1)
            {
                currStation.Text = "Current Station: " + Constants.getName(finalId);
                currTrack.Text = "Current Track: " + currPlaying;
                ToolTipService.SetToolTip(currTrack, currPlaying);
                SetLiveTile();
            }
        }
        #endregion

        #region AppBar
        private void toggleAppBar(bool type)
        {
            pTSBtn.IsEnabled = type;
            currentSongBtn.IsEnabled = type;
            stopBtn.IsEnabled = type;
            lyricsBtn.IsEnabled = type;
            // refreshBtn.IsEnabled = type;
            stnWebsiteBtn.IsEnabled = type;
            stnProgrammingBtn.IsEnabled = type;

            favBtn.IsEnabled = type;
        }

        private async void pinToStart_Click(object sender, RoutedEventArgs e)
        {
            if (appBarSelected)
            {
                var itemId = globalId;
                var itemTitle = Constants.getName(itemId);
                var itemImg = Constants.getImage(itemId);
                var itemImage = new Uri(itemImg, UriKind.Absolute);

                var tile = new SecondaryTile(
                        itemId,
                        "SG Radio: " + itemTitle,
                        "SG Radio: " + itemTitle,
                        itemId,
                        TileOptions.ShowNameOnLogo | TileOptions.ShowNameOnWideLogo,
                        itemImage,
                        itemImage
                    );

                tile.VisualElements.ForegroundText = ForegroundText.Light;

                bool pinned = await tile.RequestCreateForSelectionAsync(GetElementRectangle(sender as FrameworkElement), Placement.Below);

            }
            else
            {
                var itemId = ((SGRADIODataItem)itemClick.ClickedItem).UniqueId;
                var itemTitle = ((SGRADIODataItem)itemClick.ClickedItem).Title;
                var itemImg = ((SGRADIODataItem)itemClick.ClickedItem).ImagePath;
                var itemImage = new Uri(itemImg, UriKind.Absolute);

                var tile = new SecondaryTile(
                        itemId,
                        "SG Radio: " + itemTitle,
                        "SG Radio: " + itemTitle,
                        itemId,
                        TileOptions.ShowNameOnLogo | TileOptions.ShowNameOnWideLogo,
                        itemImage,
                        itemImage
                    );

                tile.VisualElements.ForegroundText = ForegroundText.Dark;
                await tile.RequestCreateAsync();
            }
        }

        public static Rect GetElementRectangle(FrameworkElement element)
        {
            GeneralTransform buttonTransform = element.TransformToVisual(null);
            Point point = buttonTransform.TransformPoint(new Point());
            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        }

        private void currentSong_Click(object sender, RoutedEventArgs e)
        {
            if (appBarSelected)
            {
                efrTick(0, 1);
            }
            else
            {
                efrTick(0, 0);
            }
        }

        private void lyrics_Click(object sender, RoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("It was fun while it lasted, but our radio stations no longer provide lyrics data.", "SG Radio - Lyrics");
            messageDialog.Commands.Add(new UICommand("Close", (command) =>
            {
            }));
            messageDialog.DefaultCommandIndex = 0;
            if (asyncCommand != null)
            {
                asyncCommand.Cancel();
            }

            asyncCommand = messageDialog.ShowAsync();
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            if (appBarSelected)
            {
                efrTick(0, 1);
            }
            else
            {
                efrTick(0, 0);
            }
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            completeClosure();
        }

        private async void stnWebsite_Click(object sender, RoutedEventArgs e)
        {
            string gotoURL;
            if (appBarSelected)
            {
                gotoURL = Constants.getStnWebsite(globalId);
            }
            else
            {
                gotoURL = Constants.getStnWebsite(playingId);
            }

            if (gotoURL == "Unknown")
            {
                var messageDialog = new MessageDialog("Invalid Selection.", "Error");
                messageDialog.DefaultCommandIndex = 0;

                if (asyncCommand != null)
                {
                    asyncCommand.Cancel();
                }

                asyncCommand = messageDialog.ShowAsync();
            }
            else
                await Launcher.LaunchUriAsync(new Uri(gotoURL, UriKind.Absolute));
        }

        private async void stnProgramming_Click(object sender, RoutedEventArgs e)
        {
            string gotoURL;
            if (appBarSelected)
            {
                gotoURL = Constants.getStnProgramming(globalId);
            }
            else
            {
                gotoURL = Constants.getStnProgramming(playingId);
            }

            if (gotoURL == "Unknown")
            {
                var messageDialog = new MessageDialog("Invalid Selection.", "Error");
                messageDialog.DefaultCommandIndex = 0;

                if (asyncCommand != null)
                {
                    asyncCommand.Cancel();
                }

                asyncCommand = messageDialog.ShowAsync();
            }
            else
                await Launcher.LaunchUriAsync(new Uri(gotoURL, UriKind.Absolute));
        }

        private void fav_Click(object sender, RoutedEventArgs e)
        {
            if (appBarSelected)
            {
                if (globalId == "Unknown")
                {
                    var messageDialog = new MessageDialog("Invalid Selection.", "Error");
                    messageDialog.DefaultCommandIndex = 0;

                    if (asyncCommand != null)
                    {
                        asyncCommand.Cancel();
                    }

                    asyncCommand = messageDialog.ShowAsync();
                }
                else
                {
                    App.Insert_FavoriteItem((new FavDataSQL()
                    {
                        FavID = globalId,
                    }));

                }

                initializeFavoritesHub();
            }
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            if (appBarSelected)
            {
                if (selectedFav != "")
                {
                    App.Remove_StarredItem((new StarredDataSQL()
                    {
                        StarredTitle = selectedFav,
                        StarredTime = selectedTime,
                        StarredImage = selectedImage
                    }));

                    selectedFav = "";
                    initializeStarredHub();
                }
                else
                {
                    App.Remove_FavoriteItem((new FavDataSQL()
                    {
                        FavID = globalId
                    }));

                    initializeFavoritesHub();
                }
            }
        }

        private void Ellipsis_Tapped(object sender, TappedRoutedEventArgs e)
        {
            BottomAppBar.IsOpen = true;
        }

        void BottomAppBar_Closed(object sender, object e)
        {
            ellipsisMove2.Begin();

            GridView itemGridView = (GridView)FindChildControl<GridView>(ParentHub, "itemGridView");
            GridView itemGridView2 = (GridView)FindChildControl<GridView>(ParentHub, "itemGridView1");
            GridView itemGridView3 = (GridView)FindChildControl<GridView>(ParentHub, "itemGridView2");
            GridView itemGridView4 = (GridView)FindChildControl<GridView>(ParentHub, "itemGridView3");
            if (itemGridView != null)
            {
                itemGridView.SelectedItem = null;
                itemGridView.SelectedIndex = -1;
                itemGridView.SelectedValue = null;
            }
            if (itemGridView2 != null)
            {
                itemGridView2.SelectedItem = null;
                itemGridView2.SelectedIndex = -1;
                itemGridView2.SelectedValue = null;
            }
            if (itemGridView3 != null)
            {
                itemGridView3.SelectedItem = null;
                itemGridView3.SelectedValue = null;
                itemGridView3.SelectedIndex = -1;
            }
            if (itemGridView4 != null)
            {
                itemGridView4.SelectedItem = null;
                itemGridView4.SelectedIndex = -1;
                itemGridView4.SelectedValue = null;
            }
        }

        void BottomAppBar_Opened(object sender, object e)
        {
            ellipsisMove.Begin();
        }
        #endregion

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            sliderVolume.Value = 100;

            this.Loaded += PageLoaded;
            this.Unloaded += PageUnloaded;

            this.RegisterBackgroundTask();
        }

        private async void RegisterBackgroundTask()
        {
            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            if (backgroundAccessStatus == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity ||
                backgroundAccessStatus == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == "SGRadioBackgroundTask")
                    {
                        task.Value.Unregister(true);
                    }
                }

                BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
                taskBuilder.Name = "SGRadioBackgroundTask";
                taskBuilder.TaskEntryPoint = "BackgroundTasks.SGRadioBackgroundTask";
                taskBuilder.SetTrigger(new TimeTrigger(720, false));
                var registration = taskBuilder.Register();
            }
        }

        private void PageUnloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Window_SizeChanged;
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged += Window_SizeChanged;
        }

        private void Window_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (e.Size.Width < e.Size.Height)
            {
                searchGrid.Visibility = Visibility.Collapsed;

                if (e.Size.Width < 540 && e.Size.Height >= 768)
                {
                    ParentHub.HorizontalAlignment = HorizontalAlignment.Center;
                    switchVertical();
                }
                if (e.Size.Width < 600)
                {
                    statusGrid.HorizontalAlignment = HorizontalAlignment.Center;
                    pageTitle.Visibility = Visibility.Collapsed;
                }
                else
                {
                    statusGrid.HorizontalAlignment = HorizontalAlignment.Right;
                    Thickness margin = statusGrid.Margin;
                    margin.Right = 50;
                    margin.Top = 52;
                    statusGrid.Margin = margin;
                    ParentHub.HorizontalAlignment = HorizontalAlignment.Left;
                    pageTitle.Visibility = Visibility.Visible;
                    switchHorizontal();
                }
            }
            else
            {
                searchGrid.Visibility = Visibility.Visible;
                statusGrid.HorizontalAlignment = HorizontalAlignment.Center;
                Thickness margin = statusGrid.Margin;
                margin.Top = 52;
                statusGrid.Margin = margin;
                ParentHub.HorizontalAlignment = HorizontalAlignment.Left;
                pageTitle.Visibility = Visibility.Visible;
                switchHorizontal();
            }
        }

        private void switchVertical()
        {
            ParentHub.Orientation = Orientation.Vertical;
            FavHub.Background.Opacity = 0;
            StarredHub.Background.Opacity = 0;
            Thickness hubMargin = ParentHub.Margin;
            hubMargin.Top = 50;
            ParentHub.Margin = hubMargin;
        }

        private void switchHorizontal()
        {
            ParentHub.Orientation = Orientation.Horizontal;
            FavHub.Background.Opacity = 1;
            StarredHub.Background.Opacity = 1;
            Thickness hubMargin = ParentHub.Margin;
            hubMargin.Top = 0;
            ParentHub.Margin = hubMargin;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        #region Selection Changed
        private void SelectionChangedFavourites(object sender, SelectionChangedEventArgs e)
        {
            GridView itemGridView = (GridView)FindChildControl<GridView>(ParentHub, "itemGridView");
            GridView itemGridView2 = (GridView)FindChildControl<GridView>(ParentHub, "itemGridView1");
            GridView itemGridView3 = (GridView)FindChildControl<GridView>(ParentHub, "itemGridView2");
            GridView itemGridView4 = (GridView)FindChildControl<GridView>(ParentHub, "itemGridView3");
            if (itemGridView2 != null)
            {
                itemGridView2.SelectedItem = null;
                itemGridView2.SelectedIndex = -1;
                itemGridView2.SelectedValue = null;
            }
            if (itemGridView3 != null)
            {
                itemGridView3.SelectedItem = null;
                itemGridView3.SelectedValue = null;
                itemGridView3.SelectedIndex = -1;
            }
            if (itemGridView4 != null)
            {
                itemGridView4.SelectedItem = null;
                itemGridView4.SelectedIndex = -1;
                itemGridView4.SelectedValue = null;
            }

            toggleAppBar(true);
            BottomAppBar.IsOpen = true;
            if (e.RemovedItems.Count == 0 || (e.RemovedItems.Count == 1 && e.AddedItems.Count == 1))
            {
                appBarSelected = true;
                var itemName = ((FavData)e.AddedItems[0]).FavTitle;
                var itemId = Constants.getIdFromName(itemName);
                globalId = itemId;
                removeBtn.IsEnabled = true;
            }
            else
            {
                appBarSelected = false;
                removeBtn.IsEnabled = false;
                if (mediaElement1.CurrentState == MediaElementState.Closed)
                    toggleAppBar(false);
            }
        }

        private void SelectionChangedEnglish(object sender, SelectionChangedEventArgs e)
        {
            GridView itemGridView = (GridView)FindChildControl<GridView>(ParentHub, "itemGridView");
            GridView itemGridView2 = (GridView)FindChildControl<GridView>(ParentHub, "itemGridView1");
            GridView itemGridView3 = (GridView)FindChildControl<GridView>(ParentHub, "itemGridView2");
            GridView itemGridView4 = (GridView)FindChildControl<GridView>(ParentHub, "itemGridView3");
            if (itemGridView != null)
            {
                itemGridView.SelectedItem = null;
                itemGridView.SelectedIndex = -1;
                itemGridView.SelectedValue = null;
            }
            if (itemGridView3 != null)
            {
                itemGridView3.SelectedItem = null;
                itemGridView3.SelectedValue = null;
                itemGridView3.SelectedIndex = -1;
            }
            if (itemGridView4 != null)
            {
                itemGridView4.SelectedItem = null;
                itemGridView4.SelectedIndex = -1;
                itemGridView4.SelectedValue = null;
            }

            toggleAppBar(true);
            if (e.RemovedItems.Count == 0 || (e.RemovedItems.Count == 1 && e.AddedItems.Count == 1))
            {
                appBarSelected = true;
                BottomAppBar.IsOpen = true;
                globalId = ((SGRADIODataItem)e.AddedItems[0]).UniqueId;
            }
            else
            {
                appBarSelected = false;
                BottomAppBar.IsOpen = false;
                if (mediaElement1.CurrentState == MediaElementState.Closed)
                    toggleAppBar(false);
            }
        }
        private DependencyObject FindChildControl<T>(DependencyObject control, string ctrlName)
        {
            int childNumber = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childNumber; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(control, i);
                FrameworkElement fe = child as FrameworkElement;
                if (fe == null) return null;

                if (child is T && fe.Name == ctrlName)
                {
                    return child;
                }
                else
                {
                    DependencyObject nextLevel = FindChildControl<T>(child, ctrlName);
                    if (nextLevel != null)
                        return nextLevel;
                }
            }
            return null;
        }
        private void SelectionChangedOthers(object sender, SelectionChangedEventArgs e)
        {
            GridView itemGridView = (GridView)FindChildControl<GridView>(ParentHub, "itemGridView");
            GridView itemGridView2 = (GridView)FindChildControl<GridView>(ParentHub, "itemGridView1");
            GridView itemGridView3 = (GridView)FindChildControl<GridView>(ParentHub, "itemGridView2");
            GridView itemGridView4 = (GridView)FindChildControl<GridView>(ParentHub, "itemGridView3");
            if (itemGridView != null)
            {
                itemGridView.SelectedItem = null;
                itemGridView.SelectedIndex = -1;
                itemGridView.SelectedValue = null;
            }
            if (itemGridView2 != null)
            {
                itemGridView2.SelectedItem = null;
                itemGridView2.SelectedValue = null;
                itemGridView2.SelectedIndex = -1;
            }
            if (itemGridView4 != null)
            {
                itemGridView4.SelectedItem = null;
                itemGridView4.SelectedIndex = -1;
                itemGridView4.SelectedValue = null;
            }

            toggleAppBar(true);
            if (e.RemovedItems.Count == 0 || (e.RemovedItems.Count == 1 && e.AddedItems.Count == 1))
            {
                appBarSelected = true;
                BottomAppBar.IsOpen = true;
                globalId = ((SGRADIODataItem)e.AddedItems[0]).UniqueId;
            }
            else
            {
                appBarSelected = false;
                BottomAppBar.IsOpen = false;
                if (mediaElement1.CurrentState == MediaElementState.Closed)
                    toggleAppBar(false);
            }
        }

        private void SelectionChangedStarred(object sender, SelectionChangedEventArgs e)
        {
            GridView itemGridView = (GridView)FindChildControl<GridView>(ParentHub, "itemGridView");
            GridView itemGridView2 = (GridView)FindChildControl<GridView>(ParentHub, "itemGridView1");
            GridView itemGridView3 = (GridView)FindChildControl<GridView>(ParentHub, "itemGridView2");
            GridView itemGridView4 = (GridView)FindChildControl<GridView>(ParentHub, "itemGridView3");
            if (itemGridView != null)
            {
                itemGridView.SelectedItem = null;
                itemGridView.SelectedIndex = -1;
                itemGridView.SelectedValue = null;
            }
            if (itemGridView3 != null)
            {
                itemGridView3.SelectedItem = null;
                itemGridView3.SelectedValue = null;
                itemGridView3.SelectedIndex = -1;
            }
            if (itemGridView2 != null)
            {
                itemGridView2.SelectedItem = null;
                itemGridView2.SelectedIndex = -1;
                itemGridView2.SelectedValue = null;
            }

            toggleAppBar(true);
            if (e.RemovedItems.Count == 0 || (e.RemovedItems.Count == 1 && e.AddedItems.Count == 1))
            {
                appBarSelected = true;
                BottomAppBar.IsOpen = true;
                removeBtn.IsEnabled = true;
                selectedFav = ((StarredData)e.AddedItems[0]).StarredTitle;
                selectedTime = ((StarredData)e.AddedItems[0]).StarredTime;
                selectedImage = ((StarredData)e.AddedItems[0]).StarredImage;
            }
            else
            {
                appBarSelected = false;
                BottomAppBar.IsOpen = false;
                removeBtn.IsEnabled = false;
                if (mediaElement1.CurrentState == MediaElementState.Closed)
                    toggleAppBar(false);
            }
        }
        #endregion

        #region Shutdown Timer
        private void timedShutdown1(object sender, RoutedEventArgs e)
        {
            countdown = 5;
            timerCountdown.Text = "Stopping Streams in: " + countdown + " min(s)";
            startShutdown();
        }

        private void startShutdown()
        {
            timerShutdown.Interval = TimeSpan.FromMinutes(1);
            timerShutdown.Tick += shutdown_Tick;
            timerShutdown.Start();
            timerCountdown.Visibility = Visibility.Visible;
        }

        private void shutdown_Tick(object sender, object e)
        {
            countdown--;
            timerCountdown.Text = "Stopping Streams in: " + countdown + " min(s)";

            if (countdown == 0)
            {
                timerShutdown.Stop();
                completeClosure();
                timerCountdown.Visibility = Visibility.Collapsed;
            }
        }

        private void timedShutdown2(object sender, RoutedEventArgs e)
        {
            countdown = 15;
            timerCountdown.Text = "Stopping Streams in: " + countdown + " min(s)";
            startShutdown();
        }

        private void timedShutdown3(object sender, RoutedEventArgs e)
        {
            countdown = 30;
            timerCountdown.Text = "Stopping Streams in: " + countdown + " min(s)";
            startShutdown();
        }

        private void timedShutdown4(object sender, RoutedEventArgs e)
        {
            countdown = 60;
            timerCountdown.Text = "Stopping Streams in: " + countdown + " min(s)";
            startShutdown();
        }

        private void timedShutdown5(object sender, RoutedEventArgs e)
        {
            countdown = 120;
            timerCountdown.Text = "Stopping Streams in: " + countdown + " min(s)";
            startShutdown();
        }

        private void timedShutdown6(object sender, RoutedEventArgs e)
        {
            timerShutdown.Stop();
            timerCountdown.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region Search
        private void SearchBoxEventsSuggestionsRequested(object sender, SearchBoxSuggestionsRequestedEventArgs e)
        {
            string queryText = e.QueryText;
            Windows.ApplicationModel.Search.SearchSuggestionCollection suggestionCollection = e.Request.SearchSuggestionCollection;
            foreach (string suggestion in suggestionList)
            {
                if (suggestion.StartsWith(queryText, StringComparison.CurrentCultureIgnoreCase))
                {
                    suggestionCollection.AppendQuerySuggestion(suggestion);
                }
            }
        }

        private static readonly string[] suggestionList =
            {
                "BBC World Service", "Money FM 89.3", "GOLD 90.5FM", "One FM 91.3", "Kiss 92FM", "Symphony 92.4FM", "938Now", "Class 95FM", "Power 98FM", "987FM",
                "973FM", "Asia Expat Radio", "Bible Witness Web Radio", "Hitz.fm", "Orion Station", "88.3Jia FM", "Y.E.S. 93.3FM", "Capital 95.8FM", "96.3 Hao FM", "Love 97.2FM",
                "UFM 1003", "Ria 89.7FM", "Warna 94.2FM", "Oli 96.8FM", "Radio Melody", "Naga FM", "DesiNetworks", "Desi Dance"
            };

        private void searchBoxSubmit(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            bool foundSearch = false;
            for (int i = 0; i < suggestionList.Length; i++)
            {
                if (args.QueryText == suggestionList[i])
                {
                    foundSearch = true;
                }
            }
            if (foundSearch)
            {
                startArgument = Constants.getIdFromName(args.QueryText);
                startMusic();
            }
            else
            {
                searchBox1.QueryText = "I didn't find anything :(";
            }
        }
        #endregion

        #region Json Parser
        public class Song
        {
            [JsonProperty("track")]
            public string Track { get; set; }

            [JsonProperty("artist")]
            public string Artist { get; set; }
        }

        public class PlayListItem
        {
            [JsonProperty("song")]
            public Song Song { get; set; }
        }

        public class Playlist
        {
            public string name { get; set; }
            public int created { get; set; }
            public string id { get; set; }
        }

        public class RootObject
        {
            public List<Playlist> playlist { get; set; }
        }
        #endregion
    }
}
