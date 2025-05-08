using SolveWare_Vision;

namespace TestPlugin_Demo
{
    public enum DeltaPix2Point_Enum_CT3103
    {
        工站1下相机_吸嘴1_定位片_像素差,

        工站2下相机_吸嘴2_定位片_像素差,

        工站2下相机_吸嘴3_定位片_像素差,
    }

    public class DeltaPix2Point_Provider_CT3103 : DeltaPix2Point_ProviderBase
    {
        public DeltaPix2Point_Provider_CT3103() : base()
        {

        }

        public void Add(DeltaPix2Point_Enum_CT3103 dp2pName, DeltaPix2PointDistance distance)
        {
            this.Add(dp2pName.ToString(), distance);
        }
        public DeltaPix2PointDistance this[DeltaPix2Point_Enum_CT3103 dp2pName]
        {
            get
            {
                return this[dp2pName.ToString()];
            }
        }

    }
}