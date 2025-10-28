using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
 

namespace SolveWare_TestComponents.Data 
{
    public class QWLT2_TestData
    {
        public static QWLT2_TestData GetQwlt2_Data(IDeviceStreamDataBase dutStreamData)
        {
            var qWLT2_TestDta = new QWLT2_TestData();

            foreach (var dataMenu in dutStreamData.RawDataCollection)
            {
                if (dataMenu is IRawDataMenuCollection)
                {
                    var rawd = dataMenu as IRawDataMenuCollection;
                    var type = rawd.GetType();
                    if (type.Name == "RawDataMenu_QWLT2")
                    {
                        var props = rawd.GetType().GetProperties();
                        var broEleProps = PropHelper.GetAttributeProps<RawDataBrowsableElementAttribute>(props);
                        foreach (var bp in broEleProps)
                        {
                            if (bp.Name == "MIRROR1_mid_slope_val")
                            {
                                qWLT2_TestDta.MIRROR1 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "MIRROR2_mid_slope_val")
                            {
                                qWLT2_TestDta.MIRROR2 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "LP")
                            {
                                qWLT2_TestDta.LP = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "PH_Max_Sec_1")
                            {
                                qWLT2_TestDta.PH1 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "PH_Max_Sec_2")
                            {
                                qWLT2_TestDta.PH2 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "mPd1_V")
                            {
                                qWLT2_TestDta.MPD1 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "mPd2_V")
                            {
                                qWLT2_TestDta.MPD2 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "Bais1_V")
                            {
                                qWLT2_TestDta.BIAS1 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "Bais2_V")
                            {
                                qWLT2_TestDta.BIAS2 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "Gain_mA")
                            {
                                qWLT2_TestDta.GAIN = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "SOA1_mA")
                            {
                                qWLT2_TestDta.SOA1 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "SOA2_mA")
                            {
                                qWLT2_TestDta.SOA2 = (double)bp.GetValue(rawd);
                            }
                        }
                    }
                }
            }
            return qWLT2_TestDta;
        }
        public QWLT2_TestData()
        {
            GAIN = 120;
            SOA1 = 50;
            SOA2 = 40;
            MIRROR1 = 0;
            MIRROR2 = 0;
            PH1 = 1;
            PH2 = 0;
            LP = 4;
            MPD1 = -2.5;
            MPD2 = -2.5;
            BIAS1 = -2;
            BIAS2 = -2;
        }
        public double BIAS2 { get; set; }
        public double SOA1 { get; set; }
        public double SOA2 { get; set; }
        public double MIRROR2 { get; set; }
        public double LP { get; set; }
        public double MPD1 { get; set; }
        public double MIRROR1 { get; set; }
        public double BIAS1 { get; set; }
        public double MPD2 { get; set; }
        public double PH2 { get; set; }
        public double PH1 { get; set; }
        public double GAIN { get; set; }

    }
}
