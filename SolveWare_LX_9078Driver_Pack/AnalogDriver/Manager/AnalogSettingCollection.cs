using SolveWare_BurnInCommon;
using System.Collections.Generic;
using System.Linq;

namespace SolveWare_Analog
{
    public class AnalogSettingCollection : CURDBase<AnalogSetting>//, IEnumerable<MotorSetting>
    {
        public AnalogSettingCollection()
        {
            this.ItemCollection = new List<AnalogSetting>();
        }

        public virtual bool DeleteSingleItem(string name, short cardNo, short slaveNo, short bit, short logic, AnalogType ioType, bool isExtIo)
        {
            int index = this.ItemCollection.FindIndex
                (
                x =>
                x.Name == name &&
                x.CardNo == cardNo &&
                x.SlaveNo == slaveNo &&
                x.Bit == bit &&
                x.ActiveLogic == logic &&
                x.AnalogType == ioType &&
                 x.IsExtendAnalog == isExtIo
            );
            if (index < 0)
            {
                return false;
            }

            this.ItemCollection.RemoveAt(index);
            return true;
        }

        public bool IsExistedDupAnalog()
        {
            bool anyDup = false;
            foreach (var item in this.ItemCollection)
            {
                var dupItems = this.ItemCollection.FindAll
                                                    (
                                                        mst =>
                                                        mst.ID != item.ID &&
                                                        mst.CardNo == item.CardNo &&
                                                        mst.Bit == item.Bit &&
                                                        mst.SlaveNo == item.SlaveNo &&
                                                        mst.AnalogType == item.AnalogType &&
                                                        mst.IsExtendAnalog == item.IsExtendAnalog
                                                    );
                if (dupItems?.Count >= 1)
                {
                    anyDup = true;
                }
            }
            return anyDup;
        }

        public bool isExistSameAnalog(AnalogSetting iOSetting)
        {
            return this.ItemCollection.Exists(mst =>
                                               mst.ID != iOSetting.ID &&
                                               mst.CardNo == iOSetting.CardNo &&
                                               mst.Bit == iOSetting.Bit &&
                                               mst.SlaveNo == iOSetting.SlaveNo &&
                                               mst.AnalogType == iOSetting.AnalogType &&
                                               mst.IsExtendAnalog == iOSetting.IsExtendAnalog
                                        );
        }

        public bool isExistSameAnalog(string name, short cardNo, short slaveNo, short bit, short ActiveLogic, AnalogType ioType, bool isExtIO)
        {
            return this.ItemCollection.Exists(mst =>
                                               mst.Name == name &&
                                               mst.CardNo == cardNo &&
                                               mst.Bit == bit &&
                                               mst.SlaveNo == slaveNo &&
                                               mst.AnalogType == ioType &&
                                               mst.IsExtendAnalog == isExtIO

                                        );
        }

        public int Count()
        {
            return this.ItemCollection.Count();
        }

        public List<AnalogSetting> GetSettingsDuetoAnalogType(AnalogType ioType)
        {
            return this.ItemCollection.Where(Item => Item.AnalogType == ioType).ToList();
        }

        //public IEnumerator<IOSetting> GetEnumerator()
        //{
        //    return this.ItemCollection.GetEnumerator();
        //}
    }
}