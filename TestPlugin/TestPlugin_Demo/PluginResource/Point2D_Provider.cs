using SolveWare_Vision;
using System;
using System.IO;
using System.Windows.Forms;

namespace TestPlugin_Demo
{


    public enum PixPoint2D_Enum_CT3103
    {
        ����λ���_�����_���ĵ�����,

        ����λ���_����1_��λƬ_���ĵ�����,

        ��λ1_�����_����1_���ĵ�����,

        ��λ2_�����_����2_���ĵ�����,

        ��λ2_�����_����3_���ĵ�����

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