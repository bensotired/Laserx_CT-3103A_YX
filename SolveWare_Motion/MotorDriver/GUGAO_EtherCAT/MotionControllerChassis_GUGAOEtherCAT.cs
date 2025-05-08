using SolveWare_BurnInInstruments;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_Motion
{
    public class MotionControllerChassis_GUGAOEtherCAT : InstrumentChassisBase
    {
        public short CoreID { get; set; } = 1;
        const int GTN_OPEN_PRARM = 1;
        GUGAO_BUS _BUS = default(GUGAO_BUS);
        public MotionControllerChassis_GUGAOEtherCAT(string name, string resource, bool isOnline) : base(name, resource, isOnline)
        {
            try
            {
                var resArr = resource.Split(':');
                CoreID = Convert.ToInt16(resArr[1]);

                if (Enum.TryParse<GUGAO_BUS>(resArr[0], out _BUS))
                {

                }
                else
                {
                    throw new Exception();
                }
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
            Initialize(60 * 1000);
        }
        public override void Initialize(int timeout)
        {
            if (!IsOnline) return;
            try
            {
                short ret = -1;
                short sEcatSts = -1;
                // 打开运动控制器
                ret = GUGAO_LIB.GTN_Open((short)_BUS, GTN_OPEN_PRARM);

                ThrowIfFeedbackError(ret, $"GUGAO Motion GTN_Open error!");
                // 初始化EtherCAT总线
                ret = GUGAO_LIB.GTN_InitEcatComm(CoreID);

                ThrowIfFeedbackError(ret, $"GUGAO Motion GTN_InitEcatComm error!");
                Stopwatch sw = new Stopwatch();
                sw.Start();
                // 等待EtherCAT总线初始化成功
                do
                {
                    if (sw.Elapsed.TotalMilliseconds > timeout)
                    {
                        throw new Exception("GUGAO Motion GTN_IsEcatReady error!");
                    }
                    // 读取EtherCAT总线状态
                    ret = GUGAO_LIB.GTN_IsEcatReady(CoreID, out sEcatSts);
                    Thread.Sleep(10);
                } while (sEcatSts != 1);

                sw.Stop();
                // 启动EtherCAT通讯
                ret = GUGAO_LIB.GTN_StartEcatComm(CoreID);
                ThrowIfFeedbackError(ret, $"GUGAO Motion GTN_StartEcatComm error!");
                // 系统复位
                ret = GUGAO_LIB.GTN_Reset(CoreID);
                ThrowIfFeedbackError(ret, $"GUGAO Motion GTN_Reset error!");
                ret = GUGAO_LIB.GT_GLinkDeInit(0);//Glink模块初始化前调用，断开通讯连接
                ThrowIfFeedbackError(ret, $"GUGAO Motion GT_GLinkDeInit error!");
                ret = GUGAO_LIB.GT_GLinkInitEx(0, (short)EXT_IO_CTRL_MODE.保持原有状态_DSP);
                ThrowIfFeedbackError(ret, $"GUGAO Motion GT_GLinkInitEx error!");

                byte slavenum = 0;

                if (ret == 0)
                {
                    ret = GUGAO_LIB.GT_GetGLinkOnlineSlaveNum(out slavenum);
                    ThrowIfFeedbackError(ret, $"GUGAO Motion GT_GetGLinkOnlineSlaveNum error!");
                    if (slavenum != 1)
                    {
                        throw new Exception("GUGAO Extenal IO controller communication error!");
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] Initialize error. Chassis resource = [{1}].[{2}]-[{3}]",
                                           this.Name, this.Resource, ex.Message, ex.StackTrace);
                this.ReportException(msg);
                throw new Exception(msg);
            }
        }
        public override void ClearConnection()
        {
            if (!this.IsOnline) { return; }
            try
            {
                short ret = -1;
                // 中断EtherCAT通讯
                ret = GUGAO_LIB.GTN_TerminateEcatComm(CoreID);
                ThrowIfFeedbackError(ret, $"GUGAO Motion GTN_TerminateEcatComm error!");
                // 关闭运动控制器
                ret = GUGAO_LIB.GTN_Close();
                ThrowIfFeedbackError(ret, $"GUGAO Motion GTN_Close error!");
            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] Clear connection fails. Chassis resource = [{1}].[{2}]-[{3}]",
                                            this.Name, this.Resource, ex.Message, ex.StackTrace);
                this.ReportException(msg);
            }
        }
        public override void BuildConnection(int timeout_ms)
        {
            if (!IsOnline) return;
            try
            {
                short ret = -1;
                short sEcatSts = -1;
                // 打开运动控制器
                ret = GUGAO_LIB.GTN_Open((short)_BUS, GTN_OPEN_PRARM);

                ThrowIfFeedbackError(ret, $"GUGAO Motion GTN_Open error!");
                // 初始化EtherCAT总线
                ret = GUGAO_LIB.GTN_InitEcatComm(CoreID);

                ThrowIfFeedbackError(ret, $"GUGAO Motion GTN_InitEcatComm error!");
                Stopwatch sw = new Stopwatch();
                sw.Start();
                // 等待EtherCAT总线初始化成功
                do
                {
                    if (sw.Elapsed.TotalMilliseconds > timeout_ms)
                    {
                        throw new Exception("GUGAO Motion GTN_IsEcatReady error!");
                    }
                    // 读取EtherCAT总线状态
                    ret = GUGAO_LIB.GTN_IsEcatReady(CoreID, out sEcatSts);
                    Thread.Sleep(10);
                } while (sEcatSts != 1);

                sw.Stop();
                // 启动EtherCAT通讯
                ret = GUGAO_LIB.GTN_StartEcatComm(CoreID);
                ThrowIfFeedbackError(ret, $"GUGAO Motion GTN_StartEcatComm error!");
                // 系统复位
                ret = GUGAO_LIB.GTN_Reset(CoreID);
                ThrowIfFeedbackError(ret, $"GUGAO Motion GTN_Reset error!");

                ret = GUGAO_LIB.GT_GLinkDeInit(0);//Glink模块初始化前调用，断开通讯连接
                ThrowIfFeedbackError(ret, $"GUGAO Motion GT_GLinkDeInit error!");
                ret = GUGAO_LIB.GT_GLinkInitEx(0, (short)EXT_IO_CTRL_MODE.保持原有状态_DSP);
                ThrowIfFeedbackError(ret, $"GUGAO Motion GT_GLinkInitEx error!");
            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] BuildConnection error. Chassis resource = [{1}].[{2}]-[{3}]",
                                           this.Name, this.Resource, ex.Message, ex.StackTrace);
                this.ReportException(msg);

            }
        }
        private void ThrowIfFeedbackError(short feedback, string exMsg)
        {
            if (feedback != 0)
            {
                throw new Exception($"{exMsg}!");
            }
        }
    }
}