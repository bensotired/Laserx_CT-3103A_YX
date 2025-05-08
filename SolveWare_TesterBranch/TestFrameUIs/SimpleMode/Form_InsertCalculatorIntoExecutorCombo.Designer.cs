namespace SolveWare_TesterCore 
{
    partial class Form_InsertCalculatorIntoExecutorCombo
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgv_supportedCalculatorClass = new System.Windows.Forms.DataGridView();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_calculatorParamName = new System.Windows.Forms.TextBox();
            this.tb_calculatorTypeName = new System.Windows.Forms.TextBox();
            this.lbl_info = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_confirm = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_supportedCalculatorClass)).BeginInit();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dgv_supportedCalculatorClass, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbl_info, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(541, 798);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // dgv_supportedCalculatorClass
            // 
            this.dgv_supportedCalculatorClass.AllowUserToAddRows = false;
            this.dgv_supportedCalculatorClass.AllowUserToDeleteRows = false;
            this.dgv_supportedCalculatorClass.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_supportedCalculatorClass.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column10});
            this.dgv_supportedCalculatorClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_supportedCalculatorClass.Location = new System.Drawing.Point(3, 42);
            this.dgv_supportedCalculatorClass.MultiSelect = false;
            this.dgv_supportedCalculatorClass.Name = "dgv_supportedCalculatorClass";
            this.dgv_supportedCalculatorClass.ReadOnly = true;
            this.dgv_supportedCalculatorClass.RowHeadersVisible = false;
            this.dgv_supportedCalculatorClass.RowTemplate.Height = 23;
            this.dgv_supportedCalculatorClass.Size = new System.Drawing.Size(535, 592);
            this.dgv_supportedCalculatorClass.TabIndex = 12;
            this.dgv_supportedCalculatorClass.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_supportedCalculatorClass_CellClick);
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
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.tb_calculatorParamName);
            this.panel1.Controls.Add(this.tb_calculatorTypeName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 640);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(535, 113);
            this.panel1.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(9, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "算子参数名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(9, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "算子类型";
            // 
            // tb_calculatorParamName
            // 
            this.tb_calculatorParamName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_calculatorParamName.Location = new System.Drawing.Point(121, 67);
            this.tb_calculatorParamName.Name = "tb_calculatorParamName";
            this.tb_calculatorParamName.Size = new System.Drawing.Size(396, 26);
            this.tb_calculatorParamName.TabIndex = 0;
            // 
            // tb_calculatorTypeName
            // 
            this.tb_calculatorTypeName.Enabled = false;
            this.tb_calculatorTypeName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_calculatorTypeName.Location = new System.Drawing.Point(121, 23);
            this.tb_calculatorTypeName.Name = "tb_calculatorTypeName";
            this.tb_calculatorTypeName.Size = new System.Drawing.Size(396, 26);
            this.tb_calculatorTypeName.TabIndex = 0;
            // 
            // lbl_info
            // 
            this.lbl_info.AutoSize = true;
            this.lbl_info.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_info.Location = new System.Drawing.Point(3, 0);
            this.lbl_info.Name = "lbl_info";
            this.lbl_info.Size = new System.Drawing.Size(155, 16);
            this.lbl_info.TabIndex = 14;
            this.lbl_info.Text = "当前测试模块[- -]";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel2.Controls.Add(this.btn_confirm, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btn_cancel, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 759);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(535, 36);
            this.tableLayoutPanel2.TabIndex = 15;
            // 
            // btn_confirm
            // 
            this.btn_confirm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_confirm.Location = new System.Drawing.Point(3, 3);
            this.btn_confirm.Name = "btn_confirm";
            this.btn_confirm.Size = new System.Drawing.Size(208, 30);
            this.btn_confirm.TabIndex = 11;
            this.btn_confirm.Text = "确定";
            this.btn_confirm.UseVisualStyleBackColor = true;
            this.btn_confirm.Click += new System.EventHandler(this.btn_confirm_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_cancel.Location = new System.Drawing.Point(324, 3);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(208, 30);
            this.btn_cancel.TabIndex = 11;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // Form_InsertCalculatorIntoExecutorCombo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 798);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_InsertCalculatorIntoExecutorCombo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加算子操作";
            this.Load += new System.EventHandler(this.Form_TestExecutorItem_Preview_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_supportedCalculatorClass)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.DataGridView dgv_supportedCalculatorClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_info;
        private System.Windows.Forms.TextBox tb_calculatorTypeName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_calculatorParamName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btn_confirm;
    }
}