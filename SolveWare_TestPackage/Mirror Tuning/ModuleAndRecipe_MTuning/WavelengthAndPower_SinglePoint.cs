using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_TestPackage
{
    public class PhaseScanCurrentGroup
    {
        public double Phase_1_Current_mA { get; set; }
        public double Phase_2_Current_mA { get; set; }
        public PhaseScanCurrentGroup()
        {

        }
    }
    public class WavelengthAndPower_SinglePoint
    {
        public double Wavelength_nm { get; set; }
        public double Power_dbm { get; set; }
        public bool IsLucky { get; set; }
        public WavelengthAndPower_SinglePoint()
        {
            Wavelength_nm = 0.0;
            Power_dbm = 0.0;
            IsLucky = false;
        }

        public static WavelengthAndPower_SinglePoint Get_TRUE_BLACK_SPOT()
        {
            WavelengthAndPower_SinglePoint tbs = new WavelengthAndPower_SinglePoint()
            {
                Power_dbm = -99,
                Wavelength_nm = 100,
                IsLucky = false 
        };
            return tbs;
        }
    }
}
