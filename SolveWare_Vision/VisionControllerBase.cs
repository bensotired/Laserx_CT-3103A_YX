using SolveWare_BurnInInstruments;

namespace SolveWare_Vision
{
    public abstract class VisionControllerBase : InstrumentBase, IVisionController
    {
        public VisionControllerBase(string name, string address, IInstrumentChassis chassis) : base(name, address, chassis)
        {

        }

        public virtual void Dev()
        {
            throw new System.NotImplementedException();
        }

        public TResult GetVisionResult<TResult>(string cmd, string projectName) where TResult : VisionJsonCmdBase
        {
            throw new System.NotImplementedException();
        }

        public virtual string GetVisionResult_Universal(string cmd )
        {
            throw new System.NotImplementedException();
        }
        
        public virtual string GetVisionResult_Universal(string cmd, params object[] args)
        {
            throw new System.NotImplementedException();
        }

        public virtual string GetVisionResult_UniversalWithResponTime(string cmd, int responTime_ms)
        {
            throw new System.NotImplementedException();
        }

        //public virtual string GetVisionResult_Universal(string cmd, int responTime_ms)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
