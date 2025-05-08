namespace SolveWare_TestComponents.Model
{
    partial class Form_ExecutorCharts
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
            this.tlp_exeCharts = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // tlp_exeCharts
            // 
            this.tlp_exeCharts.ColumnCount = 2;
            this.tlp_exeCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_exeCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_exeCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_exeCharts.Location = new System.Drawing.Point(0, 0);
            this.tlp_exeCharts.Name = "tlp_exeCharts";
            this.tlp_exeCharts.RowCount = 2;
            this.tlp_exeCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_exeCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_exeCharts.Size = new System.Drawing.Size(800, 566);
            this.tlp_exeCharts.TabIndex = 0;
            // 
            // Form_ExecutorCharts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 566);
            this.Controls.Add(this.tlp_exeCharts);
            this.Name = "Form_ExecutorCharts";
            this.Text = "Form_ExecutorCharts";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlp_exeCharts;
    }
}