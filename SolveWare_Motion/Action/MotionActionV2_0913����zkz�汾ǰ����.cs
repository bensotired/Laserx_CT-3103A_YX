using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_Motion
{
    public enum STATUS
    {
        FREE = 1,
        RUNNING = 2,
        PAUSE = 3,
        CANCEL = 4//急停状态其实好像和空闲是一样的
    }

    public enum HOME
    {
        HOME_WAIT=1,
        HOME_DONE=3,
    }
    public class MotionActionV2
    {

        public MotionActionV2()
        {
            this._status = STATUS.FREE;
            this._pause = false;
            this._cancellationTokenSource = new CancellationTokenSource();
        }
        private STATUS _status { get; set; }
        private bool _pause { get; set; }
        private CancellationTokenSource _cancellationTokenSource { get; set; }

        /// <summary>
        /// 用于恢复按钮
        /// </summary>
        public void Resume()
        {
            this._pause = false;
        }

        /// <summary>
        /// 用于暂停按钮
        /// </summary>
        public void Pause()
        {
            this._pause = true;
        }


        /// <summary>
        /// 用于点击按钮
        /// </summary>
        public void Cancel()
        {
            this._cancellationTokenSource.Cancel();
        }

        //启动的时候，如果点了取消  再次进入的时候需要将标志重置
        public void Reset()
        {
            _pause = false;
            if (this._cancellationTokenSource.Token.IsCancellationRequested)
            {
                this._cancellationTokenSource = new CancellationTokenSource();
            }
        }

        public STATUS GetSTATUS()
        {
            return _status;
        }

        public bool TokenIsCancellationRequested()
        {
            return this._cancellationTokenSource.Token.IsCancellationRequested;
        }

        public int MotorMoveAndStopByLimitTrigger(MotorAxisBase axis, double pos, float speedFactor = 1)
        {
            int errorCode = ErrorCodes.NoError;
            string sErr = "";

            ;
            try
            {
                do
                {
                    if (speedFactor > 1)
                        return ErrorCodes.SpeedFactorParametersSetTooHigh;
                    axis.MoveTo(pos, SpeedType.Auto, speedFactor);
                    //IOStatus ioStatus = io.Status;

                    //确认是否已经抵达目的地 或中途停止 或 IO 已被触发 或超时
                    DateTime st = DateTime.Now;
                    while (true)
                    {
                        errorCode = axis.InPositionCheck(pos);
                        if (errorCode == ErrorCodes.NoError) break;

                        if (axis.IsMoveTimeOut(st))
                            return ErrorCodes.MotorMoveTimeOutError;

                        if (axis.Interation.IsPosLimit || axis.Interation.IsNegLimit) break;

                        //if (Manage.Control.HasErrorStop(errorCode)) break;
                    }


                    //if (Manage.Control.HasErrorStop(errorCode)) break;

                }
                while (false);
            }
            catch (Exception ex)
            {
                errorCode = ErrorCodes.MotorMoveError;
            }

            return errorCode;
        }
        public int MotorMoveAndWait(MotorAxisBase axis, double pos, bool wait = true, float speedFactor = 1)
        {
            int errorCode = ErrorCodes.NoError;
            string sErr = "";
            //AxisBase2 ax = null;

            //IMotionManager motor = Manage.Control.MMgr.GetManager<IMotionManager>();

            try
            {
                do
                {
                    if (speedFactor > 1)
                        return ErrorCodes.SpeedFactorParametersSetTooHigh;
                    errorCode = axis.MoveTo(pos, SpeedType.Auto, speedFactor);
                    //确认是否已经抵达目的地 或中途停止 或 IO 已被触发
                    if (wait)
                    {
                        errorCode = axis.WaitMotionDone();
                        errorCode = axis.InPositionCheck(pos);
                    }
                }
                while (false);
            }
            catch (Exception ex)
            {
                errorCode = ErrorCodes.MotorMoveError;
            }
            return errorCode;
        }

        public int MotorMoveAndWait(MotorAxisBase axis, double pos, CancellationToken token, bool wait = true, float speedFactor = 1)
        {
            int errorCode = ErrorCodes.NoError;
            string sErr = "";
            try
            {
                do
                {
                    if (speedFactor > 1)
                        return ErrorCodes.SpeedFactorParametersSetTooHigh;
                    errorCode = (axis as Motor_GUGAO).MoveToV2(pos, SpeedType.Auto, speedFactor, new Func<bool>(() => { return token.IsCancellationRequested; }));

                    if (wait)
                    {
                        if (token.IsCancellationRequested)
                        {
                            axis.Stop();
                        }
                        errorCode = axis.WaitMotionDone();
                        errorCode = axis.InPositionCheck(pos);
                    }
                }
                while (false);
            }
            catch (Exception ex)
            {
                errorCode = ErrorCodes.MotorMoveError;
            }

            return errorCode;
        }
        public int MotorsParallelMoveAndWait(MotorAxisBase axis_1, MotorAxisBase axis_2, double pos1, double pos2, bool wait = true, float speedFactor = 1)
        {
            int errorCode = ErrorCodes.NoError;

            try
            {
                do
                {
                    if (speedFactor > 1)
                        return ErrorCodes.SpeedFactorParametersSetTooHigh;

                    List<Task> tasks = new List<Task>();
                    int errorCode1 = ErrorCodes.NoError;
                    int errorCode2 = ErrorCodes.NoError;
                    tasks.Add(new Task(() =>
                    {
                        do
                        {
                            errorCode1 = axis_1.MoveTo(pos1, SpeedType.Auto, speedFactor);


                            if (wait)
                            {
                                errorCode = axis_1.WaitMotionDone();


                                errorCode = axis_1.InPositionCheck(pos1);

                            }

                        } while (false);

                    }));
                    tasks.Add(new Task(() =>
                    {
                        do
                        {
                            errorCode2 = axis_2.MoveTo(pos2, SpeedType.Auto, speedFactor);


                            if (wait)
                            {
                                errorCode = axis_2.WaitMotionDone();


                                errorCode = axis_2.InPositionCheck(pos2);
                            }

                        } while (false);

                    }));
                    Task.Factory.ContinueWhenAll(tasks.ToArray(), act =>
                    {
                        if (errorCode1 != ErrorCodes.NoError)
                            errorCode += errorCode1;

                        if (errorCode2 != ErrorCodes.NoError)
                            errorCode += errorCode2;
                    });

                }
                while (false);
            }
            catch (Exception ex)
            {
                errorCode = ErrorCodes.MotorMoveError;
            }

            return errorCode;
        }
        public int MotorsMoveAndWait(MotorAxisBase axis_1, MotorAxisBase axis_2, double pos1, double pos2, bool wait = true, float speedFactor = 1)
        {
            int errorCode = ErrorCodes.NoError;

            try
            {
                do
                {
                    if (speedFactor > 1)
                        return ErrorCodes.SpeedFactorParametersSetTooHigh;
                    errorCode = axis_1.MoveTo(pos1, SpeedType.Auto, speedFactor);
                    if (wait)
                    {
                        //errorCode = axis_1.WaitMotionDone();
                        errorCode = axis_1.InPositionCheck(pos1);
                    }
                    errorCode = axis_2.MoveTo(pos2, SpeedType.Auto, speedFactor);
                    if (wait)
                    {
                        //errorCode = axis_2.WaitMotionDone();
                        errorCode = axis_2.InPositionCheck(pos1);
                    }
                }
                while (false);
            }
            catch (Exception ex)
            {
                errorCode = ErrorCodes.MotorMoveError;
            }

            return errorCode;
        }

       
        /// <summary>
        /// 单轴运行，也要根据多轴运动的方式来进行运动
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="pos"></param>
        /// <param name="token"></param>
        /// <param name="speedFactor"></param>
        /// <returns></returns>
        public int SingleAxisMotion(MotorAxisBase axis, double pos, float speedFactor = 1)
        {
            int errorCode = ErrorCodes.NoError;
            if (this._cancellationTokenSource.Token.IsCancellationRequested)
            {
                return ErrorCodes.EStopTrigger;
            }
            try
            {

                if (speedFactor > 1)
                {
                    return ErrorCodes.SpeedFactorParametersSetTooHigh;
                }
                axis.Set_Servo(true);//使能
                errorCode = (axis as Motor_GUGAO).MoveToV3(pos, SpeedType.Auto, speedFactor);//控制卡生效运行需要400ms
                Thread.Sleep(100);
                while (true)
                {
                    if (axis.IsMotorMoving() == false)
                    {
                        break;
                    }
                    if (this._cancellationTokenSource.Token.IsCancellationRequested)//直接取消 停止轴 关闭使能
                    {
                        axis.Stop();
                        axis.Set_Servo(false);
                        return ErrorCodes.EStopTrigger;
                    }
                    if (this._pause == true)
                    {
                        _status = STATUS.PAUSE;
                        axis.Stop();
                        axis.Set_Servo(false);
                        while (true)
                        {
                            Thread.Sleep(100);
                            if (this._cancellationTokenSource.Token.IsCancellationRequested)//直接取消 已经停止轴关闭使能
                            {
                                return ErrorCodes.EStopTrigger;
                            }
                            if (this._pause == false)//暂停后继续运行 需要先使能
                            {
                                axis.Set_Servo(true);
                                errorCode = (axis as Motor_GUGAO).MoveToV3(pos, SpeedType.Auto, speedFactor);
                                Thread.Sleep(100);
                                break;
                            }
                        }
                    }
                    Thread.Sleep(50);
                }
                axis.Set_Servo(false);//关闭使能
                errorCode = axis.InPositionCheck(pos);

            }
            catch (Exception ex)
            {
                errorCode = ErrorCodes.MotorMoveError;
            }
            return errorCode;
        }



        /// <summary>
        /// 多轴运行  顺序运动
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="pos"></param>
        /// <param name="speedFactor"></param>
        /// <returns></returns>
        public int MultipleAxisMotion(List<MotorAxisBase> axisList, AxesPosition position, float speedFactor = 1)
        {
            int errorCode = ErrorCodes.NoError;
            _status = STATUS.RUNNING;
            foreach (var ap in position)
            {
                if (this._cancellationTokenSource.Token.IsCancellationRequested)
                {
                    errorCode = ErrorCodes.EStopTrigger;//取消就不会下面运行
                    continue;
                }
                var axis = axisList.Find(item => item.Name == ap.Name);
                if (axis == null || axis.Interation.IsMoving == true)
                {
                    return ErrorCodes.MotorIsProhibitedToMove;
                }
                if (axis.Interation.IsSimulation == true)
                {

                }
                else
                {
                    try
                    {
                        double targetPos = Convert.ToDouble(ap.Position);
                        errorCode = this.SingleAxisMotion(axis, targetPos);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"轴运行错误:[{ex.Message}-{ex.StackTrace}]!");
                    }
                }
            }
            switch (errorCode)
            {
                case ErrorCodes.EStopTrigger:
                    MessageBox.Show($"用户取消运行");
                    break;
                case ErrorCodes.NoError:
                    ;
                    break;
                default:
                    MessageBox.Show($"轴运行错误:[errorCode:{ErrorCodes.NoError.ToString()}");
                    break;
            }
            _status = STATUS.FREE;
            return ErrorCodes.NoError;
        }

        //int aa(List<MotorAxisBase> axisList, AxesPosition position, float speedFactor = 1)
        //{
        //    List<Action> actions = new List<Action>();

        //    foreach (var pos in position.ItemCollection)
        //    {
        //        Action a = new Action(() => { ParallelAxesMotion(new Motor_GUGAO(new MotorSetting()), pos.Position, 1); });
        //    }
        //   var t1= Task.Factory.StartNew(() => 
        //    {
        //        Parallel.Invoke
        //        (
        //            actions.ToArray()
        //        );
        //    } );

        //    return ErrorCodes.NoError;
        //}
        #region 所有轴同步运动方式太差了

        //public int ParallelAxesMotion(List<MotorAxisBase> axisList, AxesPosition position, float speedFactor = 1)
        //{
        //    Barrier bar = new Barrier(position.ItemCollection.Count);
        //    int errorCode = ErrorCodes.NoError;
        //    _status = STATUS.RUNNING;
        //    foreach (var ap in position)
        //    {
        //        if (this._cancellationTokenSource.Token.IsCancellationRequested)
        //        {
        //            errorCode = ErrorCodes.EStopTrigger;//取消就不会下面运行
        //            continue;
        //        }
        //        var axis = axisList.Find(item => item.Name == ap.Name);
        //        if (axis == null || axis.Interation.IsMoving == true)
        //        {
        //            return ErrorCodes.MotorIsProhibitedToMove;
        //        }
        //        if (axis.Interation.IsSimulation == true)
        //        {

        //        }
        //        else
        //        {
        //            try
        //            {
        //                double targetPos = Convert.ToDouble(ap.Position);
        //                Thread thread = new Thread(()=> ParallelAxisMotion(axis, targetPos, bar));
        //                //errorCode = this.ParallelAxisMotion(axis, targetPos);
        //                thread.Start();
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show($"轴运行错误:[{ex.Message}-{ex.StackTrace}]!");
        //            }
        //        }
        //    }
        //    return errorCode;
        //}

        //public int ParallelAxisMotion(MotorAxisBase axis, double pos, Barrier bar, float speedFactor = 1)
        //{
        //    int errorCode = ErrorCodes.NoError;
        //    if (this._cancellationTokenSource.Token.IsCancellationRequested)
        //    {
        //        return ErrorCodes.EStopTrigger;
        //    }
        //    try
        //    {

        //        if (speedFactor > 1)
        //        {
        //            return ErrorCodes.SpeedFactorParametersSetTooHigh;
        //        }
        //        axis.Set_Servo(true);//使能
        //        errorCode = (axis as Motor_GUGAO).MoveToV3(pos, SpeedType.Auto, speedFactor);//控制卡生效运行需要400ms
        //        Thread.Sleep(100);
        //        while (true)
        //        {
        //            if (axis.IsMotorMoving() == false)
        //            {
        //                break;
        //            }
        //            if (this._cancellationTokenSource.Token.IsCancellationRequested)//直接取消 停止轴 关闭使能
        //            {
        //                axis.Stop();
        //                axis.Set_Servo(false);
        //                return ErrorCodes.EStopTrigger;
        //            }
        //            if (this._pause == true)
        //            {
        //                _status = STATUS.PAUSE;
        //                axis.Stop();
        //                axis.Set_Servo(false);
        //                while (true)
        //                {
        //                    Thread.Sleep(100);
        //                    if (this._cancellationTokenSource.Token.IsCancellationRequested)//直接取消 已经停止轴关闭使能
        //                    {
        //                        return ErrorCodes.EStopTrigger;
        //                    }
        //                    if (this._pause == false)//暂停后继续运行 需要先使能
        //                    {
        //                        axis.Set_Servo(true);
        //                        errorCode = (axis as Motor_GUGAO).MoveToV3(pos, SpeedType.Auto, speedFactor);
        //                        Thread.Sleep(100);
        //                        break;
        //                    }
        //                }
        //            }
        //            Thread.Sleep(50);
        //        }
        //        axis.Set_Servo(false);//关闭使能
        //        errorCode = axis.InPositionCheck(pos);

        //        switch (errorCode)
        //        {
        //            case ErrorCodes.EStopTrigger:
        //                MessageBox.Show($"用户取消运行");
        //                break;
        //            case ErrorCodes.NoError:
        //                ;
        //                break;
        //            default:
        //                MessageBox.Show($"轴运行错误:[errorCode:{ErrorCodes.NoError.ToString()}");
        //                break;
        //        }
        //        bar.SignalAndWait();
        //    }
        //    catch (Exception ex)
        //    {
        //        errorCode = ErrorCodes.MotorMoveError;
        //    }
        //    finally
        //    {
        //        _status = STATUS.FREE;
        //    }
        //    return errorCode;
        //}
        #endregion


        public int ParallelAxesMotion(List<MotorAxisBase> axisList, AxesPosition position, float speedFactor = 1)
        {
            List<Action> actions = new List<Action>(); 
            int errorCode = ErrorCodes.NoError;
            _status = STATUS.RUNNING;
            foreach (var ap in position)
            {
                if (this._cancellationTokenSource.Token.IsCancellationRequested)
                {
                    errorCode = ErrorCodes.EStopTrigger;//取消就不会下面运行
                    continue;
                }
                var axis = axisList.Find(item => item.Name == ap.Name);
                if (axis == null || axis.Interation.IsMoving == true)
                {
                    return ErrorCodes.MotorIsProhibitedToMove;
                }
                if (axis.Interation.IsSimulation == true)
                {

                }
                else
                {
                    try
                    {
                        double targetPos = Convert.ToDouble(ap.Position);
                        Action action = new Action(() => SingleAxisMotion(axis, targetPos));
                        actions.Add(action);
                        //errorCode = this.ParallelAxisMotion(axis, targetPos);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"轴运行错误:[{ex.Message}-{ex.StackTrace}]!");
                    }
                }
            }
            if (actions.Count > 0)
            {
                Task.Factory.StartNew(() =>
                {
                    Parallel.Invoke(actions.ToArray());
                    _status = STATUS.FREE;
                }
                );
               
            }
            return errorCode;
        }
        /// <summary>
        /// 单个轴进行回零运动 只是下发回零的指令
        /// </summary>
        /// <param name="axis"></param>
        public bool SingleAxisHome(MotorAxisBase axis)
        {
            bool IsFinish = false;
            if (axis == null)
            {
                return IsFinish;
            }
            if (axis.Interation.IsSimulation)
            {
                return IsFinish;
            }
            if (this._cancellationTokenSource.Token.IsCancellationRequested)
            {
                return IsFinish;
            }
            if (axis.IsProhibitToHome())
            {
                return IsFinish;
            }
            bool InstructCorrect = true;
            int reachedSignal = 0;
            ushort HomeDone = 3;
            ushort drvMode = 0;
            axis.Set_Servo(false);
            Thread.Sleep(500);
            axis.Set_Servo(true);
            Thread.Sleep(500);

            InstructCorrect=(axis as Motor_GUGAO).HomeRun();//只发回原点的指令
            if (!InstructCorrect)
            {
                axis.Stop();
                axis.Set_Servo(false);
                return IsFinish;
            }
            while (reachedSignal != HomeDone)
            {
                Thread.Sleep(10);
                reachedSignal =(axis as Motor_GUGAO).IsHomeReached(out InstructCorrect);//等待
                                                                                        //if (reachedSignal == 5 || reachedSignal ==4) { MessageBox.Show("我又5啦"); }

                var ret = GUGAO_LIB.GTN_GetEcatAxisMode(axis.MotorGeneralSetting.MotorTable.CardNo, axis.MotorGeneralSetting.MotorTable.AxisNo, out drvMode);
                if (ret != 0)
                {
                    return false;
                }

                uint di;
                 ret = GUGAO_LIB.GTN_GetEcatAxisDI(axis.MotorGeneralSetting.MotorTable.CardNo, axis.MotorGeneralSetting.MotorTable.AxisNo, out di);
                if (ret != 0)
                {
                    return false;
                }

                if (!InstructCorrect ||this._cancellationTokenSource.Token.IsCancellationRequested)
                {
                    axis.Stop();
                    axis.Set_Servo(false);
                    return IsFinish;
                }
            }
            InstructCorrect=(axis as Motor_GUGAO).HomeClearStaus();//到位后的清除轴状态
            if (!InstructCorrect)
            {
                axis.Stop();
                axis.Set_Servo(false);
                return IsFinish;
            }
            axis.Set_Servo(false);
            return true;
        }

    }
}