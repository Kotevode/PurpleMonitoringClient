using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMonitoringClient.Model.Executing
{
    [DataContract]
    class Command
    {
        [DataMember]
        int program;
        [DataMember]
        List<Node> nodes;

        public int ProgramID { get { return program; } }
        public List<Node> Nodes { get { return nodes; } }

        Command(int program, List<Node> nodes)
        {
            this.program = program;
            this.nodes = nodes;
        }

    }
}
