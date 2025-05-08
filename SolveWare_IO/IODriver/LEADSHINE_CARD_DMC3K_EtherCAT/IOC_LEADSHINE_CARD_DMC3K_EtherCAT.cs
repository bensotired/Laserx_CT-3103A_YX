using LX_BurnInSolution.Utilities;
using SolveWare_BurnInInstruments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SolveWare_IO
{
    public class IOC_LEADSHINE_CARD_DMC3K_EtherCAT : IOControllerBase
    {
        public IOC_LEADSHINE_CARD_DMC3K_EtherCAT(string name, string address, IInstrumentChassis chassis) :
            base(name, address, chassis)
        {
        }
        public override IOBase CreateIOInstance(IOSetting setting)
        {
            var io = new IO_LEADSHINE_CARD_DMC3K(setting);
            return io;
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            var grps = this._IOCollection.GroupBy((io) => new { io.IOSetting.SlaveNo, io.IOSetting.CardNo, io.IOSetting.IOType, io.IOSetting.IsExtendIO });
            foreach (var grp in grps)
            {
                //得到指定的返回的32位的数据 包括output和intput
                var ioItem = grp.First();

                if (ioItem.IOSetting.IsExtendIO == false)
                {
                    uint pVal = 0;

                    switch (ioItem.IOSetting.IOType)
                    {
                        case IOType.INPUT:
                            {
                                //portno 保留参数默认值是0  IO组号取值范围是0,1 普通的输入是0  报警/原点IO是在1
                                ushort portno = 0;
                                pVal = LTDMC.dmc_read_inport((ushort)ioItem.IOSetting.CardNo, portno);
                            }
                            break;
                        case IOType.OUTPUT:
                            {
                                //portno 默认值固定是0
                                ushort portno = 0;
                                pVal = LTDMC.dmc_read_outport((ushort)ioItem.IOSetting.CardNo, portno);
                            }
                            break;
                    }

                    foreach (var item in grp)
                    {
                        //与到0 就是低电平 就是触发  与固高的不一样
                        item.Interation.IsActive = !JuniorMath.IsBitEqualsOne(pVal, item.IOSetting.Bit);
                    }
                }
                else if (ioItem.IOSetting.IsExtendIO == true)
                {
                    //雷赛3K系列的扩展卡没有使用呢
                    //所以现在先不考虑
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
            }
            while (true);
        }
        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
        }
    }
}