namespace SolveWare_Permission
{
    partial class Form_EngPermissionVerify
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_EngPermissionVerify));
            this.cb_user = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_inpPw = new System.Windows.Forms.TextBox();
            this.btn_UserLogout = new System.Windows.Forms.Button();
            this.lbl_alert = new System.Windows.Forms.Label();
            this.tb_userID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_UserLogin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cb_user
            // 
            this.cb_user.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_user.FormattingEnabled = true;
            this.cb_user.Items.AddRange(new object[] {
            "Admin",
            "Engineer",
            "Operator"});
            this.cb_user.Location = new System.Drawing.Point(11, 22);
            this.cb_user.Margin = new System.Windows.Forms.Padding(2);
            this.cb_user.Name = "cb_user";
            this.cb_user.Size = new System.Drawing.Size(280, 20);
            this.cb_user.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "用户组";
            // 
            // tb_inpPw
            // 
            this.tb_inpPw.Location = new System.Drawing.Point(11, 122);
            this.tb_inpPw.Margin = new System.Windows.Forms.Padding(2);
            this.tb_inpPw.Name = "tb_inpPw";
            this.tb_inpPw.PasswordChar = '*';
            this.tb_inpPw.Size = new System.Drawing.Size(280, 21);
            this.tb_inpPw.TabIndex = 0;
            this.tb_inpPw.TextChanged += new System.EventHandler(this.tb_inpPw_TextChanged);
            this.tb_inpPw.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_inpPw_KeyPress);
            // 
            // btn_UserLogout
            // 
            this.btn_UserLogout.Location = new System.Drawing.Point(163, 174);
            this.btn_UserLogout.Margin = new System.Windows.Forms.Padding(2);
            this.btn_UserLogout.Name = "btn_UserLogout";
            this.btn_UserLogout.Size = new System.Drawing.Size(127, 35);
            this.btn_UserLogout.TabIndex = 2;
            this.btn_UserLogout.Text = "登出";
            this.btn_UserLogout.UseVisualStyleBackColor = true;
            this.btn_UserLogout.Click += new System.EventHandler(this.btn_UserLogout_Click);
            // 
            // lbl_alert
            // 
            this.lbl_alert.AutoSize = true;
            this.lbl_alert.Location = new System.Drawing.Point(9, 145);
            this.lbl_alert.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_alert.Name = "lbl_alert";
            this.lbl_alert.Size = new System.Drawing.Size(35, 12);
            this.lbl_alert.TabIndex = 4;
            this.lbl_alert.Text = "Info:";
            // 
            // tb_userID
            // 
            this.tb_userID.Location = new System.Drawing.Point(10, 70);
            this.tb_userID.Margin = new System.Windows.Forms.Padding(2);
            this.tb_userID.Name = "tb_userID";
            this.tb_userID.Size = new System.Drawing.Size(280, 21);
            this.tb_userID.TabIndex = 0;
            this.tb_userID.TextChanged += new System.EventHandler(this.tb_inpPw_TextChanged);
            this.tb_userID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_inpPw_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 56);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "账号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 108);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "密码";
            // 
            // btn_UserLogin
            // 
            this.btn_UserLogin.Location = new System.Drawing.Point(13, 174);
            this.btn_UserLogin.Margin = new System.Windows.Forms.Padding(2);
            this.btn_UserLogin.Name = "btn_UserLogin";
            this.btn_UserLogin.Size = new System.Drawing.Size(127, 35);
            this.btn_UserLogin.TabIndex = 2;
            this.btn_UserLogin.Text = "登入";
            this.btn_UserLogin.UseVisualStyleBackColor = true;
            this.btn_UserLogin.Click += new System.EventHandler(this.btn_UserLogin_Click);
            // 
            // Form_EngPermissionVerify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 245);
            this.Controls.Add(this.lbl_alert);
            this.Controls.Add(this.btn_UserLogin);
            this.Controls.Add(this.btn_UserLogout);
            this.Controls.Add(this.tb_userID);
            this.Controls.Add(this.tb_inpPw);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb_user);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_EngPermissionVerify";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "权限验证";
            this.Activated += new System.EventHandler(this.Form_EngineeringPermissionVerify_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_EngPermissionVerify_FormClosing);
            this.Load += new System.EventHandler(this.Form_EngineeringPermissionVerify_Load);
            this.VisibleChanged += new System.EventHandler(this.Form_EngPermissionVerify_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_user;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_inpPw;
        private System.Windows.Forms.Button btn_UserLogout;
        private System.Windows.Forms.Label lbl_alert;
        private System.Windows.Forms.TextBox tb_userID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_UserLogin;
    }
}