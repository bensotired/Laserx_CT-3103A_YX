using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolveWare_BurnInInstruments;

namespace SolveWare_BurnInInstruments
{
    public interface ITemperatureController_2700A: IInstrumentBase
    {
        void SetTargetObjectVoltageCtl(int i, double val);
        void SetTargetObjectVoltageCtlEnable(int i, int enable);
        void SetAllChannelVoltageCtlEnable(bool isEnable);
        void SetAllChannelVoltageCtl(double Value);
        float[] GetAllChannelVoltageCtl();
        double[] TargetObjectVoltageCtl { get; }
        TECValues_2700A[] GetAllReadings();

        double[] CurrentTEC_V { get; }
        double[] CurrentTEC_A { get; }
        bool IsChannelVoltageCtlStabled(int i);
        bool IsAllChannelVoltageCtlStable();
        double[] CurrentObjectVoltageCtl { get; }
        bool[] VoltageCtlStability { get; }

        //20200219 温度传感器系数
        void SetNTCSensorPara(int ch,
            double ValueTempU_DegC, double ValueRU_KOhm,   //DegC  KOhm
            double ValueTempM_DegC, double ValueRM_KOhm,
            double ValueTempL_DegC, double ValueRL_KOhm);

        void GetNTCSensorPara(int ch,
            out double ValueTempU_DegC, out double ValueRU_KOhm,
            out double ValueTempM_DegC, out double ValueRM_KOhm,
            out double ValueTempL_DegC, out double ValueRL_KOhm);

        //TEC极限电流电压
        void SetTECLimit(int ch,
            double CurrentValue_A, double VoltageValue_V);
        void GetTECLimit(int ch,
            out double CurrentValue_A, out double VoltageValue_V);

        //重启TEC
        void SavePara();

        //重启TEC
        void ResetDevice();
        void SetVolCtlUpperLimit(int ch, double voltage);
        void SetVolCtlOffset(int ch, float offset);
    }
}
