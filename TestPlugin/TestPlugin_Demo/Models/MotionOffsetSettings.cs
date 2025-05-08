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
    public class MotionOffsetSettings
    {
        [DisplayName("Motion_1_Offset(单位：mm) 左：- 右：+")]
        [PropEditable(true)]
        public double Motion_1_Offset { get; set; } = 0;

        [DisplayName("Motion_2_Offset(单位：mm) 左：- 右：+")]
        [PropEditable(true)]
        public double Motion_2_Offset { get; set; } = 0;
        
        [DisplayName("Motion_3_Offset(单位：mm) 左：- 右：+")]
        [PropEditable(true)]
        public double Motion_3_Offset { get; set; } = 0;

    }
}
