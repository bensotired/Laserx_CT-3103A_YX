
namespace SolveWare_TesterCore 
{
    partial class Form_TestExecutorCombo_Builder
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
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("节点0");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_TestExecutorCombo_Builder));
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_AddNewStepToMainCombo = new System.Windows.Forms.Button();
            this.btn_ClearStepCombo = new System.Windows.Forms.Button();
            this.btn_CreateNewStepCombo = new System.Windows.Forms.Button();
            this.btn_SaveStepCombo = new System.Windows.Forms.Button();
            this.btn_MoveDownStepToCombo = new System.Windows.Forms.Button();
            this.btn_MoveUpStepToCombo = new System.Windows.Forms.Button();
            this.btn_AddNewStepToPreCombo = new System.Windows.Forms.Button();
            this.btn_AddNewStepToPostCombo = new System.Windows.Forms.Button();
            this.treeView_TestExecutorCombo = new System.Windows.Forms.TreeView();
            this.cms_ExecutorComboTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.lv_StepComboFiles = new System.Windows.Forms.ListView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgv_localExecutors = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.treeView_TestExecutorConfigItemDetails = new System.Windows.Forms.TreeView();
            this.menu_windowSetting = new System.Windows.Forms.MenuStrip();
            this.窗口设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.浮动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.还原ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.cms_ExecutorComboTreeView.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_localExecutors)).BeginInit();
            this.menu_windowSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel4.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.treeView_TestExecutorCombo, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.lv_StepComboFiles, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1134, 592);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tableLayoutPanel2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(343, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(50, 584);
            this.panel2.TabIndex = 9;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.btn_AddNewStepToMainCombo, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.btn_ClearStepCombo, 0, 9);
            this.tableLayoutPanel2.Controls.Add(this.btn_CreateNewStepCombo, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btn_SaveStepCombo, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.btn_MoveDownStepToCombo, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.btn_MoveUpStepToCombo, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.btn_AddNewStepToPreCombo, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btn_AddNewStepToPostCombo, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 10;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(50, 584);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btn_AddNewStepToMainCombo
            // 
            this.btn_AddNewStepToMainCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_AddNewStepToMainCombo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_AddNewStepToMainCombo.Location = new System.Drawing.Point(0, 116);
            this.btn_AddNewStepToMainCombo.Margin = new System.Windows.Forms.Padding(0);
            this.btn_AddNewStepToMainCombo.Name = "btn_AddNewStepToMainCombo";
            this.btn_AddNewStepToMainCombo.Size = new System.Drawing.Size(50, 58);
            this.btn_AddNewStepToMainCombo.TabIndex = 0;
            this.btn_AddNewStepToMainCombo.Text = "->>    [主要]";
            this.btn_AddNewStepToMainCombo.UseVisualStyleBackColor = true;
            this.btn_AddNewStepToMainCombo.Click += new System.EventHandler(this.btn_AddNewStepToMainCombo_Click);
            // 
            // btn_ClearStepCombo
            // 
            this.btn_ClearStepCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_ClearStepCombo.Font = new System.Drawing.Font("宋体", 9F);
            this.btn_ClearStepCombo.Location = new System.Drawing.Point(0, 522);
            this.btn_ClearStepCombo.Margin = new System.Windows.Forms.Padding(0);
            this.btn_ClearStepCombo.Name = "btn_ClearStepCombo";
            this.btn_ClearStepCombo.Size = new System.Drawing.Size(50, 62);
            this.btn_ClearStepCombo.TabIndex = 0;
            this.btn_ClearStepCombo.Text = "清空骤表";
            this.btn_ClearStepCombo.UseVisualStyleBackColor = true;
            this.btn_ClearStepCombo.Visible = false;
            this.btn_ClearStepCombo.Click += new System.EventHandler(this.btn_ClearStepCombo_Click);
            // 
            // btn_CreateNewStepCombo
            // 
            this.btn_CreateNewStepCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_CreateNewStepCombo.Font = new System.Drawing.Font("宋体", 9F);
            this.btn_CreateNewStepCombo.Location = new System.Drawing.Point(0, 0);
            this.btn_CreateNewStepCombo.Margin = new System.Windows.Forms.Padding(0);
            this.btn_CreateNewStepCombo.Name = "btn_CreateNewStepCombo";
            this.btn_CreateNewStepCombo.Size = new System.Drawing.Size(50, 58);
            this.btn_CreateNewStepCombo.TabIndex = 0;
            this.btn_CreateNewStepCombo.Text = "新建链表";
            this.btn_CreateNewStepCombo.UseVisualStyleBackColor = true;
            this.btn_CreateNewStepCombo.Click += new System.EventHandler(this.btn_CreateNewStepCombo_Click);
            // 
            // btn_SaveStepCombo
            // 
            this.btn_SaveStepCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_SaveStepCombo.Font = new System.Drawing.Font("宋体", 9F);
            this.btn_SaveStepCombo.Location = new System.Drawing.Point(0, 406);
            this.btn_SaveStepCombo.Margin = new System.Windows.Forms.Padding(0);
            this.btn_SaveStepCombo.Name = "btn_SaveStepCombo";
            this.btn_SaveStepCombo.Size = new System.Drawing.Size(50, 58);
            this.btn_SaveStepCombo.TabIndex = 1;
            this.btn_SaveStepCombo.Text = "保存链表";
            this.btn_SaveStepCombo.UseVisualStyleBackColor = true;
            this.btn_SaveStepCombo.Click += new System.EventHandler(this.btn_SaveStepCombo_Click);
            // 
            // btn_MoveDownStepToCombo
            // 
            this.btn_MoveDownStepToCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_MoveDownStepToCombo.Font = new System.Drawing.Font("宋体", 9F);
            this.btn_MoveDownStepToCombo.Location = new System.Drawing.Point(0, 348);
            this.btn_MoveDownStepToCombo.Margin = new System.Windows.Forms.Padding(0);
            this.btn_MoveDownStepToCombo.Name = "btn_MoveDownStepToCombo";
            this.btn_MoveDownStepToCombo.Size = new System.Drawing.Size(50, 58);
            this.btn_MoveDownStepToCombo.TabIndex = 0;
            this.btn_MoveDownStepToCombo.Text = "下移单步";
            this.btn_MoveDownStepToCombo.UseVisualStyleBackColor = true;
            this.btn_MoveDownStepToCombo.Click += new System.EventHandler(this.btn_MoveDownStepToCombo_Click);
            // 
            // btn_MoveUpStepToCombo
            // 
            this.btn_MoveUpStepToCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_MoveUpStepToCombo.Font = new System.Drawing.Font("宋体", 9F);
            this.btn_MoveUpStepToCombo.Location = new System.Drawing.Point(0, 290);
            this.btn_MoveUpStepToCombo.Margin = new System.Windows.Forms.Padding(0);
            this.btn_MoveUpStepToCombo.Name = "btn_MoveUpStepToCombo";
            this.btn_MoveUpStepToCombo.Size = new System.Drawing.Size(50, 58);
            this.btn_MoveUpStepToCombo.TabIndex = 0;
            this.btn_MoveUpStepToCombo.Text = "上移单步";
            this.btn_MoveUpStepToCombo.UseVisualStyleBackColor = true;
            this.btn_MoveUpStepToCombo.Click += new System.EventHandler(this.btn_MoveUpStepToCombo_Click);
            // 
            // btn_AddNewStepToPreCombo
            // 
            this.btn_AddNewStepToPreCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_AddNewStepToPreCombo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_AddNewStepToPreCombo.Location = new System.Drawing.Point(0, 58);
            this.btn_AddNewStepToPreCombo.Margin = new System.Windows.Forms.Padding(0);
            this.btn_AddNewStepToPreCombo.Name = "btn_AddNewStepToPreCombo";
            this.btn_AddNewStepToPreCombo.Size = new System.Drawing.Size(50, 58);
            this.btn_AddNewStepToPreCombo.TabIndex = 0;
            this.btn_AddNewStepToPreCombo.Text = "->>    [前置]";
            this.btn_AddNewStepToPreCombo.UseVisualStyleBackColor = true;
            this.btn_AddNewStepToPreCombo.Click += new System.EventHandler(this.btn_AddNewStepToPreCombo_Click);
            // 
            // btn_AddNewStepToPostCombo
            // 
            this.btn_AddNewStepToPostCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_AddNewStepToPostCombo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_AddNewStepToPostCombo.Location = new System.Drawing.Point(0, 174);
            this.btn_AddNewStepToPostCombo.Margin = new System.Windows.Forms.Padding(0);
            this.btn_AddNewStepToPostCombo.Name = "btn_AddNewStepToPostCombo";
            this.btn_AddNewStepToPostCombo.Size = new System.Drawing.Size(50, 58);
            this.btn_AddNewStepToPostCombo.TabIndex = 0;
            this.btn_AddNewStepToPostCombo.Text = "->>    [后置]";
            this.btn_AddNewStepToPostCombo.UseVisualStyleBackColor = true;
            this.btn_AddNewStepToPostCombo.Click += new System.EventHandler(this.btn_AddNewStepToPostCombo_Click);
            // 
            // treeView_TestExecutorCombo
            // 
            this.treeView_TestExecutorCombo.ContextMenuStrip = this.cms_ExecutorComboTreeView;
            this.treeView_TestExecutorCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_TestExecutorCombo.Location = new System.Drawing.Point(400, 4);
            this.treeView_TestExecutorCombo.Name = "treeView_TestExecutorCombo";
            treeNode1.Name = "节点0";
            treeNode1.Text = "节点0";
            this.treeView_TestExecutorCombo.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeView_TestExecutorCombo.Size = new System.Drawing.Size(389, 584);
            this.treeView_TestExecutorCombo.TabIndex = 12;
            // 
            // cms_ExecutorComboTreeView
            // 
            this.cms_ExecutorComboTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.cms_ExecutorComboTreeView.Name = "contextMenuStrip1";
            this.cms_ExecutorComboTreeView.Size = new System.Drawing.Size(101, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem1.Text = "删除";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // lv_StepComboFiles
            // 
            this.lv_StepComboFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_StepComboFiles.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lv_StepComboFiles.GridLines = true;
            this.lv_StepComboFiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lv_StepComboFiles.HideSelection = false;
            this.lv_StepComboFiles.Location = new System.Drawing.Point(796, 4);
            this.lv_StepComboFiles.MultiSelect = false;
            this.lv_StepComboFiles.Name = "lv_StepComboFiles";
            this.lv_StepComboFiles.Size = new System.Drawing.Size(334, 584);
            this.lv_StepComboFiles.TabIndex = 8;
            this.lv_StepComboFiles.UseCompatibleStateImageBehavior = false;
            this.lv_StepComboFiles.View = System.Windows.Forms.View.Details;
            this.lv_StepComboFiles.SelectedIndexChanged += new System.EventHandler(this.lv_StepComboFiles_SelectedIndexChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dgv_localExecutors, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.treeView_TestExecutorConfigItemDetails, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(332, 584);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // dgv_localExecutors
            // 
            this.dgv_localExecutors.AllowUserToAddRows = false;
            this.dgv_localExecutors.AllowUserToDeleteRows = false;
            this.dgv_localExecutors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_localExecutors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.dgv_localExecutors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_localExecutors.Location = new System.Drawing.Point(3, 3);
            this.dgv_localExecutors.MultiSelect = false;
            this.dgv_localExecutors.Name = "dgv_localExecutors";
            this.dgv_localExecutors.ReadOnly = true;
            this.dgv_localExecutors.RowHeadersVisible = false;
            this.dgv_localExecutors.RowTemplate.Height = 23;
            this.dgv_localExecutors.Size = new System.Drawing.Size(326, 227);
            this.dgv_localExecutors.TabIndex = 4;
            this.dgv_localExecutors.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_localExecutors_CellMouseClick);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.HeaderText = "可选测试执行项";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // treeView_TestExecutorConfigItemDetails
            // 
            this.treeView_TestExecutorConfigItemDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_TestExecutorConfigItemDetails.Location = new System.Drawing.Point(3, 236);
            this.treeView_TestExecutorConfigItemDetails.Name = "treeView_TestExecutorConfigItemDetails";
            treeNode2.Name = "节点0";
            treeNode2.Text = "节点0";
            this.treeView_TestExecutorConfigItemDetails.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.treeView_TestExecutorConfigItemDetails.Size = new System.Drawing.Size(326, 345);
            this.treeView_TestExecutorConfigItemDetails.TabIndex = 9;
            // 
            // menu_windowSetting
            // 
            this.menu_windowSetting.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.窗口设置ToolStripMenuItem});
            this.menu_windowSetting.Location = new System.Drawing.Point(0, 0);
            this.menu_windowSetting.Name = "menu_windowSetting";
            this.menu_windowSetting.Size = new System.Drawing.Size(1134, 25);
            this.menu_windowSetting.TabIndex = 3;
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
            // Form_TestExecutorCombo_Builder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 617);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Controls.Add(this.menu_windowSetting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_TestExecutorCombo_Builder";
            this.Text = "Form_StepCombo";
            this.Load += new System.EventHandler(this.Form_StepCombo_Load);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.cms_ExecutorComboTreeView.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_localExecutors)).EndInit();
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
        private System.Windows.Forms.TreeView treeView_TestExecutorConfigItemDetails;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btn_SaveStepCombo;
        private System.Windows.Forms.Button btn_AddNewStepToMainCombo;
        private System.Windows.Forms.Button btn_MoveUpStepToCombo;
        private System.Windows.Forms.Button btn_MoveDownStepToCombo;
        private System.Windows.Forms.Button btn_ClearStepCombo;
        private System.Windows.Forms.Button btn_CreateNewStepCombo;
        private System.Windows.Forms.ListView lv_StepComboFiles;
        private System.Windows.Forms.TreeView treeView_TestExecutorCombo;
        private System.Windows.Forms.MenuStrip menu_windowSetting;
        private System.Windows.Forms.ToolStripMenuItem 窗口设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 浮动ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 还原ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cms_ExecutorComboTreeView;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Button btn_AddNewStepToPreCombo;
        private System.Windows.Forms.Button btn_AddNewStepToPostCombo;
    }
}