using PurpleMonitoringClient.Networking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PurpleMonitoringClient.ViewModel;
using PurpleMonitoringClient.Client;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PurpleMonitoringClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ProgramListViewModel ViewModel { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            ViewModel = new ProgramListViewModel(Dispatcher);
        }

        private void RunProgram(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                var tag = (sender as Button).Tag;
                if (tag is Model.Executing.Program)
                    RunProgram(tag as Model.Executing.Program);
            }

        }

        async void RunProgram(Model.Executing.Program program)
        {
            var wsNotifier = new WSNotifier();
            this.Frame.Navigate(typeof(ClusterInfoPage), wsNotifier);
            await wsNotifier.ExecuteAsync(
                ViewModel.Host,
                new Model.Executing.Command(
                    program.ID,
                    new List<Model.Executing.Node>()
                    ));
        }
    }
}
