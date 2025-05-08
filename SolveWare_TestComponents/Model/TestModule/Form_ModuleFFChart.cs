using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_TestComponents.Model
{
    public partial class Form_ModuleFFChart : Form
    {
        public Form_ModuleFFChart()
        {
            InitializeComponent();
        }
        public void ClearffChart() 
        {
            double[] pos = new double[] { 0, 0, 0, 0, 0, 0 };
            double[] ffh = new double[] { 0, 0, 0, 0, 0, 0 };
            double[] ffv = new double[] { 0, 0, 0, 0, 0, 0 };
            this.optical_Beam_1st.SetData("12345678", pos, ffh, pos, ffv);
            this.optical_Beam_2nd.SetData("12345678", pos, ffh, pos, ffv);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chartpos">位置，TRUE为左FALSE右</param> 
        /// <param name="sn"></param>
        /// <param name="pos">角度</param>
        /// <param name="ffh">水平方向的power</param>
        /// <param name="ffv">竖直方向</param>
        public void Setdata(bool chartpos, string sn, double[] pos, double[] ffh, double[] ffv)
        {
            if (chartpos) //true为左，false为右
            {
                this.optical_Beam_1st.SetData(sn, pos, ffh, pos, ffv);
            }
            else
            {
                this.optical_Beam_2nd.SetData(sn, pos, ffh, pos, ffv);
            }

        }
        public void SaveImage(bool chartpos, string filename)
        {
            Image pic = null;
            if (chartpos) //true为左，false为右
            {
                pic = this.optical_Beam_1st.GetPicture();
            }
            else
            {
                pic = this.optical_Beam_2nd.GetPicture();
            }
            pic.Save(filename, System.Drawing.Imaging.ImageFormat.Bmp);
           // image.Save("c:\\aaa.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
        }
    }
}
