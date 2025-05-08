using SolveWare_BurnInInstruments;
using System;
using System.Security.Cryptography;
using System.Threading;
using System.Linq;
using SolveWare_Motion;

namespace SolveWare_Analog
{
    public class Analog_LaserX_9078 : AnalogBase
    {
        public Analog_LaserX_9078(AnalogSetting setting) : base(setting)
        {
            //设置到默认的采样电阻
            var CardNo = setting.CardNo;
            var Bit = setting.Bit;
            var ResOhm = setting.Res_Ohm;


            if (LaserX_9078_Utilities.AnalogCard_ResList.ContainsKey(CardNo)==true)
            {
                LaserX_9078_Utilities.AnalogCard_ResList[CardNo][Bit] = ResOhm;
                LaserX_9078_Utilities.P9078_SetSenseRes(CardNo, Bit, ResOhm);
            }

        }

        public override void OutputValue(double val)
        {
            if (this._setting.AnalogType == AnalogType.ADC)//只有IOTYPE是output的时候才可以进行IO打开
            {
                return;
            }
            if (this._setting.IsExtendAnalog == false)
            {
                if (this._setting.AnalogType == AnalogType.ADC)//只有IOTYPE是output的时候才可以进行IO打开
                {
                    return;
                }
                if (this._setting.IsExtendAnalog == false)
                {
                    short ret = -1;
                    short carditem = this._setting.CardNo;
                    short axis = this._setting.SlaveNo;
                    short bitOffset = this._setting.Bit;
                    byte doBitValue = 0;

                    int rc = 0;
                    if (LaserX_9078_Utilities.CardIDList.Contains(carditem) &&
                        (0 <= bitOffset && bitOffset < LaserX_9078_Utilities.MOT_MAX_AIO))
                    {
                        rc = LaserX_9078_Utilities.P9078_MotionSetAout(carditem, bitOffset, doBitValue, 1);
                    }
                }
                else
                {
                    //雷赛卡没有接外接端子 外接段子需要外部用网线和扩展卡链接 然后进行拨码
                }
            }
            else
            {
                //雷赛卡没有接外接端子 外接段子需要外部用网线和扩展卡链接 然后进行拨码
            }
        }

        //public static double ReadAnalog_V(string ioName)
        //{
        //    AnalogName Name;
        //    try
        //    {
        //        Name = (AnalogName)Enum.Parse(typeof(AnalogName), ioName);
        //        return ReadAnalog_V_NoRealTime(Name);
        //    }
        //    catch
        //    {
        //        return 0;
        //    }
        //}

        //public static double ReadAnalog_V_NoRealTime(AnalogName analogName)
        //{
        //    string Name = analogName.ToString();
        //    if (AnalogList.ContainsKey(Name))
        //    {
        //        var card = AnalogList[Name];
        //        var analogItem = card.AMap.GetAnalogItem(analogName);
        //        if (analogItem == null)
        //        {
        //            LogManager.FormattedLog("Analog [{0}] is not found in AnalogCard [{1}] .", Name, card.Name);
        //        }
        //        double analogStatus = 0;
        //        if (analogItem.Type == AnalogType.AD)
        //        {
        //            analogStatus = card.ReadAD_mV_NoRealTime(analogItem.Bit);
        //        }
        //        else if (analogItem.Type == AnalogType.DA)
        //        {
        //            analogStatus = card.ReadDA_mV_NoRealTime(analogItem.Bit);
        //        }
        //        else
        //        {
        //            analogStatus = 0; //否则均为0;
        //        }
        //        return analogStatus;
        //    }
        //    return 0;
        //}
        //public static double ReadAnalog_V_NoRealTime(AnalogName analogName, int analogbit)
        //{
        //    string Name = analogName.ToString();
        //    if (AnalogList.ContainsKey(Name))
        //    {
        //        var card = AnalogList[Name];

        //        return card.ReadAD_mV_NoRealTime(analogbit + 1);

        //    }
        //    return 0;
        //}

        ////读即时数据
        //public static double ReadAnalog_mA_RealTime(AnalogName analogName)
        //{
        //    string Name = analogName.ToString();
        //    if (AnalogList.ContainsKey(Name))
        //    {
        //        var card = AnalogList[Name];
        //        var analogItem = card.AMap.GetAnalogItem(analogName);
        //        if (analogItem == null)
        //        {
        //            LogManager.FormattedLog("Analog [{0}] is not found in AnalogCard [{1}] .", Name, card.Name);
        //        }
        //        double analogStatus = 0;
        //        if (analogItem.Type == AnalogType.AD)
        //        {
        //            analogStatus = card.ReadAD_mA(analogItem.Bit);
        //        }
        //        else
        //        {
        //            analogStatus = 0; //否则均为0;
        //        }
        //        return analogStatus;
        //    }
        //    return 0;
        //}
        //public static double ReadAnalog_mA_RealTime(AnalogName analogName, int analogbit)
        //{
        //    string Name = analogName.ToString();
        //    if (AnalogList.ContainsKey(Name))
        //    {
        //        var card = AnalogList[Name];

        //        return card.ReadAD_mA(analogbit + 1);

        //    }
        //    return 0;
        //}
        /// <summary>
        /// 获得当前的电流范围
        /// </summary>
        /// <param name="dev"></param>
        /// <param name="index"></param>
        /// <param name="CurrentRange_mA"></param>
        /// <returns></returns>
        public static double GetCurrent_mA(Motor_LaserX_9078 Axis, int index)
        {

            return GetCurrent_mA(Axis.CardNo, index);
        }
        public static double GetCurrent_mA(int Card, int index)
        {
            double Voltage_V = 0;

            LaserX_9078_Utilities.P9078_MotionUpdate(Card);  //20241120
            LaserX_9078_Utilities.P9078_MotionGetAin(Card, index, ref Voltage_V);

            double Current_mA = Voltage_V * 1000 / LaserX_9078_Utilities.AnalogCard_ResList[Card][index];

            return Current_mA;



        }
        /// <summary>
        /// 获得当前的最大电流范围
        /// </summary>
        /// <param name="dev"></param>
        /// <param name="index"></param>
        /// <param name="CurrentRange_mA"></param>
        /// <returns></returns>
        public static double GetSenseCurrentRange_mA(Motor_LaserX_9078 Axis, int index)
        {
            return LaserX_9078_Utilities.P9078_GetSenseCurrentRange_mA(Axis.CardNo, index);
        }

        /// <summary>
        /// 设置当前的最大电流范围

        /// </summary>
        /// <param name="dev"></param>
        /// <param name="index"></param>
        /// <param name="CurrentRange_mA"></param>
        /// <returns></returns>
        public static int SetSenseCurrentRange_mA(Motor_LaserX_9078 Axis, int index, double CurrentRange_mA)
        {
            return LaserX_9078_Utilities.P9078_SetSenseCurrentRange_mA(Axis.CardNo, index, CurrentRange_mA);
        }
    }
}