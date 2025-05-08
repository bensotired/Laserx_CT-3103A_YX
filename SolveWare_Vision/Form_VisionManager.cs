using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SolveWare_Vision
{
    public partial class Form_VisionManager : Form, ITesterAppUI, IAccessPermissionLevel
    {
        Color YES_COLOR = Color.LimeGreen;
        Color NO_COLOR = Color.DarkGreen;

        ITesterCoreInteration _core;
        ITesterAppPluginInteration _appInteration;
        VisionManager _myVisionManager;
        object lockRefresh = new object();
        Timer timer1 = new Timer();
        Dictionary<int, TextBox> _cmdLines = new Dictionary<int, TextBox>();
   
        public Form_VisionManager()
        {
            InitializeComponent();
            _cmdLines.Clear();
            _cmdLines.Add(1, tb_vision_cmd_1);
            _cmdLines.Add(2, tb_vision_cmd_2);
            _cmdLines.Add(3, tb_vision_cmd_3);
            _cmdLines.Add(4, tb_vision_cmd_4);
            _cmdLines.Add(5, tb_vision_cmd_5);
        }
        void SaveCMDsToConfig()
        {
            DataBook<int, string> _cmdValues = new DataBook<int, string>();
            foreach (var kvp in _cmdLines)
            {
                _cmdValues.Add(kvp.Key, kvp.Value.Text);
            }
            _myVisionManager.UpdateConfigAndSave(_cmdValues);
        }
        private void Form_MotionManager_Load(object sender, EventArgs e)
        {
            RefreshOnce();
        }
 
        public AccessPermissionLevel APL
        {
            get { return AccessPermissionLevel.None; }
        }
 
        public void ConnectToAppInteration(ITesterAppPluginInteration app)
        {
            _appInteration = app;
            _myVisionManager = (VisionManager)app;
        }
        public void ConnectToCore(ITesterCoreInteration core)
        {
            _core = core as ITesterCoreInteration;
            _core.SendOutFormCoreToGUIEvent -= ReceiveMessageFromCore;
            _core.SendOutFormCoreToGUIEvent += ReceiveMessageFromCore;
        }
        public void DisconnectFromCore(ITesterCoreInteration core)
        {
            _core.SendOutFormCoreToGUIEvent -= ReceiveMessageFromCore;
        }

        private void ReceiveMessageFromCore(IMessage message)
        {
            //      throw new NotImplementedException();
        }
        private void 浮动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _appInteration.PopUI();
        }
        private void 还原ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            _appInteration.DockUI();
        }
        private void Form_MotionManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            _appInteration.DockUI();
        }




        public void RefreshOnce()
        {
            this.Invoke((EventHandler)delegate
            {
                try
                {
                    if (_myVisionManager == null)
                    {
                        return;
                    }
                    foreach (var kvp in _myVisionManager.Config.VisionCMDBook)
                    {
                        if (_cmdLines.ContainsKey(kvp.Key))
                        {
                            _cmdLines[kvp.Key].Text = kvp.Value;
                        }
                    }
                }
                catch
                {

                }
            });
    
        }

      

        private void btn_send_vision_cmd_1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.tb_vision_cmd_1.Text))
                {
                    MessageBox.Show($"视觉命令1为空!");
                }
                else
                {
                    var ret = this._myVisionManager.GetVisionResult_Universal(this.tb_vision_cmd_1.Text);
                    this.rtb_cmd_result.AppendText($"视觉命令1返回结果\r\n{ret}\r\n");
                    SaveCMDsToConfig();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"视觉命令1执行错误:[{ex.Message}-{ex.StackTrace}]!");
            }
        }

        private void btn_send_vision_cmd_2_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.tb_vision_cmd_2.Text))
                {
                    MessageBox.Show($"视觉命令2为空!");
                }
                else
                {
                    var ret = this._myVisionManager.GetVisionResult_Universal(this.tb_vision_cmd_2.Text);
                    this.rtb_cmd_result.AppendText($"视觉命令2返回结果\r\n{ret}\r\n");
                    SaveCMDsToConfig();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"视觉命令1执行错误:[{ex.Message}-{ex.StackTrace}]!");
            }
        }

        private void btn_send_vision_cmd_3_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.tb_vision_cmd_3.Text))
                {
                    MessageBox.Show($"视觉命令3为空!");
                }
                else
                {
                    var ret = this._myVisionManager.GetVisionResult_Universal(this.tb_vision_cmd_3.Text);
                    this.rtb_cmd_result.AppendText($"视觉命令3返回结果\r\n{ret}\r\n");
                    SaveCMDsToConfig();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"视觉命令1执行错误:[{ex.Message}-{ex.StackTrace}]!");
            }
        }

        private void btn_send_vision_cmd_4_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.tb_vision_cmd_4.Text))
                {
                    MessageBox.Show($"视觉命令4为空!");
                }
                else
                {
                    var ret = this._myVisionManager.GetVisionResult_Universal(this.tb_vision_cmd_4.Text);
                    this.rtb_cmd_result.AppendText($"视觉命令4返回结果\r\n{ret}\r\n");
                    SaveCMDsToConfig();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"视觉命令1执行错误:[{ex.Message}-{ex.StackTrace}]!");
            }
        }

        private void btn_send_vision_cmd_5_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.tb_vision_cmd_5.Text))
                {
                    MessageBox.Show($"视觉命令5为空!");
                }
                else
                {
                    var ret = this._myVisionManager.GetVisionResult_Universal(this.tb_vision_cmd_5.Text);
                    this.rtb_cmd_result.AppendText($"视觉命令5返回结果\r\n{ret}\r\n");
                    SaveCMDsToConfig();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"视觉命令1执行错误:[{ex.Message}-{ex.StackTrace}]!");
            }
        }

        private void btn_clear_cmd_result_Click(object sender, EventArgs e)
        {
            this.rtb_cmd_result.Clear();
        }
    }
}