using SolveWare_TestPlugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestPlugin_Demo
{
    using OpenCvSharp;
    using OpenCvSharp.Extensions;
    using Basler.Pylon;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Xml.Linq;
    using System.Threading;

    public partial class Form_OpenCV : Form_MainPage_TestPlugin<TestPluginWorker_CT3103>
    {
        private VideoCapture capture;
        private bool isopen = false;
        private bool saveImgFlag = false;
        private string filePath = "";
        public Form_OpenCV()
        {
            InitializeComponent();
        }
        private void Form_OpenCV_Load(object sender, EventArgs e)
        {
            置顶ToolStripMenuItem.Visible = false;
            取消置顶ToolStripMenuItem.Visible = false;
            窗口停靠ToolStripMenuItem.Visible = false;
            hScrollBarExposureTime.Minimum = -10;
            hScrollBarExposureTime.Maximum = 10;
            hScrollBarExposureTime.LargeChange = 1;
        }

        private void 窗口浮动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //this._core.PopUI(this);
                this._core.PopUI_DefaultSize(this);
                置顶ToolStripMenuItem.Visible = true;
                取消置顶ToolStripMenuItem.Visible = true;
                窗口停靠ToolStripMenuItem.Visible = true;
            }
            catch (Exception ex)
            {
            }
        }

        public delegate void OpenCVShow();
        public event OpenCVShow opencvshow;
        private void 窗口停靠ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //this._core.DockingMessageBoard();
                opencvshow();
                this.Close();
            }
            catch (Exception ex)
            {

            }
        }

        private void Form_OpenCV_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopSnap();
            //e.Cancel = true;
            //this._core.DockingMessageBoard();
            opencvshow();
        }

        private void btnOneshot_Click(object sender, EventArgs e)
        {
            capture = new VideoCapture(0);
            if (!capture.IsOpened())
            {
                MessageBox.Show("无法打开摄像头");
                return;
            }
            isopen = true;
            IsStopSnap = false;
            Thread video_th = new Thread(StartCapturing);
            video_th.IsBackground = true;
            video_th.Start();
            btnOneshot.Enabled = false;
        }
        public void StartCapturing()
        {
            Mat frame = new Mat();
            while (true)
            {
                try
                {
                    Thread.Sleep(1);
                    capture.Read(frame);//读取图像帧
                    if (frame.Empty())
                    {
                        break;
                    }
                    Bitmap bitmap = BitmapConverter.ToBitmap(frame);
                    if (saveImgFlag == true)
                    {
                        try
                        {
                            bitmap.Save(filePath, ImageFormat.Jpeg);
                            saveImgFlag = false;
                            MessageBox.Show("保存成功！");
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    pictureBoxCamera.Image = bitmap;
                    this.Invoke((EventHandler)delegate
                    {
                        pictureBoxCamera.Refresh();
                    });
                }
                catch (Exception ex)
                {

                    //throw;
                }


            }
        }
    
        private void btnContinues_Click(object sender, EventArgs e)
        {
            if (!isopen)
            {
                MessageBox.Show("未打开摄像头");
                return;
            }
            if (IsStopSnap)
            {
                MessageBox.Show("已停止采集");
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            // 设置对话框的标题
            saveFileDialog.Title = "Save Image";

            // 设置默认的文件名和文件类型过滤器
            saveFileDialog.FileName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            saveFileDialog.Filter = "Image files (*.Png)|*.Png|Image files (*.Jpg)|*.Jpg";

            // 显示对话框并获取用户的操作结果
            DialogResult result = saveFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                // 用户点击了保存按钮
                filePath = saveFileDialog.FileName;
                //string filter=saveFileDialog.Filter;

                // 在这里进行保存文件的操作，例如：
                // File.WriteAllText(filePath, "Hello, world!");

                Console.WriteLine("File saved to: " + filePath);
            }
            else if (result == DialogResult.Cancel)
            {
                // 用户点击了取消按钮
                Console.WriteLine("Save cancelled");
            }
            saveImgFlag = true;
        }

        private void btnStopSnap_Click(object sender, EventArgs e)
        {
            btnOneshot.Enabled = true;
            Thread.Sleep(300);
            StopSnap();
        }
        bool IsStopSnap = false;
        private void StopSnap()
        {
            if (capture == null)
            {
                return;
            }
            try
            {
                capture.Dispose();
                IsStopSnap = true;
            }
            catch (Exception exception)
            {
                this._plugin.Log_Global(exception.Message);
            }
        }

        private void hScrollBarExposureTime_Scroll(object sender, ScrollEventArgs e)
        {
            if (IsStopSnap)
            {
                return;
            }
            if (capture == null)
            {
                return;
            }
            if (!capture.IsOpened())
            {
                MessageBox.Show("无法打开摄像头");
                return;
            }

            capture.Exposure = hScrollBarExposureTime.Value;
            ExposureTimeNowValue.Text = hScrollBarExposureTime.Value.ToString();

        }

        private void 置顶ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.TopMost = true;
            }
            catch (Exception ex)
            {

            }
        }

        private void 取消置顶ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.TopMost = false;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
