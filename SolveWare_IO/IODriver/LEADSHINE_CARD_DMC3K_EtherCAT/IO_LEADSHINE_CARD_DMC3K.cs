using SolveWare_BurnInInstruments;
using System;
using System.Threading;

namespace SolveWare_IO
{
    public class IO_LEADSHINE_CARD_DMC3K : IOBase
    {
        public IO_LEADSHINE_CARD_DMC3K(IOSetting setting) : base(setting)
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
                    short axis = this._setting.SlaveNo;
                    short bitOffset = this._setting.Bit;
                    byte doBitValue = 0;

                    if (isEnable)
                    {
                        doBitValue = (byte)0;
                    }
                    else
                    {
                        doBitValue = (byte)1;
                    }
                    LTDMC.dmc_write_outbit((ushort)core, (ushort)bitOffset, (ushort)doBitValue);
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