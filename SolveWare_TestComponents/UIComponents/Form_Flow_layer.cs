using System;
using System.Drawing;
using System.Windows.Forms;

namespace SolveWare_TestComponents.Model
{
    public partial class Form_Flow_layer : Form
    {
        private const int EXE_CHARTS_SEED = 2;

        private const float COL_WIDTH = 50f;

        public Form_Flow_layer()
        {
            InitializeComponent();
            this.TopLevel = false;
            this.Dock = DockStyle.Fill;
            this.DoubleBuffered = true;
        }

        public void LayoutSubForms(Form[] UIs)
        {
            LayoutSubForms(UIs, 2, EXE_CHARTS_SEED);
        }

        public void LayoutSubForms(Form[] UIs, int columnCount)
        {
            LayoutSubForms(UIs, 2, columnCount);
        }

        int gcolumnCount = 2;
        int growCount = 2;

        public void LayoutSubForms(Form[] UIs, int rowCount, int columnCount)
        {
            try
            {
                //20230302 不理这个参数
                gcolumnCount = columnCount = 2;
                growCount = rowCount = 2;


                this.SuspendLayout();

                int flow_w = this.flow_layer.Width-30;
                int flow_h = this.flow_layer.Height;

                int plane_w = flow_w / columnCount;
                int plane_h = flow_h / rowCount;

                for (int i = 0; i < UIs.Length; i++)
                {
                    Panel p = new Panel();


                    UIs[i].Hide();
                    UIs[i].TopLevel = false;
                    UIs[i].FormBorderStyle = FormBorderStyle.None;
                    UIs[i].Dock = DockStyle.Fill;
                    UIs[i].Show();
                    p.Controls.Add(UIs[i]);
                    p.Width = plane_w;
                    p.Height = plane_h;
                              
                    this.flow_layer.Controls.Add(p);
                }
                this.ResumeLayout(true);
            }
            catch (Exception ex)
            {
            }
        }

        public void ClearCharts()
        {
            try
            {
                this.flow_layer.Controls.Clear();
            }
            catch (Exception ex)
            {
            }
        }

        private void Form_Flow_layer_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void flow_layer_Resize(object sender, EventArgs e)
        {
            try
            {
                this.SuspendLayout();

                int flow_w = this.flow_layer.Width-30;
                int flow_h = this.flow_layer.Height;

                int plane_w = flow_w / gcolumnCount;
                int plane_h = flow_h / growCount;

                for (int i = 0; i < this.flow_layer.Controls.Count; i++)
                {
                    this.flow_layer.Controls[i].Width = plane_w;
                    this.flow_layer.Controls[i].Height = plane_h;
                }
                this.ResumeLayout(true);
            }
            catch (Exception ex)
            {
            }
        }
    }
}