using SolveWare_BurnInInstruments;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_IO
{
    public abstract class IOControllerBase : InstrumentBase, IIOController
    {
        protected List<IOBase> _IOCollection { get; set; }
        public IOControllerBase(string name, string address, IInstrumentChassis chassis) : base(name, address, chassis)
        {
            _IOCollection = new List<IOBase>();
        }
        public virtual void ClearIOInstances()
        {
            _IOCollection?.Clear();
        }
        public virtual void CreateIOInstances(IOSettingCollection settingCollection)
        {
            _IOCollection?.Clear();
            foreach (var setting in settingCollection)
            {
                var ioInstance = CreateIOInstance(setting);
                _IOCollection.Add(ioInstance);
                Thread.Sleep(10);
            }
        }
        public abstract IOBase CreateIOInstance(IOSetting setting);

        public virtual IOBase GetIO(string name)
        {
            return this._IOCollection.FindLast(item => item.Name == name);
        }
        public virtual IOBase GetIO(long id)
        {
            return this._IOCollection.FindLast(item => item.IOSetting.ID == id);
        }
 
        //IO是根据卡和IO点以及名字来命名的，
        public virtual IOBase GetIO(short slaveNo, short bit, IOType iOType)
        {
            return this._IOCollection.FindLast(item => item.IOSetting.SlaveNo == slaveNo &&
                                                       item.IOSetting.Bit == bit &&
                                                       item.IOSetting.IOType== iOType);
        }
        protected virtual void EnableIOSimulation(bool isEnable)
        {
            _IOCollection.ForEach(item =>
            {
                item.Interation.IsSimulation = isEnable;
                Thread.Sleep(1);
            });
        }
 
        public virtual List<IOBase> GetIOBaseCollection()
        {
            return _IOCollection;
        }
    }
}