using LX_BurnInSolution.Utilities;
using SolveWare_BurnInInstruments;
using System;
using System.Linq;
using System.Threading;

namespace SolveWare_Analog
{
    public class AnalogC_LaserX_9078 : AnalogControllerBase
    {
        public AnalogC_LaserX_9078(string name, string address, IInstrumentChassis chassis) :
            base(name, address, chassis)
        {
        }

        public override AnalogBase CreateAnalogInstance(AnalogSetting setting)
        {
            var io = new Analog_LaserX_9078(setting);
            return io;
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            int rc = 0;
            string exMsg = "";

            var grps = this._AnalogCollection.GroupBy((io) => new { io.AnalogSetting.SlaveNo, io.AnalogSetting.CardNo, io.AnalogSetting.AnalogType, io.AnalogSetting.IsExtendAnalog });
            foreach (var grp in grps)   //按卡分组
            {
                foreach (var ioItem in grp)
                {
                    if (ioItem.AnalogSetting.IsExtendAnalog == false)
                    {
                        double pVal = 0;

                        int carditem = (ushort)ioItem.AnalogSetting.CardNo;
                        int channelitem = (ushort)ioItem.AnalogSetting.Bit;    //通道号

                        rc = LaserX_9078_Utilities.P9078_MotionUpdate(carditem);
                        if (rc != 0)
                        {
                            exMsg = $"控制卡ID[{carditem}] P9078_MotionUpdate失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                            //throw new Exception($"{exMsg}!");
                            break;
                        }

                        switch (ioItem.AnalogSetting.AnalogType)
                        {
                            case AnalogType.ADC:
                                {
                                    //portno 保留参数默认值是0  IO组号取值范围是0,1 普通的输入是0  报警/原点IO是在1
                                    ushort portno = 0;
                                    //模拟量输入
                                    if (LaserX_9078_Utilities.CardIDList.Contains(carditem) &&
                                        (0 <= channelitem && channelitem < LaserX_9078_Utilities.MOT_MAX_AIO))
                                    {
                                        rc = LaserX_9078_Utilities.P9078_MotionGetAin(carditem, channelitem, ref pVal);
                                        if (rc != 0)
                                        {
                                            exMsg = $"控制卡ID[{carditem}] P9078_MotionGetAin读取失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                                            //throw new Exception($"{exMsg}!");
                                            break;
                                        }
                                    }
                                }
                                break;

                            case AnalogType.DAC:
                                {
                                    //portno 默认值固定是0
                                    ushort portno = 0;
                                    //数字量输出
                                    rc = LaserX_9078_Utilities.P9078_MotionGetAout(carditem, channelitem, ref pVal);
                                    if (rc != 0)
                                    {
                                        exMsg = $"控制卡ID[{carditem}] P9078_MotionGetDoutEx读取失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                                        //throw new Exception($"{exMsg}!");
                                        break;
                                    }
                                }
                                break;
                        }

                        //与到0 就是低电平 就是触发  与固高的不一样
                        ioItem.Interation.Value = Math.Round(pVal, 4);
                    }
                    else if (ioItem.AnalogSetting.IsExtendAnalog == true)
                    {
                        //扩展卡
                    }
                }
            }
        }

        public override void RefreshDataLoop(CancellationToken token)
        {
            do
            {
                try
                {
                    this.RefreshDataOnceCycle(token);
                    token.ThrowIfCancellationRequested();
                }
                catch (OperationCanceledException oce)
                {
                    string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis.Name}] resource [{this._chassis.Resource}]- RefreshDataLoop is cancelled.";
                    return;
                }
                catch (Exception ex)
                {
                    string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis.Name}] resource [{this._chassis.Resource}]- RefreshDataLoop exception:{ex.Message}-{ex.StackTrace}.";
                }
                Thread.Sleep(500);
            }
            while (true);
        }

        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
        }


    }
}