using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
 

namespace SolveWare_Permission
{
    public partial class Form_EngPermissionVerify : Form 
    {
 
        Dictionary<AccessPermissionLevel, string> temporaryPermissionDict = new Dictionary<AccessPermissionLevel, string>();

     

        public Form_EngPermissionVerify()
        {
            InitializeComponent();
            InitializeUserDict();

            this.cb_user.Items.Clear();
            foreach (var key in this.temporaryPermissionDict.Keys)
            {
                this.cb_user.Items.Add(key);
            }
            this.cb_user.SelectedIndex = 0;


        }
        //public void ConnectToCore(ICoreInteration core)
        //{
        //    this._core = core;
        //}

        //public void DisconnectFromCore(ICoreInteration core)
        //{
        //}
   
        private void InitializeUserDict()
        {
            this.temporaryPermissionDict.Clear();
            this.temporaryPermissionDict.Add(AccessPermissionLevel.Operator, "HW12345");
            this.temporaryPermissionDict.Add(AccessPermissionLevel.Engineer, "HW12345");

        }

        private void ClearAlert()
        {
            this.lbl_alert.Text = string.Empty;
        }
        private void SetAlert(string alert)
        {
            this.lbl_alert.Text = alert;
        }
        private void Logout()
        {
            this.ClearAlert();
            if(PermissionManager.Instance.APL == AccessPermissionLevel.None)
            {
                MessageBox.Show($"没有已登录的用户!");
                return;
            }
            var userInfo = PermissionManager.Instance.UserInfo;
            if ( PermissionManager.Instance.TryLogout())
            {
                MessageBox.Show($"用户{userInfo}登出成功!");
                this.Close();
            }
        }
        private void Login()
        {
            this.ClearAlert();
            if (this.cb_user.SelectedItem == null)
            {
                this.SetAlert("请选择用户组！");
                //this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                return;
            }
            var userGroup = (AccessPermissionLevel)Converter.ConvertObjectTo(this.cb_user.SelectedItem.ToString(), typeof(AccessPermissionLevel));
           
            if (this.temporaryPermissionDict.ContainsKey(userGroup) == false)
            {
                //this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.SetAlert("用户组不存在！");
                return;
            }
            else
            {
                if (string.IsNullOrEmpty(this.tb_userID.Text))
                {
                    this.SetAlert("请输入ID！");
                    //this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    return;
                }
                if (string.IsNullOrEmpty(this.tb_inpPw.Text))
                {
                    this.SetAlert("请输入密码！");
                    //this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    return;
                }
                string correctPw = this.temporaryPermissionDict[userGroup];
                string inPutPw = this.tb_inpPw.Text;
                string id = this.tb_userID.Text;
                //if (correctPw.Equals(inPutPw))
                //{

                if (PermissionManager.Instance.TryLogin(userGroup, id , inPutPw))
                {
                    //CurrentOperator = this.cb_user.SelectedItem.ToString();
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    MessageBox.Show($"用户[{userGroup}][{id}]登录成功!");
                    this.Close();
                }
                //}
                else
                {
                    MessageBox.Show($"用户[{userGroup}][{id}]登录失败!");
                    //this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    this.SetAlert("用户验证不通过！");
                }
            }
        }

        private void tb_inpPw_TextChanged(object sender, EventArgs e)
        {
            this.ClearAlert();
        }

        private void tb_inpPw_KeyPress(object sender, KeyPressEventArgs e)
        {
   
        }

        private void Form_EngineeringPermissionVerify_Load(object sender, EventArgs e)
        {
 
        }

        private void Form_EngineeringPermissionVerify_Activated(object sender, EventArgs e)
        {
            this.tb_inpPw.Focus();
        }

        private void btn_UserLogin_Click(object sender, EventArgs e)
        {
            this.Login();
        }

        private void btn_UserLogout_Click(object sender, EventArgs e)
        {
            this.Logout();
        }

        private void Form_EngPermissionVerify_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void Form_EngPermissionVerify_VisibleChanged(object sender, EventArgs e)
        {
            this.tb_inpPw.Text = string.Empty;
            this.tb_userID.Text = string.Empty;

        }
    }
}