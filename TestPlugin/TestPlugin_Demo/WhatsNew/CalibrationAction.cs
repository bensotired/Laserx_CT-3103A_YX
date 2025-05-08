using SolveWare_BurnInCommon;
using SolveWare_TestComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin_Demo
{
    public sealed partial class TestPluginWorker_CT3103
    {

        public void ReCalibrateSummaryData(List<SummaryDatumItemBase> orgSummaryData, List<CalibrationItem> calDataList)
        {
            try
            {
                foreach (var calItem in calDataList)
                {
                    if (orgSummaryData.Exists(item => item.Name.ToLower() == calItem.Name.ToLower()) == true)
                    {
                        var index = orgSummaryData.FindIndex(item => item.Name.ToLower() == calItem.Name.ToLower());

                        orgSummaryData[index].Value = Convert.ToDouble(orgSummaryData[index].Value) * calItem.K + calItem.B;

                        if (calItem.Name.ToLower() == "vf1" || calItem.Name.ToLower() == "vf2" || calItem.Name.ToLower() == "vf3")
                        {
                            if (Convert.ToDouble(orgSummaryData[index].Value) < 0)
                            {
                                orgSummaryData[index].Value = 0;
                            }
                        }
                    }
                }
            }

            catch { }

        }








    }
}
