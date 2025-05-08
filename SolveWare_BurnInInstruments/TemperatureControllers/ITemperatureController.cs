using SolveWare_BurnInInstruments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public interface ITemperatureController: IInstrumentBase
    {
        void SetTargetObjectTemperature(int i, double val);
        void SetTargetObjectTemperatureEnable(int i, int enable);
        void SetAllChannelTemperatureControlEnable(bool isEnable);
        bool IsChannelTemperatureStabled(int i);
        void SetAllChannelTemperature(double Value);
        bool IsAllChannelTemperatureStable();
        float[] GetAllChannelTemperature();
        double[] CurrentObjectTemperature
        {
            get;
        }
        bool[] TemperatureStability
        {
            get;
        }
        TECValues[] GetAllReadings();
        double[] CurrentTEC_V
        {
            get;
        }
        double[] CurrentTEC_A
        {
            get;
        }
        double[] TargetObjectTemperature
        {
            get;
        }

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
        void SetTempUpperLimit(int ch, double temperature);
        void SetTempOffset(int ch, float offset);
    }
}
