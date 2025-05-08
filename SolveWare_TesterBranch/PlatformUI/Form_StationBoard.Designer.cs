namespace SolveWare_TesterCore
{
    partial class Form_StationBoard
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_StationBoard));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgv_Inst = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgv_InstChas = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.menu_Motion = new System.Windows.Forms.MenuStrip();
            this.窗口设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.浮动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.还原ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Inst)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_InstChas)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.menu_Motion.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1321, 782);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // dgv_Inst
            // 
            this.dgv_Inst.AllowUserToAddRows = false;
            this.dgv_Inst.AllowUserToDeleteRows = false;
            this.dgv_Inst.AllowUserToResizeColumns = false;
            this.dgv_Inst.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Info;
            this.dgv_Inst.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_Inst.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Inst.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column6,
            this.Column2,
            this.Column3,
            this.Column8});
            this.dgv_Inst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_Inst.Location = new System.Drawing.Point(3, 17);
            this.dgv_Inst.Name = "dgv_Inst";
            this.dgv_Inst.ReadOnly = true;
            this.dgv_Inst.RowHeadersVisible = false;
            this.dgv_Inst.RowTemplate.Height = 23;
            this.dgv_Inst.Size = new System.Drawing.Size(1307, 325);
            this.dgv_Inst.TabIndex = 1;
            this.dgv_Inst.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.Dgv_DataError);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.FillWeight = 25F;
            this.Column1.HeaderText = "名称";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column6.FillWeight = 5F;
            this.Column6.HeaderText = "地址";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.FillWeight = 5F;
            this.Column2.HeaderText = "在线";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column3.FillWeight = 20F;
            this.Column3.HeaderText = "通讯底层";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column8
            // 
            this.Column8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column8.FillWeight = 20F;
            this.Column8.HeaderText = "类型";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgv_InstChas);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(4, 82);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1313, 344);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "硬件通讯底层设置";
            // 
            // dgv_InstChas
            // 
            this.dgv_InstChas.AllowUserToAddRows = false;
            this.dgv_InstChas.AllowUserToDeleteRows = false;
            this.dgv_InstChas.AllowUserToResizeColumns = false;
            this.dgv_InstChas.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Info;
            this.dgv_InstChas.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_InstChas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_InstChas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.Column4,
            this.Column5,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.dgv_InstChas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_InstChas.Location = new System.Drawing.Point(3, 17);
            this.dgv_InstChas.Name = "dgv_InstChas";
            this.dgv_InstChas.ReadOnly = true;
            this.dgv_InstChas.RowHeadersVisible = false;
            this.dgv_InstChas.RowTemplate.Height = 23;
            this.dgv_InstChas.Size = new System.Drawing.Size(1307, 324);
            this.dgv_InstChas.TabIndex = 1;
            this.dgv_InstChas.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.Dgv_DataError);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.FillWeight = 25F;
            this.dataGridViewTextBoxColumn1.HeaderText = "名称";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column4.FillWeight = 25F;
            this.Column4.HeaderText = "资源";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column5.FillWeight = 10F;
            this.Column5.HeaderText = "在线";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.FillWeight = 10F;
            this.dataGridViewTextBoxColumn2.HeaderText = "超时(ms)";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.FillWeight = 30F;
            this.dataGridViewTextBoxColumn3.HeaderText = "类别";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgv_Inst);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(4, 433);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1313, 345);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "硬件设置";
            // 
            // menu_Motion
            // 
            this.menu_Motion.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.窗口设置ToolStripMenuItem});
            this.menu_Motion.Location = new System.Drawing.Point(0, 0);
            this.menu_Motion.Name = "menu_Motion";
            this.menu_Motion.Size = new System.Drawing.Size(1321, 25);
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
            // Form_StationBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1321, 807);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menu_Motion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "Form_StationBoard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "运行信息";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_MessageBorad_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Inst)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_InstChas)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.menu_Motion.ResumeLayout(false);
            this.menu_Motion.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dgv_Inst;
        private System.Windows.Forms.DataGridView dgv_InstChas;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.MenuStrip menu_Motion;
        private System.Windows.Forms.ToolStripMenuItem 窗口设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 浮动ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 还原ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
    }
}