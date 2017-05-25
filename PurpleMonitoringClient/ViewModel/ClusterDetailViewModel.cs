using PurpleMonitoringClient.Client;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace PurpleMonitoringClient.ViewModel
{
    public class ClusterDetailViewModel
    {
        public ObservableCollection<NodeStateViewModel> Nodes = new ObservableCollection<NodeStateViewModel>();
        CoreDispatcher dispatcher;
        INotifier notifier;

        public ClusterDetailViewModel(CoreDispatcher dispatcher, INotifier notifier)
        {
            this.dispatcher = dispatcher;
            this.notifier = notifier;
            Subscribe();
        }

        void Subscribe()
        {
            notifier.OnClusterCreated += Notifier_OnClusterCreated;
            notifier.OnClusterFinalized += Notifier_OnClusterFinalized;
            notifier.OnProcessingStarted += Notifier_OnProcessingStarted;
            notifier.OnProcessingDone += Notifier_OnProcessingDone;
        }

        async void Notifier_OnProcessingDone(object sender, ProcessingDoneEventArgs e)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                foreach(var n in Nodes) {
                    n.Done.Clear();
                    n.Undone.Clear();
                }
            });
        }

        async void Notifier_OnProcessingStarted(object sender, ProcessingStartedEventArgs e)
        {
           await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
           {
                var nodeLoad = new int[Nodes.Count];
                foreach (var j in e.Info)
                    nodeLoad[j.Node] += j.Weight;
                var maxLoad = nodeLoad.Max();

                foreach (var j in e.Info)
                    Nodes[j.Node].Undone.Add(new JobViewModel(j.Index, j.Weight, maxLoad));
            });
        }

        async void Notifier_OnClusterFinalized(object sender, ClusterFinalizedEventArgs e)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Nodes.Clear();
            });
        }

        async void Notifier_OnClusterCreated(object sender, ClusterCreatedEventArgs e)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                for (int i = 0; i < e.Size; i++)
                    Nodes.Add(new NodeStateViewModel(dispatcher, notifier, i));
            });
        }
    }
}
