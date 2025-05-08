
namespace TestPlugin_Demo
{
    partial class Form_CheckIO
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
            this.bt_OK = new System.Windows.Forms.Button();
            this.bt_Cancel = new System.Windows.Forms.Button();
            this.bt_Yes = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 42F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(121, 151);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(517, 56);
            this.label1.TabIndex = 0;
            this.label1.Text = "*****************";
            // 
            // bt_OK
            // 
            this.bt_OK.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_OK.ForeColor = System.Drawing.Color.Green;
            this.bt_OK.Location = new System.Drawing.Point(131, 314);
            this.bt_OK.Name = "bt_OK";
            this.bt_OK.Size = new System.Drawing.Size(102, 59);
            this.bt_OK.TabIndex = 1;
            this.bt_OK.Text = "继续运行";
            this.bt_OK.UseVisualStyleBackColor = true;
            this.bt_OK.Click += new System.EventHandler(this.bt_OK_Click);
            // 
            // bt_Cancel
            // 
            this.bt_Cancel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_Cancel.ForeColor = System.Drawing.Color.DarkRed;
            this.bt_Cancel.Location = new System.Drawing.Point(536, 314);
            this.bt_Cancel.Name = "bt_Cancel";
            this.bt_Cancel.Size = new System.Drawing.Size(102, 59);
            this.bt_Cancel.TabIndex = 2;
            this.bt_Cancel.Text = "结束运行";
            this.bt_Cancel.UseVisualStyleBackColor = true;
            this.bt_Cancel.Click += new System.EventHandler(this.bt_Cancel_Click);
            // 
            // bt_Yes
            // 
            this.bt_Yes.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_Yes.ForeColor = System.Drawing.Color.Black;
            this.bt_Yes.Location = new System.Drawing.Point(337, 314);
            this.bt_Yes.Name = "bt_Yes";
            this.bt_Yes.Size = new System.Drawing.Size(102, 59);
            this.bt_Yes.TabIndex = 3;
            this.bt_Yes.Text = "重新检测";
            this.bt_Yes.UseVisualStyleBackColor = true;
            this.bt_Yes.Click += new System.EventHandler(this.bt_Yes_Click);
            // 
            // Form_CheckIO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.bt_Yes);
            this.Controls.Add(this.bt_Cancel);
            this.Controls.Add(this.bt_OK);
            this.Controls.Add(this.label1);
            this.Name = "Form_CheckIO";
            this.Text = "Form_CheckIO";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bt_OK;
        private System.Windows.Forms.Button bt_Cancel;
        private System.Windows.Forms.Button bt_Yes;
    }
}