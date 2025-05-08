using Newtonsoft.Json;
using SolveWare_BurnInCommon;
using SolveWare_Motion;
using SolveWare_Vision;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestPlugin_Demo
{
    public sealed partial class TestPluginWorker_CT3103
    {
        #region
        /// <summary>
        /// 通过本地枚举封装顺序运动
        /// </summary>
        /// <param name="axesPosition"></param>
        /// <param name="order"></param>
        internal void Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103 axesPosition, SequenceOrder order, SpeedLevel speedLevel = SpeedLevel.Normal)
        {
            try
            {
                if (this.LocalResource.Positions.ContainsKey(axesPosition) == false)
                {
                    throw new Exception($"点位[{axesPosition}]串行移动错误:资源池内不包含该点位!");
                }
                int errorCode = ErrorCodes.NoError;
                var position = this.LocalResource.Positions[axesPosition];

                Dictionary<MotorAxisBase, MotionActionV2> dict = new Dictionary<MotorAxisBase, MotionActionV2>();

                //获取工位坐标中的轴
                foreach (var item in position)
                {
                    //转换为枚举轴
                    AxisNameEnum_CT3103 axisEnumObject = default(AxisNameEnum_CT3103);
                    if (Enum.TryParse(item.Name, out axisEnumObject) == false)
                    {
                        throw new Exception($"点位[{axesPosition}]串行移动错误:资源池内不包含该点位所需的轴[{item.Name}]!");
                    }
                    //获取单轴基本运动参数
                    var axisInstance = this.LocalResource.Axes[axisEnumObject];
                    //轴运动方法
                    var axisMotionAction = this.LocalResource.AxesMotionAction[axisEnumObject];
                    dict.Add(axisInstance, axisMotionAction);
                }

                MultipleAxisAction.Sequence_MoveToAxesPosition(dict, position, order, speedLevel);
            }
            catch (Exception ex)
            {
                this.Log_Global($"错误:[{ex.Message}-{ex.StackTrace}]!");
            }
            
        }
        /// <summary>
        /// 通过本地枚举封装并行运动
        /// </summary>
        /// <param name="axesPosition"></param>
        internal void Parellel_MoveToAxesPosition(AxesPositionEnum_CT3103 axesPosition, SpeedLevel speedLevel = SpeedLevel.Normal)
        {
            try
            {
                if (this.LocalResource.Positions.ContainsKey(axesPosition) == false)
                {
                    throw new Exception($"点位[{axesPosition}]串行移动错误:资源池内不包含该点位!");
                }
                int errorCode = ErrorCodes.NoError;
                var position = this.LocalResource.Positions[axesPosition];

                Dictionary<MotorAxisBase, MotionActionV2> dict = new Dictionary<MotorAxisBase, MotionActionV2>();

                //获取工位坐标中的轴
                foreach (var item in position)
                {
                    //转换为枚举轴
                    AxisNameEnum_CT3103 axisEnumObject = default(AxisNameEnum_CT3103);
                    if (Enum.TryParse(item.Name, out axisEnumObject) == false)
                    {
                        throw new Exception($"点位[{axesPosition}]串行移动错误:资源池内不包含该点位所需的轴[{item.Name}]!");
                    }
                    //获取单轴基本运动参数
                    var axisInstance = this.LocalResource.Axes[axisEnumObject];
                    //轴运动方法
                    var axisMotionAction = this.LocalResource.AxesMotionAction[axisEnumObject];
                    dict.Add(axisInstance, axisMotionAction);
                }

                MultipleAxisAction.Parallel_MoveToAxesPosition(dict, position, speedLevel);
            }
            catch (Exception ex)
            {
                this.Log_Global($"错误:[{ex.Message}-{ex.StackTrace}]!");
            }
            
        }
        /// <summary>
        /// 重置所有运动token - 一般用于停止运动后复位token使下次运动可以正常执行
        /// </summary>
        public void ResetAllMotionActionToken()
        {
            foreach (var ama in this.LocalResource.AxesMotionAction)
            {
                ama.Value.Reset();
            }
        }
        public void StopAllMotionAction()
        {
            foreach (var ama in this.LocalResource.AxesMotionAction)
            {
                ama.Value.Cancel();
            }
        }
        #endregion

        /// <summary>
        /// 计算像素比，移动距离固定位1mm
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public double CalPix(string cmd, AxisNameEnum_CT3103 axis)
        {
            string CenterList = string.Empty;
            double pos_Current_pulse = 0;
            double pos_Current_Unit = 0;
            double pos_Next_pulse = 0;
            double pos_Next_Unit = 0;
            double pix__Current = 0;
            double pix_Next = 0;

            int distance = 1;//移动距离
            string ax = axis.ToString();
            CenterList = ax[ax.Length - 1] == 'X' ? "CenterXList" : "CenterYList";
            string ret1 = this.LocalResource.VisionController.GetVisionResult_Universal(cmd);
            Newtonsoft.Json.Linq.JObject Jret1 = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(ret1);
            bool isSuccess = Convert.ToBoolean(Jret1["Success"].ToString());
            if (!isSuccess)
            {
                MessageBox.Show("匹配第一次出错");
                return 0;
            }
            pix__Current = double.Parse(Jret1[CenterList].First.ToString());
            MotorAxisBase motorAxisBase = this.LocalResource.Axes[axis];
            pos_Current_pulse = motorAxisBase.Get_CurPulse();
            pos_Current_Unit = motorAxisBase.Get_CurUnitPos();
            pos_Next_Unit = pos_Current_Unit + distance;
            this.LocalResource.AxesMotionAction[axis].SingleAxisMotion(motorAxisBase, pos_Next_Unit);
            Thread.Sleep(200);

            string ret2 = this.LocalResource.VisionController.GetVisionResult_Universal(cmd);
            Newtonsoft.Json.Linq.JObject Jret2 = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(ret2);
            isSuccess = Convert.ToBoolean(Jret2["Success"].ToString());
            if (!isSuccess)
            {
                MessageBox.Show("匹配第二次出错");
                return 0;
            }
            pix_Next = double.Parse(Jret2[CenterList].First.ToString());
            pos_Next_pulse = motorAxisBase.Get_CurPulse();
            double diffrentPix = pix_Next - pix__Current;
            //这里计算一个像素多少MM   就是mm/pix
            double mm_compare_pix = Math.Round(distance / diffrentPix, 5);

            this.LocalResource.AxesMotionAction[axis].SingleAxisMotion(motorAxisBase, pos_Current_Unit);

            return Math.Abs(mm_compare_pix);
        }
        /// <summary>
        /// 吸嘴X轴识别后运动到中心
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="axisNameEnum"></param>
        /// <param name="pPM_Enum_"></param>
        public XYCoord MoveTo(string cmd, AxisNameEnum_CT3103 axisNameEnum, PPM_Enum_CT3103 pPM_Enum_)
        {
            VisionResult_LaserX_Image_Universal visionMatch1 = this.LocalResource.VisionController.GetVisionResult_Universal(cmd, 50);
            var X = this.LocalResource.Axes[axisNameEnum].Get_CurUnitPos();
            var Y = 0;
            var Xppm = this.providerResourse.PPM_Provider[pPM_Enum_].X_ppm;
            var Yppm = this.providerResourse.PPM_Provider[pPM_Enum_].Y_ppm;

            XYCoord coord = this.CalTargetPosition(X, Y, Xppm, Yppm, visionMatch1.ImageWidth / 2, visionMatch1.ImageHeight / 2,
                                                                                       visionMatch1.PeekCenterX_Pix, visionMatch1.PeekCenterY_Pix);
            this.LocalResource.AxesMotionAction[axisNameEnum].SingleAxisMotion(this.LocalResource.Axes[axisNameEnum], coord.X);

            return coord;
        }


        public void Paused_IO(double time)
        {
            Task.Factory.StartNew(() =>
            {

                LocalResource.IOs[IONameEnum_CT3103.TWR_YEL].TurnOn(false);
                LocalResource.IOs[IONameEnum_CT3103.TWR_RED].TurnOn(true);
                LocalResource.IOs[IONameEnum_CT3103.TWR_GRN].TurnOn(false);
                LocalResource.IOs[IONameEnum_CT3103.BEEP].TurnOn(true);
                Thread.Sleep((int)(time * 1000));
                LocalResource.IOs[IONameEnum_CT3103.BEEP].TurnOn(false);

            });
        }

        public void Resume_IO()
        {
            LocalResource.IOs[IONameEnum_CT3103.TWR_YEL].TurnOn(false);
            LocalResource.IOs[IONameEnum_CT3103.TWR_RED].TurnOn(false);
            LocalResource.IOs[IONameEnum_CT3103.TWR_GRN].TurnOn(true);
            LocalResource.IOs[IONameEnum_CT3103.BEEP].TurnOn(false);
        }
    }
}
