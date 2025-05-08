
namespace TestPlugin_CoarseTuning
{
    partial class Form_Main
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
            this.Chart_Groups_main = new ChartDirector.WinChartViewer();
            this.Chart_Midlines_main = new ChartDirector.WinChartViewer();
            this.Chart_LabeledPoints_main = new ChartDirector.WinChartViewer();
            this.bt_run = new System.Windows.Forms.Button();
            this.bt_savecsv = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Chart_Groups_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chart_Midlines_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chart_LabeledPoints_main)).BeginInit();
            this.SuspendLayout();
            // 
            // Chart_Groups_main
            // 
            this.Chart_Groups_main.Location = new System.Drawing.Point(502, 0);
            this.Chart_Groups_main.Name = "Chart_Groups_main";
            this.Chart_Groups_main.Size = new System.Drawing.Size(470, 316);
            this.Chart_Groups_main.TabIndex = 0;
            this.Chart_Groups_main.TabStop = false;
            // 
            // Chart_Midlines_main
            // 
            this.Chart_Midlines_main.Location = new System.Drawing.Point(12, 322);
            this.Chart_Midlines_main.Name = "Chart_Midlines_main";
            this.Chart_Midlines_main.Size = new System.Drawing.Size(470, 316);
            this.Chart_Midlines_main.TabIndex = 1;
            this.Chart_Midlines_main.TabStop = false;
            // 
            // Chart_LabeledPoints_main
            // 
            this.Chart_LabeledPoints_main.Location = new System.Drawing.Point(502, 322);
            this.Chart_LabeledPoints_main.Name = "Chart_LabeledPoints_main";
            this.Chart_LabeledPoints_main.Size = new System.Drawing.Size(470, 316);
            this.Chart_LabeledPoints_main.TabIndex = 2;
            this.Chart_LabeledPoints_main.TabStop = false;
            // 
            // bt_run
            // 
            this.bt_run.Location = new System.Drawing.Point(145, 73);
            this.bt_run.Name = "bt_run";
            this.bt_run.Size = new System.Drawing.Size(129, 69);
            this.bt_run.TabIndex = 3;
            this.bt_run.Text = "Run";
            this.bt_run.UseVisualStyleBackColor = true;
            this.bt_run.Click += new System.EventHandler(this.bt_run_Click);
            // 
            // bt_savecsv
            // 
            this.bt_savecsv.Location = new System.Drawing.Point(145, 181);
            this.bt_savecsv.Name = "bt_savecsv";
            this.bt_savecsv.Size = new System.Drawing.Size(129, 69);
            this.bt_savecsv.TabIndex = 4;
            this.bt_savecsv.Text = "Save csvs";
            this.bt_savecsv.UseVisualStyleBackColor = true;
            this.bt_savecsv.Click += new System.EventHandler(this.bt_savecsv_Click);
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 643);
            this.Controls.Add(this.bt_savecsv);
            this.Controls.Add(this.bt_run);
            this.Controls.Add(this.Chart_LabeledPoints_main);
            this.Controls.Add(this.Chart_Midlines_main);
            this.Controls.Add(this.Chart_Groups_main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_Main";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.Chart_Groups_main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chart_Midlines_main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chart_LabeledPoints_main)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ChartDirector.WinChartViewer Chart_Groups_main;
        private ChartDirector.WinChartViewer Chart_Midlines_main;
        private ChartDirector.WinChartViewer Chart_LabeledPoints_main;
        private System.Windows.Forms.Button bt_run;
        private System.Windows.Forms.Button bt_savecsv;
    }
}