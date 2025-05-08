using SolveWare_Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin_Demo
{
    public class PixPointInfoBase
    {
        public PixPoint2D_Enum_CT3103 PixPoint_Name { get; set; }

        public VisionCalibrationEnum_CT3103 VisionPos { get; set; }

        public int DelayTime { get; set; } = 200;

        public String VisionCMD { get; set; }

        public VisionResult_LaserX_Image_Universal Pos_visionResult { get; set; }

        public PixPoint Pixpoint 
        {
            get 
            {
                if (Pos_visionResult==null)
                {
                    return new PixPoint();
                }
                return new PixPoint(Pos_visionResult.PeekCenterX_Pix, Pos_visionResult.PeekCenterY_Pix);
            } 
        }


    }
}
