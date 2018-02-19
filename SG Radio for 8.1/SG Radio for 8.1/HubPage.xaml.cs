/*
 * 	  {
        "UniqueId": "1009",
        "Title": "The Live Radio",
        "ImagePath": "ms-appx:///Images/English/portal_10_600_C.png",
        "Content" : "http://s3.viastreaming.net:8530/"
      },
 */

using SG_Radio_for_8._1.Common;
using SG_Radio_for_8._1.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Data.Json;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Reflection;
using Windows.UI.Notifications;
using NotificationsExtensions.TileContent;
using Windows.ApplicationModel.Background;
using SM.Media.Playlists;
using SM.Media.Web;
using SM.Media;
using SM.Media.Segments;
using System.Net.Http.Headers;

namespace SG_Radio_for_8._1
{
    public sealed partial class HubPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        string playingId;
        string globalId;
        string holdThisSource;
        string playingImg;
        string selectedFav, selectedTime, selectedImage;
        public static string startArgument;
        private bool appBarSelected = false;
        private int countdown;
        private DispatcherTimer anitimer;
        ItemClickEventArgs itemClick;
        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer timerStandby = new DispatcherTimer();
        DispatcherTimer timerShutdown = new DispatcherTimer();
        IAsyncOperation<IUICommand> asyncCommand;
        SystemMediaTransportControls systemControls;
        readonly IHttpClients _httpClients;
        WinRtMediaStreamSource _tsMediaStreamSource;
        PlaylistSegmentManager _playlist;
        IMediaElementManager _mediaElementManager;
        ITsMediaManager _tsMediaManager;

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
            _httpClients = new HttpClients(userAgent: new ProductInfoHeaderValue("Unknown", "0.0"));

