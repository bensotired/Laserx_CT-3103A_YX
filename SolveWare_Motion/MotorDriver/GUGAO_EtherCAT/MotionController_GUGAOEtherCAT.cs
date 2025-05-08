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
    public class MotionController_GUGAOEtherCAT : MotionControllerBase, IMotionController
    {
        const int PHASE_SEARCHING_TIMEOUT_SECONDS = 60 * 1000;
        public MotionController_GUGAOEtherCAT(string name, string address, IInstrumentChassis chassis) : base(name, address, chassis)
        {
            _AxesCollection = new List<MotorAxisBase>();
        }
        public override MotorAxisBase CreateAxisInstance(MotorSetting setting)
        {
            var axis = new Motor_GUGAO(setting);
            if (axis.Interation.IsSimulation == false)
            {
                if (this.IsOnline)
                {
                    axis.Clear_AlarmSignal();
                    Thread.Sleep(10);
                    axis.Init();
                    if (axis.MotorGeneralSetting.MotorTable.IsPhaseSearchNeeded)
                    {
                        var ret = GUGAO_LIB.GTN_SetEcatAxisOnThreshold(setting.MotorTable.CardNo, setting.MotorTable.AxisNo, PHASE_SEARCHING_TIMEOUT_SECONDS);
                        if (ret != 0)
                        {
                            throw new Exception($"GUGAO motor driver setting [{setting.MotorTable.CardNo}-{ setting.MotorTable.AxisNo}] phase searching timeout  exception.");
                        }
                    }
                }
            }
            return axis;
        }
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