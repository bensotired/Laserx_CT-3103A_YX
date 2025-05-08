using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public interface ITH2810D : IInstrumentBase
    {
        /// <summary>
        /// 设定TH2810D 的测试速度
        /// </summary>
        TH2810D_SPEED SPEED { get; set; }
        /// <summary>
        /// 设定测试结果的显示方式
        /// </summary>
        TH2810D_DISPlay DISPlay { get; set; }

        /// <summary>
        /// 设定测试信号源的频率
        /// </summary>
        TH2810D_FREQ FREQuency { get; set; }
        /// <summary>
        /// 设定主副被测参数的组合
        /// </summary>
        TH2810D_PARA PARAmeter { set; }
        string GetPARAmeter();
        /// <summary>
        /// 设定测试信号源的输出电压
        /// </summary>
        TH2810D_LEV LEVel { set; }
        string GetLEVel();

        /// <summary>
        /// 设定信号源的输出电阻
        /// </summary>
        TH2810D_SR SRESistor { set; }

        int GetSRESistor();

        /// <summary>
        /// 触发一次测量或设定触发方式
        /// </summary>
        TH2810D_TRIG TRIGger { set; }
        string GetTRIGger();

        /// <summary>
        /// 对不同的测试电压和信号源内阻下执行OPEN 或SHORt 清零操作
        /// </summary>
        TH2810D_CORR CORRection { set; }
        /// <summary>
        /// 比较功能
        /// </summary>
        bool IsCOMParator { get; set; }
        ///// <summary>
        ///// 打开比较功能
        ///// </summary>
        //void COMParator_ON();
        ///// <summary>
        ///// 关闭比较功能
        ///// </summary>
        //void COMParator_OFF();
        //string GetCOMParator();

        /// <summary>
        /// 设定被测件的等效电路方式
        /// </summary>
        TH2810D_EQU EQUivalent { set; }
        string GetEQUivalent();

        /// <summary>
        /// 设定量程选择方式或设定当前测试量程
        /// </summary>
        TH2810D_RANG RANGe { set; }
        string GetRANGe();
        /// <summary>
        /// 设定蜂鸣器的讯响状态
        /// </summary>
        TH2810D_ALAR ALARm { set; }
        string GetALARm();

        #region LIMit 子系统命令
        /// <summary>
        /// 查询返回最近一次主副参数的测试结果
        /// </summary>
        /// <returns></returns>
        string GetFETCh();
        /// <summary>
        /// 设定比较功能副参数的上限和下限值
        /// </summary>
        /// <param name="lowlimit"></param>
        /// <param name="highlimit"></param>
        void SECondary(double lowlimit, double highlimit);
        string GetSECondary();
        /// <summary>
        /// 设定比较功能各档的上下极限值
        /// </summary>
        /// <param name="lowlimit"></param>
        /// <param name="highlimit"></param>
        void BIN(int n,double lowlimit, double highlimit);
        string GetBIN(int n);
        /// <summary>
        /// 设定标称值
        /// </summary>
        string NOMinal_C { get; set; }

        string NOMinal_L { get; set; }

        string NOMinal_Z { get; set; }

        string NOMinal_R { get; set; }

        #endregion
    }
}
