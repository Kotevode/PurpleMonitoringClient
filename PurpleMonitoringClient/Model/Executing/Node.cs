using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMonitoringClient.Model.Executing
{
    class Node
    {
        string hostName;
        int coreNumber;

        public string HostName { get { return hostName; } }
        public int CoreNumber { get { return coreNumber; } }
        
        [JsonConstructor]
        Node(string hostName, int coreNumber)
        {
            this.hostName = hostName;
            this.coreNumber = coreNumber;
        }
    }
}
