using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_Motion;
using SolveWare_Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace TestPlugin_Demo
{
    /// <summary>
    /// 封装CT40410A的轴点位运动方法
    /// </summary>
    public partial class PluginBizService : TesterAppPluginModel
    {

        TestPluginResourceProvider_CT40410A pluginResourse = null;

        ProviderManager_CT40410A providerResource = null;

        public void Setup(TestPluginResourceProvider_CT40410A pluResource, ProviderManager_CT40410A proResource)
        {
            this.pluginResourse = pluResource;
            this.providerResource = proResource;
        }


        public PixPerMM RunMotionAction_To_Calculate_PPM
        (
       VisionCalibrationEnum_CT40410A vcal_phy_pos1,
       VisionCalibrationEnum_CT40410A vcal_phy_pos2,
       AxisNameEnum_CT40410A x_Axis,
       AxisNameEnum_CT40410A y_Axis,
       string vcal_cmd
         )
        {
            VisionResult_LaserX_Image_Universal pos1_visionResult = null;
            VisionResult_LaserX_Image_Universal pos2_visionResult = null;
            return RunMotionAction_To_Calculate_PPM
                    (
                        vcal_phy_pos1,
                        vcal_phy_pos2,
                        x_Axis,
                        y_Axis,
                        vcal_cmd,
                        out pos1_visionResult,
                        out pos2_visionResult
                    );
        }



        public PixPerMM RunMotionAction_To_Calculate_PPM
            (
                VisionCalibrationEnum_CT40410A vcal_phy_pos1,
                VisionCalibrationEnum_CT40410A vcal_phy_pos2,
                AxisNameEnum_CT40410A x_Axis,
                AxisNameEnum_CT40410A y_Axis,
                string vcal_cmd,
                out VisionResult_LaserX_Image_Universal pos1_visionResult,
                out VisionResult_LaserX_Image_Universal pos2_visionResult
            )
        {
            PixPerMM ppmResult = new PixPerMM();
            pos1_visionResult = new VisionResult_LaserX_Image_Universal();
            pos2_visionResult = new VisionResult_LaserX_Image_Universal();

            this.Run_VisionCal_Position(vcal_phy_pos1);
            Thread.Sleep(200);

            pos1_visionResult = this.RunVisionMethod(vcal_cmd, "");

            if (pos1_visionResult.Success)
            {

            }
            else
            {
                var msg = $"在[{vcal_phy_pos1}]识别失败[{pos1_visionResult.ErrorMsg}]!";
                this.Log_File(msg);
                // MessageBox.Show(msg);
                return ppmResult;
            }
            var phy_pos1 = this.Get_2D_Axes_Current_Physical_Position
                            (
                               x_Axis,
                               y_Axis
                            );

            this.Run_VisionCal_Position(vcal_phy_pos2);

            Thread.Sleep(200);
            pos2_visionResult = this.RunVisionMethod(vcal_cmd, "");
            if (pos2_visionResult.Success)
            {

            }
            else
            {
                var msg = $"在[{vcal_phy_pos2}]识别失败[{pos2_visionResult.ErrorMsg}]!";
                this.Log_File(msg);
                //MessageBox.Show(msg);

                return ppmResult;
            }
            var phy_pos2 = this.Get_2D_Axes_Current_Physical_Position
                        (
                               x_Axis,
                               y_Axis
                        );

            ppmResult = this.Calculate_PixPerMM_2D_2Points
            (
                pos1_visionResult,
                pos2_visionResult,
                phy_pos1,
                phy_pos2
            );
            return ppmResult;
        }


        public void RunMotionAction_To_Calculate_PPM(PPMInfoBase pPMInfo)
        {
            this.Run_VisionCal_Position(pPMInfo.VisionPos1);
            Thread.Sleep(pPMInfo.DelayTime);

            pPMInfo.Pos1_visionResult = this.CameraRecognize(pPMInfo.VisionCMD);

            var phy_pos1 = this.Get_2D_Axes_Current_Physical_Position(pPMInfo.X_Axis, pPMInfo.Y_Axis);

            this.Run_VisionCal_Position(pPMInfo.VisionPos2);
            Thread.Sleep(pPMInfo.DelayTime);

            pPMInfo.Pos2_visionResult = this.CameraRecognize(pPMInfo.VisionCMD);

            var phy_pos2 = this.Get_2D_Axes_Current_Physical_Position(pPMInfo.X_Axis, pPMInfo.Y_Axis);

            pPMInfo.PixperMM = this.Calculate_PixPerMM_2D_2Points(pPMInfo.Pos1_visionResult, pPMInfo.Pos2_visionResult, phy_pos1, phy_pos2);

        }


        public void RunMotionAction_To_Verify_PPM(PPMInfoBase pPMInfo)
        {
            this.Run_VisionCal_Position(pPMInfo.VisionPos3_Debug);
            Thread.Sleep(pPMInfo.DelayTime);
            pPMInfo.Pos3_Debug_visionResult = this.CameraRecognize(pPMInfo.VisionCMD);

            var phy_pos3 = this.Get_2D_Axes_Current_Physical_Position(pPMInfo.X_Axis, pPMInfo.Y_Axis);

            var delta_pos3_pos1_pix_x = pPMInfo.Pos3_Debug_visionResult.PeekCenterX_Pix - pPMInfo.Pos1_visionResult.PeekCenterX_Pix;
            var delta_pos3_pos1_pix_y = pPMInfo.Pos3_Debug_visionResult.PeekCenterY_Pix - pPMInfo.Pos1_visionResult.PeekCenterY_Pix;

            var xy_mm = this.ConvertDistance_PixToMM(pPMInfo.PixperMM, delta_pos3_pos1_pix_x, delta_pos3_pos1_pix_y);

            var destPos3_act_pos_x = phy_pos3.X - xy_mm.X;
            var destPos3_act_pos_y = phy_pos3.Y - xy_mm.Y;

            Dictionary<AxisNameEnum_CT40410A, double> dict = new Dictionary<AxisNameEnum_CT40410A, double>();
            dict.Add(pPMInfo.X_Axis, destPos3_act_pos_x);
            dict.Add(pPMInfo.Y_Axis, destPos3_act_pos_y);

            this.Parellel_MoveTo_Abs_Position(dict);
            Thread.Sleep(pPMInfo.DelayTime);
            pPMInfo.Pos1_Debug_visionResult = this.CameraRecognize(pPMInfo.VisionCMD);
        }

        public void RunMotionAction_To_Calculate_Deltapix(DeltapixInfoBase deltapixInfo)
        {
            this.Run_VisionCal_Position(deltapixInfo.VisionPos1);
            Thread.Sleep(deltapixInfo.DelayTime);
            deltapixInfo.Pos1_visionResult = this.CameraRecognize(deltapixInfo.VisionCMD1);
            deltapixInfo.Pos1_Coord = this.Get_3D_Axes_Current_Physical_Position(deltapixInfo.X_Axis, deltapixInfo.Y_Axis, deltapixInfo.Z_Axis);

            this.Run_VisionCal_Position(deltapixInfo.VisionPos2);
            Thread.Sleep(deltapixInfo.DelayTime);
            deltapixInfo.Pos2_visionResult = this.CameraRecognize(deltapixInfo.VisionCMD2);
            deltapixInfo.Pos2_Coord = this.Get_3D_Axes_Current_Physical_Position(deltapixInfo.X_Axis, deltapixInfo.Y_Axis, deltapixInfo.Z_Axis);


            var ppm_x = deltapixInfo.PixperMM.X_ppm;
            var ppm_y = deltapixInfo.PixperMM.Y_ppm;


            var delta_pos2_pos1_pos_x = deltapixInfo.Pos2_Coord.X - deltapixInfo.Pos1_Coord.X;
            var delta_pos2_pos1_pos_y = deltapixInfo.Pos2_Coord.Y - deltapixInfo.Pos1_Coord.Y;

            var delta_pos2_pos1_pix_x = ppm_x * delta_pos2_pos1_pos_x;
            var delta_pos2_pos1_pix_y = ppm_y * delta_pos2_pos1_pos_y;

            deltapixInfo.Dp2p.Delta_X_Pix = delta_pos2_pos1_pix_x;
            deltapixInfo.Dp2p.Delta_Y_Pix = delta_pos2_pos1_pix_y;

        }

        public void RunMotionAction_To_Verify_Deltapix(DeltapixInfoBase deltapixInfo)
        {
            var xy_mm = this.ConvertDistance_PixToMM(deltapixInfo.PixperMM, deltapixInfo.Dp2p.Delta_X_Pix, deltapixInfo.Dp2p.Delta_Y_Pix);

            var act_pos_x = deltapixInfo.Pos2_Coord.X - xy_mm.X;
            var act_pos_y = deltapixInfo.Pos2_Coord.Y - xy_mm.Y;

            Dictionary<AxisNameEnum_CT40410A, double> dict = new Dictionary<AxisNameEnum_CT40410A, double>();
            dict.Add(deltapixInfo.X_Axis, act_pos_x);
            dict.Add(deltapixInfo.Y_Axis, act_pos_y);
            dict.Add(deltapixInfo.Z_Axis, deltapixInfo.Pos1_Coord.Z);

            this.Parellel_MoveTo_Abs_Position(dict);
            Thread.Sleep(deltapixInfo.DelayTime);
            deltapixInfo.Pos1_Debug_visionResult = this.CameraRecognize(deltapixInfo.VisionCMD1);
        }

        public PixPoint RunMotionAction_To_Calculate_PixPoint(VisionCalibrationEnum_CT40410A visionCalibrationEnum, string vision_CMD)
        {
            this.Run_VisionCal_Position(visionCalibrationEnum);
            Thread.Sleep(200);
            var VisionCalResult = this.CameraRecognize(vision_CMD);
            return new PixPoint(VisionCalResult.PeekCenterX_Pix, VisionCalResult.PeekCenterY_Pix);
        }

        public void RunMotionAction_To_Calculate_PixPoint(PixPointInfoBase pixPointInfo)
        {
            this.Run_VisionCal_Position(pixPointInfo.VisionPos);
            Thread.Sleep(pixPointInfo.DelayTime);
            pixPointInfo.Pos_visionResult = this.CameraRecognize(pixPointInfo.VisionCMD);

        }



        public void Nozzle_Move_With_offset_manual(
            VisionCalibrationEnum_CT40410A visCalPos,
            DeltaPix2Point_Enum_CT40410A deltaPix2Point,
            PPM_Enum_CT40410A pPM,
            AxisNameEnum_CT40410A axisX_Name,
            AxisNameEnum_CT40410A axisY_Name,
            double offset_manual_x,
            double offset_manual_y)
        {
            string axisX_name = Enum.GetName(typeof(AxisNameEnum_CT40410A), axisX_Name);

            string axisY_name = Enum.GetName(typeof(AxisNameEnum_CT40410A), axisY_Name);

            if (this.pluginResourse.VCALPositions.ContainsKey(visCalPos) == false)
            {
                throw new Exception($"资源池内不包含该点位[{visCalPos}]!!");
            }

            var position = this.pluginResourse.VCALPositions[visCalPos];
            double axisX_pos = 0;
            double axisY_pos = 0;

            foreach (var item in position)
            {
                if (item.Name == axisX_name)
                {
                    axisX_pos = item.Position;
                }
                else if (item.Name == axisY_name)
                {
                    axisY_pos = item.Position;
                }
            }

            var delta_pix_x = this.providerResource.Dp2p_Provider[deltaPix2Point].Delta_X_Pix;
            var delta_pix_y = this.providerResource.Dp2p_Provider[deltaPix2Point].Delta_Y_Pix;
            var xy_mm = this.ConvertDistance_PixToMM(this.providerResource.PPM_Provider[pPM],
                delta_pix_x, delta_pix_y);

            var act_pos_x = axisX_pos + xy_mm.X + offset_manual_x;
            var act_pos_y = axisY_pos + xy_mm.Y + offset_manual_y;

            Dictionary<AxisNameEnum_CT40410A, double> dict = new Dictionary<AxisNameEnum_CT40410A, double>();
            dict.Add(axisX_Name, act_pos_x);
            dict.Add(axisY_Name, act_pos_y);

            this.Parellel_MoveTo_Abs_Position(dict);
        }


        public void Nozzle_Move_With_offset_manualAndvision(
           VisionCalibrationEnum_CT40410A visCalPos,
           DeltaPix2Point_Enum_CT40410A deltaPix2Point,
           PPM_Enum_CT40410A pPM,
           AxisNameEnum_CT40410A axisX_Name,
           AxisNameEnum_CT40410A axisY_Name,
           double offset_manual_x = 0,
           double offset_manual_y = 0,
           double offset_vision_x = 0,
           double offset_vision_y = 0)
        {
            string axisX_name = Enum.GetName(typeof(AxisNameEnum_CT40410A), axisX_Name);
            string axisY_name = Enum.GetName(typeof(AxisNameEnum_CT40410A), axisY_Name);

            if (this.pluginResourse.VCALPositions.ContainsKey(visCalPos) == false)
            {
                throw new Exception($"资源池内不包含该点位[{visCalPos}]!!");
            }

            var position = this.pluginResourse.VCALPositions[visCalPos];
            double axisX_pos = 0;
            double axisY_pos = 0;

            foreach (var item in position)
            {
                if (item.Name == axisX_name)
                {
                    axisX_pos = item.Position;
                }
                else if (item.Name == axisY_name)
                {
                    axisY_pos = item.Position;
                }
            }

            var delta_pix_x = this.providerResource.Dp2p_Provider[deltaPix2Point].Delta_X_Pix;
            var delta_pix_y = this.providerResource.Dp2p_Provider[deltaPix2Point].Delta_Y_Pix;
            var xy_mm = this.ConvertDistance_PixToMM(this.providerResource.PPM_Provider[pPM],
                delta_pix_x, delta_pix_y);

            var act_pos_x = axisX_pos + xy_mm.X + offset_manual_x - offset_vision_x;
            var act_pos_y = axisY_pos + xy_mm.Y + offset_manual_y - offset_vision_y;

            Dictionary<AxisNameEnum_CT40410A, double> dict = new Dictionary<AxisNameEnum_CT40410A, double>();
            dict.Add(axisX_Name, act_pos_x);
            dict.Add(axisY_Name, act_pos_y);

            this.Parellel_MoveTo_Abs_Position(dict);

        }





        /// <summary>
        /// Wait_IO_Open
        /// </summary>
        /// <param name="ioName"></param>
        /// <param name="timeSpan">最大等待时间</param>
        /// <param name="errorMsg"></param>
        public void Wait_IO_Open(IONameEnum_CT40410A ioName, int timeSpan, string errorMsg)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            do
            {
                var result = this.pluginResourse.IOs[ioName].Interation.IsActive;
                if (result == true)
                {
                    break;
                }
                if (sw.Elapsed.TotalMilliseconds > timeSpan)
                {
                    throw new Exception(errorMsg);
                }
                Thread.Sleep(20);
            } while (true);

        }


        /// <summary>
        /// Wait_IO_Close
        /// </summary>
        /// <param name="ioName"></param>
        /// <param name="timeSpan">最大等待时间</param>
        /// <param name="errorMsg"></param>
        public void Wait_IO_Close(IONameEnum_CT40410A ioName, int timeSpan, string errorMsg)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            do
            {
                var result = this.pluginResourse.IOs[ioName].Interation.IsActive;
                if (result == false)
                {
                    break;
                }
                if (sw.Elapsed.TotalMilliseconds > timeSpan)
                {
                    throw new Exception(errorMsg);
                }
                Thread.Sleep(20);
            } while (true);
        }

        public void OutputControl(Dictionary<IONameEnum_CT40410A, bool> dict)
        {
            foreach (var item in dict)
            {
                var ioName = item.Key;
                var isEnable = item.Value;

                this.pluginResourse.IOs[ioName].TurnOn(isEnable);

                this.pluginResourse.Log_File("吸嘴1吹大气电磁阀关闭!");

                if (isEnable == true)
                {
                    Wait_IO_Open(ioName, 5000, $"[{ioName}]信号检测超时");
                }
                else if (isEnable == false)
                {
                    Wait_IO_Close(ioName, 5000, $"[{ioName}]信号检测超时");
                }
            }
        }


        /// <summary>
        /// 吸嘴模组下降
        /// </summary>
        /// <param name="iOName">金属触点名称</param>
        /// <param name="axisName">Z轴名称</param>
        /// <param name="circleDistance"></param>
        /// <param name="timeSpan">最大运动时间</param>
        /// <param name="limitDistance">最大运动距离</param>
        public void Axis_Z_Drop(IONameEnum_CT40410A iOName, AxisNameEnum_CT40410A axisName, double circleDistance, int timeSpan, double limitDistance)
        {
            //下降前检查
            var result_CheckBeforeDrop = this.pluginResourse.IOs[iOName].Interation.IsActive;
            if (result_CheckBeforeDrop == false)
            {
                throw new Exception($"吸嘴金属触点状态异常[{iOName}]，请检查！");
                //return;
            }

            if (circleDistance < 0 || circleDistance > 1)
            {
                throw new Exception($"循环位移设置不合理，请重新设置！");
                //return;
            }

            if (limitDistance < 0)
            {
                throw new Exception($"运动距离设置不合理，请重新设置！");
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();

            var axisPara = this.pluginResourse.Axes[axisName];
            var axisMotionAction = this.pluginResourse.AxesMotionAction[axisName];

            var limitPos = axisPara.Interation.CurrentPosition + limitDistance;

            do
            {
                var condition_EndDrop = this.pluginResourse.IOs[iOName].Interation.IsActive;
                if (condition_EndDrop == false)
                {
                    axisPara.Stop();
                    break;
                }
                double curPos = axisPara.Interation.CurrentPosition;
                double targetPos = curPos + circleDistance;
                if (targetPos > limitPos)
                {
                    axisPara.Stop();
                    break;
                }
                int error = axisMotionAction.SingleAxisMotion(axisPara, targetPos);
                if (sw.Elapsed.TotalMilliseconds > timeSpan)
                {
                    axisPara.Stop();
                    throw new Exception("运动超时！");
                }

            } while (true);

        }

        /// <summary>
        /// 吸嘴模组下降
        /// </summary>
        /// <param name="iOName">金属触点信号</param>
        /// <param name="axisName">Z轴名称</param>
        /// <param name="circleDistance">循环移动距离</param>
        public void Axis_Z_Drop(IONameEnum_CT40410A iOName, AxisNameEnum_CT40410A axisName, double circleDistance, SpeedLevel spLvl = SpeedLevel.Normal)
        {
            //下降前检查
            var result_CheckBeforeDrop = this.pluginResourse.IOs[iOName].Interation.IsActive;
            if (result_CheckBeforeDrop == false)
            {
                Thread.Sleep(10);
                result_CheckBeforeDrop = this.pluginResourse.IOs[iOName].Interation.IsActive; 
                if (result_CheckBeforeDrop == false)
                {
                    Thread.Sleep(10);
                    result_CheckBeforeDrop = this.pluginResourse.IOs[iOName].Interation.IsActive;
                    if (result_CheckBeforeDrop == false)
                    {
                        throw new Exception($"吸嘴金属触点状态异常[{iOName}]，请检查！");
                    }
                    
                }
            }
            if (circleDistance < 0 || circleDistance > 1)
            {
                throw new Exception($"循环位移设置不合理，请重新设置！");
            }
            var axisPara = this.pluginResourse.Axes[axisName];
            var axisMotionAction = this.pluginResourse.AxesMotionAction[axisName];


            Func<bool> func = new Func<bool>(() =>
            {
                var condition_EndDrop = this.pluginResourse.IOs[iOName].Interation.IsActive;
                if (condition_EndDrop == false)
                {
                    return true;
                }
                return false;
            });


            do
            {
                double curPos = axisPara.Interation.CurrentPosition;
                double targetPos = curPos + circleDistance;
                int errorCode = axisMotionAction.SingleAxisMotion(axisPara, targetPos, spLvl, func);
                if (errorCode != ErrorCodes.NoError)
                {
                    axisPara.Stop();
                    break;
                }

                var condition_EndDrop = this.pluginResourse.IOs[iOName].Interation.IsActive;
                if (condition_EndDrop == false)
                {
                    axisPara.Stop();
                    break;
                }
            } while (true);
        }





        //真空打开后，才可以判别流量计信号

        //吸嘴

        //下降前：检查金属触点是否断开。闭合下降。断开报异常

        //下降至缓冲位过程中：检查金属触点是否断开。断开报异常。缓冲位设置高度过低

        //上升至待机位后：
        //1.检查流量计是否有信号。无信号报异常。吸嘴未吸取到物料、流量计设置异常、传感器异常
        //2.检查金属触点是否断开。闭合正常，断开报异常。金属触点回弹异常，传感器异常

        //吸嘴平移过程中：检查流量计是否有信号。无信号报异常。芯片丢失、流量计设置异常、传感器异常、吸嘴吸料不稳




        /// <summary>
        /// 测试台取料
        /// </summary>
        /// <param name="plat_Vac_Output"></param>
        /// <param name="plat_Vac_FlowMeter_Input"></param>
        /// <param name="nozzle_Vac_Output"></param>
        public void PlatTakeChip(IONameEnum_CT40410A plat_Vac_Output, IONameEnum_CT40410A plat_Vac_FlowMeter_Input, IONameEnum_CT40410A nozzle_Vac_Output,
            int delayTime1 = 200, int delayTime2 = 200, int delayTime3 = 500)
        {
            //Z轴下降后、载台真空开启前延时
            Thread.Sleep(delayTime1);

            this.pluginResourse.IOs[plat_Vac_Output].TurnOn(true);
            this.Log_File($"测试台上下料位置，载台真空电磁阀打开!");
            Wait_IO_Open(plat_Vac_Output, 5000, $"测试台上下料位置，载台真空电磁阀[{plat_Vac_Output}]打开异常");
            this.Log_File($"测试台上下料位置，载台真空已打开!");

            //载台真空开启后、吸嘴吹小气前延时
            Thread.Sleep(delayTime2);

            var feedbackResult = this.pluginResourse.IOs[plat_Vac_FlowMeter_Input].Interation.IsActive;
            if (feedbackResult == false)
            {
                //MessageBox.Show("测试台1上下料位置，真空流量计检测异常");
                this.Log_File($"测试台上下料位置，真空流量计[{plat_Vac_FlowMeter_Input}]检测异常!");
            }

            this.pluginResourse.IOs[nozzle_Vac_Output].TurnOn(false);
            this.Log_File($"测试台上下料位置,吸嘴真空电磁阀关闭!");
            Wait_IO_Close(nozzle_Vac_Output, 5000, $"测试台上下料位置,吸嘴真空电磁阀{nozzle_Vac_Output}关闭异常");
            this.Log_File("测试台上下料位置,吸嘴真空已关闭!");

            //吸嘴吹小气后、Z轴提升前延时
            Thread.Sleep(delayTime3);
        }

        /// <summary>
        /// 吸嘴取料
        /// </summary>
        /// <param name="nozzle_Vac_Output"></param>
        /// <param name="nozzle_Vac_FlowMeter_Input"></param>
        /// <param name="testbench_Vac_Output"></param>
        public void NozzleTakeChip(IONameEnum_CT40410A nozzle_Vac_Output, IONameEnum_CT40410A nozzle_Vac_FlowMeter_Input, IONameEnum_CT40410A testbench_Vac_Output,
            int delayTime1 = 200, int delayTime2 = 200, int delayTime3 = 500)
        {
            //Z轴下降后、吸嘴抽真空开启前延时
            Thread.Sleep(delayTime1);

            this.pluginResourse.IOs[nozzle_Vac_Output].TurnOn(true);
            this.Log_File($"测试台上下料位置，吸嘴真空电磁阀打开!");
            Wait_IO_Open(nozzle_Vac_Output, 5000, $"测试台上下料位置，吸嘴真空电磁阀[{nozzle_Vac_Output}]打开异常");
            this.Log_File($"测试台上下料位置，吸嘴真空已打开!");

            //吸嘴抽真空后、载台关真空前延时
            Thread.Sleep(delayTime2);

            var feedbackResult = this.pluginResourse.IOs[nozzle_Vac_FlowMeter_Input].Interation.IsActive;
            if (feedbackResult == false)
            {
                this.Log_File($"测试台上下料位置，吸嘴真空流量计[{nozzle_Vac_FlowMeter_Input}]检测异常!");
            }

            this.pluginResourse.IOs[testbench_Vac_Output].TurnOn(false);
            this.Log_File($"测试台上下料位置,孔位真空电磁阀关闭!");
            Wait_IO_Close(testbench_Vac_Output, 5000, $"测试台上下料位置,孔位真空电磁阀{testbench_Vac_Output}关闭异常");
            this.Log_File("测试台上下料位置,孔位真空已关闭!");

            //载台关真空后，Z轴提升前延时
            Thread.Sleep(delayTime3);
        }


        /// <summary>
        /// 模组移动至相机位置拍照
        /// </summary>
        /// <param name="visionCalPosition"></param>
        /// <param name="visionCMD"></param>
        public void MoveAndVisionCal(VisionCalibrationEnum_CT40410A visionCalPosition, string visionCMD)
        {
            this.Run_VisionCal_Position(visionCalPosition);
            Thread.Sleep(200);
            var VisionCalResult = this.RunVisionMethod(visionCMD, "");
            if (VisionCalResult.Success)
            {
                this.Log_File($"在当前位置[{visionCalPosition}]触发相机指令[{visionCMD}]识别成功！");
            }
            else
            {
                this.Log_File($"在当前位置[{visionCalPosition}]触发相机指令[{visionCMD}]识别失败！[{VisionCalResult.ErrorMsg}]");
                throw new Exception($"在当前位置[{visionCalPosition}]触发相机指令[{visionCMD}]识别失败！[{VisionCalResult.ErrorMsg}]");
            }
        }




        /// <summary>
        /// 移动至下相机拍照并补偿角度
        /// </summary>
        /// <param name="visionCalPosition">相机识别位置</param>
        /// <param name="visionCMD">相机识别命令</param>
        /// <param name="angleAxisName">角度补偿轴</param>
        public void CompensateAngle(VisionCalibrationEnum_CT40410A visionCalPosition, string visionCMD, AxisNameEnum_CT40410A angleAxisName, double angle_limit = 15, int calibrationTimes = 1)
        {
            this.Run_VisionCal_Position(visionCalPosition);
            Thread.Sleep(200);
            VisionResult_LaserX_Image_Universal VisionCalResult = null;
            int times = 0;
            do
            {
                VisionCalResult = this.RunVisionMethod(visionCMD, 200);
                if (VisionCalResult.Success)
                {
                    this.Log_File($"在当前位置[{visionCalPosition}]相机识别成功！识别次数[{times + 1}]");
                    break;
                }
                else
                {
                    times++;
                    this.Log_File($"在当前位置[{visionCalPosition}]相机第[{times}]次识别失败！[{VisionCalResult.ErrorMsg}]");
                }
                if (times >= calibrationTimes)
                {
                    throw new Exception($"在当前位置[{visionCalPosition}]相机识别失败！总识别次数[{times}][{VisionCalResult.ErrorMsg}]");
                }
            } while (true);


            var delta_angle = VisionCalResult.PeekAngle;

            var angle_abs = Math.Abs(delta_angle);

            if (angle_abs > angle_limit)
            {
                throw new Exception($"在当前位置[{visionCalPosition}]相机触发[{visionCMD}]命令，芯片角度偏转过大！");
            }

            var axisPara = this.pluginResourse.Axes[angleAxisName];

            var axisMotionAction = this.pluginResourse.AxesMotionAction[angleAxisName];

            double curPos = axisPara.Interation.CurrentPosition;

            double targetPos = (curPos - delta_angle);

            axisMotionAction.SingleAxisMotion(axisPara, targetPos);
            Thread.Sleep(200);


            var VCalResultAfterComp = this.CameraRecognize(visionCMD);

            this.Log_File($"补偿前角度{VisionCalResult.PeekAngle},补偿后角度{VCalResultAfterComp.PeekAngle}");
        }


        /// <summary>
        /// 移动至下相机拍照并补偿角度
        /// 返回需要补偿的XY距离
        /// </summary>
        /// <param name="visionCalPosition"></param>
        /// <param name="visionCMD"></param>
        /// <param name="angleAxisName"></param>
        /// <param name="pixPoint2D"></param>
        /// <param name="pPM"></param>
        /// <param name="calibrationTimes"></param>
        /// <returns></returns>
        public XYCoord BottomCam_CompensateAngle(VisionCalibrationEnum_CT40410A visionCalPosition,
            string visionCMD,
            AxisNameEnum_CT40410A angleAxisName,
            PixPoint2D_Enum_CT40410A pixPoint2D,
            PPM_Enum_CT40410A pPM,
            double angle_limit = 15)
        {
            this.Run_VisionCal_Position(visionCalPosition);
            Thread.Sleep(200);
            var cmdList = this.providerResource.VisionComboCommand_Provider[visionCMD];
            VisionResult_LaserX_Image_Universal visionResult = new VisionResult_LaserX_Image_Universal();
            foreach (var cmd in cmdList)
            {
                visionResult = CameraRecognize(cmd, false);
                if (visionResult.Success == true)
                {
                    break;
                }
                else if (visionResult.Success == false)
                {
                }
                Thread.Sleep(20);
            }
            if (visionResult.Success == false)
            {
                //【1】校正前识别error
                throw new Exception($"在当前位置[{visionCalPosition}]相机识别失败！");
            }

            var delta_angle = visionResult.PeekAngle;

            var angle_abs = Math.Abs(delta_angle);

            if (angle_abs > angle_limit)
            {
                //【2】校正中角度偏转过大
                throw new Exception($"在当前位置[{visionCalPosition}]相机触发[{visionCMD}]命令，芯片角度偏转过大！");
            }

            var axisPara = this.pluginResourse.Axes[angleAxisName];

            var axisMotionAction = this.pluginResourse.AxesMotionAction[angleAxisName];

            double curPos = axisPara.Interation.CurrentPosition;

            double targetPos = (curPos - delta_angle);

            axisMotionAction.SingleAxisMotion(axisPara, targetPos);
            Thread.Sleep(200);


            var VCalResultAfterComp = this.CameraRecognize(visionCMD);
            //【3】校正后识别不到

            var xy_mm = this.Calculate_PhyOffset_Marker_Marker(pixPoint2D, pPM, VCalResultAfterComp);

            this.Log_File($"补偿前角度{visionResult.PeekAngle},补偿后角度{VCalResultAfterComp.PeekAngle}");
            return new XYCoord(xy_mm.X, xy_mm.Y);
        }

        /// <summary>
        /// 移动至下相机拍照并补偿角度
        /// 返回需要补偿的XY距离
        /// </summary>
        /// <param name="visionCalPosition"></param>
        /// <param name="visionCMD"></param>
        /// <param name="angleAxisName"></param>
        /// <param name="pixPoint2D"></param>
        /// <param name="pPM"></param>
        /// <param name="angle_limit"></param>
        /// <returns></returns>
        public OperationResult<XYCoord> BottomCam_IdentityAndCompensate(
            VisionCalibrationEnum_CT40410A visionCalPosition,
            string visionCMD,
            AxisNameEnum_CT40410A angleAxisName,
            PixPoint2D_Enum_CT40410A pixPoint2D,
            PPM_Enum_CT40410A pPM,
            double angle_limit = 15)
        {
            int errorCode = 0;
            string errorMsg = string.Empty;

            this.Run_VisionCal_Position(visionCalPosition);

            Thread.Sleep(200);
            var cmdList = this.providerResource.VisionComboCommand_Provider[visionCMD];
            VisionResult_LaserX_Image_Universal visionResult = new VisionResult_LaserX_Image_Universal();
            foreach (var cmd in cmdList)
            {
                visionResult = CameraRecognize(cmd, false);
                if (visionResult.Success == true)
                {
                    break;
                }
                else if (visionResult.Success == false)
                {
                }
                Thread.Sleep(20);
            }
            if (visionResult.Success == false)
            {
                //【1】校正前识别error
                //errorMsg = $"[校正前]在当前位置[{ visionCalPosition}]相机识别失败";

                errorCode = ErrorCode_CT40410A.Error_校正前_相机识别异常;
                errorMsg = ErrorCode_CT40410A.ErrorMessageMap[errorCode];
                return OperationResult.FailResult(new XYCoord(), errorMsg, errorCode);
            }


            var delta_angle = visionResult.PeekAngle;
            var angle_abs = Math.Abs(delta_angle);
            if (angle_abs > angle_limit)
            {
                //【2】校正中角度偏转过大
                //throw new Exception($"在当前位置[{visionCalPosition}]相机触发[{visionCMD}]命令，芯片角度偏转过大！");
                //errorMsg = $"[校正中]在当前位置[{visionCalPosition}]相机触发[{visionCMD}]命令，芯片角度偏转过大！";
                //errorCode = 22222;
                errorCode = ErrorCode_CT40410A.Error_校正中_角度偏转过大;
                errorMsg = ErrorCode_CT40410A.ErrorMessageMap[errorCode];

                return OperationResult.FailResult(new XYCoord(), errorMsg, errorCode);
            }

            var axisPara = this.pluginResourse.Axes[angleAxisName];
            var axisMotionAction = this.pluginResourse.AxesMotionAction[angleAxisName];
            double curPos = axisPara.Interation.CurrentPosition;
            double targetPos = (curPos - delta_angle);
            axisMotionAction.SingleAxisMotion(axisPara, targetPos);


            Thread.Sleep(200);
            VisionResult_LaserX_Image_Universal VCalResultAfterComp = new VisionResult_LaserX_Image_Universal();
            foreach (var cmd in cmdList)
            {
                VCalResultAfterComp = CameraRecognize(cmd, false);
                if (VCalResultAfterComp.Success == true)
                {
                    break;
                }
                else if (VCalResultAfterComp.Success == false)
                {
                }
                Thread.Sleep(20);
            }
            if (VCalResultAfterComp.Success == false)
            {
                //【3】校正后识别error
                //errorMsg = $"[校正后]在当前位置[{ visionCalPosition}]相机识别失败";
                //errorCode = 22222;
                errorCode = ErrorCode_CT40410A.Error_校正后_相机识别异常;
                errorMsg = ErrorCode_CT40410A.ErrorMessageMap[errorCode];
                return OperationResult.FailResult(new XYCoord(), errorMsg, errorCode);
            }

            var xy_mm = this.Calculate_PhyOffset_Marker_Marker(pixPoint2D, pPM, VCalResultAfterComp);
            this.Log_File($"补偿前角度{visionResult.PeekAngle},补偿后角度{VCalResultAfterComp.PeekAngle}");

            return OperationResult.SuccessResult(new XYCoord(xy_mm.X, xy_mm.Y));
        }

        public void Nozzle_Compensate_Angle(
                VisionResult_LaserX_Image_Universal visionResult,
                AxisNameEnum_CT40410A angleAxisName,
                double angle_limit = 15)
        {


            var delta_angle = visionResult.PeekAngle;
            var angle_abs = Math.Abs(delta_angle);
            if (angle_abs > angle_limit)
            {
                throw new Exception("Nozzle_Compensate_Angle_校正中_角度偏转过大");
            }
            var axisPara = this.pluginResourse.Axes[angleAxisName];
            var axisMotionAction = this.pluginResourse.AxesMotionAction[angleAxisName];
            double curPos = axisPara.Interation.CurrentPosition;
            double targetPos = (curPos - delta_angle);
            axisMotionAction.SingleAxisMotion(axisPara, targetPos);
            this.Log_Global($"补偿前角度{visionResult.PeekAngle}");
        }


        public XYCoord BottomCam_Identity_Cal_Offset(
            string visionCMD,
            PixPoint2D_Enum_CT40410A pixPoint2D,
            PPM_Enum_CT40410A pPM)
        {
            VisionResult_LaserX_Image_Universal VCalResultAfterComp = new VisionResult_LaserX_Image_Universal();

            var cmdList = this.providerResource.VisionComboCommand_Provider[visionCMD];
            foreach (var cmd in cmdList)
            {
                VCalResultAfterComp = CameraRecognize(cmd, false);
                if (VCalResultAfterComp.Success == true)
                {
                    break;
                }
                else if (VCalResultAfterComp.Success == false)
                {
                }
                Thread.Sleep(20);
            }
            if (VCalResultAfterComp.Success == false)
            {
                throw new Exception("角度校正后识别异常");
            }
            var xy_mm = this.Calculate_PhyOffset_Marker_Marker(pixPoint2D, pPM, VCalResultAfterComp);
            this.Log_Global($"补偿后角度{VCalResultAfterComp.PeekAngle}");
            return new XYCoord(xy_mm.X, xy_mm.Y);
        }


    }
}
