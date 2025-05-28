using SolveWare_Vision;
using System;
using System.IO;
using System.Windows.Forms;

namespace TestPlugin_Demo
{


    public enum PixPoint2D_Enum_CT3103
    {
        精定位相机_顶针口_中心点像素,       // Precision camera, pin port, center pixel
        精定位相机_腔体1_定位片_中心点像素,  // Precision camera, cavity 1, positioning piece, center pixel
        工位1_下相机_腔体1_中心点像素,      // Workstation 1, bottom camera, cavity 1, center pixel
        工位2_下相机_腔体2_中心点像素,      // Workstation 2, bottom camera, cavity 2, center pixel
        工位2_下相机_腔体3_中心点像素       // Workstation 2, bottom camera, cavity 3, center pixel
    }

    public class PixelPoint_Provider_CT3103 : PixelPoint_ProviderBase
    {
        public PixelPoint_Provider_CT3103() : base()
        {

        }

        public void Add(PixPoint2D_Enum_CT3103 ppmName, PixPoint ppm)
        {
            this.Add(ppmName.ToString(), ppm);
        }
        public PixPoint this[PixPoint2D_Enum_CT3103 ppmName]
        {
            get
            {
                return this[ppmName.ToString()];
            }
        }
    }

}