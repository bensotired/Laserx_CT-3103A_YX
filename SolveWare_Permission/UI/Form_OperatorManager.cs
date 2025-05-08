using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SolveWare_Permission
{
    public partial class Form_OperatorManager : Form, IAccessPermissionLevel
    {
      
        public   AccessPermissionLevel APL
        {
            get
            {
                return AccessPermissionLevel.Admin;
            }
        }
        public Form_OperatorManager()
        {
            InitializeComponent();
            //ReLoadOperatorFile();
            this.dgv_Operators.DataError += Dgv_Operators_DataError;
        }

        private void Dgv_Operators_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
           
        }

     

        private void Form_OperatorManager_Load(object sender, EventArgs e)
        {
            try
            {
                ReloadOperatorDGV();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化用户管理组件错误[{ex.Message}-{ex.StackTrace}]", "初始化用户管理组件");
                
            }
        }

        private void ReloadOperatorDGV()
        {
            try
            {
                this.dgv_Operators.Rows.Clear();
                //this.dgv_Operators.RowCount = this._core.UserList.Count;
                int i = 0;
                foreach (var item in PermissionManager.Instance.UserList )
                {
                    DataGridViewRow row = CreateNewRow();
                    this.dgv_Operators.Rows.Add(row);
                    this.dgv_Operators.Rows[i].Cells[0].Value = item.APL;
                    this.dgv_Operators.Rows[i].Cells[1].Value = item.ID;
                    this.dgv_Operators.Rows[i].Cells[2].Value = item.Password;
                    i++;
                }
            }
            catch
            {

            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow row = CreateNewRow();

                this.dgv_Operators.Rows.Add(row);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"新建用户错误[{ex.Message}-{ex.StackTrace}]", "新建用户");
                //this._core?.ReportException("Create User Exception", ExpectedException.USER_ALARM, ex);
            }
        }

        private static DataGridViewRow CreateNewRow()
        {
            DataGridViewRow row = new DataGridViewRow();
            DataGridViewComboBoxCell grpCell = new DataGridViewComboBoxCell();
            grpCell.Items.Add(AccessPermissionLevel.Engineer.ToString());
            grpCell.Items.Add(AccessPermissionLevel.Operator.ToString());
            grpCell.Value = AccessPermissionLevel.Operator.ToString();

            row.Cells.Add(grpCell);
            DataGridViewTextBoxCell nameCell = new DataGridViewTextBoxCell();
            nameCell.Value = "";
            row.Cells.Add(nameCell);

            DataGridViewTextBoxCell pwdCell = new DataGridViewTextBoxCell();
            row.Cells.Add(pwdCell);
            return row;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //string dirName = Path.GetDirectoryName(filePath);
                //if (Directory.Exists(dirName) == false)
                //{
                //    Directory.CreateDirectory(dirName);
                //}
                //var Dic_Operators = new Dictionary<string, string>();
                var Dic_Operators =new  List<PermissionItem>();

                for (int i = 0; i < this.dgv_Operators.RowCount; i++)
                {
                    if (this.dgv_Operators.Rows[i].Cells[0].Value != null &&
                        this.dgv_Operators.Rows[i].Cells[1].Value != null &&
                        this.dgv_Operators.Rows[i].Cells[2].Value != null)
                    {
                        var apl  = this.dgv_Operators.Rows[i].Cells[0].Value.ToString();
                        var id = this.dgv_Operators.Rows[i].Cells[1].Value.ToString();
                        var pwd = this.dgv_Operators.Rows[i].Cells[2].Value.ToString();

                        if (Dic_Operators.Exists(item=> item.ID == id ) == false)//新的账号
                        {
                            Dic_Operators.Add( new PermissionItem()
                            {
                                APL = apl,
                                ID = id,
                                Password =pwd
                            });
                        }
                        else //已经存在账号直接更新密码
                        {
                            var tempItem = Dic_Operators.Find(item => item.ID == id);
                            tempItem.APL = apl;
                            tempItem.Password = pwd;
                        }
                    }
                }
                PermissionManager.Instance.SaveOperatorFile(Dic_Operators);
     
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存用户错误[{ex.Message}-{ex.StackTrace}]", "保存用户");
                //this._core?.ReportException("Save User Exception", ExpectedException.USER_ALARM, ex);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            try
            {
                DialogResult RSS = MessageBox.Show(this, "确定要删除选中行数据码？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (RSS == DialogResult.Yes)
                {
                    var account = dgv_Operators.CurrentRow.Cells[1].Value;
                    dgv_Operators.Rows.Remove(dgv_Operators.CurrentRow);

                    string msg = string.Format("删除账号 [{0}] 成功!", account);
                    //this._core?.Log_Global(msg);
                    MessageBox.Show(msg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除用户错误[{ex.Message}-{ex.StackTrace}]", "删除用户");
     
            }
        }

        private void Form_OperatorManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void Form_OperatorManager_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                //ReLoadOperatorFile();
                ReloadOperatorDGV();
                //for (int i = 0; i < Dic_Operators.Count; i++)
                //{
                //    this.dgv_Operators.Rows[i].Cells[0].Value = Names[i];
                //    this.dgv_Operators.Rows[i].Cells[1].Value = Pwds[i];
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化用户管理组件错误[{ex.Message}-{ex.StackTrace}]", "初始化用户管理组件");
                //this._core?.ReportException("初始化用户管理组件错误", ExpectedException.USER_ALARM, ex);
            }
        }
    }
}
