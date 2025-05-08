using SolveWare_BurnInInstruments;
using System;
using System.Threading;

namespace SolveWare_IO
{
    public class IO_GUGAO_GNME601_1 : IOBase
    {
        public IO_GUGAO_GNME601_1(IOSetting setting) : base(setting)
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
                short ret = -1;
                short core = this._setting.CardNo;
                short axis = this._setting.SlaveNo;
                short bitOffset = this._setting.Bit;
                byte doBitValue = 0; ;

                if (isEnable)
                {
                    if (_setting.ActiveLogic == 1)
                    {
                        doBitValue = (byte)1;
                    }
                    else
                    {
                        doBitValue = (byte)0;
                    }
                }
                else
                {
                    if (_setting.ActiveLogic == 1)
                    {
                        doBitValue = (byte)0;
                    }
                    else
                    {
                        doBitValue = (byte)1;
                    }
                }

                GUGAO_LIB.GTN_SetEcatAxisDOBit(core, axis, bitOffset, doBitValue);
            }

            else
            {
                short ret = -1;
                short core = this._setting.CardNo;
                short axis = this._setting.SlaveNo;
                short bitOffset = this._setting.Bit;
                byte doBitValue = 0; ;

                if (isEnable)
                {
                    if (_setting.ActiveLogic == 1)
                    {
                        doBitValue = (byte)1;
                    }
                    else
                    {
                        doBitValue = (byte)0;
                    }
                }
                else
                {
                    if (_setting.ActiveLogic == 1)
                    {
                        doBitValue = (byte)0;
                    }
                    else
                    {
                        doBitValue = (byte)1;
                    }
                }
                //public static extern short GT_SetGLinkDoBit(short slaveno, short doIndex, byte value);
                GUGAO_LIB.GT_SetGLinkDoBit(this._setting.SlaveNo, bitOffset, doBitValue);
                //GUGAO_LIB.GTN_SetEcatAxisDOBit(core, axis, bitOffset, doBitValue);
            }

        }
    }
}