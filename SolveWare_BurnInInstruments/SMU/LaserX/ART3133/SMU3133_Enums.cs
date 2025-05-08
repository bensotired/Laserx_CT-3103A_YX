using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{

    public enum ART_AI_TermCfg
    {
        //public const Int32 ArtDAQ_Val_Cfg_Default = -1;     //Default
        //public const Int32 ArtDAQ_Val_RSE = 10083; // RSE
        //public const Int32 ArtDAQ_Val_NRSE = 10078; // NRSE
        //public const Int32 ArtDAQ_Val_Diff = 10106; // Differential
        //public const Int32 ArtDAQ_Val_PseudoDiff = 12529; // Pseudodifferential

        Default = -1,
        Nrse = 10078,
        Rse = 10083,
        Differential = 10106,
        Pseudodifferential = 12529
    }

    public enum PDCurrentRange
    {
        档位1_20uA,//11   20ua
        档位2_200uA,//01   200ua
        档位3_2mA,//10   2ma
        档位4_20mA//00    20ma

    }

    public enum SMU3133AIChannel
    {  
        通道0,            //PD1_aiCh = 0;
        通道1,            //PD2_aiCh = 1;
        通道2,            //MPD1_aiCh = 2;
        通道3,            //MPD2_aiCh = 3;
        通道4,            //TEC1_aiCh = 4;
        通道5,            //TEC2_aiCh = 5;
        通道6,            //NTC1_aiCh = 6;
        通道7             //NTC2_aiCh = 7;
    }
}             
              