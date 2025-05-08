using NationalInstruments.DAQmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public class SMUConfiguration
    {
        public SMUConfiguration()
        {
            DeviceID = 1;
            LD_Current_Drive_Channel = 0;
            LD_Current_Sense_Channel = 0;
            LD_Voltage_Sense_Channel = 1;
            PD_Current_Sense_Channel = 2;
            MPD_Current_Sense_Channel = 3;
            LD_Current_Source_K = 1;
            LD_Current_Sense_K = 1;
            LD_Voltage_Sense_K = 1;
            PD_Current_Sense_K = 1;
            AOSampleClockRate = 100000;
            AISampleClockRate = 100000;
        }
        public int DeviceID { get; set; }
        public int LD_Current_Drive_Channel { get; set; }
        public double LD_Current_Source_Range { get; set; }
        public double LD_Current_Source_K { get; set; }
        public double LD_Current_Source_B { get; set; }


        public AITerminalConfiguration LD_Current_Sense_Channel_Type { get; set; }
        public int LD_Current_Sense_Channel { get; set; }
        public double LD_Current_Sense_Range { get; set; }
        public double LD_Current_Sense_K { get; set; }
        public double LD_Current_Sense_B { get; set; }


        public AITerminalConfiguration LD_Voltage_Sense_Channel_Type { get; set; }
        public int LD_Voltage_Sense_Channel { get; set; }
        public double LD_Voltage_Sense_Range { get; set; }
        public double LD_Voltage_Sense_K { get; set; }
        public double LD_Voltage_Sense_B { get; set; }



        public AITerminalConfiguration PD_Current_Sense_Channel_Type { get; set; }
        public int PD_Current_Sense_Channel { get; set; }
        public double PD_Current_Sense_Range { get; set; }
        public double PD_Current_Sense_K { get; set; }
        public double PD_Current_Sense_B { get; set; }

        public AITerminalConfiguration MPD_Current_Sense_Channel_Type { get; set; }
        public int MPD_Current_Sense_Channel { get; set; }
        public double MPD_Current_Sense_Range { get; set; }
        public double MPD_Current_Sense_K { get; set; }
        public double MPD_Current_Sense_B { get; set; }

        public int Enable_Output_IOChannel { get; set; }
        public int Discharge_IOChannel { get; set; }
        public int ExternalSourceSwitch_IOChannel { get; set; }
        public int ExternalTrigger_PFIChannel { get; set; }


        public double AOSampleClockRate { get; set; }
        public double AISampleClockRate { get; set; }

    }
}
