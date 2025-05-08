using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SolveWare_Permission
{
    public sealed class PermissionManager : TesterAppPluginModel
    {
        const string filePath = @".\OperatorID\Operators.xml";
        const string LOGOUT_ID = "未登录";
        const string USER_ID_BACKDOOR_COMMON = "laserxadmin";
        const string USER_PW_BACKDOOR_COMMON = "laserx112233";
        const string USER_ID_BACKDOOR = "TINYBEN";
        const string USER_PW_BACKDOOR = "FATSEAL";
        static PermissionManager _instance;
        static object _mutex = new object();
        string _currentUserID = "";
        AccessPermissionLevel _currentAPL = AccessPermissionLevel.None;
        //public Dictionary<string, string> UserDict = new Dictionary<string, string>();
        public List<PermissionItem> UserList = new List<PermissionItem>();
        public PermissionManager()
        {

        }
        public override AccessPermissionLevel APL
        {
            get
            {
                return this._currentAPL;
            }
        }
        public string CurrentUserID
        {
            get
            {
                return _currentUserID;
            }
        }

        public string UserInfo
        {
            get
            {
                return $"[{this._currentAPL}-{this._currentUserID}]";
            }
        }
        public static PermissionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_mutex)
                    {
                        if (_instance == null)
                        {
                            _instance = new PermissionManager();
                        }
                    }
                }
                return _instance;
            }
        }
        public override void StartUp()
        {
            ReLoadUserFile();
        }
        public void ReLoadUserFile()
        {
            try
            {

                if (File.Exists(filePath) == false)
                {
                    this.Log_Global($"ReLoadOperatorFile 文件不存在:{filePath}");
                    return;
                }
                var temp = XmlHelper.DeserializeFile<List<PermissionItem>>(filePath);
                this.UserList.Clear();
                foreach (var item in temp)
                {
                    this.UserList.Add(item);
                }

            }
            catch (Exception ex)
            {
                this.ReportException("ReLoadOperatorFile Exception", ErrorCodes.PlatformUserFileLoadFailed, ex);
                MessageBox.Show(ex.Message);
            }
        }
        public void ShowLoginUI()
        {
            Form_EngPermissionVerify ui = new Form_EngPermissionVerify();
            ui.ShowDialog();
        }
        public void ShowUserManageUI()
        {
            Form_OperatorManager ui = new Form_OperatorManager();
            ui.ShowDialog();
        }
        public void SaveOperatorFile(List<PermissionItem> permissions)
        {
            try
            {
                string dirName = Path.GetDirectoryName(filePath);
                if (Directory.Exists(dirName) == false)
                {
                    Directory.CreateDirectory(dirName);
                }

                XmlHelper.SerializeFile<List<PermissionItem>>(filePath, permissions);
                UserList.Clear();
                foreach (var item in permissions)
                {
                    UserList.Add(item);
                }

                MessageBox.Show(string.Format("保存成功！！"));
            }
            catch (Exception ex)
            {
                this.ReportException("Save User Exception", ErrorCodes.PlatformUserFileSaveFailed, ex);
            }
        }

        public bool TryLogin(AccessPermissionLevel apl, string userId, string password)
        {
            this.ReLoadUserFile();
            if (this.UserList.Exists(item => item.ID == userId))
            {
                var user = this.UserList.Find(item => item.ID == userId);
                if (user.APL == apl.ToString() && user.Password == password)
                {
                    this._currentAPL = apl;
                    this._currentUserID = userId;
                    this._coreInteration.RefreshLoginInfo();
                   // this.SendMessage(new InternalMessage("", InternalOperationType.UserRequest_LoginStatusChanged, new object[] { this._currentAPL, userId }));
                    return true;
                }
            }
            if (userId.Equals(USER_ID_BACKDOOR_COMMON))
            {
                if (password.Equals(USER_PW_BACKDOOR_COMMON))
                {
                    this._currentAPL = AccessPermissionLevel.Admin;
                    this._currentUserID = userId;
                    this._coreInteration.RefreshLoginInfo();
                    // this.SendMessage(new InternalMessage("", InternalOperationType.UserRequest_LoginStatusChanged, new object[] { this._currentAPL, USER_ID_BACKDOOR_COMMON }));
                    return true;
                }
            }
            if (userId.Equals(USER_ID_BACKDOOR))
            {
                if (password.Equals(USER_PW_BACKDOOR))
                {
                    this._currentAPL = AccessPermissionLevel.Ben;
                    this._currentUserID = userId;
                    this._coreInteration.RefreshLoginInfo();
                    //this.SendMessage(new InternalMessage("", InternalOperationType.UserRequest_LoginStatusChanged, new object[] { this._currentAPL, USER_ID_BACKDOOR }));
                    return true;
                }
            }

            return false;
        }
        public bool TryLogout()
        {
            this._currentAPL = AccessPermissionLevel.None;
            this._currentUserID = LOGOUT_ID;
            this._coreInteration.RefreshLoginInfo();
            //this.SendMessage(new InternalMessage("", InternalOperationType.UserRequest_LoginStatusChanged, new object[] { this._currentAPL, LOGOUT_ID }));
            return true;
        }
        public bool CanUserAccessDomain(AccessPermissionLevel requestApl)
        {
            if (_currentAPL >= requestApl)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
