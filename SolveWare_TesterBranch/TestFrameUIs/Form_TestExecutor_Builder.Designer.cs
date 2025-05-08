
namespace SolveWare_TesterCore 
{
    partial class Form_TestExecutor_Builder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_TestExecutor_Builder));
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.dgv_ImportedTestModuleClass = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_supportedCalculatorClass = new System.Windows.Forms.DataGridView();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_UpdateTestModule_To_TestExecutorConfigTree = new System.Windows.Forms.Button();
            this.tv_CreateTestExecutorConfigTree = new System.Windows.Forms.Button();
            this.btn_UpdateCalculator_To_TestExecutorConfigTree = new System.Windows.Forms.Button();
            this.btn_SaveTestExecutorConfigTree = new System.Windows.Forms.Button();
            this.listView_TestExecutorConfig = new System.Windows.Forms.ListView();
            this.treeView_TestExecutorConfigItem = new System.Windows.Forms.TreeView();
            this.cms_ExecutorConfigItemTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_windowSetting = new System.Windows.Forms.MenuStrip();
            this.窗口设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.浮动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.还原ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ImportedTestModuleClass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_supportedCalculatorClass)).BeginInit();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.cms_ExecutorConfigItemTreeView.SuspendLayout();
            this.menu_windowSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel3.Controls.Add(this.dgv_ImportedTestModuleClass, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.dgv_supportedCalculatorClass, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.listView_TestExecutorConfig, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.treeView_TestExecutorConfigItem, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1134, 592);
            this.tableLayoutPanel3.TabIndex = 9;
            // 
            // dgv_ImportedTestModuleClass
            // 
            this.dgv_ImportedTestModuleClass.AllowUserToAddRows = false;
            this.dgv_ImportedTestModuleClass.AllowUserToDeleteRows = false;
            this.dgv_ImportedTestModuleClass.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_ImportedTestModuleClass.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            this.dgv_ImportedTestModuleClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_ImportedTestModuleClass.Location = new System.Drawing.Point(3, 3);
            this.dgv_ImportedTestModuleClass.MultiSelect = false;
            this.dgv_ImportedTestModuleClass.Name = "dgv_ImportedTestModuleClass";
            this.dgv_ImportedTestModuleClass.ReadOnly = true;
            this.dgv_ImportedTestModuleClass.RowHeadersVisible = false;
            this.dgv_ImportedTestModuleClass.RowTemplate.Height = 23;
            this.dgv_ImportedTestModuleClass.Size = new System.Drawing.Size(334, 290);
            this.dgv_ImportedTestModuleClass.TabIndex = 5;
            this.dgv_ImportedTestModuleClass.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_ImportedTestModuleClass_CellMouseClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "已导入的测试模块类型(Test Module)";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dgv_supportedCalculatorClass
            // 
            this.dgv_supportedCalculatorClass.AllowUserToAddRows = false;
            this.dgv_supportedCalculatorClass.AllowUserToDeleteRows = false;
            this.dgv_supportedCalculatorClass.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_supportedCalculatorClass.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column10});
            this.dgv_supportedCalculatorClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_supportedCalculatorClass.Location = new System.Drawing.Point(3, 299);
            this.dgv_supportedCalculatorClass.MultiSelect = false;
            this.dgv_supportedCalculatorClass.Name = "dgv_supportedCalculatorClass";
            this.dgv_supportedCalculatorClass.ReadOnly = true;
            this.dgv_supportedCalculatorClass.RowHeadersVisible = false;
            this.dgv_supportedCalculatorClass.RowTemplate.Height = 23;
            this.dgv_supportedCalculatorClass.Size = new System.Drawing.Size(334, 290);
            this.dgv_supportedCalculatorClass.TabIndex = 7;
            // 
            // Column10
            // 
            this.Column10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column10.FillWeight = 25F;
            this.Column10.HeaderText = "可支持算子类型(Calculator)";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(343, 3);
            this.panel1.Name = "panel1";
            this.tableLayoutPanel3.SetRowSpan(this.panel1, 2);
            this.panel1.Size = new System.Drawing.Size(50, 586);
            this.panel1.TabIndex = 9;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.btn_UpdateTestModule_To_TestExecutorConfigTree, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.tv_CreateTestExecutorConfigTree, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.btn_UpdateCalculator_To_TestExecutorConfigTree, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.btn_SaveTestExecutorConfigTree, 0, 3);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 10;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(50, 586);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // btn_UpdateTestModule_To_TestExecutorConfigTree
            // 
            this.btn_UpdateTestModule_To_TestExecutorConfigTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_UpdateTestModule_To_TestExecutorConfigTree.Location = new System.Drawing.Point(3, 61);
            this.btn_UpdateTestModule_To_TestExecutorConfigTree.Name = "btn_UpdateTestModule_To_TestExecutorConfigTree";
            this.btn_UpdateTestModule_To_TestExecutorConfigTree.Size = new System.Drawing.Size(44, 52);
            this.btn_UpdateTestModule_To_TestExecutorConfigTree.TabIndex = 0;
            this.btn_UpdateTestModule_To_TestExecutorConfigTree.Text = "更新测试模块到测试项";
            this.btn_UpdateTestModule_To_TestExecutorConfigTree.UseVisualStyleBackColor = true;
            this.btn_UpdateTestModule_To_TestExecutorConfigTree.Click += new System.EventHandler(this.btn_UpdateTestModule_To_TestExecutorConfigTree_Click);
            // 
            // tv_CreateTestExecutorConfigTree
            // 
            this.tv_CreateTestExecutorConfigTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tv_CreateTestExecutorConfigTree.Location = new System.Drawing.Point(3, 3);
            this.tv_CreateTestExecutorConfigTree.Name = "tv_CreateTestExecutorConfigTree";
            this.tv_CreateTestExecutorConfigTree.Size = new System.Drawing.Size(44, 52);
            this.tv_CreateTestExecutorConfigTree.TabIndex = 1;
            this.tv_CreateTestExecutorConfigTree.Text = "新建测试项";
            this.tv_CreateTestExecutorConfigTree.UseVisualStyleBackColor = true;
            this.tv_CreateTestExecutorConfigTree.Click += new System.EventHandler(this.tv_CreateTestExecutorConfigTree_Click);
            // 
            // btn_UpdateCalculator_To_TestExecutorConfigTree
            // 
            this.btn_UpdateCalculator_To_TestExecutorConfigTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_UpdateCalculator_To_TestExecutorConfigTree.Location = new System.Drawing.Point(3, 119);
            this.btn_UpdateCalculator_To_TestExecutorConfigTree.Name = "btn_UpdateCalculator_To_TestExecutorConfigTree";
            this.btn_UpdateCalculator_To_TestExecutorConfigTree.Size = new System.Drawing.Size(44, 52);
            this.btn_UpdateCalculator_To_TestExecutorConfigTree.TabIndex = 2;
            this.btn_UpdateCalculator_To_TestExecutorConfigTree.Text = "添加算子到测试项";
            this.btn_UpdateCalculator_To_TestExecutorConfigTree.UseVisualStyleBackColor = true;
            this.btn_UpdateCalculator_To_TestExecutorConfigTree.Click += new System.EventHandler(this.btn_UpdateCalculator_To_TestExecutorConfigTree_Click);
            // 
            // btn_SaveTestExecutorConfigTree
            // 
            this.btn_SaveTestExecutorConfigTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_SaveTestExecutorConfigTree.Location = new System.Drawing.Point(3, 177);
            this.btn_SaveTestExecutorConfigTree.Name = "btn_SaveTestExecutorConfigTree";
            this.btn_SaveTestExecutorConfigTree.Size = new System.Drawing.Size(44, 52);
            this.btn_SaveTestExecutorConfigTree.TabIndex = 3;
            this.btn_SaveTestExecutorConfigTree.Text = "保存测试项";
            this.btn_SaveTestExecutorConfigTree.UseVisualStyleBackColor = true;
            this.btn_SaveTestExecutorConfigTree.Click += new System.EventHandler(this.btn_SaveTestExecutorConfigTree_Click);
            // 
            // listView_TestExecutorConfig
            // 
            this.listView_TestExecutorConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_TestExecutorConfig.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView_TestExecutorConfig.GridLines = true;
            this.listView_TestExecutorConfig.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView_TestExecutorConfig.HideSelection = false;
            this.listView_TestExecutorConfig.Location = new System.Drawing.Point(795, 3);
            this.listView_TestExecutorConfig.MultiSelect = false;
            this.listView_TestExecutorConfig.Name = "listView_TestExecutorConfig";
            this.tableLayoutPanel3.SetRowSpan(this.listView_TestExecutorConfig, 2);
            this.listView_TestExecutorConfig.Size = new System.Drawing.Size(336, 586);
            this.listView_TestExecutorConfig.TabIndex = 10;
            this.listView_TestExecutorConfig.UseCompatibleStateImageBehavior = false;
            this.listView_TestExecutorConfig.View = System.Windows.Forms.View.Details;
            this.listView_TestExecutorConfig.SelectedIndexChanged += new System.EventHandler(this.listView_TestExecutorConfig_SelectedIndexChanged);
            // 
            // treeView_TestExecutorConfigItem
            // 
            this.treeView_TestExecutorConfigItem.ContextMenuStrip = this.cms_ExecutorConfigItemTreeView;
            this.treeView_TestExecutorConfigItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_TestExecutorConfigItem.Location = new System.Drawing.Point(399, 3);
            this.treeView_TestExecutorConfigItem.Name = "treeView_TestExecutorConfigItem";
            treeNode1.Name = "节点0";
            treeNode1.Text = "节点0";
            this.treeView_TestExecutorConfigItem.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.tableLayoutPanel3.SetRowSpan(this.treeView_TestExecutorConfigItem, 2);
            this.treeView_TestExecutorConfigItem.Size = new System.Drawing.Size(390, 586);
            this.treeView_TestExecutorConfigItem.TabIndex = 8;
            // 
            // cms_ExecutorConfigItemTreeView
            // 
            this.cms_ExecutorConfigItemTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除ToolStripMenuItem});
            this.cms_ExecutorConfigItemTreeView.Name = "contextMenuStrip1";
            this.cms_ExecutorConfigItemTreeView.Size = new System.Drawing.Size(101, 26);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // menu_windowSetting
            // 
            this.menu_windowSetting.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.窗口设置ToolStripMenuItem});
            this.menu_windowSetting.Location = new System.Drawing.Point(0, 0);
            this.menu_windowSetting.Name = "menu_windowSetting";
            this.menu_windowSetting.Size = new System.Drawing.Size(1134, 25);
            this.menu_windowSetting.TabIndex = 10;
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
            // Form_TestExecutor_Builder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 617);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.menu_windowSetting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_TestExecutor_Builder";
            this.Text = "Form_TestModule";
            this.Load += new System.EventHandler(this.Form_TestModule_Load);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ImportedTestModuleClass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_supportedCalculatorClass)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.cms_ExecutorConfigItemTreeView.ResumeLayout(false);
            this.menu_windowSetting.ResumeLayout(false);
            this.menu_windowSetting.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.DataGridView dgv_ImportedTestModuleClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.TreeView treeView_TestExecutorConfigItem;
        private System.Windows.Forms.DataGridView dgv_supportedCalculatorClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button btn_UpdateTestModule_To_TestExecutorConfigTree;
        private System.Windows.Forms.Button tv_CreateTestExecutorConfigTree;
        private System.Windows.Forms.Button btn_UpdateCalculator_To_TestExecutorConfigTree;
        private System.Windows.Forms.Button btn_SaveTestExecutorConfigTree;
        private System.Windows.Forms.ListView listView_TestExecutorConfig;
        private System.Windows.Forms.MenuStrip menu_windowSetting;
        private System.Windows.Forms.ToolStripMenuItem 窗口设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 浮动ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 还原ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cms_ExecutorConfigItemTreeView;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
    }
}