using SolveWare_BurnInInstruments;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_Motion
{
    public class IOController_GUGAOEtherCAT : InstrumentBase //, IMotionController
    {
        public IOController_GUGAOEtherCAT()
        {
        }
         
        //public  virtual MotorAxisBase CreateAxisInstance(MotorSetting setting, bool isSimulaton)
        //{
        //    var axis = new Motor_GUGAO(setting, isSimulaton);
        //    return axis;
        //}

        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }
    }
}