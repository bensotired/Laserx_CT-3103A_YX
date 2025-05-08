using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_LIV : RawDataCollectionBase<RawDatumItem_LIV>
    {
        public RawData_LIV()
        {
            //DutName = string.Empty;
            //DutChannel = 0;
            //Temperature = 25;
        }

        //两者都用DutName，DutChannel
        //也可以只用一个。
        //调试模式：关注产品名，调试模式单通道，多产品
        //运行模式：多产品，多通道

        //[RawDataBrowsableElement]
        //public string DutName { get; set; }

        //[RawDataBrowsableElement]
        //public int DutChannel { get; set; }


        //[RawDataBrowsableElement]
        //public double Temperature { get; set; }

    }
}