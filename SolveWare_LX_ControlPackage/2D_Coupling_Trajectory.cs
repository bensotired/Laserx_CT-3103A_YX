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

namespace LX_2DChart
{
    //耦合轨迹显示
    public partial class Coupling_Trajectory : UserControl
    {
        private Graphics g;

        private double[] FH_Pos;
        private double[] FH_Value;
        private double[] FV_Pos;
        private double[] FV_Value;

        private const int PicLeftWidth = 100; //图像左侧色带 200像素

        private const float PicZoon = 0.1f; // 0.1分度
        private const int PicWidth = (int)(180 / PicZoon); //图像宽度    180度  0.1分度
        private const int PicHeight = (int)(180 / PicZoon); //图像高度   180度  0.1分度

        private const int PicCenter_X = PicWidth / 2 + PicLeftWidth;
        private const int PicCenter_Y = PicHeight / 2;

        private const int PicCenter_Xpos = 900; //0度的地方是位置90度;
        private const int PicCenter_Ypos = 900;

        Bitmap bmp = null;


        public Coupling_Trajectory()
        {
            InitializeComponent();


            int w = PicLeftWidth + PicWidth;
            int h = PicHeight;
            bmp = new Bitmap(w, h);

            g = Graphics.FromImage(bmp);

        }



        private void UserControl1_Paint(object sender, PaintEventArgs e)
        {
            //Update_Paint();
        }

        private void Update_Paint()
        {
            this.BackgroundImage = null;
            this.BackgroundImage = bmp;
            this.BackgroundImageLayout = ImageLayout.Zoom;
        }

        private void DrawFormat_Before()
        {
            g.Clear(BackColor);
            //g.Clear(Color.Red);

            SolidBrush myBrush = new SolidBrush(Color.Black);
            //底色
            g.FillRectangle(myBrush, PicLeftWidth, 0, PicLeftWidth + PicWidth, PicHeight);

            int toppos = 10;
            int downpos = PicHeight - PicLeftWidth / 2 - 10;
            int leftpos = 10;
            int rightpos = PicLeftWidth - 2 * leftpos;
            //左下角
            g.DrawString("Lin", new Font("Times New Roman", PicLeftWidth/3), myBrush, leftpos, PicHeight - PicLeftWidth/2);

            //左上角
            myBrush.Color = Color.White;
            g.DrawString(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), new Font("Times New Roman", PicLeftWidth/3), myBrush, PicLeftWidth+leftpos, toppos);


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
            g.DrawLine(myPen, PicLeftWidth, PicCenter_Y, PicLeftWidth + PicWidth, PicCenter_Y);  //水平线
            g.DrawLine(myPen, PicCenter_X, 0, PicCenter_X, PicHeight);  //垂直线




        }

        private void DrawFormat_After()
        {
            Pen myPen = new Pen(Color.Gray, 5);
            g.DrawLine(myPen, PicLeftWidth, PicCenter_Y, PicLeftWidth + PicWidth, PicCenter_Y);  //水平线
            g.DrawLine(myPen, PicCenter_X, 0, PicCenter_X, PicHeight);  //垂直线
        }


