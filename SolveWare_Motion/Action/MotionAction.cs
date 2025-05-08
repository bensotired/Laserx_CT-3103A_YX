using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_Motion
{
    public class MotionAction
    {
        //public int MotorMoveAndStopByIOItemTrigger(MotorAxisBase axisName, double pos, string iOName, bool isStatus_On, float speedFactor = 1)
        //{
        //    int errorCode = ErrorCodes.NoError;
        //    string sErr = "";
        //    AxisBase2 ax = null;
        //    IOItemBase iOIitem = null;

        //    IMotionManager motor = Manage.Control.MMgr.GetManager<IMotionManager>();
        //    IIOManager iO = Manage.Control.MMgr.GetManager<IIOManager>();

        //    try
        //    {
        //        do
        //        {
        //            if (speedFactor > 1)
        //                return ErrorCodes.SpeedFactorParametersSetTooHigh;

        //            iOIitem = iO.GetTool(iOName);
        //            if (iOIitem == null) return ErrorCodes.IOItemNotFoundError;
        //            if (Manage.Control.HasErrorStop(errorCode)) break;

        //            IOStatus status = isStatus_On ? IOStatus.On : IOStatus.Off;
        //            if (iOIitem.Status == status) break;


        //            ax = motor.GetTool(axisName);
        //            if (ax == null) return ErrorCodes.MotorNotFound;
        //            if (Manage.Control.HasErrorStop(errorCode)) break;


        //            ax.MoveTo(pos, SpeedType.Auto, speedFactor);
        //            //IOStatus ioStatus = io.Status;

        //            //确认是否已经抵达目的地 或中途停止 或 IO 已被触发 或超时
        //            DateTime st = DateTime.Now;
        //            while(true)
        //            {
        //                errorCode = ax.InPositionCheck(pos);
        //                if (errorCode == ErrorCodes.NoError) break;

        //                if (ax.IsMoveTimeOut(st))
        //                    return ErrorCodes.MotorMoveTimeOutError;

        //                if (iOIitem.Status == status) break;

        //                if (Manage.Control.HasErrorStop(errorCode)) break;
        //            }


        //            if (Manage.Control.HasErrorStop(errorCode)) break;

        //        }
        //        while (false);
        //    }
        //    catch (Exception ex)
        //    {
        //        errorCode = ErrorCodes.MotorMoveError;
        //    }

        //    return errorCode;
        //}
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

                    //ax = motor.GetTool(axisName);
                    //if (ax == null) return ErrorCodes.MotorNotFound;
                    //if (Manage.Control.HasErrorStop(errorCode)) break;


                    errorCode = axis.MoveTo(pos, SpeedType.Auto, speedFactor);
                    //if (Manage.Control.HasErrorStop(errorCode)) break;


                    //确认是否已经抵达目的地 或中途停止 或 IO 已被触发
                    if (wait)
                    {
                        errorCode = axis.WaitMotionDone();
                        //if (Manage.Control.HasErrorStop(errorCode)) break;

                        errorCode = axis.InPositionCheck(pos);
                        //if (Manage.Control.HasErrorStop(errorCode)) break;
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
            //AxisBase2 ax = null;

            //IMotionManager motor = Manage.Control.MMgr.GetManager<IMotionManager>();

            try
            {
                do
                {
                    if (speedFactor > 1)
                        return ErrorCodes.SpeedFactorParametersSetTooHigh;

                    //ax = motor.GetTool(axisName);
                    //if (ax == null) return ErrorCodes.MotorNotFound;
                    //if (Manage.Control.HasErrorStop(errorCode)) break;


                    //errorCode = axis.MoveTo(pos, SpeedType.Auto, speedFactor);

                    errorCode = (axis as Motor_GUGAO).MoveToV2(pos, SpeedType.Auto, speedFactor, new Func<bool>(() => { return token.IsCancellationRequested; }));

                    //if (Manage.Control.HasErrorStop(errorCode)) break;


                    //确认是否已经抵达目的地 或中途停止 或 IO 已被触发
                    if (wait)
                    {
                        if (token.IsCancellationRequested)
                        {
                            axis.Stop();
                        }
                        errorCode = axis.WaitMotionDone();
                        //if (Manage.Control.HasErrorStop(errorCode)) break;

                        errorCode = axis.InPositionCheck(pos);
                        //if (Manage.Control.HasErrorStop(errorCode)) break;
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
                        errorCode = axis_1.WaitMotionDone();
                        errorCode = axis_1.InPositionCheck(pos1);
                    }
                    errorCode = axis_2.MoveTo(pos2, SpeedType.Auto, speedFactor);
                    if (wait)
                    {
                        errorCode = axis_2.WaitMotionDone();
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

        //public int MoveSlowDownAndStopIfIOItemTrigger(string axisName, double pos, double slowGap, string iOName, bool ioStatus_On, double overShootGap = 0, float speedFactor = 1)
        //{
        //    int errorCode = ErrorCodes.NoError;
        //    string sErr = "";
        //    AxisBase2 ax = null;
        //    IOItemBase io = null;
        //    bool isDestinationShort = false;
        //    double factor = speedFactor / 100;

        //    IMotionManager motor = Manage.Control.MMgr.GetManager<IMotionManager>();
        //    IIOManager iO = Manage.Control.MMgr.GetManager<IIOManager>();


        //    try
        //    {
        //        do
        //        {
        //            if (speedFactor > 1)
        //                return ErrorCodes.SpeedFactorParametersSetTooHigh;

        //            io = iO.GetTool(iOName);
        //            if (io == null) return ErrorCodes.IOItemNotFoundError;
        //            if (Manage.Control.HasErrorStop(errorCode)) break;

        //            IOStatus ioStatus = ioStatus_On ? IOStatus.On : IOStatus.Off;
        //            if (io.Status == ioStatus) break;


        //            ax = motor.GetTool(axisName);
        //            if (ax == null) return ErrorCodes.MotorNotFound;
        //            if (Manage.Control.HasErrorStop(errorCode)) break;

        //            isDestinationShort = Math.Abs(slowGap) > Math.Abs(pos - ax.Get_CurUnitPos());

        //            double targetPos = 0;
        //            DateTime st = DateTime.Now;
        //            if (isDestinationShort)
        //            {
        //                targetPos = pos + overShootGap;
        //                errorCode = ax.MoveTo(targetPos, SpeedType.Auto, speedFactor);
        //                if (Manage.Control.HasErrorStop(errorCode)) break;

        //                while (true)
        //                {
        //                    if (!ax.IsMoving && ax.InPositionCheck(targetPos) == ErrorCodes.NoError) break;
        //                    if (io.Status == ioStatus) break;
        //                    if (Manage.Control.HasErrorStop(errorCode)) break;
        //                    if (ax.IsMoveTimeOut(st))
        //                        return ErrorCodes.MotorMoveTimeOutError;

        //                }
        //            }
        //            else
        //            {
        //                st = DateTime.Now;
        //                targetPos = pos - slowGap;
        //                errorCode = ax.MoveTo(targetPos, SpeedType.Auto, speedFactor);
        //                if (Manage.Control.HasErrorStop(errorCode)) break;

        //                while (true)
        //                {
        //                    if (!ax.IsMoving && ax.InPositionCheck(targetPos) == ErrorCodes.NoError) break;
        //                    if (io.Status == ioStatus) break;
        //                    if (Manage.Control.HasErrorStop(errorCode)) break;
        //                    if (ax.IsMoveTimeOut(st))
        //                        return ErrorCodes.MotorMoveTimeOutError;

        //                }


        //                targetPos = pos + overShootGap;
        //                st = DateTime.Now;
        //                errorCode = ax.MoveTo(targetPos, SpeedType.Auto, speedFactor);
        //                if (Manage.Control.HasErrorStop(errorCode)) break;

        //                while (true)
        //                {
        //                    if (!ax.IsMoving && ax.InPositionCheck(targetPos) == ErrorCodes.NoError) break;
        //                    if (io.Status == ioStatus) break;
        //                    if (Manage.Control.HasErrorStop(errorCode)) break;
        //                    if (ax.IsMoveTimeOut(st))
        //                        return ErrorCodes.MotorMoveTimeOutError;

        //                }
        //            }

        //            if (Manage.Control.HasErrorStop(errorCode)) break;

        //        }
        //        while (false);
        //    }
        //    catch (Exception ex)
        //    {
        //        errorCode = ErrorCodes.MotorMoveError;
        //        Manage.Control.Log($"{ErrorCodes.GetErrorDescription(errorCode)} {Environment.NewLine} {sErr} {Environment.NewLine} {ex.Message}", true, errorCode);
        //    }

        //    return errorCode;
        //}

        //public int MotorHomeAndWait(MotorAxisBase axis, bool wait = true)
        //{
        //    int errorCode = ErrorCodes.NoError;

        //    try
        //    {
        //        do
        //        {

        //            errorCode = axis.HomeMove();
        //            if (errorCode != ErrorCodes.NoError)
        //            {
        //                errorCode = ErrorCodes.MotorHomingError;
        //                break;
        //            }


        //            if (wait)
        //            {
        //                errorCode = axis.WaitHomeDone();
        //                if (errorCode != ErrorCodes.NoError)
        //                {
        //                    errorCode = ErrorCodes.MotorHomeDoneWaitError;
        //                    break;
        //                }
        //            }

        //            axis.SetHomeDone(true);
        //        }
        //        while (false);
        //    }
        //    catch (Exception ex)
        //    {
        //        errorCode = ErrorCodes.MotorHomingError;

        //    }



        //    return errorCode;
        //}
        public int MotorMoveAndWait(List<MotorAxisBase> axisList, AxesPosition pos, bool wait, CancellationToken token)
        {
            foreach (var ap in pos)
            {
                if (token.IsCancellationRequested)
                {
                    return ErrorCodes.EStopTrigger;
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
                        //先使能
                        axis.Set_Servo(true);

                        int errorCode = ErrorCodes.NoError;
                        double absolutePos = Convert.ToDouble(ap.Position);
                        double targetPos = absolutePos;
                        string sErr = string.Empty;
                        errorCode = this.MotorMoveAndWait(axis, targetPos, token);

                        //关闭使能
                        axis.Set_Servo(false);

                        if (errorCode != ErrorCodes.NoError)
                            MessageBox.Show($"轴运行错误:[{ErrorCodes.GetErrorDescription(errorCode)}]!");


                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show($"轴运行错误:[{ex.Message}-{ex.StackTrace}]!");
                    }
                }
            }
            return ErrorCodes.NoError;
        }

        public void SetPauseState(MotorAxisBase axis, bool isPause)
        {
            (axis as Motor_GUGAO)._pause = isPause;
        }
        public void SetPauseState(List<MotorAxisBase> axisList, AxesPosition pos, bool isPause)
        {
            foreach (var ap in pos)
            {
                var axis = axisList.Find(item => item.Name == ap.Name);
                (axis as Motor_GUGAO)._pause = isPause;
            }
        }
    }
}