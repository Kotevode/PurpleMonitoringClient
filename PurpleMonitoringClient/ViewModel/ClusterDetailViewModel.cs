using PurpleMonitoringClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace PurpleMonitoringClient.ViewModel
{
    public class ClusterDetailViewModel
    {
        public ObservableCollection<NodeStateViewModel> Nodes = new ObservableCollection<NodeStateViewModel>();
        public Cluster Cluster;
        CoreDispatcher dispatcher;

        public ClusterDetailViewModel(Cluster cluster, CoreDispatcher dispatcher)
        {
            this.Cluster = cluster;
            Cluster.OnUpdated += Cluster_OnUpdated;
            this.dispatcher = dispatcher;
            Refresh();
        }

        public async void Refresh()
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                Nodes.Clear();
                var maxLoad = (from n in Cluster.Nodes
                               select n.TotalWeight).Max();
                if (maxLoad == 0)
                    return;
                foreach (var n in Cluster.Nodes)
                    Nodes.Add(new NodeStateViewModel(n, maxLoad, dispatcher));
            });
        }

        private void Cluster_OnUpdated(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}
