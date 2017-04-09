using PurpleMonitoringClient.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static PurpleMonitoringClient.Model.Cluster;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PurpleMonitoringClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoggerPage : Page
    {
        public LoggerViewModel ViewModel { get; private set; }

        public LoggerPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Logger)
            {
                ViewModel = new LoggerViewModel(e.Parameter as Logger, Dispatcher);
            }
            else
            {
                throw new NotSupportedException("Parameter not supported yet");
            }
        }
    }
}
