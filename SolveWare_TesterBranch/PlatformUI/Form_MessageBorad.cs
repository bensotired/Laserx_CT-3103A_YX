using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_BurnInLog;
using SolveWare_BurnInMessage;
using System;
using System.Windows.Forms;

namespace SolveWare_TesterCore
{
    public partial class Form_MessageBorad : Form, ITesterCoreLink, IAccessPermissionLevel
    {
        int logLineCount = 0;
        int exceptionLineCount = 0;
        public Form_MessageBorad()
        {
            InitializeComponent();
            //this.TopLevel = false;
        }
        ITesterCoreInteration _core;

        public AccessPermissionLevel APL
        {
            get
            {
                return AccessPermissionLevel.None;
            }
        }

        public void ConnectToCore(ITesterCoreInteration core)
        {
            this._core = core;
            this._core.SendMessageToGuiAction -= new Action<IMessage>(OnUpdateLog);
            this._core.SendMessageToGuiAction += new Action<IMessage>(OnUpdateLog);
            this._core.SendExceptionMessageToGuiAction -= new Action<IMessage>(OnUpdateExceptionLog);
            this._core.SendExceptionMessageToGuiAction += new Action<IMessage>(OnUpdateExceptionLog);
            //this._core.SendExceptionMessageToGuiAction -= new Action<IMessage>(OnUpdateExceptionView);
            //this._core.SendExceptionMessageToGuiAction += new Action<IMessage>(OnUpdateExceptionView);
        }
        public void DisconnectFromCore(ITesterCoreInteration core)
        {
            this._core.SendMessageToGuiAction -= new Action<IMessage>(OnUpdateLog);
            this._core.SendExceptionMessageToGuiAction -= new Action<IMessage>(OnUpdateExceptionLog);
            this._core = null;

        }

        void UpdateExceptionBoardTips(object eCount)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((EventHandler)delegate
                {
                    this.tp_exceptionLog.Text = $"异常信息:[{eCount}]条";
                });
            }
            else
            {
                this.tp_exceptionLog.Text = $"异常信息:[{eCount}]条";
            }

        }
        void OnUpdateLog(IMessage msg)
        {
            switch (msg.Type)
            {
                case EnumMessageType.Internal:
                    break;
                default:
                    {
                        UpdateLog(msg);
                    }
                    break;
            }
        }
        void UpdateLog(IMessage msg)
        {
            //if (this.InvokeRequired)
            //{
            this.BeginInvoke((EventHandler)delegate
            {
                try
                {
                    if (this.logLineCount > 500)
                    {
                        lock (this.rtb_Log)
                        {
                            this.rtb_Log.Clear();
                            this.logLineCount = 0;
                            this.rtb_Log.AppendText("\n" + msg.Message);
                            //this.rtb_Log.Select(this.rtb_Log.TextLength, 0);
                            this.rtb_Log.ScrollToCaret();
                            this.logLineCount++;
                        }
                    }
                    else
                    {
                        this.rtb_Log.AppendText("\n" + msg.Message);
                        //this.rtb_Log.Select(this.rtb_Log.TextLength, 0);
                        this.rtb_Log.ScrollToCaret();
                        this.logLineCount++;
                    }

                }
                catch (Exception ex)
                {
                }
            });
            //}
            //else
            //{
            //    try
            //    {
            //        this.rtb_Log.AppendText("\n" + msg.Message);
            //        //this.rtb_Log.Select(this.rtb_Log.TextLength, 0);
            //        this.rtb_Log.ScrollToCaret();
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //}


            //if (this.InvokeRequired)
            //{
            //    this.Invoke((EventHandler)delegate
            //    {
            //        try
            //        {
            //            this.rtb_Log.AppendText("\n" + msg.Message);
            //            //this.rtb_Log.Select(this.rtb_Log.TextLength, 0);
            //            this.rtb_Log.ScrollToCaret();
            //        }
            //        catch (Exception ex)
            //        {
            //        }
            //    });
            //}
            //else
            //{
            //    try
            //    {
            //        this.rtb_Log.AppendText("\n" + msg.Message);
            //        //this.rtb_Log.Select(this.rtb_Log.TextLength, 0);
            //        this.rtb_Log.ScrollToCaret();
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //}
        }
        void OnUpdateExceptionLog(IMessage msg)
        {
            switch (msg.Type)
            {
                case EnumMessageType.Internal:
                    {
                        var iMsg = msg as InternalMessage;
                        switch (iMsg.OperationType)
                        {
                            case InternalOperationType.ExceptionCountChanged:
                                UpdateExceptionBoardTips(iMsg.Context);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case EnumMessageType.Exception:
                    {
                        UpdateExceptionLog(msg);
                    }
                    break;
                default:
                    {
                    
                    }
                    break;
            }
        }

        void UpdateExceptionLog(IMessage msg)
        {
            //if (this.rtb_exceptionLog.InvokeRequired)
            //{
            this.rtb_exceptionLog.BeginInvoke((EventHandler)delegate
            {
                try
                {
                    if (this.exceptionLineCount > 500)
                    {
                        lock (this.rtb_exceptionLog)
                        {
                            this.rtb_exceptionLog.Clear();
                            this.exceptionLineCount = 0;
                            this.rtb_exceptionLog.AppendText("\n" + msg.Message);
                            //this.rtb_Log.Select(this.rtb_Log.TextLength, 0);
                            this.rtb_exceptionLog.ScrollToCaret();
                            this.exceptionLineCount++;
                        }
                    }
                    else
                    {
                        this.rtb_exceptionLog.AppendText("\n" + msg.Message);
                        //this.rtb_Log.Select(this.rtb_Log.TextLength, 0);
                        this.rtb_exceptionLog.ScrollToCaret();
                        this.exceptionLineCount++;
                    }

                }
                catch (Exception)
                {
                }
            });
            //}
            //else
            //{
            //    try
            //    {
            //        this.rtb_exceptionLog.AppendText("\n" + msg.ToString());
            //        this.rtb_exceptionLog.ScrollToCaret();
            //    }
            //    catch (Exception)
            //    {
            //    }
            //}
        }
        void OnUpdateExceptionView(IMessage msg)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((EventHandler)delegate
                {
                    UpdateExceptionView(msg);
                });
            }
            else
            {
                UpdateExceptionView(msg);
            }
        }

        void UpdateExceptionView(IMessage msg)
        {
            try
            {
                if (msg is ExceptionMessage)
                {
                    //var exMsg = msg as ExceptionMessage;
                    //var rIdx = this.dgv_exceptionView.Rows.Add();
                    //this.dgv_exceptionView.Rows[rIdx].SetValues(exMsg.DateTimeString, exMsg.ErrorCode, exMsg.Message);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
            }
        }
        private void Form_MessageBorad_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this._core.DockingMessageBoard();
            //this._core.SendToCore(new InternalMessage("", InternalOperationType.UserRequest_DockMessageBoard, null));
        }

        private void rtb_Log_MouseClick(object sender, MouseEventArgs e)
        {

        }
        private void MessageBoardHandle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.cms_MessageBoard.Show(MousePosition.X, MousePosition.Y);
            }
        }
        private void 窗口浮动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.Parent.Controls.Count > 0)
                {
                    this.Parent.Controls.Clear();
                }
                this._core.PopUI(this);
            }
            catch (Exception ex)
            {
            }
        }
        private void 窗口停靠ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //tab_auxiUI
                this._core.DockingMessageBoard();
            }
            catch (Exception ex)
            {

            }
        }
    }
}