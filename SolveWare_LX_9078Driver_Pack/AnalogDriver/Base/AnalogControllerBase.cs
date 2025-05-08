using SolveWare_BurnInInstruments;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_Analog
{
    public abstract class AnalogControllerBase : InstrumentBase, IAnalogController
    {
        protected List<AnalogBase> _AnalogCollection { get; set; }

        public AnalogControllerBase(string name, string address, IInstrumentChassis chassis) : base(name, address, chassis)
        {
            _AnalogCollection = new List<AnalogBase>();
        }

        public virtual void ClearIOInstances()
        {
            _AnalogCollection?.Clear();
        }

        public virtual void CreateIOInstances(AnalogSettingCollection settingCollection)
        {
            _AnalogCollection?.Clear();
            foreach (var setting in settingCollection)
            {
                var ioInstance = CreateAnalogInstance(setting);
                _AnalogCollection.Add(ioInstance);
                Thread.Sleep(10);
            }
        }

        public abstract AnalogBase CreateAnalogInstance(AnalogSetting setting);

        public virtual AnalogBase GetAnalog(string name)
        {
            return this._AnalogCollection.FindLast(item => item.Name == name);
        }

        public virtual AnalogBase GetAnalog(long id)
        {
            return this._AnalogCollection.FindLast(item => item.AnalogSetting.ID == id);
        }

        //IO是根据卡和IO点以及名字来命名的，
        public virtual AnalogBase GetAnalog(short slaveNo, short bit, AnalogType iOType)
        {
            return this._AnalogCollection.FindLast(item => item.AnalogSetting.SlaveNo == slaveNo &&
                                                       item.AnalogSetting.Bit == bit &&
                                                       item.AnalogSetting.AnalogType == iOType);
        }

        protected virtual void EnableIOSimulation(bool isEnable)
        {
            _AnalogCollection.ForEach(item =>
            {
                item.Interation.IsSimulation = isEnable;
                Thread.Sleep(1);
            });
        }

        public virtual List<AnalogBase> GetIOBaseCollection()
        {
            return _AnalogCollection;
        }
    }
}