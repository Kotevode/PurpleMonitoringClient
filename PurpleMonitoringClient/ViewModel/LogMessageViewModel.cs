using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PurpleMonitoringClient.Model.Cluster.Logger;

namespace PurpleMonitoringClient.ViewModel
{
    public class LogMessageViewModel
    {
        public string Timestamp { get; private set; }
        public string Body { get; private set; }
        public LogMessageViewModel(LogMessage message)
        {
            Timestamp = message.Timestamp.ToString();
            Body = message.Message;
        }
    }
}
