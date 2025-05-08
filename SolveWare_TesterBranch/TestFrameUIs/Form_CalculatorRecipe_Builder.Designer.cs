
namespace SolveWare_TesterCore 
{
    partial class Form_CalculatorRecipe_Builder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_CalculatorRecipe_Builder));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgv_CalculateRecipe = new System.Windows.Forms.DataGridView();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_SaveAs_CRecipe = new System.Windows.Forms.Button();
            this.btn_Del_CRecipe = new System.Windows.Forms.Button();
            this.lbl_CRecipeType = new System.Windows.Forms.Label();
            this.cmb_CalculateRecipeList = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_Save_CRecipe = new System.Windows.Forms.Button();
            this.lbl_CurrentCRec = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dgv_NewCalcRec = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_CreateNewCRec = new System.Windows.Forms.Button();
            this.btn_AddNewCRec = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cmb_CalcRecipeType = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_CalculateRecipe)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_NewCalcRec)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1180, 752);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tableLayoutPanel1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1172, 726);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "计算Recipe编辑";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dgv_CalculateRecipe, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1172, 726);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // dgv_CalculateRecipe
            // 
            this.dgv_CalculateRecipe.AllowUserToAddRows = false;
            this.dgv_CalculateRecipe.AllowUserToDeleteRows = false;
            this.dgv_CalculateRecipe.AllowUserToResizeColumns = false;
            this.dgv_CalculateRecipe.AllowUserToResizeRows = false;
            this.dgv_CalculateRecipe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_CalculateRecipe.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column4,
            this.Column5,
            this.Column6});
            this.dgv_CalculateRecipe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_CalculateRecipe.Location = new System.Drawing.Point(3, 148);
            this.dgv_CalculateRecipe.Name = "dgv_CalculateRecipe";
            this.dgv_CalculateRecipe.RowHeadersVisible = false;
            this.dgv_CalculateRecipe.RowTemplate.Height = 23;
            this.dgv_CalculateRecipe.Size = new System.Drawing.Size(1166, 575);
            this.dgv_CalculateRecipe.TabIndex = 0;
            this.dgv_CalculateRecipe.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.Dgv_DataError);
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column4.FillWeight = 50F;
            this.Column4.HeaderText = "说明";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column5.FillWeight = 30F;
            this.Column5.HeaderText = "名称";
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column6.FillWeight = 20F;
            this.Column6.HeaderText = "设定值";
            this.Column6.Name = "Column6";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_SaveAs_CRecipe);
            this.panel1.Controls.Add(this.btn_Del_CRecipe);
            this.panel1.Controls.Add(this.lbl_CRecipeType);
            this.panel1.Controls.Add(this.cmb_CalculateRecipeList);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btn_Save_CRecipe);
            this.panel1.Controls.Add(this.lbl_CurrentCRec);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1166, 139);
            this.panel1.TabIndex = 0;
            // 
            // btn_SaveAs_CRecipe
            // 
            this.btn_SaveAs_CRecipe.Location = new System.Drawing.Point(618, 85);
            this.btn_SaveAs_CRecipe.Name = "btn_SaveAs_CRecipe";
            this.btn_SaveAs_CRecipe.Size = new System.Drawing.Size(217, 32);
            this.btn_SaveAs_CRecipe.TabIndex = 14;
            this.btn_SaveAs_CRecipe.Text = "加载项另存";
            this.btn_SaveAs_CRecipe.UseVisualStyleBackColor = true;
            this.btn_SaveAs_CRecipe.Click += new System.EventHandler(this.btn_SaveAs_CRecipe_Click);
            // 
            // btn_Del_CRecipe
            // 
            this.btn_Del_CRecipe.Location = new System.Drawing.Point(903, 33);
            this.btn_Del_CRecipe.Name = "btn_Del_CRecipe";
            this.btn_Del_CRecipe.Size = new System.Drawing.Size(217, 32);
            this.btn_Del_CRecipe.TabIndex = 10;
            this.btn_Del_CRecipe.Text = "删除加载项";
            this.btn_Del_CRecipe.UseVisualStyleBackColor = true;
            this.btn_Del_CRecipe.Click += new System.EventHandler(this.btn_Del_CRecipe_Click);
            // 
            // lbl_CRecipeType
            // 
            this.lbl_CRecipeType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_CRecipeType.Location = new System.Drawing.Point(358, 90);
            this.lbl_CRecipeType.Name = "lbl_CRecipeType";
            this.lbl_CRecipeType.Size = new System.Drawing.Size(219, 23);
            this.lbl_CRecipeType.TabIndex = 13;
            this.lbl_CRecipeType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmb_CalculateRecipeList
            // 
            this.cmb_CalculateRecipeList.FormattingEnabled = true;
            this.cmb_CalculateRecipeList.Location = new System.Drawing.Point(30, 41);
            this.cmb_CalculateRecipeList.Name = "cmb_CalculateRecipeList";
            this.cmb_CalculateRecipeList.Size = new System.Drawing.Size(282, 20);
            this.cmb_CalculateRecipeList.TabIndex = 3;
            this.cmb_CalculateRecipeList.SelectionChangeCommitted += new System.EventHandler(this.cmb_CalculateRecipe_SelectionChangeCommitted);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(356, 78);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 12);
            this.label9.TabIndex = 12;
            this.label9.Text = "加载项类型：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "计算Recipe：";
            // 
            // btn_Save_CRecipe
            // 
            this.btn_Save_CRecipe.Location = new System.Drawing.Point(618, 33);
            this.btn_Save_CRecipe.Name = "btn_Save_CRecipe";
            this.btn_Save_CRecipe.Size = new System.Drawing.Size(217, 32);
            this.btn_Save_CRecipe.TabIndex = 7;
            this.btn_Save_CRecipe.Text = "保存文件";
            this.btn_Save_CRecipe.UseVisualStyleBackColor = true;
            this.btn_Save_CRecipe.Click += new System.EventHandler(this.btn_Save_CRecipe_Click);
            // 
            // lbl_CurrentCRec
            // 
            this.lbl_CurrentCRec.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_CurrentCRec.Location = new System.Drawing.Point(358, 38);
            this.lbl_CurrentCRec.Name = "lbl_CurrentCRec";
            this.lbl_CurrentCRec.Size = new System.Drawing.Size(219, 23);
            this.lbl_CurrentCRec.TabIndex = 9;
            this.lbl_CurrentCRec.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(356, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "当前加载项：";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.tableLayoutPanel2);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(1172, 726);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "新建计算Recipe";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.dgv_NewCalcRec, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1172, 726);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // dgv_NewCalcRec
            // 
            this.dgv_NewCalcRec.AllowUserToAddRows = false;
            this.dgv_NewCalcRec.AllowUserToDeleteRows = false;
            this.dgv_NewCalcRec.AllowUserToResizeRows = false;
            this.dgv_NewCalcRec.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_NewCalcRec.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6});
            this.dgv_NewCalcRec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_NewCalcRec.Location = new System.Drawing.Point(3, 148);
            this.dgv_NewCalcRec.Name = "dgv_NewCalcRec";
            this.dgv_NewCalcRec.RowHeadersVisible = false;
            this.dgv_NewCalcRec.RowTemplate.Height = 23;
            this.dgv_NewCalcRec.Size = new System.Drawing.Size(1166, 575);
            this.dgv_NewCalcRec.TabIndex = 3;
            this.dgv_NewCalcRec.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.Dgv_DataError);
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.FillWeight = 50F;
            this.dataGridViewTextBoxColumn4.HeaderText = "说明";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.FillWeight = 30F;
            this.dataGridViewTextBoxColumn5.HeaderText = "名称";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.FillWeight = 20F;
            this.dataGridViewTextBoxColumn6.HeaderText = "设定值";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btn_CreateNewCRec);
            this.panel2.Controls.Add(this.btn_AddNewCRec);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.cmb_CalcRecipeType);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1166, 139);
            this.panel2.TabIndex = 0;
            // 
            // btn_CreateNewCRec
            // 
            this.btn_CreateNewCRec.Location = new System.Drawing.Point(535, 48);
            this.btn_CreateNewCRec.Name = "btn_CreateNewCRec";
            this.btn_CreateNewCRec.Size = new System.Drawing.Size(217, 32);
            this.btn_CreateNewCRec.TabIndex = 0;
            this.btn_CreateNewCRec.Text = "新建计算Recipe";
            this.btn_CreateNewCRec.UseVisualStyleBackColor = true;
            this.btn_CreateNewCRec.Click += new System.EventHandler(this.btn_CreateNewCRec_Click);
            // 
            // btn_AddNewCRec
            // 
            this.btn_AddNewCRec.Location = new System.Drawing.Point(811, 48);
            this.btn_AddNewCRec.Name = "btn_AddNewCRec";
            this.btn_AddNewCRec.Size = new System.Drawing.Size(217, 32);
            this.btn_AddNewCRec.TabIndex = 0;
            this.btn_AddNewCRec.Text = "保存新建Recipe";
            this.btn_AddNewCRec.UseVisualStyleBackColor = true;
            this.btn_AddNewCRec.Click += new System.EventHandler(this.btn_AddNewCRec_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "类型选择:";
            // 
            // cmb_CalcRecipeType
            // 
            this.cmb_CalcRecipeType.FormattingEnabled = true;
            this.cmb_CalcRecipeType.Location = new System.Drawing.Point(88, 54);
            this.cmb_CalcRecipeType.Name = "cmb_CalcRecipeType";
            this.cmb_CalcRecipeType.Size = new System.Drawing.Size(371, 20);
            this.cmb_CalcRecipeType.TabIndex = 1;
            // 
            // Form_CalculatorRecipe_Builder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 752);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_CalculatorRecipe_Builder";
            this.Text = "Form_CalculatorRecipe";
            this.Load += new System.EventHandler(this.Form_CalculatorRecipe_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_CalculateRecipe)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_NewCalcRec)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btn_SaveAs_CRecipe;
        private System.Windows.Forms.Label lbl_CRecipeType;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btn_Del_CRecipe;
        private System.Windows.Forms.Label lbl_CurrentCRec;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_Save_CRecipe;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmb_CalculateRecipeList;
        private System.Windows.Forms.DataGridView dgv_CalculateRecipe;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_AddNewCRec;
        private System.Windows.Forms.ComboBox cmb_CalcRecipeType;
        private System.Windows.Forms.Button btn_CreateNewCRec;
        private System.Windows.Forms.DataGridView dgv_NewCalcRec;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel2;
    }
}