namespace SolveWare_Vision
{
    partial class Form_VisionManager
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_VisionManager));
            this.menu_Motion = new System.Windows.Forms.MenuStrip();
            this.窗口设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.浮动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.还原ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tb_vision_cmd_1 = new System.Windows.Forms.TextBox();
            this.tb_vision_cmd_2 = new System.Windows.Forms.TextBox();
            this.tb_vision_cmd_3 = new System.Windows.Forms.TextBox();
            this.tb_vision_cmd_4 = new System.Windows.Forms.TextBox();
            this.tb_vision_cmd_5 = new System.Windows.Forms.TextBox();
            this.btn_send_vision_cmd_1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_send_vision_cmd_3 = new System.Windows.Forms.Button();
            this.btn_send_vision_cmd_2 = new System.Windows.Forms.Button();
            this.btn_send_vision_cmd_4 = new System.Windows.Forms.Button();
            this.btn_send_vision_cmd_5 = new System.Windows.Forms.Button();
            this.rtb_cmd_result = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_clear_cmd_result = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.menu_Motion.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menu_Motion
            // 
            this.menu_Motion.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.窗口设置ToolStripMenuItem});
            this.menu_Motion.Location = new System.Drawing.Point(0, 0);
            this.menu_Motion.Name = "menu_Motion";
            this.menu_Motion.Size = new System.Drawing.Size(1180, 25);
            this.menu_Motion.TabIndex = 1;
            this.menu_Motion.Text = "menuStrip1";
            // 
            // 窗口设置ToolStripMenuItem
            // 
            this.窗口设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.浮动ToolStripMenuItem,
            this.还原ToolStripMenuItem});
            this.窗口设置ToolStripMenuItem.Name = "窗口设置ToolStripMenuItem";
            this.窗口设置ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.窗口设置ToolStripMenuItem.Text = "窗口设置";
            // 
            // 浮动ToolStripMenuItem
            // 
            this.浮动ToolStripMenuItem.Name = "浮动ToolStripMenuItem";
            this.浮动ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.浮动ToolStripMenuItem.Text = "浮动";
            this.浮动ToolStripMenuItem.Click += new System.EventHandler(this.浮动ToolStripMenuItem_Click);
            // 
            // 还原ToolStripMenuItem
            // 
            this.还原ToolStripMenuItem.Name = "还原ToolStripMenuItem";
            this.还原ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.还原ToolStripMenuItem.Text = "还原";
            this.还原ToolStripMenuItem.Click += new System.EventHandler(this.还原ToolStripMenuItem_Click);
            // 
            // tb_vision_cmd_1
            // 
            this.tb_vision_cmd_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_vision_cmd_1.Font = new System.Drawing.Font("宋体", 13F);
            this.tb_vision_cmd_1.Location = new System.Drawing.Point(206, 4);
            this.tb_vision_cmd_1.Name = "tb_vision_cmd_1";
            this.tb_vision_cmd_1.Size = new System.Drawing.Size(615, 27);
            this.tb_vision_cmd_1.TabIndex = 2;
            this.tb_vision_cmd_1.Text = "cmd 1";
            // 
            // tb_vision_cmd_2
            // 
            this.tb_vision_cmd_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_vision_cmd_2.Font = new System.Drawing.Font("宋体", 13F);
            this.tb_vision_cmd_2.Location = new System.Drawing.Point(206, 40);
            this.tb_vision_cmd_2.Name = "tb_vision_cmd_2";
            this.tb_vision_cmd_2.Size = new System.Drawing.Size(615, 27);
            this.tb_vision_cmd_2.TabIndex = 2;
            this.tb_vision_cmd_2.Text = "cmd 2";
            // 
            // tb_vision_cmd_3
            // 
            this.tb_vision_cmd_3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_vision_cmd_3.Font = new System.Drawing.Font("宋体", 13F);
            this.tb_vision_cmd_3.Location = new System.Drawing.Point(206, 76);
            this.tb_vision_cmd_3.Name = "tb_vision_cmd_3";
            this.tb_vision_cmd_3.Size = new System.Drawing.Size(615, 27);
            this.tb_vision_cmd_3.TabIndex = 2;
            this.tb_vision_cmd_3.Text = "cmd 3";
            // 
            // tb_vision_cmd_4
            // 
            this.tb_vision_cmd_4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_vision_cmd_4.Font = new System.Drawing.Font("宋体", 13F);
            this.tb_vision_cmd_4.Location = new System.Drawing.Point(206, 112);
            this.tb_vision_cmd_4.Name = "tb_vision_cmd_4";
            this.tb_vision_cmd_4.Size = new System.Drawing.Size(615, 27);
            this.tb_vision_cmd_4.TabIndex = 2;
            this.tb_vision_cmd_4.Text = "cmd 4";
            // 
            // tb_vision_cmd_5
            // 
            this.tb_vision_cmd_5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_vision_cmd_5.Font = new System.Drawing.Font("宋体", 13F);
            this.tb_vision_cmd_5.Location = new System.Drawing.Point(206, 148);
            this.tb_vision_cmd_5.Name = "tb_vision_cmd_5";
            this.tb_vision_cmd_5.Size = new System.Drawing.Size(615, 27);
            this.tb_vision_cmd_5.TabIndex = 2;
            this.tb_vision_cmd_5.Text = "cmd 5";
            // 
            // btn_send_vision_cmd_1
            // 
            this.btn_send_vision_cmd_1.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_send_vision_cmd_1.Location = new System.Drawing.Point(828, 4);
            this.btn_send_vision_cmd_1.Name = "btn_send_vision_cmd_1";
            this.btn_send_vision_cmd_1.Size = new System.Drawing.Size(75, 29);
            this.btn_send_vision_cmd_1.TabIndex = 3;
            this.btn_send_vision_cmd_1.Text = "发送";
            this.btn_send_vision_cmd_1.UseVisualStyleBackColor = true;
            this.btn_send_vision_cmd_1.Click += new System.EventHandler(this.btn_send_vision_cmd_1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "视觉命令1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.51839F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.48161F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 353F));
            this.tableLayoutPanel1.Controls.Add(this.btn_send_vision_cmd_1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btn_send_vision_cmd_3, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btn_send_vision_cmd_2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btn_send_vision_cmd_4, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.btn_send_vision_cmd_5, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.rtb_cmd_result, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.tb_vision_cmd_1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tb_vision_cmd_2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tb_vision_cmd_3, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tb_vision_cmd_5, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.tb_vision_cmd_4, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1180, 727);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "视觉命令2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "视觉命令3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "视觉命令4";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "视觉命令5";
            // 
            // btn_send_vision_cmd_3
            // 
            this.btn_send_vision_cmd_3.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_send_vision_cmd_3.Location = new System.Drawing.Point(828, 76);
            this.btn_send_vision_cmd_3.Name = "btn_send_vision_cmd_3";
            this.btn_send_vision_cmd_3.Size = new System.Drawing.Size(75, 29);
            this.btn_send_vision_cmd_3.TabIndex = 3;
            this.btn_send_vision_cmd_3.Text = "发送";
            this.btn_send_vision_cmd_3.UseVisualStyleBackColor = true;
            this.btn_send_vision_cmd_3.Click += new System.EventHandler(this.btn_send_vision_cmd_3_Click);
            // 
            // btn_send_vision_cmd_2
            // 
            this.btn_send_vision_cmd_2.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_send_vision_cmd_2.Location = new System.Drawing.Point(828, 40);
            this.btn_send_vision_cmd_2.Name = "btn_send_vision_cmd_2";
            this.btn_send_vision_cmd_2.Size = new System.Drawing.Size(75, 29);
            this.btn_send_vision_cmd_2.TabIndex = 3;
            this.btn_send_vision_cmd_2.Text = "发送";
            this.btn_send_vision_cmd_2.UseVisualStyleBackColor = true;
            this.btn_send_vision_cmd_2.Click += new System.EventHandler(this.btn_send_vision_cmd_2_Click);
            // 
            // btn_send_vision_cmd_4
            // 
            this.btn_send_vision_cmd_4.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_send_vision_cmd_4.Location = new System.Drawing.Point(828, 112);
            this.btn_send_vision_cmd_4.Name = "btn_send_vision_cmd_4";
            this.btn_send_vision_cmd_4.Size = new System.Drawing.Size(75, 29);
            this.btn_send_vision_cmd_4.TabIndex = 3;
            this.btn_send_vision_cmd_4.Text = "发送";
            this.btn_send_vision_cmd_4.UseVisualStyleBackColor = true;
            this.btn_send_vision_cmd_4.Click += new System.EventHandler(this.btn_send_vision_cmd_4_Click);
            // 
            // btn_send_vision_cmd_5
            // 
            this.btn_send_vision_cmd_5.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_send_vision_cmd_5.Location = new System.Drawing.Point(828, 148);
            this.btn_send_vision_cmd_5.Name = "btn_send_vision_cmd_5";
            this.btn_send_vision_cmd_5.Size = new System.Drawing.Size(75, 29);
            this.btn_send_vision_cmd_5.TabIndex = 3;
            this.btn_send_vision_cmd_5.Text = "发送";
            this.btn_send_vision_cmd_5.UseVisualStyleBackColor = true;
            this.btn_send_vision_cmd_5.Click += new System.EventHandler(this.btn_send_vision_cmd_5_Click);
            // 
            // rtb_cmd_result
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.rtb_cmd_result, 2);
            this.rtb_cmd_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_cmd_result.Location = new System.Drawing.Point(206, 184);
            this.rtb_cmd_result.Name = "rtb_cmd_result";
            this.tableLayoutPanel1.SetRowSpan(this.rtb_cmd_result, 2);
            this.rtb_cmd_result.Size = new System.Drawing.Size(970, 539);
            this.rtb_cmd_result.TabIndex = 5;
            this.rtb_cmd_result.Text = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_clear_cmd_result);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 184);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(195, 29);
            this.panel1.TabIndex = 7;
            // 
            // btn_clear_cmd_result
            // 
            this.btn_clear_cmd_result.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_clear_cmd_result.Location = new System.Drawing.Point(120, 0);
            this.btn_clear_cmd_result.Name = "btn_clear_cmd_result";
            this.btn_clear_cmd_result.Size = new System.Drawing.Size(75, 29);
            this.btn_clear_cmd_result.TabIndex = 6;
            this.btn_clear_cmd_result.Text = "清空返回";
            this.btn_clear_cmd_result.UseVisualStyleBackColor = true;
            this.btn_clear_cmd_result.Click += new System.EventHandler(this.btn_clear_cmd_result_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "返回结果";
            // 
            // Form_VisionManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 752);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menu_Motion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_VisionManager";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_MotionManager_FormClosing);
            this.Load += new System.EventHandler(this.Form_MotionManager_Load);
            this.menu_Motion.ResumeLayout(false);
            this.menu_Motion.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menu_Motion;
        private System.Windows.Forms.ToolStripMenuItem 窗口设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 浮动ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 还原ToolStripMenuItem;
        private System.Windows.Forms.TextBox tb_vision_cmd_1;
        private System.Windows.Forms.TextBox tb_vision_cmd_2;
        private System.Windows.Forms.TextBox tb_vision_cmd_3;
        private System.Windows.Forms.TextBox tb_vision_cmd_4;
        private System.Windows.Forms.TextBox tb_vision_cmd_5;
        private System.Windows.Forms.Button btn_send_vision_cmd_1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_send_vision_cmd_3;
        private System.Windows.Forms.Button btn_send_vision_cmd_2;
        private System.Windows.Forms.Button btn_send_vision_cmd_4;
        private System.Windows.Forms.Button btn_send_vision_cmd_5;
        private System.Windows.Forms.RichTextBox rtb_cmd_result;
        private System.Windows.Forms.Button btn_clear_cmd_result;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
    }
}

