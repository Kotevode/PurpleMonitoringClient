using PurpleMonitoringClient.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using static PurpleMonitoringClient.Client.JobStatusEventArgs;

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
            notifier.OnJobStatus += Notifier_OnJobStatus;
            notifier.OnProcessingDone += Notifier_OnProcessingDone;
            notifier.OnLogMessage += Notifier_OnLogMessage;
        }

        async void Notifier_OnLogMessage(object sender, LogMessageEventArgs e)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Messages.Add(new LogMessageViewModel(e.Body, e.Time));
            });
        }

        async void Notifier_OnProcessingDone(object sender, ProcessingDoneEventArgs e)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Messages.Add(new LogMessageViewModel(
                    "Обработка задач завершена",
                    e.Time
                    ));
            });
        }

        async void Notifier_OnJobStatus(object sender, JobStatusEventArgs e)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                string m = null;
                switch (e.Status)
                {
                    case JobStatus.Done:
                        m = String.Format("Задача {0} выполнена", e.Index);
                        break;
                    case JobStatus.Error:
                        m = String.Format("Выполнение задачи {0} прервано возникновением ошибки", e.Index);
                        break;
                    case JobStatus.Waiting:
                        m = String.Format("Задача {0} ожидает начала выполнения", e.Index);
                        break;
                    case JobStatus.Running:
                        m = String.Format("Задача {0} выполняется", e.Index);
                        break;
                }
                if (m != null)
                    Messages.Add(
                        new LogMessageViewModel(m, e.Time)
                            );
            });
        }

        async void Notifier_OnProcessingStarted(object sender, ProcessingStartedEventArgs e)
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
