namespace SolveWare_TestSpecification
{
    partial class Form_TestSpecInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_TestSpecInfo));
            this.txt_testSpecName = new System.Windows.Forms.TextBox();
            this.btn_confirmNewSpec = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_testSpecVer = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txt_testSpecName
            // 
            this.txt_testSpecName.Location = new System.Drawing.Point(74, 39);
            this.txt_testSpecName.Name = "txt_testSpecName";
            this.txt_testSpecName.Size = new System.Drawing.Size(270, 21);
            this.txt_testSpecName.TabIndex = 0;
            // 
            // btn_confirmNewSpec
            // 
            this.btn_confirmNewSpec.Location = new System.Drawing.Point(74, 140);
            this.btn_confirmNewSpec.Name = "btn_confirmNewSpec";
            this.btn_confirmNewSpec.Size = new System.Drawing.Size(118, 23);
            this.btn_confirmNewSpec.TabIndex = 1;
            this.btn_confirmNewSpec.Text = "确定";
            this.btn_confirmNewSpec.UseVisualStyleBackColor = true;
            this.btn_confirmNewSpec.Click += new System.EventHandler(this.btn_confirmNewSpec_Click);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "规格版本";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "规格名称";
            // 
            // txt_testSpecVer
            // 
            this.txt_testSpecVer.Location = new System.Drawing.Point(74, 83);
            this.txt_testSpecVer.Name = "txt_testSpecVer";
            this.txt_testSpecVer.Size = new System.Drawing.Size(270, 21);
            this.txt_testSpecVer.TabIndex = 0;
            // 
            // Form_TestSpecInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 190);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_confirmNewSpec);
            this.Controls.Add(this.txt_testSpecVer);
            this.Controls.Add(this.txt_testSpecName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_TestSpecInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新增测试规格";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_testSpecName;
        private System.Windows.Forms.Button btn_confirmNewSpec;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_testSpecVer;
    }
}