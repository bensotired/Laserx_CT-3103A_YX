using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_Motion;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.InteropServices;
using LX_BurnInSolution.Utilities;

namespace SolveWare_TestPackage
{
    public partial class LaserX_9078_Cmp_Function
    {

        protected static void Log_Global(string log)
        {
            //this._core?.Log_Global($"{this.Name} {log}");
            //this._core?.Log_Global($"[{this.Name} @ {this._exeInteration.Name}] {log}.");
        }


        Motor_LaserX_9078 objAxis;
        int CardNo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Traj_Axis">轴</param>
        /// <param name="OutputNo">输出点(0->7)</param>
        /// <param name="SignLevel">true:上升沿 false:下降沿</param>
        /// <param name="keep_us">信号维持时间</param>
        public LaserX_9078_Cmp_Function(Motor_LaserX_9078 Traj_Axis, int OutputNo, bool SignLevel, double keep_us)
        {

            if (Traj_Axis.GetType() != typeof(Motor_LaserX_9078))
            {
                throw new Exception(string.Format("位置比较功能, 仅支持LaserX_9078类型的轴"));
            }
            if (OutputNo < 0 || OutputNo > 7)
            {
                throw new Exception(string.Format("脉冲输出点设定错误, 仅支持Out 0 -> 7"));
            }


            try
            {
                if (th_AddPos != null)
                {
                    th_AddPos.Abort();
                }
            }
            catch (Exception ex)
            {

            }
            


            objAxis = null;

            objAxis = Traj_Axis as Motor_LaserX_9078;

            CardNo = objAxis.CardNo;

            //信号
            int level = 0;  //下降电平
            if (SignLevel)
            {
                //上升电平
                level = 1;
            }


            int keep_ns = (int)(keep_us * 1000);
            if (keep_ns < 1000) keep_ns = 1000;
            if (keep_ns > 2000000) keep_ns = 2000000;

            //禁止位置比较器(仅支持带编码器的轴)
            int rc = LaserX_9078_Utilities.P9078_CmpEnable(CardNo, 0, 0, level, objAxis.AxisNo, keep_ns);
            if (rc != 0)
            {
                throw new Exception(string.Format("ERROR: P9078_CmpEnable({0}） fails({1})\n", CardNo, rc));
            }

            Thread.Sleep(30);

            //使能位置比较器
            rc = LaserX_9078_Utilities.P9078_CmpEnable(CardNo, 0, 1, level, objAxis.AxisNo, keep_ns);
            if (rc != 0)
            {
                throw new Exception(string.Format("ERROR: P9078_CmpEnable({0}） fails({1})\n", CardNo, rc));
            }

            Thread.Sleep(30);

            #region 设定输出点
            //读出寄存器
            uint reg = 0;
            rc = LaserX_9078_Utilities.P9078_MotionInd(CardNo, 0x1820, ref reg);
            if (rc != 0)
            {
                throw new Exception(string.Format("ERROR: P9078_MotionInd({0}） fails({1})\n", CardNo, rc));
            }

            reg &= 0xfffff0e3;   //清除需要用的3位
            reg |= ((uint)objAxis.AxisNo << 2) & 0x1C;  //设定轴号


            reg &= 0xfffff0ff;   //清除需要用的4位
            reg |= (1 << 11);   //使能比较器输出口
            reg |= (uint)(OutputNo << 8); //设定输出口

            uint reg2 = 0;
            rc = LaserX_9078_Utilities.P9078_MotionInd(CardNo, 0x1820, ref reg2);


            //回写寄存器
            rc = LaserX_9078_Utilities.P9078_MotionOutd(CardNo, 0x1820, reg);
            if (rc != 0)
            {
                throw new Exception(string.Format("ERROR: P9078_MotionOutd({0}） fails({1})\n", CardNo, rc));
            }

            #endregion

        }

        public void LaserX_9078_CmpAxis_Close()
        {
            LaserX_9078_Utilities.MOT_CMP_STAT_EX cmpStatEx = new LaserX_9078_Utilities.MOT_CMP_STAT_EX();

            var rc = LaserX_9078_Utilities.P9078_CmpGetStatusEx(CardNo, 0, ref cmpStatEx);



            //if (objAxis == null) return;

            //将控制轴从插补规划器解除绑定
            LaserX_9078_Utilities.P9078_CmpEnable(CardNo, 0, 0, 0, 0, 1000);

            Thread.Sleep(30);

            objAxis = null;
            CmpPosition = null;
        }

        //比较位置
        private Queue<double> qCmpPos = new Queue<double>();

        private List<double> CmpPosition = null;



        private double ManualMinVelocity;
        private double ManualMaxVelocity;
        private double ManualAcc;


        //开一个任务, 
        private Thread th_AddPos = null;


