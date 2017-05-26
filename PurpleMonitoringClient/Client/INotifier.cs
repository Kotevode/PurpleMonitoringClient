using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PurpleMonitoringClient.Client
{
    public class ClusterEventArgs<T>
    {
        DateTime time;
        T content;

        public DateTime Time => time;
        public T Content => content;

        [JsonConstructor]
        public ClusterEventArgs(int time, T content)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            epoch.AddSeconds(time);
            this.time = epoch;
            this.content = content;
        }

        public ClusterEventArgs(DateTime time, T content)
        {
            this.time = time;
            this.content = content;
        }

    }

    public class ClusterCreated
    {
        int size;

        public int Size => size;

        [JsonConstructor]
        public ClusterCreated(int size)
        {
            this.size = size;
        }

    }

    public class ProcessingStarted
    {
        public class JobInfo
        {
            int weight;
            int node;
            int index;

            public int Weight => weight;
            public int Node => node;
            public int Index => index;

            [JsonConstructor]
            public JobInfo(int weight, int node, int index)
            {
                this.weight = weight;
                this.node = node;
                this.index = index;
            }
        }

        List<JobInfo> info;

        public List<JobInfo> Info => info;

        [JsonConstructor]
        public ProcessingStarted(List<JobInfo> info)
        {
            this.info = info;
        }
    }

    public class JobStatusChanged
    {
        public enum JobStatus
        {
            Waiting,
            Running,
            Done,
            Error
        }

        int index;
        JobStatus status;

        public int Index => index;
        public JobStatus Status => status;

        [JsonConstructor]
        public JobStatusChanged(int index, JobStatus status)
        {
            this.index = index;
            this.status = status;
        }
    }

    public class ProcessingDone {}

    public class ClusterFinalized {}
    
    public class LogMessage
    {
        string body;

        public string Body => body;

        LogMessage(string body)
        {
            this.body = body;
        }
    }

    public class Terminated
    {
        public bool Succeed { get; set; }
        public string Message { get; set; }
    }

    public interface INotifier
    {
        event EventHandler<ClusterEventArgs<ClusterCreated>> OnClusterCreated;
        event EventHandler<ClusterEventArgs<ProcessingStarted>> OnProcessingStarted;
        event EventHandler<ClusterEventArgs<JobStatusChanged>> OnJobStatusChanged;
        event EventHandler<ClusterEventArgs<ProcessingDone>> OnProcessingDone;
        event EventHandler<ClusterEventArgs<ClusterFinalized>> OnClusterFinalized;
        event EventHandler<ClusterEventArgs<LogMessage>> OnLogMessage;
        event EventHandler<ClusterEventArgs<Terminated>> OnTerminated;
    }
}
