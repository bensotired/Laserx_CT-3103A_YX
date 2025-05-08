using SolveWare_BurnInInstruments;
using System.Collections.Generic;

namespace SolveWare_IO
{
    public interface IIOController:IInstrumentBase
    {
        void ClearIOInstances();
        IOBase CreateIOInstance(IOSetting setting);
        void CreateIOInstances(IOSettingCollection settingCollection);
        IOBase GetIO(long id);
        IOBase GetIO(string name);
        IOBase GetIO(short slaveNo, short bit, IOType iOType);
        List<IOBase> GetIOBaseCollection();
   

    }
}