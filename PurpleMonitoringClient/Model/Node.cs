using PurpleMonitoringClient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMonitoringClient.Model
{
    public class Node
    {
        public List<Job> Jobs = new List<Job>();
        public event EventHandler OnUpdate;
        public INotifier Notifier;
        public int Index { get; set; }

        public Node(int index, INotifier notifier)
        {
            this.Index = index;
            this.Notifier = notifier;
            Subscribe();
        }

        private void Subscribe()
        {
            Notifier.OnProcessingStarted += Notifier_OnProcessingStarted;
            Notifier.OnJobsDistributed += Notifier_OnJobsDistributed;
            Notifier.OnJobStatus += Notifier_OnJobStatus;
            Notifier.OnProcessingDone += Notifier_OnProcessingDone;
        }

        private void Notifier_OnProcessingDone(object sender, ProcessingDoneEventArgs e)
        {
            Jobs.Clear();
            OnUpdate(this, new EventArgs());
        }

        private void Notifier_OnJobStatus(object sender, JobStatusEventArgs e)
        {
            (from j in Jobs
             join s in e.JobStatuses on j.Index equals s.Index
             select new { Job = j, NewStatus = s.Status })
             .Select((x) => x.Job.Status = x.NewStatus);
            OnUpdate(this, new EventArgs());
        }

        private void Notifier_OnJobsDistributed(object sender, JobsDistributedEventArgs e)
        {
            foreach (var j in e.Jobs)
                if (j.Node.Value == Index)
                    Jobs.Add(new Job()
                    {
                        Index = j.Index,
                        Weight = j.Weight,
                    });
            OnUpdate(this, new EventArgs());
        }

        private void Notifier_OnProcessingStarted(object sender, ProcessingStartedEventArgs e)
        {
            Jobs.Clear();
            OnUpdate(this, new EventArgs());
        }
    }
}
