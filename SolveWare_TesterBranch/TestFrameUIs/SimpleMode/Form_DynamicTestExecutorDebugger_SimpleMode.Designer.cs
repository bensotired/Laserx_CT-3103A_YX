namespace SolveWare_TesterCore 
{
    partial class Form_DynamicTestExecutorDebugger_SimpleMode
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnl_data = new System.Windows.Forms.Panel();
            this.pnl_opera = new System.Windows.Forms.Panel();
            this.chk_enableSaveImage = new System.Windows.Forms.CheckBox();
            this.tb_debugDataPathComment = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_debugInterval_ms = new System.Windows.Forms.TextBox();
            this.tb_debugTimes = new System.Windows.Forms.TextBox();
            this.btn_debugSelectDataPath = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_debugTimes = new System.Windows.Forms.Button();
            this.tb_debugDataPath = new System.Windows.Forms.TextBox();
            this.chk_enableSaveDataDialog = new System.Windows.Forms.CheckBox();
            this.btn_debugStop = new System.Windows.Forms.Button();
            this.btn_debugOnce = new System.Windows.Forms.Button();
            this.lbl_rp = new System.Windows.Forms.Label();
            this.cb_pluginResourceProvider = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tp_debugger = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnl_opera.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tp_debugger.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pnl_data, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnl_opera, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 82.20339F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.79661F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1271, 707);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pnl_data
            // 
            this.pnl_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_data.Location = new System.Drawing.Point(4, 4);
            this.pnl_data.Name = "pnl_data";
            this.pnl_data.Size = new System.Drawing.Size(1263, 572);
            this.pnl_data.TabIndex = 0;
            // 
            // pnl_opera
            // 
            this.pnl_opera.Controls.Add(this.chk_enableSaveImage);
            this.pnl_opera.Controls.Add(this.tb_debugDataPathComment);
            this.pnl_opera.Controls.Add(this.label3);
            this.pnl_opera.Controls.Add(this.label2);
            this.pnl_opera.Controls.Add(this.tb_debugInterval_ms);
            this.pnl_opera.Controls.Add(this.tb_debugTimes);
            this.pnl_opera.Controls.Add(this.btn_debugSelectDataPath);
            this.pnl_opera.Controls.Add(this.label5);
            this.pnl_opera.Controls.Add(this.label1);
            this.pnl_opera.Controls.Add(this.btn_debugTimes);
            this.pnl_opera.Controls.Add(this.tb_debugDataPath);
            this.pnl_opera.Controls.Add(this.chk_enableSaveDataDialog);
            this.pnl_opera.Controls.Add(this.btn_debugStop);
            this.pnl_opera.Controls.Add(this.btn_debugOnce);
            this.pnl_opera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_opera.Location = new System.Drawing.Point(4, 583);
            this.pnl_opera.Name = "pnl_opera";
            this.pnl_opera.Size = new System.Drawing.Size(1263, 120);
            this.pnl_opera.TabIndex = 1;
            // 
            // chk_enableSaveImage
            // 
            this.chk_enableSaveImage.AutoSize = true;
            this.chk_enableSaveImage.Location = new System.Drawing.Point(460, 53);
            this.chk_enableSaveImage.Name = "chk_enableSaveImage";
            this.chk_enableSaveImage.Size = new System.Drawing.Size(72, 16);
            this.chk_enableSaveImage.TabIndex = 12;
            this.chk_enableSaveImage.Text = "保存图片";
            this.chk_enableSaveImage.UseVisualStyleBackColor = true;
            this.chk_enableSaveImage.Visible = false;
            // 
            // tb_debugDataPathComment
            // 
            this.tb_debugDataPathComment.Location = new System.Drawing.Point(97, 25);
            this.tb_debugDataPathComment.Name = "tb_debugDataPathComment";
            this.tb_debugDataPathComment.Size = new System.Drawing.Size(518, 21);
            this.tb_debugDataPathComment.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(694, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "运行间隔时间(ms)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(694, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "运行次数";
            // 
            // tb_debugInterval_ms
            // 
            this.tb_debugInterval_ms.Location = new System.Drawing.Point(696, 74);
            this.tb_debugInterval_ms.Name = "tb_debugInterval_ms";
            this.tb_debugInterval_ms.Size = new System.Drawing.Size(100, 21);
            this.tb_debugInterval_ms.TabIndex = 9;
            this.tb_debugInterval_ms.Text = "200";
            // 
            // tb_debugTimes
            // 
            this.tb_debugTimes.Location = new System.Drawing.Point(696, 25);
            this.tb_debugTimes.Name = "tb_debugTimes";
            this.tb_debugTimes.Size = new System.Drawing.Size(100, 21);
            this.tb_debugTimes.TabIndex = 9;
            this.tb_debugTimes.Text = "10";
            // 
            // btn_debugSelectDataPath
            // 
            this.btn_debugSelectDataPath.Location = new System.Drawing.Point(621, 72);
            this.btn_debugSelectDataPath.Name = "btn_debugSelectDataPath";
            this.btn_debugSelectDataPath.Size = new System.Drawing.Size(31, 23);
            this.btn_debugSelectDataPath.TabIndex = 8;
            this.btn_debugSelectDataPath.Text = "...";
            this.btn_debugSelectDataPath.UseVisualStyleBackColor = true;
            this.btn_debugSelectDataPath.Click += new System.EventHandler(this.btn_debugSelectDataPath_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "文件名备注";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "默认存储路径";
            // 
            // btn_debugTimes
            // 
            this.btn_debugTimes.Location = new System.Drawing.Point(822, 18);
            this.btn_debugTimes.Name = "btn_debugTimes";
            this.btn_debugTimes.Size = new System.Drawing.Size(167, 32);
            this.btn_debugTimes.TabIndex = 6;
            this.btn_debugTimes.Text = "按次数运行";
            this.btn_debugTimes.UseVisualStyleBackColor = true;
            this.btn_debugTimes.Click += new System.EventHandler(this.btn_debugTimes_Click);
            // 
            // tb_debugDataPath
            // 
            this.tb_debugDataPath.Location = new System.Drawing.Point(97, 72);
            this.tb_debugDataPath.Name = "tb_debugDataPath";
            this.tb_debugDataPath.Size = new System.Drawing.Size(518, 21);
            this.tb_debugDataPath.TabIndex = 5;
            // 
            // chk_enableSaveDataDialog
            // 
            this.chk_enableSaveDataDialog.AutoSize = true;
            this.chk_enableSaveDataDialog.Location = new System.Drawing.Point(543, 53);
            this.chk_enableSaveDataDialog.Name = "chk_enableSaveDataDialog";
            this.chk_enableSaveDataDialog.Size = new System.Drawing.Size(72, 16);
            this.chk_enableSaveDataDialog.TabIndex = 4;
            this.chk_enableSaveDataDialog.Text = "保存数据";
            this.chk_enableSaveDataDialog.UseVisualStyleBackColor = true;
            // 
            // btn_debugStop
            // 
            this.btn_debugStop.Location = new System.Drawing.Point(1032, 67);
            this.btn_debugStop.Name = "btn_debugStop";
            this.btn_debugStop.Size = new System.Drawing.Size(167, 32);
            this.btn_debugStop.TabIndex = 0;
            this.btn_debugStop.Text = "停止运行";
            this.btn_debugStop.UseVisualStyleBackColor = true;
            this.btn_debugStop.Click += new System.EventHandler(this.btn_debugStop_Click);
            // 
            // btn_debugOnce
            // 
            this.btn_debugOnce.Location = new System.Drawing.Point(1032, 18);
            this.btn_debugOnce.Name = "btn_debugOnce";
            this.btn_debugOnce.Size = new System.Drawing.Size(167, 32);
            this.btn_debugOnce.TabIndex = 0;
            this.btn_debugOnce.Text = "运行单次";
            this.btn_debugOnce.UseVisualStyleBackColor = true;
            this.btn_debugOnce.Click += new System.EventHandler(this.btn_debugOnce_Click);
            // 
            // lbl_rp
            // 
            this.lbl_rp.AutoSize = true;
            this.lbl_rp.Location = new System.Drawing.Point(31, 18);
            this.lbl_rp.Name = "lbl_rp";
            this.lbl_rp.Size = new System.Drawing.Size(113, 12);
            this.lbl_rp.TabIndex = 3;
            this.lbl_rp.Text = "选择测试组件资源池";
            // 
            // cb_pluginResourceProvider
            // 
            this.cb_pluginResourceProvider.FormattingEnabled = true;
            this.cb_pluginResourceProvider.Location = new System.Drawing.Point(33, 33);
            this.cb_pluginResourceProvider.Name = "cb_pluginResourceProvider";
            this.cb_pluginResourceProvider.Size = new System.Drawing.Size(314, 20);
            this.cb_pluginResourceProvider.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tp_debugger);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 85);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1285, 739);
            this.tabControl1.TabIndex = 1;
            // 
            // tp_debugger
            // 
            this.tp_debugger.Controls.Add(this.tableLayoutPanel1);
            this.tp_debugger.Location = new System.Drawing.Point(4, 22);
            this.tp_debugger.Name = "tp_debugger";
            this.tp_debugger.Padding = new System.Windows.Forms.Padding(3);
            this.tp_debugger.Size = new System.Drawing.Size(1277, 713);
            this.tp_debugger.TabIndex = 0;
            this.tp_debugger.Text = "单项调试";
            this.tp_debugger.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1291, 827);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cb_pluginResourceProvider);
            this.panel2.Controls.Add(this.lbl_rp);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1285, 76);
            this.panel2.TabIndex = 2;
            // 
            // Form_DynamicTestExecutorDebugger_SimpleMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1291, 827);
            this.Controls.Add(this.tableLayoutPanel3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_DynamicTestExecutorDebugger_SimpleMode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form_TestExecutorDebugger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_TestExecutorDebugger_FormClosing);
            this.Load += new System.EventHandler(this.Form_TestExecutorDebugger_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnl_opera.ResumeLayout(false);
            this.pnl_opera.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tp_debugger.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnl_data;
        private System.Windows.Forms.Panel pnl_opera;
        private System.Windows.Forms.Button btn_debugOnce;
        private System.Windows.Forms.Label lbl_rp;
        private System.Windows.Forms.ComboBox cb_pluginResourceProvider;
        private System.Windows.Forms.CheckBox chk_enableSaveDataDialog;
        private System.Windows.Forms.Button btn_debugStop;
        private System.Windows.Forms.Button btn_debugTimes;
        private System.Windows.Forms.TextBox tb_debugDataPath;
        private System.Windows.Forms.Button btn_debugSelectDataPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_debugTimes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_debugInterval_ms;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tp_debugger;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox tb_debugDataPathComment;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chk_enableSaveImage;
    }
}