using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMonitoringClient.Client
{
    public class JobInfoMessage
    {
        public int Weight { get; set; }
        public int? Node { get; set; }
        public int Index { get; set; }
    }
}
