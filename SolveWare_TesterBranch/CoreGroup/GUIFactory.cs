using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
//using SolveWare_BurnInGUIElements;

namespace SolveWare_TesterCore
{
    public class GUIFactory : TesterPluginModel
    {
        static object _mutex = new object();
        static GUIFactory _instance;
        Dictionary<UserControlType, UserControl> _userControlPool = new Dictionary<UserControlType, UserControl>();
        Dictionary<GUIType, Form> _guiPool = new Dictionary<GUIType, Form>();
        Dictionary<Type, Form> _customizePool = new Dictionary<Type, Form>();
        public static GUIFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_mutex)
                    {
                        if (_instance == null)
                        {
                            _instance = new GUIFactory();
                        }
                    }
                }
                return _instance;
            }
        }
        public object GetCustomizeGUI(Type uiType)
        {
            if (_customizePool.ContainsKey(uiType))
            {

            }
            else
            {
                var form = AssemblyManager.CreateInstance<Form>($"{uiType.Namespace}.{uiType.Name}");
                _customizePool.Add(uiType, form);
            }
            return _customizePool[uiType];
        }
        public void ShowPopupGUI(GUIType guiT, object args = null)
        {
            var gui = GetGUI(guiT);
            AccessPermissionLevel guiAPL = AccessPermissionLevel.Ben;
            try
            {
                guiAPL = (gui as IAccessPermissionLevel).APL;
            }
            catch
            {

            }
            
            if (this._coreInteration.CanUserAccessDomain(guiAPL))
            {
                this._coreInteration.GUIRunUIInvokeAction(() =>
                {

                    try
                    {
                     
                        gui.Hide();
 
                        gui.TopLevel = true;
                        gui.FormBorderStyle = FormBorderStyle.Sizable;

                        int w =  SystemInformation.WorkingArea.Width;
                        int h =  SystemInformation.WorkingArea.Height;
                        gui.Width = w -300;
                        gui.Height = h - 100;
                        gui.Location = new Point((w - gui.Width) / 2, (h - gui.Height) / 2);

                        gui?.Show();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show($"获取{guiT}窗口失败:[{ex.Message}-{ex.StackTrace}]!", "界面元素错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                
                });
            }
            else
            {
                MessageBox.Show($"当前用户组[{ this._coreInteration.CurrentAPL}]不能访问该组件!", "权限不足", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
        public Form GetGUI(GUIType guiT)
        {
            if (_guiPool.ContainsKey(guiT))
            {
                var gui = this._guiPool[guiT];
                if (gui == null || gui.IsDisposed)
                {
                    this._guiPool[guiT] = CreateGUI(guiT);
                }
            }
            else
            {
                this._guiPool.Add(guiT, CreateGUI(guiT));
            }
            return this._guiPool[guiT];
        }
        public UserControl GetUserControl(UserControlType guiT)
        {
            if (_userControlPool.ContainsKey(guiT))
            {
                var gui = this._userControlPool[guiT];
                if (gui == null || gui.IsDisposed)
                {
                    this._userControlPool[guiT] = CreateUserControl(guiT);
                }
            }
            else
            {
                this._userControlPool.Add(guiT, CreateUserControl(guiT));
            }
            return this._userControlPool[guiT];
        }
        Form CreateGUI(GUIType guiT)
        {
            Form frm = null;
            try
            {
                switch (guiT)
                {
                    //case GUIType.GUI_LAUNCH_BI:
                    //    {
                    //        frm = new Form_LaunchBI();
                    //        this._coreInteration.LinkToCore(frm as ICoreLink);
                    //    }
                    //    break;
                    case GUIType.GUI_MESSAGE_BOARD:
                        {
                            frm = new Form_MessageBorad();
                            this._coreInteration.LinkToCore(frm as ITesterCoreLink);
                        }
                        break;
                    //case GUIType.GUI_NanoTrak_BOARD:
                    //    {
                    //        frm = new Thorlabs_NanoTrakUI(); 
                    //        this._coreInteration.LinkToCore(frm as ITesterCoreLink);
                    //    }
                    //    break;
                    case GUIType.GUI_STATION_BOARD:
                        {
                            frm = new Form_StationBoard();
                            this._coreInteration.LinkToCore(frm as ITesterCoreLink);
                        }
                        break;
                    case GUIType.GUI_TEST_FRAME_BOARD:
                        {
                            frm = new Form_TestFrameBoard();
                            this._coreInteration.LinkToCore(frm as ITesterCoreLink);
                        }
                        break;
                        //    case GUIType.GUI_CHART_BOARD:
                        //        {
                        //            frm = new Form_ChartBoard();
                        //            this._coreInteration.LinkToCore(frm as ICoreLink);
                        //        }
                        //        break;
                        //    case GUIType.GUI_DATASNAP_BOARD:
                        //        {
                        //            frm = new Form_DataSnapBoard();
                        //            this._coreInteration.LinkToCore(frm as ICoreLink);
                        //        }
                        //        break;
                        //}
                }
            }
            catch
            {
            }
            return frm;
        }
        UserControl CreateUserControl(UserControlType guiT)
        {
            UserControl uc = null;
            //try
            //{
            //    switch (guiT)
            //    {
            //        case UserControlType.GUI_RACK_PNL_10X2X2:
            //            {
            //                uc = new Pnl_RACK_10X2X2();
            //            }
            //            break;
            //        case UserControlType.GUI_RACK_PNL_10X4:
            //            {
            //                uc = new Pnl_RACK_10X4();
            //            }
            //            break;
            //    }
            //}
            //catch
            //{
            //}
            return uc;
        }
        public void PopUI_DefaultSize(object uiObject)
        {
            AccessPermissionLevel guiAPL = AccessPermissionLevel.Ben;
            try
            {
                guiAPL = (uiObject as IAccessPermissionLevel).APL;
            }
            catch (Exception ex)
            {
                MessageBox.Show
                (
                    $"获取窗口失败:[{ex.Message}-{ex.StackTrace}]!",
                    "界面元素错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return;
            }

            if (this._coreInteration.CanUserAccessDomain(guiAPL))
            {
                this._coreInteration.GUIRunUIInvokeAction(() =>
                {
                    try
                    {
                        var gui = uiObject as Form;
                        if (gui.Parent != null)
                        {
                            (gui.Parent as Control).Controls.Clear();
                        }
                        gui.Hide();

                        gui.FormBorderStyle = FormBorderStyle.Sizable;
                        gui.StartPosition = FormStartPosition.CenterScreen;
                        gui.TopLevel = true;
                        gui?.Show();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show
                        (
                            $"获取窗口失败:[{ex.Message}-{ex.StackTrace}]!",
                            "界面元素错误",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }

                });
            }
            else
            {
                MessageBox.Show
                (
                    $"当前用户组[{ this._coreInteration.CurrentAPL}]不能访问该组件!",
                    "权限不足",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Asterisk
                );
            }
        }
        public void PopUI(object uiObject)
        {
            AccessPermissionLevel guiAPL = AccessPermissionLevel.Ben;
            try
            {
                guiAPL = (uiObject as IAccessPermissionLevel).APL;
            }
            catch (Exception ex)
            {
                MessageBox.Show
                (
                    $"获取窗口失败:[{ex.Message}-{ex.StackTrace}]!",
                    "界面元素错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return;
            }

            if (this._coreInteration.CanUserAccessDomain(guiAPL))
            {
                this._coreInteration.GUIRunUIInvokeAction(() =>
                {
                    try
                    {
                        var gui = uiObject as Form;
                        if (gui.Parent != null)
                        {
                            (gui.Parent as Control).Controls.Clear();
                        }
                        gui.Hide();

                        gui.FormBorderStyle = FormBorderStyle.Sizable;

                        int w = SystemInformation.WorkingArea.Width;
                        int h = SystemInformation.WorkingArea.Height;
                        gui.Width = w - 300;
                        gui.Height = h - 100;
                        gui.Location = new Point((w - gui.Width) / 2, (h - gui.Height) / 2);
                        gui.TopLevel = true;
                        gui?.Show();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show
                        (
                            $"获取窗口失败:[{ex.Message}-{ex.StackTrace}]!",
                            "界面元素错误",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }

                });
            }
            else
            {
                MessageBox.Show
                (
                    $"当前用户组[{ this._coreInteration.CurrentAPL}]不能访问该组件!",
                    "权限不足",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Asterisk
                );
            }
        }
    }
}