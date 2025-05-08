using LX_BurnInSolution.Utilities;
using SolveWare_BurnInInstruments;
using System;
using System.Linq;
using System.Threading;

namespace SolveWare_IO
{
    public class IOC_LaserX_9078 : IOControllerBase
    {
        public IOC_LaserX_9078(string name, string address, IInstrumentChassis chassis) :
            base(name, address, chassis)
        {
        }

        public override IOBase CreateIOInstance(IOSetting setting)
        {
            var io = new IO_LaserX_9078(setting);
            return io;
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            int rc = 0;
            string exMsg = "";

            var grps = this._IOCollection.GroupBy((io) => new { io.IOSetting.SlaveNo, io.IOSetting.CardNo, io.IOSetting.IOType, io.IOSetting.IsExtendIO });
            foreach (var grp in grps)
            {
                //得到指定的返回的32位的数据 包括output和intput
                var ioItem = grp.First();

                if (ioItem.IOSetting.IsExtendIO == false)
                {
                    uint pVal = 0;

                    int carditem = (ushort)ioItem.IOSetting.CardNo;

                    rc = SolveWare_BurnInInstruments.LaserX_9078_Utilities.P9078_MotionUpdate(carditem);
                    if (rc != 0)
                    {
                        exMsg = $"控制卡ID[{carditem}] P9078_MotionUpdate失败[{SolveWare_BurnInInstruments.LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                        //throw new Exception($"{exMsg}!");
                        break;
                    }

                    switch (ioItem.IOSetting.IOType)
                    {
                        case IOType.INPUT:
                            {
                                //portno 保留参数默认值是0  IO组号取值范围是0,1 普通的输入是0  报警/原点IO是在1
                                ushort portno = 0;
                                //数字量输入
                                rc = SolveWare_BurnInInstruments.LaserX_9078_Utilities.P9078_MotionGetDinEx(carditem, out pVal);
                                if (rc != 0)
                                {
                                    exMsg = $"控制卡ID[{carditem}] P9078_MotionGetDinEx读取失败[{SolveWare_BurnInInstruments.LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                                    //throw new Exception($"{exMsg}!");
                                    break;
                                }
                            }
                            break;

                        case IOType.OUTPUT:
                            {
                                //portno 默认值固定是0
                                ushort portno = 0;
                                //数字量输出
                                rc = SolveWare_BurnInInstruments.LaserX_9078_Utilities.P9078_MotionGetDoutEx(carditem, out pVal);
                                if (rc != 0)
                                {
                                    exMsg = $"控制卡ID[{carditem}] P9078_MotionGetDoutEx读取失败[{SolveWare_BurnInInstruments.LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                                    //throw new Exception($"{exMsg}!");
                                    break;
                                }
                            }
                            break;
                    }

                    foreach (var item in grp)
                    {
                        //与到0 就是低电平 就是触发  与固高的不一样
                        item.Interation.IsActive = _Update_ActiveLogic(pVal, item.IOSetting.Bit, item.IOSetting.ActiveLogic);
                    }
                }
                else if (ioItem.IOSetting.IsExtendIO == true)
                {
                    //扩展卡
                }
            }
        }

        //看不懂平台, 抄作业
        public bool _Update_ActiveLogic(uint decVal, int bitIndex, int ActiveLogic)
        {
            if (ActiveLogic == 1)
            {
                return !((decVal & (1 << (bitIndex))) != 0);
            }
            else
            {
                return (decVal & (1 << (bitIndex))) != 0;
            }
        }

        public override void RefreshDataLoop(CancellationToken token)
        {
            do
            {
                try
                {
                    this.RefreshDataOnceCycle(token);
                    Thread.Sleep(5);
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
                Thread.Sleep(5);
            }
            while (true);
        }

        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
        }
    }
}