        public Image GetPicture()
        {

            Update_Paint();

            var newbmp = bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), bmp.PixelFormat);
            return newbmp;
        }

        /// <summary>
        /// 填充绘图数据
        /// </summary>
        /// <param name="tX_Pos_mm">水平坐标序列(-90 -> +90)</param>
        /// <param name="tY_Pos_mm">垂直坐标序列</param>
        /// <param name="tFV_Pos_Deg">垂直角度序列(-90 -> +90)</param>
        /// <param name="tFV_Power">垂直功率序列</param>
        public void SetData(double[] tX_Pos_mm, double[] tY_Pos_mm, double[] tFV_Pos_Deg, double[] tFV_Power)
        {
            //待更新

            //FH_Pos = tFH_Pos_Deg;
            //FH_Value = tFH_Power;
            //FV_Pos = tFV_Pos_Deg;
            //FV_Value = tFV_Power;

            //g.Clear(BackColor);

            //if (FH_Pos == null || FH_Value == null || FV_Pos == null || FV_Value == null)
            //    return;

            //if (FH_Pos.Length <= 0 || FH_Value.Length <= 0 || FV_Pos.Length <= 0 || FV_Value.Length <= 0)
            //    return;

            //if (FH_Pos.Length != FH_Value.Length)
            //    return;
            //if (FV_Pos.Length != FV_Value.Length)
            //    return;

            ////归一化值
            //double[] FH_Value_To1 = new double[FH_Value.Length];
            //double[] FV_Value_To1 = new double[FV_Value.Length];

            ////找到最大位置
            //double FHmax = double.MinValue;
            //double FHmin = double.MaxValue;
            //int FHmaxpos =0;
            //double FVmax = double.MinValue;
            //double FVmin = double.MaxValue;
            //int FVmaxpos =0;

            //for(int i=0;i<FH_Pos.Length;i++)
            //{
            //    if(FH_Value[i]>FHmax)
            //    {
            //        FHmax = FH_Value[i];
            //        FHmaxpos = i;
            //    }

            //    if(FH_Value[i]<FHmin)
            //    {
            //        FHmin = FH_Value[i];
            //    }
            //}
            //for (int i = 0; i < FV_Pos.Length; i++)
            //{
            //    if (FV_Value[i] > FVmax)
            //    {
            //        FVmax = FV_Value[i];
            //        FVmaxpos = i;
            //    }

            //    if(FV_Value[i]<FVmin)
            //    {
            //        FVmin = FV_Value[i];
            //    }
            //}

            //if (FHmax == 0 || FVmax == 0)
            //    return;

            ////数据归一化
            //for(int i=0;i<FH_Pos.Length;i++)
            //{
            //    FH_Value_To1[i] = (FH_Value[i]-FHmin) / (FHmax-FHmin);
            //}
            //for(int i=0;i<FV_Pos.Length;i++)
            //{
            //    FV_Value_To1[i] = (FV_Value[i]-FVmin) / (FVmax-FVmin);
            //}



            //DrawFormat_Before();
            //SolidBrush myBrush = new SolidBrush(Color.Black);


            ////Pen myPen = new Pen(Color.Black);

            ////循环画椭圆 0.001灰度画一次圆
            //#region 画斑点
            //int step = 1000;
            //for (int v = 0; v < step; v++)
            //{
            //    //绘制颜色是多少的椭圆
            //    float fv = (float)v / (float)step;

            //    //从曲线获取长轴短轴和位置
            //    PointF c_xy = new PointF();
            //    double cx = 0;
            //    double cy = 0;
            //    bool ok = GetPos(
            //        FH_Pos, FH_Value_To1, FHmaxpos,
            //        FV_Pos, FV_Value_To1, FVmaxpos,
            //        fv,
            //        ref c_xy,   //中心
            //        ref cx,     //H轴长
            //        ref cy     //V轴长
            //        );

            //    if (ok)
            //    {
            //        //获取颜色
            //        Color col = Rainbow(v / (float)step);
            //        myBrush.Color = col;
            //        //画椭圆
            //        float x1 = (float)(PicCenter_X + c_xy.X / PicZoon - cx / 2 / PicZoon);
            //        float y1 = (float)(PicCenter_Y + c_xy.Y / PicZoon - cy / 2 / PicZoon);

            //        float w = (float)(cx / PicZoon);
            //        float h = (float)(cy / PicZoon);

            //        g.FillEllipse(myBrush, x1, y1, w, h);
            //        //g.DrawEllipse(myPen, x1, y1, w, h);
            //    }
            //}
            //#endregion
            
            //DrawFormat_After();

            //#region 画曲线
            //Pen myPen = new Pen(Color.Red, 8);

            //List<PointF> FVPoint = new List<PointF>();
            //List<PointF> FHPoint = new List<PointF>();

            ////FV向右画曲线
            //{
            //    int Area_x1 = PicLeftWidth;
            //    int Area_y1 = 0;

            //    int Area_x2 = PicLeftWidth + PicWidth / 3;
            //    int Area_y2 = PicHeight;


            //    for (int i = 0; i < FV_Value_To1.Length; i++)
            //    {
            //        float y = (float)((Area_y2 - Area_y1) / (90f-(-90f)) * (FV_Pos[i]-(-90)) + Area_y1);  //角度向下增加
            //        float x = (float)((Area_x2 - Area_x1) / 1 * (FV_Value_To1[i]-0) + Area_x1);             //能量向右增加
            //        FVPoint.Add(new PointF(x, y));
            //    }

            //    g.DrawLines(myPen, FVPoint.ToArray());

            //}

            ////FH向上画曲线
            //{
            //    int Area_x1 = PicLeftWidth;
            //    int Area_y1 = PicHeight - PicHeight / 3;

            //    int Area_x2 = PicLeftWidth + PicWidth;
            //    int Area_y2 = PicHeight;


            //    for (int i = 0; i < FH_Value_To1.Length; i++)
            //    {
            //        float x = (float)((Area_x2 - Area_x1) / (90f-(-90f)) * (FH_Pos[i]-(-90)) + Area_x1);  //角度向右增加
            //        float y = (float)((Area_y2 - Area_y1) / (0f-1f) * (FV_Value_To1[i]-0) + Area_y2);             //能量向上增加

            //        FHPoint.Add(new PointF(x, y));
            //    }

            //    g.DrawLines(myPen, FHPoint.ToArray());

            //}

            //#endregion

            //Update_Paint();
        }


        //归一化到彩虹色
        Color Rainbow(float progress)
        {
            float step= 1f / 7f;  //颜色分8级
            float colorstep = step / 255f; //颜色变化
            if (progress >= 1)
                return Color.FromArgb(255, 255, 255, 255); //白色

            int div = (int)(progress / step);  //级别

            int ascending = (int)((progress - step * div) / colorstep);//增加
            int descending = 255 - ascending;   //减少


            Color col = new Color();
            switch ((int)div)
            {
                case 0:
                    col= Color.FromArgb(255, 0, 0, ascending);         //黑到蓝  增加
                    break;
                case 1:
                    col=  Color.FromArgb(255, 0, ascending, 255);      //蓝到亮蓝  增加
                    break;
                case 2:
                    col=  Color.FromArgb(255, 0, 255, descending);      //亮蓝到绿  减少
                    break;
                case 3:
                    col=  Color.FromArgb(255, ascending, 255, 0);      //绿到黄  增加
                    break;
                case 4:
                    col=  Color.FromArgb(255, 255, descending, 0);      //黄到红  减少
                    break;
                case 5:
                    col=  Color.FromArgb(255, 255, 0, ascending);      //红到青  增加
                    break;
                case 6:
                    col=  Color.FromArgb(255, 255, ascending, 255);      //青到白  增加
                    break;
                default:  //case 7
                    col=  Color.FromArgb(255, 255, 255, 255);   //白色
                    break;
            }
            //Console.WriteLine($"{progress},{div},{ascending},{descending},{col.R},{col.G},{col.B}");

            return col;
        }

        //计算椭圆范围
        private bool GetPos(
                    double[] FH_Pos, double[] FH_Value_To1,int FHmaxpos,
                    double[] FV_Pos, double[] FV_Value_To1,int FVmaxpos,
                    double fv,
                    ref PointF c_xy,   //中心
                    ref double cx,     //H轴长
                    ref double cy     //V轴长
                    )
        {
            double FHpos_L=-1, FHpos_R = -1;
            double FVpos_L = -1, FVpos_R = -1;

            bool bFH_L = false, bFH_R = false;
            bool bFV_L = false, bFV_R = false;

            //曲线寻找fv的位置
            #region FH
            for (int i=0;i<FHmaxpos;i++)
            {
                if(FH_Value_To1[i]==fv)
                {
                    FHpos_L = FH_Pos[i];
                    bFH_L = true;
                    break;
                }
                else if(FH_Value_To1[i] < fv && FH_Value_To1[i+1] > fv)
                {
                    FHpos_L = (FH_Pos[i]+ FH_Pos[i+1])/2;
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
                else if (FH_Value_To1[i-1] > fv && FH_Value_To1[i] < fv)
                {
                    FHpos_R = (FH_Pos[i-1] + FH_Pos[i]) / 2;
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
            if((bFH_L & bFH_R & bFV_L & bFV_R)==false)
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
            DrawFormat_Before();
            Update_Paint();
        }


    }
}
