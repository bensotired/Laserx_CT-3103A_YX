namespace SolveWare_TesterCore
{
    partial class Form_UniversalTesterMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_UniversalTesterMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsm_User = new System.Windows.Forms.ToolStripMenuItem();
            this.管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.登录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tssl_BD = new System.Windows.Forms.ToolStripMenuItem();
            this.配置参数管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.切换配置参数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.ss_mainInfo = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel11 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_CurrentUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_CurrentProductName = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlp_main = new System.Windows.Forms.TableLayoutPanel();
            this.tab_MainPage = new System.Windows.Forms.TabControl();
            this.tp_StationHardwareSetting = new System.Windows.Forms.TabPage();
            this.tp_TestFrame = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnl_auxi = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabOESGuiTest = new System.Windows.Forms.TabPage();
            this.btnOpenOesForm = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.ss_mainInfo.SuspendLayout();
            this.tlp_main.SuspendLayout();
            this.tab_MainPage.SuspendLayout();
            this.tp_StationHardwareSetting.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsm_User,
            this.tssl_BD,
            this.配置参数管理ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1468, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsm_User
            // 
            this.tsm_User.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.管理ToolStripMenuItem,
            this.登录ToolStripMenuItem});
            this.tsm_User.Name = "tsm_User";
            this.tsm_User.Size = new System.Drawing.Size(44, 20);
            this.tsm_User.Text = "用户";
            // 
            // 管理ToolStripMenuItem
            // 
            this.管理ToolStripMenuItem.Name = "管理ToolStripMenuItem";
            this.管理ToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.管理ToolStripMenuItem.Text = "管理";
            this.管理ToolStripMenuItem.Click += new System.EventHandler(this.管理ToolStripMenuItem_Click);
            // 
            // 登录ToolStripMenuItem
            // 
            this.登录ToolStripMenuItem.Name = "登录ToolStripMenuItem";
            this.登录ToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.登录ToolStripMenuItem.Text = "登录";
            this.登录ToolStripMenuItem.Click += new System.EventHandler(this.登录ToolStripMenuItem_Click);
            // 
            // tssl_BD
            // 
            this.tssl_BD.Name = "tssl_BD";
            this.tssl_BD.Size = new System.Drawing.Size(76, 20);
            this.tssl_BD.Text = "                   ";
            this.tssl_BD.Visible = false;
            // 
            // 配置参数管理ToolStripMenuItem
            // 
            this.配置参数管理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.切换配置参数ToolStripMenuItem,
            this.toolStripMenuItem2});
            this.配置参数管理ToolStripMenuItem.Name = "配置参数管理ToolStripMenuItem";
            this.配置参数管理ToolStripMenuItem.Size = new System.Drawing.Size(91, 20);
            this.配置参数管理ToolStripMenuItem.Text = "配置参数管理";
            // 
            // 切换配置参数ToolStripMenuItem
            // 
            this.切换配置参数ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3});
            this.切换配置参数ToolStripMenuItem.Name = "切换配置参数ToolStripMenuItem";
            this.切换配置参数ToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.切换配置参数ToolStripMenuItem.Text = "切换产品配置参数";
            this.切换配置参数ToolStripMenuItem.DropDownOpening += new System.EventHandler(this.切换配置参数ToolStripMenuItem_DropDownOpening);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(77, 22);
            this.toolStripMenuItem3.Text = " ";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(239, 22);
            this.toolStripMenuItem2.Text = "新建产品配置参数(按当前配置)";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // ss_mainInfo
            // 
            this.ss_mainInfo.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ss_mainInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel11,
            this.tssl_CurrentUser,
            this.toolStripStatusLabel1,
            this.tssl_CurrentProductName});
            this.ss_mainInfo.Location = new System.Drawing.Point(0, 819);
            this.ss_mainInfo.Name = "ss_mainInfo";
            this.ss_mainInfo.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.ss_mainInfo.Size = new System.Drawing.Size(1468, 22);
            this.ss_mainInfo.TabIndex = 3;
            this.ss_mainInfo.Text = "statusStrip1";
            // 
            // toolStripStatusLabel11
            // 
            this.toolStripStatusLabel11.Name = "toolStripStatusLabel11";
            this.toolStripStatusLabel11.Size = new System.Drawing.Size(59, 17);
            this.toolStripStatusLabel11.Text = "当前用户:";
            // 
            // tssl_CurrentUser
            // 
            this.tssl_CurrentUser.Name = "tssl_CurrentUser";
            this.tssl_CurrentUser.Size = new System.Drawing.Size(44, 17);
            this.tssl_CurrentUser.Text = "未登录";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(84, 17);
            this.toolStripStatusLabel1.Text = "当前产品类型:";
            // 
            // tssl_CurrentProductName
            // 
            this.tssl_CurrentProductName.Name = "tssl_CurrentProductName";
            this.tssl_CurrentProductName.Size = new System.Drawing.Size(31, 17);
            this.tssl_CurrentProductName.Text = "未知";
            // 
            // tlp_main
            // 
            this.tlp_main.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlp_main.ColumnCount = 2;
            this.tlp_main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.91008F));
            this.tlp_main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.08992F));
            this.tlp_main.Controls.Add(this.tab_MainPage, 0, 0);
            this.tlp_main.Controls.Add(this.tableLayoutPanel1, 1, 0);
            this.tlp_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_main.Location = new System.Drawing.Point(0, 24);
            this.tlp_main.Margin = new System.Windows.Forms.Padding(0);
            this.tlp_main.Name = "tlp_main";
            this.tlp_main.RowCount = 1;
            this.tlp_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tlp_main.Size = new System.Drawing.Size(1468, 795);
            this.tlp_main.TabIndex = 4;
            // 
            // tab_MainPage
            // 
            this.tab_MainPage.Controls.Add(this.tp_StationHardwareSetting);
            this.tab_MainPage.Controls.Add(this.tp_TestFrame);
            this.tab_MainPage.Controls.Add(this.tabOESGuiTest);
            this.tab_MainPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab_MainPage.Enabled = false;
            this.tab_MainPage.Location = new System.Drawing.Point(4, 4);
            this.tab_MainPage.Multiline = true;
            this.tab_MainPage.Name = "tab_MainPage";
            this.tab_MainPage.SelectedIndex = 0;
            this.tab_MainPage.Size = new System.Drawing.Size(1076, 787);
            this.tab_MainPage.TabIndex = 29;
            this.tab_MainPage.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tab_MainPage_Selecting);
            // 
            // tp_StationHardwareSetting
            // 
            this.tp_StationHardwareSetting.Controls.Add(this.btnOpenOesForm);
            this.tp_StationHardwareSetting.Location = new System.Drawing.Point(4, 22);
            this.tp_StationHardwareSetting.Name = "tp_StationHardwareSetting";
            this.tp_StationHardwareSetting.Padding = new System.Windows.Forms.Padding(3);
            this.tp_StationHardwareSetting.Size = new System.Drawing.Size(1068, 761);
            this.tp_StationHardwareSetting.TabIndex = 1;
            this.tp_StationHardwareSetting.Text = "内外设配置";
            this.tp_StationHardwareSetting.UseVisualStyleBackColor = true;
            // 
            // tp_TestFrame
            // 
            this.tp_TestFrame.Location = new System.Drawing.Point(4, 22);
            this.tp_TestFrame.Name = "tp_TestFrame";
            this.tp_TestFrame.Size = new System.Drawing.Size(1068, 761);
            this.tp_TestFrame.TabIndex = 2;
            this.tp_TestFrame.Text = "测试综合设置";
            this.tp_TestFrame.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnl_auxi, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1087, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(377, 787);
            this.tableLayoutPanel1.TabIndex = 30;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(71, 6);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(70, 5, 70, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(235, 29);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 29;
            this.pictureBox1.TabStop = false;
            // 
            // pnl_auxi
            // 
            this.pnl_auxi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_auxi.Location = new System.Drawing.Point(4, 44);
            this.pnl_auxi.Name = "pnl_auxi";
            this.pnl_auxi.Size = new System.Drawing.Size(369, 739);
            this.pnl_auxi.TabIndex = 30;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tabOESGuiTest
            // 
            this.tabOESGuiTest.Location = new System.Drawing.Point(4, 22);
            this.tabOESGuiTest.Name = "tabOESGuiTest";
            this.tabOESGuiTest.Size = new System.Drawing.Size(1068, 761);
            this.tabOESGuiTest.TabIndex = 3;
            this.tabOESGuiTest.Text = "OES GUI test (temporary)";
            this.tabOESGuiTest.UseVisualStyleBackColor = true;
            // 
            // btnOpenOesForm
            // 
            this.btnOpenOesForm.AutoSize = true;
            this.btnOpenOesForm.Location = new System.Drawing.Point(397, 211);
            this.btnOpenOesForm.Name = "btnOpenOesForm";
            this.btnOpenOesForm.Size = new System.Drawing.Size(91, 23);
            this.btnOpenOesForm.TabIndex = 0;
            this.btnOpenOesForm.Text = "Open OES form";
            this.btnOpenOesForm.UseVisualStyleBackColor = true;
            // 
            // Form_UniversalTesterMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1468, 841);
            this.Controls.Add(this.tlp_main);
            this.Controls.Add(this.ss_mainInfo);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_UniversalTesterMain";
            this.Text = "Form_UniversalTesterMain";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_UniversalTesterMain_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ss_mainInfo.ResumeLayout(false);
            this.ss_mainInfo.PerformLayout();
            this.tlp_main.ResumeLayout(false);
            this.tab_MainPage.ResumeLayout(false);
            this.tp_StationHardwareSetting.ResumeLayout(false);
            this.tp_StationHardwareSetting.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsm_User;
        private System.Windows.Forms.ToolStripMenuItem 管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 登录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tssl_BD;
        private System.Windows.Forms.StatusStrip ss_mainInfo;
        private System.Windows.Forms.ToolStripStatusLabel tssl_CurrentUser;
        private System.Windows.Forms.TableLayoutPanel tlp_main;
        private System.Windows.Forms.TabControl tab_MainPage;
        private System.Windows.Forms.TabPage tp_StationHardwareSetting;
        private System.Windows.Forms.TabPage tp_TestFrame;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel11;
        private System.Windows.Forms.Panel pnl_auxi;
        private System.Windows.Forms.ToolStripMenuItem 配置参数管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 切换配置参数ToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tssl_CurrentProductName;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TabPage tabOESGuiTest;
        private System.Windows.Forms.Button btnOpenOesForm;
    }
}