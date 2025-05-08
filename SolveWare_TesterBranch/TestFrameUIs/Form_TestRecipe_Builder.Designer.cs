
namespace SolveWare_TesterCore 
{
    partial class Form_TestRecipe_Builder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_TestRecipe_Builder));
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dgv_NewTestRec = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_AddNewTRec = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_CreateNewTRec = new System.Windows.Forms.Button();
            this.cmb_TestRecipeType = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgv_TestRecipe = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_SaveAs_TRecipe = new System.Windows.Forms.Button();
            this.btn_Save_TRecipe = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_TRecipeType = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmb_TestRecipeList = new System.Windows.Forms.ComboBox();
            this.btn_Del_TRecipe = new System.Windows.Forms.Button();
            this.lbl_CurrentTRec = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_NewTestRec)).BeginInit();
            this.panel2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TestRecipe)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.tableLayoutPanel2);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1172, 726);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "新建测试Recipe";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.dgv_NewTestRec, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1166, 720);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // dgv_NewTestRec
            // 
            this.dgv_NewTestRec.AllowUserToAddRows = false;
            this.dgv_NewTestRec.AllowUserToDeleteRows = false;
            this.dgv_NewTestRec.AllowUserToResizeRows = false;
            this.dgv_NewTestRec.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_NewTestRec.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.dgv_NewTestRec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_NewTestRec.Location = new System.Drawing.Point(3, 147);
            this.dgv_NewTestRec.Name = "dgv_NewTestRec";
            this.dgv_NewTestRec.RowHeadersVisible = false;
            this.dgv_NewTestRec.RowTemplate.Height = 23;
            this.dgv_NewTestRec.Size = new System.Drawing.Size(1160, 570);
            this.dgv_NewTestRec.TabIndex = 3;
            this.dgv_NewTestRec.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.Dgv_DataError);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.FillWeight = 50F;
            this.dataGridViewTextBoxColumn1.HeaderText = "说明";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.FillWeight = 30F;
            this.dataGridViewTextBoxColumn2.HeaderText = "名称";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.FillWeight = 20F;
            this.dataGridViewTextBoxColumn3.HeaderText = "设定值";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btn_AddNewTRec);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.btn_CreateNewTRec);
            this.panel2.Controls.Add(this.cmb_TestRecipeType);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1160, 138);
            this.panel2.TabIndex = 0;
            // 
            // btn_AddNewTRec
            // 
            this.btn_AddNewTRec.Location = new System.Drawing.Point(856, 44);
            this.btn_AddNewTRec.Name = "btn_AddNewTRec";
            this.btn_AddNewTRec.Size = new System.Drawing.Size(217, 32);
            this.btn_AddNewTRec.TabIndex = 0;
            this.btn_AddNewTRec.Text = "保存新建Recipe";
            this.btn_AddNewTRec.UseVisualStyleBackColor = true;
            this.btn_AddNewTRec.Click += new System.EventHandler(this.btn_AddNewTRec_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(77, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "类型选择：";
            // 
            // btn_CreateNewTRec
            // 
            this.btn_CreateNewTRec.Location = new System.Drawing.Point(596, 45);
            this.btn_CreateNewTRec.Name = "btn_CreateNewTRec";
            this.btn_CreateNewTRec.Size = new System.Drawing.Size(217, 32);
            this.btn_CreateNewTRec.TabIndex = 0;
            this.btn_CreateNewTRec.Text = "新建测试Recipe";
            this.btn_CreateNewTRec.UseVisualStyleBackColor = true;
            this.btn_CreateNewTRec.Click += new System.EventHandler(this.btn_CreateNewTRec_Click);
            // 
            // cmb_TestRecipeType
            // 
            this.cmb_TestRecipeType.FormattingEnabled = true;
            this.cmb_TestRecipeType.Location = new System.Drawing.Point(148, 51);
            this.cmb_TestRecipeType.Name = "cmb_TestRecipeType";
            this.cmb_TestRecipeType.Size = new System.Drawing.Size(372, 20);
            this.cmb_TestRecipeType.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1172, 726);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "测试Recipe编辑";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dgv_TestRecipe, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1166, 720);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // dgv_TestRecipe
            // 
            this.dgv_TestRecipe.AllowUserToAddRows = false;
            this.dgv_TestRecipe.AllowUserToDeleteRows = false;
            this.dgv_TestRecipe.AllowUserToResizeRows = false;
            this.dgv_TestRecipe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_TestRecipe.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dgv_TestRecipe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_TestRecipe.Location = new System.Drawing.Point(3, 147);
            this.dgv_TestRecipe.Name = "dgv_TestRecipe";
            this.dgv_TestRecipe.RowHeadersVisible = false;
            this.dgv_TestRecipe.RowTemplate.Height = 23;
            this.dgv_TestRecipe.Size = new System.Drawing.Size(1160, 570);
            this.dgv_TestRecipe.TabIndex = 0;
            this.dgv_TestRecipe.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.Dgv_DataError);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.FillWeight = 50F;
            this.Column1.HeaderText = "说明";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.FillWeight = 30F;
            this.Column2.HeaderText = "名称";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column3.FillWeight = 20F;
            this.Column3.HeaderText = "设定值";
            this.Column3.Name = "Column3";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_SaveAs_TRecipe);
            this.panel1.Controls.Add(this.btn_Save_TRecipe);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lbl_TRecipeType);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cmb_TestRecipeList);
            this.panel1.Controls.Add(this.btn_Del_TRecipe);
            this.panel1.Controls.Add(this.lbl_CurrentTRec);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1160, 138);
            this.panel1.TabIndex = 0;
            // 
            // btn_SaveAs_TRecipe
            // 
            this.btn_SaveAs_TRecipe.Location = new System.Drawing.Point(582, 83);
            this.btn_SaveAs_TRecipe.Name = "btn_SaveAs_TRecipe";
            this.btn_SaveAs_TRecipe.Size = new System.Drawing.Size(217, 32);
            this.btn_SaveAs_TRecipe.TabIndex = 12;
            this.btn_SaveAs_TRecipe.Text = "加载项另存";
            this.btn_SaveAs_TRecipe.UseVisualStyleBackColor = true;
            this.btn_SaveAs_TRecipe.Click += new System.EventHandler(this.btn_SaveAs_TRecipe_Click);
            // 
            // btn_Save_TRecipe
            // 
            this.btn_Save_TRecipe.Location = new System.Drawing.Point(582, 29);
            this.btn_Save_TRecipe.Name = "btn_Save_TRecipe";
            this.btn_Save_TRecipe.Size = new System.Drawing.Size(217, 32);
            this.btn_Save_TRecipe.TabIndex = 1;
            this.btn_Save_TRecipe.Text = "保存加载项";
            this.btn_Save_TRecipe.UseVisualStyleBackColor = true;
            this.btn_Save_TRecipe.Click += new System.EventHandler(this.btn_Save_TRecipe_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "测试Recipe：";
            // 
            // lbl_TRecipeType
            // 
            this.lbl_TRecipeType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_TRecipeType.Location = new System.Drawing.Point(338, 88);
            this.lbl_TRecipeType.Name = "lbl_TRecipeType";
            this.lbl_TRecipeType.Size = new System.Drawing.Size(219, 23);
            this.lbl_TRecipeType.TabIndex = 11;
            this.lbl_TRecipeType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(336, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 10;
            this.label10.Text = "当前加载项：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(336, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "加载项类型：";
            // 
            // cmb_TestRecipeList
            // 
            this.cmb_TestRecipeList.FormattingEnabled = true;
            this.cmb_TestRecipeList.Location = new System.Drawing.Point(29, 34);
            this.cmb_TestRecipeList.Name = "cmb_TestRecipeList";
            this.cmb_TestRecipeList.Size = new System.Drawing.Size(282, 20);
            this.cmb_TestRecipeList.TabIndex = 0;
            this.cmb_TestRecipeList.SelectionChangeCommitted += new System.EventHandler(this.cmb_TestRecipeList_SelectionChangeCommitted);
            // 
            // btn_Del_TRecipe
            // 
            this.btn_Del_TRecipe.Location = new System.Drawing.Point(843, 29);
            this.btn_Del_TRecipe.Name = "btn_Del_TRecipe";
            this.btn_Del_TRecipe.Size = new System.Drawing.Size(217, 32);
            this.btn_Del_TRecipe.TabIndex = 1;
            this.btn_Del_TRecipe.Text = "删除加载项";
            this.btn_Del_TRecipe.UseVisualStyleBackColor = true;
            this.btn_Del_TRecipe.Click += new System.EventHandler(this.btn_Del_TRecipe_Click);
            // 
            // lbl_CurrentTRec
            // 
            this.lbl_CurrentTRec.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_CurrentTRec.Location = new System.Drawing.Point(338, 34);
            this.lbl_CurrentTRec.Name = "lbl_CurrentTRec";
            this.lbl_CurrentTRec.Size = new System.Drawing.Size(219, 23);
            this.lbl_CurrentTRec.TabIndex = 11;
            this.lbl_CurrentTRec.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1180, 752);
            this.tabControl1.TabIndex = 4;
            // 
            // Form_TestRecipe_Builder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 752);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_TestRecipe_Builder";
            this.Text = "Form_TestRecipe";
            this.Load += new System.EventHandler(this.Form_TestRecipe_Load);
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_NewTestRec)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TestRecipe)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_AddNewTRec;
        private System.Windows.Forms.ComboBox cmb_TestRecipeType;
        private System.Windows.Forms.Button btn_CreateNewTRec;
        private System.Windows.Forms.DataGridView dgv_NewTestRec;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btn_SaveAs_TRecipe;
        private System.Windows.Forms.Label lbl_TRecipeType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_CurrentTRec;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_Del_TRecipe;
        private System.Windows.Forms.Button btn_Save_TRecipe;
        private System.Windows.Forms.ComboBox cmb_TestRecipeList;
        private System.Windows.Forms.DataGridView dgv_TestRecipe;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel2;
    }
}