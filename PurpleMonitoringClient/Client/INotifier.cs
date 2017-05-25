using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMonitoringClient.Client
{
    public class NotifierArgs
    {
        public DateTime Time;
    }

    public class ClusterCreatedEventArgs: NotifierArgs
    {
        public int Size { get; set; }
    }

    public class ProcessingStartedEventArgs: NotifierArgs
    {
        public class JobInfo
        {
            public int Weight { get; set; }
            public int Node { get; set; }
            public int Index { get; set; }
        }

        public IEnumerable<JobInfo> Info { get; set; }

    }

    public class JobStatusEventArgs: NotifierArgs
    {
        public enum JobStatus
        {
            Waiting,
            Running,
            Done,
            Error
        }

        public int Index { get; set; }
        public JobStatus Status { get; set; }
    }

    public class ProcessingDoneEventArgs: NotifierArgs {}

    public class ClusterFinalizedEventArgs : NotifierArgs {}
    
    public class LogMessageEventArgs: NotifierArgs
    {
        public string Body { get; set; }
    }

    public interface INotifier
    {
        event EventHandler<ClusterCreatedEventArgs> OnClusterCreated;
        event EventHandler<ProcessingStartedEventArgs> OnProcessingStarted;
        event EventHandler<JobStatusEventArgs> OnJobStatus;
        event EventHandler<ProcessingDoneEventArgs> OnProcessingDone;
        event EventHandler<ClusterFinalizedEventArgs> OnClusterFinalized;
        event EventHandler<LogMessageEventArgs> OnLogMessage;
    }
}
