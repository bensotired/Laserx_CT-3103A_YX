using SolveWare_BurnInInstruments;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LX_BurnInSolution.Utilities;

namespace SolveWare_IO
{
    public class IOC_GUGAO_GNME601_1_EtherCAT : IOControllerBase 
    {
        public IOC_GUGAO_GNME601_1_EtherCAT(string name, string address, IInstrumentChassis chassis) : 
            base(name, address, chassis)
        {
        }

        public override IOBase CreateIOInstance(IOSetting setting)
        {
            var io = new IO_GUGAO_GNME601_1(setting);
            return io;
        }
   
        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            var grps = this._IOCollection.GroupBy((io) => new { io.IOSetting.SlaveNo, io.IOSetting.CardNo, io.IOSetting.IOType ,io.IOSetting.IsExtendIO});//3


            foreach (var grp in grps)
            {
                //得到指定的返回的32位的数据 包括output和intput
                var ioItem = grp.First();

                if (ioItem.IOSetting.IsExtendIO == false)
                {
                    uint pVal = 0;
                    string tabPrefix = "";
                    switch (ioItem.IOSetting.IOType)
                    {
                        case IOType.INPUT:
                            {
                                GUGAO_LIB.GTN_GetEcatAxisDI(ioItem.IOSetting.CardNo, ioItem.IOSetting.SlaveNo, out pVal);//125945372
                                tabPrefix = "INPUT";
                            }
                            break;
                        case IOType.OUTPUT:
                            {
                                GUGAO_LIB.GTN_GetEcatAxisDO(ioItem.IOSetting.CardNo, ioItem.IOSetting.SlaveNo, out pVal);
                                tabPrefix = "OUTPUT";
                            }
                            break;
                    }
 
                    foreach (var item in grp)
                    {
                        //TRUE代表与到了 绿色
                        item.Interation.IsActive = JuniorMath.IsBitEqualsOne(pVal, item.IOSetting.Bit);
                    }
                }
                else if (ioItem.IOSetting.IsExtendIO == true)
                {
                    //询问固高技术支持扩展io读取方法再更新
                    //以下方法不适用
 
                    ushort retByte = 2;
                    byte[] pVal = new byte[retByte];

                    string tabPrefix = "";
                    switch (ioItem.IOSetting.IOType)
                    {
                        case IOType.INPUT:
                            {
                                GUGAO_LIB.GT_GetGLinkDi(ioItem.IOSetting.SlaveNo, 0, out pVal[0], retByte);//125945372
 
                                tabPrefix = "INPUT";
                            }
                            break;
                        case IOType.OUTPUT:
                            {
                                GUGAO_LIB.GT_GetGLinkDo(ioItem.IOSetting.SlaveNo, 0, out pVal[0], retByte);
                        
                                tabPrefix = "OUTPUT";
                            }
                            break;
                    }
              
                    var merginVals = BitConverter.ToInt16(pVal,0);
        
                    foreach (var item in grp)
                    {
                        //TRUE代表与到了 绿色
                        item.Interation.IsActive = JuniorMath.IsBitEqualsOne(merginVals, item.IOSetting.Bit);
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
    }
}