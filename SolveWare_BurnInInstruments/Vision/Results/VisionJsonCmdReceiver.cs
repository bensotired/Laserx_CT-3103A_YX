using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public class VisionJsonCmdReceiver : VisionJsonCmdBase 
    {
 
        public bool Success { get; set; }
        public int ErrorId { get; set; }
        public string ErrorMsg { get; set; }
        public VisionJsonCmdReceiver() : base()
        {

        }
        [JsonIgnore]
        public VisionResultErrorCode ErrorCode
        {
            get
            {
                return (VisionResultErrorCode)this.ErrorId;
            }
        }
    }
}