using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMonitoringClient.ViewModel
{
    public class LogMessageViewModel
    {
        public string Timestamp { get; private set; }
        public string Body { get; private set; }
        public LogMessageViewModel(string body, DateTime time)
        {
            Timestamp = time.ToString();
            Body = body;
        }
    }
}
