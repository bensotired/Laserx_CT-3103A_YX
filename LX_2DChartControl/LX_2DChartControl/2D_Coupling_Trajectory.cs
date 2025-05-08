using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace LX_BurnInSolution.Charts
{
    //数据组
    public struct str2DCouplingData
    {
        public string Name;
        public double[] X_Pos_mm;
        public double[] Y_Pos_mm;
        public double[] Power;
    }

    //耦合轨迹显示
    public partial class Coupling_Trajectory : UserControl
    {
        private Graphics g;

        private string UserInfoMsg;

        private const int PicLeftWidth = 100; //图像左侧色带 200像素

        private const float PicZoon = 0.1f; // 0.1分度
        private const int PicWidth = (int)(180 / PicZoon); //图像宽度    180度  0.1分度
        private const int PicHeight = (int)(180 / PicZoon); //图像高度   180度  0.1分度

        private const int PicCenter_X = PicWidth / 2 + PicLeftWidth;
        private const int PicCenter_Y = PicHeight / 2;

        private const int PicCenter_Xpos = 900; //0度的地方是位置90度;
        private const int PicCenter_Ypos = 900;

        private const float ShowPercent =(float)( 4.0f / 5.0f); //显示达到4/5区域

        Bitmap bmp = null;
        string className;   //类名

        public Coupling_Trajectory()
        {
            InitializeComponent();

            //int w = PicLeftWidth + PicWidth;
            //int h = PicHeight;
            //try
            //{
            //    bmp = new Bitmap(w, h, PixelFormat.Format16bppRgb565);

            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}

            //try
            //{
            //    g = Graphics.FromImage(bmp);

            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}

            className = this.GetType().Name;

        }

        ~Coupling_Trajectory()
        {
            try
            {
                if (g != null)
                {
                    g.Dispose();
                }
            }
            catch (Exception)
            {
            }

            try
            {
                if (bmp != null)
                {
                    if (bmp != null)
                    {
                        bmp.Dispose();
                    }
                }
            }
            catch (Exception)
            {
            }

            GC.Collect();

        }

        private void UserControl1_Paint(object sender, PaintEventArgs e)
        {
            //Update_Paint();
        }

        private void Update_Paint()
        {
            //this.BackgroundImage = null;
            //GC.Collect();
            //this.BackgroundImage = bmp;
            //this.BackgroundImageLayout = ImageLayout.Zoom;
        }

        private void DrawFormat_Before(double X_Pos_center,double Y_Pos_center,double Axis_Pos_halfsize, bool ShowDateTime = false)
        {
            //g.Clear(System.Drawing.SystemColors.Control);
            g.Clear(Color.White);

            SolidBrush myBrush = new SolidBrush(Color.Black);
            //底色
            g.FillRectangle(myBrush, PicLeftWidth, 0, PicLeftWidth + PicWidth, PicHeight);

            int toppos = 10;
            int downpos = PicHeight - PicLeftWidth / 2 - 10;
            int leftpos = 10;
            int rightpos = PicLeftWidth - 2 * leftpos;

            //左下角
            g.DrawString("Lin", new Font("Times New Roman", PicLeftWidth / 3), myBrush, leftpos, PicHeight - PicLeftWidth / 2);

            //版本号
            myBrush.Color = Color.White;
            //g.DrawString($"{className} Ver.{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}",
            //    new Font("Times New Roman", PicLeftWidth/3), myBrush, PicCenter_X + leftpos, toppos);

            if (ShowDateTime)
            {
                //日期
                g.DrawString("DateTime: " + DateTime.Now.ToString("yyyy /MM/dd HH:mm:ss"), new Font("Times New Roman", PicLeftWidth / 3), myBrush, PicCenter_X + leftpos, toppos + PicLeftWidth / 3 + 10);

                //显示SN号码
                g.DrawString("UserInfo: " + UserInfoMsg, new Font("Times New Roman", PicLeftWidth / 3), myBrush, PicCenter_X + leftpos, toppos + (PicLeftWidth / 3 + 10) * 2);
            }

            //色带图
            //Pen myPen = new Pen(Color.Gray, 5);
            int step = 1000;
            for (int v = 0; v < step; v++)
            {
                //绘制颜色是多少的椭圆
                float fv = (float)v / (float)step;

                //获取颜色
                Color col = Rainbow(v / (float)step);
                myBrush.Color = col;

                float dpos = ((float)toppos - (float)downpos) / ((float)step - 0) * (float)v + (float)downpos;
                //画直线
                g.FillRectangle(myBrush, leftpos, toppos, rightpos, dpos);

            }

            Pen myPen = new Pen(Color.Gray, 5);
            g.DrawLine(myPen, PicLeftWidth + 250, PicCenter_Y, PicLeftWidth + PicWidth, PicCenter_Y);  //水平线
            g.DrawLine(myPen, PicCenter_X, 0, PicCenter_X, PicHeight - 120);  //垂直线

            myBrush.Color = Color.White;
            //绘制坐标线和数值
            //垂直
            {
                int strleftpos = 50;
                float strOffset = PicLeftWidth / 3 / 1.5f;

                //0
                float StrPos_Y_0 = PicCenter_Y;
                g.DrawString($"{Y_Pos_center:f4}", new Font("Times New Roman", PicLeftWidth / 3), myBrush, PicLeftWidth + strleftpos, StrPos_Y_0 - strOffset);

                // 2/3
                double strP666 = Y_Pos_center + Axis_Pos_halfsize * ShowPercent;
                float StrPos_Y_P666 = StrPos_Y_0 - PicCenter_Y * ShowPercent;
                g.DrawString($"{strP666:f4}", new Font("Times New Roman", PicLeftWidth / 3), myBrush, PicLeftWidth + strleftpos, StrPos_Y_P666 - strOffset);

                double strN666 = Y_Pos_center - Axis_Pos_halfsize * ShowPercent;
                float StrPos_Y_N666 = StrPos_Y_0 + PicCenter_Y * ShowPercent;
                g.DrawString($"{strN666:f4}", new Font("Times New Roman", PicLeftWidth / 3), myBrush, PicLeftWidth + strleftpos, StrPos_Y_N666 - strOffset);


                //画直线
                g.DrawLine(myPen, PicLeftWidth, StrPos_Y_0, PicLeftWidth + strleftpos / 2, StrPos_Y_0);  //水平线
                g.DrawLine(myPen, PicLeftWidth, StrPos_Y_P666, PicLeftWidth + strleftpos / 2, StrPos_Y_P666);  //水平线
                g.DrawLine(myPen, PicLeftWidth, StrPos_Y_N666, PicLeftWidth + strleftpos / 2, StrPos_Y_N666);  //水平线
                //g.DrawLine(myPen, PicLeftWidth, StrPos_Y_N45, PicLeftWidth + strleftpos / 2, StrPos_Y_N45);  //水平线
                //g.DrawLine(myPen, PicLeftWidth, StrPos_Y_P45, PicLeftWidth + strleftpos / 2, StrPos_Y_P45);  //水平线
                //g.DrawLine(myPen, PicLeftWidth, StrPos_Y_N675, PicLeftWidth + strleftpos / 2, StrPos_Y_N675);  //水平线
                //g.DrawLine(myPen, PicLeftWidth, StrPos_Y_P675, PicLeftWidth + strleftpos / 2, StrPos_Y_P675);  //水平线

                ////画直线
                //g.DrawLine(myPen, PicLeftWidth, PicHeight, PicLeftWidth + strleftpos, PicHeight - strleftpos);  //水平线
                //g.DrawString("+/-90", new Font("Times New Roman", PicLeftWidth / 3), myBrush, PicLeftWidth + strleftpos - 10, PicHeight - PicLeftWidth / 3 - 60);

            }

            //水平
            {
                int strleftpos = 50;
                float strOffset = 60;

                //0
                float StrPos_X_0 = PicCenter_X;
                g.DrawString($"{X_Pos_center:f4}", new Font("Times New Roman", PicLeftWidth / 3), myBrush, StrPos_X_0 - strleftpos , PicHeight - PicLeftWidth / 3 - strOffset);

                // 2/3
                double strN666 = X_Pos_center - Axis_Pos_halfsize * ShowPercent;
                float StrPos_X_N666 = StrPos_X_0 - PicCenter_Y * ShowPercent;
                g.DrawString($"{strN666:f4}", new Font("Times New Roman", PicLeftWidth / 3), myBrush, StrPos_X_N666 - strleftpos, PicHeight - PicLeftWidth / 3 - strOffset);

                 double strP666 = X_Pos_center + Axis_Pos_halfsize * ShowPercent;
               float StrPos_X_P666 = StrPos_X_0 + PicCenter_Y* ShowPercent;
                g.DrawString($"{strP666:f4}", new Font("Times New Roman", PicLeftWidth / 3), myBrush, StrPos_X_P666 - strleftpos, PicHeight - PicLeftWidth / 3 - strOffset);



                //画直线
                g.DrawLine(myPen, StrPos_X_0, PicHeight - strleftpos / 2, StrPos_X_0, PicHeight);  //垂直
                g.DrawLine(myPen, StrPos_X_N666, PicHeight - strleftpos / 2, StrPos_X_N666, PicHeight);  //垂直
                g.DrawLine(myPen, StrPos_X_P666, PicHeight - strleftpos / 2, StrPos_X_P666, PicHeight);  //垂直


            }



        }

        private void DrawFormat_After()
        {
            //Pen myPen = new Pen(Color.Gray, 5);
            //g.DrawLine(myPen, PicLeftWidth + 250, PicCenter_Y, PicLeftWidth + PicWidth, PicCenter_Y);  //水平线
            //g.DrawLine(myPen, PicCenter_X, 0, PicCenter_X, PicHeight - 120);  //垂直线
        }


        public Image GetPicture()
        {

            Update_Paint();

            var newbmp = bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), bmp.PixelFormat);
            return newbmp;
        }


        public void SavePictrue(string imagePath)
        {
            Update_Paint();

            try
            {
                bmp.Save(imagePath, System.Drawing.Imaging.ImageFormat.Png);
            }
            catch (Exception ex)
            {
            }

        }



        /// <summary>
        /// 填充绘图数据
        /// </summary>
        /// <param name="tX_Pos_mm">水平位置序列</param>
        /// <param name="tY_Pos_mm">垂直位置序列</param>
        /// <param name="t_Power">功率序列</param>
        public void SetData(string Infomsg, double[] tX_Pos_mm, double[] tY_Pos_mm, double[] t_Power)
        {
            str2DCouplingData[] tData = new str2DCouplingData[] {
                new str2DCouplingData()
                {
                    Name = Infomsg,
                    X_Pos_mm = tX_Pos_mm,
                    Y_Pos_mm = tY_Pos_mm,
                    Power = t_Power
                }
            };

            SetData(Infomsg, tData);

        }
        public void SetData(string Infomsg, double[] tX_Pos_mm, double[] tY_Pos_mm, double[] t_Power, double Center_X, double Center_Y)
        {
            str2DCouplingData[] tData = new str2DCouplingData[] {
                new str2DCouplingData()
                {
                    Name = Infomsg,
                    X_Pos_mm = tX_Pos_mm,
                    Y_Pos_mm = tY_Pos_mm,
                    Power = t_Power
                }
            };

            SetData(Infomsg, tData, Center_X, Center_Y);

        }

        public void SetData(string Infomsg, str2DCouplingData[] tData)
        {
            //找到最大位置
            double Pmax = double.MinValue;
            double Pmin = double.MaxValue;
            double X_Pos_max = double.MinValue;
            double X_Pos_min = double.MaxValue;
            double Y_Pos_max = double.MinValue;
            double Y_Pos_min = double.MaxValue;

            foreach (var item in tData)
            {
                Pmax = Math.Max(Pmax, item.Power.Max());
                Pmin = Math.Min(Pmin, item.Power.Min());

                X_Pos_max = Math.Max(X_Pos_max, item.X_Pos_mm.Max());
                Y_Pos_max = Math.Max(Y_Pos_max, item.Y_Pos_mm.Max());

                X_Pos_min = Math.Min(X_Pos_min, item.X_Pos_mm.Min());
                Y_Pos_min = Math.Min(Y_Pos_min, item.Y_Pos_mm.Min());
            }

            double X_Pos_center = (X_Pos_max + X_Pos_min) / 2;
            double Y_Pos_center = (Y_Pos_max + Y_Pos_min) / 2;

            SetData(Infomsg, tData, X_Pos_center, Y_Pos_center);
        }

        /// <summary>
        /// 填充绘图数据
        /// </summary>
        /// <param name="tX_Pos_mm">水平位置序列</param>
        /// <param name="tY_Pos_mm">垂直位置序列</param>
        /// <param name="t_Power">功率序列</param>
        public void SetData(string Infomsg, str2DCouplingData[] tData, double Center_X, double Center_Y)
        {
            //用户信息
            UserInfoMsg = Infomsg;

            int picw = PicLeftWidth + PicWidth;
            int pich = PicHeight;
            try
            {
                bmp = new Bitmap(picw, pich, PixelFormat.Format16bppRgb565);

            }
            catch (Exception ex)
            {

                throw;
            }

            try
            {
                g = Graphics.FromImage(bmp);

            }
            catch (Exception ex)
            {

                throw;
            }

            //找到最大位置
            double Pmax = double.MinValue;
            double Pmin = double.MaxValue;
            double X_Pos_max = double.MinValue;
            double X_Pos_min = double.MaxValue;
            double Y_Pos_max = double.MinValue;
            double Y_Pos_min = double.MaxValue;

            foreach (var item in tData)
            {
                if (item.X_Pos_mm == null || item.Y_Pos_mm == null || item.Power == null)
                    return;

                if (item.X_Pos_mm.Length <= 0 || item.Y_Pos_mm.Length <= 0 || item.Power.Length <= 0)
                    return;

                if (item.X_Pos_mm.Length != item.Power.Length)
                    return;
                if (item.Y_Pos_mm.Length != item.Power.Length)
                    return;

                Pmax = Math.Max(Pmax, item.Power.Max());
                Pmin = Math.Min(Pmin, item.Power.Min());

                X_Pos_max = Math.Max(X_Pos_max, item.X_Pos_mm.Max());
                Y_Pos_max = Math.Max(Y_Pos_max, item.Y_Pos_mm.Max());

                X_Pos_min = Math.Min(X_Pos_min, item.X_Pos_mm.Min());
                Y_Pos_min = Math.Min(Y_Pos_min, item.Y_Pos_mm.Min());
            }

            double X_Pos_center = Center_X;
            double Y_Pos_center = Center_Y;


            double Pos_halfsize = Math.Abs(X_Pos_max - X_Pos_center);
            Pos_halfsize = Math.Max(Pos_halfsize, Math.Abs(X_Pos_min - X_Pos_center));

            Pos_halfsize = Math.Max(Pos_halfsize, Math.Abs(Y_Pos_max - Y_Pos_center));
            Pos_halfsize = Math.Max(Pos_halfsize, Math.Abs(Y_Pos_min - Y_Pos_center));

            //显示范围
            double Axis_Pos_halfsize = Pos_halfsize / ShowPercent;  //XY方向的最大, 显示到2/3区域

            DrawFormat_Before(X_Pos_center, Y_Pos_center, Axis_Pos_halfsize, true);     //显示绘制时间

            //线宽度
            float org_w = (float)(9);   //全范围基础线宽

            //先画大的, 再画小的
            List<str2DCouplingData> lstData = new List<str2DCouplingData>();
            lstData.AddRange(tData);

            //如果compare()方法返回负整数，表示obj1小于obj2；
            //如果compare()方法返回零，表示obj1等于obj2；
            //如果compare()方法返回正整数，表示obj1大于obj2。

            lstData.Sort((o2, o1) =>
            {
                double o2size = Math.Max(o2.X_Pos_mm.Max() - o2.X_Pos_mm.Min(), o2.Y_Pos_mm.Max() - o2.Y_Pos_mm.Min());
                double o1size = Math.Max(o1.X_Pos_mm.Max() - o1.X_Pos_mm.Min(), o1.Y_Pos_mm.Max() - o1.Y_Pos_mm.Min());

                //double o2power = o2.Power.Max();
                //double o1power = o1.Power.Max();

                //权重
                double o2val = o2size;// o2power * 0.2 + o2size * 0.8;
                double o1val = o1size;// o1power * 0.2 + o1size * 0.8;

                return o2val.CompareTo(o1val);
            });
            lstData.Reverse();

            //画刷
            Brush aGradientBrush;

            foreach (var item in lstData)
            {
                double[] Power_Value = item.Power;
                double[] X_Pos = item.X_Pos_mm;
                double[] Y_Pos = item.Y_Pos_mm;

                double item_halfsize = Math.Max(X_Pos.Max() - X_Pos.Min(), Y_Pos.Max() - Y_Pos.Min()) / 2;

                float w = (float)(org_w * (item_halfsize / Pos_halfsize));   //全范围基础线宽的倍数

                if (w < 3) w = 3;

                //归一化功率值
                double[] Power_Value_To1 = new double[Power_Value.Length];


                if ((Pmax - Pmin) != 0)
                {
                    //数据归一化
                    for (int i = 0; i < Power_Value.Length; i++)
                    {
                        Power_Value_To1[i] = (Power_Value[i] - Pmin) / (Pmax - Pmin);
                    }
                }
                else
                {
                    for (int i = 0; i < Power_Value.Length; i++)
                    {
                        Power_Value_To1[i] = 0.5;
                    }
                }



                SolidBrush myBrush = new SolidBrush(Color.Black);


                //Pen myPen = new Pen(Color.Black);


                //循环画斑点
                #region 画斑点
                for (int v = 0; v < Power_Value.Length; v++)
                {
                    {
                        //获取颜色
                        Color col = Rainbow((float)(Power_Value_To1[v]));

                        myBrush.Color = col;
                        //画圆形
                        float x1 = (float)(PicCenter_X + (X_Pos[v] - X_Pos_center) * (PicHeight / 2) / Axis_Pos_halfsize);
                        float y1 = (float)(PicCenter_Y + (Y_Pos[v] - Y_Pos_center) * (PicHeight / 2) / Axis_Pos_halfsize);

                        float h = w;

                        g.FillEllipse(myBrush, x1 - w / 2, y1 - w / 2, w, h);
                    }
                }
                #endregion

                #region 画渐变线
                for (int v = 1; v < Power_Value.Length; v++)
                {
                    {
                        //获取颜色
                        Color col_1 = Rainbow((float)(Power_Value_To1[v - 1]));
                        Color col_2 = Rainbow((float)(Power_Value_To1[v]));

                        //坐标
                        float x1 = (float)(PicCenter_X + (X_Pos[v - 1] - X_Pos_center) * (PicHeight / 2) / Axis_Pos_halfsize);
                        float y1 = (float)(PicCenter_Y + (Y_Pos[v - 1] - Y_Pos_center) * (PicHeight / 2) / Axis_Pos_halfsize);

                        float x2 = (float)(PicCenter_X + (X_Pos[v] - X_Pos_center) * (PicHeight / 2) / Axis_Pos_halfsize);
                        float y2 = (float)(PicCenter_Y + (Y_Pos[v] - Y_Pos_center) * (PicHeight / 2) / Axis_Pos_halfsize);

                        double len = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));

                        if (len > 5)
                        {
                            aGradientBrush = new LinearGradientBrush(new PointF(x1, y1), new PointF(x2, y2), col_1, col_2);

                            using (Pen aGradientPen = new Pen(aGradientBrush, w))
                            {
                                g.DrawLine(aGradientPen, new PointF(x1, y1), new PointF(x2, y2));
                            }
                        }

                    }
                }
                #endregion
            }


            DrawFormat_After();

            Update_Paint();

            this.BackgroundImage = null;
            GC.Collect();
            this.BackgroundImage = bmp;
            this.BackgroundImageLayout = ImageLayout.Zoom;

        }

        //归一化到彩虹色
        Color Rainbow(float progress)
        {
            if (progress >= 1) progress = 0.999f;
            float div = (Math.Abs(progress % 1) * 4);
            int ascending = (int)((div % 1) * 255);
            int descending = 255 - ascending;

            Color col = new Color();
            switch ((int)div)
            {
                case 0:
                    col= Color.FromArgb(255, 0, ascending, 255);     //蓝到亮蓝  增加
                    break; 
                case 1:
                    col = Color.FromArgb(255, 0, 255, descending);      //亮蓝到绿  减少
                    break; 
                case 2:
                    col = Color.FromArgb(255, ascending, 255, 0);     //绿到黄  增加
                    break; 
                default: //case 3:
                    col = Color.FromArgb(255, 255, descending, 0);   //黄红 减少
                    break; 
            }

            //float step = 1f / 7f;  //颜色分8级
            //float colorstep = step / 255f; //颜色变化
            //if (progress >= 1)
            //    return Color.FromArgb(255, 255, 255, 255); //白色

            //int div = (int)(progress / step);  //级别

            //int ascending = (int)((progress - step * div) / colorstep);//增加
            //int descending = 255 - ascending;   //减少


            //Color col = new Color();
            //switch ((int)div)
            //{
            //    case 0:
            //        col = Color.FromArgb(255, 0, 0, ascending);         //黑到蓝  增加
            //        break;
            //    case 1:
            //        col = Color.FromArgb(255, 0, ascending, 255);      //蓝到亮蓝  增加
            //        break;
            //    case 2:
            //        col = Color.FromArgb(255, 0, 255, descending);      //亮蓝到绿  减少
            //        break;
            //    case 3:
            //        col = Color.FromArgb(255, ascending, 255, 0);      //绿到黄  增加
            //        break;
            //    case 4:
            //        col = Color.FromArgb(255, 255, descending, 0);      //黄到红  减少
            //        break;
            //    case 5:
            //        col = Color.FromArgb(255, 255, 0, ascending);      //红到青  增加
            //        break;
            //    case 6:
            //        col = Color.FromArgb(255, 255, ascending, 255);      //青到白  增加
            //        break;
            //    default:  //case 7
            //        col = Color.FromArgb(255, 255, 255, 255);   //白色
            //        break;
            //}
            //Console.WriteLine($"{progress},{div},{ascending},{descending},{col.R},{col.G},{col.B}");

            return col;
        }

        //计算椭圆范围
        private bool GetPos(
                    double[] FH_Pos, double[] FH_Value_To1, int FHmaxpos,
                    double[] FV_Pos, double[] FV_Value_To1, int FVmaxpos,
                    double dfv,
                    ref PointF c_xy,   //中心
                    ref double cx,     //H轴长
                    ref double cy     //V轴长
                    )
        {
            float fv = (float)dfv;

            double FHpos_L = -1, FHpos_R = -1;
            double FVpos_L = -1, FVpos_R = -1;

            bool bFH_L = false, bFH_R = false;
            bool bFV_L = false, bFV_R = false;

            //曲线寻找fv的位置
            #region FH
            for (int i = 0; i < FHmaxpos; i++)
            {
                if (FH_Value_To1[i] == fv)
                {
                    FHpos_L = FH_Pos[i];
                    bFH_L = true;
                    break;
                }
                else if (FH_Value_To1[i] < fv && FH_Value_To1[i + 1] > fv)
                {
                    FHpos_L = (FH_Pos[i] + FH_Pos[i + 1]) / 2;
                    bFH_L = true;
                    break;
                }
            }

            for (int i = FH_Value_To1.Length - 1; i > FHmaxpos; i--)
            {
                if (FH_Value_To1[i] == fv)
                {
                    FHpos_R = FH_Pos[i];
                    bFH_R = true;
                    break;
                }
                else if (FH_Value_To1[i - 1] > fv && FH_Value_To1[i] < fv)
                {
                    FHpos_R = (FH_Pos[i - 1] + FH_Pos[i]) / 2;
                    bFH_R = true;
                    break;
                }
            }
            #endregion

            #region FV
            for (int i = 0; i < FVmaxpos; i++)
            {
                if (FV_Value_To1[i] == fv)
                {
                    FVpos_L = FV_Pos[i];
                    bFV_L = true;
                    break;
                }
                else if (FV_Value_To1[i] < fv && FV_Value_To1[i + 1] > fv)
                {
                    FVpos_L = (FV_Pos[i] + FV_Pos[i + 1]) / 2;
                    bFV_L = true;
                    break;
                }
            }

            for (int i = FV_Value_To1.Length - 1; i > FVmaxpos; i--)
            {
                if (FV_Value_To1[i] == fv)
                {
                    FVpos_R = FV_Pos[i];
                    bFV_R = true;
                    break;
                }
                else if (FV_Value_To1[i - 1] > fv && FV_Value_To1[i] < fv)
                {
                    FVpos_R = (FV_Pos[i - 1] + FV_Pos[i]) / 2;
                    bFV_R = true;
                    break;
                }
            }
            #endregion

            //没有找到
            if ((bFH_L & bFH_R & bFV_L & bFV_R) == false)
            {
                return false;
            }

            //根据位置计算
            c_xy = new PointF();
            c_xy.X = (float)((FHpos_L + FHpos_R) / 2);
            c_xy.Y = (float)((FVpos_L + FVpos_R) / 2);

            cx = Math.Abs(FHpos_L - FHpos_R);
            cy = Math.Abs(FVpos_L - FVpos_R);

            return true;
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            //DrawFormat_Before();
            //Update_Paint();

            //this.BackgroundImage = null;
            //GC.Collect();
            //this.BackgroundImage = bmp;
            //this.BackgroundImageLayout = ImageLayout.Zoom;


        }


    }
}
