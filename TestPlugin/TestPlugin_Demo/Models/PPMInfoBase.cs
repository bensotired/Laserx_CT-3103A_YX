using SolveWare_Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin_Demo
{
    public class PPMInfoBase
    {
        public PPM_Enum_CT3103 PPM_Name { get; set; }

        public VisionCalibrationEnum_CT3103 VisionPos1 { get; set; }

        public VisionCalibrationEnum_CT3103 VisionPos2 { get; set; }

        public VisionCalibrationEnum_CT3103 VisionPos3_Debug { get; set; }

        public int DelayTime { get; set; } = 5000;

        public AxisNameEnum_CT3103 X_Axis { get; set; }

        public AxisNameEnum_CT3103 Y_Axis { get; set; }

        public String VisionCMD { get; set; }



        public VisionResult_LaserX_Image_Universal Pos1_visionResult { get; set; }

        public VisionResult_LaserX_Image_Universal Pos2_visionResult { get; set; }

        public VisionResult_LaserX_Image_Universal Pos3_Debug_visionResult { get; set; }

        public VisionResult_LaserX_Image_Universal Pos1_Debug_visionResult { get; set; }

        public PixPerMM PixperMM { get; set; }


        public PPMInfoBase()
        {
            Pos1_visionResult = new VisionResult_LaserX_Image_Universal();
            Pos2_visionResult = new VisionResult_LaserX_Image_Universal();
            Pos3_Debug_visionResult = new VisionResult_LaserX_Image_Universal();
            Pos1_Debug_visionResult = new VisionResult_LaserX_Image_Universal();
            PixperMM = new PixPerMM();
        }





    }
}
