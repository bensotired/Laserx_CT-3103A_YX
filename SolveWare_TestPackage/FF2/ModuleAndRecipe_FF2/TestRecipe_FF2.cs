using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_FF2 : TestRecipeBase
    {
        public TestRecipe_FF2()
        {
            this.Current_A = 1;
            this.IsRightShort = true;
            this.ShortSpeed = 100;
            this.IsRightLong = true;
            this.LongSpeed = 100;
            this.ShortStartAngle = -30;
            this.ShortStopAngle = 30;
            this.ShortMaxCurrent = 0.1;
            this.LongStartAngle = -30;
            this.LongStopAngle = 30;
            this.LongMaxCurrent = 0.004;


        }

        [DisplayName("驱动电流")]
        [PropEditable(true)]
        public double Current_A { get; set; }
        [DisplayName("右短摆臂FarField")]
        [PropEditable(true)]
        public bool IsRightShort { get; set; }

        [DisplayName("右短摆臂速度")]
        [PropEditable(true)]
        public double ShortSpeed { get; set; }

        [DisplayName("右短摆臂开始角度")]
        [PropEditable(true)]
        public double ShortStartAngle { get; set; }
        [DisplayName("右短摆臂停止角度")]
        [PropEditable(true)]
        public double ShortStopAngle { get; set; }
        [DisplayName("右短摆臂设置当前的最大电流范围")]
        [PropEditable(true)]
        public double ShortMaxCurrent { get; set; }

        [DisplayName("右长摆臂FarField")]
        [PropEditable(true)]
        public bool IsRightLong { get; set; }

        [DisplayName("右长摆臂速度")]
        [PropEditable(true)]
        public double LongSpeed { get; set; }

        [DisplayName("右长摆臂开始角度")]
        [PropEditable(true)]
        public double LongStartAngle { get; set; }
        [DisplayName("右长摆臂停止角度")]
        [PropEditable(true)]
        public double LongStopAngle { get; set; }
        [DisplayName("右长摆臂设置当前的最大电流范围")]
        [PropEditable(true)]
        public double LongMaxCurrent { get; set; }
    }
}
