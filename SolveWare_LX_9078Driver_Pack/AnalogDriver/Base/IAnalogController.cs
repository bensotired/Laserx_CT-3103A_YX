using SolveWare_BurnInInstruments;
using System.Collections.Generic;

namespace SolveWare_Analog
{
    public interface IAnalogController : IInstrumentBase
    {
        void ClearIOInstances();

        AnalogBase CreateAnalogInstance(AnalogSetting setting);

        void CreateIOInstances(AnalogSettingCollection settingCollection);

        AnalogBase GetAnalog(long id);

        AnalogBase GetAnalog(string name);

        AnalogBase GetAnalog(short slaveNo, short bit, AnalogType iOType);

        List<AnalogBase> GetIOBaseCollection();
    }
}