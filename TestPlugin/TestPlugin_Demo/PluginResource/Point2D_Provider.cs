using SolveWare_Vision;
using System;
using System.IO;
using System.Windows.Forms;

namespace TestPlugin_Demo
{


    public enum PixPoint2D_Enum_CT3103
    {
        精定位相机_顶针块_中心点像素,       // Precision positioning camera - ejector pin block - center pixel
        精定位相机_母治1_定位片_中心点像素, // Precision positioning camera - female die 1 - positioning piece - center pixel
        工位1_下相机_母治1_中心点像素,      // Station 1 - bottom camera - female die 1 - center pixel
        工位2_下相机_母治2_中心点像素,      // Station 2 - bottom camera - female die 2 - center pixel
        工位2_下相机_母治3_中心点像素       // Station 2 - bottom camera - female die 3 - center pixel
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