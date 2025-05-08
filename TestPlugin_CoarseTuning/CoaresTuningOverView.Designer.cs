
namespace TestPlugin_CoarseTuning
{
    partial class CoaresTuningOverView
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.Chart_Groups = new ChartDirector.WinChartViewer();
            this.Chart_Midlines = new ChartDirector.WinChartViewer();
            this.Chart_LabeledPoints = new ChartDirector.WinChartViewer();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Chart_Groups)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chart_Midlines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chart_LabeledPoints)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.Chart_Groups, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.Chart_Midlines, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.Chart_LabeledPoints, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(955, 629);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // Chart_Groups
            // 
            this.Chart_Groups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Chart_Groups.Location = new System.Drawing.Point(3, 3);
            this.Chart_Groups.Name = "Chart_Groups";
            this.Chart_Groups.Size = new System.Drawing.Size(471, 308);
            this.Chart_Groups.TabIndex = 0;
            this.Chart_Groups.TabStop = false;
            // 
            // Chart_Midlines
            // 
            this.Chart_Midlines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Chart_Midlines.Location = new System.Drawing.Point(480, 3);
            this.Chart_Midlines.Name = "Chart_Midlines";
            this.Chart_Midlines.Size = new System.Drawing.Size(472, 308);
            this.Chart_Midlines.TabIndex = 1;
            this.Chart_Midlines.TabStop = false;
            // 
            // Chart_LabeledPoints
            // 
            this.Chart_LabeledPoints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Chart_LabeledPoints.Location = new System.Drawing.Point(3, 317);
            this.Chart_LabeledPoints.Name = "Chart_LabeledPoints";
            this.Chart_LabeledPoints.Size = new System.Drawing.Size(471, 309);
            this.Chart_LabeledPoints.TabIndex = 2;
            this.Chart_LabeledPoints.TabStop = false;
            // 
            // CoaresTuningOverView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 629);
            this.Controls.Add(this.tableLayoutPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CoaresTuningOverView";
            this.Text = "CoaresTuningOverView";
            this.Load += new System.EventHandler(this.CoaresTuningOverView_Load);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Chart_Groups)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chart_Midlines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chart_LabeledPoints)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private ChartDirector.WinChartViewer Chart_Groups;
        private ChartDirector.WinChartViewer Chart_Midlines;
        private ChartDirector.WinChartViewer Chart_LabeledPoints;
    }
}