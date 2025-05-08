using NationalInstruments.DAQmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public class SMU3133Config
    {
        public SMU3133Config()
        {
            CardNo = 1;

            PD1_aiCh = 0;
            PD2_aiCh = 1;

            MPD1_aiCh = 2;
            MPD2_aiCh = 3;

            TEC1_aiCh = 4;
            TEC2_aiCh = 5;

            NTC1_aiCh = 6;
            NTC2_aiCh = 7;





            PD1_Range_doCh_1 = 6;
            PD1_Range_doCh_2 = 7;
            PD2_Range_doCh_1 = 4;
            PD2_Range_doCh_2 = 5;

            MPD1_Range_doCh_1 = 2;
            MPD1_Range_doCh_2 = 3;
            MPD2_Range_doCh_1 = 0;
            MPD2_Range_doCh_2 = 1;

            ArtPFICh = 0;

            PerPFICh = 4;

            AITerminal = ART_AI_TermCfg.Differential;
            SampleClockRate = 10 * 1000;
            Timeout_s = 10;
            Range = 5;
            Range_Tec = 10;
        }


        public int CardNo { get; set; }

        public int PD1_aiCh { get; set; }
        public int PD2_aiCh { get; set; }
        public int MPD1_aiCh { get; set; }
        public int MPD2_aiCh { get; set; }
        public int TEC1_aiCh { get; set; }
        public int TEC2_aiCh { get; set; }
        public int NTC1_aiCh { get; set; }
        public int NTC2_aiCh { get; set; }



        public int PD1_Range_doCh_1 { get; set; }
        public int PD1_Range_doCh_2 { get; set; }

        public int PD2_Range_doCh_1 { get; set; }
        public int PD2_Range_doCh_2 { get; set; }



        public int MPD1_Range_doCh_1 { get; set; }
        public int MPD1_Range_doCh_2 { get; set; }

        public int MPD2_Range_doCh_1 { get; set; }
        public int MPD2_Range_doCh_2 { get; set; }


        /// <summary>
        /// 接收源表触发端口
        /// </summary>
        public int ArtPFICh { get; set; }


        /// <summary>
        /// 接收电机脉冲触发端口
        /// </summary>
        public int PerPFICh { get; set; }


        public ART_AI_TermCfg AITerminal { get; set; }

        public int SampleClockRate { get; set; }

        public double Timeout_s { get; set; }


        public double Range { get; set; }

        public double Range_Tec { get; set; }

    }
}
