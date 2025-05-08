using SolveWare_BurnInInstruments;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SolveWare_BurnInInstruments
{
    public class MOC_Chassis_LaserX_9078 : InstrumentChassisBase
    {
        enum ExecutionFlag : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001,
        };
        [DllImport("kernel32.dll")] static extern uint SetThreadExecutionState(ExecutionFlag flags);

        //public int CoreID { get; set; }

        public MOC_Chassis_LaserX_9078(string name, string resource, bool isOnline) : base(name, resource, isOnline)
        {
            try
            {
                //获取板卡的的ID
                //var resArr = resource.Split(':');
                //CoreID = Convert.ToInt16(resArr[1]);
            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] Constructor error, Chassis resource = [{1}].[{2}]-[{3}]", this.Name, this.Resource, ex.Message, ex.StackTrace);
                this.ReportException(msg);
                throw new Exception(msg, ex);
            }
        }

        public override void Initialize()
        {
            int timeout = 60 * 1000;
            Initialize(timeout);
        }

        public override void Initialize(int timeout)
        {
            if (!IsOnline) return;
            try
            {
                var PCIE_IDs = this.Resource.Split(new char[] { ':', ',', ' ' }); //PCIE:2

                int rc = 0;
                string exMsg = string.Empty;
                int[] IDs = new int[LaserX_9078_Utilities.MOT_MAX_DEVICE];
                int actCnt = 0;

                rc = LaserX_9078_Utilities.P9078_MotionGetDevIDs(IDs, IDs.Length, ref actCnt);
                if (rc != 0)
                {
                    exMsg = $"获取控制卡失败![{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                    throw new Exception($"{exMsg}!");
                }
                if (actCnt < 1)
                {
                    exMsg = $"未搜索到控制卡";
                    throw new Exception($"{exMsg}!");
                }

                //建立本机卡ID编号的列表
                LaserX_9078_Utilities.CardIDList = new int[actCnt];
                LaserX_9078_Utilities.ResIsAdjusted = new bool[actCnt];

                for (int i = 0; i < actCnt; i++)
                {
                    LaserX_9078_Utilities.CardIDList[i] = IDs[i];
                    LaserX_9078_Utilities.ResIsAdjusted[i] = false;
                }



                //直接进行初始化对应板卡
                foreach (var item in PCIE_IDs)
                {
                    int _t;
                    if (int.TryParse(item, out _t))
                    {
                        if (Array.IndexOf(LaserX_9078_Utilities.CardIDList, _t) < 0)
                        {
                            exMsg = $"不存在控制卡ID[{_t}]";
                            throw new Exception($"{exMsg}!");
                        }

                        rc = LaserX_9078_Utilities.P9078_MotionInit(_t);
                        if (rc != 0)
                        {
                            exMsg = $"控制卡ID[{_t}]初始化失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                            throw new Exception($"{exMsg}!");
                        }
                        

                        //填充10K的电阻
                        if (LaserX_9078_Utilities.AnalogCard_ResList.ContainsKey(_t) == false)
                        {
                            Dictionary<int, double> temp = new Dictionary<int, double>();
                            LaserX_9078_Utilities.AnalogCard_ResList.Add(_t, temp);
                            for (int i = 0; i < LaserX_9078_Utilities.MOT_MAX_AIO; i++)
                            {
                                LaserX_9078_Utilities.AnalogCard_ResList[_t].Add(i, 10000);
                            }
                        }
                    }
                }
                
                //2024-8-2 yxw 软件运行后禁止系统进入待机状态
                SetThreadExecutionState(ExecutionFlag.ES_CONTINUOUS | ExecutionFlag.ES_SYSTEM_REQUIRED | ExecutionFlag.ES_AWAYMODE_REQUIRED | ExecutionFlag.ES_DISPLAY_REQUIRED);


            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] Initialize error. Chassis resource = [{1}].[{2}]-[{3}]", this.Name, this.Resource, ex.Message, ex.StackTrace);
                this.ReportException(msg);
                throw new Exception(msg);
            }
        }

        public override void ClearConnection()
        {
            try
            {
                foreach (var item in LaserX_9078_Utilities.CardIDList)
                {
                    LaserX_9078_Utilities.P9078_MotionDeinit(item);
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] Initialize error. Chassis resource = [{1}].[{2}]-[{3}]", this.Name, this.Resource, ex.Message, ex.StackTrace);
                this.ReportException(msg);
                throw new Exception(msg);
            }
        }

        public override void BuildConnection(int timeout_ms)
        {
            this.Initialize(timeout_ms);
        }
    }
}