using SolveWare_BurnInInstruments;
using System.Collections.Generic;

namespace SolveWare_Motion
{
    public interface IMotionController : IInstrumentBase
    {
        void StopAxesReading();
        void ClearAxisInstances();
        MotorAxisBase CreateAxisInstance(MotorSetting setting );
        void CreateAxisInstances(MotorSettingCollection motorSettings );

        MotorAxisBase GetAxis(long id);
        MotorAxisBase GetAxis(string AxisName);
        MotorAxisBase GetAxis(short CardNo, short AxisNo);
        List<MotorAxisBase> GetAxesCollection();
        void AddAxisInstanceToCollection(MotorAxisBase axisInstance);

        List<MotorRuntimeInteration> AxesPositionMonitor { get; }

        void LoadSpecificConfig(object configOcj);
    }
}