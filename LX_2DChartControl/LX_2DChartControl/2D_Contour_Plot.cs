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
    public struct str2DContourPlotData
    {
        public string Name;
        public double[] X_Value;
        public double[] Y_Value;
        public double[] Power;
    }

    public struct str2DLine
    {
        public string Name;
        public PointF P1;
        public PointF P2;
    }

    public struct str2DPoint
    {
        public string Name;
        public PointF P1;
    }

    public enum eShowMode
    {
        点云,
        彩云
    };

    //耦合轨迹显示
    public partial class Contour_Plot : UserControl
    {
        private struct strContourPlotData
        {
            public double X_Value;
            public double Y_Value;
            public double Power_Value_To1;
            public Color col;   //该点的颜色
        }
        private Graphics g;

        private string UserInfoMsg;

        private const int PicLeftWidth = 200; //图像左侧位置参数 200像素
        private const int PicRightWidth = 400; //图像右侧色带 300像素
        private const int PicDownHeight = 80; //图像下色带 300像素

        private const float PicZoon = 0.1f; // 0.1分度
        private const int PicWidth = (int)(180 / PicZoon); //图像宽度    180度  0.1分度
        private const int PicHeight = (int)(180 / PicZoon); //图像高度   180度  0.1分度

        private const int PicCenter_X = PicWidth / 2 + PicLeftWidth;
        private const int PicCenter_Y = PicHeight / 2;

        private const int PicCenter_Xpos = 900; //0度的地方是位置90度;
        private const int PicCenter_Ypos = 900;

        private const float ShowPercent =(float)( 95.0f / 100.0f); //显示达到4/5区域

        Bitmap bmp = null;
        string className;   //类名

        public Contour_Plot()
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

        ~Contour_Plot()
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

        private void DrawFormat_Before(double X_Pos_center,double Y_Pos_center,double Axis_Pos_halfsize, double Rainbow_min, double Rainbow_max, bool ShowDateTime = false)
        {
            //g.Clear(System.Drawing.SystemColors.Control);
            //g.Clear(Color.LightGray);
            g.Clear(Color.FromArgb(255, 71,71,71));

            SolidBrush myBrush = new SolidBrush(Color.Black);
            //底色
            //g.FillRectangle(myBrush, PicLeftWidth, 0, PicLeftWidth + PicWidth, PicHeight);
            //g.FillRectangle(myBrush, 0, 0, PicLeftWidth + PicWidth, PicHeight + PicRightWidth);

            int toppos = 100;
            int downpos = (int)(PicHeight *0.4);// - PicLeftWidth / 2 - 10;
            int leftpos = PicLeftWidth + PicWidth + 10;
            int rightpos = 100;// PicLeftWidth - 2 * leftpos;

            //左下角
            //g.DrawString("Lin", new Font("Times New Roman", PicLeftWidth / 3), myBrush, leftpos, PicHeight - PicLeftWidth / 2);

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

            Pen myPen = new Pen(Color.Gray, 5);

            //色带图
            int step = 1000;
            for (int v = 0; v < step; v++)
            {
                //绘制颜色是多少的椭圆
                float fv = (float)v / (float)step;

                //获取颜色
                Color col = Rainbow(v / (float)step);
                myBrush.Color = col;

                float dpos = ((float)toppos - (float)downpos) / ((float)step - 0) * (float)v + (float)downpos;
                //画色
                g.FillRectangle(myBrush, leftpos, toppos, rightpos, dpos);

            }
            //标准外色带图
            {
                int height = 40;
                //获取颜色
                Color col = Rainbow(1);
                myBrush.Color = col;
                //画色
                g.FillRectangle(myBrush, leftpos, toppos- height, rightpos, height);

                //获取颜色
                col = Rainbow(0);
                myBrush.Color = col;
                //画色
                g.FillRectangle(myBrush, leftpos, toppos+downpos, rightpos, height);

                //画线
                myBrush.Color = Color.Black;
                g.DrawLine(myPen, leftpos, toppos, leftpos+ rightpos, toppos);  //水平线
                g.DrawLine(myPen, leftpos, toppos + downpos, leftpos + rightpos, toppos + downpos);  //水平线

            }
            //参数值
            {
                myBrush.Color = Color.White;

                int strleftpos = leftpos + rightpos + 5;
                int strFontleftpos = 50;
                float strOffset = 25;// PicLeftWidth / 3 / 1.5f;

                //0
                float StrPos_Y_0 = toppos;
                g.DrawString($"{Rainbow_max:f4}", new Font("Times New Roman", PicLeftWidth / 6), myBrush, strleftpos + strFontleftpos, StrPos_Y_0 - strOffset);

                // 2/3
                float StrPos_Y_1 = toppos+downpos;
                g.DrawString($"{Rainbow_min:f4}", new Font("Times New Roman", PicLeftWidth / 6), myBrush, strleftpos + strFontleftpos, StrPos_Y_1 - strOffset);



                //画直线
                g.DrawLine(myPen, strleftpos, StrPos_Y_0, strleftpos+ 25, StrPos_Y_0);  //水平线
                g.DrawLine(myPen, strleftpos, StrPos_Y_1, strleftpos+ 25, StrPos_Y_1);  //水平线
                                                                                                               //g.DrawLine(myPen, PicLeftWidth, StrPos_Y_N45, PicLeftWidth + strleftpos / 2, StrPos_Y_N45);  //水平线
                                                                                                               //g.DrawLine(myPen, PicLeftWidth, StrPos_Y_P45, PicLeftWidth + strleftpos / 2, StrPos_Y_P45);  //水平线
                                                                                                               //g.DrawLine(myPen, PicLeftWidth, StrPos_Y_N675, PicLeftWidth + strleftpos / 2, StrPos_Y_N675);  //水平线
                                                                                                               //g.DrawLine(myPen, PicLeftWidth, StrPos_Y_P675, PicLeftWidth + strleftpos / 2, StrPos_Y_P675);  //水平线

                ////画直线
                //g.DrawLine(myPen, PicLeftWidth, PicHeight, PicLeftWidth + strleftpos, PicHeight - strleftpos);  //水平线
                //g.DrawString("+/-90", new Font("Times New Roman", PicLeftWidth / 3), myBrush, PicLeftWidth + strleftpos - 10, PicHeight - PicLeftWidth / 3 - 60);
            }



            
            //g.DrawLine(myPen, PicLeftWidth + 250, PicCenter_Y, PicLeftWidth + PicWidth, PicCenter_Y);  //水平线
            //g.DrawLine(myPen, PicCenter_X, 0, PicCenter_X, PicHeight - 120);  //垂直线

            myBrush.Color = Color.White;
            //绘制坐标线和数值
            //垂直
            {
                int strleftpos = 50;
                int strFontleftpos = -160;
                float strOffset = 25;// PicLeftWidth / 3 / 1.5f;

                //0
                float StrPos_Y_0 = PicCenter_Y;
                g.DrawString($"{Y_Pos_center:f4}", new Font("Times New Roman", PicLeftWidth / 6), myBrush, PicLeftWidth + strFontleftpos, StrPos_Y_0 - strOffset);

                // 2/3
                double strP666 = Y_Pos_center + Axis_Pos_halfsize * ShowPercent;
                float StrPos_Y_P666 = StrPos_Y_0 - PicCenter_Y * ShowPercent;
                g.DrawString($"{strP666:f4}", new Font("Times New Roman", PicLeftWidth / 6), myBrush, PicLeftWidth + strFontleftpos, StrPos_Y_P666 - strOffset);

                double strN666 = Y_Pos_center - Axis_Pos_halfsize * ShowPercent;
                float StrPos_Y_N666 = StrPos_Y_0 + PicCenter_Y * ShowPercent;
                g.DrawString($"{strN666:f4}", new Font("Times New Roman", PicLeftWidth / 6), myBrush, PicLeftWidth + strFontleftpos, StrPos_Y_N666 - strOffset);


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
                float strOffset = -80;

                //0
                float StrPos_X_0 = PicCenter_X;
                g.DrawString($"{X_Pos_center:f4}", new Font("Times New Roman", PicLeftWidth / 6), myBrush, StrPos_X_0 - strleftpos, PicHeight - PicLeftWidth / 3 - strOffset);

                // 2/3
                double strN666 = X_Pos_center - Axis_Pos_halfsize * ShowPercent;
                float StrPos_X_N666 = StrPos_X_0 - PicCenter_Y * ShowPercent;
                g.DrawString($"{strN666:f4}", new Font("Times New Roman", PicLeftWidth / 6), myBrush, StrPos_X_N666 - strleftpos, PicHeight - PicLeftWidth / 3 - strOffset);

                double strP666 = X_Pos_center + Axis_Pos_halfsize * ShowPercent;
                float StrPos_X_P666 = StrPos_X_0 + PicCenter_Y * ShowPercent;
                g.DrawString($"{strP666:f4}", new Font("Times New Roman", PicLeftWidth / 6), myBrush, StrPos_X_P666 - strleftpos, PicHeight - PicLeftWidth / 3 - strOffset);



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



        ///// <summary>
        ///// 填充绘图数据
        ///// </summary>
        ///// <param name="tX_Pos_mm">水平位置序列</param>
        ///// <param name="tY_Pos_mm">垂直位置序列</param>
        ///// <param name="t_Power">功率序列</param>
        //public void SetData(string Infomsg, double[] tX_Pos_mm, double[] tY_Pos_mm, double[] t_Power)
        //{
        //    str2DCouplingData[] tData = new str2DCouplingData[] {
        //        new str2DCouplingData()
        //        {
        //            Name = Infomsg,
        //            X_Pos_mm = tX_Pos_mm,
        //            Y_Pos_mm = tY_Pos_mm,
        //            Power = t_Power
        //        }
        //    };

        //    SetData(Infomsg, tData);

        //}
        //public void SetData(string Infomsg, double[] tX_Pos_mm, double[] tY_Pos_mm, double[] t_Power, double Center_X, double Center_Y)
        //{
        //    str2DCouplingData[] tData = new str2DCouplingData[] {
        //        new str2DCouplingData()
        //        {
        //            Name = Infomsg,
        //            X_Pos_mm = tX_Pos_mm,
        //            Y_Pos_mm = tY_Pos_mm,
        //            Power = t_Power
        //        }
        //    };

        //    SetData(Infomsg, tData, Center_X, Center_Y);

        //}

        private eShowMode ShowMode
        {
            get; set;
        }


        public void SetData(string Infomsg, str2DContourPlotData tData, bool Colorful_Clouds, str2DLine[] Lines, str2DPoint[] Points)
        {
            //找到最大位置
            double Pmax = double.MinValue;
            double Pmin = double.MaxValue;
            double X_Pos_max = double.MinValue;
            double X_Pos_min = double.MaxValue;
            double Y_Pos_max = double.MinValue;
            double Y_Pos_min = double.MaxValue;

            //foreach (var item in tData)
            {
                var item = tData;
                Pmax = Math.Max(Pmax, item.Power.Max());
                Pmin = Math.Min(Pmin, item.Power.Min());

                X_Pos_max = Math.Max(X_Pos_max, item.X_Value.Max());
                Y_Pos_max = Math.Max(Y_Pos_max, item.Y_Value.Max());

                X_Pos_min = Math.Min(X_Pos_min, item.X_Value.Min());
                Y_Pos_min = Math.Min(Y_Pos_min, item.Y_Value.Min());
            }

            double X_Pos_center = (X_Pos_max + X_Pos_min) / 2;
            double Y_Pos_center = (Y_Pos_max + Y_Pos_min) / 2;

            SetData(Infomsg, tData, X_Pos_center, Y_Pos_center, Colorful_Clouds, Lines, Points);
        }

        
        /// <summary>
        /// 填充绘图数据
        /// </summary>
        /// <param name="tX_Pos_mm">水平位置序列</param>
        /// <param name="tY_Pos_mm">垂直位置序列</param>
        /// <param name="t_Power">功率序列</param>
        public void SetData(string Infomsg, str2DContourPlotData tData, double Center_X, double Center_Y, bool Colorful_Clouds, str2DLine[] Lines, str2DPoint[] Points)
        {
            //用户信息
            UserInfoMsg = Infomsg;

            int picw = PicLeftWidth + PicWidth + PicRightWidth;
            int pich = PicHeight + PicDownHeight;
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
                //g.SmoothingMode = SmoothingMode.AntiAlias;

            }
            catch (Exception ex)
            {

                throw;
            }

            //异常值被定义为小于Q1－1.5IQR或大于Q3+1.5IQR的值。
            //1、下四分位数Q1
            //等于该样本中所有数值由小到大排列后第25 % 的数字。
            //确定四分位数的位置。Qi所在位置 = i（n + 1）/ 4，其中i = 1，2，3。n表示序列中包含的项数。
            //根据位置，计算相应的四分位数(以数组为例)。
            //Q1所在的位置 =（14 + 1）/ 4 = 3.75，
            //Q1 = 0.25×第三项 + 0.75×第四项 = 0.25×17 + 0.75×19 = 18.5；
            // 2、中位数（第二个四分位数）Q2
            //中位数，等于该样本中所有数值由小到大排列后第50 % 的数字。
            //Q2所在的位置 = 2（14 + 1）/ 4 = 7.5，
            //Q2 = 0.5×第七项 + 0.5×第八项 = 0.5×25 + 0.5×28 = 26.5；
            //3、上四分位数Q3
            //等于该样本中所有数值由小到大排列后第75 % 的数字
            //计算方法同下分位数。
            //Q3所在的位置 = 3（14 + 1）/ 4 = 11.25，
            //Q3 = 0.75×第十一项 + 0.25×第十二项 = 0.75×34 + 0.25×35 = 34.25；
            //4、上限
            //上限是非异常范围内的最大值。
            //首先要知道什么是四分位距如何计算的？
            //四分位距（interquartile range, IQR），又称四分差。
            //四分位距IQR = Q3 - Q1，那么上限 = Q3 + 1.5IQR
            //5、下限
            //下限是非异常范围内的最小值。
            //下限 = Q1 - 1.5IQR
            //6、异常值
            //在内限与外限之间的异常值为温和的异常值（mild outliers）
            //在外限以外的为极端的异常值（extreme outliers）

            //计算有效范围
            double IQR_K = 1.25;

            double Q1, Q2, Q3, IQR, Limit_min, Limit_max;
            {
                List<double> lstPower = new List<double>();

                lstPower.AddRange(tData.Power);
                int lstcount = lstPower.Count();


                lstPower.Sort();    //需要从小到大排序

                Q1 = lstPower[lstcount * 1 / 4];
                Q2 = lstPower[lstcount * 2 / 4];
                Q3 = lstPower[lstcount * 3 / 4];

                IQR = Math.Abs(Q1 - Q3);

                Limit_min = Q1 - IQR_K * IQR;
                Limit_max = Q3 + IQR_K * IQR;
            }


            //找到最大位置
            double Pmax = Limit_max;// double.MinValue;
            double Pmin = Limit_min;// double.MaxValue;
            double X_Pos_max = double.MinValue;
            double X_Pos_min = double.MaxValue;
            double Y_Pos_max = double.MinValue;
            double Y_Pos_min = double.MaxValue;

            //foreach (var item in tData)
            
            {
                var item = tData;
                if (item.X_Value == null || item.Y_Value == null || item.Power == null)
                    return;

                if (item.X_Value.Length <= 0 || item.Y_Value.Length <= 0 || item.Power.Length <= 0)
                    return;

                if (item.X_Value.Length != item.Power.Length)
                    return;
                if (item.Y_Value.Length != item.Power.Length)
                    return;

                //Pmax = Math.Max(Pmax, item.Power.Max());
                //Pmin = Math.Min(Pmin, item.Power.Min());

                X_Pos_max = Math.Max(X_Pos_max, item.X_Value.Max());
                Y_Pos_max = Math.Max(Y_Pos_max, item.Y_Value.Max());

                X_Pos_min = Math.Min(X_Pos_min, item.X_Value.Min());
                Y_Pos_min = Math.Min(Y_Pos_min, item.Y_Value.Min());
            }

            double X_Pos_center = Center_X;
            double Y_Pos_center = Center_Y;


            double Pos_halfsize = Math.Abs(X_Pos_max - X_Pos_center);
            Pos_halfsize = Math.Max(Pos_halfsize, Math.Abs(X_Pos_min - X_Pos_center));

            Pos_halfsize = Math.Max(Pos_halfsize, Math.Abs(Y_Pos_max - Y_Pos_center));
            Pos_halfsize = Math.Max(Pos_halfsize, Math.Abs(Y_Pos_min - Y_Pos_center));

            //显示范围
            double Axis_Pos_halfsize = Pos_halfsize / ShowPercent;  //XY方向的最大, 显示到2/3区域

            DrawFormat_Before(X_Pos_center, Y_Pos_center, Axis_Pos_halfsize, Pmax, Pmin, false);     //显示绘制时间


            //线宽度
            float org_w = (float)(3);   //全范围基础线宽

            //画刷
            Brush aGradientBrush;

            //foreach (var item in lstData)
            {
                var item = tData;
                double[] Power_Value = item.Power;
                double[] X_Pos = item.X_Value;
                double[] Y_Pos = item.Y_Value;

                double item_halfsize = Math.Max(X_Pos.Max() - X_Pos.Min(), Y_Pos.Max() - Y_Pos.Min()) / 2;

                float w = (float)(org_w * (item_halfsize / Pos_halfsize));   //全范围基础线宽的倍数

                if (w < 3) w = 3;

                //归一化功率值
                double[] Power_Value_To1 = new double[Power_Value.Length];


                #region 功率数据归一化
                if ((Pmax - Pmin) != 0)
                {
                    for (int i = 0; i < Power_Value.Length; i++)
                    {
                        if (Power_Value[i] <= Pmin)
                        {
                            Power_Value_To1[i] = 0;
                        }
                        else if (Power_Value[i] >= Pmax)
                        {
                            Power_Value_To1[i] = 1;
                        }
                        else
                        {
                            Power_Value_To1[i] = (Power_Value[i] - Pmin) / (Pmax - Pmin);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Power_Value.Length; i++)
                    {
                        Power_Value_To1[i] = 0.5;
                    }
                }
                #endregion


                #region 将XY坐标数组整理成二维数组

                strContourPlotData[,] Point_2D;
                int Point_2D_XCount = 0;
                int Point_2D_YCount = 0;


                {
                    var grouped_X = X_Pos.GroupBy(n => n);
                    var grouped_Y = Y_Pos.GroupBy(n => n);

                    Point_2D_XCount = grouped_X.Count();
                    Point_2D_YCount = grouped_Y.Count();

                    //建立二维数组
                    Point_2D = new strContourPlotData[Point_2D_XCount, Point_2D_YCount];

                    //建立排序
                    List<double> lstgrouped_X = new List<double>();
                    List<double> lstgrouped_Y = new List<double>();
                    foreach (var group in grouped_X)
                    {
                        lstgrouped_X.Add(group.Key);
                    }
                    foreach (var group in grouped_Y)
                    {
                        lstgrouped_Y.Add(group.Key);
                    }

                    lstgrouped_X.Sort();
                    lstgrouped_Y.Sort();

                    //建立编号查询
                    Dictionary<double, int> Xindex = new Dictionary<double, int>();
                    Dictionary<double, int> Yindex = new Dictionary<double, int>();

                    int i = 0;
                    foreach (var group in lstgrouped_X)
                    {
                        Xindex.Add(group, i++);
                    }
                    i = 0;
                    foreach (var group in lstgrouped_Y)
                    {
                        Yindex.Add(group, i++);
                    }

                    Color bColor = Color.FromArgb(255, 71, 71, 71);
                    //填充空数据
                    for (int x = 0; x < Point_2D_XCount; x++)
                    {
                        for (int y = 0; y < Point_2D_YCount; y++)
                        {
                            strContourPlotData t = new strContourPlotData();
                            t.X_Value = lstgrouped_X[x];
                            t.Y_Value = lstgrouped_Y[y];
                            t.Power_Value_To1 = 0;
                            t.col = bColor;// Color.Black;    //空点显示黑色
                            Point_2D[x, y] = t;
                        }
                    }

                    //填充有效数据
                    for (int v = 0; v < Power_Value.Length; v++)
                    {
                        strContourPlotData t = new strContourPlotData();
                        t.X_Value = X_Pos[v];
                        t.Y_Value = Y_Pos[v];
                        t.Power_Value_To1 = Power_Value_To1[v];
                        t.col = Rainbow((float)(Power_Value_To1[v]));

                        Point_2D[Xindex[X_Pos[v]], Yindex[Y_Pos[v]]] = t;
                    }

                }
                #endregion


                #region 画矩形
                if(Colorful_Clouds) //画彩云
                {
                    for (int yindex = 0; yindex < Point_2D_YCount - 1; yindex++)
                    {
                        for (int xindex = 0; xindex < Point_2D_XCount - 1; xindex++)
                        {
                            //获取颜色
                            Color col_1 = Point_2D[xindex + 0, yindex + 0].col;
                            Color col_2 = Point_2D[xindex + 1, yindex + 0].col;
                            Color col_3 = Point_2D[xindex + 1, yindex + 1].col;
                            Color col_4 = Point_2D[xindex + 0, yindex + 1].col;

                            //坐标
                            float x1 = (float)(PicCenter_X + (Point_2D[xindex + 0, yindex + 0].X_Value - X_Pos_center) * (PicHeight / 2) / Axis_Pos_halfsize);
                            float x2 = (float)(PicCenter_X + (Point_2D[xindex + 1, yindex + 0].X_Value - X_Pos_center) * (PicHeight / 2) / Axis_Pos_halfsize);
                            float x3 = (float)(PicCenter_X + (Point_2D[xindex + 1, yindex + 1].X_Value - X_Pos_center) * (PicHeight / 2) / Axis_Pos_halfsize);
                            float x4 = (float)(PicCenter_X + (Point_2D[xindex + 0, yindex + 1].X_Value - X_Pos_center) * (PicHeight / 2) / Axis_Pos_halfsize);

                            float y1 = (float)(PicCenter_Y + (-Point_2D[xindex + 0, yindex + 0].Y_Value + Y_Pos_center) * (PicHeight / 2) / Axis_Pos_halfsize);
                            float y2 = (float)(PicCenter_Y + (-Point_2D[xindex + 1, yindex + 0].Y_Value + Y_Pos_center) * (PicHeight / 2) / Axis_Pos_halfsize);
                            float y3 = (float)(PicCenter_Y + (-Point_2D[xindex + 1, yindex + 1].Y_Value + Y_Pos_center) * (PicHeight / 2) / Axis_Pos_halfsize);
                            float y4 = (float)(PicCenter_Y + (-Point_2D[xindex + 0, yindex + 1].Y_Value + Y_Pos_center) * (PicHeight / 2) / Axis_Pos_halfsize);

                            w = 3;

                            #region 绘制矩形外框
                            if (xindex == 0)
                            {
                                //上水平
                                aGradientBrush = new LinearGradientBrush(new PointF(x1, y1), new PointF(x2, y2), col_1, col_2);
                                using (Pen aGradientPen = new Pen(aGradientBrush, w))
                                {
                                    g.DrawLine(aGradientPen, new PointF(x1, y1), new PointF(x2, y2));
                                }

                            }

                            if (yindex == 0)
                            {
                                //左垂直
                                aGradientBrush = new LinearGradientBrush(new PointF(x4, y4), new PointF(x1, y1), col_4, col_1);
                                using (Pen aGradientPen = new Pen(aGradientBrush, w))
                                {
                                    g.DrawLine(aGradientPen, new PointF(x4, y4), new PointF(x1, y1));
                                }
                            }

                            //右垂直
                            aGradientBrush = new LinearGradientBrush(new PointF(x2, y2), new PointF(x3, y3), col_2, col_3);
                            using (Pen aGradientPen = new Pen(aGradientBrush, w))
                            {
                                g.DrawLine(aGradientPen, new PointF(x2, y2), new PointF(x3, y3));
                            }
                            //下水平
                            aGradientBrush = new LinearGradientBrush(new PointF(x3, y3), new PointF(x4, y4), col_3, col_4);
                            using (Pen aGradientPen = new Pen(aGradientBrush, w))
                            {
                                g.DrawLine(aGradientPen, new PointF(x3, y3), new PointF(x4, y4));
                            }
                            #endregion

                            #region 画渐变矩形

                            // 创建线组
                            GraphicsPath path = new GraphicsPath(new PointF[] {
                                    new PointF(x1,y1),
                                    new PointF(x2,y2),
                                    new PointF(x3,y3),
                                    new PointF(x4,y4)
                                }, new byte[] {
                                    (byte)PathPointType.Start,
                                    (byte)PathPointType.Line,
                                    (byte)PathPointType.Line,
                                    (byte)PathPointType.Line,
                                });

                            // 路径笔刷
                            PathGradientBrush pathGradientBrush = new PathGradientBrush(path);
                            // 设置路径中的点对应的颜色数组
                            pathGradientBrush.SurroundColors = new Color[] { col_1, col_2, col_3, col_4 };
                            pathGradientBrush.CenterColor = GetAverageColor(pathGradientBrush.SurroundColors);
                            g.FillPath(pathGradientBrush, path); // 填充路径
                            #endregion

                        }
                    }
                }
                #endregion



                SolidBrush myBrush = new SolidBrush(Color.Black);

                //循环画斑点
                #region 画斑点
                if (Colorful_Clouds) //画彩云
                {
                    w = 3;
                }
                else
                {
                    w = 9;
                }
                for (int v = 0; v < Power_Value.Length; v++)
                {
                    //获取颜色
                    Color col = Rainbow((float)(Power_Value_To1[v]));

                    myBrush.Color = col;
                    //画圆形
                    float x1 = (float)(PicCenter_X + (X_Pos[v] - X_Pos_center) * (PicHeight / 2) / Axis_Pos_halfsize);
                    float y1 = (float)(PicCenter_Y + (-Y_Pos[v] + Y_Pos_center) * (PicHeight / 2) / Axis_Pos_halfsize);

                    float h = w;

                    g.FillEllipse(myBrush, x1 - w / 2, y1 - w / 2, w, h);
                }
                #endregion

                #region 画虚线
                if(Lines!=null)
                {
                    Pen myPen = new Pen(Color.Gray, 10);
                    myPen.DashStyle = DashStyle.Dot;

                    Random random = new Random();

                    foreach (var lineitem in Lines)
                    {
                        Color randomColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                        myPen.Color = Color.Red;// randomColor;

                        //这里要算出来直线的2端才行

                        //坐标
                        float x1 = (float)(PicCenter_X + (lineitem.P1.X - X_Pos_center) * (PicHeight / 2) / Axis_Pos_halfsize);
                        float x2 = (float)(PicCenter_X + (lineitem.P2.X - X_Pos_center) * (PicHeight / 2) / Axis_Pos_halfsize);

                        float y1 = (float)(PicCenter_Y + (-lineitem.P1.Y + Y_Pos_center) * (PicHeight / 2) / Axis_Pos_halfsize);
                        float y2 = (float)(PicCenter_Y + (-lineitem.P2.Y + Y_Pos_center) * (PicHeight / 2) / Axis_Pos_halfsize);

                        g.DrawLine(myPen, x1, y1, x2, y2);  //垂直

                    }

                }
                #endregion

                #region 画标志
                if (Points != null)
                {
                    w = 20;
                    foreach (var pointitem in Points)
                    {
                        myBrush.Color = Color.Red;

                        //画圆形
                        float x1 = (float)(PicCenter_X + (pointitem.P1.X - X_Pos_center) * (PicHeight / 2) / Axis_Pos_halfsize);
                        float y1 = (float)(PicCenter_Y + (-pointitem.P1.Y + Y_Pos_center) * (PicHeight / 2) / Axis_Pos_halfsize);

                        float h = w;

                        g.FillEllipse(myBrush, x1 - w / 2, y1 - w / 2, w, h);

                        myBrush.Color = Color.Black;

                        g.DrawString($"{pointitem.Name}", new Font("Times New Roman", PicLeftWidth / 4), myBrush, x1 - 50, y1 - 100);


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

        //计算出平均色
        public Color GetAverageColor(params Color[] colors)
        {
            int redTotal = 0, greenTotal = 0, blueTotal = 0;
            foreach (Color color in colors)
            {
                redTotal += color.R;
                greenTotal += color.G;
                blueTotal += color.B;
            }
            int count = colors.Length;
            int redAverage = redTotal / count;
            int greenAverage = greenTotal / count;
            int blueAverage = blueTotal / count;
            return Color.FromArgb(redAverage, greenAverage, blueAverage);
        }

        //归一化到彩虹色
        Color Rainbow(float progress)
        {
            float Gamma = 2.0f; //Gamma


            if (progress >= 1) progress = 0.999f;
            float[] ValTable = new float[]
            {
                0,                  //玫红
               0.18f,//1f / 6f,    //蓝
               0.37f,//2f / 6f,    //亮蓝
               0.58f,//3f / 6f,    //绿
               0.68f,//4f / 6f,    //黄
               0.83f,//5f / 6f,
                1                   //红
            };

            //计算出颜色
            float R = 0, G = 0, B = 0;


            #region 找到R颜色段
            {
                float[] R_ValTable = new float[]
                {
                    ValTable[0],    //0->1 减少
                    ValTable[1],    //1->2 =0
                    ValTable[2],    //2->4 增加到250
                    //ValTable[3],
                    ValTable[4],    //4->5 增加到255  
                    ValTable[5],    //5->6=255
                    ValTable[6],
                };

                float[] tTable = R_ValTable;
                int div = 0;
                for (int i = 0; i < tTable.Length - 1; i++)
                {
                    if (tTable[i] <= progress && progress < tTable[i + 1])
                    {
                        div = i;
                        break;
                    }
                }

                float valto1 = (progress - tTable[div]) / (tTable[div + 1] - tTable[div]);

                float ascending_Line = 255 * valto1;
                float descending_Line = 255 - ascending_Line;

                switch ((int)div)
                {
                    case 0:
                        R = descending_Line;
                        break;
                    case 1:
                        R = 0;
                        break;
                    case 2:
                        R = (250f - 0) / (255f - 0) * (ascending_Line - 0) + 0;
                        break;
                    case 3:
                        R = (255f - 250f) / (255f - 0) * (ascending_Line - 0) + 250f;
                        break;
                    default:
                        R = 255;
                        break;
                }

            }
            #endregion

            #region 找到G颜色段
            {
                float[] G_ValTable = new float[]
                {
                    ValTable[0],    //0->1  =0
                    ValTable[1],    //1->2  增加到226
                    ValTable[2],    //2->3  从226增加到255
                    ValTable[3],    //3->4  =255
                    ValTable[4],    //4->5  减少
                    ValTable[6]
                };

                float[] tTable = G_ValTable;
                int div = 0;
                for (int i = 0; i < tTable.Length - 1; i++)
                {
                    if (tTable[i] <= progress && progress < tTable[i + 1])
                    {
                        div = i;
                        break;
                    }
                }

                float valto1 = (progress - tTable[div]) / (tTable[div + 1] - tTable[div]);

                float ascending_Line = 255 * valto1;
                float descending_Line = 255 - ascending_Line;

                switch ((int)div)
                {
                    case 0:
                        G = 0;
                        break;
                    case 1:
                        G = (226f - 0) / (255f - 0) * (ascending_Line - 0) + 0;
                       // G = ascending_Line;
                        break;
                    case 2:
                        G = (255f - 226f) / (255f - 0) * (ascending_Line - 0) + 226f;
                        break;
                    case 3:
                        G = 255;
                        break;
                    default:
                        G = descending_Line;
                        break;
                }

            }
            #endregion

            #region 找到B颜色段
            {
                float[] B_ValTable = new float[]
                {
                    ValTable[0],    //0->2 =255
                    //ValTable[1],    
                    ValTable[2],    //2->3 255减少到95
                    ValTable[3],    //3->6 从95减少到0
                    //ValTable[4],
                    //ValTable[5],
                    ValTable[6]
                };

                float[] tTable = B_ValTable;
                int div = 0;
                for (int i = 0; i < tTable.Length - 1; i++)
                {
                    if (tTable[i] <= progress && progress < tTable[i + 1])
                    {
                        div = i;
                        break;
                    }
                }

                float valto1 = (progress - tTable[div]) / (tTable[div + 1] - tTable[div]);

                float ascending_Line = 255 * valto1;
                float descending_Line = 255 - ascending_Line;

                switch ((int)div)
                {
                    case 0:
                        B = 255;
                        break;
                    case 1:
                        B = (255f - 95f) / (255f - 0) * (descending_Line - 0) + 95f; ;
                        break;
                    case 2:
                        B = (95f - 0f) / (255f - 0) * (descending_Line - 0) + 0f; ;
                        break;
                    default:
                        B = descending_Line;
                        break;
                }

            }
            #endregion

            Color col = new Color();
            col = Color.FromArgb(255, (int)R, (int)G, (int)B);

            return col;
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
