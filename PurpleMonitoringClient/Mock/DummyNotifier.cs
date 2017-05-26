using PurpleMonitoringClient.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static PurpleMonitoringClient.Client.JobStatusChanged;
using static PurpleMonitoringClient.Client.ProcessingStarted;

namespace PurpleMonitoringClient.Mock
{
    public class DummyNotifier : INotifier
    {
        public event EventHandler<ClusterEventArgs<ClusterCreated>> OnClusterCreated;
        public event EventHandler<ClusterEventArgs<ProcessingStarted>> OnProcessingStarted;
        public event EventHandler<ClusterEventArgs<JobStatusChanged>> OnJobStatusChanged;
        public event EventHandler<ClusterEventArgs<ProcessingDone>> OnProcessingDone;
        public event EventHandler<ClusterEventArgs<ClusterFinalized>> OnClusterFinalized;
        public event EventHandler<ClusterEventArgs<LogMessage>> OnLogMessage;
        public event EventHandler<ClusterEventArgs<Terminated>> OnTerminated;

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
            SendOnTerminated();
        }

        private void SendClusterFinalized()
        {
            OnClusterFinalized?.Invoke(
                this,
                new ClusterEventArgs<ClusterFinalized>(DateTime.Now, new ClusterFinalized())
                );
        }

        private void SendClusterCreated()
        {
            OnClusterCreated?.Invoke(
                this,
                new ClusterEventArgs<ClusterCreated>(
                    DateTime.Now,
                    new ClusterCreated(size)));
        }

        private void SendProcessingDone()
        {
            OnProcessingDone?.Invoke(
                this, 
                new ClusterEventArgs<ProcessingDone>(
                    DateTime.Now,
                    new ProcessingDone()));
        }

        private void SendProcessingStarted()
        {
            var i = 0;
            var info = Jobs.Select(j => new JobInfo(j.Weight, i++ % size, j.Index)).ToList();
            OnProcessingStarted?.Invoke(
                this,
                new ClusterEventArgs<ProcessingStarted>(
                    DateTime.Now,
                    new ProcessingStarted(info)));

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

        private void SendOnTerminated()
        {
            OnTerminated?.Invoke(
                this,
                new ClusterEventArgs<Terminated>(
                    DateTime.Now,
                    new Terminated { Succeed = true }));
        }

        private async Task ProcessJob(int rank)
        {
            Debug.WriteLine("Processing on {0} started", rank);
            int i = 0;
            foreach (var j in Jobs)
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
            OnJobStatusChanged?.Invoke(
                this, 
                new ClusterEventArgs<JobStatusChanged>(
                    DateTime.Now,
                    new JobStatusChanged(index, status)));
        }
    }
}
