using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SG_Radio_for_8._1
{
    public sealed partial class SettingsUserControl : UserControl
    {
        public SettingsUserControl()
        {
            this.InitializeComponent();
            Loaded += new RoutedEventHandler(PreferencesUserControl_Loaded);
        }

        void PreferencesUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("bgColor"))
            {
                switch (ApplicationData.Current.RoamingSettings.Values["bgColor"].ToString())
                {
                    case "Black":
                        bgCombo.SelectedItem = "Black";
                        break;
                    case "White":
                        bgCombo.SelectedItem = "White";
                        break;
                    case "Red":
                        bgCombo.SelectedItem = "Red";
                        break;
                    case "Yellow":
                        bgCombo.SelectedItem = "Yellow";
                        break;
                    case "Green":
                        bgCombo.SelectedItem = "Green";
                        break;
                    case "Cyan":
                        bgCombo.SelectedItem = "Cyan";
                        break;
                    case "Blue":
                        bgCombo.SelectedItem = "Blue";
                        break;
                    case "Purple":
                        bgCombo.SelectedItem = "Purple";
                        break;
                    case "Brown":
                        bgCombo.SelectedItem = "Brown";
                        break;
                }
            }

            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("txtColor"))
            {
                switch (ApplicationData.Current.RoamingSettings.Values["txtColor"].ToString())
                {
                    case "Black":
                        txtCombo.SelectedItem = "Black";
                        break;
                    case "White":
                        txtCombo.SelectedItem = "White";
                        break;
                    case "Red":
                        txtCombo.SelectedItem = "Red";
                        break;
                    case "Yellow":
                        txtCombo.SelectedItem = "Yellow";
                        break;
                    case "Green":
                        txtCombo.SelectedItem = "Green";
                        break;
                    case "Cyan":
                        txtCombo.SelectedItem = "Cyan";
                        break;
                    case "Blue":
                        txtCombo.SelectedItem = "Blue";
                        break;
                    case "Purple":
                        txtCombo.SelectedItem = "Purple";
                        break;
                    case "Brown":
                        txtCombo.SelectedItem = "Brown";
                        break;
                }
            }
        }

        private void txtColorChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationData.Current.RoamingSettings.Values["txtColor"] = txtCombo.SelectedItem;
        }

        private void bgColorChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationData.Current.RoamingSettings.Values["bgColor"] = bgCombo.SelectedItem;
        }
    }
}
