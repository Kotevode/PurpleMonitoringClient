using PurpleMonitoringClient.Model;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PurpleMonitoringClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ClusterState : Page
    {
        public ClusterDetailViewModel ViewModel { get; set; }

        public ClusterState()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Cluster)
            {
                ViewModel = new ClusterDetailViewModel(e.Parameter as Cluster, this.Dispatcher);
            }
            else
            {
                throw new NotSupportedException("Parameter is not supported");
            }
        }

    }
}
