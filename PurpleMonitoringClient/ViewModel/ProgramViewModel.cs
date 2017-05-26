using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurpleMonitoringClient.Model.Executing;

namespace PurpleMonitoringClient.ViewModel
{
    using Model.Executing;

    public class ProgramViewModel
    {
        public Program Program { get; set; }

        public string Name => Program.Name;
        public string Description => Program.Description;
        
        public ProgramViewModel(Program program)
        {
            Program = program;
        }
    }
}
