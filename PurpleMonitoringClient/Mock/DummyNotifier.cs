using PurpleMonitoringClient.Client;
using PurpleMonitoringClient.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMonitoringClient.Mock
{
    public class DummyNotifier : INotifier
    {
        public event EventHandler<ClusterCreatedEventArgs> OnClusterCreated;
        public event EventHandler<ProcessingStartedEventArgs> OnProcessingStarted;
        public event EventHandler<JobsDistributedEventArgs> OnJobsDistributed;
        public event EventHandler<JobStatusEventArgs> OnJobStatus;
        public event EventHandler<ProcessingDoneEventArgs> OnProcessingDone;
        public event EventHandler<ClusterFinalizedEventArgs> OnClusterFinalized;
        public event EventHandler<LogMessageEventArgs> OnLogMessage;

        List<Job> Jobs = new List<Job>();
        int size;
        Guid ClusterGUID;

        public DummyNotifier(int clusterSize, int jobCount, Guid clusterGUID)
        {
            MakeJobs(jobCount);
            size = clusterSize;
            ClusterGUID = clusterGUID;
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
            SendProcessStarted();
            SendJobDistributed();
            SendProcessingLog();
            SendProcessingDone();
        }

        private void SendProcessingDone()
        {
            OnProcessingDone?.Invoke(this, new ProcessingDoneEventArgs()
            {
                ClusterGUID = ClusterGUID
            });
        }

        private void SendJobDistributed()
        {
            var i = 0;
            OnJobsDistributed?.Invoke(this, new JobsDistributedEventArgs() {
                ClusterGUID = ClusterGUID,
                Jobs = Jobs.Select(j => new JobInfoMessage()
                {
                    Index = j.Index,
                    Node = i++ % size,
                    Weight = j.Weight
                })
            });

        }

        private void SendProcessStarted()
        {
            var i = 0;
            OnProcessingStarted?.Invoke(this, new ProcessingStartedEventArgs() {
                ClusterGUID = ClusterGUID,
                Jobs = Jobs.Select(j => new JobInfoMessage()
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
                ClusterGUID = ClusterGUID,
                JobStatuses = new List<JobStatusMessage>
                    {
                        new JobStatusMessage()
                        {
                            Index = index,
                            Status = status,
                            Timestamp = DateTime.Now
                        }
                    }
            });
        }
    }
}