            this.Loaded += HubPage_Loaded;
        }

        void HubPage_Loaded(object sender, RoutedEventArgs e)
        {
            anitimer = new DispatcherTimer();
            anitimer.Tick += anitimer_Tick;
            anitimer.Interval = TimeSpan.FromSeconds(2);
            anitimer.Start();
        }

        void anitimer_Tick(object sender, object e)
        {
            ellipsisMove.Begin();
            EllipsisGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
            ellipsisMove2.Begin();
            anitimer.Stop();
        }

        void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            string input;
            SystemMediaTransportControls.GetForCurrentView().DisplayUpdater.Type = MediaPlaybackType.Music;
            if (SystemMediaTransportControls.GetForCurrentView().DisplayUpdater.MusicProperties.Title != "")
            {
                input = @"I'm listening to " + SystemMediaTransportControls.GetForCurrentView().DisplayUpdater.MusicProperties.Title + " on " + SystemMediaTransportControls.GetForCurrentView().DisplayUpdater.MusicProperties.Artist + " using SG Radio! <br>http://apps.microsoft.com/webpdp/app/sg-radio/81dd2ed1-8b0a-4017-a6d8-cb7b28837c2a";
            }
            else
            {
                input = @"I'm listening to Singapore's Radio Stations using SG Radio! <br>http://apps.microsoft.com/webpdp/app/sg-radio/81dd2ed1-8b0a-4017-a6d8-cb7b28837c2a";
            }

            DataRequest request = args.Request;

            request.Data.Properties.Title = "SG Radio for Windows 8";
            request.Data.Properties.Description = string.Format(input);

            var formatted = Windows.ApplicationModel.DataTransfer.HtmlFormatHelper.CreateHtmlFormat(input);
            request.Data.SetHtmlFormat(formatted);
        }

        private void checkForFirstRun()
        {
            if (!ApplicationData.Current.RoamingSettings.Values.ContainsKey("FirstRun81R6"))
            {
                var messageDialog = new MessageDialog("Welcome to SG Radio!\n\nChangelog:\n- FIXED: Pesky stations. Fixed Kiss 92 stream migration", "SG Radio Update: v3.4.0.5");
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

                ApplicationData.Current.RoamingSettings.Values["FirstRun81R6"] = true;
            }
        }

        private async void sendEmail()
        {
            await Launcher.LaunchUriAsync(new Uri("mailto://roc@reignofcomputer.com?subject=SG Radio for Windows 8.1", UriKind.Absolute));
        }

        private async void rateApp()
        {
            var storeURI = new Uri("ms-windows-store:PDP?PFN=20694ReignOfComputer.SGRadio_whkbyfjgnjkag");
            await Windows.System.Launcher.LaunchUriAsync(storeURI);
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
                    TileUpdater updater = Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile(TileName);
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
                var tileNotification = new Windows.UI.Notifications.TileNotification(tileXml);
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            }
            catch (Exception)
            {
            }
        }

        async void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            mediaElement1.Stop();

            if (null != _tsMediaManager)
                _tsMediaManager.Close();

            if (null != _playlist)
            {
                var t = _playlist.StopAsync();
            }

            if (null != _mediaElementManager)
                await _mediaElementManager.CloseAsync();

            itemClick = e;
            progressring.IsActive = true;
            var itemId = ((SGRADIODataItem)e.ClickedItem).UniqueId;
            var itemName = ((SGRADIODataItem)e.ClickedItem).Title;
            var itemImage = ((SGRADIODataItem)e.ClickedItem).ImagePath;
            var itemContent = ((SGRADIODataItem)e.ClickedItem).Content;
            if (itemId == "1011")
            {
                playSafra(0);
                return;
            }
            if (itemId == "2008")
            {
                playSafra(1);
            }

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
            efrTick(0);
            efrTicker();
            mediaElement1.Play();
            systemControls.PlaybackStatus = MediaPlaybackStatus.Playing;
            toggleAppBar(true);
        }

        private async void playSafra(int station)
        {
            string stationName = "";
            string stationPlaylist = "";

            if (station == 0)
            {
                stationName = "Power 98 FM";
                stationPlaylist = "power98";
                globalId = "1011";
                playingId = "1011";
                playingImg = "ms-appx:///Images/English/portal_12_600_C.png";
                holdThisSource = "safra1";
            }
            else
            {
                stationName = "88.3 Jia FM";
                stationPlaylist = "883jia";
                globalId = "2008";
                playingId = "2008";
                playingImg = "ms-appx:///Images/English/portal_9_600_C.png";
                holdThisSource = "safra2";
            }

            SystemMediaTransportControlsDisplayUpdater updater = systemControls.DisplayUpdater;
            updater.Type = MediaPlaybackType.Music;
            updater.MusicProperties.Artist = "SG Radio - " + stationName;
            updater.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(playingImg, UriKind.Absolute));
            updater.Update();
            efrTick(0);
            efrTicker();
            systemControls.PlaybackStatus = MediaPlaybackStatus.Playing;
            toggleAppBar(true);

            if (null != _playlist)
            {
                _playlist.Dispose();
                _playlist = null;
            }

            if (null != _tsMediaStreamSource)
            {
                _tsMediaStreamSource.Dispose();
                _tsMediaStreamSource = null;
            }

            var segmentsFactory = new SegmentsFactory(_httpClients);

            var programManager = new ProgramManager(_httpClients, segmentsFactory.CreateStreamSegments)
            {
                Playlists = new[]
                                                 {
                                                     new Uri("http://ms1.clickhere2.com/live/" + stationPlaylist + "/playlist.m3u8")
                                                 }
            };

            SM.Media.Playlists.Program program;
            ISubProgram subProgram;

            try
            {
                var programs = await programManager.LoadAsync();

                program = programs.Values.FirstOrDefault();

                if (null == program)
                {
                    return;
                }

                subProgram = program.SubPrograms.OrderByDescending(sp => sp.Bandwidth).FirstOrDefault();

                if (null == subProgram)
                {
                    return;
                }
            }
            catch (Exception)
            {
                return;
            }

            var programClient = _httpClients.CreatePlaylistClient(program.Url);

            _playlist = new PlaylistSegmentManager(uri => new CachedWebRequest(uri, programClient), subProgram, segmentsFactory.CreateStreamSegments);

            _mediaElementManager = new MediaElementManager(Dispatcher,
                () =>
                {
                    var me = mediaElement1;

                    return me;
                },
                me =>
                {
                });

            var segmentReaderManager = new SegmentReaderManager(new[] { _playlist }, _httpClients.CreateSegmentClient);

            _tsMediaStreamSource = new WinRtMediaStreamSource();

            _tsMediaManager = new TsMediaManager(segmentReaderManager, _mediaElementManager, _tsMediaStreamSource);

            _tsMediaManager.Play();

            progressring.IsActive = false;
            statusGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private async void ItemView_ItemClickFav(object sender, ItemClickEventArgs e)
        {
            mediaElement1.Stop();

            if (null != _tsMediaManager)
                _tsMediaManager.Close();

            if (null != _playlist)
            {
                var t = _playlist.StopAsync();
            }

            if (null != _mediaElementManager)
                await _mediaElementManager.CloseAsync();

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

            if (itemId == "1011")
            {
                playSafra(0);
                return;
            }
            if (itemId == "2008")
            {
                playSafra(1);
            }

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
            efrTick(0);
            efrTicker();
            mediaElement1.Play();
            systemControls.PlaybackStatus = MediaPlaybackStatus.Playing;
            toggleAppBar(true);
        }

        private async void startMusic()
        {
            if (startArgument == "")
            {
                return;
            }

            mediaElement1.Stop();

            if (null != _tsMediaManager)
                _tsMediaManager.Close();

            if (null != _playlist)
            {
                var t = _playlist.StopAsync();
            }

            if (null != _mediaElementManager)
                await _mediaElementManager.CloseAsync();

            progressring.IsActive = true;
            var itemName = Constants.getName(startArgument);
            var itemId = startArgument;
            var itemImage = Constants.getImage(itemId);
            var itemContent = Constants.getStream(itemId);

            globalId = itemId;
            playingId = itemId;
            playingImg = itemImage;

            if (itemId == "1011")
            {
                playSafra(0);
                return;
            }
            if (itemId == "2008")
            {
                playSafra(1);
            }

            mediaElement1.Source = new Uri(itemContent, UriKind.Absolute);
            holdThisSource = @Constants.getStream(startArgument);
            SystemMediaTransportControlsDisplayUpdater updater = systemControls.DisplayUpdater;
            updater.Type = MediaPlaybackType.Music;
            updater.MusicProperties.Artist = "SG Radio - " + itemName;
            updater.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(itemImage, UriKind.Absolute));
            updater.Update();
            efrTick(0);
            efrTicker();
            mediaElement1.Play();
            systemControls.PlaybackStatus = MediaPlaybackStatus.Playing;
            toggleAppBar(true);
        }

        #region MediaControl
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
            statusGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
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

        private async void completeClosure()
        {
            if (null != _tsMediaManager)
                _tsMediaManager.Close();

            if (null != _playlist)
            {
                var t = _playlist.StopAsync();
            }

            if (null != _mediaElementManager)
                await _mediaElementManager.CloseAsync();

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                mediaElement1.Stop();
                mediaElement1.Source = null;
                progressring.IsActive = false;
                statusGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                toggleAppBar(false);
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                timer.Stop();

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

                systemControls.PlaybackStatus = MediaPlaybackStatus.Stopped;
            });
        }

        private async void restartSource()
        {
            if (holdThisSource == "safra1")
            {
                playSafra(0);
                return;
            }
            else if (holdThisSource == "safra2")
            {
                playSafra(1);
                return;
            }

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
        #endregion

        #region TitleFetch
        private void efrFetch(int appbarMode)
        {
            if (appbarMode == 0)
            {
                if (playingId == "1001" || playingId == "1002" || playingId == "2007")
                {
                    sphTitle(0, 0);
                }

                else if (playingId == "1008")
                {
                    fm973Title(0, 0);
                }

                else if (playingId == "1009")
                {
                    tlrTitle(0, 0);
                }

                else if (playingId == "1010")
                {
                    bbcTitle(0, 0);
                }

                else if (playingId == "1011")
                {
                    p98Title(0, 0);
                }

                else if (playingId == "2008")
                {
                    jiaTitle(0, 0);
                }

                else
                {
                    mediacorpTitle(0, 0);
                }
            }
            else if (appbarMode == 1)
            {
                if (globalId == "1001" || globalId == "1002" || globalId == "2007")
                {
                    sphTitle(0, 1);
                }

                else if (globalId == "1008")
                {
                    fm973Title(0, 1);
                }

                else if (globalId == "1009")
                {
                    tlrTitle(0, 1);
                }

                else if (globalId == "1010")
                {
                    bbcTitle(0, 1);
                }

                else if (globalId == "1011")
                {
                    p98Title(0, 0);
                }

                else if (globalId == "2008")
                {
                    jiaTitle(0, 0);
                }

                else
                {
                    mediacorpTitle(0, 1);
                }
            }
        }

        private void efrTick(int appbarMode)
        {
            if (appbarMode == 0)
            {
                if (playingId == "1001" || playingId == "1002" || playingId == "2007")
                {
                    sphTitle(1, 0);
                }

                else if (playingId == "1008")
                {
                    fm973Title(1, 0);
                }

                else if (playingId == "1009")
                {
                    tlrTitle(1, 0);
                }

                else if (playingId == "1010")
                {
                    bbcTitle(1, 0);
                }

                else if (playingId == "1011")
                {
                    p98Title(1, 0);
                }

                else if (playingId == "2008")
                {
                    jiaTitle(1, 0);
                }

                else
                {
                    mediacorpTitle(1, 0);
                }
            }
            else if (appbarMode == 1)
            {
                if (globalId == "1001" || globalId == "1002" || globalId == "2007")
                {
                    sphTitle(1, 1);
                }

                else if (globalId == "1008")
                {
                    fm973Title(1, 1);
                }

                else if (globalId == "1009")
                {
                    tlrTitle(1, 1);
                }

                else if (globalId == "1010")
                {
                    bbcTitle(1, 1);
                }

                else if (globalId == "1011")
                {
                    p98Title(1, 1);
                }

                else if (globalId == "2008")
                {
                    jiaTitle(1, 1);
                }

                else
                {
                    mediacorpTitle(1, 1);
                }
            }
        }

        void efrTicker()
        {
            timer.Tick += (s, o) =>
            {
                efrTick(0);
            };
            timer.Interval = new TimeSpan(0, 0, 30);
            bool enabled = timer.IsEnabled;
            timer.Start();
        }

        private async void p98Title(int type, int appbarMode)
        {
            var uri = new Uri("http://www.power98.com.sg/songfeed/nowplaying.php");
            var client = new HttpClient();
            string response = "";

            try
            {
                response = await client.GetStringAsync(uri);
            }
            catch (Exception)
            {
                return;
            }

            JsonObject parser = JsonObject.Parse(response);
            var rootArtist = parser.GetNamedString("artist");
            var rootTrack = parser.GetNamedString("song");

            if (appbarMode == 0)
            {
                SystemMediaTransportControlsDisplayUpdater updater = systemControls.DisplayUpdater;
                updater.MusicProperties.Title = rootArtist + " - " + rootTrack;
                updater.Update();
            }

            if (type == 0)
            {
                var messageDialog = new MessageDialog(rootArtist + " - " + rootTrack, "SG Radio - Power 98 FM: Now Playing");
                messageDialog.Commands.Add(new UICommand("Close", (command) =>
                {
                }));
                messageDialog.Commands.Add(new UICommand("Copy to Clipboard", (command) =>
                {
                    DataPackage package = new DataPackage();
                    package.SetText(rootArtist + " - " + rootTrack);
                    Clipboard.SetContent(package);
                }));
                messageDialog.Commands.Add(new UICommand("Star", (command) =>
                {
                    App.Insert_StarredItem((new StarredDataSQL()
                    {
                        StarredTitle = rootArtist + " - " + rootTrack,
                        StarredImage = Constants.getImage("1011"),
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
                currStation.Text = "Current Station: Power 98 FM";
                currTrack.Text = "Current Track: " + rootArtist + " - " + rootTrack;
                ToolTipService.SetToolTip(currTrack, rootArtist + " - " + rootTrack);
                SetLiveTile();
            }
        }

        private async void jiaTitle(int type, int appbarMode)
        {
            var uri = new Uri("http://www.883jia.com.sg/songfeed/nowplaying.php");
            var client = new HttpClient();
            string response = "";

            try
            {
                response = await client.GetStringAsync(uri);
            }
            catch (Exception)
            {
                return;
            }

            JsonObject parser = JsonObject.Parse(response);
            var rootArtist = parser.GetNamedString("artist");
            var rootTrack = parser.GetNamedString("song");
            var finalTrack = WebUtility.HtmlDecode(rootArtist + " - " + rootTrack);

            if (appbarMode == 0)
            {
                SystemMediaTransportControlsDisplayUpdater updater = systemControls.DisplayUpdater;
                updater.MusicProperties.Title = finalTrack;
                updater.Update();
            }

            if (type == 0)
            {
                var messageDialog = new MessageDialog(finalTrack, "SG Radio - 88.3 Jia FM: Now Playing");
                messageDialog.Commands.Add(new UICommand("Close", (command) =>
                {
                }));
                messageDialog.Commands.Add(new UICommand("Copy to Clipboard", (command) =>
                {
                    DataPackage package = new DataPackage();
                    package.SetText(finalTrack);
                    Clipboard.SetContent(package);
                }));
                messageDialog.Commands.Add(new UICommand("Star", (command) =>
                {
                    App.Insert_StarredItem((new StarredDataSQL()
                    {
                        StarredTitle = finalTrack,
                        StarredImage = Constants.getImage("2008"),
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
                currStation.Text = "Current Station: 88.3 Jia FM";
                currTrack.Text = "Current Track: " + finalTrack;
                ToolTipService.SetToolTip(currTrack, finalTrack);
                SetLiveTile();
            }
        }

        private async void bbcTitle(int type, int appbarMode)
        {
            var uri = new Uri("http://polling.bbc.co.uk/modules/onairpanel/include/bbc_world_service.jsonp");
            var client = new HttpClient();
            string response = "";

            try
            {
                response = await client.GetStringAsync(uri);
            }
            catch (Exception)
            {
                return;
            }

            var titleStart = response.IndexOf("On Now : ");
            titleStart = titleStart + 9;

            var testString = response.Substring(titleStart, response.Length - titleStart);

            var closureStart = testString.IndexOf("\\\">") + 3;
            var closureEnd = testString.IndexOf("<\\/a>");

            var titleFinish = testString.Substring(closureStart, closureEnd - closureStart);

            if (appbarMode == 0)
            {
                SystemMediaTransportControlsDisplayUpdater updater = systemControls.DisplayUpdater;
                updater.MusicProperties.Title = titleFinish;
                updater.Update();
            }

            if (type == 0)
            {
                var messageDialog = new MessageDialog(titleFinish, "SG Radio - BBC World Service: Now Playing");
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
                        StarredImage = Constants.getImage("1010"),
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

        private async void tlrTitle(int type, int appbarMode)
        {
            var uri = new Uri("http://theliveradio.sg/rcs/NowPlaying.txt?windows8cache=" + DateTime.Now);
            var client = new HttpClient();

            string response = "";

            try
            {
                response = await client.GetStringAsync(uri);
                if (response == "")
                    response = "Non-stop Hit Music";
            }
            catch (Exception)
            {
                return;
            }

            if (appbarMode == 0)
            {
                SystemMediaTransportControlsDisplayUpdater updater = systemControls.DisplayUpdater;
                updater.MusicProperties.Title = response;
                updater.Update();
            }

            if (type == 0)
            {
                var messageDialog = new MessageDialog(response, "SG Radio - The Live Radio: Now Playing");
                messageDialog.Commands.Add(new UICommand("Close", (command) =>
                {
                }));
                messageDialog.Commands.Add(new UICommand("Copy to Clipboard", (command) =>
                {
                    DataPackage package = new DataPackage();
                    package.SetText(response);
                    Clipboard.SetContent(package);
                }));
                messageDialog.Commands.Add(new UICommand("Star", (command) =>
                {
                    App.Insert_StarredItem((new StarredDataSQL()
                    {
                        StarredTitle = response,
                        StarredImage = Constants.getImage("1009"),
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
                currStation.Text = "Current Station: The Live Radio";
                currTrack.Text = "Current Track: " + response;
                ToolTipService.SetToolTip(currTrack, response);
                SetLiveTile();
            }
        }

        private async void sphTitle(int type, int appbarMode)
        {
            var uri = new Uri("http://meta.radioactive.sg/index.php?format=json&m=sph-kiss92");
            var client = new HttpClient();
            string finalId = "1000";

            if (appbarMode == 0)
            {
                switch (playingId)
                {
                    case "1001":
                        uri = new Uri("http://meta.radioactive.sg/index.php?format=json&m=913fm");
                        break;
                    case "1002":
                        uri = new Uri("http://meta.radioactive.sg/index.php?format=json&m=sph-kiss92");
                        break;
                    case "2007":
                        uri = new Uri("http://meta.radioactive.sg/index.php?format=json&m=1003fm");
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
                    case "1001":
                        uri = new Uri("http://meta.radioactive.sg/index.php?format=json&m=913fm");
                        break;
                    case "1002":
                        uri = new Uri("http://meta.radioactive.sg/index.php?format=json&m=sph-kiss92");
                        break;
                    case "2007":
                        uri = new Uri("http://meta.radioactive.sg/index.php?format=json&m=1003fm");
                        break;
                    default:
                        break;
                }
                finalId = globalId;
            }

            string response = "";

            try
            {
                response = await client.GetStringAsync(uri);
            }
            catch (Exception)
            {
                return;
            }

            JsonObject parser = JsonObject.Parse(response);
            var rootArtist = parser.GetNamedString("artist");
            var rootTrack = parser.GetNamedString("track");

            if (appbarMode == 0)
            {
                SystemMediaTransportControlsDisplayUpdater updater = systemControls.DisplayUpdater;
                updater.MusicProperties.Title = rootArtist + " - " + rootTrack;
                updater.Update();
            }

            if (type == 0)
            {
                var messageDialog = new MessageDialog(rootArtist + " - " + rootTrack, "SG Radio - " + Constants.getName(finalId) + ": Now Playing");
                messageDialog.Commands.Add(new UICommand("Close", (command) =>
                {
                }));
                messageDialog.Commands.Add(new UICommand("Copy to Clipboard", (command) =>
                {
                    DataPackage package = new DataPackage();
                    package.SetText(rootArtist + " - " + rootTrack);
                    Clipboard.SetContent(package);
                }));
                messageDialog.Commands.Add(new UICommand("Star", (command) =>
                {
                    App.Insert_StarredItem((new StarredDataSQL()
                    {
                        StarredTitle = rootArtist + " - " + rootTrack,
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
                currTrack.Text = "Current Track: " + rootArtist + " - " + rootTrack;
                ToolTipService.SetToolTip(currTrack, rootArtist + " - " + rootTrack);
                SetLiveTile();
            }
        }

        private async void fm973Title(int type, int appbarMode)
        {
            var fm973uri = "http://cast5.serverhostingcenter.com/js.php/radio973/streaminfo/rnd0";
            HttpClient client = new HttpClient();
            string finalTrack = String.Empty;

            try
            {
                var fm973get = new HttpRequestMessage(HttpMethod.Get, fm973uri);
                HttpResponseMessage data = await client.SendAsync(fm973get);
                HttpContent content = data.Content;
                data.EnsureSuccessStatusCode();

                string response = await content.ReadAsStringAsync();

                StringReader read = new StringReader(response);
                string line = read.ReadLine();
                string[] split1 = line.Split(',');
                string value = split1[1];

                string line2 = split1[1];
                string[] split2 = line2.Split(':');
                string index2 = split2[0];
                string value2 = split2[1];

                finalTrack = value2;

                if (appbarMode == 0)
                {
                    SystemMediaTransportControlsDisplayUpdater updater = systemControls.DisplayUpdater;
                    updater.MusicProperties.Title = finalTrack.Substring(2, finalTrack.Length - 3);
                    updater.Update();
                }

                if (type == 0)
                {
                    var messageDialog = new MessageDialog(finalTrack.Substring(2, finalTrack.Length - 3), "SG Radio - 973FM" + ": Now Playing");
                    messageDialog.Commands.Add(new UICommand("Close", (command) =>
                    {
                    }));
                    messageDialog.Commands.Add(new UICommand("Copy to Clipboard", (command) =>
                    {
                        DataPackage package = new DataPackage();
                        package.SetText(finalTrack);
                        Clipboard.SetContent(package);
                    }));
                    messageDialog.Commands.Add(new UICommand("Star", (command) =>
                    {
                        App.Insert_StarredItem((new StarredDataSQL()
                        {
                            StarredTitle = finalTrack.Substring(2, finalTrack.Length - 3),
                            StarredImage = Constants.getImage("1008"),
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
                    currStation.Text = "Current Station: 973FM";
                    currTrack.Text = "Current Track: " + finalTrack.Substring(2, finalTrack.Length - 3);
                    ToolTipService.SetToolTip(currTrack, finalTrack.Substring(2, finalTrack.Length - 3));
                    SetLiveTile();
                }
            }
            catch (Exception)
            {
            }
        }

        private async void mediacorpTitle(int type, int appbarMode)
        {
            string mediacorpAPI = "";
            string finalId = "1000";
            if (appbarMode == 0)
            {
                mediacorpAPI = Constants.getTitle(playingId);
                finalId = playingId;
            }

            else if (appbarMode == 1)
            {
                mediacorpAPI = Constants.getTitle(globalId);
                finalId = globalId;
            }

            HttpClient req = new HttpClient();

            try
            {
                var message2 = new HttpRequestMessage(HttpMethod.Get, mediacorpAPI);
                HttpResponseMessage data = await req.SendAsync(message2);
                HttpContent content = data.Content;
                data.EnsureSuccessStatusCode();

                string response2 = await content.ReadAsStringAsync();

                string artistStr = @"<span class=""albumartist"">";
                string titleStr = @"<span class=""albumtitle"">";
                string endStr = "</span>";

                int artistStrIndex = response2.IndexOf(artistStr);
                int titleStrIndex = response2.IndexOf(titleStr);
                if (artistStrIndex != -1 && titleStrIndex != -1)
                {
                    artistStrIndex += artistStr.Length;
                    titleStrIndex += titleStr.Length;

                    string Artist = WebUtility.HtmlDecode(response2.Substring(artistStrIndex, response2.IndexOf(endStr, artistStrIndex) - artistStrIndex));
                    string Title = WebUtility.HtmlDecode(response2.Substring(titleStrIndex, response2.IndexOf(endStr, titleStrIndex) - titleStrIndex));

                    if (appbarMode == 0)
                    {
                        SystemMediaTransportControlsDisplayUpdater updater = systemControls.DisplayUpdater;
                        systemControls.DisplayUpdater.MusicProperties.Title = Artist + " - " + Title;
                        updater.Update();
                    }

                    if (type == 0)
                    {
                        var messageDialog = new MessageDialog(Artist + " - " + Title, "SG Radio - " + Constants.getName(finalId) + ": Now Playing");
                        messageDialog.Commands.Add(new UICommand("Close", (command) =>
                        {
                        }));
                        messageDialog.Commands.Add(new UICommand("Copy to Clipboard", (command) =>
                        {
                            DataPackage package = new DataPackage();
                            package.SetText(Artist + " - " + Title);
                            Clipboard.SetContent(package);
                        }));
                        messageDialog.Commands.Add(new UICommand("Star", (command) =>
                        {
                            App.Insert_StarredItem((new StarredDataSQL()
                            {
                                StarredTitle = Artist + " - " + Title,
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
                        currTrack.Text = "Current Track: " + Artist + " - " + Title;
                        ToolTipService.SetToolTip(currTrack, Artist + " - " + Title);
                        SetLiveTile();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region LyricsFetch
        private async void mediacorpLyrics(int appbarMode)
        {
            string lyricsAPI = "";
            string finalId = "1000";
            if (appbarMode == 0)
            {
                lyricsAPI = Constants.getLyrics(playingId);
                finalId = playingId;
            }

            else if (appbarMode == 1)
            {
                lyricsAPI = Constants.getLyrics(globalId);
                finalId = globalId;
            }

            HttpClient client = new HttpClient();

            try
            {
                var lyricsData = new HttpRequestMessage(HttpMethod.Get, lyricsAPI);
                HttpResponseMessage data = await client.SendAsync(lyricsData);
                HttpContent content = data.Content;
                data.EnsureSuccessStatusCode();

                string response2 = await content.ReadAsStringAsync();

                response2 = response2.Replace("<br>", "\n");

                string lyricsStr = @"<div class=""lyricstext"">";
                string endStr = "</div>";

                int lyricsStrIndex = response2.IndexOf(lyricsStr);
                if (lyricsStrIndex != -1)
                {
                    lyricsStrIndex += lyricsStr.Length;

                    string Lyrics = WebUtility.HtmlDecode(response2.Substring(lyricsStrIndex, response2.IndexOf(endStr, lyricsStrIndex) - lyricsStrIndex));

                    if ((Lyrics == "Sorry! We are unable to display the lyrics for this song right now.") || (Lyrics == "We are not in a position to display these lyrics due to licensing restrictions. Sorry for the inconvenience."))
                    {
                        var messageDialog = new MessageDialog("No lyrics currently available.", Constants.getName(finalId) + ": Lyrics");
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
                    else
                    {
                        lyricsDialog.Title = Constants.getName(finalId) + ": Lyrics";
                        lyricsText.Text = Lyrics.Trim();
                        lyricsDialog.IsOpen = true;
                    }
                }
                else
                {
                    var messageDialog = new MessageDialog("No lyrics currently available.", Constants.getName(finalId) + ": Lyrics");
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
            }
            catch (Exception)
            {
            }
        }

        private void fm973Lyrics()
        {
            var messageDialog = new MessageDialog("Lyrics are not available for this station.", "SG Radio - 973FM: Lyrics");
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

        private void tlrLyrics()
        {
            var messageDialog = new MessageDialog("Lyrics are not available for this station.", "SG Radio - The Live Radio: Lyrics");
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

        private void bbcLyrics()
        {
            var messageDialog = new MessageDialog("Lyrics are not available for this station.", "SG Radio - BBC World Service: Lyrics");
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

        private void p98Lyrics()
        {
            var messageDialog = new MessageDialog("Lyrics are not available for this station.", "SG Radio - Power 98 FM: Lyrics");
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

        private void jiaLyrics()
        {
            var messageDialog = new MessageDialog("Lyrics are not available for this station.", "SG Radio - 88.3 Jia FM: Lyrics");
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

        private async void sphLyrics(int appbarMode)
        {
            string lyricsAPI = "";
            string finalId = "1000";
            if (appbarMode == 0)
            {
                lyricsAPI = Constants.getLyrics(playingId);
                finalId = playingId;
            }

            else if (appbarMode == 1)
            {
                lyricsAPI = Constants.getLyrics(globalId);
                finalId = globalId;
            }

            var uri = new Uri(lyricsAPI);
            var client = new HttpClient();

            string response = "";

            try
            {
                response = await client.GetStringAsync(uri);
            }
            catch (Exception)
            {
                return;
            }

            JsonObject parser = JsonObject.Parse(response);
            var rootLyrics = parser.GetNamedString("lyrics");

            if (rootLyrics == "Sorry! We are unable to display the lyrics for this song right now.")
            {
                var messageDialog = new MessageDialog("No lyrics currently available.", "SG Radio - " + Constants.getName(finalId) + ": Lyrics");
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
            else
            {
                lyricsDialog.Title = Constants.getName(finalId) + ": Lyrics";
                lyricsText.Text = rootLyrics.Trim().Replace("<div id=\"lyrics\" class=\"SCREENONLY\" itemprop=\"description\">", "");
                lyricsDialog.IsOpen = true;
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
            refreshBtn.IsEnabled = type;
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
                // for windows 8.1
                /* var tile = new SecondaryTile(
                     itemId,
                     "SG Radio: " + itemTitle,
                     "?ItemId=" + itemId, // activation arguments, can be used for: "?aa=test&bb=c"
                     itemImage,
                     TileSize.Wide310x150 | TileSize.Square150x150 
                     );*/

                tile.VisualElements.ForegroundText = ForegroundText.Light; // Depends on this one :D

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
                efrFetch(1);
            }
            else
            {
                efrFetch(0);
            }
        }

        private void lyrics_Click(object sender, RoutedEventArgs e)
        {
            if (appBarSelected)
            {
                if (globalId == "1001" || globalId == "1002" || globalId == "2007")
                {
                    sphLyrics(1);
                }
                else if (globalId == "1008")
                {
                    fm973Lyrics();
                }
                else if (globalId == "1009")
                {
                    tlrLyrics();
                }
                else if (globalId == "1010")
                {
                    bbcLyrics();
                }
                else if (globalId == "1011")
                {
                    p98Lyrics();
                }
                else if (globalId == "2008")
                {
                    jiaLyrics();
                }
                else
                {
                    mediacorpLyrics(1);
                }
            }
            else
            {
                if (playingId == "1001" || playingId == "1002" || playingId == "2007")
                {
                    sphLyrics(0);
                }
                else if (playingId == "1008")
                {
                    fm973Lyrics();
                }
                else if (playingId == "1009")
                {
                    tlrLyrics();
                }
                else if (playingId == "1010")
                {
                    bbcLyrics();
                }
                else if (playingId == "1011")
                {
                    p98Lyrics();
                }
                else if (playingId == "2008")
                {
                    jiaLyrics();
                }
                else
                {
                    mediacorpLyrics(0);
                }
            }
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            if (appBarSelected)
            {
                efrTick(1);
            }
            else
            {
                efrTick(0);
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
                searchGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                if (e.Size.Width < 540 && e.Size.Height >= 768)
                {
                    ParentHub.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                    switchVertical();
                }
                if (e.Size.Width < 600)
                {
                    statusGrid.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                    pageTitle.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
                else
                {
                    statusGrid.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Right;
                    Thickness margin = statusGrid.Margin;
                    margin.Right = 50;
                    margin.Top = 52;
                    statusGrid.Margin = margin;
                    ParentHub.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
                    pageTitle.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    switchHorizontal();
                }
            }
            else
            {
                searchGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
                statusGrid.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                Thickness margin = statusGrid.Margin;
                margin.Top = 52;
                statusGrid.Margin = margin;
                ParentHub.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
                pageTitle.Visibility = Windows.UI.Xaml.Visibility.Visible;
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

        private void VolumeChangedEvent(object sender, RangeBaseValueChangedEventArgs e)
        {
            mediaElement1.Volume = sliderVolume.Value / 100;
        }

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

        private void standbyClose(object sender, RoutedEventArgs e)
        {
            standbyGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
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

            standbyGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

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
            timerCountdown.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private void shutdown_Tick(object sender, object e)
        {
            countdown--;
            timerCountdown.Text = "Stopping Streams in: " + countdown + " min(s)";

            if (countdown == 0)
            {
                timerShutdown.Stop();
                completeClosure();
                timerCountdown.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
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
            timerCountdown.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

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
                "Gold 90.5FM", "HOT FM91.3", "Kiss 92FM", "Symphony 92.4FM", "938LIVE", "Class 95FM", "987FM", "Lush 99.5FM", "973FM", "Ria 89.7FM",
                "Y.E.S. 93.3FM", "Warna 94.2FM", "Capital 95.8", "XFM 96.3", "Oli 96.8FM", "Love 97.2FM", "UFM 1003", "90.5", "91.3", "92.0",
                "92.4", "93.8", "95.0", "99.5", "97.3", "89.7", "93.3", "94.2", "95.8", "96.3",
                "96.8", "97.2", "100.3", "BBC World Service", "Power 98 FM", "88.3 Jia FM" // "The Live Radio"
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

        private void weSpeakCode()
        {
            searchBox1.PlaceholderText = "We Speak Code";
        }
    }
}
