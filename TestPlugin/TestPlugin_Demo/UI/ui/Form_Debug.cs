using LX_BurnInSolution.Utilities;
using SolveWare_TestPlugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestPlugin_Demo
{
    public partial class Form_Debug : Form_MainPage_TestPlugin<TestPluginWorker_CT3103>
    {
        System.Timers.Timer Axes_refreshTimer;
        System.Timers.Timer IO_refreshTimer;
        Dictionary<string, Control> m_dict;
        Color col_on = Color.LimeGreen;
        Color col_off = Color.OrangeRed;


        public Form_Debug()
        {
            InitializeComponent();
            Axes_refreshTimer = new System.Timers.Timer(200);
            Axes_refreshTimer.Elapsed += Axes_refreshTimer_Elapsed;
            IO_refreshTimer = new System.Timers.Timer(200);
            IO_refreshTimer.Elapsed += IO_refreshTimer_Elapsed;

            m_dict = GuiExternFuns.LX_GetFormControls(this);
        }

        private void Form_Debug_Load(object sender, EventArgs e)
        {

            置顶ToolStripMenuItem.Visible = false;
            取消置顶ToolStripMenuItem.Visible = false;
            窗口停靠ToolStripMenuItem.Visible = false;
        }

        private void Form_Debug_FormClosing(object sender, FormClosingEventArgs e)
        {
            debugShow();
        }

        #region RefresUI
        private void Axes_refreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Axes_refreshTimer.Stop();
            try
            {
                RefreshGUI_Axes();
            }
            catch
            {

            }
            Axes_refreshTimer.Start();

        }
        private void IO_refreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            IO_refreshTimer.Stop();
            try
            {
                RefreshGUI_IO();
            }
            catch
            {

            }
            IO_refreshTimer.Start();

        }
        private void RefreshGUI_Axes()
        {
            this.Invoke((EventHandler)delegate
            {
                var Axec = Enum.GetNames(typeof(AxisNameEnum_CT3103));
                for (int i = 0; i < Axec.Length; i++)
                {
                    var axec = (AxisNameEnum_CT3103)Enum.Parse(typeof(AxisNameEnum_CT3103), Axec[i]);
                    string el_p = string.Format("btn_Axis_{0}_P_Limit", axec);
                    string el_n = string.Format("btn_Axis_{0}_N_Limit", axec);
                    string absPos = string.Format("tb_Axis_{0}_ABSPOS", axec);
                    if (this.m_dict.ContainsKey(el_p))
                    {
                        var ctrl = GuiExternFuns.LX_GetCtrl<Button>(el_p, this.m_dict);
                        ctrl.BackColor = this._plugin.LocalResource.Axes[axec].Get_PEL_Signal() ? col_on : col_off;
                    }
                    if (this.m_dict.ContainsKey(el_n))
                    {
                        var ctrl = GuiExternFuns.LX_GetCtrl<Button>(el_n, this.m_dict);
                        ctrl.BackColor = this._plugin.LocalResource.Axes[axec].Get_NEL_Signal() ? col_on : col_off;
                    }
                    if (this.m_dict.ContainsKey(absPos))
                    {
                        var ctrl = GuiExternFuns.LX_GetCtrl<TextBox>(absPos, this.m_dict);
                        ctrl.Text = this._plugin.LocalResource.Axes[axec].Get_CurUnitPos().ToString("F5");
                    }
                }


            });
        }
        private void RefreshGUI_IO()
        {
            this.Invoke((EventHandler)delegate
            {
                //var io1 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_TEST_HEAD_L].Interation.IsActive;
                //if (io1)
                //{
                //    this.btn_SEN_TEST_HEAD_L.BackColor = col_on;
                //}
                //else
                //{
                //    this.btn_SEN_TEST_HEAD_L.BackColor = col_off;
                //}
                //var io2 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_TEST_HEAD_R].Interation.IsActive;
                //if (io2)
                //{
                //    this.btn_SEN_TEST_HEAD_R.BackColor = col_on;
                //}
                //else
                //{
                //    this.btn_SEN_TEST_HEAD_R.BackColor = col_off;
                //}
                var io3 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_FLOW].Interation.IsActive;
                if (io3)
                {
                    this.btn_SEN_FLOW.BackColor = col_on;
                }
                else
                {
                    this.btn_SEN_FLOW.BackColor = col_off;
                }
                var io4 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_PICKER_ROTATE_L].Interation.IsActive;
                if (io4)
                {
                    this.btn_SEN_PICKER_ROTATE_L.BackColor = col_on;
                }
                else
                {
                    this.btn_SEN_PICKER_ROTATE_L.BackColor = col_off;
                }
                var io5 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_PICKER_ROTATE_R].Interation.IsActive;
                if (io5)
                {
                    this.btn_SEN_PICKER_ROTATE_R.BackColor = col_on;
                }
                else
                {
                    this.btn_SEN_PICKER_ROTATE_R.BackColor = col_off;
                }
                var io6 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_PICKER_ROTATE_L].Interation.IsActive;
                if (io6)
                {
                    this.btn_SEN_PICKER_ROTATE_L.BackColor = col_on;
                }
                else
                {
                    this.btn_SEN_PICKER_ROTATE_L.BackColor = col_off;
                }
                var io7 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_STG_L_CARRIER_UP].Interation.IsActive;
                if (io7)
                {
                    this.btn_SEN_STG_L_CARRIER_UP.BackColor = col_on;
                }
                else
                {
                    this.btn_SEN_STG_L_CARRIER_UP.BackColor = col_off;
                }
                var io8 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_STG_L_CARRIER_DN].Interation.IsActive;
                if (io8)
                {
                    this.btn_SEN_STG_L_CARRIER_DN.BackColor = col_on;
                }
                else
                {
                    this.btn_SEN_STG_L_CARRIER_DN.BackColor = col_off;
                }
                var io9 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_STG_L_CARRIER_LOAD].Interation.IsActive;
                if (io9)
                {
                    this.btn_SEN_STG_L_CARRIER_LOAD.BackColor = col_on;
                }
                else
                {
                    this.btn_SEN_STG_L_CARRIER_LOAD.BackColor = col_off;
                }
                //var io10 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_STG_L_HOLDER_UP].Interation.IsActive;
                //if (io10)
                //{
                //    this.btn_SEN_STG_L_HOLDER_UP.BackColor = col_on;
                //}
                //else
                //{
                //    this.btn_SEN_STG_L_HOLDER_UP.BackColor = col_off;
                //}
                //var io11 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_STG_L_HOLDER_DN].Interation.IsActive;
                //if (io11)
                //{
                //    this.btn_SEN_STG_L_HOLDER_DN.BackColor = col_on;
                //}
                //else
                //{
                //    this.btn_SEN_STG_L_HOLDER_DN.BackColor = col_off;
                //}
                var io12 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_Vacuum_L].Interation.IsActive;
                if (io12)
                {
                    this.btn_Vacuum_L.BackColor = col_on;
                }
                else
                {
                    this.btn_Vacuum_L.BackColor = col_off;
                }
                var io13 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_STG_R_CARRIER_UP].Interation.IsActive;
                if (io13)
                {
                    this.btn_SEN_STG_R_CARRIER_UP.BackColor = col_on;
                }
                else
                {
                    this.btn_SEN_STG_R_CARRIER_UP.BackColor = col_off;
                }
                var io14 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_STG_R_CARRIER_DN].Interation.IsActive;
                if (io14)
                {
                    this.btn_SEN_STG_R_CARRIER_DN.BackColor = col_on;
                }
                else
                {
                    this.btn_SEN_STG_R_CARRIER_DN.BackColor = col_off;
                }
                var io15 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_STG_R_CARRIER_LOAD].Interation.IsActive;
                if (io15)
                {
                    this.btn_SEN_STG_R_CARRIER_LOAD.BackColor = col_on;
                }
                else
                {
                    this.btn_SEN_STG_R_CARRIER_LOAD.BackColor = col_off;
                }
                //var io16 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_STG_R_HOLDER_UP].Interation.IsActive;
                //if (io16)
                //{
                //    this.btn_SEN_STG_R_HOLDER_UP.BackColor = col_on;
                //}
                //else
                //{
                //    this.btn_SEN_STG_R_HOLDER_UP.BackColor = col_off;
                //}
                //var io17 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_STG_R_HOLDER_DN].Interation.IsActive;
                //if (io17)
                //{
                //    this.btn_SEN_STG_R_HOLDER_DN.BackColor = col_on;
                //}
                //else
                //{
                //    this.btn_SEN_STG_R_HOLDER_DN.BackColor = col_off;
                //}
                var io18 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_Vacuum_R].Interation.IsActive;
                if (io18)
                {
                    this.btn_Vacuum_R.BackColor = col_on;
                }
                else
                {
                    this.btn_Vacuum_R.BackColor = col_off;
                }
                var io19 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_PICKER_UP].Interation.IsActive;
                if (io19)
                {
                    this.btn_SEN_PICKER_UP.BackColor = col_on;
                }
                else
                {
                    this.btn_SEN_PICKER_UP.BackColor = col_off;
                }
                var io20 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_PICKER_DN].Interation.IsActive;
                if (io20)
                {
                    this.btn_SEN_PICKER_DN.BackColor = col_on;
                }
                else
                {
                    this.btn_SEN_PICKER_DN.BackColor = col_off;
                }
                var io21 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_TRAY_OUT_UP].Interation.IsActive;
                if (io21)
                {
                    this.btn_SEN_TRAY_OUT_UP.BackColor = col_on;
                }
                else
                {
                    this.btn_SEN_TRAY_OUT_UP.BackColor = col_off;
                }
                var io22 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_TRAY_IN_UP].Interation.IsActive;
                if (io22)
                {
                    this.btn_SEN_TRAY_IN_UP.BackColor = col_on;
                }
                else
                {
                    this.btn_SEN_TRAY_IN_UP.BackColor = col_off;
                }
                var io23 = this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_PICKER_GRAB_ON].Interation.IsActive;
                if (io23)
                {
                    this.btn_SEN_PICKER_GRAB_ON.Text = "抓手状态[收紧]";
                }
                else
                {
                    this.btn_SEN_PICKER_GRAB_ON.Text = "抓手状态[松开]";
                }

            });
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TabPage tabPage = tabControl1.SelectedTab;
            if (tabPage.Text == "轴")
            {
                Axes_refreshTimer.Start();
            }
            else
            {
                Axes_refreshTimer.Stop();
            }
            if (tabPage.Text == "IO")
            {
                IO_refreshTimer.Start();
            }
            else
            {
                IO_refreshTimer.Stop();
            }
        }
        #endregion

        #region  菜单栏
        string pass = "";
        void doc_EventCloseWindow(string pwd)
        {
            if (pwd == "yes")
            {
                pass = "yes";
            }
        }

        private void 进入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pass = "";
            Form_EngPassword frmEngPassword = new Form_EngPassword();
            frmEngPassword.EventCloseWindow += new DCloseWindow(doc_EventCloseWindow);
            frmEngPassword.ShowDialog();
            if (pass == "yes")
            {
                //this.tabTestPlan.Parent = tabControl1;
                //this.tabPage2.Parent = tabControl1;
                //this.tabPage5.Parent = tabControl1;
                //this.tabPage6.Parent = tabControl1;
                //this.tabPage7.Parent = tabControl1;
                //this.tabPage8.Parent = tabControl1;
                //this.tabControl1.SelectedIndex = 3;

                //this.进入ToolStripMenuItem.Enabled = false;
                //this.退出ToolStripMenuItem.Enabled = true;
            }

        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            //this.进入ToolStripMenuItem.Enabled = true;
            //this.退出ToolStripMenuItem.Enabled = false;
            //tabPage2.Parent = null;
            ////tabPage4.Parent = null;
            //tabPage5.Parent = null;
            //tabPage6.Parent = null;
            //tabPage7.Parent = null;
            //tabPage8.Parent = null;
            //tabTestPlan.Parent = null;
        }
        private void 窗口浮动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //this._core.PopUI(this);
                this._core.PopUI(this);
                置顶ToolStripMenuItem.Visible = true;
                取消置顶ToolStripMenuItem.Visible = true;
                窗口停靠ToolStripMenuItem.Visible = true;
            }
            catch (Exception ex)
            {
            }
        }
        public delegate void DebugShow();
        public event DebugShow debugShow;
        private void 窗口停靠ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //this._core.DockingMessageBoard();
                debugShow();
                this.Close();
            }
            catch (Exception ex)
            {

            }
        }

        private void 置顶ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.TopMost = true;
            }
            catch (Exception ex)
            {

            }
        }

        private void 取消置顶ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.TopMost = false;
            }
            catch (Exception ex)
            {

            }
        }


        #endregion

        #region 复位、急停
        private void pb_EmergencyStop_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.StopAllMotionAction();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}!");
            }
        }

        private void bt_home_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认是否进行整机台复位", "复位确认", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Frm_ResetPlatform frm = new Frm_ResetPlatform();
                this._plugin.Reset_HomeStation();
                frm.ConnectToAppInteration(this._plugin);
                frm.ConnectToCore(this._core);
                frm.Homestation += new Frm_ResetPlatform.HomeStation(this._plugin.Run_HomeStation);
                frm.CancelHome += new Frm_ResetPlatform.CancelHomeStation(this._plugin.Cancel_HomeStation);
                frm.LocalResource = this._plugin.LocalResource;
                frm.ShowDialog();
            }
        }

        private void btn_CancelHomeStation_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.Cancel_HomeStation();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}!");
            }
        }
        #endregion

        #region IO控制
        private void btn_opera_CYL_TEST_HEAD_L_Click(object sender, EventArgs e)
        {
            try
            {
                //this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_TEST_HEAD_L].TurnOn(true);
                //this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_TEST_HEAD_R].TurnOn(false);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_CYL_TEST_HEAD_R_Click(object sender, EventArgs e)
        {
            try
            {
                //this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_TEST_HEAD_L].TurnOn(false);
                //this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_TEST_HEAD_R].TurnOn(true);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_CYL_PICKER_ROTATE_L_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_ROTATE_L].TurnOn(true);
                this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_ROTATE_R].TurnOn(false);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_CYL_PICKER_ROTATE_R_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_ROTATE_L].TurnOn(false);
                this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_ROTATE_R].TurnOn(true);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_STG_L_HOLDER_UP_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.IOs[IONameEnum_CT3103.Pedestal_Vacuum_L].TurnOn(false);
                //    this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_STG_L_HOLDER_UP].TurnOn(true);
                //    this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_STG_L_HOLDER_DN].TurnOn(false);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_STG_L_HOLDER_DN_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.IOs[IONameEnum_CT3103.Pedestal_Vacuum_L].TurnOn(true);
                //this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_STG_L_HOLDER_UP].TurnOn(false);
                //this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_STG_L_HOLDER_DN].TurnOn(true);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_STG_R_HOLDER_UP_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.IOs[IONameEnum_CT3103.Pedestal_Vacuum_R].TurnOn(false);
                //this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_STG_R_HOLDER_UP].TurnOn(true);
                //this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_STG_R_HOLDER_DN].TurnOn(false);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_STG_R_HOLDER_DN_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.IOs[IONameEnum_CT3103.Pedestal_Vacuum_R].TurnOn(true);
                //this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_STG_R_HOLDER_UP].TurnOn(false);
                //this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_STG_R_HOLDER_DN].TurnOn(true);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_CYL_PICKER_UP_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_UP].TurnOn(true);
                this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_DN].TurnOn(false);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_CYL_PICKER_DN_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_UP].TurnOn(false);
                this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_DN].TurnOn(true);
            }
            catch (Exception ex)
            {

            }
        }
        private void btn_opera_CYL_PICKER_GRAB_ON_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_GRAB_ON].TurnOn(true);
                this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_GRAB_OFF].TurnOn(false);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_CYL_PICKER_GRAB_OFF_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_GRAB_ON].TurnOn(false);
                this._plugin.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_GRAB_OFF].TurnOn(true);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region 轴控制
        private void btn_opera_Axis_LY_P_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LY].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.LY].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LY], position + step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LY_P_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LY].Jog(true);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LY_P_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LY].Stop();
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LY_N_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LY].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.LY].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LY], position - step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LY_N_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LY].Jog(false);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LY_N_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LY].Stop();
            }
            catch (Exception ex)
            {

            }
        }
        private void btn_opera_Axis_LX_P_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LX].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.LX].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LX], position + step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LX_P_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LX].Jog(true);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LX_P_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LX].Stop();
            }
            catch (Exception ex)
            {

            }
        }
        private void btn_opera_Axis_LX_N_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LX].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.LX].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LX], position - step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LX_N_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LX].Jog(false);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LX_N_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LX].Stop();
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_home_Axis_LX_Click(object sender, EventArgs e)
        {
            try
            {
                this.btn_home_Axis_LX.Enabled = false;
                Task.Factory.StartNew(() =>
                {
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LX].HomeRun();
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LX].WaitHomeDone(new System.Threading.CancellationTokenSource());
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LX].SetCurrentPositionToZero();

                    this.Invoke((EventHandler)delegate
                    {
                        this.btn_home_Axis_LX.Enabled = true;
                    });
                });

            }
            catch (Exception ex)
            {

            }
        }

        private void btn_home_Axis_LY_Click(object sender, EventArgs e)
        {
            try
            {
                this.btn_home_Axis_LY.Enabled = false;
                Task.Factory.StartNew(() =>
                {
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LY].HomeRun();
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LY].WaitHomeDone(new System.Threading.CancellationTokenSource());
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LY].SetCurrentPositionToZero();

                    this.Invoke((EventHandler)delegate
                    {
                        this.btn_home_Axis_LY.Enabled = true;
                    });
                });

            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LZ_P_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LZ].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.LZ].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LZ], position + step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LZ_P_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LZ].Jog(true);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LZ_P_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LZ].Stop();
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LZ_N_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LZ].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.LZ].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LZ], position - step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LZ_N_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LZ].Jog(false);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LZ_N_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LZ].Stop();
            }
            catch (Exception ex)
            {

            }
        }
        private void btn_home_Axis_LZ_Click(object sender, EventArgs e)
        {
            try
            {
                this.btn_home_Axis_LZ.Enabled = false;
                Task.Factory.StartNew(() =>
                {
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LZ].HomeRun();
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LZ].WaitHomeDone(new System.Threading.CancellationTokenSource());
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LZ].SetCurrentPositionToZero();

                    this.Invoke((EventHandler)delegate
                    {
                        this.btn_home_Axis_LZ.Enabled = true;
                    });
                });

            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_T_IN_Z_P_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.T_IN_Z].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.T_IN_Z].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.T_IN_Z], position + step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_T_IN_Z_P_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.T_IN_Z].Jog(true);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_T_IN_Z_P_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.T_IN_Z].Stop();
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_T_IN_Z_N_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.T_IN_Z].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.T_IN_Z].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.T_IN_Z], position - step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_T_IN_Z_N_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.T_IN_Z].Jog(false);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_T_IN_Z_N_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.T_IN_Z].Stop();
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_home_Axis_T_IN_Z_Click(object sender, EventArgs e)
        {
            try
            {
                this.btn_home_Axis_T_IN_Z.Enabled = false;
                Task.Factory.StartNew(() =>
                {
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.T_IN_Z].HomeRun();
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.T_IN_Z].WaitHomeDone(new System.Threading.CancellationTokenSource());
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.T_IN_Z].SetCurrentPositionToZero();

                    this.Invoke((EventHandler)delegate
                    {
                        this.btn_home_Axis_T_IN_Z.Enabled = true;
                    });
                });

            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_T_OUT_Z_P_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.T_OUT_Z].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.T_OUT_Z].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.T_OUT_Z], position + step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_T_OUT_Z_P_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.T_OUT_Z].Jog(true);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_T_OUT_Z_P_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.T_OUT_Z].Stop();
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_T_OUT_Z_N_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.T_OUT_Z].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.T_OUT_Z].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.T_OUT_Z], position - step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_T_OUT_Z_N_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.T_OUT_Z].Jog(false);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_T_OUT_Z_N_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.T_OUT_Z].Stop();
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_home_Axis_T_OUT_Z_Click(object sender, EventArgs e)
        {
            try
            {
                this.btn_home_Axis_T_OUT_Z.Enabled = false;
                Task.Factory.StartNew(() =>
                {
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.T_OUT_Z].HomeRun();
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.T_OUT_Z].WaitHomeDone(new System.Threading.CancellationTokenSource());
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.T_OUT_Z].SetCurrentPositionToZero();

                    this.Invoke((EventHandler)delegate
                    {
                        this.btn_home_Axis_T_OUT_Z.Enabled = true;
                    });
                });

            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_RY_P_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RY].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.RY].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RY], position + step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_RY_P_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RY].Jog(true);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_RY_P_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RY].Stop();
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_RY_N_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RY].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.RY].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RY], position - step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_RY_N_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RY].Jog(false);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_RY_N_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RY].Stop();
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_home_Axis_RY_Click(object sender, EventArgs e)
        {
            try
            {
                this.btn_home_Axis_RY.Enabled = false;
                Task.Factory.StartNew(() =>
                {
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RY].HomeRun();
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RY].WaitHomeDone(new System.Threading.CancellationTokenSource());
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RY].SetCurrentPositionToZero();

                    this.Invoke((EventHandler)delegate
                    {
                        this.btn_home_Axis_RY.Enabled = true;
                    });
                });

            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_RX_P_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RX].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.RX].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RX], position + step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_RX_P_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RX].Jog(true);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_RX_P_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RX].Stop();
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_RX_N_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RX].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.RX].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RX], position - step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_RX_N_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RX].Jog(false);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_RX_N_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RX].Stop();
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_home_Axis_RX_Click(object sender, EventArgs e)
        {
            try
            {
                this.btn_home_Axis_RX.Enabled = false;
                Task.Factory.StartNew(() =>
                {
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RX].HomeRun();
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RX].WaitHomeDone(new System.Threading.CancellationTokenSource());
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RX].SetCurrentPositionToZero();

                    this.Invoke((EventHandler)delegate
                    {
                        this.btn_home_Axis_RX.Enabled = true;
                    });
                });

            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_RZ_P_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RZ].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.RZ].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RZ], position + step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_RZ_P_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RZ].Jog(true);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_RZ_P_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RZ].Stop();
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_RZ_N_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RZ].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.RZ].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RZ], position - step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_RZ_N_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RZ].Jog(false);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_RZ_N_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RZ].Stop();
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_home_Axis_RZ_Click(object sender, EventArgs e)
        {
            try
            {
                this.btn_home_Axis_RZ.Enabled = false;
                Task.Factory.StartNew(() =>
                {
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RZ].HomeRun();
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RZ].WaitHomeDone(new System.Threading.CancellationTokenSource());
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.RZ].SetCurrentPositionToZero();

                    this.Invoke((EventHandler)delegate
                    {
                        this.btn_home_Axis_RZ.Enabled = true;
                    });
                });

            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_Y_P_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.Y].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.Y].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.Y], position + step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_Y_P_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.Y].Jog(true);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_Y_P_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.Y].Stop();
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_Y_N_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.Y].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.Y].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.Y], position - step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_Y_N_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.Y].Jog(false);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_Y_N_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.Y].Stop();
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_home_Axis_Y_Click(object sender, EventArgs e)
        {
            try
            {
                this.btn_home_Axis_Y.Enabled = false;
                Task.Factory.StartNew(() =>
                {
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.Y].HomeRun();
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.Y].WaitHomeDone(new System.Threading.CancellationTokenSource());
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.Y].SetCurrentPositionToZero();

                    this.Invoke((EventHandler)delegate
                    {
                        this.btn_home_Axis_Y.Enabled = true;
                    });
                });

            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LNY_N_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNY].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.LNY].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNY], position - step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LNY_N_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNY].Jog(false);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LNY_N_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNY].Stop();
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LNY_P_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNY].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.LNY].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNY], position + step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LNY_P_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNY].Jog(true);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LNY_P_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNY].Stop();
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_home_Axis_LNY_Click(object sender, EventArgs e)
        {
            try
            {
                this.btn_home_Axis_LNY.Enabled = false;
                Task.Factory.StartNew(() =>
                {
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNY].HomeRun();
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNY].WaitHomeDone(new System.Threading.CancellationTokenSource());
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNY].SetCurrentPositionToZero();

                    this.Invoke((EventHandler)delegate
                    {
                        this.btn_home_Axis_LNY.Enabled = true;
                    });
                });

            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LNX_P_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNX].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.LNX].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNX], position + step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LNX_P_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNX].Jog(true);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LNX_P_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNX].Stop();
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LNX_N_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNX].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.LNX].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNX], position - step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LNX_N_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNX].Jog(false);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LNX_N_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNX].Stop();
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_home_Axis_LNX_Click(object sender, EventArgs e)
        {
            try
            {
                this.btn_home_Axis_LNX.Enabled = false;
                Task.Factory.StartNew(() =>
                {
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNX].HomeRun();
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNX].WaitHomeDone(new System.Threading.CancellationTokenSource());
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNX].SetCurrentPositionToZero();

                    this.Invoke((EventHandler)delegate
                    {
                        this.btn_home_Axis_LNX.Enabled = true;
                    });
                });

            }
            catch (Exception ex)
            {

            }
        }


        private void btn_opera_Axis_LNZ_P_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNZ].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.LNZ].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNZ], position + step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LNZ_P_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNZ].Jog(true);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LNZ_P_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNZ].Stop();
            }
            catch (Exception ex)
            {

            }

        }

        private void btn_opera_Axis_LNZ_N_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_moveMode_0.Checked) return;  //JOG模式
                double step = 0;
                if (rb_moveMode_1.Checked)
                {
                    step = 1;
                }
                else if (rb_moveMode_2.Checked)
                {
                    step = 0.1;
                }
                else if (rb_moveMode_3.Checked)
                {
                    step = 0.01;
                }
                else if (rb_moveMode_4.Checked)
                {
                    step = 0.001;
                }
                else if (rb_moveMode_5.Checked)
                {
                    step = 0.0001;
                }
                var position = this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNZ].Get_CurUnitPos();
                this._plugin.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.LNZ].SingleAxisMotion(this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNZ], position - step);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LNZ_N_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNZ].Jog(false);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_opera_Axis_LNZ_N_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.rb_moveMode_0.Checked) return;  //JOG模式

                this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNZ].Stop();
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_home_Axis_LNZ_Click(object sender, EventArgs e)
        {
            try
            {
                this.btn_home_Axis_LNZ.Enabled = false;
                Task.Factory.StartNew(() =>
                {
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNZ].HomeRun();
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNZ].WaitHomeDone(new System.Threading.CancellationTokenSource());
                    this._plugin.LocalResource.Axes[AxisNameEnum_CT3103.LNZ].SetCurrentPositionToZero();

                    this.Invoke((EventHandler)delegate
                    {
                        this.btn_home_Axis_LNZ.Enabled = true;
                    });
                });

            }
            catch (Exception ex)
            {

            }
        }


        #endregion

        #region 载台扎针位工站
        private void bt_left_Click(object sender, EventArgs e)
        {
            try
            {
                var ssi = this._plugin.GetSlaveStation_Initializer("左载台模组");
                if (ssi == null)
                {
                    return;
                }

                SolveWare_SlaveStation.Form_SlaveStationUI form_SlaveStationUI = new SolveWare_SlaveStation.Form_SlaveStationUI();

                form_SlaveStationUI.RunInitializer
                    (
                        ssi.Name,
                        ssi.MotorAxisBases,
                        ssi.DirectionReverseList,
                        ssi.AxisPositions,
                        ssi.IOBases,
                        this._plugin.LocalResource.Local_AxesPosition_ResourceProvider,
                                       null
                    );

                form_SlaveStationUI.Show();
            }
            catch
            {

            }
        }

        private void bt_right_Click(object sender, EventArgs e)
        {
            try
            {
                var ssi = this._plugin.GetSlaveStation_Initializer("右载台模组");
                if (ssi == null)
                {
                    return;
                }

                SolveWare_SlaveStation.Form_SlaveStationUI form_SlaveStationUI = new SolveWare_SlaveStation.Form_SlaveStationUI();

                form_SlaveStationUI.RunInitializer
                    (
                        ssi.Name,
                        ssi.MotorAxisBases,
                        ssi.DirectionReverseList,
                        ssi.AxisPositions,
                        ssi.IOBases,
                        this._plugin.LocalResource.Local_AxesPosition_ResourceProvider,
                                       null
                    );

                form_SlaveStationUI.Show();
            }
            catch
            {

            }
        }
        #endregion

    }
}
