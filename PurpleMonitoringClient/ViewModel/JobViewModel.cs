using PurpleMonitoringClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;

namespace PurpleMonitoringClient.ViewModel
{
    public class JobViewModel
    {
        public Job Job { get; private set; }
        public double BlockHeight { get; private set; }
        public int Weight { get; private set; }
        public Color BackgroundColor { get; private set; }

        public JobViewModel(Job job, int maxLoad)
        {
            this.Job = job;
            this.BlockHeight = (double)job.Weight / maxLoad * 300;
            switch (Job.Status)
            {
                case Client.JobStatus.Running:
                    BackgroundColor = Colors.Yellow;
                    break;
                case Client.JobStatus.Done:
                    BackgroundColor = Colors.Green;
                    break;
                case Client.JobStatus.Error:
                    BackgroundColor = Colors.Red;
                    break;
                default:
                    BackgroundColor = Colors.White;
                    break;
            }
        }

    }
}
