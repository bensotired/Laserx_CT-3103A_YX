namespace SolveWare_TesterCore
{
    partial class Form_NewExecutorCombo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_NewExecutorCombo));
            this.tb_newTestExecutorComboName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_confirmNewTestExecutorConfigItem = new System.Windows.Forms.Button();
            this.cb_testPluginTypes = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tb_newTestExecutorComboName
            // 
            this.tb_newTestExecutorComboName.Location = new System.Drawing.Point(142, 29);
            this.tb_newTestExecutorComboName.Name = "tb_newTestExecutorComboName";
            this.tb_newTestExecutorComboName.Size = new System.Drawing.Size(256, 21);
            this.tb_newTestExecutorComboName.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "测试链定义名";
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(278, 116);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(120, 22);
            this.btn_cancel.TabIndex = 3;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_confirmNewTestExecutorConfigItem
            // 
            this.btn_confirmNewTestExecutorConfigItem.Location = new System.Drawing.Point(142, 116);
            this.btn_confirmNewTestExecutorConfigItem.Name = "btn_confirmNewTestExecutorConfigItem";
            this.btn_confirmNewTestExecutorConfigItem.Size = new System.Drawing.Size(118, 22);
            this.btn_confirmNewTestExecutorConfigItem.TabIndex = 4;
            this.btn_confirmNewTestExecutorConfigItem.Text = "确定";
            this.btn_confirmNewTestExecutorConfigItem.UseVisualStyleBackColor = true;
            this.btn_confirmNewTestExecutorConfigItem.Click += new System.EventHandler(this.btn_confirmNewTestExecutorConfigItem_Click);
            // 
            // cb_testPluginTypes
            // 
            this.cb_testPluginTypes.FormattingEnabled = true;
            this.cb_testPluginTypes.Location = new System.Drawing.Point(142, 70);
            this.cb_testPluginTypes.Name = "cb_testPluginTypes";
            this.cb_testPluginTypes.Size = new System.Drawing.Size(256, 20);
            this.cb_testPluginTypes.TabIndex = 5;
            this.cb_testPluginTypes.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "测试链适用组件类型";
            this.label1.Visible = false;
            // 
            // Form_NewExecutorCombo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 165);
            this.Controls.Add(this.cb_testPluginTypes);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_confirmNewTestExecutorConfigItem);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_newTestExecutorComboName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_NewExecutorCombo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新增测试链";
            this.Load += new System.EventHandler(this.Form_NewExecutorConfigItem_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tb_newTestExecutorComboName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_confirmNewTestExecutorConfigItem;
        private System.Windows.Forms.ComboBox cb_testPluginTypes;
        private System.Windows.Forms.Label label1;
    }
}