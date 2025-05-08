using System;
using System.Drawing;
using System.Windows.Forms;

namespace SolveWare_TestComponents.Model
{
    public partial class Form_ExecutorCharts : Form
    {
        const int EXE_CHARTS_SEED = 2;
 
        const float COL_WIDTH = 50f;
        public Form_ExecutorCharts()
        {
            InitializeComponent();
        }
        public void LayoutCharts(Form[] chartUIs)
        {
            try
            {
                this.SuspendLayout();
                this.tlp_exeCharts.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single; 
                this.tlp_exeCharts.RowCount = 0;
                this.tlp_exeCharts.RowStyles.Clear();

                this.tlp_exeCharts.ColumnCount = 0;
                this.tlp_exeCharts.ColumnStyles.Clear();


                var colCount = EXE_CHARTS_SEED;
                var rowCount = chartUIs.Length / EXE_CHARTS_SEED + chartUIs.Length % EXE_CHARTS_SEED;
                var rowH = 100.0f / rowCount;
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    this.tlp_exeCharts.RowStyles.Add(new RowStyle(SizeType.Percent, rowH));
                }
                for (int colIndex = 0; colIndex < colCount; colIndex++)
                {
                    this.tlp_exeCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, COL_WIDTH));
                }
                this.tlp_exeCharts.Location = new Point(0, 0);
                this.tlp_exeCharts.Dock = DockStyle.Fill;

                for(int i = 0; i < chartUIs.Length; i++)
                {
                    chartUIs[i].Hide();
                    chartUIs[i].TopLevel = false;
                    chartUIs[i].FormBorderStyle = FormBorderStyle.None;
                    chartUIs[i].Dock = DockStyle.Fill;
                    chartUIs[i].Show();

                    this.tlp_exeCharts.Controls.Add(chartUIs[i], i % EXE_CHARTS_SEED, i / EXE_CHARTS_SEED);
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
                this.tlp_exeCharts.Controls.Clear();
            }
            catch (Exception ex)
            {
            }
        }
    }
}