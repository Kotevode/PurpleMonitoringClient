using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMonitoringClient.Client
{
    public enum JobStatus
    {
        Waiting,
        Running,
        Done,
        Error
    }

    public class JobStatusMessage
    {
        public DateTime Timestamp { get; set; }
        public JobStatus Status { get; set; }
        public int Index { get; set; }
    }
}
