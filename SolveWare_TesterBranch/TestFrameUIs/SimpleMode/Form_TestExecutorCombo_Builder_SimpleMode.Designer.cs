
namespace SolveWare_TesterCore 
{
    partial class Form_TestExecutorCombo_Builder_SimpleMode
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("节点0");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_TestExecutorCombo_Builder_SimpleMode));
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgv_localExecutors = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cms_previewTestModuleTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_SaveStepCombo = new System.Windows.Forms.Button();
            this.btn_AddNewStepToMainCombo = new System.Windows.Forms.Button();
            this.btn_MoveDownStepToCombo = new System.Windows.Forms.Button();
            this.btn_CreateNewStepCombo = new System.Windows.Forms.Button();
            this.btn_MoveUpStepToCombo = new System.Windows.Forms.Button();
            this.btn_AddNewStepToPreCombo = new System.Windows.Forms.Button();
            this.btn_AddNewStepToPostCombo = new System.Windows.Forms.Button();
            this.treeView_TestExecutorCombo = new System.Windows.Forms.TreeView();
            this.cms_ExecutorComboTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsm_deleteTestModuleFromCombo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsm_deleteCalculatorFromTestModule = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsm_InsertCalculatorIntoTestModule = new System.Windows.Forms.ToolStripMenuItem();
            this.lv_StepComboFiles = new System.Windows.Forms.ListView();
            this.cms_listView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_windowSetting = new System.Windows.Forms.MenuStrip();
            this.窗口设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.浮动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.还原ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_localExecutors)).BeginInit();
            this.cms_previewTestModuleTreeView.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.cms_ExecutorComboTreeView.SuspendLayout();
            this.cms_listView.SuspendLayout();
            this.menu_windowSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.treeView_TestExecutorCombo, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.lv_StepComboFiles, 2, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 30);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1512, 741);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dgv_localExecutors, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 5);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 655F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(444, 656);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // dgv_localExecutors
            // 
            this.dgv_localExecutors.AllowUserToAddRows = false;
            this.dgv_localExecutors.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_localExecutors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_localExecutors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_localExecutors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.dgv_localExecutors.ContextMenuStrip = this.cms_previewTestModuleTreeView;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_localExecutors.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_localExecutors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_localExecutors.Location = new System.Drawing.Point(4, 4);
            this.dgv_localExecutors.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgv_localExecutors.MultiSelect = false;
            this.dgv_localExecutors.Name = "dgv_localExecutors";
            this.dgv_localExecutors.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_localExecutors.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_localExecutors.RowHeadersVisible = false;
            this.dgv_localExecutors.RowHeadersWidth = 51;
            this.dgv_localExecutors.RowTemplate.Height = 23;
            this.dgv_localExecutors.Size = new System.Drawing.Size(436, 648);
            this.dgv_localExecutors.TabIndex = 4;
            this.dgv_localExecutors.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_localExecutors_CellMouseClick);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.HeaderText = "可选测试执行项";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // cms_previewTestModuleTreeView
            // 
            this.cms_previewTestModuleTreeView.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cms_previewTestModuleTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.cms_previewTestModuleTreeView.Name = "cms_previewTestModuleTreeView";
            this.cms_previewTestModuleTreeView.Size = new System.Drawing.Size(139, 28);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(138, 24);
            this.toolStripMenuItem2.Text = "预览内容";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 10;
            this.tableLayoutPanel4.SetColumnSpan(this.tableLayoutPanel3, 3);
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.Controls.Add(this.btn_SaveStepCombo, 8, 0);
            this.tableLayoutPanel3.Controls.Add(this.btn_AddNewStepToMainCombo, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btn_MoveDownStepToCombo, 5, 0);
            this.tableLayoutPanel3.Controls.Add(this.btn_CreateNewStepCombo, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btn_MoveUpStepToCombo, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.btn_AddNewStepToPreCombo, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.btn_AddNewStepToPostCombo, 3, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 670);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1502, 66);
            this.tableLayoutPanel3.TabIndex = 13;
            // 
            // btn_SaveStepCombo
            // 
            this.btn_SaveStepCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_SaveStepCombo.Font = new System.Drawing.Font("宋体", 12F);
            this.btn_SaveStepCombo.Location = new System.Drawing.Point(1200, 0);
            this.btn_SaveStepCombo.Margin = new System.Windows.Forms.Padding(0);
            this.btn_SaveStepCombo.Name = "btn_SaveStepCombo";
            this.btn_SaveStepCombo.Size = new System.Drawing.Size(150, 66);
            this.btn_SaveStepCombo.TabIndex = 1;
            this.btn_SaveStepCombo.Text = "保存链表";
            this.btn_SaveStepCombo.UseVisualStyleBackColor = true;
            this.btn_SaveStepCombo.Click += new System.EventHandler(this.btn_SaveStepCombo_Click);
            // 
            // btn_AddNewStepToMainCombo
            // 
            this.btn_AddNewStepToMainCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_AddNewStepToMainCombo.Font = new System.Drawing.Font("宋体", 12F);
            this.btn_AddNewStepToMainCombo.Location = new System.Drawing.Point(300, 0);
            this.btn_AddNewStepToMainCombo.Margin = new System.Windows.Forms.Padding(0);
            this.btn_AddNewStepToMainCombo.Name = "btn_AddNewStepToMainCombo";
            this.btn_AddNewStepToMainCombo.Size = new System.Drawing.Size(150, 66);
            this.btn_AddNewStepToMainCombo.TabIndex = 0;
            this.btn_AddNewStepToMainCombo.Text = "添加到测试项到[主要]";
            this.btn_AddNewStepToMainCombo.UseVisualStyleBackColor = true;
            this.btn_AddNewStepToMainCombo.Click += new System.EventHandler(this.btn_AddNewStepToMainCombo_Click);
            // 
            // btn_MoveDownStepToCombo
            // 
            this.btn_MoveDownStepToCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_MoveDownStepToCombo.Font = new System.Drawing.Font("宋体", 12F);
            this.btn_MoveDownStepToCombo.Location = new System.Drawing.Point(750, 0);
            this.btn_MoveDownStepToCombo.Margin = new System.Windows.Forms.Padding(0);
            this.btn_MoveDownStepToCombo.Name = "btn_MoveDownStepToCombo";
            this.btn_MoveDownStepToCombo.Size = new System.Drawing.Size(150, 66);
            this.btn_MoveDownStepToCombo.TabIndex = 0;
            this.btn_MoveDownStepToCombo.Text = "下移测试模块";
            this.btn_MoveDownStepToCombo.UseVisualStyleBackColor = true;
            this.btn_MoveDownStepToCombo.Click += new System.EventHandler(this.btn_MoveDownStepToCombo_Click);
            // 
            // btn_CreateNewStepCombo
            // 
            this.btn_CreateNewStepCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_CreateNewStepCombo.Font = new System.Drawing.Font("宋体", 12F);
            this.btn_CreateNewStepCombo.Location = new System.Drawing.Point(0, 0);
            this.btn_CreateNewStepCombo.Margin = new System.Windows.Forms.Padding(0);
            this.btn_CreateNewStepCombo.Name = "btn_CreateNewStepCombo";
            this.btn_CreateNewStepCombo.Size = new System.Drawing.Size(150, 66);
            this.btn_CreateNewStepCombo.TabIndex = 0;
            this.btn_CreateNewStepCombo.Text = "新建链表";
            this.btn_CreateNewStepCombo.UseVisualStyleBackColor = true;
            this.btn_CreateNewStepCombo.Click += new System.EventHandler(this.btn_CreateNewStepCombo_Click);
            // 
            // btn_MoveUpStepToCombo
            // 
            this.btn_MoveUpStepToCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_MoveUpStepToCombo.Font = new System.Drawing.Font("宋体", 12F);
            this.btn_MoveUpStepToCombo.Location = new System.Drawing.Point(600, 0);
            this.btn_MoveUpStepToCombo.Margin = new System.Windows.Forms.Padding(0);
            this.btn_MoveUpStepToCombo.Name = "btn_MoveUpStepToCombo";
            this.btn_MoveUpStepToCombo.Size = new System.Drawing.Size(150, 66);
            this.btn_MoveUpStepToCombo.TabIndex = 0;
            this.btn_MoveUpStepToCombo.Text = "上移测试模块";
            this.btn_MoveUpStepToCombo.UseVisualStyleBackColor = true;
            this.btn_MoveUpStepToCombo.Click += new System.EventHandler(this.btn_MoveUpStepToCombo_Click);
            // 
            // btn_AddNewStepToPreCombo
            // 
            this.btn_AddNewStepToPreCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_AddNewStepToPreCombo.Font = new System.Drawing.Font("宋体", 12F);
            this.btn_AddNewStepToPreCombo.Location = new System.Drawing.Point(150, 0);
            this.btn_AddNewStepToPreCombo.Margin = new System.Windows.Forms.Padding(0);
            this.btn_AddNewStepToPreCombo.Name = "btn_AddNewStepToPreCombo";
            this.btn_AddNewStepToPreCombo.Size = new System.Drawing.Size(150, 66);
            this.btn_AddNewStepToPreCombo.TabIndex = 0;
            this.btn_AddNewStepToPreCombo.Text = "添加到测试项到[前置]";
            this.btn_AddNewStepToPreCombo.UseVisualStyleBackColor = true;
            this.btn_AddNewStepToPreCombo.Click += new System.EventHandler(this.btn_AddNewStepToPreCombo_Click);
            // 
            // btn_AddNewStepToPostCombo
            // 
            this.btn_AddNewStepToPostCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_AddNewStepToPostCombo.Font = new System.Drawing.Font("宋体", 12F);
            this.btn_AddNewStepToPostCombo.Location = new System.Drawing.Point(450, 0);
            this.btn_AddNewStepToPostCombo.Margin = new System.Windows.Forms.Padding(0);
            this.btn_AddNewStepToPostCombo.Name = "btn_AddNewStepToPostCombo";
            this.btn_AddNewStepToPostCombo.Size = new System.Drawing.Size(150, 66);
            this.btn_AddNewStepToPostCombo.TabIndex = 0;
            this.btn_AddNewStepToPostCombo.Text = "添加到测试项到[后置]";
            this.btn_AddNewStepToPostCombo.UseVisualStyleBackColor = true;
            this.btn_AddNewStepToPostCombo.Click += new System.EventHandler(this.btn_AddNewStepToPostCombo_Click);
            // 
            // treeView_TestExecutorCombo
            // 
            this.treeView_TestExecutorCombo.ContextMenuStrip = this.cms_ExecutorComboTreeView;
            this.treeView_TestExecutorCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_TestExecutorCombo.Location = new System.Drawing.Point(458, 5);
            this.treeView_TestExecutorCombo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.treeView_TestExecutorCombo.Name = "treeView_TestExecutorCombo";
            treeNode1.Name = "节点0";
            treeNode1.Text = "节点0";
            this.treeView_TestExecutorCombo.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeView_TestExecutorCombo.Size = new System.Drawing.Size(595, 656);
            this.treeView_TestExecutorCombo.TabIndex = 12;
            // 
            // cms_ExecutorComboTreeView
            // 
            this.cms_ExecutorComboTreeView.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cms_ExecutorComboTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsm_deleteTestModuleFromCombo,
            this.toolStripSeparator1,
            this.tsm_deleteCalculatorFromTestModule,
            this.toolStripSeparator2,
            this.tsm_InsertCalculatorIntoTestModule});
            this.cms_ExecutorComboTreeView.Name = "contextMenuStrip1";
            this.cms_ExecutorComboTreeView.Size = new System.Drawing.Size(199, 88);
            // 
            // tsm_deleteTestModuleFromCombo
            // 
            this.tsm_deleteTestModuleFromCombo.Name = "tsm_deleteTestModuleFromCombo";
            this.tsm_deleteTestModuleFromCombo.Size = new System.Drawing.Size(198, 24);
            this.tsm_deleteTestModuleFromCombo.Text = "删除此测试模块";
            this.tsm_deleteTestModuleFromCombo.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(195, 6);
            // 
            // tsm_deleteCalculatorFromTestModule
            // 
            this.tsm_deleteCalculatorFromTestModule.Name = "tsm_deleteCalculatorFromTestModule";
            this.tsm_deleteCalculatorFromTestModule.Size = new System.Drawing.Size(198, 24);
            this.tsm_deleteCalculatorFromTestModule.Text = "删除此算子";
            this.tsm_deleteCalculatorFromTestModule.Click += new System.EventHandler(this.tsm_deleteCalculatorFromTestModule_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(195, 6);
            // 
            // tsm_InsertCalculatorIntoTestModule
            // 
            this.tsm_InsertCalculatorIntoTestModule.Name = "tsm_InsertCalculatorIntoTestModule";
            this.tsm_InsertCalculatorIntoTestModule.Size = new System.Drawing.Size(198, 24);
            this.tsm_InsertCalculatorIntoTestModule.Text = "插入算子到此模块";
            this.tsm_InsertCalculatorIntoTestModule.Click += new System.EventHandler(this.tsm_InsertCalculatorIntoTestModule_Click);
            // 
            // lv_StepComboFiles
            // 
            this.lv_StepComboFiles.ContextMenuStrip = this.cms_listView;
            this.lv_StepComboFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_StepComboFiles.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lv_StepComboFiles.GridLines = true;
            this.lv_StepComboFiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lv_StepComboFiles.HideSelection = false;
            this.lv_StepComboFiles.Location = new System.Drawing.Point(1062, 5);
            this.lv_StepComboFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lv_StepComboFiles.MultiSelect = false;
            this.lv_StepComboFiles.Name = "lv_StepComboFiles";
            this.lv_StepComboFiles.Size = new System.Drawing.Size(445, 656);
            this.lv_StepComboFiles.TabIndex = 8;
            this.lv_StepComboFiles.UseCompatibleStateImageBehavior = false;
            this.lv_StepComboFiles.View = System.Windows.Forms.View.Details;
            this.lv_StepComboFiles.SelectedIndexChanged += new System.EventHandler(this.lv_StepComboFiles_SelectedIndexChanged);
            // 
            // cms_listView
            // 
            this.cms_listView.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cms_listView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.cms_listView.Name = "cms_listView";
            this.cms_listView.Size = new System.Drawing.Size(109, 28);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(108, 24);
            this.toolStripMenuItem1.Text = "删除";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click_1);
            // 
            // menu_windowSetting
            // 
            this.menu_windowSetting.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menu_windowSetting.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.窗口设置ToolStripMenuItem});
            this.menu_windowSetting.Location = new System.Drawing.Point(0, 0);
            this.menu_windowSetting.Name = "menu_windowSetting";
            this.menu_windowSetting.Size = new System.Drawing.Size(1512, 30);
            this.menu_windowSetting.TabIndex = 3;
            this.menu_windowSetting.Text = "menuStrip1";
            // 
            // 窗口设置ToolStripMenuItem
            // 
            this.窗口设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.浮动ToolStripMenuItem,
            this.还原ToolStripMenuItem});
            this.窗口设置ToolStripMenuItem.Name = "窗口设置ToolStripMenuItem";
            this.窗口设置ToolStripMenuItem.Size = new System.Drawing.Size(83, 26);
            this.窗口设置ToolStripMenuItem.Text = "窗口设置";
            // 
            // 浮动ToolStripMenuItem
            // 
            this.浮动ToolStripMenuItem.Name = "浮动ToolStripMenuItem";
            this.浮动ToolStripMenuItem.Size = new System.Drawing.Size(122, 26);
            this.浮动ToolStripMenuItem.Text = "浮动";
            // 
            // 还原ToolStripMenuItem
            // 
            this.还原ToolStripMenuItem.Name = "还原ToolStripMenuItem";
            this.还原ToolStripMenuItem.Size = new System.Drawing.Size(122, 26);
            this.还原ToolStripMenuItem.Text = "还原";
            // 
            // Form_TestExecutorCombo_Builder_SimpleMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1512, 771);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Controls.Add(this.menu_windowSetting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form_TestExecutorCombo_Builder_SimpleMode";
            this.Text = "Form_StepCombo";
            this.Load += new System.EventHandler(this.Form_StepCombo_Load);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_localExecutors)).EndInit();
            this.cms_previewTestModuleTreeView.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.cms_ExecutorComboTreeView.ResumeLayout(false);
            this.cms_listView.ResumeLayout(false);
            this.menu_windowSetting.ResumeLayout(false);
            this.menu_windowSetting.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dgv_localExecutors;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.Button btn_SaveStepCombo;
        private System.Windows.Forms.Button btn_AddNewStepToMainCombo;
        private System.Windows.Forms.Button btn_MoveUpStepToCombo;
        private System.Windows.Forms.Button btn_MoveDownStepToCombo;
        private System.Windows.Forms.Button btn_CreateNewStepCombo;
        private System.Windows.Forms.ListView lv_StepComboFiles;
        private System.Windows.Forms.TreeView treeView_TestExecutorCombo;
        private System.Windows.Forms.MenuStrip menu_windowSetting;
        private System.Windows.Forms.ToolStripMenuItem 窗口设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 浮动ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 还原ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cms_ExecutorComboTreeView;
        private System.Windows.Forms.ToolStripMenuItem tsm_deleteTestModuleFromCombo;
        private System.Windows.Forms.Button btn_AddNewStepToPreCombo;
        private System.Windows.Forms.Button btn_AddNewStepToPostCombo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ContextMenuStrip cms_previewTestModuleTreeView;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem tsm_deleteCalculatorFromTestModule;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsm_InsertCalculatorIntoTestModule;
        private System.Windows.Forms.ContextMenuStrip cms_listView;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}