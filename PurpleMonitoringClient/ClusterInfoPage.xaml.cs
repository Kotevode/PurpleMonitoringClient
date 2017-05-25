using PurpleMonitoringClient.Client;
using PurpleMonitoringClient.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PurpleMonitoringClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ClusterInfoPage : Page
    {
        public ClusterInfoPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ColorizeTitleBar();
            if (e.Parameter is INotifier)
            {
                ClusterStatePageFrame.Navigate(typeof(ClusterState), e.Parameter);
                LoggerPageFrame.Navigate(typeof(LoggerPage), e.Parameter);
            }
        }

        private void ColorizeTitleBar()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = (this.Background as SolidColorBrush).Color;
            titleBar.ButtonBackgroundColor = (this.Background as SolidColorBrush).Color;
        }

    }
}
