using PurpleMonitoringClient.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PurpleMonitoringClient.Client.JobStatusEventArgs;
using static PurpleMonitoringClient.Client.ProcessingStartedEventArgs;

namespace PurpleMonitoringClient.Mock
{
    public class DummyNotifier : INotifier
    {
        public event EventHandler<ClusterCreatedEventArgs> OnClusterCreated;
        public event EventHandler<ProcessingStartedEventArgs> OnProcessingStarted;
        public event EventHandler<JobStatusEventArgs> OnJobStatus;
        public event EventHandler<ProcessingDoneEventArgs> OnProcessingDone;
        public event EventHandler<ClusterFinalizedEventArgs> OnClusterFinalized;
        public event EventHandler<LogMessageEventArgs> OnLogMessage;

        class Job
        {
            public int Index { get; set; }
            public JobStatus Status { get; set; }
            public int Weight { get; set; }
        }

        List<Job> Jobs = new List<Job>();
        int size;

        public DummyNotifier(int clusterSize, int jobCount)
        {
            MakeJobs(jobCount);
            size = clusterSize;
        }

        private void MakeJobs(int jobCount)
        {
            var rand = new Random();
            for (int i = 0; i < jobCount; i++)
            {
                Jobs.Add(new Job()
                {
                    Index = i,
                    Status = JobStatus.Waiting,
                    Weight = rand.Next(1, 10)
                });
            }
        }

        public void Run()
        {
            SendClusterCreated();
            SendProcessingStarted();
            SendProcessingLog();
            SendProcessingDone();
            SendClusterFinalized();
        }

        private void SendClusterFinalized()
        {
            OnClusterFinalized?.Invoke(this, new ClusterFinalizedEventArgs()
            {
                Time = DateTime.Now
            });
        }

        private void SendClusterCreated()
        {
            OnClusterCreated?.Invoke(this, new ClusterCreatedEventArgs()
            {
                Size = size
            });
        }

        private void SendProcessingDone()
        {
            OnProcessingDone?.Invoke(this, new ProcessingDoneEventArgs()
            {
                Time = DateTime.Now
            });
        }

        private void SendProcessingStarted()
        {
            var i = 0;
            OnProcessingStarted?.Invoke(this, new ProcessingStartedEventArgs() {
                Info = Jobs.Select(j => new JobInfo()
                {
                    Index = j.Index,
                    Node = i++ % size,
                    Weight = j.Weight
                })
            });

        }
        
        private void SendProcessingLog()
        {
            var tasks = new Task[size];
            for (int i = 0; i < size; i++)
            {
                var rank = i;
                tasks[i] = ProcessJob(rank);
            }
            Task.WaitAll(tasks);
        }

        private async Task ProcessJob(int rank)
        {
            Debug.WriteLine("Processing on {0} started", rank);
            int i = 0;
            foreach(var j in Jobs)
            {
                if (i++ % size != rank)
                    continue;
                SendJobStatus(j.Index, JobStatus.Running);
                await Task.Delay(1000 * j.Weight);
                Debug.WriteLine("Job done {0}", j.Index);
                SendJobStatus(j.Index, JobStatus.Done);
            }
        }

        void SendJobStatus(int index, JobStatus status)
        {
            OnJobStatus?.Invoke(this, new JobStatusEventArgs()
            {
                Time = DateTime.Now,
                Index = index,
                Status = status
            });
        }
    }
}
