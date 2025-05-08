namespace SolveWare_TesterCore
{
    partial class Form_NewExecutorConfigItem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_NewExecutorConfigItem));
            this.tb_newTestExecutorConfigItemName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_confirmNewTestExecutorConfigItem = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tb_newTestExecutorConfigItemName
            // 
            this.tb_newTestExecutorConfigItemName.Location = new System.Drawing.Point(111, 29);
            this.tb_newTestExecutorConfigItemName.Name = "tb_newTestExecutorConfigItemName";
            this.tb_newTestExecutorConfigItemName.Size = new System.Drawing.Size(287, 21);
            this.tb_newTestExecutorConfigItemName.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "测试项定义名";
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(278, 64);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(120, 22);
            this.btn_cancel.TabIndex = 3;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_confirmNewTestExecutorConfigItem
            // 
            this.btn_confirmNewTestExecutorConfigItem.Location = new System.Drawing.Point(111, 64);
            this.btn_confirmNewTestExecutorConfigItem.Name = "btn_confirmNewTestExecutorConfigItem";
            this.btn_confirmNewTestExecutorConfigItem.Size = new System.Drawing.Size(118, 22);
            this.btn_confirmNewTestExecutorConfigItem.TabIndex = 4;
            this.btn_confirmNewTestExecutorConfigItem.Text = "确定";
            this.btn_confirmNewTestExecutorConfigItem.UseVisualStyleBackColor = true;
            this.btn_confirmNewTestExecutorConfigItem.Click += new System.EventHandler(this.btn_confirmNewTestExecutorConfigItem_Click);
            // 
            // Form_NewExecutorConfigItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 120);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_confirmNewTestExecutorConfigItem);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_newTestExecutorConfigItemName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_NewExecutorConfigItem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新增测试项";
            this.Load += new System.EventHandler(this.Form_NewExecutorConfigItem_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tb_newTestExecutorConfigItemName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_confirmNewTestExecutorConfigItem;
    }
}