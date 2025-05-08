namespace SolveWare_Analog
{
    partial class Form_New_IP_AnalogStarter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_New_IP_AnalogStarter));
            this.tb_newAxisName = new System.Windows.Forms.TextBox();
            this.btn_confirmNewAxis = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.nud_CardNo = new System.Windows.Forms.NumericUpDown();
            this.nud_ioNo = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_externIOCard = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.nud_ioSlave = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nud_CardNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ioNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ioSlave)).BeginInit();
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
            this.btn_confirmNewAxis.Location = new System.Drawing.Point(58, 208);
            this.btn_confirmNewAxis.Name = "btn_confirmNewAxis";
            this.btn_confirmNewAxis.Size = new System.Drawing.Size(118, 23);
            this.btn_confirmNewAxis.TabIndex = 1;
            this.btn_confirmNewAxis.Text = "确定";
            this.btn_confirmNewAxis.UseVisualStyleBackColor = true;
            this.btn_confirmNewAxis.Click += new System.EventHandler(this.btn_confirmNewAxis_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(208, 208);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(120, 23);
            this.btn_cancel.TabIndex = 1;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // nud_CardNo
            // 
            this.nud_CardNo.Location = new System.Drawing.Point(74, 69);
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
            // nud_ioNo
            // 
            this.nud_ioNo.Location = new System.Drawing.Point(74, 164);
            this.nud_ioNo.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.nud_ioNo.Name = "nud_ioNo";
            this.nud_ioNo.Size = new System.Drawing.Size(71, 21);
            this.nud_ioNo.TabIndex = 2;
            this.nud_ioNo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "控制卡号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 166);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "端口号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "ADC名称";
            // 
            // cb_externIOCard
            // 
            this.cb_externIOCard.AutoSize = true;
            this.cb_externIOCard.Location = new System.Drawing.Point(247, 69);
            this.cb_externIOCard.Name = "cb_externIOCard";
            this.cb_externIOCard.Size = new System.Drawing.Size(72, 16);
            this.cb_externIOCard.TabIndex = 6;
            this.cb_externIOCard.Text = "扩展卡IO";
            this.cb_externIOCard.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "分卡号";
            // 
            // nud_ioSlave
            // 
            this.nud_ioSlave.Location = new System.Drawing.Point(74, 115);
            this.nud_ioSlave.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nud_ioSlave.Name = "nud_ioSlave";
            this.nud_ioSlave.Size = new System.Drawing.Size(71, 21);
            this.nud_ioSlave.TabIndex = 10;
            this.nud_ioSlave.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Form_New_IP_AnalogStarter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 250);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nud_ioSlave);
            this.Controls.Add(this.cb_externIOCard);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nud_ioNo);
            this.Controls.Add(this.nud_CardNo);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_confirmNewAxis);
            this.Controls.Add(this.tb_newAxisName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_New_IP_AnalogStarter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新增输入ADC";
            this.Load += new System.EventHandler(this.Form_NewAxisStarter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nud_CardNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ioNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ioSlave)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_newAxisName;
        private System.Windows.Forms.Button btn_confirmNewAxis;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.NumericUpDown nud_CardNo;
        private System.Windows.Forms.NumericUpDown nud_ioNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cb_externIOCard;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nud_ioSlave;
    }
}