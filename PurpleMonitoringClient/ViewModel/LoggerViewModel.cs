using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using static PurpleMonitoringClient.Model.Cluster;
using static PurpleMonitoringClient.Model.Cluster.Logger;

namespace PurpleMonitoringClient.ViewModel
{
    public class LoggerViewModel
    {
        public ObservableCollection<LogMessageViewModel> Messages = new ObservableCollection<LogMessageViewModel>();
        public Logger Logger { get; private set; }
        public CoreDispatcher dispatcher;

        public LoggerViewModel(Logger logger, CoreDispatcher dispatcher)
        {
            this.Logger = logger;
            this.dispatcher = dispatcher;
            this.Logger.Log.CollectionChanged += Log_CollectionChanged;
        }

        private async void Log_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var newItems = e.NewItems.Cast<LogMessage>();
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                foreach (var m in newItems)
                    Messages.Add(new LogMessageViewModel(m));
            });
        }
    }
}
