using SolveWare_BurnInCommon;
using SolveWare_Motion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TestPlugin_Demo
{
    public partial class PluginBizService
    {
        //运行通用点位
        //运行用于视觉校准的点位


        public void ResetAllMotionActionToken()
        {
            foreach (var ama in this.pluginResourse.AxesMotionAction)
            {
                ama.Value.Reset();
            }
        }

        public void StopAllMotionAction()
        {
            foreach (var axisMotionAction in this.pluginResourse.AxesMotionAction)
            {
                axisMotionAction.Value.Cancel();
            }
        }

        public Dictionary<AxisNameEnum_CT40410A, double> Get_Axes_Current_Physical_Position(AxesPositionEnum_CT40410A axesPosition)
        {
            if (this.pluginResourse.Positions.ContainsKey(axesPosition) == false)
            {
                throw new Exception($"资源池内不包含该点位[{axesPosition}]!");
            }
            Dictionary<AxisNameEnum_CT40410A, double> temp = new Dictionary<AxisNameEnum_CT40410A, double>();
            var position = this.pluginResourse.Positions[axesPosition];
            foreach (var item in position)
            {
                AxisNameEnum_CT40410A axisEnumObject = default(AxisNameEnum_CT40410A);
                if (Enum.TryParse(item.Name, out axisEnumObject) == false)
                {
                    throw new Exception($"资源池内不包含该点位[{axesPosition}]所需的轴[{item.Name}]!");
                }
                var axisInstance = this.pluginResourse.Axes[axisEnumObject];
                temp.Add(axisEnumObject, axisInstance.Interation.CurrentPosition);
            }
            return temp;
        }

        public Dictionary<AxisNameEnum_CT40410A, double> Get_VCal_Axes_Current_Physical_Position(VisionCalibrationEnum_CT40410A axesPosition)
        {
            if (this.pluginResourse.VCALPositions.ContainsKey(axesPosition) == false)
            {
                throw new Exception($"资源池内不包含该点位[{axesPosition}]!");
            }
            Dictionary<AxisNameEnum_CT40410A, double> temp = new Dictionary<AxisNameEnum_CT40410A, double>();
            var position = this.pluginResourse.VCALPositions[axesPosition];
            foreach (var item in position)
            {
                AxisNameEnum_CT40410A axisEnumObject = default(AxisNameEnum_CT40410A);
                if (Enum.TryParse(item.Name, out axisEnumObject) == false)
                {
                    throw new Exception($"资源池内不包含该点位[{axesPosition}]所需的轴[{item.Name}]!");
                }
                var axisInstance = this.pluginResourse.Axes[axisEnumObject];
                temp.Add(axisEnumObject, axisInstance.Interation.CurrentPosition);
            }
            return temp;
        }

        public double Get_Axis_Current_Physical_Position(AxisNameEnum_CT40410A axisName)
        {
            if (this.pluginResourse.Axes.ContainsKey(axisName) == false)
            {
                throw new Exception($"资源池内不包含该点位所需的轴[{axisName}]!");
            }

            var axisPosi = this.pluginResourse.Axes[axisName].Interation.CurrentPosition;

            return axisPosi;
        }

        public XYCoord Get_2D_Axes_Current_Physical_Position(AxisNameEnum_CT40410A x_AxisName, AxisNameEnum_CT40410A y_AxisName)
        {
            if (this.pluginResourse.Axes.ContainsKey(x_AxisName) == false)
            {
                throw new Exception($"资源池内不包含该点位所需的轴[{x_AxisName}]!");
            }
            if (this.pluginResourse.Axes.ContainsKey(y_AxisName) == false)
            {
                throw new Exception($"资源池内不包含该点位所需的轴[{y_AxisName}]!");
            }
            
            var axisX_pos = this.pluginResourse.Axes[x_AxisName].Interation.CurrentPosition;

            var axisY_pos = this.pluginResourse.Axes[y_AxisName].Interation.CurrentPosition;

            return new XYCoord(axisX_pos, axisY_pos);
        }


        public XYZCoord Get_3D_Axes_Current_Physical_Position(AxisNameEnum_CT40410A x_AxisName, AxisNameEnum_CT40410A y_AxisName, AxisNameEnum_CT40410A z_AxisName)
        {
            if (this.pluginResourse.Axes.ContainsKey(x_AxisName) == false)
            {
                throw new Exception($"资源池内不包含该点位所需的轴[{x_AxisName}]!");
            }
            if (this.pluginResourse.Axes.ContainsKey(y_AxisName) == false)
            {
                throw new Exception($"资源池内不包含该点位所需的轴[{y_AxisName}]!");
            }
            if (this.pluginResourse.Axes.ContainsKey(z_AxisName) == false)
            {
                throw new Exception($"资源池内不包含该点位所需的轴[{z_AxisName}]!");
            }

            var axisX_pos = this.pluginResourse.Axes[x_AxisName].Interation.CurrentPosition;

            var axisY_pos = this.pluginResourse.Axes[y_AxisName].Interation.CurrentPosition;

            var axisZ_pos = this.pluginResourse.Axes[z_AxisName].Interation.CurrentPosition;

            return new XYZCoord(axisX_pos, axisY_pos, axisZ_pos);
        }












        public void Run_VisionCal_Position(VisionCalibrationEnum_CT40410A pos, SpeedLevel speedLevel = SpeedLevel.Normal, Func<bool> breakFunc = null)
        {
            this.Sequence_MoveTo_VCal_AxesPosition(pos, speedLevel, breakFunc);
        }

        public void Run_VisionCal_Position(VisionCalibrationEnum_CT40410A vcal_Pos, CancellationToken token, SpeedLevel speedLevel = SpeedLevel.Normal)
        {
            this.Sequence_MoveTo_VCal_AxesPosition(vcal_Pos, token, speedLevel);
        }


        public void Run_Common_Position(AxesPositionEnum_CT40410A pos, SpeedLevel speedLevel = SpeedLevel.Normal, Func<bool> breakFunc = null)
        {
            this.Sequence_MoveTo_AxesPosition(pos, speedLevel, breakFunc);
        }

        public void Run_Common_Position(AxesPositionEnum_CT40410A vcal_Pos, CancellationToken token, SpeedLevel speedLevel = SpeedLevel.Normal)
        {
            this.Sequence_MoveTo_AxesPosition(vcal_Pos, token, speedLevel);
        }


        private void Sequence_MoveTo_AxesPosition(AxesPositionEnum_CT40410A axesPosition, CancellationToken token, SpeedLevel speedLevel = SpeedLevel.Normal)
        {
            if (this.pluginResourse.Positions.ContainsKey(axesPosition) == false)
            {
                throw new Exception($"点位[{axesPosition}]串行移动错误:资源池内不包含该点位!");
            }
            int errorCode = ErrorCodes.NoError;
            var position = this.pluginResourse.Positions[axesPosition];
            //获取工位坐标中的轴
            foreach (var item in position)
            {
                //转换为枚举轴
                AxisNameEnum_CT40410A axisEnumObject = default(AxisNameEnum_CT40410A);
                if (Enum.TryParse(item.Name, out axisEnumObject) == false)
                {
                    throw new Exception($"点位[{axesPosition}]串行移动错误:资源池内不包含该点位所需的轴[{item.Name}]!");
                }
                //获取单轴基本运动参数
                var axisInstance = this.pluginResourse.Axes[axisEnumObject];
                //轴运动方法
                var axisMotionAction = this.pluginResourse.AxesMotionAction[axisEnumObject];

                if (axisMotionAction.TokenIsCancellationRequested())
                {
                    throw new Exception($"点位[{axesPosition}]串行移动错误:轴[{item.Name}]取消信号被激活!");
                }

                try
                {
                    errorCode = axisMotionAction.SingleAxisMotion(axisInstance, item.Position, speedLevel, new Func<bool>(() => { return token.IsCancellationRequested; }));
                    if (errorCode != ErrorCodes.NoError)
                    {
                        axisInstance.Stop();
                        throw new Exception($"点位[{axesPosition}]串行移动错误:轴[{axisEnumObject}]运动异常[{errorCode}]!");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void Sequence_MoveTo_AxesPosition(AxesPositionEnum_CT40410A axesPosition, SpeedLevel speedLevel = SpeedLevel.Normal, Func<bool> breakFunc = null)
        {
            if (this.pluginResourse.Positions.ContainsKey(axesPosition) == false)
            {
                throw new Exception($"点位[{axesPosition}]串行移动错误:资源池内不包含该点位!");
            }
            int errorCode = ErrorCodes.NoError;
            //由枚举转化为实体
            var position = this.pluginResourse.Positions[axesPosition];
            //获取工位坐标中的轴
            foreach (var item in position)
            {
                //转换为枚举轴
                AxisNameEnum_CT40410A axisEnumObject = default(AxisNameEnum_CT40410A);
                if (Enum.TryParse(item.Name, out axisEnumObject) == false)
                {
                    throw new Exception($"点位[{axesPosition}]串行移动错误:资源池内不包含该点位所需的轴[{item.Name}]!");
                }
                //获取单轴基本运动参数
                var axisInstance = this.pluginResourse.Axes[axisEnumObject];
                //轴运动方法
                var axisMotionAction = this.pluginResourse.AxesMotionAction[axisEnumObject];


                if (axisMotionAction.TokenIsCancellationRequested())
                {
                    throw new Exception($"点位[{axesPosition}]串行移动错误:轴[{item.Name}]取消信号被激活!");
                }

                try
                {
                    errorCode = axisMotionAction.SingleAxisMotion(axisInstance, item.Position, speedLevel, breakFunc);
                    if (errorCode != ErrorCodes.NoError)
                    {
                        axisInstance.Stop();
                        throw new Exception($"点位[{axesPosition}]串行移动错误:轴[{axisEnumObject}]运动异常[{errorCode}]!");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void Sequence_MoveTo_VCal_AxesPosition(VisionCalibrationEnum_CT40410A axesPosition, CancellationToken token, SpeedLevel speedLevel = SpeedLevel.Normal)
        {
            if (this.pluginResourse.VCALPositions.ContainsKey(axesPosition) == false)
            {
                throw new Exception($"点位[{axesPosition}]串行移动错误:资源池内不包含该点位!");
            }
            int errorCode = ErrorCodes.NoError;
            //由枚举转化为实体
            var position = this.pluginResourse.VCALPositions[axesPosition];
            //获取工位坐标中的轴
            foreach (var item in position)
            {
                //转换为枚举轴
                AxisNameEnum_CT40410A axisEnumObject = default(AxisNameEnum_CT40410A);
                if (Enum.TryParse(item.Name, out axisEnumObject) == false)
                {
                    throw new Exception($"点位[{axesPosition}]串行移动错误:资源池内不包含该点位所需的轴[{item.Name}]!");
                }
                //获取单轴基本运动参数
                var axisInstance = this.pluginResourse.Axes[axisEnumObject];
                //轴运动方法
                var axisMotionAction = this.pluginResourse.AxesMotionAction[axisEnumObject];


                if (axisMotionAction.TokenIsCancellationRequested())
                {
                    throw new Exception($"点位[{axesPosition}]串行移动错误:轴[{item.Name}]取消信号被激活!");
                }

                try
                {
                    errorCode = axisMotionAction.SingleAxisMotion(axisInstance, item.Position, speedLevel, new Func<bool>(() => { return token.IsCancellationRequested; }));
                    if (errorCode != ErrorCodes.NoError)
                    {
                        axisInstance.Stop();
                        throw new Exception($"点位[{axesPosition}]串行移动错误:轴[{axisEnumObject}]运动异常[{errorCode}]!");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void Sequence_MoveTo_VCal_AxesPosition(VisionCalibrationEnum_CT40410A axesPosition, SpeedLevel speedLevel = SpeedLevel.Normal, Func<bool> breakFunc = null)
        {
            if (this.pluginResourse.VCALPositions.ContainsKey(axesPosition) == false)
            {
                throw new Exception($"点位[{axesPosition}]串行移动错误:资源池内不包含该点位!");
            }
            int errorCode = ErrorCodes.NoError;
            //由枚举转化为实体
            var position = this.pluginResourse.VCALPositions[axesPosition];
            //获取工位坐标中的轴
            foreach (var item in position)
            {
                //转换为枚举轴
                AxisNameEnum_CT40410A axisEnumObject = default(AxisNameEnum_CT40410A);
                if (Enum.TryParse(item.Name, out axisEnumObject) == false)
                {
                    throw new Exception($"点位[{axesPosition}]串行移动错误:资源池内不包含该点位所需的轴[{item.Name}]!");
                }
                //获取单轴基本运动参数
                var axisInstance = this.pluginResourse.Axes[axisEnumObject];
                //轴运动方法
                var axisMotionAction = this.pluginResourse.AxesMotionAction[axisEnumObject];


                if (axisMotionAction.TokenIsCancellationRequested())
                {
                    throw new Exception($"点位[{axesPosition}]串行移动错误:轴[{item.Name}]取消信号被激活!");
                }

                try
                {
                    errorCode = axisMotionAction.SingleAxisMotion(axisInstance, item.Position, speedLevel, breakFunc);
                    if (errorCode != ErrorCodes.NoError)
                    {
                        axisInstance.Stop();
                        throw new Exception($"点位[{axesPosition}]串行移动错误:轴[{axisEnumObject}]运动异常[{errorCode}]!");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public void Parellel_MoveTo_AxesPosition(AxesPositionEnum_CT40410A axesPosition, SpeedLevel speedLevel = SpeedLevel.Normal, Func<bool> breakFunc = null)
        {
            if (this.pluginResourse.Positions.ContainsKey(axesPosition) == false)
            {
                throw new Exception($"点位[{axesPosition}]串行移动错误:资源池内不包含该点位!");
            }
            int errorCode = ErrorCodes.NoError;
            var position = this.pluginResourse.Positions[axesPosition];

            Dictionary<MotorAxisBase, MotionActionV2> dict = new Dictionary<MotorAxisBase, MotionActionV2>();

            //获取工位坐标中的轴
            foreach (var item in position)
            {
                //转换为枚举轴
                AxisNameEnum_CT40410A axisEnumObject = default(AxisNameEnum_CT40410A);
                if (Enum.TryParse(item.Name, out axisEnumObject) == false)
                {
                    throw new Exception($"点位[{axesPosition}]串行移动错误:资源池内不包含该点位所需的轴[{item.Name}]!");
                }
                //获取单轴基本运动参数
                var axisInstance = this.pluginResourse.Axes[axisEnumObject];
                //轴运动方法
                var axisMotionAction = this.pluginResourse.AxesMotionAction[axisEnumObject];
                dict.Add(axisInstance, axisMotionAction);
            }

            MultipleAxisAction.Parallel_MoveToAxesPosition(dict, position, speedLevel, breakFunc);
        }


        public void Parellel_MoveTo_VCal_AxesPosition(VisionCalibrationEnum_CT40410A axesPosition, SpeedLevel speedLevel = SpeedLevel.Normal, Func<bool> breakFunc = null)
        {
            if (this.pluginResourse.VCALPositions.ContainsKey(axesPosition) == false)
            {
                throw new Exception($"点位[{axesPosition}]并行移动错误:资源池内不包含该点位!");
            }
            var position = this.pluginResourse.VCALPositions[axesPosition];

            Dictionary<MotorAxisBase, MotionActionV2> dict = new Dictionary<MotorAxisBase, MotionActionV2>();

            //获取工位坐标中的轴
            foreach (var item in position)
            {
                //转换为枚举轴
                AxisNameEnum_CT40410A axisEnumObject = default(AxisNameEnum_CT40410A);
                if (Enum.TryParse(item.Name, out axisEnumObject) == false)
                {
                    throw new Exception($"点位[{axesPosition}]并行移动错误:资源池内不包含该点位所需的轴[{item.Name}]!");
                }
                //获取单轴基本运动参数
                var axisInstance = this.pluginResourse.Axes[axisEnumObject];
                //轴运动方法
                var axisMotionAction = this.pluginResourse.AxesMotionAction[axisEnumObject];
                dict.Add(axisInstance, axisMotionAction);
            }

            MultipleAxisAction.Parallel_MoveToAxesPosition(dict, position, speedLevel, breakFunc);
        }

        //最后一个轴串行，其他轴并行
        public void PS_MoveTo_VCal_AxesPosition(VisionCalibrationEnum_CT40410A axesPosition, SpeedLevel speedLevel = SpeedLevel.Normal, Func<bool> breakFunc = null)
        {
            if (this.pluginResourse.VCALPositions.ContainsKey(axesPosition) == false)
            {
                throw new Exception($"点位[{axesPosition}]并行移动错误:资源池内不包含该点位!");
            }
            int errorCode = ErrorCodes.NoError;
            var position = this.pluginResourse.VCALPositions[axesPosition];
            Dictionary<MotorAxisBase, MotionActionV2> dict = new Dictionary<MotorAxisBase, MotionActionV2>();

            var length = position.ItemCollection.Count;

            if (length<1)
            {
                throw new Exception($"点位[{axesPosition}]并行移动错误:无可运行的轴资源！");
            }
            else if (length==1)
            {
                var axisPosi = position.ItemCollection.First();
                AxisNameEnum_CT40410A axisEnumObject = default(AxisNameEnum_CT40410A);
                if (Enum.TryParse(axisPosi.Name, out axisEnumObject) == false)
                {
                    throw new Exception($"点位[{axesPosition}]并行移动错误:资源池内不包含该点位所需的轴[{axisPosi.Name}]!");
                }
                SingleAxis_MoveTo_Abs_Position(axisEnumObject, axisPosi.Position, breakFunc);
            }
            else if (length>1)
            {
                var axisPosi = position.ItemCollection.Last();

                //position.ItemCollection.Remove(axisPosi);
                AxisNameEnum_CT40410A axisEnumObject = default(AxisNameEnum_CT40410A);

                foreach (var item in position)
                {
                    if (item.Name == axisPosi.Name)
                    {
                        continue;
                    }
                    if (Enum.TryParse(item.Name, out axisEnumObject) == false)
                    {
                        throw new Exception($"点位[{axesPosition}]并行移动错误:资源池内不包含该点位所需的轴[{item.Name}]!");
                    }
                    var axisInstance = this.pluginResourse.Axes[axisEnumObject];
                    var axisMotionAction = this.pluginResourse.AxesMotionAction[axisEnumObject];
                    dict.Add(axisInstance, axisMotionAction);
                    if (dict.Count == (length - 1))
                    {
                        break;
                    }
                }

                if (Enum.TryParse(axisPosi.Name, out axisEnumObject) == false)
                {
                    throw new Exception($"点位[{axesPosition}]并行移动错误:资源池内不包含该点位所需的轴[{axisPosi.Name}]!");
                }

                MultipleAxisAction.Parallel_MoveToAxesPosition(dict, position, speedLevel, breakFunc);

                SingleAxis_MoveTo_Abs_Position(axisEnumObject, axisPosi.Position, breakFunc);

            }
        }



        public void Parellel_MoveTo_VCal_AxesPosition(Func<bool> breakFunc = null, params VisionCalibrationEnum_CT40410A[] vCalPositions)
        {
            List<Action> moveActions = new List<Action>();
            foreach (var item in vCalPositions)
            {
                moveActions.Add(new Action(() =>
                {
                    Parellel_MoveTo_VCal_AxesPosition
                    (axesPosition: item,
                    breakFunc: breakFunc);

                }));
            }
            try
            {
                Parallel.Invoke(moveActions.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Parellel_MoveTo_Abs_Position(Dictionary<AxisNameEnum_CT40410A, double> dict, Func<bool> breakFunc = null)
        {
            if (dict.Count == 0)
            {
                throw new Exception($"轴运行资源为空！！！");
            }

            List<Action> moveActions = new List<Action>();
            foreach (var item in dict)
            {
                var axisName = item.Key;
                var targetPosition_mm = item.Value;
                if (this.pluginResourse.Axes.ContainsKey(axisName) == false)
                {
                    throw new Exception($"资源池内不包含该点位所需的轴[{axisName}]!!");
                }
                var axisInstance = this.pluginResourse.Axes[axisName];
                var axisMotionAction = this.pluginResourse.AxesMotionAction[axisName];


                moveActions.Add(new Action(() =>
                {
                    var errorCode = axisMotionAction.SingleAxisMotion(
                        axis: axisInstance,
                        pos: targetPosition_mm,
                        breakFunc: breakFunc);
                    if (errorCode != ErrorCodes.NoError)
                    {
                        axisInstance.Stop();
                        throw new Exception($"轴[{axisName}]运动异常[{errorCode}]!");
                    }
                }));
            }

            try
            {
                Parallel.Invoke(moveActions.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SingleAxis_MoveTo_Abs_Position(AxisNameEnum_CT40410A axisName, double targetPosition_mm, Func<bool> breakFunc = null)
        {
            if (this.pluginResourse.Axes.ContainsKey(axisName) == false)
            {
                throw new Exception($"资源池内不包含该点位所需的轴[{axisName}]!!");
            }
            try
            {
                var axisInstance = this.pluginResourse.Axes[axisName];
                var axisMotionAction = this.pluginResourse.AxesMotionAction[axisName];
                var errorCode = axisMotionAction.SingleAxisMotion
                    (   axis: axisInstance,
                        pos: targetPosition_mm,
                        breakFunc: breakFunc
                     );
                if (errorCode != ErrorCodes.NoError)
                {
                    axisInstance.Stop();
                    throw new Exception($"轴[{axisName}]运动异常[{errorCode}]!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"轴[{axisName}]移动到目标点[{targetPosition_mm}]错误:[{ex.Message} - {ex.StackTrace}]!");
            }
        }



        //轴顺序翻转
        public void Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A axesPosition, SequenceOrder order, SpeedLevel speedLevel = SpeedLevel.Normal)
        {
            if (this.pluginResourse.Positions.ContainsKey(axesPosition) == false)
            {
                throw new Exception($"点位[{axesPosition}]串行移动错误:资源池内不包含该点位!");
            }
            int errorCode = ErrorCodes.NoError;
            var position = this.pluginResourse.Positions[axesPosition];

            Dictionary<MotorAxisBase, MotionActionV2> dict = new Dictionary<MotorAxisBase, MotionActionV2>();

            //获取工位坐标中的轴
            foreach (var item in position)
            {
                //转换为枚举轴
                AxisNameEnum_CT40410A axisEnumObject = default(AxisNameEnum_CT40410A);
                if (Enum.TryParse(item.Name, out axisEnumObject) == false)
                {
                    throw new Exception($"点位[{axesPosition}]串行移动错误:资源池内不包含该点位所需的轴[{item.Name}]!");
                }
                //获取单轴基本运动参数
                var axisInstance = this.pluginResourse.Axes[axisEnumObject];
                //轴运动方法
                var axisMotionAction = this.pluginResourse.AxesMotionAction[axisEnumObject];
                dict.Add(axisInstance, axisMotionAction);
            }

            MultipleAxisAction.Sequence_MoveToAxesPosition(dict, position, order, speedLevel);
        }

    }
}
