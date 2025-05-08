//using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using System;
using System.ComponentModel;

namespace SolveWare_IO
{
    [Serializable]
    public class IOSetting : CURDItem, ICURDItem
    {
        public IOSetting()
        {
            //this.GetType().GetProperties();

            this.IsExtendIO = false;


        }
        //马达名称
        //[Category("Axis Table")]
        //[DisplayName("马达名称")]
        //[Description("Axis Name")]
        //[PropEditable(true)]
        //public string Name { get; set; }

        [DisplayName("控制卡号")]
        [Description("Card #No")]
        [PropEditable(true)]
        public short CardNo { get; set; }


        [DisplayName("分卡号")]
        [Description("Axis #No")]
        [PropEditable(true)]
        public short SlaveNo { get; set; }


        [DisplayName("端子号")]
        [Description("Bit #No")]
        [PropEditable(true)]
        public short Bit { get; set; }

        [DisplayName("有效信号(1/0)")]
        [Description("ActiveLogic")]
        [PropEditable(true)]
        public short ActiveLogic { get; set; }

        [DisplayName("类型(输入/输出)")]
        [Description("IOType")]
        [PropEditable(true)]
        public IOType IOType { get; set; }



        [DisplayName("有效信号(1/0)")]
        [Description("IsExtendIO")]
        [PropEditable(true)]
        public bool IsExtendIO { get; set; }


    }
}