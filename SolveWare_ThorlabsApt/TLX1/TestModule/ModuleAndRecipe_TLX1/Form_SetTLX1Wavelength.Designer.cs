
namespace SolveWare_BurnInInstruments.TLX1.TestModule
{
    partial class Form_SetTLX1Wavelength
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
            this.label1 = new System.Windows.Forms.Label();
            this.txt_power = new System.Windows.Forms.TextBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(96, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(235, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "请输入可调激光器(TLX1)的功率：";
            // 
            // txt_power
            // 
            this.txt_power.Location = new System.Drawing.Point(99, 94);
            this.txt_power.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_power.Name = "txt_power";
            this.txt_power.Size = new System.Drawing.Size(165, 25);
            this.txt_power.TabIndex = 1;
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(132, 152);
            this.btn_ok.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(92, 39);
            this.btn_ok.TabIndex = 2;
            this.btn_ok.Text = "确认";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // Form_SetTLX1Wavelength
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 308);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.txt_power);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form_SetTLX1Wavelength";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form_SetTLX1Wavelength";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_power;
        private System.Windows.Forms.Button btn_ok;
    }
}