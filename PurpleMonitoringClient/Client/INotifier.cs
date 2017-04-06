using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMonitoringClient.Client
{
    public class NotifierArgs
    {
        public Guid ClusterGUID { get; set; }
    }

    public class ClusterCreatedEventArgs: NotifierArgs
    {
        public string Name { get; set; }
        public int Size { get; set; }
    }

    public class ProcessingStartedEventArgs: NotifierArgs
    {
        public IEnumerable<JobInfoMessage> Jobs { get; set; }
    }
    
    public class JobsDistributedEventArgs: NotifierArgs
    {
        public IEnumerable<JobInfoMessage> Jobs { get; set; }
    }

    public class JobStatusEventArgs: NotifierArgs
    {
        public IEnumerable<JobStatusMessage> JobStatuses { get; set; }
    }

    public class ProcessingDoneEventArgs: NotifierArgs {}

    public class ClusterFinalizedEventArgs : NotifierArgs {}
    
    public class LogMessageEventArgs: NotifierArgs
    {
        public IEnumerable<Message> Messages { get; set; }
    }

    public interface INotifier
    {
        event EventHandler<ClusterCreatedEventArgs> OnClusterCreated;
        event EventHandler<ProcessingStartedEventArgs> OnProcessingStarted;
        event EventHandler<JobsDistributedEventArgs> OnJobsDistributed;
        event EventHandler<JobStatusEventArgs> OnJobStatus;
        event EventHandler<ProcessingDoneEventArgs> OnProcessingDone;
        event EventHandler<ClusterFinalizedEventArgs> OnClusterFinalized;
        event EventHandler<LogMessageEventArgs> OnLogMessage;
    }
}
