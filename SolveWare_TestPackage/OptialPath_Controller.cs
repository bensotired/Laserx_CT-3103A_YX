using SolveWare_BurnInInstruments;
using SolveWare_IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_TestPackage
{
    public class Circuit_Controller
    {
        public static void TapPD_ConnectTo(IOBase switchPD_IO, TapPD_Circuit powerReader)
        {
            if (switchPD_IO != null)
            {
                switch (powerReader)
                {
                    case SolveWare_TestPackage.TapPD_Circuit.AlignmentSystem:
                        {
                            switchPD_IO.TurnOn(false);
                        }
                        break;
                    case SolveWare_TestPackage.TapPD_Circuit.SMU:
                        {
                            switchPD_IO.TurnOn(true);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
    public class OptialPath_Controller
    {
     
        public static void SwitchTo(OpticalSwitch opticalSwitch, OptialPath optialPath)
        {
            if (opticalSwitch != null &&
                opticalSwitch.IsOnline == true)
            {
                if (opticalSwitch.SetCH((byte)optialPath) == false)
                {
                    string msg = $"光开关通道切换到[{optialPath}]失败！";
                    throw new Exception(msg);
                }
            }
        }
    }
}