using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMonitoringClient.Model.Executing
{
    public class Program
    {
        int id;
        string name;
        string description;

        public int ID { get { return id; } }
        public string Name { get { return name; } }
        public string Description { get { return description; } }
        
        [JsonConstructor]
        Program(int id, string name, string description)
        {
            this.id = id;
            this.name = name;
            this.description = description;
        }

    }
}
