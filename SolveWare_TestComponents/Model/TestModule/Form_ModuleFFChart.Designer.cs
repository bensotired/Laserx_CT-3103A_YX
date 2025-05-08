
namespace SolveWare_TestComponents.Model
{
    partial class Form_ModuleFFChart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ModuleFFChart));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.optical_Beam_2nd = new LX_2DChart.Optical_Beam_Reconstruction();
            this.optical_Beam_1st = new LX_2DChart.Optical_Beam_Reconstruction();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.optical_Beam_2nd, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.optical_Beam_1st, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // optical_Beam_2nd
            // 
            //this.optical_Beam_2nd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("optical_Beam_2nd.BackgroundImage")));
            this.optical_Beam_2nd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.optical_Beam_2nd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optical_Beam_2nd.Location = new System.Drawing.Point(404, 4);
            this.optical_Beam_2nd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.optical_Beam_2nd.Name = "optical_Beam_2nd";
            this.optical_Beam_2nd.Size = new System.Drawing.Size(392, 442);
            this.optical_Beam_2nd.TabIndex = 1;
            // 
            // optical_Beam_1st
            // 
            //this.optical_Beam_1st.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("optical_Beam_1st.BackgroundImage")));//这个放在哪？
            this.optical_Beam_1st.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.optical_Beam_1st.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optical_Beam_1st.Location = new System.Drawing.Point(4, 4);
            this.optical_Beam_1st.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.optical_Beam_1st.Name = "optical_Beam_1st";
            this.optical_Beam_1st.Size = new System.Drawing.Size(392, 442);
            this.optical_Beam_1st.TabIndex = 0;
            // 
            // Form_ModuleFFChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_ModuleFFChart";
            this.Text = "Form_ModuleFFChart";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private LX_2DChart.Optical_Beam_Reconstruction optical_Beam_2nd;
        private LX_2DChart.Optical_Beam_Reconstruction optical_Beam_1st;
    }
}