using PurpleMonitoringClient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMonitoringClient.Model
{
    public class Job
    {
        public int Index { get; set; }
        public int Weight { get; set; }
        public JobStatus Status { get; set; }

        public void UpdateStatus(JobStatus status)
        {
            Status = status;
        }
    }
}
