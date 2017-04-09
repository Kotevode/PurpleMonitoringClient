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
    public class NodeStateViewModel
    {
        public ObservableCollection<JobViewModel> Undone = new ObservableCollection<JobViewModel>();
        public ObservableCollection<JobViewModel> Done = new ObservableCollection<JobViewModel>();
        public int Index { get; private set; }
        public Node Node { get; private set; }
        int maxLoad;
        CoreDispatcher dispatcher;

        public NodeStateViewModel(Node node, int maxLoad, CoreDispatcher dispatcher)
        {
            this.Node = node;
            this.maxLoad = maxLoad;
            this.dispatcher = dispatcher;
            this.Node.OnUpdate += Node_OnUpdate;
        }

        private async void Node_OnUpdate(object sender, EventArgs e)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                Undone.Clear();
                Done.Clear();
                var viewModels = (from j in Node.Jobs
                                 select new JobViewModel(j, maxLoad)).ToList();
                foreach (var vm in viewModels)
                    if (vm.Job.Status == Client.JobStatus.Waiting ||
                        vm.Job.Status == Client.JobStatus.Running)
                        Undone.Add(vm);
                    else
                        Done.Add(vm);
            });
        }
    }
}
