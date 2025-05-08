using SolveWare_BurnInInstruments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_Vision
{
    public interface IVisionController: IInstrumentBase
    {
        void Dev();
        TResult GetVisionResult<TResult>(string cmd, string projectName) where TResult : VisionJsonCmdBase;
        string GetVisionResult_Universal(string cmd );
        string GetVisionResult_UniversalWithResponTime(string cmd, int responTime_ms);
        string GetVisionResult_Universal(string cmd,params object[]  args);
    }
}
