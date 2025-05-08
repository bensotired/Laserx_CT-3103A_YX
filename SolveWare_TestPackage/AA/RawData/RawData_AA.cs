using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    public class RawData_AA : RawDataCollectionBase<RawDatumItem_AA>
    {
        public RawData_AA()
        {
            Period_ms = 0.0;

            EnableLD = false;
            Current_LD_mA = 0.0;
            EnableEA = false;
            Voltage_EA_V = 0.0;
            EnableSOA = false;
            Current_SOA_mA = 0.0;

            LastScanSlot = 0;
            Pmax_mA = 0.0;
            X_Pos_Pmax_mm = 0.0;
            Y_Pos_Pmax_mm = 0.0;
            Z_Pos_Pmax_mm = 0.0;
        }

        #region 主显参数

        [RawDataBrowsableElement]
        public int LastScanSlot { get; set; }

        [RawDataBrowsableElement]
        public double Pmax_mA { get; set; }

        [RawDataBrowsableElement]
        public double X_Pos_Pmax_mm { get; set; }

        [RawDataBrowsableElement]
        public double Y_Pos_Pmax_mm { get; set; }

        [RawDataBrowsableElement]
        public double Z_Pos_Pmax_mm { get; set; }

        #endregion

        #region 其他参数

        public double Period_ms { get; set; }
        public bool EnableLD { get; set; }
        public double Current_LD_mA { get; set; }
        public bool EnableSOA { get; set; }
        public double Current_SOA_mA { get; set; }
        public bool EnableEA { get; set; }
        public double Voltage_EA_V { get; set; }

        #endregion
    }
}