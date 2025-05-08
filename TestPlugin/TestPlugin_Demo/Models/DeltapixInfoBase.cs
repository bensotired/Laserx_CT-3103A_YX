using SolveWare_Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin_Demo
{
    public class DeltapixInfoBase
    {
        public DeltapixInfoBase()
        {
            PixperMM = new PixPerMM();
            Pos1_Coord = new XYZCoord();
            Pos2_Coord = new XYZCoord(); 
            Dp2p = new DeltaPix2PointDistance();
        }
        public DeltaPix2Point_Enum_CT3103 Deltapix_Name { get; set; }

        public VisionCalibrationEnum_CT3103 VisionPos1 { get; set; }

        public VisionCalibrationEnum_CT3103 VisionPos2 { get; set; }

        public int DelayTime { get; set; } = 200;

        public AxisNameEnum_CT3103 X_Axis { get; set; }

        public AxisNameEnum_CT3103 Y_Axis { get; set; }

        public AxisNameEnum_CT3103 Z_Axis { get; set; }


        public String VisionCMD1 { get; set; }

        public String VisionCMD2 { get; set; }

        public PPM_Enum_CT3103 PPM_Name { get; set; }

        public PixPerMM PixperMM { get; set; }




        public VisionResult_LaserX_Image_Universal Pos1_visionResult { get; set; }

        public VisionResult_LaserX_Image_Universal Pos2_visionResult { get; set; }

        public VisionResult_LaserX_Image_Universal Pos1_Debug_visionResult { get; set; }

        public XYZCoord Pos1_Coord { get; set; }

        public XYZCoord Pos2_Coord { get; set; }

        public DeltaPix2PointDistance Dp2p { get; set; } 

    }
}
