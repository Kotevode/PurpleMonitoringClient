using PurpleMonitoringClient.Client;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Core;
using static PurpleMonitoringClient.Client.JobStatusChanged;

namespace PurpleMonitoringClient.ViewModel
{
    public class LoggerViewModel
    {
        public ObservableCollection<LogMessageViewModel> Messages = new ObservableCollection<LogMessageViewModel>();
        public CoreDispatcher dispatcher;
        public INotifier notifier;

        public LoggerViewModel(CoreDispatcher dispatcher, INotifier notifier)
        {
            this.dispatcher = dispatcher;
            this.notifier = notifier;
            Subscribe();
        }

        void Subscribe()
        {
            notifier.OnProcessingStarted += Notifier_OnProcessingStarted;
            notifier.OnJobStatusChanged += Notifier_OnJobStatusChanged;
            notifier.OnProcessingDone += Notifier_OnProcessingDone;
            notifier.OnLogMessage += Notifier_OnLogMessage;
        }

        async void Notifier_OnLogMessage(object sender, ClusterEventArgs<LogMessage> e)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Messages.Add(new LogMessageViewModel(e.Content.Body, e.Time));
            });
        }

        async void Notifier_OnProcessingDone(object sender, ClusterEventArgs<ProcessingDone> e)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Messages.Add(new LogMessageViewModel(
                    "Обработка задач завершена",
                    e.Time
                    ));
            });
        }

        async void Notifier_OnJobStatusChanged(object sender, ClusterEventArgs<JobStatusChanged> e)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                string m = null;
                switch (e.Content.Status)
                {
                    case JobStatus.Done:
                        m = String.Format("Задача {0} выполнена", e.Content.Index);
                        break;
                    case JobStatus.Error:
                        m = String.Format("Выполнение задачи {0} прервано возникновением ошибки", e.Content.Index);
                        break;
                    case JobStatus.Waiting:
                        m = String.Format("Задача {0} ожидает начала выполнения", e.Content.Index);
                        break;
                    case JobStatus.Running:
                        m = String.Format("Задача {0} выполняется", e.Content.Index);
                        break;
                }
                if (m != null)
                    Messages.Add(
                        new LogMessageViewModel(m, e.Time)
                            );
            });
        }

        async void Notifier_OnProcessingStarted(object sender, ClusterEventArgs<ProcessingStarted> e)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Messages.Add(
                    new LogMessageViewModel("Старт обработки задач", e.Time)
                    );
            });
        }
    }
}
