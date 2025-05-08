using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin_Demo
{
    [Serializable]
    public class DelayTimeSettings
    {

        [DisplayName("上料盘取料真空开启后延时_ms")]

        [PropEditable(true)]
        public int DelayTime_1 { get; set; }

        [DisplayName("测试台放料真空关闭后延时_ms")]

        [PropEditable(true)]
        public int DelayTime_2 { get; set; }

        [DisplayName("触水点延时ms")]

        [PropEditable(true)]
        public int DelayTime_3 { get; set; }

        [DisplayName("测试台取料真空开启后延时_ms")]

        [PropEditable(true)]
        public int DelayTime_4 { get; set; }

        [DisplayName("下料盘放料真空关闭后延时_ms")]

        [PropEditable(true)]
        public int DelayTime_5 { get; set; }

        [DisplayName("DelayTime_6")]

        [PropEditable(true)]
        public int DelayTime_6 { get; set; }

        [DisplayName("DelayTime_7")]

        [PropEditable(true)]
        public int DelayTime_7 { get; set; }

        [DisplayName("DelayTime_8")]

        [PropEditable(true)]
        public int DelayTime_8 { get; set; }

        public DelayTimeSettings()
        {
            DelayTime_1 = 200;
            DelayTime_2 = 200;
            DelayTime_3 = 200;
            DelayTime_4 = 200;
            DelayTime_5 = 200;
            DelayTime_6 = 200;
            DelayTime_7 = 200;
            DelayTime_8 = 200;


        }

    }
}
