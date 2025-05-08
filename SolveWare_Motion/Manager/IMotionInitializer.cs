using SolveWare_BurnInInstruments;

namespace SolveWare_Motion
{
    public interface IMotionController : IInstrumentBase
    {
        void ClearAxisInstances();
        MotorAxisBase CreateAxisInstance(MotorSetting setting );
        void CreateAxisInstances(MotorSettingCollection motorSettings );

        MotorAxisBase GetAxis(long id);
        MotorAxisBase GetAxis(string AxisName);
        void EnableAxesSimulation(bool isEnable);
        void StopAxesStatusReading();
        void StartAxesStatusReading();
    }
}