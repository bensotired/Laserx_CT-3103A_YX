namespace SolveWare_TestComponents.Model
{
    partial class Form_ZedGraphChart
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
            this.components = new System.ComponentModel.Container();
            this.zedGraph_Chart = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // zedGraph_Chart
            // 
            this.zedGraph_Chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraph_Chart.Location = new System.Drawing.Point(0, 0);
            this.zedGraph_Chart.Name = "zedGraph_Chart";
            this.zedGraph_Chart.ScrollGrace = 0D;
            this.zedGraph_Chart.ScrollMaxX = 0D;
            this.zedGraph_Chart.ScrollMaxY = 0D;
            this.zedGraph_Chart.ScrollMaxY2 = 0D;
            this.zedGraph_Chart.ScrollMinX = 0D;
            this.zedGraph_Chart.ScrollMinY = 0D;
            this.zedGraph_Chart.ScrollMinY2 = 0D;
            this.zedGraph_Chart.Size = new System.Drawing.Size(780, 472);
            this.zedGraph_Chart.TabIndex = 0;
            this.zedGraph_Chart.UseExtendedPrintDialog = true;
            // 
            // Form_ModuleChart3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 472);
            this.Controls.Add(this.zedGraph_Chart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_ModuleChart3";
            this.Text = "FormModuleChart";
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl zedGraph_Chart;
    }
}