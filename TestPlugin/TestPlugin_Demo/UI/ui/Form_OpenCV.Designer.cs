
namespace TestPlugin_Demo
{
    partial class Form_OpenCV
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.窗口浮动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.窗口停靠ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.置顶ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label42 = new System.Windows.Forms.Label();
            this.ExposureTimeMinValue = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.ExposureTimeMaxValue = new System.Windows.Forms.Label();
            this.ExposureTimeNowValue = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnStopSnap = new System.Windows.Forms.Button();
            this.btnContinues = new System.Windows.Forms.Button();
            this.btnOneshot = new System.Windows.Forms.Button();
            this.hScrollBarExposureTime = new System.Windows.Forms.HScrollBar();
            this.pictureBoxCamera = new System.Windows.Forms.PictureBox();
            this.取消置顶ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCamera)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.窗口浮动ToolStripMenuItem,
            this.窗口停靠ToolStripMenuItem,
            this.置顶ToolStripMenuItem,
            this.取消置顶ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(638, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 窗口浮动ToolStripMenuItem
            // 
            this.窗口浮动ToolStripMenuItem.Name = "窗口浮动ToolStripMenuItem";
            this.窗口浮动ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.窗口浮动ToolStripMenuItem.Text = "窗口浮动";
            this.窗口浮动ToolStripMenuItem.Click += new System.EventHandler(this.窗口浮动ToolStripMenuItem_Click);
            // 
            // 窗口停靠ToolStripMenuItem
            // 
            this.窗口停靠ToolStripMenuItem.Name = "窗口停靠ToolStripMenuItem";
            this.窗口停靠ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.窗口停靠ToolStripMenuItem.Text = "窗口停靠";
            this.窗口停靠ToolStripMenuItem.Click += new System.EventHandler(this.窗口停靠ToolStripMenuItem_Click);
            // 
            // 置顶ToolStripMenuItem
            // 
            this.置顶ToolStripMenuItem.Name = "置顶ToolStripMenuItem";
            this.置顶ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.置顶ToolStripMenuItem.Text = "置顶";
            this.置顶ToolStripMenuItem.Click += new System.EventHandler(this.置顶ToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pictureBoxCamera, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 63F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.38438F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(638, 653);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.189435F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 89.79964F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.010929F));
            this.tableLayoutPanel3.Controls.Add(this.panel5, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.panel6, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.hScrollBarExposureTime, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 429);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(632, 221);
            this.tableLayoutPanel3.TabIndex = 23;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label42);
            this.panel5.Controls.Add(this.ExposureTimeMinValue);
            this.panel5.Controls.Add(this.label46);
            this.panel5.Controls.Add(this.ExposureTimeMaxValue);
            this.panel5.Controls.Add(this.ExposureTimeNowValue);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(26, 0);
            this.panel5.Margin = new System.Windows.Forms.Padding(0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(567, 40);
            this.panel5.TabIndex = 0;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(56, 11);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(53, 12);
            this.label42.TabIndex = 5;
            this.label42.Text = "曝光时间";
            // 
            // ExposureTimeMinValue
            // 
            this.ExposureTimeMinValue.AutoSize = true;
            this.ExposureTimeMinValue.Location = new System.Drawing.Point(147, 11);
            this.ExposureTimeMinValue.Name = "ExposureTimeMinValue";
            this.ExposureTimeMinValue.Size = new System.Drawing.Size(23, 12);
            this.ExposureTimeMinValue.TabIndex = 9;
            this.ExposureTimeMinValue.Text = "-10";
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(307, 11);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(77, 12);
            this.label46.TabIndex = 15;
            this.label46.Text = "ExposureTime";
            // 
            // ExposureTimeMaxValue
            // 
            this.ExposureTimeMaxValue.AutoSize = true;
            this.ExposureTimeMaxValue.Location = new System.Drawing.Point(180, 11);
            this.ExposureTimeMaxValue.Name = "ExposureTimeMaxValue";
            this.ExposureTimeMaxValue.Size = new System.Drawing.Size(17, 12);
            this.ExposureTimeMaxValue.TabIndex = 11;
            this.ExposureTimeMaxValue.Text = "10";
            // 
            // ExposureTimeNowValue
            // 
            this.ExposureTimeNowValue.AutoSize = true;
            this.ExposureTimeNowValue.Location = new System.Drawing.Point(443, 11);
            this.ExposureTimeNowValue.Name = "ExposureTimeNowValue";
            this.ExposureTimeNowValue.Size = new System.Drawing.Size(11, 12);
            this.ExposureTimeNowValue.TabIndex = 13;
            this.ExposureTimeNowValue.Text = "0";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnStopSnap);
            this.panel6.Controls.Add(this.btnContinues);
            this.panel6.Controls.Add(this.btnOneshot);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(26, 60);
            this.panel6.Margin = new System.Windows.Forms.Padding(0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(567, 161);
            this.panel6.TabIndex = 8;
            // 
            // btnStopSnap
            // 
            this.btnStopSnap.Location = new System.Drawing.Point(357, 48);
            this.btnStopSnap.Name = "btnStopSnap";
            this.btnStopSnap.Size = new System.Drawing.Size(104, 44);
            this.btnStopSnap.TabIndex = 3;
            this.btnStopSnap.Text = "停止采集";
            this.btnStopSnap.UseVisualStyleBackColor = true;
            this.btnStopSnap.Click += new System.EventHandler(this.btnStopSnap_Click);
            // 
            // btnContinues
            // 
            this.btnContinues.Location = new System.Drawing.Point(212, 48);
            this.btnContinues.Name = "btnContinues";
            this.btnContinues.Size = new System.Drawing.Size(104, 44);
            this.btnContinues.TabIndex = 2;
            this.btnContinues.Text = "保存照片";
            this.btnContinues.UseVisualStyleBackColor = true;
            this.btnContinues.Click += new System.EventHandler(this.btnContinues_Click);
            // 
            // btnOneshot
            // 
            this.btnOneshot.Location = new System.Drawing.Point(67, 48);
            this.btnOneshot.Name = "btnOneshot";
            this.btnOneshot.Size = new System.Drawing.Size(104, 44);
            this.btnOneshot.TabIndex = 1;
            this.btnOneshot.Text = "打开相机";
            this.btnOneshot.UseVisualStyleBackColor = true;
            this.btnOneshot.Click += new System.EventHandler(this.btnOneshot_Click);
            // 
            // hScrollBarExposureTime
            // 
            this.hScrollBarExposureTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hScrollBarExposureTime.Location = new System.Drawing.Point(26, 40);
            this.hScrollBarExposureTime.Name = "hScrollBarExposureTime";
            this.hScrollBarExposureTime.Size = new System.Drawing.Size(567, 20);
            this.hScrollBarExposureTime.TabIndex = 9;
            this.hScrollBarExposureTime.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarExposureTime_Scroll);
            // 
            // pictureBoxCamera
            // 
            this.pictureBoxCamera.BackColor = System.Drawing.Color.White;
            this.pictureBoxCamera.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxCamera.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxCamera.Name = "pictureBoxCamera";
            this.pictureBoxCamera.Size = new System.Drawing.Size(632, 420);
            this.pictureBoxCamera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCamera.TabIndex = 22;
            this.pictureBoxCamera.TabStop = false;
            // 
            // 取消置顶ToolStripMenuItem
            // 
            this.取消置顶ToolStripMenuItem.Name = "取消置顶ToolStripMenuItem";
            this.取消置顶ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.取消置顶ToolStripMenuItem.Text = "取消置顶";
            this.取消置顶ToolStripMenuItem.Click += new System.EventHandler(this.取消置顶ToolStripMenuItem_Click);
            // 
            // Form_OpenCV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 678);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form_OpenCV";
            this.Text = "Form_OpenCV";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_OpenCV_FormClosing);
            this.Load += new System.EventHandler(this.Form_OpenCV_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCamera)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 窗口浮动ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 窗口停靠ToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBoxCamera;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label ExposureTimeMinValue;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label ExposureTimeMaxValue;
        private System.Windows.Forms.Label ExposureTimeNowValue;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnStopSnap;
        private System.Windows.Forms.Button btnContinues;
        private System.Windows.Forms.Button btnOneshot;
        private System.Windows.Forms.HScrollBar hScrollBarExposureTime;
        private System.Windows.Forms.ToolStripMenuItem 置顶ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 取消置顶ToolStripMenuItem;
    }
}