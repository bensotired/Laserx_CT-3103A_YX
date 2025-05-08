namespace SolveWare_TesterCore
{
    partial class Form_CreateNewProductConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_CreateNewProductConfig));
            this.txt_newName = new System.Windows.Forms.TextBox();
            this.btn_confirmNewFile = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txt_newName
            // 
            this.txt_newName.Location = new System.Drawing.Point(97, 31);
            this.txt_newName.Name = "txt_newName";
            this.txt_newName.Size = new System.Drawing.Size(221, 21);
            this.txt_newName.TabIndex = 0;
            // 
            // btn_confirmNewFile
            // 
            this.btn_confirmNewFile.Location = new System.Drawing.Point(97, 74);
            this.btn_confirmNewFile.Name = "btn_confirmNewFile";
            this.btn_confirmNewFile.Size = new System.Drawing.Size(82, 23);
            this.btn_confirmNewFile.TabIndex = 1;
            this.btn_confirmNewFile.Text = "确定";
            this.btn_confirmNewFile.UseVisualStyleBackColor = true;
            this.btn_confirmNewFile.Click += new System.EventHandler(this.btn_confirmNewFile_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(236, 74);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(82, 23);
            this.btn_cancel.TabIndex = 1;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "产品名：";
            // 
            // Form_CreateNewProductConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 126);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_confirmNewFile);
            this.Controls.Add(this.txt_newName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_CreateNewProductConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新增产品配置名称";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_newName;
        private System.Windows.Forms.Button btn_confirmNewFile;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Label label3;
    }
}