using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SG_Radio_for_8._1
{
    public sealed partial class AboutUserControl : UserControl
    {
        public AboutUserControl()
        {
            this.InitializeComponent();
        }

        private async void MobileApp_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://www.windowsphone.com/en-sg/store/app/sg-radio/f61d74a8-bc2c-441a-95d3-bbc3cfac7fe0", UriKind.Absolute));
        }
    }
}
