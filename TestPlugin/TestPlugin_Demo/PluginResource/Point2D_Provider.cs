using SolveWare_Vision;
using System;
using System.IO;
using System.Windows.Forms;

namespace TestPlugin_Demo
{


    public enum PixPoint2D_Enum_CT3103
    {
        精定位相机_顶针孔_中心点像素,

        精定位相机_吸嘴1_定位片_中心点像素,

        工位1_下相机_吸嘴1_中心点像素,

        工位2_下相机_吸嘴2_中心点像素,

        工位2_下相机_吸嘴3_中心点像素

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