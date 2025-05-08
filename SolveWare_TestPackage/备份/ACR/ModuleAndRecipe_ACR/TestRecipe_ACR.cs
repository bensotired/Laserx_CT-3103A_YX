using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents;
using System.ComponentModel;
using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;

namespace SolveWare_TestPackage
{
    //[Recipe]
    public class TestRecipe_ACR : TestRecipeBase
    {

        public TestRecipe_ACR()
        {
            TRIGger = TH2810D_TRIG.INTernal;
            DISPlay = TH2810D_DISPlay.DIRect;
            SPEED = TH2810D_SPEED.FAST;
            SRESistor = TH2810D_SR.R_100;
            FREQuency = TH2810D_FREQ.Hz_120;
            PARAmeter = TH2810D_PARA.RQ;
            LEVel = TH2810D_LEV.V2;
            RANGe = TH2810D_RANG.AUTO;
            Timeinterval_s = 5;
        }
        

        [DisplayName("触发方式")]
        [Description("TRIGger")]
        [PropEditable(true)]
        public TH2810D_TRIG TRIGger { get; set; }


        [DisplayName("测试结果的显示方式")]
        [Description("DISPlay")]
        [PropEditable(true)]
        public TH2810D_DISPlay DISPlay { get; set; }


        [DisplayName("测试速度")]
        [Description("SPEED")]
        [PropEditable(true)]
        public TH2810D_SPEED SPEED { get; set; }


        [DisplayName("信号源的输出电阻")]
        [Description("SRESistor")]
        [PropEditable(true)]
        public TH2810D_SR SRESistor { get; set; }

        [DisplayName("测试信号源的频率")]
        [Description("FREQuency")]
        [PropEditable(true)]
        public TH2810D_FREQ FREQuency { get; set; }


        [DisplayName("主副被测参数的组合")]
        [Description("PARAmeter")]
        [PropEditable(true)]
        public TH2810D_PARA PARAmeter { get; set; }

        [DisplayName("测试信号源的输出电压")]
        [Description("LEVel")]
        [PropEditable(true)]
        public TH2810D_LEV LEVel { get; set; }

        [DisplayName("设定量程选择方式或设定当前测试量程")]
        [Description("RANGe")]
        [PropEditable(true)]
        public TH2810D_RANG RANGe { get; set; }
        [DisplayName("设定采样时长_秒")]
        [Description("Time")]
        [PropEditable(true)]
        public double Timeinterval_s { get; set; }

    }

}