        private void _AddPosFun()
        {
            LaserX_9078_Utilities.MOT_CMP_STAT_EX cmpStatEx = new LaserX_9078_Utilities.MOT_CMP_STAT_EX();

            bool moved = false; //是否运动过

            try
            {
                while (true)
                {

                    if (qCmpPos.Count <= 0) //数据队列空了
                    {
                        return;
                    }

                    if (moved == false && objAxis.IsMotorMoving() == true) //运动了
                    {
                        moved = true; //运动起来了
                    }
                    else if (objAxis == null || (moved == true && objAxis.IsMotorMoving() == false)) //运动停了
                    {
                        return;
                    }


                    //比较队列是否已经满了
                    var rc = LaserX_9078_Utilities.P9078_CmpGetStatusEx(CardNo, 0, ref cmpStatEx);
                    if (rc != 0)
                    {
                        //失败
                        return;
                    }
                    if (cmpStatEx.queFull == 0) //不满
                    {
                        //取一个位置
                        double pos = qCmpPos.Dequeue();

                        //queFull 为1表示队列满，为0表示队列未满
                        //加到队列
                        rc = LaserX_9078_Utilities.P9078_CmpSetRefEx(CardNo, 0, pos);
                        if (rc != 0)
                        {
                            //throw new Exception(string.Format("ERROR: P9078_CmpSetRef({0}） fails({1})\n", this.CardNo, rc));
                        }
                    }


                    //Thread.Sleep(1);
                }

            }
            catch (Exception ex)
            {


            }
        }

        #region 启动带比较功能的绝对运动

        private int CmpPara(double UnitPos, double[] lstPosition)
        {
            string exMsg;
            int errorCode = ErrorCodes.NoError;

            if (objAxis == null)
            {
                exMsg = $"数据点中不存在插补轴参数";
                return ErrorCodes.Unexecuted;
            }

            CmpPosition = new List<double>();

            if (lstPosition != null)
            {
                double StartPos = objAxis.Get_CurUnitPos();
                double EndPos = UnitPos;

                double minPos = Math.Min(StartPos, EndPos);
                double maxPos = Math.Max(StartPos, EndPos);

                foreach (var item in lstPosition)
                {
                    if (minPos < item && item < maxPos)
                    {
                        CmpPosition.Add(item);
                    }
                }

                if (CmpPosition.Count > 0)
                {
                    CmpPosition.Sort();
                    if (StartPos > EndPos)    //如果是倒着走
                    {
                        CmpPosition.Reverse();  //反转位置序列
                    }

                    qCmpPos.Clear();
                    //将数据压入队列
                    foreach (var item in CmpPosition)
                    {
                        qCmpPos.Enqueue(item);
                    }

                    //如果数据序列足够

                    LaserX_9078_Utilities.MOT_CMP_STAT_EX cmpStatEx = new LaserX_9078_Utilities.MOT_CMP_STAT_EX();

                    while (true)
                    {
                        if (qCmpPos.Count <= 0) //数据队列空了
                        {
                            break;
                        }
                        //比较队列是否已经满了
                        var rc = LaserX_9078_Utilities.P9078_CmpGetStatusEx(CardNo, 0, ref cmpStatEx);
                        if (rc != 0)
                        {
                            //失败
                            break;
                        }
                        if (cmpStatEx.queFull == 0) //不满
                        {
                            //取一个位置
                            double pos = qCmpPos.Dequeue();

                            //queFull 为1表示队列满，为0表示队列未满
                            //加到队列
                            rc = LaserX_9078_Utilities.P9078_CmpSetRefEx(CardNo, 0, pos);
                            if (rc != 0)
                            {
                                //throw new Exception(string.Format("ERROR: P9078_CmpSetRef({0}） fails({1})\n", this.CardNo, rc));
                            }
                        }
                        else
                        {
                            //缓冲满了, 队列还有数据
                            if (qCmpPos.Count > 0) //数据队列
                            {
                                //开辟线程不断压入数据到板卡
                                try
                                {
                                    //if (th_AddPos == null)
                                    {
                                        th_AddPos = new Thread(new ThreadStart(_AddPosFun));
                                        th_AddPos.IsBackground = true;
                                        th_AddPos.Start();

                                        Thread.Sleep(200);

                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                                break;
                            }
                        }

                    }

                }
            }
            return errorCode;
        }

        //按照既定速度运行
        public int CmpMoveToV3(double UnitPos, double[] lstPosition, SpeedType speedType, SpeedLevel speedLevel)
        {
            var rtn = CmpPara(UnitPos, lstPosition);
            if (rtn != 0)
                return rtn;

            //运动位置
            return objAxis.MoveToV3(UnitPos, speedType, speedLevel);

        }
        
        //按照预定速度运行
        public int CmpMoveToV3(double UnitPos, double[] lstPosition, double speed)
        {
            var rtn = CmpPara(UnitPos, lstPosition);
            if (rtn != 0)
                return rtn;

            //运动位置
            return objAxis.MoveToV3(UnitPos, speed);

        }
        #endregion

    }
}