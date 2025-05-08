using System.Threading;
using SolveWare_BurnInInstruments;

namespace SolveWare_Vision
{
    public class VisionController_LaserX_Image : VisionControllerBase, IVisionController
    {
        const int Delay_ms = 200;
        public VisionController_LaserX_Image(string name, string address, IInstrumentChassis chassis) : base(name, address, chassis)
        {

        }
        public override void Dev()
        {
            var debug = this.GetVisionResult<VisionResult_OCR>("QueryVersion", "");
        }
        public VisionResult_LaserX_Image_Universal GetVisionResult_Universal(string cmd, string projectName)
        {
            VisionJsonCmdSender cw = new VisionJsonCmdSender(cmd, projectName);
            var val = this._chassis.Query(cw.GetCommand(), Delay_ms);
            return VisionJsonDataConverter.ConvertJsonDataTo<VisionResult_LaserX_Image_Universal>(val);
        }
        public override string GetVisionResult_Universal(string cmd)
        {
            VisionJsonCmdSender cw = new VisionJsonCmdSender(cmd, string.Empty);
            var val = this._chassis.Query(cw.GetCommand(), 50);
            return val;
        }
        public override string GetVisionResult_UniversalWithResponTime(string cmd, int responTime_ms)
        {
            VisionJsonCmdSender cw = new VisionJsonCmdSender(cmd, string.Empty);
            var val = this._chassis.Query(cw.GetCommand(), responTime_ms);
            return val;
        }
        public VisionResult_LaserX_Image_Universal GetVisionResult_Universal(string cmd, int delay)
        {
            VisionJsonCmdSender cw = new VisionJsonCmdSender(cmd, string.Empty);
            var val = this._chassis.Query(cw.GetCommand(), delay);
            return VisionJsonDataConverter.ConvertJsonDataTo<VisionResult_LaserX_Image_Universal>(val);
        }
        //public TResult GetVisionResult<TResult>(string cmd, string projectName) where TResult : VisionJsonCmdBase
        //{
        //    VisionJsonCmdSender cw = new VisionJsonCmdSender(cmd, projectName);
        //    var val = this._chassis.Query(cw.GetCommand(), Delay_ms);
        //    return VisionJsonDataConverter.ConvertJsonDataTo<TResult>(val);
        //}
        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
            //throw new System.NotImplementedException();
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            //throw new System.NotImplementedException();
        }
    }
}