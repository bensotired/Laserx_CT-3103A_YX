using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolveWare_Motion
{
    public static class MultipleAxisAction
    {

        public static int Sequence_MoveToAxesPosition(Dictionary<MotorAxisBase, MotionActionV2> axisVsActionDict,
            AxesPosition originalPosition, 
            SequenceOrder seqOrder , SpeedLevel speedLevel = SpeedLevel.Normal)
        {
            int errorCode = ErrorCodes.NoError;
            AxesPosition orderAp = new AxesPosition();

            switch (seqOrder)
            {
                case SequenceOrder.Normal:
                    orderAp = originalPosition;
                    break;
                case SequenceOrder.Reverse:
                    orderAp = originalPosition.Reverse();
                    break;
            }

            foreach (var ap in orderAp)
            {
                MotorAxisBase axis = null;
                MotionActionV2 action = null;

                foreach (var kvp in axisVsActionDict)
                {
                    if (kvp.Key.Name == ap.Name)
                    {
                        axis = kvp.Key;
                        action = kvp.Value;
                        break;
                    }
                }
                if (axis == null ||
                    action == null ||
                    axis.IsMotorMoving() ||
                    action.TokenIsCancellationRequested()
                    )
                {
                    return ErrorCodes.Sequence_MoveToAxesPositionFailed;
                }
                if (axis.Interation.IsSimulation)
                {

                }
                else
                {
                    try
                    {
                        errorCode = action.SingleAxisMotion(axis, ap.Position , speedLevel);
                        switch (errorCode)
                        {
                            case ErrorCodes.NoError:
                                break;
                            default:
                                {
                                    action.Cancel();
                                    axis.Stop();
                                }
                                return errorCode;
                                break;
                        }
                    }
                    catch
                    {
                        return ErrorCodes.Sequence_MoveToAxesPositionFailed;
                    }
                }
            }
            return errorCode;
        }
        public static int Parallel_MoveToAxesPosition(Dictionary<MotorAxisBase, MotionActionV2> axisVsActionDict, AxesPosition originalPosition , SpeedLevel speedLevel = SpeedLevel.Normal,Func<bool> breakFunc = null)
        {
            int errorCode = ErrorCodes.NoError;

            List<Action> moveActions = new List<Action>();
            foreach (var ap in originalPosition)
            {
                MotorAxisBase axis = null;
                MotionActionV2 action = null;

                foreach (var kvp in axisVsActionDict)
                {
                    if (kvp.Key.Name == ap.Name)
                    {
                        axis = kvp.Key;
                        action = kvp.Value;
                        break;
                    }
                }
                if (axis == null ||
                    action == null ||
                    axis.IsMotorMoving() ||
                    action.TokenIsCancellationRequested()
                    )
                {
                    return ErrorCodes.Sequence_MoveToAxesPositionFailed;
                }
                if (axis.Interation.IsSimulation)
                {

                }
                else
                {
                    moveActions.Add(new Action(() => 
                    {
                       var subErrorCode = action.SingleAxisMotion(axis, ap.Position, speedLevel, breakFunc);
                        switch (subErrorCode)
                        {
                            case ErrorCodes.NoError:
                                break;
                            default:
                                {
                                    action.Cancel();
                                    axis.Stop();
                                }
                                break;
                        }
                    }));
                }
            }
            try
            {
                if (moveActions.Count > 0)
                {
                    Parallel.Invoke(moveActions.ToArray());
                }
            }
            catch (Exception ex)
            {
                errorCode = ErrorCodes.Parallel_MoveToAxesPositionFailed;
            }
            
            return errorCode;
        }
    }
}