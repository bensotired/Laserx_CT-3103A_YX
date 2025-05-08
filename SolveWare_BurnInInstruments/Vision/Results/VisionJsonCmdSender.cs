using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public class VisionJsonCmdSender : VisionJsonCmdBase
    {
    
        public VisionJsonCmdSender() : base()
        {
        }
        public VisionJsonCmdSender(string cmd, string projectName) : base(cmd)
        {
            this.ProjectName = projectName;
        }
        public VisionJsonCmdSender(string cmd, string projectName, string terminator) : base(cmd, terminator)
        {
            this.ProjectName = projectName;
            //this.CommandSN = projectName;
        }
    }
}