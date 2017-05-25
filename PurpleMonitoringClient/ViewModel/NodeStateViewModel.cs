using PurpleMonitoringClient.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using static PurpleMonitoringClient.Client.JobStatusEventArgs;

namespace PurpleMonitoringClient.ViewModel
{
    public class NodeStateViewModel
    {
        public ObservableCollection<JobViewModel> Undone = new ObservableCollection<JobViewModel>();
        public ObservableCollection<JobViewModel> Done = new ObservableCollection<JobViewModel>();
        public int Index { get; private set; }
        CoreDispatcher dispatcher;
        INotifier notifier;

        public NodeStateViewModel(CoreDispatcher dispatcher, INotifier notifier, int index)
        {
            this.Index = index;
            this.dispatcher = dispatcher;
            Subscribe(notifier);
        }

        void Subscribe(INotifier notifier)
        {
            notifier.OnJobStatus += Notifier_OnJobStatus;
        }

        async private void Notifier_OnJobStatus(object sender, JobStatusEventArgs e)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                JobViewModel vm = null;
                if ((vm = Undone.FirstOrDefault(x => x.Index == e.Index)) != null)
                {
                    if (e.Status == JobStatus.Done || e.Status == JobStatus.Error)
                    {
                        Undone.Remove(vm);
                        Done.Add(vm);
                    }
                    vm.Status = e.Status;
                }
                else if ((vm = Done.FirstOrDefault(x => x.Index == e.Index)) != null)
                {
                    if (e.Status == JobStatus.Waiting || e.Status == JobStatus.Running)
                    {
                        Done.Remove(vm);
                        Undone.Add(vm);
                    }
                    vm.Status = e.Status;
                }
            });
        }
    }
}
