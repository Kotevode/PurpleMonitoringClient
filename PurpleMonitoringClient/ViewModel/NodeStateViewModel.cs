using PurpleMonitoringClient.Client;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Core;
using static PurpleMonitoringClient.Client.JobStatusChanged;

namespace PurpleMonitoringClient.ViewModel
{
    public class NodeStateViewModel
    {
        public ObservableCollection<JobViewModel> Undone = new ObservableCollection<JobViewModel>();
        public ObservableCollection<JobViewModel> Done = new ObservableCollection<JobViewModel>();
        public int Index { get; private set; }
        CoreDispatcher dispatcher;

        public NodeStateViewModel(CoreDispatcher dispatcher, INotifier notifier, int index)
        {
            this.Index = index;
            this.dispatcher = dispatcher;
            Subscribe(notifier);
        }

        void Subscribe(INotifier notifier)
        {
            notifier.OnJobStatusChanged += Notifier_OnJobStatusChanged;
        }

        async private void Notifier_OnJobStatusChanged(object sender, ClusterEventArgs<JobStatusChanged> e)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var content = e.Content;
                JobViewModel vm = null;
                if ((vm = Undone.FirstOrDefault(x => x.Index == content.Index)) != null)
                {
                    if (content.Status == JobStatus.Done || content.Status == JobStatus.Error)
                    {
                        Undone.Remove(vm);
                        Done.Add(vm);
                    }
                    vm.Status = content.Status;
                }
                else if ((vm = Done.FirstOrDefault(x => x.Index == content.Index)) != null)
                {
                    if (content.Status == JobStatus.Waiting || content.Status == JobStatus.Running)
                    {
                        Done.Remove(vm);
                        Undone.Add(vm);
                    }
                    vm.Status = content.Status;
                }
            });
        }
    }
}
