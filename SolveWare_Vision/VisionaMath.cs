using System;
using System.Drawing;

namespace SolveWare_Vision
{
    public static class VisionaMath
    {
        public static PointF ConvertDistance_PixToMM(PixPerMM ppmVal, double x_distance_mm, double y_distance_mm)
        {
            var delta_mm_x = Convert.ToSingle(x_distance_mm / ppmVal.X_ppm);
            var delta_mm_y = Convert.ToSingle(y_distance_mm / ppmVal.Y_ppm);

            return new PointF(delta_mm_x, delta_mm_y);
        }
        public static PixPerMM Calculate_PixPerMM_2D_2Points(VisionResult_LaserX_Image_Universal point1_VResult, VisionResult_LaserX_Image_Universal point2_VResult, PointF point1_Phy, PointF point2_Phy)
        {
            var delta_x_pix = point2_VResult.PeekCenterX_Pix - point1_VResult.PeekCenterX_Pix;//pix
            var delta_y_pix = point2_VResult.PeekCenterY_Pix - point1_VResult.PeekCenterY_Pix;//pix


            var delta_x_mm = point2_Phy.X - point1_Phy.X;//mm
            var delta_y_mm = point2_Phy.Y - point1_Phy.Y;//mm

            var pixPerMM_x = delta_x_pix / delta_x_mm;
            var pixPerMM_y = delta_y_pix / delta_y_mm;

            return new PixPerMM(pixPerMM_x, pixPerMM_y);
        }
    }
}