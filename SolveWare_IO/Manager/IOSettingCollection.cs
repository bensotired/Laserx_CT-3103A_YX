using SolveWare_BurnInCommon;
using System.Collections.Generic;
using System.Linq;

namespace SolveWare_IO
{

    public class IOSettingCollection : CURDBase<IOSetting>//, IEnumerable<MotorSetting>
    {

        public IOSettingCollection()
        {
            this.ItemCollection = new List<IOSetting>();

        }
        public virtual bool DeleteSingleItem(string name, short cardNo, short slaveNo, short bit, short logic, IOType ioType, bool isExtIo)
        {
            int index = this.ItemCollection.FindIndex
                (
                x =>
                x.Name == name &&
                x.CardNo == cardNo &&
                x.SlaveNo == slaveNo &&
                x.Bit == bit &&
                x.ActiveLogic == logic &&
                x.IOType == ioType &&
                 x.IsExtendIO == isExtIo
            );
            if (index < 0)
            {
                return false;
            }

            this.ItemCollection.RemoveAt(index);
            return true;
        }
        public bool IsExistedDupIO()
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
                                                        mst.IOType == item.IOType &&
                                                        mst.IsExtendIO == item.IsExtendIO
                                                    );
                if (dupItems?.Count >= 1)
                {
                    anyDup = true;
                }
            }
            return anyDup;
 
        }

        public bool isExistSameIO(IOSetting iOSetting)
        {
           return this.ItemCollection.Exists(mst =>
                                              mst.ID != iOSetting.ID &&
                                              mst.CardNo == iOSetting.CardNo &&
                                              mst.Bit == iOSetting.Bit &&
                                              mst.SlaveNo == iOSetting.SlaveNo &&
                                              mst.IOType == iOSetting.IOType &&
                                              mst.IsExtendIO == iOSetting.IsExtendIO
                                       );

        }
        public bool isExistSameIO(string name,short cardNo, short slaveNo, short bit, short ActiveLogic, IOType ioType ,bool isExtIO)
        {
            return this.ItemCollection.Exists(mst =>
                                               mst.Name == name &&
                                               mst.CardNo == cardNo &&
                                               mst.Bit == bit &&
                                               mst.SlaveNo == slaveNo &&
                                               mst.IOType == ioType &&
                                               mst.IsExtendIO == isExtIO

                                        );

        }
        public int Count()
        {
            return this.ItemCollection.Count();
        }
        public List<IOSetting> GetSettingsDuetoIOType(IOType ioType)
        {
            return this.ItemCollection.Where(Item=> Item.IOType== ioType).ToList();

        }
        //public IEnumerator<IOSetting> GetEnumerator()
        //{
        //    return this.ItemCollection.GetEnumerator();
        //}

       
    }
}