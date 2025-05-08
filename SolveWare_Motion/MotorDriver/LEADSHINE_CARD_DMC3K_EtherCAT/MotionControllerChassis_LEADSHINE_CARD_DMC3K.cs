using SolveWare_BurnInInstruments;
using System;
using System.Windows.Forms;

namespace SolveWare_Motion
{
    //这个模块主要是如何进入卡  断开卡和连接等
    public class MotionControllerChassis_LEADSHINE_CARD_DMC3K : InstrumentChassisBase
    {
        public short CoreID { get; set; } = 1;
        LEADSHINE_BUS _BUS = default(LEADSHINE_BUS);
        //这个是干嘛的 需要研究清楚
        public MotionControllerChassis_LEADSHINE_CARD_DMC3K(string name, string resource, bool isOnline) : base(name, resource, isOnline)
        {
            try
            {
                var resArr = resource.Split(':');
                CoreID = Convert.ToInt16(resArr[1]);

                if (Enum.TryParse<LEADSHINE_BUS>(resArr[0], out _BUS))
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
        public override void Initialize(int timeout)//运动控制卡初始化
        {
            if (!IsOnline) return;
            try
            {
                string exMsg = string.Empty;

                //雷赛运动初始化运动控制卡
                short num = LTDMC.dmc_board_init();//获取卡数量
                if (num <= 0 || num > 8)
                {
                    exMsg = $"初始卡失败出错,一个机台控制卡限位数目为1-8,实际寻找到的卡的数目为:{num}";
                    throw new Exception($"{exMsg}!");
                }
                ushort _num = 0;
                ushort[] cardids = new ushort[8];
                uint[] cardtypes = new uint[8];

                //_num           返回初始化成功的卡数
                //cardtypes   返回控制卡固件类型 数组  类型为十六进制
                //cardids     返回控制卡硬件 ID 号 数组，卡号按从小到大顺序排列
                short res = LTDMC.dmc_get_CardInfList(ref _num, cardtypes, cardids);
                if (res != 0)
                {
                    MessageBox.Show("获取卡信息失败!");
                    throw new Exception("获取卡信息失败!");
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
        public override void ClearConnection() //清除连接？为什么清除  //中断通讯  关闭运动控制卡
        {
            try
            {
                LTDMC.dmc_board_close();
            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] ClearConnection error. Chassis resource = [{1}].[{2}]-[{3}]",
                                           this.Name, this.Resource, ex.Message, ex.StackTrace);
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