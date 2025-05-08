
namespace SolveWare_TesterCore
{
    partial class Form_changeModelName
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
            this.txt_ModelName = new System.Windows.Forms.TextBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_NoCopy = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txt_ModelName
            // 
            this.txt_ModelName.Location = new System.Drawing.Point(73, 91);
            this.txt_ModelName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_ModelName.Name = "txt_ModelName";
            this.txt_ModelName.Size = new System.Drawing.Size(407, 25);
            this.txt_ModelName.TabIndex = 0;
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(363, 148);
            this.btn_ok.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(119, 51);
            this.btn_ok.TabIndex = 1;
            this.btn_ok.Text = "确认";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_NoCopy
            // 
            this.btn_NoCopy.Location = new System.Drawing.Point(73, 148);
            this.btn_NoCopy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_NoCopy.Name = "btn_NoCopy";
            this.btn_NoCopy.Size = new System.Drawing.Size(119, 51);
            this.btn_NoCopy.TabIndex = 2;
            this.btn_NoCopy.Text = "不复制模块";
            this.btn_NoCopy.UseVisualStyleBackColor = true;
            this.btn_NoCopy.Click += new System.EventHandler(this.btn_NoCopy_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(70, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "复制后的模块名字为：";
            // 
            // Form_changeModelName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 238);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_NoCopy);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.txt_ModelName);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form_changeModelName";
            this.Text = "Form_changeModelName";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_changeModelName_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_ModelName;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_NoCopy;
        private System.Windows.Forms.Label label1;
    }
}