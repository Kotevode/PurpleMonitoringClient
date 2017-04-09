using PurpleMonitoringClient.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMonitoringClient.Model
{
    public class Cluster
    {
        #region Logger

        public class Logger
        {
            public class LogMessage
            {
                public DateTime Timestamp { get; set; }
                public string Message { get; set; }
            }

            public INotifier Notifier { get; set; }
            public ObservableCollection<LogMessage> Log = new ObservableCollection<LogMessage>();

            public Logger(INotifier notifier)
            {
                this.Notifier = notifier;
                Subscribe();
            }

            #region ClientBindings

            void Subscribe()
            {
                Notifier.OnProcessingStarted += Notifier_OnProcessingStarted;
                Notifier.OnJobsDistributed += Notifier_OnJobsDistributed;
                Notifier.OnJobStatus += Notifer_OnJobStatus;
                Notifier.OnProcessingDone += Notifier_OnProcessingDone;
                Notifier.OnLogMessage += Notifier_OnLogMessage;
            }

            private void Notifier_OnLogMessage(object sender, LogMessageEventArgs e)
            {
                foreach (var m in e.Messages)
                    Log.Add(new LogMessage()
                    {
                        Message = m.Body,
                        Timestamp = m.Timestamp
                    });
            }

            private void Notifier_OnProcessingDone(object sender, ProcessingDoneEventArgs e)
            {
                Log.Add(new LogMessage()
                {
                    Message = "Обработка задач завершена",
                    Timestamp = DateTime.Now
                });
            }

            private void Notifer_OnJobStatus(object sender, JobStatusEventArgs e)
            {
                foreach (var s in e.JobStatuses)
                {
                    string m = null;
                    switch (s.Status)
                    {
                        case JobStatus.Done:
                            m = String.Format("Задача {0} выполнена", s.Index);
                            break;
                        case JobStatus.Error:
                            m = String.Format("Выполнение задачи {0} прервано возникновением ошибки", s.Index);
                            break;
                        case JobStatus.Waiting:
                            m = String.Format("Задача {0} ожидает начала выполнения", s.Index);
                            break;
                        case JobStatus.Running:
                            m = String.Format("Задача {0} выполняется", s.Index);
                            break;
                    }
                    if (m != null)
                        Log.Add(new LogMessage()
                        {
                            Message = m,
                            Timestamp = s.Timestamp
                        });
                }
            }

            private void Notifier_OnJobsDistributed(object sender, JobsDistributedEventArgs e)
            {
                Log.Add(new LogMessage()
                {
                    Message = "Задачи распределены между вычислительными узлами",
                    Timestamp = DateTime.Now
                });
            }

            private void Notifier_OnProcessingStarted(object sender, ProcessingStartedEventArgs e)
            {
                Log.Add(new LogMessage()
                {
                    Message = "Старт обработки задач",
                    Timestamp = DateTime.Now
                });
            }

            #endregion

        }

        #endregion

        #region Filter

        public class Filter : INotifier
        {
            public event EventHandler<ClusterCreatedEventArgs> OnClusterCreated;
            public event EventHandler<ProcessingStartedEventArgs> OnProcessingStarted;
            public event EventHandler<JobsDistributedEventArgs> OnJobsDistributed;
            public event EventHandler<JobStatusEventArgs> OnJobStatus;
            public event EventHandler<ProcessingDoneEventArgs> OnProcessingDone;
            public event EventHandler<ClusterFinalizedEventArgs> OnClusterFinalized;
            public event EventHandler<LogMessageEventArgs> OnLogMessage;
            public Guid GUID { get; private set; }
            public INotifier Notifier;

            public Filter(Guid GUID, INotifier notifier)
            {
                this.GUID = GUID;
                this.Notifier = notifier;
                Subscribe();
            }

            void Subscribe()
            {
                Notifier.OnProcessingStarted += Notifier_OnProcessingStarted;
                Notifier.OnJobsDistributed += Notifier_OnJobsDistributed;
                Notifier.OnJobStatus += Notifier_OnJobStatus;
                Notifier.OnLogMessage += Notifier_OnLogMessage;
                Notifier.OnProcessingDone += Notifier_OnProcessingDone;
            }

            private void Notifier_OnProcessingDone(object sender, ProcessingDoneEventArgs e)
            {
                if (e.ClusterGUID != GUID)
                    return;
                OnProcessingDone(sender, e);
            }

            private void Notifier_OnLogMessage(object sender, LogMessageEventArgs e)
            {
                if (e.ClusterGUID != GUID)
                    return;
                OnLogMessage(sender, e);
            }

            private void Notifier_OnJobStatus(object sender, JobStatusEventArgs e)
            {
                if (e.ClusterGUID != GUID)
                    return;
                OnJobStatus(sender, e);
            }

            private void Notifier_OnJobsDistributed(object sender, JobsDistributedEventArgs e)
            {
                if (e.ClusterGUID != GUID)
                    return;
                OnJobsDistributed(sender, e);
            }

            private void Notifier_OnProcessingStarted(object sender, ProcessingStartedEventArgs e)
            {
                if (e.ClusterGUID != GUID)
                    return;
                OnProcessingStarted(sender, e);
            }

        }

        #endregion

        public Guid GUID { get; private set; }
        public INotifier Notifier { get; private set; }
        public Logger MessgeLogger { get; private set; }
        public List<Node> Nodes { get; private set; }
        public event EventHandler OnUpdated;
        public string Name { get; private set; }

        public Cluster(int size, string name, Guid GUID, INotifier notifier)
        {
            this.Nodes = new List<Node>();
            this.Notifier = new Filter(GUID, notifier);
            for (int i = 0; i < size; i++)
                Nodes.Add(new Node(i, Notifier));
            this.Name = name;
            this.GUID = GUID;
            this.MessgeLogger = new Logger(new Filter(GUID, notifier));
            Subscribe();
        }

        private void Subscribe()
        {
            Notifier.OnJobsDistributed += Notifier_OnJobsDistributed;
        }

        private void Notifier_OnJobsDistributed(object sender, JobsDistributedEventArgs e)
        {
            OnUpdated(this, new EventArgs());
        }
    }
}
