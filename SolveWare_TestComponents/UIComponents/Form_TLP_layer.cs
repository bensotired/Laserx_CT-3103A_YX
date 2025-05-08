using System;
using System.Drawing;
using System.Windows.Forms;

namespace SolveWare_TestComponents.Model
{
    public partial class Form_TLP_layer : Form
    {
        const int EXE_CHARTS_SEED = 2;
 
        const float COL_WIDTH = 50f;
        public Form_TLP_layer()
        {
            InitializeComponent();
            this.TopLevel = false;
            this.Dock = DockStyle.Fill;
            this.DoubleBuffered = true;
        }
        public void LayoutSubForms(Form[] UIs)
        {
            try
            {
                this.SuspendLayout();
                this.tlp_layer.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single; 
                this.tlp_layer.RowCount = 0;
                this.tlp_layer.RowStyles.Clear();

                this.tlp_layer.ColumnCount = 0;
                this.tlp_layer.ColumnStyles.Clear();


                var colCount = EXE_CHARTS_SEED;
                var rowCount = UIs.Length / EXE_CHARTS_SEED + UIs.Length % EXE_CHARTS_SEED;
                var rowH = 100.0f / rowCount;
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    this.tlp_layer.RowStyles.Add(new RowStyle(SizeType.Percent, rowH));
                }
                for (int colIndex = 0; colIndex < colCount; colIndex++)
                {
                    this.tlp_layer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, COL_WIDTH));
                }
                this.tlp_layer.Location = new Point(0, 0);
                this.tlp_layer.Dock = DockStyle.Fill;

                for(int i = 0; i < UIs.Length; i++)
                {
                    UIs[i].Hide();
                    UIs[i].TopLevel = false;
                    UIs[i].FormBorderStyle = FormBorderStyle.None;
                    UIs[i].Dock = DockStyle.Fill;
                    UIs[i].Show();
                    var colIndex = i % EXE_CHARTS_SEED;
                    var rowIndex = i / EXE_CHARTS_SEED;
                    this.tlp_layer.Controls.Add(UIs[i], i % EXE_CHARTS_SEED, i / EXE_CHARTS_SEED);
                }
        
                this.ResumeLayout(false);
            }
            catch (Exception ex)
            {
            }
        }
        public void LayoutSubForms(Form[] UIs,int columnCount)
        {
            try
            {
                this.SuspendLayout();
                this.tlp_layer.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                this.tlp_layer.RowCount = 0;
                this.tlp_layer.RowStyles.Clear();

                this.tlp_layer.ColumnCount = 0;
                this.tlp_layer.ColumnStyles.Clear();


                //var colCount = EXE_CHARTS_SEED;
                var rowCount = UIs.Length / columnCount + UIs.Length % columnCount;
                var rowH = 100.0f / rowCount;
                this.tlp_layer.RowCount = rowCount;
                this.tlp_layer.ColumnCount = columnCount;
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                  
                    this.tlp_layer.RowStyles.Add(new RowStyle(SizeType.Percent, rowH));
                }
                for (int colIndex = 0; colIndex < columnCount; colIndex++)
                {
                    this.tlp_layer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, COL_WIDTH));
                }
                this.tlp_layer.Location = new Point(0, 0);
                this.tlp_layer.Dock = DockStyle.Fill;

                for (int i = 0; i < UIs.Length; i++)
                {
                    UIs[i].Hide();
                    UIs[i].TopLevel = false;
                    UIs[i].FormBorderStyle = FormBorderStyle.None;
                    UIs[i].Dock = DockStyle.Fill;
                    UIs[i].Show();
                    var colIndex = i % columnCount;
                    var rowIndex = i / columnCount;
                    this.tlp_layer.Controls.Add(UIs[i], i % columnCount, i / columnCount);
                }

                this.ResumeLayout(false);
            }
            catch (Exception ex)
            {
            }
        }
        public void LayoutSubForms(Form[] UIs, int rowCount ,int columnCount)
        {
            try
            {
                this.SuspendLayout();
                this.tlp_layer.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                this.tlp_layer.RowCount = rowCount;
                this.tlp_layer.RowStyles.Clear();

                this.tlp_layer.ColumnCount = columnCount;
                this.tlp_layer.ColumnStyles.Clear();


                //var colCount = EXE_CHARTS_SEED;
                //var rowCount = UIs.Length / columnCount + UIs.Length % columnCount;
                var rowH = 100.0f / rowCount;
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    this.tlp_layer.RowStyles.Add(new RowStyle(SizeType.Percent, rowH));
                }
                for (int colIndex = 0; colIndex < columnCount; colIndex++)
                {
                    this.tlp_layer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, COL_WIDTH));
                }
                this.tlp_layer.Location = new Point(0, 0);
                this.tlp_layer.Dock = DockStyle.Fill;

                for (int i = 0; i < UIs.Length; i++)
                {
                    UIs[i].Hide();
                    UIs[i].TopLevel = false;
                    UIs[i].FormBorderStyle = FormBorderStyle.None;
                    UIs[i].Dock = DockStyle.Fill;
                    UIs[i].Show();
                    var colIndex = i % columnCount;
                    var rowIndex = i / columnCount;
                    this.tlp_layer.Controls.Add(UIs[i], i % columnCount, i / columnCount);
                }

                this.ResumeLayout(false);
            }
            catch (Exception ex)
            {
            }
        }
        public void ClearCharts()
        {
            try
            {
                this.tlp_layer.Controls.Clear();
            }
            catch (Exception ex)
            {
            }
        }

        private void Form_TLP_layer_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}