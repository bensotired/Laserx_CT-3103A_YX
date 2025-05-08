
namespace TestPlugin_Demo
{
    partial class Form_CoarseTuning
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart_coarse = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel_coarse = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.chart_coarse)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart_coarse
            // 
            chartArea1.Name = "ChartArea1";
            this.chart_coarse.ChartAreas.Add(chartArea1);
            this.chart_coarse.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart_coarse.Legends.Add(legend1);
            this.chart_coarse.Location = new System.Drawing.Point(3, 3);
            this.chart_coarse.Name = "chart_coarse";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart_coarse.Series.Add(series1);
            this.chart_coarse.Size = new System.Drawing.Size(886, 292);
            this.chart_coarse.TabIndex = 0;
            this.chart_coarse.Text = "chart1";
            // 
            // panel_coarse
            // 
            this.panel_coarse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_coarse.Location = new System.Drawing.Point(3, 301);
            this.panel_coarse.Name = "panel_coarse";
            this.panel_coarse.Size = new System.Drawing.Size(886, 292);
            this.panel_coarse.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.chart_coarse, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel_coarse, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(892, 596);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // Form_CoarseTuning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 596);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form_CoarseTuning";
            this.Text = "Form_CoarseTuning";
            ((System.ComponentModel.ISupportInitialize)(this.chart_coarse)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart_coarse;
        private System.Windows.Forms.Panel panel_coarse;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}