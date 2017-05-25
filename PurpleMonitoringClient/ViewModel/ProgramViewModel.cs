using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurpleMonitoringClient.Model.Executing;

namespace PurpleMonitoringClient.ViewModel
{
    public class ProgramViewModel
    {
        Model.Executing.Program program;

        public string Name => program.Name;
        public string Description => program.Description;

        public ProgramViewModel(Model.Executing.Program program)
        {
            this.program = program;
        }

    }
}
