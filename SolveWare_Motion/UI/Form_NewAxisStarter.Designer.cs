namespace SolveWare_Motion 
{
    partial class Form_NewAxisStarter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_NewAxisStarter));
            this.tb_newAxisName = new System.Windows.Forms.TextBox();
            this.btn_confirmNewAxis = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.nud_CardNo = new System.Windows.Forms.NumericUpDown();
            this.nud_AxisNo = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_copyCurrentAxisConfig = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nud_CardNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_AxisNo)).BeginInit();
            this.SuspendLayout();
            // 
            // tb_newAxisName
            // 
            this.tb_newAxisName.Location = new System.Drawing.Point(74, 24);
            this.tb_newAxisName.Name = "tb_newAxisName";
            this.tb_newAxisName.Size = new System.Drawing.Size(270, 21);
            this.tb_newAxisName.TabIndex = 0;
            // 
            // btn_confirmNewAxis
            // 
            this.btn_confirmNewAxis.Location = new System.Drawing.Point(74, 140);
            this.btn_confirmNewAxis.Name = "btn_confirmNewAxis";
            this.btn_confirmNewAxis.Size = new System.Drawing.Size(118, 23);
            this.btn_confirmNewAxis.TabIndex = 1;
            this.btn_confirmNewAxis.Text = "确定";
            this.btn_confirmNewAxis.UseVisualStyleBackColor = true;
            this.btn_confirmNewAxis.Click += new System.EventHandler(this.btn_confirmNewAxis_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(224, 140);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(120, 23);
            this.btn_cancel.TabIndex = 1;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // nud_CardNo
            // 
            this.nud_CardNo.Location = new System.Drawing.Point(74, 62);
            this.nud_CardNo.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nud_CardNo.Name = "nud_CardNo";
            this.nud_CardNo.Size = new System.Drawing.Size(71, 21);
            this.nud_CardNo.TabIndex = 2;
            this.nud_CardNo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nud_AxisNo
            // 
            this.nud_AxisNo.Location = new System.Drawing.Point(74, 100);
            this.nud_AxisNo.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nud_AxisNo.Name = "nud_AxisNo";
            this.nud_AxisNo.Size = new System.Drawing.Size(71, 21);
            this.nud_AxisNo.TabIndex = 2;
            this.nud_AxisNo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "控制卡号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "轴号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "轴名称";
            // 
            // cb_copyCurrentAxisConfig
            // 
            this.cb_copyCurrentAxisConfig.AutoSize = true;
            this.cb_copyCurrentAxisConfig.Location = new System.Drawing.Point(180, 63);
            this.cb_copyCurrentAxisConfig.Name = "cb_copyCurrentAxisConfig";
            this.cb_copyCurrentAxisConfig.Size = new System.Drawing.Size(180, 16);
            this.cb_copyCurrentAxisConfig.TabIndex = 4;
            this.cb_copyCurrentAxisConfig.Text = "复制当前轴配置作为新轴配置";
            this.cb_copyCurrentAxisConfig.UseVisualStyleBackColor = true;
            // 
            // Form_NewAxisStarter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 190);
            this.Controls.Add(this.cb_copyCurrentAxisConfig);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nud_AxisNo);
            this.Controls.Add(this.nud_CardNo);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_confirmNewAxis);
            this.Controls.Add(this.tb_newAxisName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_NewAxisStarter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新增轴";
            this.Load += new System.EventHandler(this.Form_NewAxisStarter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nud_CardNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_AxisNo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_newAxisName;
        private System.Windows.Forms.Button btn_confirmNewAxis;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.NumericUpDown nud_CardNo;
        private System.Windows.Forms.NumericUpDown nud_AxisNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cb_copyCurrentAxisConfig;
    }
}