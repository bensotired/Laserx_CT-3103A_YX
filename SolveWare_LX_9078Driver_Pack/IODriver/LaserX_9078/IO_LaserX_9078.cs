using SolveWare_BurnInInstruments;
using System;
using System.Security.Cryptography;
using System.Threading;

namespace SolveWare_IO
{
    public class IO_LaserX_9078 : IOBase
    {
        public IO_LaserX_9078(IOSetting setting) : base(setting)
        {
        }

        public override void TurnOn(bool isEnable)
        {
            if (this._setting.IOType == IOType.INPUT)//只有IOTYPE是output的时候才可以进行IO打开
            {
                return;
            }
            if (this._setting.IsExtendIO == false)
            {
                if (this._setting.IOType == IOType.INPUT)//只有IOTYPE是output的时候才可以进行IO打开
                {
                    return;
                }
                if (this._setting.IsExtendIO == false)
                {
                    short ret = -1;
                    short core = this._setting.CardNo;
                    short axis = (short)this._setting.SlaveNo;
                    short bitOffset = this._setting.Bit;
                    byte doBitValue = 0;

                    if (isEnable)
                    {
                        if (_setting.ActiveLogic == 1)
                        {
                            doBitValue = (byte)0;
                        }
                        else
                        {
                            doBitValue = (byte)1;
                        }
                        //doBitValue = (byte)1;
                    }
                    else
                    {
                        if (_setting.ActiveLogic == 1)
                        {
                            doBitValue = (byte)1;
                        }
                        else
                        {
                            doBitValue = (byte)0;
                        }
                        //doBitValue = (byte)0;
                    }
                    SolveWare_BurnInInstruments.LaserX_9078_Utilities.P9078_MotionSetDout(core, bitOffset, doBitValue, 1);
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
    }
}