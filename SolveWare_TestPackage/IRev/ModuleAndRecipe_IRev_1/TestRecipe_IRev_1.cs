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
    public class TestRecipe_IRev_1 : TestRecipeBase
    {
        public TestRecipe_IRev_1()
        {
            Start_voltage_V = -3;
            Step_voltage_V = 0.1;
            Stop_voltage_V = 3;
        }
        [DisplayName("驱动电压1")]
        [PropEditable(true)]
        public double Start_voltage_V { get; set; }
        [DisplayName("驱动电压2")]
        [PropEditable(true)]
        public double Step_voltage_V { get; set; }
        [DisplayName("驱动电压3")]
        [PropEditable(true)]
        public double Stop_voltage_V { get; set; }

    }
}


//start_voltage_V
//start_voltage_V
//start_voltage_V
