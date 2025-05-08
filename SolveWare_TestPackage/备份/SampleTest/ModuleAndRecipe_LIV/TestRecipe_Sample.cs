using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System.ComponentModel;
     //this.EAVoltage_V = 0;
     //       this.complianceVoltage_V = 2.5;
     //       this.EAComplianceCurrent_mA = 150;
     //       this.PDBiasVoltage_V = 0;
     //       this.PDComplianceCurrent_mA = 20;
     //       this.K2400_NPLC = 0;
     //       this.MPDBiasVoltage = 0;
     //       this.MPDComplianceCurrent_mA = 1;
namespace SolveWare_TestPackage
{
    public class TestRecipe_Sample : TestRecipeBase
    {

        public TestRecipe_Sample()
        {
            this.I_Start_mA = 0.0;
            this.I_Stop_mA = 100.0;
            this.I_Step_mA = 1.0;
            this.EnableEAOutput = false;
            this.ReadMPD = false;
        }
 
        [DisplayName("扫描起始电流(mA)")]
      
        [PropEditable(true)]
        public double I_Start_mA { get; set; }
 
        [DisplayName("扫描结束电流(mA)")]
       
        [PropEditable(true)]
        public double I_Stop_mA { get; set; }
 
        [DisplayName("扫描步进电流(mA)")]
      
        [PropEditable(true)]
        public double I_Step_mA { get; set; }


        [DisplayName("是否启用EA输出")]
   
        [PropEditable(true)]
        public bool EnableEAOutput { get; set; }


        [DisplayName("EA电压")]
 
        [PropEditable(true)]
        public double EAVoltage_V { get; set; }

        [DisplayName("限制电压")]
 
        [PropEditable(true)]
        public double complianceVoltage_V { get; set; }

        [DisplayName("EA限制电流")]
        
        [PropEditable(true)]
        public double EAComplianceCurrent_mA { get; set; }

        [DisplayName("PD偏置电压")]
       
        [PropEditable(true)]
        public double PDBiasVoltage_V { get; set; }

        [DisplayName("PD限制电流")]
      
        [PropEditable(true)]
        public double PDComplianceCurrent_mA { get; set; }

        [DisplayName("交流电一个周期是50HZ 1代表一个完整的1HZ  为两点间采集时间2ms 可配置1,0.1,0.01")]
 
        [PropEditable(true)]
        public double K2400_NPLC { get; set; }


        [DisplayName("是否读取背光电流")]
 
        [PropEditable(true)]
        public bool ReadMPD { get; set; }

        [DisplayName("MPDBias电压")]
 
        [PropEditable(true)]
        public double MPDBiasVoltage { get; set; }

        [DisplayName("MPD限定电流")]
 
        [PropEditable(true)]
        public double MPDComplianceCurrent_mA { get; set; }

    }
}