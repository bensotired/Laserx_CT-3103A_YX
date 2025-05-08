
namespace SolveWare_TesterCore 
{
    partial class Form_TestExecutorCombo_InstrumentEditor_SimpleMode
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("节点0");
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_TestExecutorCombo_InstrumentEditor_SimpleMode));
            this.cms_ExecutorComboTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.复制模块ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_windowSetting = new System.Windows.Forms.MenuStrip();
            this.窗口设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.浮动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.还原ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_editComboParam = new System.Windows.Forms.Button();
            this.btn_SaveCRCombo = new System.Windows.Forms.Button();
            this.btn_AddNewTestRcpToCombo = new System.Windows.Forms.Button();
            this.btn_AddAxisToCombo = new System.Windows.Forms.Button();
            this.btn_AddPositionToCombo = new System.Windows.Forms.Button();
            this.treeView_editExecutorCombo = new System.Windows.Forms.TreeView();
            this.lv_StepComboProfileFiles = new System.Windows.Forms.ListView();
            this.cms_listView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.dgv_supportedInstruments = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_instType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.tp_resourceSelector = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgv_supportedAxes = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgv_supportedAxesPosition = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cms_ExecutorComboTreeView.SuspendLayout();
            this.menu_windowSetting.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.cms_listView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_supportedInstruments)).BeginInit();
            this.tableLayoutPanel6.SuspendLayout();
            this.tp_resourceSelector.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_supportedAxes)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_supportedAxesPosition)).BeginInit();
            this.SuspendLayout();
            // 
            // cms_ExecutorComboTreeView
            // 
            this.cms_ExecutorComboTreeView.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cms_ExecutorComboTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.复制模块ToolStripMenuItem});
            this.cms_ExecutorComboTreeView.Name = "contextMenuStrip1";
            this.cms_ExecutorComboTreeView.Size = new System.Drawing.Size(125, 48);
            this.cms_ExecutorComboTreeView.Opening += new System.ComponentModel.CancelEventHandler(this.cms_ExecutorComboTreeView_Opening);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem1.Text = "单项调试";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // 复制模块ToolStripMenuItem
            // 
            this.复制模块ToolStripMenuItem.Name = "复制模块ToolStripMenuItem";
            this.复制模块ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.复制模块ToolStripMenuItem.Text = "复制模块";
            this.复制模块ToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItem1_Copy_Click);
            // 
            // menu_windowSetting
            // 
            this.menu_windowSetting.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menu_windowSetting.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.窗口设置ToolStripMenuItem});
            this.menu_windowSetting.Location = new System.Drawing.Point(0, 0);
            this.menu_windowSetting.Name = "menu_windowSetting";
            this.menu_windowSetting.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menu_windowSetting.Size = new System.Drawing.Size(1329, 25);
            this.menu_windowSetting.TabIndex = 4;
            this.menu_windowSetting.Text = "menuStrip1";
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
            // 
            // 还原ToolStripMenuItem
            // 
            this.还原ToolStripMenuItem.Name = "还原ToolStripMenuItem";
            this.还原ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.还原ToolStripMenuItem.Text = "还原";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 10;
            this.tableLayoutPanel6.SetColumnSpan(this.tableLayoutPanel1, 3);
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.Controls.Add(this.btn_editComboParam, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btn_SaveCRCombo, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.btn_AddNewTestRcpToCombo, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btn_AddAxisToCombo, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btn_AddPositionToCombo, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 675);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1321, 69);
            this.tableLayoutPanel1.TabIndex = 15;
            // 
            // btn_editComboParam
            // 
            this.btn_editComboParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_editComboParam.Font = new System.Drawing.Font("宋体", 10F);
            this.btn_editComboParam.Location = new System.Drawing.Point(399, 3);
            this.btn_editComboParam.Name = "btn_editComboParam";
            this.btn_editComboParam.Size = new System.Drawing.Size(126, 63);
            this.btn_editComboParam.TabIndex = 3;
            this.btn_editComboParam.Text = "编辑链表参数";
            this.btn_editComboParam.UseVisualStyleBackColor = true;
            this.btn_editComboParam.Click += new System.EventHandler(this.btn_editComboParam_Click);
            // 
            // btn_SaveCRCombo
            // 
            this.btn_SaveCRCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_SaveCRCombo.Font = new System.Drawing.Font("宋体", 10F);
            this.btn_SaveCRCombo.Location = new System.Drawing.Point(927, 3);
            this.btn_SaveCRCombo.Name = "btn_SaveCRCombo";
            this.btn_SaveCRCombo.Size = new System.Drawing.Size(126, 63);
            this.btn_SaveCRCombo.TabIndex = 1;
            this.btn_SaveCRCombo.Text = "保存链表";
            this.btn_SaveCRCombo.UseVisualStyleBackColor = true;
            this.btn_SaveCRCombo.Click += new System.EventHandler(this.btn_SaveCRCombo_Click);
            // 
            // btn_AddNewTestRcpToCombo
            // 
            this.btn_AddNewTestRcpToCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_AddNewTestRcpToCombo.Font = new System.Drawing.Font("宋体", 10F);
            this.btn_AddNewTestRcpToCombo.Location = new System.Drawing.Point(3, 3);
            this.btn_AddNewTestRcpToCombo.Name = "btn_AddNewTestRcpToCombo";
            this.btn_AddNewTestRcpToCombo.Size = new System.Drawing.Size(126, 63);
            this.btn_AddNewTestRcpToCombo.TabIndex = 2;
            this.btn_AddNewTestRcpToCombo.Text = "更新仪器配置";
            this.btn_AddNewTestRcpToCombo.UseVisualStyleBackColor = true;
            this.btn_AddNewTestRcpToCombo.Click += new System.EventHandler(this.btn_AddNewTestRcpToCombo_Click);
            // 
            // btn_AddAxisToCombo
            // 
            this.btn_AddAxisToCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_AddAxisToCombo.Font = new System.Drawing.Font("宋体", 10F);
            this.btn_AddAxisToCombo.Location = new System.Drawing.Point(135, 3);
            this.btn_AddAxisToCombo.Name = "btn_AddAxisToCombo";
            this.btn_AddAxisToCombo.Size = new System.Drawing.Size(126, 63);
            this.btn_AddAxisToCombo.TabIndex = 4;
            this.btn_AddAxisToCombo.Text = "更新轴配置";
            this.btn_AddAxisToCombo.UseVisualStyleBackColor = true;
            this.btn_AddAxisToCombo.Click += new System.EventHandler(this.btn_AddAxisToCombo_Click);
            // 
            // btn_AddPositionToCombo
            // 
            this.btn_AddPositionToCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_AddPositionToCombo.Font = new System.Drawing.Font("宋体", 10F);
            this.btn_AddPositionToCombo.Location = new System.Drawing.Point(267, 3);
            this.btn_AddPositionToCombo.Name = "btn_AddPositionToCombo";
            this.btn_AddPositionToCombo.Size = new System.Drawing.Size(126, 63);
            this.btn_AddPositionToCombo.TabIndex = 5;
            this.btn_AddPositionToCombo.Text = "更新位置配置";
            this.btn_AddPositionToCombo.UseVisualStyleBackColor = true;
            this.btn_AddPositionToCombo.Click += new System.EventHandler(this.btn_AddPositionToCombo_Click);
            // 
            // treeView_editExecutorCombo
            // 
            this.treeView_editExecutorCombo.ContextMenuStrip = this.cms_ExecutorComboTreeView;
            this.treeView_editExecutorCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_editExecutorCombo.Location = new System.Drawing.Point(402, 4);
            this.treeView_editExecutorCombo.Name = "treeView_editExecutorCombo";
            treeNode1.Name = "节点0";
            treeNode1.Text = "节点0";
            this.treeView_editExecutorCombo.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeView_editExecutorCombo.Size = new System.Drawing.Size(524, 664);
            this.treeView_editExecutorCombo.TabIndex = 12;
            // 
            // lv_StepComboProfileFiles
            // 
            this.lv_StepComboProfileFiles.ContextMenuStrip = this.cms_listView;
            this.lv_StepComboProfileFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_StepComboProfileFiles.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lv_StepComboProfileFiles.GridLines = true;
            this.lv_StepComboProfileFiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lv_StepComboProfileFiles.HideSelection = false;
            this.lv_StepComboProfileFiles.Location = new System.Drawing.Point(933, 4);
            this.lv_StepComboProfileFiles.MultiSelect = false;
            this.lv_StepComboProfileFiles.Name = "lv_StepComboProfileFiles";
            this.lv_StepComboProfileFiles.Size = new System.Drawing.Size(392, 664);
            this.lv_StepComboProfileFiles.TabIndex = 13;
            this.lv_StepComboProfileFiles.UseCompatibleStateImageBehavior = false;
            this.lv_StepComboProfileFiles.View = System.Windows.Forms.View.Details;
            this.lv_StepComboProfileFiles.SelectedIndexChanged += new System.EventHandler(this.lv_StepComboProfileFiles_SelectedIndexChanged);
            // 
            // cms_listView
            // 
            this.cms_listView.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cms_listView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.cms_listView.Name = "cms_listView";
            this.cms_listView.Size = new System.Drawing.Size(101, 26);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem2.Text = "删除";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // dgv_supportedInstruments
            // 
            this.dgv_supportedInstruments.AllowUserToAddRows = false;
            this.dgv_supportedInstruments.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_supportedInstruments.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_supportedInstruments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_supportedInstruments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.col_instType});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_supportedInstruments.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_supportedInstruments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_supportedInstruments.Location = new System.Drawing.Point(3, 3);
            this.dgv_supportedInstruments.MultiSelect = false;
            this.dgv_supportedInstruments.Name = "dgv_supportedInstruments";
            this.dgv_supportedInstruments.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_supportedInstruments.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_supportedInstruments.RowHeadersVisible = false;
            this.dgv_supportedInstruments.RowHeadersWidth = 51;
            this.dgv_supportedInstruments.RowTemplate.Height = 23;
            this.dgv_supportedInstruments.Size = new System.Drawing.Size(377, 632);
            this.dgv_supportedInstruments.TabIndex = 4;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.FillWeight = 50F;
            this.dataGridViewTextBoxColumn5.HeaderText = "可选仪器";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // col_instType
            // 
            this.col_instType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_instType.FillWeight = 50F;
            this.col_instType.HeaderText = "类型";
            this.col_instType.MinimumWidth = 6;
            this.col_instType.Name = "col_instType";
            this.col_instType.ReadOnly = true;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel6.ColumnCount = 3;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel6.Controls.Add(this.lv_StepComboProfileFiles, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.treeView_editExecutorCombo, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.tp_resourceSelector, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(1329, 748);
            this.tableLayoutPanel6.TabIndex = 2;
            // 
            // tp_resourceSelector
            // 
            this.tp_resourceSelector.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tp_resourceSelector.Controls.Add(this.tabPage3);
            this.tp_resourceSelector.Controls.Add(this.tabPage1);
            this.tp_resourceSelector.Controls.Add(this.tabPage2);
            this.tp_resourceSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tp_resourceSelector.Location = new System.Drawing.Point(4, 4);
            this.tp_resourceSelector.Multiline = true;
            this.tp_resourceSelector.Name = "tp_resourceSelector";
            this.tp_resourceSelector.SelectedIndex = 0;
            this.tp_resourceSelector.Size = new System.Drawing.Size(391, 664);
            this.tp_resourceSelector.TabIndex = 16;
            this.tp_resourceSelector.SelectedIndexChanged += new System.EventHandler(this.tp_resourceSelector_SelectedIndexChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dgv_supportedInstruments);
            this.tabPage3.Location = new System.Drawing.Point(4, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage3.Size = new System.Drawing.Size(383, 638);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "仪器与外设";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgv_supportedAxes);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage1.Size = new System.Drawing.Size(384, 639);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "轴";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgv_supportedAxes
            // 
            this.dgv_supportedAxes.AllowUserToAddRows = false;
            this.dgv_supportedAxes.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_supportedAxes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgv_supportedAxes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_supportedAxes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_supportedAxes.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgv_supportedAxes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_supportedAxes.Location = new System.Drawing.Point(3, 3);
            this.dgv_supportedAxes.MultiSelect = false;
            this.dgv_supportedAxes.Name = "dgv_supportedAxes";
            this.dgv_supportedAxes.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_supportedAxes.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgv_supportedAxes.RowHeadersVisible = false;
            this.dgv_supportedAxes.RowHeadersWidth = 51;
            this.dgv_supportedAxes.RowTemplate.Height = 23;
            this.dgv_supportedAxes.Size = new System.Drawing.Size(378, 633);
            this.dgv_supportedAxes.TabIndex = 5;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.FillWeight = 50F;
            this.dataGridViewTextBoxColumn1.HeaderText = "可选轴";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgv_supportedAxesPosition);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage2.Size = new System.Drawing.Size(384, 639);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "运动位置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgv_supportedAxesPosition
            // 
            this.dgv_supportedAxesPosition.AllowUserToAddRows = false;
            this.dgv_supportedAxesPosition.AllowUserToDeleteRows = false;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_supportedAxesPosition.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgv_supportedAxesPosition.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_supportedAxesPosition.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_supportedAxesPosition.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgv_supportedAxesPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_supportedAxesPosition.Location = new System.Drawing.Point(3, 3);
            this.dgv_supportedAxesPosition.MultiSelect = false;
            this.dgv_supportedAxesPosition.Name = "dgv_supportedAxesPosition";
            this.dgv_supportedAxesPosition.ReadOnly = true;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_supportedAxesPosition.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgv_supportedAxesPosition.RowHeadersVisible = false;
            this.dgv_supportedAxesPosition.RowHeadersWidth = 51;
            this.dgv_supportedAxesPosition.RowTemplate.Height = 23;
            this.dgv_supportedAxesPosition.Size = new System.Drawing.Size(378, 633);
            this.dgv_supportedAxesPosition.TabIndex = 6;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.FillWeight = 50F;
            this.dataGridViewTextBoxColumn3.HeaderText = "可选位置";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // Form_TestExecutorCombo_InstrumentEditor_SimpleMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1329, 773);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel6);
            this.Controls.Add(this.menu_windowSetting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_TestExecutorCombo_InstrumentEditor_SimpleMode";
            this.Text = "Form_CRCombo";
            this.Load += new System.EventHandler(this.Form_CRCombo_Load);
            this.cms_ExecutorComboTreeView.ResumeLayout(false);
            this.menu_windowSetting.ResumeLayout(false);
            this.menu_windowSetting.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.cms_listView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_supportedInstruments)).EndInit();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tp_resourceSelector.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_supportedAxes)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_supportedAxesPosition)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menu_windowSetting;
        private System.Windows.Forms.ToolStripMenuItem 窗口设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 浮动ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 还原ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cms_ExecutorComboTreeView;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.DataGridView dgv_supportedInstruments;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_instType;
        private System.Windows.Forms.ListView lv_StepComboProfileFiles;
        private System.Windows.Forms.TreeView treeView_editExecutorCombo;
        private System.Windows.Forms.Button btn_SaveCRCombo;
        private System.Windows.Forms.Button btn_AddNewTestRcpToCombo;
        private System.Windows.Forms.Button btn_editComboParam;
        private System.Windows.Forms.ContextMenuStrip cms_listView;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.TabControl tp_resourceSelector;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dgv_supportedAxes;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgv_supportedAxesPosition;
        private System.Windows.Forms.Button btn_AddAxisToCombo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.Button btn_AddPositionToCombo;
        private System.Windows.Forms.ToolStripMenuItem 复制模块ToolStripMenuItem;
    }
}