using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TestPlugin_Demo
{
    public partial class Form_CoarseTuning : Form
    {
        string CoarseTuningPath { get; set; }
        public Form_CoarseTuning(string coarseTuningPath)
        {
            InitializeComponent();
            CoarseTuningPath = coarseTuningPath;
            UpDataCoarseTuning();
        }

        private void UpDataCoarseTuning()
        {
            try
            {

                DataTable dataTable = new DataTable();

                using (StreamReader reader = new StreamReader(CoarseTuningPath))
                {
                    string[] headers = reader.ReadLine().Split(','); // 读取第一行作为表头

                    foreach (string header in headers)
                    {
                        dataTable.Columns.Add(header); // 设置表头列名
                    }

                    while (!reader.EndOfStream)
                    {
                        string[] rows = reader.ReadLine().Split(',');

                        DataRow dataRow = dataTable.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dataRow[i] = rows[i];
                        }
                        dataTable.Rows.Add(dataRow); // 添加行数据
                    }
                }
                string CH = "CH";
                List<double> CHList = new List<double>();
                foreach (DataRow row in dataTable.Rows)
                {
                    CHList.Add(Convert.ToDouble(row[CH]));
                }

                string M1 = "Mirror1[mA]";//Mirror1[mA]
                List<double> M1List = new List<double>();
                foreach (DataRow row in dataTable.Rows)
                {
                    M1List.Add(Convert.ToDouble(row[M1]));
                }

                string M2 = "Mirror2[mA]";
                List<double> M2List = new List<double>();
                foreach (DataRow row in dataTable.Rows)
                {
                    M2List.Add(Convert.ToDouble(row[M2]));
                }

                UpDataChart(CHList, M1List, M2List);
                UpDataMaxtrix(CHList);
            }
            catch (Exception ex)
            {
                throw new Exception($"coarseTuning eorr [{ex.Message}]");
            }
        }
        private void UpDataChart(List<double> CHList, List<double> M1List, List<double> M2List)
        {
            this.chart_coarse.ChartAreas.Clear();
            this.chart_coarse.Series.Clear();
            this.chart_coarse.Titles.Clear();

            ChartArea chartArea = new ChartArea();
            chartArea.AxisX = new Axis();
            chartArea.AxisX.IsStartedFromZero = false;
            chartArea.AxisY = new Axis();
            chartArea.AxisY.Maximum = M1List.Max();
            chartArea.AxisY.Minimum = M1List.Min();
            chartArea.AxisY.IsStartedFromZero = false;
            chartArea.AxisY2 = new Axis();
            chartArea.AxisY2.Maximum = M2List.Max();
            chartArea.AxisY2.Minimum = M2List.Min();
            chartArea.AxisY2.IsStartedFromZero = false;

            chart_coarse.ChartAreas.Add(chartArea);
            chart_coarse.Titles.Add("CoarseTuning");

            Series series = new Series("M1");
            series.ChartType = SeriesChartType.Point;
            series.XAxisType = AxisType.Primary;
            series.YAxisType = AxisType.Primary;

            for (int i = 0; i < CHList.Count; i++)
            {
                series.Points.AddXY(CHList[i], M1List[i]);
            }
            chart_coarse.Series.Add(series);

            Series series2 = new Series("M2");
            series2.ChartType = SeriesChartType.Point;
            series2.XAxisType = AxisType.Primary;
            series2.YAxisType = AxisType.Secondary;

            for (int i = 0; i < CHList.Count; i++)
            {
                series2.Points.AddXY(CHList[i], M2List[i]);
            }
            chart_coarse.Series.Add(series2);


        }
        private void UpDataMaxtrix(List<double> CHList)
        {
            var min = -19;
            var max = 120;

            int row = 20;
            int column = 15;


            for (int i = 1; i <= row; i++)
            {
                for (int m = 1; m <= column; m++)
                {

                    Button button = new Button();
                    button.Enabled = false;
                    button.AutoSize = false;
                    button.Size = new Size(40, 25);
                    var w = 35;
                    var h = 20;
                    button.Location = new Point(w * m + 100, h * i);
                    button.TextAlign = ContentAlignment.MiddleCenter;
                    button.Text = min.ToString();
                    if (CHList.Contains(min))
                    {
                        button.BackColor = Color.Green;
                    }
                    else
                    {
                        button.BackColor = Color.Gray;
                    }
                    panel_coarse.Controls.Add(button);
                    min++;
                    if (min > max)
                    {
                        return;
                    }
                }
            }
        }
    }
}
