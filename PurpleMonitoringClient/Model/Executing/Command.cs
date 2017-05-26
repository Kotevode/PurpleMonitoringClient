using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PurpleMonitoringClient.Model.Executing
{
    class Command
    {
        [JsonProperty]
        int program;
        [JsonProperty]
        List<Node> nodes;

        [JsonIgnore]
        public int ProgramID => program;
        [JsonIgnore]
        public List<Node> Nodes => nodes;
        
        public Command(int program, List<Node> nodes)
        {
            this.program = program;
            this.nodes = nodes;
        }

    }
}
