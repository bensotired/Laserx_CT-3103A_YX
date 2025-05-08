using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SolveWare_BurnInInstruments
{
    public enum SourceMeterMode
    {
        SourceVoltageSenceCurrent,
        SourceCurrentSenceVoltage,
        Unknown
    }
    public enum Keithley2602BChannel
    {
        CHA,
        CHB
    }
    public enum SourceMeterSenceMode : int
    {
        SENSE_LOCAL = 0,//0 or smuX.SENSE_LOCAL: Selects local sense (2-wire) 
        SENSE_REMOTE = 1, //1 or smuX.SENSE_REMOTE: Selects remote sense (4-wire)
        SENSE_CALA = 3 //3 or smuX.SENSE_CALA: Selects calibration sense mode 
    }
    public enum SourceMeterFuncitonMode
    {
        Source,
        Measure
    }
    public enum SourceMeterAutoZero : int
    {
        AUTOZERO_OFF = 0,//0 or smuX.AUTOZERO_OFF: Autozero disabled 
        AUTOZERO_ONCE = 1, //1 or smuX.AUTOZERO_ONCE: Performs autozero once, then disables autozero 
        AUTOZERO_AUTO = 2 //2 or smuX.AUTOZERO_AUTO: Automatic checking of reference and zero measurements; 
    }


    public enum YOKOGAWASourceResponMode
    {
        Normal,
        Stable,
    }
    public enum YOKOGAWASourceShapeMode
    {
        DC,
        Pulse,
    }
    public enum YOKOGAWASourceWorkMode
    {
        FIX,//固定值 扫描关闭
        SWE,//线性或对数 扫描
        LIST,//可编程扫描
        SING//单步扫描
    }
    public enum YOKOGAWA_SWEEP_TriggerMode
    {
        EXT,   //外部启动
        AUX,   //辅助触发
        TIM1,  //Timer1触发
        TIM2,  //Timer2触发
        SENS   //测量结束触发
    }
    public enum YOKOGAWA_SOURCE_TriggerMode
    {
        EXT,   //外部启动
        AUX,   //辅助触发
        TIM1,  //Timer1触发
        TIM2,  //Timer2触发
        SENS   //测量结束触发
    }
    public enum YOKOGAWA_SENSE_TriggerMode
    {
        SOUR,   //源变化触发
        SWE,    //扫描结束触发  
        AUX,    //辅助触发
        TIM1,   //Timer1触发
        TIM2,   //Timer2触发
        IMM     //立即触发
    }
}