using SolveWare_TestComponents.UI;

namespace SolveWare_BinSorter
{
    partial class Form_BinSorter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_BinSorter));
            this.contextMenuStrip_BinEditor = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmi_AddNewBinJudgeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmi_DeleteBinJudgeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_saveSpec = new System.Windows.Forms.Button();
            this.btn_copySpec = new System.Windows.Forms.Button();
            this.cb_BinToEdit = new System.Windows.Forms.ComboBox();
            this.btn_addSpec = new System.Windows.Forms.Button();
            this.btn_deleteSpec = new System.Windows.Forms.Button();
            this.pdgv_specEditor = new SolveWare_TestComponents.UI.PropertyDataGirdView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.contextMenuStrip_BinEditor.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pdgv_specEditor)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip_BinEditor
            // 
            this.contextMenuStrip_BinEditor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.tsmi_AddNewBinJudgeItem,
            this.toolStripSeparator2,
            this.tsmi_DeleteBinJudgeItem});
            this.contextMenuStrip_BinEditor.Name = "contextMenuStrip_BinEditor";
            this.contextMenuStrip_BinEditor.Size = new System.Drawing.Size(197, 60);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(193, 6);
            // 
            // tsmi_AddNewBinJudgeItem
            // 
            this.tsmi_AddNewBinJudgeItem.Name = "tsmi_AddNewBinJudgeItem";
            this.tsmi_AddNewBinJudgeItem.Size = new System.Drawing.Size(196, 22);
            this.tsmi_AddNewBinJudgeItem.Text = "添加单个空白判断项";
            this.tsmi_AddNewBinJudgeItem.Click += new System.EventHandler(this.tsmi_AddNewBinJudgeItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(193, 6);
            // 
            // tsmi_DeleteBinJudgeItem
            // 
            this.tsmi_DeleteBinJudgeItem.Name = "tsmi_DeleteBinJudgeItem";
            this.tsmi_DeleteBinJudgeItem.Size = new System.Drawing.Size(196, 22);
            this.tsmi_DeleteBinJudgeItem.Text = "删除当前选中的判断项";
            this.tsmi_DeleteBinJudgeItem.Click += new System.EventHandler(this.tsmi_DeleteBinJudgeItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btn_saveSpec);
            this.panel1.Controls.Add(this.btn_copySpec);
            this.panel1.Controls.Add(this.cb_BinToEdit);
            this.panel1.Controls.Add(this.btn_addSpec);
            this.panel1.Controls.Add(this.btn_deleteSpec);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(401, 372);
            this.panel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(17, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 14);
            this.label1.TabIndex = 4;
            this.label1.Text = "现有bin规格:";
            // 
            // btn_saveSpec
            // 
            this.btn_saveSpec.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold);
            this.btn_saveSpec.Location = new System.Drawing.Point(19, 117);
            this.btn_saveSpec.Name = "btn_saveSpec";
            this.btn_saveSpec.Size = new System.Drawing.Size(356, 25);
            this.btn_saveSpec.TabIndex = 3;
            this.btn_saveSpec.Text = "保存当前BIN设置";
            this.btn_saveSpec.UseVisualStyleBackColor = true;
            this.btn_saveSpec.Click += new System.EventHandler(this.btn_saveSpec_Click);
            // 
            // btn_copySpec
            // 
            this.btn_copySpec.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold);
            this.btn_copySpec.Location = new System.Drawing.Point(19, 80);
            this.btn_copySpec.Name = "btn_copySpec";
            this.btn_copySpec.Size = new System.Drawing.Size(356, 25);
            this.btn_copySpec.TabIndex = 3;
            this.btn_copySpec.Text = "复制当前BIN设置并新增";
            this.btn_copySpec.UseVisualStyleBackColor = true;
            this.btn_copySpec.Click += new System.EventHandler(this.btn_copySpec_Click);
            // 
            // cb_BinToEdit
            // 
            this.cb_BinToEdit.FormattingEnabled = true;
            this.cb_BinToEdit.Location = new System.Drawing.Point(19, 45);
            this.cb_BinToEdit.Name = "cb_BinToEdit";
            this.cb_BinToEdit.Size = new System.Drawing.Size(356, 20);
            this.cb_BinToEdit.TabIndex = 0;
            this.cb_BinToEdit.SelectionChangeCommitted += new System.EventHandler(this.cb_SpecToEdit_SelectionChangeCommitted);
            // 
            // btn_addSpec
            // 
            this.btn_addSpec.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold);
            this.btn_addSpec.Location = new System.Drawing.Point(20, 154);
            this.btn_addSpec.Name = "btn_addSpec";
            this.btn_addSpec.Size = new System.Drawing.Size(356, 25);
            this.btn_addSpec.TabIndex = 1;
            this.btn_addSpec.Text = "添加空白BIN设置";
            this.btn_addSpec.UseVisualStyleBackColor = true;
            this.btn_addSpec.Click += new System.EventHandler(this.btn_addSpec_Click);
            // 
            // btn_deleteSpec
            // 
            this.btn_deleteSpec.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold);
            this.btn_deleteSpec.Location = new System.Drawing.Point(19, 191);
            this.btn_deleteSpec.Name = "btn_deleteSpec";
            this.btn_deleteSpec.Size = new System.Drawing.Size(356, 25);
            this.btn_deleteSpec.TabIndex = 2;
            this.btn_deleteSpec.Text = "删除当前BIN设置";
            this.btn_deleteSpec.UseVisualStyleBackColor = true;
            this.btn_deleteSpec.Click += new System.EventHandler(this.btn_deleteSpec_Click);
            // 
            // pdgv_specEditor
            // 
            this.pdgv_specEditor.AllowUserToAddRows = false;
            this.pdgv_specEditor.AllowUserToDeleteRows = false;
            this.pdgv_specEditor.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Info;
            this.pdgv_specEditor.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.pdgv_specEditor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.pdgv_specEditor.ContextMenuStrip = this.contextMenuStrip_BinEditor;
            this.pdgv_specEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pdgv_specEditor.Location = new System.Drawing.Point(412, 4);
            this.pdgv_specEditor.Name = "pdgv_specEditor";
            this.pdgv_specEditor.RowHeadersVisible = false;
            this.pdgv_specEditor.RowTemplate.Height = 23;
            this.pdgv_specEditor.Size = new System.Drawing.Size(752, 372);
            this.pdgv_specEditor.TabIndex = 5;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pdgv_specEditor, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1168, 380);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // Form_BinSorter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1168, 380);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_BinSorter";
            this.Text = "Form_TestSpecManager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_TestSpecManager_FormClosing);
            this.Load += new System.EventHandler(this.Form_TestSpecManager_Load);
            this.contextMenuStrip_BinEditor.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pdgv_specEditor)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_BinEditor;
        private System.Windows.Forms.ToolStripMenuItem tsmi_AddNewBinJudgeItem;
        private System.Windows.Forms.ToolStripMenuItem tsmi_DeleteBinJudgeItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_saveSpec;
        private System.Windows.Forms.Button btn_copySpec;
        private System.Windows.Forms.ComboBox cb_BinToEdit;
        private System.Windows.Forms.Button btn_addSpec;
        private System.Windows.Forms.Button btn_deleteSpec;
        private PropertyDataGirdView pdgv_specEditor;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}