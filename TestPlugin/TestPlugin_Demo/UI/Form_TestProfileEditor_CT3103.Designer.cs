
namespace TestPlugin_Demo
{
     partial class Form_TestProfileEditor_CT3103
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_TestProfileEditor_CT3103));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tab_ComboToProfile = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.treeView_TestExecutor_1 = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_ClearExecutor_1 = new System.Windows.Forms.Button();
            this.btn_UpdateComboToExecutor_1 = new System.Windows.Forms.Button();
            this.cb_localTestExecutorComboFiles = new System.Windows.Forms.ComboBox();
            this.tab_TestParamToProfile = new System.Windows.Forms.TabPage();
            this.tab_TestParamEditorLayer = new System.Windows.Forms.TabControl();
            this.tab_BinToProfile = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.pnl_binSort = new System.Windows.Forms.Panel();
            this.tab_CalibrationToProfile = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.dgv_CalibrationData_PosLoaderToProfile = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.tab_SpecToProfile = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.pdgv_specEditor = new SolveWare_TestComponents.UI.PropertyDataGirdView();
            this.panel5 = new System.Windows.Forms.Panel();
            this.tb_CurrentProfileSpecTag = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_SpecToUse = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btn_createNewTestProfile = new System.Windows.Forms.Button();
            this.btn_saveTestProfile = new System.Windows.Forms.Button();
            this.lv_TestProfileFiles = new System.Windows.Forms.ListView();
            this.CMS_del = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssl_currentEditingTestProfile = new System.Windows.Forms.ToolStripStatusLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.cob_TestPur = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tab_ComboToProfile.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tab_TestParamToProfile.SuspendLayout();
            this.tab_BinToProfile.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tab_CalibrationToProfile.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_CalibrationData_PosLoaderToProfile)).BeginInit();
            this.panel3.SuspendLayout();
            this.tab_SpecToProfile.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pdgv_specEditor)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.CMS_del.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1256, 742);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tab_ComboToProfile);
            this.tabControl1.Controls.Add(this.tab_TestParamToProfile);
            this.tabControl1.Controls.Add(this.tab_BinToProfile);
            this.tabControl1.Controls.Add(this.tab_CalibrationToProfile);
            this.tabControl1.Controls.Add(this.tab_SpecToProfile);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 111);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1256, 631);
            this.tabControl1.TabIndex = 0;
            // 
            // tab_ComboToProfile
            // 
            this.tab_ComboToProfile.Controls.Add(this.tableLayoutPanel2);
            this.tab_ComboToProfile.Location = new System.Drawing.Point(4, 22);
            this.tab_ComboToProfile.Name = "tab_ComboToProfile";
            this.tab_ComboToProfile.Padding = new System.Windows.Forms.Padding(3);
            this.tab_ComboToProfile.Size = new System.Drawing.Size(1248, 605);
            this.tab_ComboToProfile.TabIndex = 1;
            this.tab_ComboToProfile.Text = "测试项目链配置";
            this.tab_ComboToProfile.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.treeView_TestExecutor_1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1242, 599);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // treeView_TestExecutor_1
            // 
            this.treeView_TestExecutor_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_TestExecutor_1.Location = new System.Drawing.Point(3, 92);
            this.treeView_TestExecutor_1.Name = "treeView_TestExecutor_1";
            this.treeView_TestExecutor_1.Size = new System.Drawing.Size(1236, 504);
            this.treeView_TestExecutor_1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btn_ClearExecutor_1);
            this.panel1.Controls.Add(this.btn_UpdateComboToExecutor_1);
            this.panel1.Controls.Add(this.cb_localTestExecutorComboFiles);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1236, 83);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "可选测试项目链表";
            // 
            // btn_ClearExecutor_1
            // 
            this.btn_ClearExecutor_1.Location = new System.Drawing.Point(810, 28);
            this.btn_ClearExecutor_1.Name = "btn_ClearExecutor_1";
            this.btn_ClearExecutor_1.Size = new System.Drawing.Size(264, 29);
            this.btn_ClearExecutor_1.TabIndex = 1;
            this.btn_ClearExecutor_1.Text = "清空测试项目链表(测试站1号）";
            this.btn_ClearExecutor_1.UseVisualStyleBackColor = true;
            this.btn_ClearExecutor_1.Click += new System.EventHandler(this.btn_ClearExecutor_1_Click);
            // 
            // btn_UpdateComboToExecutor_1
            // 
            this.btn_UpdateComboToExecutor_1.Location = new System.Drawing.Point(540, 28);
            this.btn_UpdateComboToExecutor_1.Name = "btn_UpdateComboToExecutor_1";
            this.btn_UpdateComboToExecutor_1.Size = new System.Drawing.Size(264, 29);
            this.btn_UpdateComboToExecutor_1.TabIndex = 1;
            this.btn_UpdateComboToExecutor_1.Text = "配置所选测试项目链表到(测试站1号）";
            this.btn_UpdateComboToExecutor_1.UseVisualStyleBackColor = true;
            this.btn_UpdateComboToExecutor_1.Click += new System.EventHandler(this.btn_UpdateComboToExecutor_1_Click);
            // 
            // cb_localTestExecutorComboFiles
            // 
            this.cb_localTestExecutorComboFiles.FormattingEnabled = true;
            this.cb_localTestExecutorComboFiles.Location = new System.Drawing.Point(51, 33);
            this.cb_localTestExecutorComboFiles.Name = "cb_localTestExecutorComboFiles";
            this.cb_localTestExecutorComboFiles.Size = new System.Drawing.Size(470, 20);
            this.cb_localTestExecutorComboFiles.TabIndex = 0;
            // 
            // tab_TestParamToProfile
            // 
            this.tab_TestParamToProfile.Controls.Add(this.tab_TestParamEditorLayer);
            this.tab_TestParamToProfile.Location = new System.Drawing.Point(4, 22);
            this.tab_TestParamToProfile.Name = "tab_TestParamToProfile";
            this.tab_TestParamToProfile.Padding = new System.Windows.Forms.Padding(3);
            this.tab_TestParamToProfile.Size = new System.Drawing.Size(1248, 605);
            this.tab_TestParamToProfile.TabIndex = 3;
            this.tab_TestParamToProfile.Text = "测试参数配置";
            this.tab_TestParamToProfile.UseVisualStyleBackColor = true;
            // 
            // tab_TestParamEditorLayer
            // 
            this.tab_TestParamEditorLayer.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tab_TestParamEditorLayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab_TestParamEditorLayer.Location = new System.Drawing.Point(3, 3);
            this.tab_TestParamEditorLayer.Name = "tab_TestParamEditorLayer";
            this.tab_TestParamEditorLayer.SelectedIndex = 0;
            this.tab_TestParamEditorLayer.Size = new System.Drawing.Size(1242, 599);
            this.tab_TestParamEditorLayer.TabIndex = 1;
            // 
            // tab_BinToProfile
            // 
            this.tab_BinToProfile.Controls.Add(this.tableLayoutPanel3);
            this.tab_BinToProfile.Location = new System.Drawing.Point(4, 22);
            this.tab_BinToProfile.Name = "tab_BinToProfile";
            this.tab_BinToProfile.Padding = new System.Windows.Forms.Padding(3);
            this.tab_BinToProfile.Size = new System.Drawing.Size(1248, 605);
            this.tab_BinToProfile.TabIndex = 2;
            this.tab_BinToProfile.Text = "分Bin配置";
            this.tab_BinToProfile.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.pnl_binSort, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1242, 599);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // pnl_binSort
            // 
            this.pnl_binSort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_binSort.Location = new System.Drawing.Point(3, 3);
            this.pnl_binSort.Name = "pnl_binSort";
            this.pnl_binSort.Size = new System.Drawing.Size(1236, 593);
            this.pnl_binSort.TabIndex = 0;
            // 
            // tab_CalibrationToProfile
            // 
            this.tab_CalibrationToProfile.Controls.Add(this.tableLayoutPanel6);
            this.tab_CalibrationToProfile.Location = new System.Drawing.Point(4, 22);
            this.tab_CalibrationToProfile.Name = "tab_CalibrationToProfile";
            this.tab_CalibrationToProfile.Padding = new System.Windows.Forms.Padding(3);
            this.tab_CalibrationToProfile.Size = new System.Drawing.Size(1248, 605);
            this.tab_CalibrationToProfile.TabIndex = 5;
            this.tab_CalibrationToProfile.Text = "校准系数编辑";
            this.tab_CalibrationToProfile.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.Controls.Add(this.dgv_CalibrationData_PosLoaderToProfile, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.228374F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.77163F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(1242, 599);
            this.tableLayoutPanel6.TabIndex = 1;
            // 
            // dgv_CalibrationData_PosLoaderToProfile
            // 
            this.dgv_CalibrationData_PosLoaderToProfile.AllowUserToAddRows = false;
            this.dgv_CalibrationData_PosLoaderToProfile.AllowUserToDeleteRows = false;
            this.dgv_CalibrationData_PosLoaderToProfile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_CalibrationData_PosLoaderToProfile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dgv_CalibrationData_PosLoaderToProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_CalibrationData_PosLoaderToProfile.Location = new System.Drawing.Point(4, 42);
            this.dgv_CalibrationData_PosLoaderToProfile.Name = "dgv_CalibrationData_PosLoaderToProfile";
            this.dgv_CalibrationData_PosLoaderToProfile.RowHeadersVisible = false;
            this.dgv_CalibrationData_PosLoaderToProfile.RowTemplate.Height = 23;
            this.dgv_CalibrationData_PosLoaderToProfile.Size = new System.Drawing.Size(1234, 553);
            this.dgv_CalibrationData_PosLoaderToProfile.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.FillWeight = 40F;
            this.Column1.HeaderText = "系数名";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.FillWeight = 30F;
            this.Column2.HeaderText = "K值";
            this.Column2.Name = "Column2";
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column3.FillWeight = 30F;
            this.Column3.HeaderText = "B值";
            this.Column3.Name = "Column3";
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1234, 31);
            this.panel3.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Right;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(1158, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "校准参数";
            // 
            // tab_SpecToProfile
            // 
            this.tab_SpecToProfile.Controls.Add(this.panel4);
            this.tab_SpecToProfile.Location = new System.Drawing.Point(4, 22);
            this.tab_SpecToProfile.Name = "tab_SpecToProfile";
            this.tab_SpecToProfile.Padding = new System.Windows.Forms.Padding(3);
            this.tab_SpecToProfile.Size = new System.Drawing.Size(1248, 605);
            this.tab_SpecToProfile.TabIndex = 0;
            this.tab_SpecToProfile.Text = "测试规格配置";
            this.tab_SpecToProfile.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.tableLayoutPanel4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1242, 599);
            this.panel4.TabIndex = 5;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.pdgv_specEditor, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.panel5, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1242, 599);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // pdgv_specEditor
            // 
            this.pdgv_specEditor.AllowUserToAddRows = false;
            this.pdgv_specEditor.AllowUserToDeleteRows = false;
            this.pdgv_specEditor.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Info;
            this.pdgv_specEditor.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.pdgv_specEditor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.pdgv_specEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pdgv_specEditor.Location = new System.Drawing.Point(3, 152);
            this.pdgv_specEditor.Name = "pdgv_specEditor";
            this.pdgv_specEditor.RowHeadersVisible = false;
            this.pdgv_specEditor.RowTemplate.Height = 23;
            this.pdgv_specEditor.Size = new System.Drawing.Size(1236, 444);
            this.pdgv_specEditor.TabIndex = 6;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.tb_CurrentProfileSpecTag);
            this.panel5.Controls.Add(this.label3);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Controls.Add(this.cb_SpecToUse);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1236, 143);
            this.panel5.TabIndex = 0;
            // 
            // tb_CurrentProfileSpecTag
            // 
            this.tb_CurrentProfileSpecTag.Location = new System.Drawing.Point(103, 103);
            this.tb_CurrentProfileSpecTag.Name = "tb_CurrentProfileSpecTag";
            this.tb_CurrentProfileSpecTag.ReadOnly = true;
            this.tb_CurrentProfileSpecTag.Size = new System.Drawing.Size(611, 21);
            this.tb_CurrentProfileSpecTag.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(101, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "当前方案使用的测试规格";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(101, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "可选测试项目链表";
            // 
            // cb_SpecToUse
            // 
            this.cb_SpecToUse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_SpecToUse.FormattingEnabled = true;
            this.cb_SpecToUse.Location = new System.Drawing.Point(103, 41);
            this.cb_SpecToUse.Name = "cb_SpecToUse";
            this.cb_SpecToUse.Size = new System.Drawing.Size(611, 20);
            this.cb_SpecToUse.TabIndex = 1;
            this.cb_SpecToUse.SelectionChangeCommitted += new System.EventHandler(this.cb_SpecToUse_SelectionChangeCommitted);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tableLayoutPanel5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1250, 105);
            this.panel2.TabIndex = 1;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.panel6, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.lv_TestProfileFiles, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1250, 105);
            this.tableLayoutPanel5.TabIndex = 3;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label6);
            this.panel6.Controls.Add(this.cob_TestPur);
            this.panel6.Controls.Add(this.btn_createNewTestProfile);
            this.panel6.Controls.Add(this.btn_saveTestProfile);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(3, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(619, 99);
            this.panel6.TabIndex = 5;
            // 
            // btn_createNewTestProfile
            // 
            this.btn_createNewTestProfile.Location = new System.Drawing.Point(38, 31);
            this.btn_createNewTestProfile.Name = "btn_createNewTestProfile";
            this.btn_createNewTestProfile.Size = new System.Drawing.Size(246, 23);
            this.btn_createNewTestProfile.TabIndex = 0;
            this.btn_createNewTestProfile.Text = "新建测试方案";
            this.btn_createNewTestProfile.UseVisualStyleBackColor = true;
            this.btn_createNewTestProfile.Click += new System.EventHandler(this.btn_createNewTestProfile_Click);
            // 
            // btn_saveTestProfile
            // 
            this.btn_saveTestProfile.Location = new System.Drawing.Point(323, 31);
            this.btn_saveTestProfile.Name = "btn_saveTestProfile";
            this.btn_saveTestProfile.Size = new System.Drawing.Size(246, 23);
            this.btn_saveTestProfile.TabIndex = 0;
            this.btn_saveTestProfile.Text = "保存测试方案";
            this.btn_saveTestProfile.UseVisualStyleBackColor = true;
            this.btn_saveTestProfile.Click += new System.EventHandler(this.btn_saveTestProfile_Click);
            // 
            // lv_TestProfileFiles
            // 
            this.lv_TestProfileFiles.ContextMenuStrip = this.CMS_del;
            this.lv_TestProfileFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_TestProfileFiles.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lv_TestProfileFiles.GridLines = true;
            this.lv_TestProfileFiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lv_TestProfileFiles.HideSelection = false;
            this.lv_TestProfileFiles.Location = new System.Drawing.Point(628, 3);
            this.lv_TestProfileFiles.MultiSelect = false;
            this.lv_TestProfileFiles.Name = "lv_TestProfileFiles";
            this.lv_TestProfileFiles.Size = new System.Drawing.Size(619, 99);
            this.lv_TestProfileFiles.TabIndex = 9;
            this.lv_TestProfileFiles.UseCompatibleStateImageBehavior = false;
            this.lv_TestProfileFiles.View = System.Windows.Forms.View.Details;
            this.lv_TestProfileFiles.SelectedIndexChanged += new System.EventHandler(this.lv_TestProfileFiles_SelectedIndexChanged);
            // 
            // CMS_del
            // 
            this.CMS_del.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除ToolStripMenuItem});
            this.CMS_del.Name = "CMS_del";
            this.CMS_del.Size = new System.Drawing.Size(101, 26);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssl_currentEditingTestProfile});
            this.statusStrip1.Location = new System.Drawing.Point(0, 742);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1256, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "当前编辑测试方案:[- - ]";
            // 
            // tssl_currentEditingTestProfile
            // 
            this.tssl_currentEditingTestProfile.Name = "tssl_currentEditingTestProfile";
            this.tssl_currentEditingTestProfile.Size = new System.Drawing.Size(145, 17);
            this.tssl_currentEditingTestProfile.Text = "当前编辑的测试方案:[- - ]";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(377, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "测试阶段：";
            // 
            // cob_TestPur
            // 
            this.cob_TestPur.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cob_TestPur.FormattingEnabled = true;
            this.cob_TestPur.Location = new System.Drawing.Point(448, 60);
            this.cob_TestPur.Name = "cob_TestPur";
            this.cob_TestPur.Size = new System.Drawing.Size(121, 20);
            this.cob_TestPur.TabIndex = 3;
            // 
            // Form_TestProfileEditor_CT3103
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1256, 764);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "Form_TestProfileEditor_CT3103";
            this.Text = "CT3103测试方案编辑";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form_TestProfileEditor_CT3103_Load);
            this.VisibleChanged += new System.EventHandler(this.Form_TestProfileEditor_CT3103_VisibleChanged);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tab_ComboToProfile.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tab_TestParamToProfile.ResumeLayout(false);
            this.tab_BinToProfile.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tab_CalibrationToProfile.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_CalibrationData_PosLoaderToProfile)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tab_SpecToProfile.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pdgv_specEditor)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.CMS_del.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btn_saveTestProfile;
        private System.Windows.Forms.ListView lv_TestProfileFiles;
        private System.Windows.Forms.Button btn_createNewTestProfile;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tssl_currentEditingTestProfile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tab_TestParamToProfile;
        private System.Windows.Forms.TabControl tab_TestParamEditorLayer;
        private System.Windows.Forms.TabPage tab_BinToProfile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel pnl_binSort;
        private System.Windows.Forms.TabPage tab_CalibrationToProfile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.DataGridView dgv_CalibrationData_PosLoaderToProfile;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tab_ComboToProfile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TreeView treeView_TestExecutor_1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_ClearExecutor_1;
        private System.Windows.Forms.Button btn_UpdateComboToExecutor_1;
        private System.Windows.Forms.ComboBox cb_localTestExecutorComboFiles;
        private System.Windows.Forms.TabPage tab_SpecToProfile;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private SolveWare_TestComponents.UI.PropertyDataGirdView pdgv_specEditor;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox tb_CurrentProfileSpecTag;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_SpecToUse;
        private System.Windows.Forms.ContextMenuStrip CMS_del;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cob_TestPur;
    }
}