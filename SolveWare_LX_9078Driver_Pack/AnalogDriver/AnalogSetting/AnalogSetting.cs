//using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using System;
using System.ComponentModel;

namespace SolveWare_Analog
{
    [Serializable]
    public class AnalogSetting : CURDItem, ICURDItem
    {
        public AnalogSetting()
        {
            //this.GetType().GetProperties();

            this.IsExtendAnalog = false;
            this.Res_Ohm = 100000;
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

        [DisplayName("通道号")]
        [Description("Bit #No")]
        [PropEditable(true)]
        public short Bit { get; set; }

        [DisplayName("有效信号(1/0)")]
        [Description("ActiveLogic")]
        [PropEditable(true)]
        public short ActiveLogic { get; set; }

        [DisplayName("类型(输入/输出)")]
        [Description("AnalogType")]
        [PropEditable(true)]
        public AnalogType AnalogType { get; set; }

        [DisplayName("初始采样电阻")]
        [Description("ResOhm")]
        [PropEditable(true)]
        public int Res_Ohm { get; set; }

        [DisplayName("有效信号(1/0)")]
        [Description("IsExtendAnalog")]
        [PropEditable(true)]
        public bool IsExtendAnalog { get; set; }


    }
}