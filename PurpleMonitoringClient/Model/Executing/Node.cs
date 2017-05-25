using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMonitoringClient.Model.Executing
{
    [DataContract]
    class Node
    {
        [DataMember]
        string hostName;
        [DataMember]
        int coreNumber;

        public string HostName { get { return hostName; } }
        public int CoreNumber { get { return coreNumber; } }

        Node(string hostName, int coreNumber)
        {
            this.hostName = hostName;
            this.coreNumber = coreNumber;
        }
    }
}
