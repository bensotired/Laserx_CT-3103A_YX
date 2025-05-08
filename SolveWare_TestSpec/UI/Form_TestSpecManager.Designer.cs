using SolveWare_TestComponents.UI;

namespace SolveWare_TestSpecification
{
    partial class Form_TestSpecManager
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_TestSpecManager));
            this.menu_Motion = new System.Windows.Forms.MenuStrip();
            this.窗口设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.浮动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.还原ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_copySpec = new System.Windows.Forms.Button();
            this.btn_saveSpec = new System.Windows.Forms.Button();
            this.btn_deleteSpec = new System.Windows.Forms.Button();
            this.btn_deleteSpecItem = new System.Windows.Forms.Button();
            this.btn_addSpec = new System.Windows.Forms.Button();
            this.btn_addSpecItem = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cb_SpecToEdit = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pdgv_specEditor = new SolveWare_TestComponents.UI.PropertyDataGirdView();
            this.label1 = new System.Windows.Forms.Label();
            this.menu_Motion.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pdgv_specEditor)).BeginInit();
            this.SuspendLayout();
            // 
            // menu_Motion
            // 
            this.menu_Motion.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.窗口设置ToolStripMenuItem});
            this.menu_Motion.Location = new System.Drawing.Point(0, 0);
            this.menu_Motion.Name = "menu_Motion";
            this.menu_Motion.Size = new System.Drawing.Size(1164, 25);
            this.menu_Motion.TabIndex = 2;
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
            // btn_copySpec
            // 
            this.btn_copySpec.Location = new System.Drawing.Point(179, 60);
            this.btn_copySpec.Name = "btn_copySpec";
            this.btn_copySpec.Size = new System.Drawing.Size(216, 25);
            this.btn_copySpec.TabIndex = 3;
            this.btn_copySpec.Text = "复制当前测试规格并新增";
            this.btn_copySpec.UseVisualStyleBackColor = true;
            this.btn_copySpec.Click += new System.EventHandler(this.btn_copySpec_Click);
            // 
            // btn_saveSpec
            // 
            this.btn_saveSpec.Location = new System.Drawing.Point(470, 60);
            this.btn_saveSpec.Name = "btn_saveSpec";
            this.btn_saveSpec.Size = new System.Drawing.Size(216, 25);
            this.btn_saveSpec.TabIndex = 3;
            this.btn_saveSpec.Text = "保存当前测试规格";
            this.btn_saveSpec.UseVisualStyleBackColor = true;
            this.btn_saveSpec.Click += new System.EventHandler(this.btn_saveSpec_Click);
            // 
            // btn_deleteSpec
            // 
            this.btn_deleteSpec.Location = new System.Drawing.Point(777, 91);
            this.btn_deleteSpec.Name = "btn_deleteSpec";
            this.btn_deleteSpec.Size = new System.Drawing.Size(216, 25);
            this.btn_deleteSpec.TabIndex = 2;
            this.btn_deleteSpec.Text = "删除当前测试规格";
            this.btn_deleteSpec.UseVisualStyleBackColor = true;
            this.btn_deleteSpec.Click += new System.EventHandler(this.btn_deleteSpec_Click);
            // 
            // btn_deleteSpecItem
            // 
            this.btn_deleteSpecItem.Location = new System.Drawing.Point(777, 60);
            this.btn_deleteSpecItem.Name = "btn_deleteSpecItem";
            this.btn_deleteSpecItem.Size = new System.Drawing.Size(216, 25);
            this.btn_deleteSpecItem.TabIndex = 2;
            this.btn_deleteSpecItem.Text = "删除当前测试规格项";
            this.btn_deleteSpecItem.UseVisualStyleBackColor = true;
            this.btn_deleteSpecItem.Click += new System.EventHandler(this.btn_deleteSpecItem_Click);
            // 
            // btn_addSpec
            // 
            this.btn_addSpec.Location = new System.Drawing.Point(179, 91);
            this.btn_addSpec.Name = "btn_addSpec";
            this.btn_addSpec.Size = new System.Drawing.Size(216, 25);
            this.btn_addSpec.TabIndex = 1;
            this.btn_addSpec.Text = "添加空白测试规格";
            this.btn_addSpec.UseVisualStyleBackColor = true;
            this.btn_addSpec.Click += new System.EventHandler(this.btn_addSpec_Click);
            // 
            // btn_addSpecItem
            // 
            this.btn_addSpecItem.Location = new System.Drawing.Point(470, 91);
            this.btn_addSpecItem.Name = "btn_addSpecItem";
            this.btn_addSpecItem.Size = new System.Drawing.Size(216, 25);
            this.btn_addSpecItem.TabIndex = 1;
            this.btn_addSpecItem.Text = "添加空白测试规格项";
            this.btn_addSpecItem.UseVisualStyleBackColor = true;
            this.btn_addSpecItem.Click += new System.EventHandler(this.btn_addSpecItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btn_saveSpec);
            this.panel1.Controls.Add(this.btn_copySpec);
            this.panel1.Controls.Add(this.btn_deleteSpecItem);
            this.panel1.Controls.Add(this.cb_SpecToEdit);
            this.panel1.Controls.Add(this.btn_addSpecItem);
            this.panel1.Controls.Add(this.btn_addSpec);
            this.panel1.Controls.Add(this.btn_deleteSpec);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1156, 131);
            this.panel1.TabIndex = 2;
            // 
            // cb_SpecToEdit
            // 
            this.cb_SpecToEdit.FormattingEnabled = true;
            this.cb_SpecToEdit.Location = new System.Drawing.Point(179, 34);
            this.cb_SpecToEdit.Name = "cb_SpecToEdit";
            this.cb_SpecToEdit.Size = new System.Drawing.Size(814, 20);
            this.cb_SpecToEdit.TabIndex = 0;
            this.cb_SpecToEdit.SelectionChangeCommitted += new System.EventHandler(this.cb_SpecToEdit_SelectionChangeCommitted);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pdgv_specEditor, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1164, 688);
            this.tableLayoutPanel1.TabIndex = 3;
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
            this.pdgv_specEditor.Location = new System.Drawing.Point(4, 142);
            this.pdgv_specEditor.Name = "pdgv_specEditor";
            this.pdgv_specEditor.RowHeadersVisible = false;
            this.pdgv_specEditor.RowTemplate.Height = 23;
            this.pdgv_specEditor.Size = new System.Drawing.Size(1156, 542);
            this.pdgv_specEditor.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "现有测试规格:";
            // 
            // Form_TestSpecManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1164, 713);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menu_Motion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_TestSpecManager";
            this.Text = "Form_TestSpecManager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_TestSpecManager_FormClosing);
            this.Load += new System.EventHandler(this.Form_TestSpecManager_Load);
            this.menu_Motion.ResumeLayout(false);
            this.menu_Motion.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pdgv_specEditor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menu_Motion;
        private System.Windows.Forms.ToolStripMenuItem 窗口设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 浮动ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 还原ToolStripMenuItem;
        private System.Windows.Forms.Button btn_saveSpec;
        private System.Windows.Forms.Button btn_deleteSpecItem;
        private System.Windows.Forms.Button btn_addSpecItem;
        private PropertyDataGirdView pdgv_specEditor;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cb_SpecToEdit;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btn_addSpec;
        private System.Windows.Forms.Button btn_deleteSpec;
        private System.Windows.Forms.Button btn_copySpec;
        private System.Windows.Forms.Label label1;
    }
}