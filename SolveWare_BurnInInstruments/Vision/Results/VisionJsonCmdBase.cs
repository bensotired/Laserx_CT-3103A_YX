using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments 
{
    public class VisionJsonCmdBase
    {
        public string CommandName { get; set; }
        public string CommandSN { get; set; }
        public string ProjectName { get; set; }

        protected string _terminator = "\n";
        public VisionJsonCmdBase()
        {

        }
        public VisionJsonCmdBase(string cmd)
        {
            this.CommandName = cmd;
        }
        public VisionJsonCmdBase(string cmd, string terminator) : this(cmd)
        {
            _terminator = terminator;
        }
        public VisionJsonCmdBase(string cmd, string cmdSn, string terminator) : this(cmd, terminator)
        {
            CommandSN = cmdSn;
        }

        public string GetCommand()
        {
            var jstrVal = $"{JsonConvert.SerializeObject(this)}{ this._terminator }";
            return jstrVal;
        }
    }
}
