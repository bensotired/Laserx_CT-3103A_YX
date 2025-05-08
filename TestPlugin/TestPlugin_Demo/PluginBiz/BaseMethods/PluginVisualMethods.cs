using SolveWare_Vision;
using System;
using System.Threading;

namespace TestPlugin_Demo
{
    public partial class PluginBizService
    {
        public VisionResult_LaserX_Image_Universal RunVisionMethod(string cmd, string pn)
        {
            VisionResult_LaserX_Image_Universal result = new VisionResult_LaserX_Image_Universal();
            result = this.pluginResourse.VisionController.GetVisionResult_Universal(cmd, pn);
            return result;
        }

        public VisionResult_LaserX_Image_Universal RunVisionMethod(string cmd, int respon_ms)
        {
            VisionResult_LaserX_Image_Universal result = new VisionResult_LaserX_Image_Universal();
            result = this.pluginResourse.VisionController.GetVisionResult_Universal(cmd, respon_ms);
            return result;
        }

        public VisionResult_LaserX_Image_Universal CameraRecognize(string visionCMD, bool isThrowException = false)
        {
            //int times = 1;
            //int respon_ms = 200;
            //var indentifyTimes = providerResource.RunSettings_Provider._RunParamSettings.VisionIndentifyTimes_Limit;
            //VisionResult_LaserX_Image_Universal VisionCalResult = new VisionResult_LaserX_Image_Universal();
            //do
            //{
            //    VisionCalResult = this.RunVisionMethod(visionCMD, respon_ms);
            //    if (VisionCalResult.Success)
            //    {
            //        this.Log_File($"当前视觉识别，触发[{visionCMD}]命令，第[{times}]次识别成功！");
            //        return VisionCalResult;
            //    }
            //    if (times >= indentifyTimes)
            //    {
            //        this.Log_File($"触发[{visionCMD}]命令，相机识别异常！识别次数[{indentifyTimes}][{VisionCalResult.ErrorMsg}]");
            //        if (isThrowException == true)
            //        {
            //            throw new Exception($"当前触发[{visionCMD}]命令，相机识别异常！[{VisionCalResult.ErrorMsg}]");
            //        }
            //        else
            //        {
            //            return VisionCalResult;

            //        }
            //    }
            //    Thread.Sleep(100);
            //    times++;
            //} while (true);
            return new VisionResult_LaserX_Image_Universal();
        }



        /// <summary>
        /// 通过图像识别结果计算PPM
        /// </summary>
        /// <param name="point1_VResult"></param>
        /// <param name="point2_VResult"></param>
        /// <param name="point1_Phy"></param>
        /// <param name="point2_Phy"></param>
        /// <returns></returns>
        public PixPerMM Calculate_PixPerMM_2D_2Points(
                 VisionResult_LaserX_Image_Universal point1_VResult,
                 VisionResult_LaserX_Image_Universal point2_VResult,
                 XYCoord point1_Phy,
                 XYCoord point2_Phy)
        {
            var delta_x_pix = point2_VResult.PeekCenterX_Pix - point1_VResult.PeekCenterX_Pix;//pix
            var delta_y_pix = point2_VResult.PeekCenterY_Pix - point1_VResult.PeekCenterY_Pix;//pix


            var delta_x_mm = point2_Phy.X - point1_Phy.X;//mm
            var delta_y_mm = point2_Phy.Y - point1_Phy.Y;//mm

            var pixPerMM_x = delta_x_pix / delta_x_mm;
            var pixPerMM_y = delta_y_pix / delta_y_mm;

            return new PixPerMM(pixPerMM_x, pixPerMM_y);
        }



        /// <summary>
        /// 像素转换为实际物理位置
        /// </summary>
        /// <param name="ppmVal"></param>
        /// <param name="delta_x_pix"></param>
        /// <param name="delta_y_pix"></param>
        /// <returns></returns>
        public XYCoord ConvertDistance_PixToMM(PixPerMM ppmVal, double delta_x_pix, double delta_y_pix)
        {
            var delta_mm_x = Convert.ToSingle(delta_x_pix / ppmVal.X_ppm);
            var delta_mm_y = Convert.ToSingle(delta_y_pix / ppmVal.Y_ppm);
            return new XYCoord(delta_mm_x, delta_mm_y);
        }

        /// <summary>
        /// 计算当前图像标记中心点坐标与模板图像标记中心点间的距离
        /// 用于下相机二次识别补偿
        /// </summary>
        /// <param name="pixPoint2D">模板中心点像素坐标</param>
        /// <param name="pPM">模板在当前相机下的PPM</param>
        /// <param name="visionResult">当前拍照结果</param>
        /// <returns>视觉补偿offset</returns>
        public XYCoord Calculate_PhyOffset_Marker_Marker(
            PixPoint2D_Enum_CT40410A pixPoint2D,
            PPM_Enum_CT40410A pPM,
            VisionResult_LaserX_Image_Universal visionResult)
        {
            var nozzle_pix_x = this.providerResource.PixPoint_Provider[pixPoint2D].X_Pix;
            var nozzle_pix_y = this.providerResource.PixPoint_Provider[pixPoint2D].Y_Pix;
            var delta_x_pix = visionResult.PeekCenterX_Pix - nozzle_pix_x;
            var delta_y_pix = visionResult.PeekCenterY_Pix - nozzle_pix_y;
            var xy_mm = this.ConvertDistance_PixToMM(this.providerResource.PPM_Provider[pPM], delta_x_pix, delta_y_pix);
            return new XYCoord(xy_mm.X, xy_mm.Y);
        }


        /// <summary>
        /// 计算图像中心点与当前图像标记中心点间的距离
        /// </summary>
        /// <param name="visionResult"></param>
        /// <param name="pPM"></param>
        /// <returns></returns>
        public XYCoord Calculate_PhyOffset_ImageCenter_Marker(
            VisionResult_LaserX_Image_Universal visionResult,
            PPM_Enum_CT40410A pPM)
        {
            var delta_x_pix = visionResult.ImageCenter_X - visionResult.PeekCenterX_Pix;
            var delta_y_pix = visionResult.ImageCenter_Y - visionResult.PeekCenterY_Pix;

            var xy_mm = this.ConvertDistance_PixToMM(this.providerResource.PPM_Provider[pPM], delta_x_pix, delta_y_pix);

            return new XYCoord(xy_mm.X, xy_mm.Y);
        }
      

    }
}
