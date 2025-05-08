using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using SolveWare_TestPlugin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SolveWare_TesterCore
{
    public partial class Form_UniversalTesterMain : Form
    {
        public Form_UniversalTesterMain()
        {
            InitializeComponent();
        }
        const string PAGE_STATION_HARDWARE = "tp_StationHardwareSetting";
        const string PAGE_TEST_FRAME = "tp_TestFrame";

        ITesterCoreInteration _core;
        Dictionary<string, ToolStripLabel> _pluginStatusLabels = new Dictionary<string, ToolStripLabel>();
        Dictionary<string, ToolStripLabel> _monitorStatusLabels = new Dictionary<string, ToolStripLabel>();
        public void ConnectToCore(ITesterCoreInteration core)
        {
            this._core = core;
            this._core.SendOutFormCoreToGUIEvent -= ReceiveMessageFromCore;
            this._core.SendOutFormCoreToGUIEvent += ReceiveMessageFromCore;
            this._core.SendOutFormCoreToGUIEventWithReturn += _core_SendOutFormCoreToGUIEventWithReturn;
            this.Text = string.Format("LASER X TEST SOLUTION II - {0}", _core.PlatformVersionInfomation);
        }

        private object _core_SendOutFormCoreToGUIEventWithReturn(IMessage message)
        {
            switch (message.Type)
            {

                case EnumMessageType.Internal:
                    return this.HandleInternalMessageWithReturn(message);
                    break;
            }
            return new object();
        }
        private object HandleInternalMessageWithReturn(IMessage message)
        {
            try
            {
                var im = message as InternalMessage;
                switch (im.OperationType)
                {
                    case InternalOperationType.CoreRequest_GUIRunUIInvokeFunc:
                        {
                            return this.RunInvokeFunc((Func<object>)im.Context);
                        }
                        break;
                }
            }
            catch
            {

            }
            return new object();
        }
        private void ReceiveMessageFromCore(IMessage message)
        {
            switch (message.Type)
            {
                case EnumMessageType.Global:
                    this.HandleGlobalMessage(message);
                    break;
                case EnumMessageType.Internal:
                    this.HandleInternalMessage(message);
                    break;
            }
        }
        private void HandleGlobalMessage(IMessage message)
        {

        }
        private void HandleInternalMessage(IMessage message)
        {
            try
            {
                var im = message as InternalMessage;
                switch (im.OperationType)
                {
                    case InternalOperationType.CoreRequest_GUIMessageBoardDockingInvokeAction:
                        {
                            this.Invoke((EventHandler)delegate
                            {
                                var frm = (Form)im.Context;
                                frm.Hide();
                                frm.TopLevel = false;
                                frm.FormBorderStyle = FormBorderStyle.None;
                                frm.Dock = DockStyle.Fill;
                                frm.Show();
                                pnl_auxi.Controls.Clear();
                                pnl_auxi.Controls.Add(frm);
                            });
                        }
                        break;
                    //case InternalOperationType.CoreRequest_GUINanoTrakBoardDockingInvokeAction:
                    //    {
                    //        this.Invoke((EventHandler)delegate
                    //        {
                    //            var frm = (Form)im.Context;
                    //            frm.Hide();
                    //            frm.TopLevel = false;
                    //            frm.FormBorderStyle = FormBorderStyle.None;
                    //            frm.Dock = DockStyle.Fill;
                    //            frm.Show();
                    //            pnl_auxi.Controls.Clear();
                    //            pnl_auxi.Controls.Add(frm);
                    //        });
                    //    }
                    //    break;
                    case InternalOperationType.CoreRequest_GUIStationBoardDockingInvokeAction:
                        {
                            this.Invoke((EventHandler)delegate
                            {
                                var frm = (Form)im.Context;
                                frm.Hide();
                                frm.TopLevel = false;
                                frm.FormBorderStyle = FormBorderStyle.None;
                                frm.Dock = DockStyle.Fill;
                                frm.Show();
                                this.tab_MainPage.TabPages[PAGE_STATION_HARDWARE].Controls.Clear();
                                this.tab_MainPage.TabPages[PAGE_STATION_HARDWARE].Controls.Add(frm);
                            });
                        }
                        break;
                    case InternalOperationType.CoreRequest_GUITestFrameBoardDockingInvokeAction:
                        {
                            this.Invoke((EventHandler)delegate
                            {
                                var frm = (Form)im.Context;
                                frm.Hide();
                                frm.TopLevel = false;
                                frm.FormBorderStyle = FormBorderStyle.None;
                                frm.Dock = DockStyle.Fill;
                                frm.Show();
                                this.tab_MainPage.TabPages[PAGE_TEST_FRAME].Controls.Clear();
                                this.tab_MainPage.TabPages[PAGE_TEST_FRAME].Controls.Add(frm);
                            });
                        }
                        break;

                    case InternalOperationType.CoreRequest_GUIMulti_TestPluginMainPageDockingInvokeAction:
                        {
                            this.Invoke((EventHandler)delegate
                            {
                                ITesterAppPluginInteration app = (ITesterAppPluginInteration)im.Context;
                                var frm = (Form)app.GetUI();
                                frm.Hide();
                                frm.TopLevel = false;
                                frm.FormBorderStyle = FormBorderStyle.None;
                                frm.Dock = DockStyle.Fill;
                                frm.Show();
                                TabPage pluginPage = new TabPage($"[{app.Name}]Ver.[{app.VersionInfo}]")
                                {
                                    Name = app.Name
                                };

                                pluginPage.Controls.Clear();
                                pluginPage.Controls.Add(frm);
                                this.tab_MainPage.TabPages.Insert(0, pluginPage);

                            });
                        }
                        break;
                    case InternalOperationType.CoreRequest_GUIRunUIInvokeAction:
                        {
                            this.RunInvokeAction((Action)im.Context);
                        }
                        break;
                    case InternalOperationType.CoreRequest_GUIRunUIInvokeActionSYNC:
                        {
                            this.RunInvokeActionSYNC((Action)im.Context);
                        }
                        break;
                    case InternalOperationType.Layout_CreateOrDockTesterAppTabPage:
                        {
                            this.Invoke((EventHandler)delegate
                            {
                                ITesterAppPluginInteration app = (ITesterAppPluginInteration)im.Context;

                                var frm = (Form)app.GetUI();
                                frm.Hide();
                                frm.TopLevel = false;
                                frm.FormBorderStyle = FormBorderStyle.None;
                                frm.Dock = DockStyle.Fill;
                                frm.Show();

                                TabPage tp = null;
                                if (this.tab_MainPage.TabPages.ContainsKey(app.Name))
                                {
                                    tp = this.tab_MainPage.TabPages[app.Name];
                                }
                                else
                                {
                                    tp = new TabPage(app.Name)
                                    {
                                        Name = app.Name
                                    };
                                    this.tab_MainPage.TabPages.Add(tp);
                                }
                                this.tab_MainPage.TabPages[app.Name].Controls.Clear();
                                this.tab_MainPage.TabPages[app.Name].Controls.Add(frm);
                            });
                        }
                        break;
                    case InternalOperationType.Layout_HideTesterAppTabPage:
                        {
                            this.Invoke((EventHandler)delegate
                            {
                                ITesterAppPluginInteration app = (ITesterAppPluginInteration)im.Context;
                                if (this.tab_MainPage.TabPages.ContainsKey(app.Name))
                                {

                                    this.tab_MainPage.TabPages.RemoveByKey(app.Name);
                                }
                            });
                        }
                        break;
                    case InternalOperationType.UserRequest_LoginStatusChanged:
                        {
                            this.Invoke((EventHandler)delegate { this.RefreshUserInfo(im.Context); });
                        }
                        break;
                    case InternalOperationType.TestStation_Initialized:
                        {
                            this.Invoke((EventHandler)delegate
                            {
                                this.tab_MainPage.Enabled = true;
                                this.tab_MainPage.SelectedIndex = 0;
                            });
                        }
                        break;
                    case InternalOperationType.CoreRequest_CreateMonitorStatusInfo:
                        {
                            this.Invoke((EventHandler)delegate
                            {
                                this.CreateMonitorStatusInfoStrip(im.Context as Dictionary<string, Tuple<string, InstrMonitorStatus>>);
                            });
                        }
                        break;
                    case InternalOperationType.CoreRequest_UpdateMonitorStatusInfo:
                        {
                            this.BeginInvoke((EventHandler)delegate
                            {
                                this.UpdateMonitorStatusInfoStrip(im.Context as Dictionary<string, Tuple<string, InstrMonitorStatus>>);
                            });
                        }
                        break;
                    case InternalOperationType.CoreRequest_CreateTestPluginStatusInfo:
                        {
                            this.Invoke((EventHandler)delegate
                            {
                                this.CreateTestPluginStatusInfoStrip(im.Context as Dictionary<string, Tuple<string, PluginOnlineStatus, PluginRunStatus>>);
                            });
                        }
                        break;
                    case InternalOperationType.CoreRequest_UpdateTestPluginStatusInfo:
                        {
                            this.BeginInvoke((EventHandler)delegate
                            {
                                this.UpdateTestPluginStatusInfoStrip(im.Context as Dictionary<string, Tuple<string, PluginOnlineStatus, PluginRunStatus>>);
                            });
                        }
                        break;
                    case InternalOperationType.NotifyUI_ProductConfigChanged:
                        {
                            this.BeginInvoke((EventHandler)delegate
                            {
                                this.Invoke((EventHandler)delegate { this.RefreshProductInfo(im.Context); });

                            });
                        }
                        break;
                }
            }
            catch (Exception ex)
            {

                this._core.ReportException(ex.Message, ErrorCodes.PlatformUIActionFailed, ex);
            }
        }
        public void RunInvokeAction(Action guiInvokeAction)
        {
            try
            {
                this.BeginInvoke(guiInvokeAction);
            }
            catch (Exception ex)
            {

            }
        }
        public void RunInvokeActionSYNC(Action guiInvokeAction)
        {
            try
            {
                this.Invoke(guiInvokeAction);
            }
            catch (Exception ex)
            {

            }
        }
        public object RunInvokeFunc(Func<object> guiInvokeAction)
        {
            try
            {
                return this.Invoke(guiInvokeAction);
            }
            catch (Exception ex)
            {

            }
            return new object();
        }
        private void 登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._core.ShowLoginUI();
        }

        private void 管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._core.ShowUserManageUI();

        }
        private void RefreshUserInfo(object info)
        {
            try
            {
                object[] infos = info as object[];
                tssl_CurrentUser.Text = $"{info}";

            }
            catch { }
        }
        private void RefreshProductInfo(object info)
        {
            try
            {

                tssl_CurrentProductName.Text = $"[{info}]";

            }
            catch { }
        }
        private void UpdateMonitorStatusInfoStrip(Dictionary<string, Tuple<string, InstrMonitorStatus>> monitorStatus)
        {

            try
            {
                foreach (var item in monitorStatus)
                {
                    if (_monitorStatusLabels.ContainsKey(item.Key))
                    {
                        _monitorStatusLabels[item.Key].Text = item.Value.Item1;
                        switch (item.Value.Item2)
                        {
                            case InstrMonitorStatus.Info:
                                {
                                    _monitorStatusLabels[item.Key].BackColor = Color.FromKnownColor(KnownColor.Info);
                                }
                                break;
                            case InstrMonitorStatus.Normal:
                                {
                                    _monitorStatusLabels[item.Key].BackColor = Color.FromKnownColor(KnownColor.LawnGreen);
                                    _monitorStatusLabels[item.Key].ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                                }
                                break;
                            case InstrMonitorStatus.Abnormal:
                                {
                                    _monitorStatusLabels[item.Key].BackColor = Color.FromKnownColor(KnownColor.Red);
                                    _monitorStatusLabels[item.Key].ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void CreateMonitorStatusInfoStrip(Dictionary<string, Tuple<string, InstrMonitorStatus>> monitorStatus)
        {
            try
            {

                foreach (var item in monitorStatus)
                {
                    if (_monitorStatusLabels.ContainsKey(item.Key))
                    {
                        _monitorStatusLabels[item.Key].Text = item.Value.Item1;
                    }
                    else
                    {
                        var staLbl = new ToolStripLabel()
                        {
                            Name = item.Key,
                            Text = item.Value.Item1
                        };
                        _monitorStatusLabels.Add(item.Key, staLbl);
                    }
                }

                this.ss_mainInfo.Items.AddRange(this._monitorStatusLabels.Values.ToArray());

            }
            catch (Exception ex)
            {
            }
        }

        private void CreateTestPluginStatusInfoStrip(Dictionary<string, Tuple<string, PluginOnlineStatus, PluginRunStatus>> pluginStatus)
        {
            try
            {

                foreach (var item in pluginStatus)
                {
                    if (_pluginStatusLabels.ContainsKey(item.Key))
                    {
                        _pluginStatusLabels[item.Key].Text = item.Value.Item1;
                    }
                    else
                    {
                        var staLbl = new ToolStripLabel()
                        {
                            Name = item.Key,
                            Text = item.Value.Item1
                        };
                        _pluginStatusLabels.Add(item.Key, staLbl);
                    }
                }

                this.ss_mainInfo.Items.AddRange(this._pluginStatusLabels.Values.ToArray());
                timer1.Interval = 100;
                timer1.Enabled = true;
            }
            catch (Exception ex)
            {
            }
        }
        private void UpdateTestPluginStatusInfoStrip(Dictionary<string, Tuple<string, PluginOnlineStatus, PluginRunStatus>> pluginStatus)
        {

            try
            {
                foreach (var item in pluginStatus)
                {
                    if (_pluginStatusLabels.ContainsKey(item.Key))
                    {
                        _pluginStatusLabels[item.Key].Text = item.Value.Item1;
                        _pluginStatusLabels[item.Key].ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                        switch (item.Value.Item2)
                        {
                            case PluginOnlineStatus.Online:
                                {
                                    switch (item.Value.Item3)
                                    {
                                        case PluginRunStatus.NotHomeYet:
                                            {
                                                _pluginStatusLabels[item.Key].BackColor = Color.FromKnownColor(KnownColor.HotPink);
                                            }
                                            break;
                                        case PluginRunStatus.Idle:
                                            {
                                                _pluginStatusLabels[item.Key].BackColor = Color.FromKnownColor(KnownColor.Orange);
                                            }
                                            break;
                                        case PluginRunStatus.Ready:
                                            {
                                                _pluginStatusLabels[item.Key].BackColor = Color.FromKnownColor(KnownColor.Turquoise);
                                            }
                                            break;
                                        case PluginRunStatus.Running:
                                            {
                                                _pluginStatusLabels[item.Key].BackColor = Color.FromKnownColor(KnownColor.LightSkyBlue);
                                            }
                                            break;
                                        case PluginRunStatus.Stopped:
                                            {
                                                _pluginStatusLabels[item.Key].BackColor = Color.FromKnownColor(KnownColor.Yellow);
                                            }
                                            break;
                                        case PluginRunStatus.Invalid:
                                            {
                                                _pluginStatusLabels[item.Key].BackColor = Color.FromKnownColor(KnownColor.Violet);
                                            }
                                            break;
                                        case PluginRunStatus.Error:
                                            {
                                                _pluginStatusLabels[item.Key].BackColor = Color.FromKnownColor(KnownColor.Red);
                                            }
                                            break;
                                        case PluginRunStatus.Finished:
                                            {
                                                _pluginStatusLabels[item.Key].BackColor = Color.FromKnownColor(KnownColor.LawnGreen);
                                            }
                                            break;
                                    }

                                }
                                break;
                            case PluginOnlineStatus.Offline:
                                {

                                    _pluginStatusLabels[item.Key].BackColor = Color.FromKnownColor(KnownColor.DimGray);
                                }
                                break;

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void tab_MainPage_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (this.tab_MainPage.TabPages[e.TabPageIndex].Controls != null &&
                this.tab_MainPage.TabPages[e.TabPageIndex].Controls.Count > 0)
            {
                var appName = this.tab_MainPage.TabPages[e.TabPageIndex].Name;
                switch (appName)
                {
                    case PAGE_STATION_HARDWARE:
                    case PAGE_TEST_FRAME:
                        break;
                    default:
                        {
                            if (this._core.CanUserAccessResourceProviderUI(appName) == false)
                            {
                                e.Cancel = true;
                                switch (this._core.ResourceOwner)
                                {
                                    case GenernalResourceOwner.Platform:
                                        {
                                            MessageBox.Show($"当前平台资源使用者为[{this._core.ResourceOwner}],不允许访问此页面[{appName}]!");
                                        }
                                        break;
                                    case GenernalResourceOwner.Plugin:
                                        {
                                            if((Control.ModifierKeys& Keys.Shift)== Keys.Shift)
                                            {
                                                e.Cancel = false;
                                            }
                                            else
                                            {
                                                MessageBox.Show($"当前平台资源使用者为[{this._core.ResourceOwner}-{this._core.ResourceOwnerName}],不允许访问此页面[{appName}]!");
                                            }
                                        }
                                        break;
                                }

                                
                            }
                        }
                        break;
                }
            }
        }



        private void 切换配置参数ToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            try
            {
                var tsm = (sender as ToolStripMenuItem);
                tsm.DropDownItems.Clear();
                var prods = this._core.GetProductConfigFolderNames();
                foreach (var prod in prods)
                {
                    ToolStripMenuItem tsi = new ToolStripMenuItem(prod);
                    tsi.Click += Tsi_Click;
                    tsm.DropDownItems.Add(tsi);
                }
            }
            catch
            {

            }
        }

        private void Tsi_Click(object sender, EventArgs e)
        {
            try
            {
                var tsm = (sender as ToolStripMenuItem);
                this._core.SwitchProductConfig(tsm.Text);

            }
            catch
            {

            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                Form_CreateNewProductConfig frm = new Form_CreateNewProductConfig();

                var dr = frm.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    this._core.CreateProductConfig(frm.NewProductName);
                }
                else
                {

                }

            }
            catch
            {

            }

        }

        private void Form_UniversalTesterMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("确定退出程序？", "退出询问", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this._core.TryReleaseResourceBeforeClosing();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this._core.CanUserAccessDomain(AccessPermissionLevel.Engineer))
            {
                this.tp_TestFrame.Parent = this.tab_MainPage;
            }
            else
            {
                this.tp_TestFrame.Parent = null;
            }
        }
    }

}