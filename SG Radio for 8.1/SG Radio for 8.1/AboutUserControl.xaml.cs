using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

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
            await Launcher.LaunchUriAsync(new Uri("http://www.windowsphone.com/en-sg/store/app/sg-radio/f61d74a8-bc2c-441a-95d3-bbc3cfac7fe0", UriKind.Absolute));
        }
    }
}
