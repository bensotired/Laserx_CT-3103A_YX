using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_TestPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestPlugin_Demo
{
    using LX_Utilities;
    using Microsoft.WindowsAPICodePack.Dialogs;
    using MySql.Data.MySqlClient;
    using OpenCvSharp;
    using SolveWare_BurnInInstruments;
    using System.Data;
    using System.IO;
    using System.Text;
    using TestPlugin_CoarseTuning;

    public partial class Form_MainPage_CT3103 : Form_MainPage_TestPlugin<TestPluginWorker_CT3103>
    {

        Form _enUI;

        Form _rtUI;

        Form _binUI;

        Form _outPutSettingsUI;


        int delay = 100;


        //DelayTimeSettings delayTimeSettings;

        //MotionOffsetSettings motionOffsetSettings;

        //private Camera camera = null;

        private VideoCapture capture;
        private bool isopen = false;
        private bool saveImgFlag = false;
        private string filePath = "";
        public Form_MainPage_CT3103()
        {
            InitializeComponent();
        }

        private void Form_MainPage_CT3103_Load(object sender, EventArgs e)
        {
            try
            {
                //测试入口UI
                _enUI = this._plugin.GetTestEnteranceUI(true);
                pnl_TestEnterance.Controls.Clear();
                pnl_TestEnterance.Controls.Add(_enUI);
                _enUI.Show();

                //测试数据浏览UI
                _rtUI = this._plugin.GetRuntimeOverviewUI(true);
                pnl_RuntimeOverviewPage.Controls.Clear();
                pnl_RuntimeOverviewPage.Controls.Add(_rtUI);
                _rtUI.Show();

                OpenCVShow();
                DebugShow();
                CoarseTuningShow();
                //tabPage调试.Parent = null;

                this._plugin.SqlConn = this._plugin.Dser();
                IsConn();

                timer_Engineer.Enabled = true;
                timer_Engineer.Interval = 200;

                tabPage数据库.Parent = null;

                if (this.APL == AccessPermissionLevel.None)
                {
                    Task.Factory.StartNew(() =>
                    {
                        this._plugin.Log_Global("请登录后使用！！！");
                        //MessageBox.Show($"请登录后使用！！！");
                        Form_Log log = new Form_Log();
                        log.ShowDialog();
                    });

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}!");
            }
        }


        public override void RefreshOnce()
        {
            try
            {
                (this._rtUI as ITesterAppUI)?.RefreshOnce();
                (this._enUI as ITesterAppUI)?.RefreshOnce();
                (this._binUI as ITesterAppUI)?.RefreshOnce();
                (this._outPutSettingsUI as ITesterAppUI)?.RefreshOnce();
            }
            catch (Exception ex)
            {

            }
        }
        private void tb_MainPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                (this._rtUI as ITesterAppUI)?.RefreshOnce();
                (this._enUI as ITesterAppUI)?.RefreshOnce();
                (this._binUI as ITesterAppUI)?.RefreshOnce();
                (this._outPutSettingsUI as ITesterAppUI)?.RefreshOnce();

                //TabPage tabPage = tb_MainPage.SelectedTab;

                //if (tabPage.Text == "单步调试")
                //{
                //    if (this._plugin.Interation.RunStatus != PluginRunStatus.Idle)
                //    {
                //        MessageBox.Show("请在单步调试前进行复位操作！！！");
                //    }

                //}
            }
            catch (Exception ex)
            {

            }
        }

        #region 工程模式 放弃使用
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
                tabPage调试.Parent = tb_MainPage;
                tb_MainPage.SelectedIndex = 3;
                //this.进入ToolStripMenuItem.Enabled = false;
                //this.退出ToolStripMenuItem.Enabled = true;
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tb_MainPage.SelectedIndex = 0;
            //this.进入ToolStripMenuItem.Enabled = true;
            //this.退出ToolStripMenuItem.Enabled = false;
            tabPage调试.Parent = null;
        }


        #endregion

        #region DGV通用方法

        const int INFO_COL_INDEX = 0;
        const int KEY_COL_INDEX = 1;
        const int VAL_COL_INDEX = 2;

        public void Updatedgv(DataGridView dgv, object sourceObject, int infoColIndex, int keyColIndex, int valColIndex)
        {
            try
            {
                dgv.Rows.Clear();
                UIGeneric.FillListDGV_InfoKeyValue(dgv, sourceObject, infoColIndex, keyColIndex, valColIndex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public Dictionary<string, object> GetDataFromPdgv(DataGridView dgv, int keyColIndex, int valColIndex)
        {
            try
            {
                return UIGeneric.Grab_DGV_KeyValueDict(dgv, keyColIndex, valColIndex);
            }
            catch (Exception ex)
            {
                throw new Exception($"当前界面获取数据异常，异常原因：[{ex.Message}]");
            }
        }


        public void UpdateLastRecipe(object sourceObject, Dictionary<string, object> dict)
        {
            try
            {
                ReflectionTool.SetPropertyValues(sourceObject, dict);
            }
            catch (Exception ex)
            {
                throw new Exception("更新Recipe数据异常，异常原因：" + ex.Message);
            }
        }


        private void Dgv_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                var dgv = sender as DataGridView;
                if (e.Exception is System.ArgumentException)
                {
                    if (dgv[e.ColumnIndex, e.RowIndex] is DataGridViewComboBoxCell)
                    {
                        var value = dgv[e.ColumnIndex, e.RowIndex].Value;
                        var valueType = (dgv[e.ColumnIndex, e.RowIndex].Tag as PropertyInfo).PropertyType;

                        if (valueType.IsEnum)
                        {
                            dgv[e.ColumnIndex, e.RowIndex].Value = Converter.ConvertObjectTo(value, valueType).ToString();
                        }
                        else
                        {
                            dgv[e.ColumnIndex, e.RowIndex].Value = Converter.ConvertObjectTo(value, valueType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        #endregion


        #region 综合功能 废弃

        private void btn_RunResourceLauncher_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.RunResourceProvider();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}");
            }
        }
        private void bt_保存New配置_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.providerResourse.SwitchProductConfig();
                this._plugin.Log_Global("保存新配置信息成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}");
            }
        }


        private void btn_HomeStation_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认是否进行整机台复位", "复位确认", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Frm_ResetPlatform frm = new Frm_ResetPlatform();
                this._plugin.Reset_HomeStation();
                frm.ConnectToAppInteration(this._plugin);
                frm.ConnectToCore(this._core);
                frm.Homestation += new Frm_ResetPlatform.HomeStation(this._plugin.Run_HomeStation);
                frm.LocalResource = this._plugin.LocalResource;
                frm.ShowDialog();

                //this._plugin.Reset_HomeStation();
                //Task.Factory.StartNew(() =>
                //{
                //    try
                //    {
                //        var isHomeOk = this._plugin.Run_HomeStation();
                //        //UpdateData();RunParamSettings
                //        this.Invoke((EventHandler)delegate
                //        {
                //            Update_DGV_DelayTimeSettings();
                //            Update_DGV_MotionOffsetSettings();
                //        });

                //        if (isHomeOk == true)
                //        {
                //            MessageBox.Show("整机台复位结束！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                //            //MessageBox.Show("整机台复位结束！");
                //        }
                //        else
                //        {
                //            MessageBox.Show("整机台复位失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                //            //MessageBox.Show("整机台复位失败！");
                //        }

                //    }
                //    catch (Exception ex)
                //    {
                //        MessageBox.Show($"整机台复位错误:[{ex.Message}-{ex.StackTrace}]!");
                //    }
                //});
            }
            else
            {

            }
        }


        private void btn_CancelHomeStation_Click(object sender, EventArgs e)
        {
            this._plugin.Cancel_HomeStation();
        }



        private void bt_上料配置_Click(object sender, EventArgs e)
        {
            try
            {
                //var frm_InPutSettingsEditor = new Frm_InPutSettingsEditor();
                //frm_InPutSettingsEditor.ConnectToAppInteration(this._plugin);
                //frm_InPutSettingsEditor.ConnectToCore(this._core);
                //var dr = frm_InPutSettingsEditor.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"上料盘信息编辑窗口打开错误:[{ex.Message}-{ex.StackTrace}]!");
            }
        }

        private void bt_放料配置_Click(object sender, EventArgs e)
        {
            try
            {
                //var frm_OutPutSettingsEditor = new Frm_OutPutSettingsEditor_FixSorter();
                //frm_OutPutSettingsEditor.ConnectToAppInteration(this._plugin);
                //frm_OutPutSettingsEditor.ConnectToCore(this._core);
                //var dr = frm_OutPutSettingsEditor.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"放料盘信息编辑窗口打开错误:[{ex.Message}-{ex.StackTrace}]!");
            }
        }



        #endregion

        #region OpenCV 相机  废弃
        //private void hScrollBarExposureTime_Scroll(object sender, ScrollEventArgs e)
        //{
        //    if (capture == null)
        //    {
        //        return;
        //    }
        //    if (!capture.IsOpened())
        //    {
        //        MessageBox.Show("无法打开摄像头");
        //        return;
        //    }

        //    capture.Exposure = hScrollBarExposureTime.Value;
        //    ExposureTimeNowValue.Text = hScrollBarExposureTime.Value.ToString();

        //}

        //private void btnOneshot_Click(object sender, EventArgs e)
        //{
        //    capture = new VideoCapture(0);
        //    if (!capture.IsOpened())
        //    {
        //        MessageBox.Show("无法打开摄像头");
        //        return;
        //    }
        //    isopen = true;
        //    Thread video_th = new Thread(StartCapturing);
        //    video_th.IsBackground = true;
        //    video_th.Start();
        //    btnOneshot.Enabled = false;
        //}
        //public void StartCapturing()
        //{
        //    Mat frame = new Mat();
        //    while (true)
        //    {
        //        try
        //        {
        //            Thread.Sleep(1);
        //            capture.Read(frame);//读取图像帧
        //            if (frame.Empty())
        //            {
        //                break;
        //            }
        //            Bitmap bitmap = BitmapConverter.ToBitmap(frame);
        //            if (saveImgFlag == true)
        //            {
        //                try
        //                {
        //                    bitmap.Save(filePath, ImageFormat.Jpeg);
        //                    saveImgFlag = false;
        //                    MessageBox.Show("保存成功！");
        //                }
        //                catch (Exception ex)
        //                {
        //                }
        //            }
        //            pictureBoxCamera.Image = bitmap;
        //            this.Invoke((EventHandler)delegate
        //            {
        //                pictureBoxCamera.Refresh();
        //            });
        //        }
        //        catch (Exception ex)
        //        {

        //            //throw;
        //        }


        //    }
        //}

        //private void btnContinues_Click(object sender, EventArgs e)
        //{
        //    if (!isopen)
        //    {
        //        MessageBox.Show("未打开摄像头");
        //        return;
        //    }
        //    SaveFileDialog saveFileDialog = new SaveFileDialog();
        //    // 设置对话框的标题
        //    saveFileDialog.Title = "Save Image";

        //    // 设置默认的文件名和文件类型过滤器
        //    saveFileDialog.FileName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
        //    saveFileDialog.Filter = "Image files (*.Png)|*.Png|Image files (*.Jpg)|*.Jpg";

        //    // 显示对话框并获取用户的操作结果
        //    DialogResult result = saveFileDialog.ShowDialog();

        //    if (result == DialogResult.OK)
        //    {
        //        // 用户点击了保存按钮
        //        filePath = saveFileDialog.FileName;
        //        //string filter=saveFileDialog.Filter;

        //        // 在这里进行保存文件的操作，例如：
        //        // File.WriteAllText(filePath, "Hello, world!");

        //        Console.WriteLine("File saved to: " + filePath);
        //    }
        //    else if (result == DialogResult.Cancel)
        //    {
        //        // 用户点击了取消按钮
        //        Console.WriteLine("Save cancelled");
        //    }
        //    saveImgFlag = true;
        //}

        //private void btnStopSnap_Click(object sender, EventArgs e)
        //{
        //    btnOneshot.Enabled = true;
        //    Thread.Sleep(300);
        //    StopSnap();
        //}
        //private void StopSnap()
        //{
        //    if (capture == null)
        //    {
        //        return;
        //    }
        //    try
        //    {
        //        capture.Dispose();
        //        capture = null;
        //    }
        //    catch (Exception exception)
        //    {
        //        this._plugin.Log_Global(exception.Message);
        //    }
        //}

        #endregion

        private void OpenCVShow()
        {
            panel_CV.Controls.Clear();
            Form_OpenCV openCV = new Form_OpenCV();
            openCV.ConnectToAppInteration(this._plugin);
            openCV.ConnectToCore(this._core);
            openCV.opencvshow += new Form_OpenCV.OpenCVShow(OpenCVShow);
            openCV.TopLevel = false;
            openCV.Dock = DockStyle.Fill;
            panel_CV.Controls.Add(openCV);
            openCV.Show();
        }

        private void DebugShow()
        {
            panel_debug.Controls.Clear();
            Form_Debug _Debug = new Form_Debug();
            _Debug.ConnectToAppInteration(this._plugin);
            _Debug.ConnectToCore(this._core);
            _Debug.debugShow += new Form_Debug.DebugShow(DebugShow);
            _Debug.TopLevel = false;
            _Debug.Dock = DockStyle.Fill;
            panel_debug.Controls.Add(_Debug);
            _Debug.Show();
        }
        private void CoarseTuningShow()
        {
            try
            {
                panel_coarsetuning.Controls.Clear();
                Form_Main _Main = new Form_Main();
                _Main.TopLevel = false;
                _Main.Dock = DockStyle.Fill;
                panel_coarsetuning.Controls.Add(_Main);
                _Main.Show();
            }
            catch (Exception ex)
            {
                this._core.Log_Global(ex.Message);
            }

        }


        #region 数据库
        string TableName = string.Empty;
        string PostBIColumn = string.Empty;
        private void GetTableName()
        {
            if (this.rb_GS.Checked)
            {
                TableName = "GoldenSample";
            }
            if (this.rb_PreBI.Checked)
            {
                TableName = "PreBI";
            }
            if (this.rb_PostBI.Checked)
            {
                TableName = "PostBI";

            }
        }
        private void cb_PostBIColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            PostBIColumn = this.cb_PostBIColumn.Text;
        }

        private void btn_GetWO_Click(object sender, EventArgs e)
        {
            try
            {
                GetTableName();
                string sqlCmd = string.Format("Select distinct WorkOrder from LX_DB.dbo.[{0}]", TableName);
                DataSet dt = SqlHelper.ExecuteDataset(this._plugin.SqlConn.DBConnStr, sqlCmd);

                cbo_WorkOrder.DisplayMember = "WorkOrder";
                cbo_WorkOrder.DataSource = dt.Tables[0];
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_GetSubOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbo_WorkOrder.Text == "")
                {
                    MessageBox.Show("请选择工单号");
                    return;
                }
                GetTableName();
                using (var conn = new MySqlConnection(this._plugin.SqlConn.DBConnStr))
                {
                    conn.Open();
                    string sqlCmd = string.Format("Select distinct SubOrder from {0} where WorkOrder = '{1}'", TableName, cbo_WorkOrder.Text);


                    MySqlDataAdapter mySql = new MySqlDataAdapter(sqlCmd, conn);
                    var dataSet = new DataSet();

                    mySql.Fill(dataSet);
                    cbo_SubOrder.DisplayMember = "SubOrder";
                    cbo_SubOrder.DataSource = dataSet.Tables[0];



                }

            }
            catch (Exception ex)
            {

            }

        }

        private void btn_GetPN_Click(object sender, EventArgs e)
        {
            try
            {
                GetTableName();

                string sqlCmd;
                if (cbo_WorkOrder.Text != "")
                {
                    sqlCmd = string.Format("Select distinct PartNumber from {0} where WorkOrder = '{1}'", TableName, cbo_WorkOrder.Text);
                }
                else
                {
                    sqlCmd = string.Format("Select distinct PartNumber from {0}", TableName);
                }
                using (var conn = new MySqlConnection(this._plugin.SqlConn.DBConnStr))
                {
                    conn.Open();
                    MySqlDataAdapter mySql = new MySqlDataAdapter(sqlCmd, conn);
                    var dataSet = new DataSet();

                    mySql.Fill(dataSet);
                    cbo_PartNumber.DisplayMember = "PartNumber";
                    cbo_PartNumber.DataSource = dataSet.Tables[0];
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void btn_GetCarrierID_Click(object sender, EventArgs e)
        {
            try
            {
                GetTableName();

                string sqlCmd;
                if (cbo_WorkOrder.Text == "")
                {

                    MessageBox.Show("请选择工单号");
                    return;

                }
                else if (cbo_SubOrder.Text == "")
                {
                    sqlCmd = string.Format("Select distinct CarrierID from {0} where WorkOrder = '{1}'", TableName, cbo_WorkOrder.Text);

                }
                else
                {
                    sqlCmd = string.Format("Select distinct CarrierID from {0} where WorkOrder = '{1}' and SubOrder = '{2}'", TableName, cbo_WorkOrder.Text, cbo_SubOrder.Text);
                }
                using (var conn = new MySqlConnection(this._plugin.SqlConn.DBConnStr))
                {
                    conn.Open();
                    MySqlDataAdapter mySql = new MySqlDataAdapter(sqlCmd, conn);
                    var dataSet = new DataSet();

                    mySql.Fill(dataSet);
                    cbo_CarrierID.DisplayMember = "CarrierID";
                    cbo_CarrierID.DataSource = dataSet.Tables[0];
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void btn_GetChipID_Click(object sender, EventArgs e)
        {
            try
            {
                string condition = "";
                if (cbo_WorkOrder.Text != "")
                {
                    condition += string.Format("WorkOrder = '{0}' ", cbo_WorkOrder.Text);
                }
                if (cbo_SubOrder.Text != "")
                {
                    condition = condition == "" ? string.Format("SubOrder = '{0}' ", cbo_SubOrder.Text) : condition + string.Format("and SubOrder = '{0}' ", cbo_SubOrder.Text);
                }
                if (cbo_PartNumber.Text != "")
                {
                    condition = condition == "" ? string.Format("PartNumber = '{0}' ", cbo_PartNumber.Text) : condition + string.Format("and PartNumber = '{0}' ", cbo_PartNumber.Text);
                }
                if (cbo_CarrierID.Text != "")
                {
                    condition = condition == "" ? string.Format("CarrierID = '{0}' ", cbo_CarrierID.Text) : condition + string.Format("and CarrierID = '{0}' ", cbo_CarrierID.Text);
                }
                if (condition != "")
                {
                    condition = "where " + condition;
                }
                GetTableName();
                string sqlCmd = string.Format("Select distinct ChipID from {0} {1}", TableName, condition);
                using (var conn = new MySqlConnection(this._plugin.SqlConn.DBConnStr))
                {
                    conn.Open();
                    MySqlDataAdapter mySql = new MySqlDataAdapter(sqlCmd, conn);
                    var dataSet = new DataSet();

                    mySql.Fill(dataSet);
                    cbo_ChipID.DisplayMember = "ChipID";
                    cbo_ChipID.DataSource = dataSet.Tables[0];
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_GetTestStations_Click(object sender, EventArgs e)
        {
            try
            {
                GetTableName();
                string sqlCmd = string.Format("Select distinct Station from {0}", TableName);
                using (var conn = new MySqlConnection(this._plugin.SqlConn.DBConnStr))
                {
                    conn.Open();
                    MySqlDataAdapter mySql = new MySqlDataAdapter(sqlCmd, conn);
                    var dataSet = new DataSet();

                    mySql.Fill(dataSet);
                    cbo_TestStation.DisplayMember = "Station";
                    cbo_TestStation.DataSource = dataSet.Tables[0];
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_QueryData_Click(object sender, EventArgs e)
        {
            try
            {

                //string tableName = SqlConnStr.DBTableName;
                //SqlHelper.c = SqlConnStr.DBConnStr;
                GetTableName();
                string condition = "";
                if (cbo_WorkOrder.Text != "")
                {
                    condition += string.Format("WorkOrder = '{0}' ", cbo_WorkOrder.Text);
                }
                if (cbo_SubOrder.Text != "")
                {
                    condition = condition == "" ? string.Format("SubOrder = '{0}' ", cbo_SubOrder.Text) : condition + string.Format("and SubOrder = '{0}' ", cbo_SubOrder.Text);
                }
                if (cbo_PartNumber.Text != "")
                {
                    condition = condition == "" ? string.Format("PartNumber = '{0}' ", cbo_PartNumber.Text) : condition + string.Format("and PartNumber = '{0}' ", cbo_PartNumber.Text);
                }
                if (cbo_CarrierID.Text != "")
                {
                    condition = condition == "" ? string.Format("CarrierID = '{0}' ", cbo_CarrierID.Text) : condition + string.Format("and CarrierID = '{0}' ", cbo_CarrierID.Text);
                }
                if (cbo_ChipID.Text != "")
                {
                    condition = condition == "" ? string.Format("ChipID = '{0}' ", cbo_ChipID.Text) : condition + string.Format("and ChipID = '{0}' ", cbo_ChipID.Text);
                }
                string purpose = "";
                if (this.rb_PostBI.Checked)
                {
                    purpose = string.Format("Purpose = '{0}' ", PostBIColumn);
                }
                else
                {
                    purpose = string.Format("Purpose = '{0}' ", TableName);
                }
                condition = condition == "" ? "(" + (purpose == "" ? "1=1" : purpose) + ")" : condition + "and " + "(" + (purpose == "" ? "1=1" : purpose) + ")";
                //if (!cb_PreBI.Checked || !cb_PostBI.Checked ||
                //    !cb_PostBI2.Checked || !cb_GS.Checked)
                //{

                //    if (cb_PreBI.Checked)
                //    {
                //        purpose = purpose == "" ? string.Format("Purpose = '{0}' ", "PreBI") : purpose + string.Format("or Purpose = '{0}' ", "PreBI");
                //    }
                //    if (cb_PostBI.Checked)
                //    {
                //        purpose = purpose == "" ? string.Format("Purpose = '{0}' ", "PostBI") : purpose + string.Format("or Purpose = '{0}' ", "PostBI");
                //    }
                //    if (cb_PostBI2.Checked)
                //    {
                //        purpose = purpose == "" ? string.Format("Purpose = '{0}' ", "PostBI2") : purpose + string.Format("or Purpose = '{0}' ", "PostBI2");
                //    }
                //    if (cb_GS.Checked)
                //    {
                //        purpose = purpose == "" ? string.Format("Purpose = '{0}' ", "GoldenSample") : purpose + string.Format("or Purpose = '{0}' ", "GoldenSample");
                //    }
                //    condition = condition == "" ? "(" + (purpose == "" ? "1=1" : purpose) + ")" : condition + "and " + "(" + (purpose == "" ? "1=1" : purpose) + ")";
                //}
                if (!cb_Pass.Checked || !cb_Fail.Checked)
                {
                    if (cb_Pass.Checked)
                    {
                        condition = condition == "" ? string.Format("Result = '{0}' ", "PASS") : condition + string.Format("and Result = '{0}' ", "PASS");
                    }
                    if (cb_Fail.Checked)
                    {
                        condition = condition == "" ? string.Format("Result = '{0}' ", "FAIL") : condition + string.Format("and Result = '{0}' ", "FAIL");
                    }
                }
                if (dtpFrom.Checked)
                {
                    condition = condition == "" ? string.Format("StartTime > '{0}' ", dtpFrom.Text) : condition + string.Format("and StartTime > '{0}' ", dtpFrom.Text);
                }
                if (dtpTo.Checked)
                {
                    condition = condition == "" ? string.Format("EndTime < '{0}' ", dtpTo.Text) : condition + string.Format("and EndTime < '{0}' ", dtpTo.Text);
                }
                if (cbo_TestStation.Text != "")
                {
                    condition = condition == "" ? string.Format("Station = '{0}' ", cbo_TestStation.Text) : condition + string.Format("and Station = '{0}' ", cbo_TestStation.Text);
                }
                if (condition != "")
                {
                    condition = "where " + condition;
                }
                string sqlCmd = string.Format("Select TOP 1000 * from {0} {1} ORDER BY EndTime DESC", TableName, condition);
                using (var conn = new MySqlConnection(this._plugin.SqlConn.DBConnStr))
                {
                    conn.Open();
                    MySqlDataAdapter mySql = new MySqlDataAdapter(sqlCmd, conn);
                    var dataSet = new DataSet();

                    mySql.Fill(dataSet);
                    dgv_TestData.DataSource = dataSet.Tables[0];
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_ExportData_Click(object sender, EventArgs e)
        {
            try
            {
                ExportToCSV(dgv_TestData, "");
            }
            catch (Exception ex)
            {

            }
        }
        public static void ExportToCSV(DataGridView dgv, string fileName)
        {
            if (dgv.Rows.Count < 1)
            {
                MessageBox.Show("没有记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog sfDialog = new SaveFileDialog();
            sfDialog.Filter = "CSV文件(*.csv)|*.csv|文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            sfDialog.FilterIndex = 0;
            sfDialog.FileName = fileName;
            if (sfDialog.ShowDialog() == DialogResult.OK)
            {
                string strFileName = sfDialog.FileName;
                StreamWriter sw = new StreamWriter(strFileName, false, Encoding.UTF8);
                string strLine = "";
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (col.Visible)
                    {
                        strLine += col.HeaderText.Trim() + ",";
                    }
                }
                strLine = strLine.Substring(0, strLine.Length - 1);
                sw.WriteLine(strLine);
                sw.Flush();

                foreach (DataGridViewRow dgvr in dgv.Rows)
                {
                    strLine = "";
                    foreach (DataGridViewCell dgvc in dgvr.Cells)
                    {
                        if (dgvc.Visible)
                        {
                            if (dgvc.Value == null)
                            {
                                strLine += ",";
                            }
                            else
                            {
                                strLine += dgvc.Value.ToString() + ",";
                            }
                        }
                    }
                    sw.WriteLine(strLine);
                    sw.Flush();
                }
                sw.Close();
                MessageBox.Show(string.Format("数据已成功导出至/n{0}/n文件中!", strFileName), "导出成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_SelectData_Click(object sender, EventArgs e)
        {
            try
            {
                var path = Path.GetFullPath(Application.StartupPath);
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = path;
                var ret = ofd.ShowDialog();
                if (ret == System.Windows.Forms.DialogResult.OK)
                {
                    tb_DataFile.Text = ofd.FileName;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_UploadData_Click(object sender, EventArgs e)
        {

        }

        private void rb_PostBI_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rb_PostBI.Checked)
            {
                this.cb_PostBIColumn.Enabled = true;
                this.cb_PostBIColumn.Items.Clear();
                this.cb_PostBIColumn.Items.AddRange(this._plugin.SqlConn.Post_ColumnNameList.ToArray());
            }
            else
            {
                this.cb_PostBIColumn.Enabled = false;
            }
        }
        private void IsConn()
        {
            if (!this._plugin.SqlConn.EnableSql)
            {
                return;
            }
            try
            {
                using (var conn = new MySqlConnection(this._plugin.SqlConn.DBConnStr))
                {
                    conn.Open();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        this._plugin.Log_Global("连接到 MySQL 服务器成功！");
                    }
                    else
                    {
                        MessageBox.Show("无法连接到 MySQL 服务器。");
                        this._plugin.Log_Global("无法连接到 MySQL 服务器。");
                        return;
                    }

                    string sql1 = $"create table if not exists {this._plugin.SqlConn.DBTableName_GS} (WorkOrder varchar(255) default null)";
                    MySqlCommand mySql = new MySqlCommand(sql1, conn);
                    mySql.ExecuteNonQuery();
                    string sql2 = $"create table if not exists {this._plugin.SqlConn.DBTableName_Pre} (WorkOrder varchar(255) default null)";
                    mySql = new MySqlCommand(sql2, conn);
                    mySql.ExecuteNonQuery();
                    string sql3 = $"create table if not exists {this._plugin.SqlConn.DBTableName_Post} (WorkOrder varchar(255) default null)";
                    mySql = new MySqlCommand(sql3, conn);
                    mySql.ExecuteNonQuery();

                    mySql.Dispose();
                }
            }
            catch (Exception ex)
            {
                this._plugin.Log_Global($"发生错误：{ex.Message}");
            }
        }

        #endregion

        private void timer_Engineer_Tick(object sender, EventArgs e)
        {
            if (this._core.CanUserAccessDomain(AccessPermissionLevel.Admin))
            {
                tabPage测试.Parent = tb_MainPage;
                tabPage数据库.Parent = tb_MainPage;
                tabPage调试.Parent = tb_MainPage;
                tabPage仪表.Parent = tb_MainPage;
                tabPage视频.Parent = tb_MainPage;

            }
            else if (this._core.CanUserAccessDomain(AccessPermissionLevel.Engineer))
            {
                tabPage测试.Parent = tb_MainPage;
                tabPage数据库.Parent = null;
                tabPage调试.Parent = tb_MainPage;
                tabPage仪表.Parent = tb_MainPage;
                tabPage视频.Parent = tb_MainPage;
            }
            else if (this._core.CanUserAccessDomain(AccessPermissionLevel.Operator))
            {
                tabPage测试.Parent = tb_MainPage;
                tabPage数据库.Parent = null;
                tabPage调试.Parent = null;
                tabPage仪表.Parent = null;
                tabPage视频.Parent = null;
            }
            else if (this._core.CanUserAccessDomain(AccessPermissionLevel.None))
            {
                tabPage测试.Parent = null;
                tabPage数据库.Parent = null;
                tabPage调试.Parent = null;
                tabPage仪表.Parent = null;
                tabPage视频.Parent = null;
            }
        }

        #region 仪表
        private void bt_GAIN_ON_Click(object sender, EventArgs e)
        {
            try
            {
                double current = 0;
                double voltag = 0;
                if (double.TryParse(this.tb_GAIN_V.Text, out voltag) == false)
                {
                    MessageBox.Show("目标电压格式错误！");
                    return;
                }
                if (double.TryParse(this.tb_GAIN_I.Text, out current) == false)
                {
                    MessageBox.Show("目标电流格式错误！");
                    return;
                }
                //this._plugin.LocalResource.GAIN.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(current, voltag);
                this._plugin.LocalResource.GAIN.SetupAndEnableSourceOutput_SinglePoint_Current_mA(current, voltag);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_GAIN_OFF_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.GAIN.IsOutputOn = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_SOA1_ON_Click(object sender, EventArgs e)
        {
            try
            {
                double current = 0;
                double voltag = 0;
                if (double.TryParse(this.tb_SOA1_V.Text, out voltag) == false)
                {
                    MessageBox.Show("目标电压格式错误！");
                    return;
                }
                if (double.TryParse(this.tb_SOA1_I.Text, out current) == false)
                {
                    MessageBox.Show("目标电流格式错误！");
                    return;
                }
                this._plugin.LocalResource.SOA1.SetupAndEnableSourceOutput_SinglePoint_Current_mA(current, voltag);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_SOA1_OFF_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.SOA1.IsOutputOn = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_SOA2_ON_Click(object sender, EventArgs e)
        {
            try
            {
                double current = 0;
                double voltag = 0;
                if (double.TryParse(this.tb_SOA2_V.Text, out voltag) == false)
                {
                    MessageBox.Show("目标电压格式错误！");
                    return;
                }
                if (double.TryParse(this.tb_SOA2_I.Text, out current) == false)
                {
                    MessageBox.Show("目标电流格式错误！");
                    return;
                }
                this._plugin.LocalResource.SOA2.SetupAndEnableSourceOutput_SinglePoint_Current_mA(current, voltag);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_SOA2_OFF_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.SOA2.IsOutputOn = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_LP_ON_Click(object sender, EventArgs e)
        {
            try
            {
                double current = 0;
                double voltag = 0;
                if (double.TryParse(this.tb_LP_V.Text, out voltag) == false)
                {
                    MessageBox.Show("目标电压格式错误！");
                    return;
                }
                if (double.TryParse(this.tb_LP_I.Text, out current) == false)
                {
                    MessageBox.Show("目标电流格式错误！");
                    return;
                }
                this._plugin.LocalResource.LP.SetupAndEnableSourceOutput_SinglePoint_Current_mA(current, voltag);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_LP_OFF_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.LP.IsOutputOn = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_PH1_ON_Click(object sender, EventArgs e)
        {
            try
            {
                double current = 0;
                double voltag = 0;
                if (double.TryParse(this.tb_PH1_V.Text, out voltag) == false)
                {
                    MessageBox.Show("目标电压格式错误！");
                    return;
                }
                if (double.TryParse(this.tb_PH1_I.Text, out current) == false)
                {
                    MessageBox.Show("目标电流格式错误！");
                    return;
                }
                this._plugin.LocalResource.PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA(current, voltag);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_PH1_OFF_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.PH1.IsOutputOn = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_PH2_ON_Click(object sender, EventArgs e)
        {
            try
            {
                double current = 0;
                double voltag = 0;
                if (double.TryParse(this.tb_PH2_V.Text, out voltag) == false)
                {
                    MessageBox.Show("目标电压格式错误！");
                    return;
                }
                if (double.TryParse(this.tb_PH2_I.Text, out current) == false)
                {
                    MessageBox.Show("目标电流格式错误！");
                    return;
                }
                this._plugin.LocalResource.PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA(current, voltag);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_PH2_OFF_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.PH2.IsOutputOn = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_MIRROR1_ON_Click(object sender, EventArgs e)
        {
            try
            {
                double current = 0;
                double voltag = 0;
                if (double.TryParse(this.tb_MIRROR1_V.Text, out voltag) == false)
                {
                    MessageBox.Show("目标电压格式错误！");
                    return;
                }
                if (double.TryParse(this.tb_MIRROR1_I.Text, out current) == false)
                {
                    MessageBox.Show("目标电流格式错误！");
                    return;
                }
                this._plugin.LocalResource.MIRROR1.SetupAndEnableSourceOutput_SinglePoint_Current_mA(current, voltag);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_MIRROR1_OFF_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.MIRROR1.IsOutputOn = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_MIRROR2_ON_Click(object sender, EventArgs e)
        {
            try
            {
                double current = 0;
                double voltag = 0;
                if (double.TryParse(this.tb_MIRROR2_V.Text, out voltag) == false)
                {
                    MessageBox.Show("目标电压格式错误！");
                    return;
                }
                if (double.TryParse(this.tb_MIRROR2_I.Text, out current) == false)
                {
                    MessageBox.Show("目标电流格式错误！");
                    return;
                }
                this._plugin.LocalResource.MIRROR2.SetupAndEnableSourceOutput_SinglePoint_Current_mA(current, voltag);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_MIRROR2_OFF_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.MIRROR2.IsOutputOn = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_Bias1_ON_Click(object sender, EventArgs e)
        {
            try
            {
                double current = 0;
                double voltag = 0;
                if (double.TryParse(this.tb_Bias1_V.Text, out voltag) == false)
                {
                    MessageBox.Show("目标电压格式错误！");
                    return;
                }
                if (double.TryParse(this.tb_Bias1_I.Text, out current) == false)
                {
                    MessageBox.Show("目标电流格式错误！");
                    return;
                }
                this._plugin.LocalResource.BIAS1.SetupAndEnableSourceOutput_SinglePoint_Voltage_V(voltag, current);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_Bias1_OFF_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.BIAS1.IsOutputOn = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_Bias2_ON_Click(object sender, EventArgs e)
        {
            try
            {
                double current = 0;
                double voltag = 0;
                if (double.TryParse(this.tb_Bias2_V.Text, out voltag) == false)
                {
                    MessageBox.Show("目标电压格式错误！");
                    return;
                }
                if (double.TryParse(this.tb_Bias2_I.Text, out current) == false)
                {
                    MessageBox.Show("目标电流格式错误！");
                    return;
                }
                this._plugin.LocalResource.BIAS2.SetupAndEnableSourceOutput_SinglePoint_Voltage_V(voltag, current);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_Bias2_OFF_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.BIAS2.IsOutputOn = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_MPD1_ON_Click(object sender, EventArgs e)
        {
            try
            {
                double current = 0;
                double voltag = 0;
                if (double.TryParse(this.tb_MPD1_V.Text, out voltag) == false)
                {
                    MessageBox.Show("目标电压格式错误！");
                    return;
                }
                if (double.TryParse(this.tb_MPD1_I.Text, out current) == false)
                {
                    MessageBox.Show("目标电流格式错误！");
                    return;
                }
                this._plugin.LocalResource.MPD1.SetupAndEnableSourceOutput_SinglePoint_Voltage_V(voltag, current);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_MPD1_OFF_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.MPD1.IsOutputOn = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_MPD2_ON_Click(object sender, EventArgs e)
        {
            try
            {
                double current = 0;
                double voltag = 0;
                if (double.TryParse(this.tb_MPD2_V.Text, out voltag) == false)
                {
                    MessageBox.Show("目标电压格式错误！");
                    return;
                }
                if (double.TryParse(this.tb_MPD2_I.Text, out current) == false)
                {
                    MessageBox.Show("目标电流格式错误！");
                    return;
                }
                this._plugin.LocalResource.MPD2.SetupAndEnableSourceOutput_SinglePoint_Voltage_V(voltag, current);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_MPD2_OFF_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.MPD2.IsOutputOn = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }


        private void bt_GetGAIN_Click(object sender, EventArgs e)
        {
            try
            {
                var voltage = this._plugin.LocalResource.GAIN.ReadVoltage_V();
                this.lab_GetGAIN_V.Text = voltage.ToString();

                var current = Math.Round(this._plugin.LocalResource.GAIN.ReadCurrent_A() * 1000.0, 4);
                this.lab_GetGAIN_I.Text = current.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_GetSOA1_Click(object sender, EventArgs e)
        {
            try
            {
                var voltage = this._plugin.LocalResource.SOA1.ReadVoltage_V();
                this.lab_GetSOA1_V.Text = voltage.ToString();


                var current = Math.Round(this._plugin.LocalResource.SOA1.ReadCurrent_A() * 1000.0, 4);// this._plugin.LocalResource.SOA1.ReadCurrent_A() * 1000.0;
                this.lab_GetSOA1_I.Text = current.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_GetSOA2_Click(object sender, EventArgs e)
        {
            try
            {
                var voltage = this._plugin.LocalResource.SOA2.ReadVoltage_V();
                this.lab_GetSOA2_V.Text = voltage.ToString();


                var current = Math.Round(this._plugin.LocalResource.SOA2.ReadCurrent_A() * 1000.0, 4);
                this.lab_GetSOA2_I.Text = current.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_GetLP_Click(object sender, EventArgs e)
        {
            try
            {
                var voltage = this._plugin.LocalResource.LP.ReadVoltage_V();
                this.lab_GetLP_V.Text = voltage.ToString();

                var current = Math.Round(this._plugin.LocalResource.LP.ReadCurrent_A() * 1000.0, 4);
                this.lab_GetLP_I.Text = current.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_GetPH1_Click(object sender, EventArgs e)
        {
            try
            {
                var voltage = this._plugin.LocalResource.PH1.ReadVoltage_V();
                this.lab_GetPH1_V.Text = voltage.ToString();



                var current = Math.Round(this._plugin.LocalResource.PH1.ReadCurrent_A() * 1000.0, 4);
                this.lab_GetPH1_I.Text = current.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_GetPH2_Click(object sender, EventArgs e)
        {
            try
            {
                var voltage = this._plugin.LocalResource.PH2.ReadVoltage_V();
                this.lab_GetPH2_V.Text = voltage.ToString();

                var current = Math.Round(this._plugin.LocalResource.PH2.ReadCurrent_A() * 1000.0, 4);
                this.lab_GetPH2_I.Text = current.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_GetMIRROR1_Click(object sender, EventArgs e)
        {
            try
            {
                var voltage = this._plugin.LocalResource.MIRROR1.ReadVoltage_V();
                this.lab_GetMIRROR1_V.Text = voltage.ToString();

                var current = Math.Round(this._plugin.LocalResource.MIRROR1.ReadCurrent_A() * 1000.0, 4);
                this.lab_GetMIRROR1_I.Text = current.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_GetMIRROR2_Click(object sender, EventArgs e)
        {
            try
            {
                var voltage = this._plugin.LocalResource.MIRROR2.ReadVoltage_V();
                this.lab_GetMIRROR2_V.Text = voltage.ToString();


                var current = Math.Round(this._plugin.LocalResource.MIRROR2.ReadCurrent_A() * 1000.0, 4);
                this.lab_GetMIRROR2_I.Text = current.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_GetBIAS1_Click(object sender, EventArgs e)
        {
            try
            {
                var current = Math.Round(this._plugin.LocalResource.BIAS1.ReadCurrent_A() * 1000.0, 4);// this._plugin.LocalResource.BIAS1.ReadCurrent_A() * 1000;
                this.lab_GetBIAS1_I.Text = current.ToString();

                var voltage = this._plugin.LocalResource.BIAS1.ReadVoltage_V();
                this.lab_GetBIAS1_V.Text = voltage.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_GetBIAS2_Click(object sender, EventArgs e)
        {
            try
            {
                var current = Math.Round(this._plugin.LocalResource.BIAS2.ReadCurrent_A() * 1000.0, 4);// this._plugin.LocalResource.BIAS2.ReadCurrent_A() * 1000;
                this.lab_GetBIAS2_I.Text = current.ToString();

                var voltage = this._plugin.LocalResource.BIAS2.ReadVoltage_V();
                this.lab_GetBIAS2_V.Text = voltage.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_GetMPD1_Click(object sender, EventArgs e)
        {
            try
            {
                var current = Math.Round(this._plugin.LocalResource.MPD1.ReadCurrent_A() * 1000.0, 4);//this._plugin.LocalResource.MPD1.ReadCurrent_A() * 1000;
                this.lab_GetMPD1_I.Text = current.ToString();

                var voltage = this._plugin.LocalResource.MPD1.ReadVoltage_V();
                this.lab_GetMPD1_V.Text = voltage.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_GetMPD2_Click(object sender, EventArgs e)
        {
            try
            {
                var current = Math.Round(this._plugin.LocalResource.MPD2.ReadCurrent_A() * 1000.0, 4);//this._plugin.LocalResource.MPD2.ReadCurrent_A() * 1000;
                this.lab_GetMPD2_I.Text = current.ToString();

                var voltage = this._plugin.LocalResource.MPD2.ReadVoltage_V();
                this.lab_GetMPD2_V.Text = voltage.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }
        private void bt_GetPD_Click(object sender, EventArgs e)
        {
            try
            {
                double wave = 0;
                if (double.TryParse(this.tb_wave.Text, out wave) == false)
                {
                    MessageBox.Show("目标电流格式错误！");
                    return;
                }
                var current = Math.Round(this._plugin.LocalResource.PD.ReadCurrent_A() * 1000.0, 4);//this._plugin.LocalResource.PD.ReadCurrent_A() * 1000;

                string configFileFullPath = System.IO.Path.Combine(Application.StartupPath, "ConfigFiles", "CalibrationFactor.xml");
                PDCalibrationData compensation = XmlHelper.DeserializeFile<PDCalibrationData>(configFileFullPath);
                var k = compensation.PD_K[wave];
                this.lab_GetPD_I.Text = current.ToString();


                //var voltage = this._plugin.LocalResource.PD.ReadVoltage_V();
                this.lab_GetPD_V.Text = (k * current).ToString("f3");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }
        public class PDCalibrationData
        {
            public string Description { get; set; }
            public double PD_B { get; set; }
            public DataBook<double, double> PD_K { get; set; }
        }
        private void bt_PD_ON_Click(object sender, EventArgs e)
        {
            try
            {
                double current = 0;
                if (double.TryParse(this.tb_PD_I.Text, out current) == false)
                {
                    MessageBox.Show("目标电流格式错误！");
                    return;
                }
                this._plugin.LocalResource.PD.SetupAndEnableSourceOutput_SinglePoint_Voltage_V(0, current);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_PD_OFF_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.PD.IsOutputOn = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }
        private void bt_OFF_All_Click(object sender, EventArgs e)
        {
            try
            {
                Merged_PXIe_4143.DisableAllSectionOutput();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_ted_start_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rb_left.Checked)
                {
                    this._plugin.LocalResource.IOs[IONameEnum_CT3103.TEC_Right].TurnOn(false);
                    this._plugin.LocalResource.IOs[IONameEnum_CT3103.TEC_Left].TurnOn(true);
                }
                else if (this.rb_right.Checked)
                {
                    this._plugin.LocalResource.IOs[IONameEnum_CT3103.TEC_Left].TurnOn(false);
                    this._plugin.LocalResource.IOs[IONameEnum_CT3103.TEC_Right].TurnOn(true);
                }
                float start = 0;
                if (!float.TryParse(this.tb_temp.Text, out start))
                {
                    return;
                }

                this._plugin.LocalResource.tED4015.SetTemperatureSetpoint(start);
                this._plugin.LocalResource.tED4015.IsOutPut(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_ted_stop_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.tED4015.IsOutPut(false);
                if (this.rb_left.Checked)
                {

                    this._plugin.LocalResource.IOs[IONameEnum_CT3103.TEC_Left].TurnOn(false);
                }
                else if (this.rb_right.Checked)
                {
                    this._plugin.LocalResource.IOs[IONameEnum_CT3103.TEC_Right].TurnOn(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_gettemp_Click(object sender, EventArgs e)
        {
            try
            {
                this.lab_ted.Text = this._plugin.LocalResource.tED4015.GetTempTemperature().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        #endregion

        private void bt_LIV_Click(object sender, EventArgs e)
        {
            float start = 0;
            if (!float.TryParse(this.tb_liv_start.Text, out start))
            {
                return;
            }
            float step = 0;
            if (!float.TryParse(this.tb_liv_step.Text, out step))
            {
                return;
            }
            float end = 0;
            if (!float.TryParse(this.tb_liv_end.Text, out end))
            {
                return;
            }
            float complianceVoltage_V = 0;
            if (!float.TryParse(this.tb_liv_compV.Text, out complianceVoltage_V))
            {
                return;
            }
            float pdBiasVoltage_V = 0;
            if (!float.TryParse(this.tb_liv_pdbiasV.Text, out pdBiasVoltage_V))
            {
                return;
            }
            float pdComplianceCurrent_mA = 0;
            if (!float.TryParse(this.tb_liv_pdcompCurr.Text, out pdComplianceCurrent_mA))
            {
                return;
            }
            Merged_PXIe_4143.Reset();
            if (this.comboBox1.Text == "GAIN")
            {
                //Merged_PXIe_4143.Sweep_LD_PD(start, step, end, complianceVoltage_V, pdBiasVoltage_V, pdComplianceCurrent_mA, 0);
                //double[] dfbCurrents_A = ArrayMath.CalculateArray(start, end, step);
                //var GAINresut = this._plugin.LocalResource.GAIN.Fetch_MeasureVals(dfbCurrents_A.Length, 10 * 1000.0);
                //var GAIN_Current = GAINresut.CurrentMeasurements;
                //var GAIN_Voltage = GAINresut.VoltageMeasurements;
                //var PD_Current = this._plugin.LocalResource.PD.Fetch_MeasureVals(dfbCurrents_A.Length, 10 * 1000.0).CurrentMeasurements;
                //Print(this.comboBox1.Text,GAIN_Current, GAIN_Voltage, PD_Current);
            }
            else if (this.comboBox1.Text == "SOA1")
            {
                //double sourceDelay_s = 0.01;
                //double apertureTime_s = 0.005;
                //double[] dfbCurrents_A = ArrayMath.CalculateArray(start, end, step);
                //this._plugin.LocalResource.SOA1.SetupMaster_Sequence_SourceCurrent_SenseVoltage(start, step, end, complianceVoltage_V, sourceDelay_s, apertureTime_s, true);
                //this._plugin.LocalResource.PD.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(pdBiasVoltage_V, pdComplianceCurrent_mA, dfbCurrents_A.Length, sourceDelay_s, apertureTime_s, true);

                //this._plugin.LocalResource.SOA1.TriggerOutputOn = true;
                //this._plugin.LocalResource.PD.TriggerOutputOn = true;

                //var slavers = new PXISourceMeter_4143[] { this._plugin.LocalResource.PD };
                //Merged_PXIe_4143.ConfigureMultiChannelSynchronization(this._plugin.LocalResource.SOA1, slavers);
                //Merged_PXIe_4143.Trigger(this._plugin.LocalResource.SOA1, slavers);
                //var SOA1resut = this._plugin.LocalResource.SOA1.Fetch_MeasureVals(dfbCurrents_A.Length, 10 * 1000.0);
                //var SOA1_Current = SOA1resut.CurrentMeasurements;//Merged_PXIe_4143.FetchLDSweepData(this._plugin.LocalResource.SOA1, SweepData.Sense_Current_mA, dfbCurrents_A.Length);
                //var SOA1_Voltage = SOA1resut.VoltageMeasurements;//Merged_PXIe_4143.FetchLDSweepData(this._plugin.LocalResource.SOA2, SweepData.Sense_Voltage_V, dfbCurrents_A.Length);
                //var PD_Current = this._plugin.LocalResource.PD.Fetch_MeasureVals(dfbCurrents_A.Length, 10 * 1000.0).CurrentMeasurements;
                //Print(this.comboBox1.Text, SOA1_Current, SOA1_Voltage, PD_Current);
            }



        }
        private void Print(string Section, double[] Current, double[] Voltage, double[] PD_Current)
        {
            #region 存表格
            try
            {
                string dir = @"C:\Users\Administrator\Desktop\";

                var defaultFileName = string.Concat("LIV_", BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now), FileExtension.CSV);

                var finalFileName = $@"{dir}\{defaultFileName}";

                string name = "Current,Voltage,PD_Current";
                using (StreamWriter sw = new StreamWriter(finalFileName, false, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(name);
                    for (int i = 0; i < PD_Current.Length; i++)
                    {
                        sw.WriteLine($"{Current[i]},{Voltage[i]},{PD_Current[i]}");
                    }
                }
                this._core.Log_Global("打印完成！");
            }
            catch (Exception ex)
            {
                this._core.Log_Global($"打印失败！ [{ex.Message}]-[{ex.StackTrace}]");
            }


            #endregion
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                string dir = @"D:\CT-3103\LaserX_TesterLibrary\Data\4_6_DO721_2_TM038_E-02-09_20240520_141822\DO721#2\TM038\4_6_DO721_2_TM038_E-02-09_20240520_141822\(DO721_2_TM038_E-02-09)\CoarseTuning";

                Directory.CreateDirectory(dir);

                dir = @"D:\CT-3103\LaserX_TesterLibrary\Data\4_6_DO721_2_TM038_E-02-09_20240520_141822\DO721#2\TM038\4_6_DO721_2_TM038_E-02-09_20240520_141822\(DO721_2_TM038_E-02-09)\CoarseTuning\DO721#2_TM038_4_6_DO721_2_TM038_E-02-09_20240520_141822_CoarseTuning#Deviations@55.00C_2024-05-20_14-25-56.csv";
                using (StreamWriter sw = new StreamWriter(dir, false, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine();
                    sw.WriteLine();
                }



                //int count;
                //List<string> CarrierNumberlist = new List<string>();
                //var ispass = this._plugin.GetCarrierNumber(out CarrierNumberlist,out count);
                //var chipnumberlist = this._plugin.GetCarrierChipNumber(CarrierNumberlist[0]);


                //string CoarseTuningPath = @"D:\CT-3103\LaserX_TesterLibrary\Data\Coarse_tuning\DO123\TM346\T7891\(SW_EXAMPLE)\CoarseTuning\DO123_TM346_T7891_CoarseTuning#Deviations@55.00C_2024-04-11_14-50-22.csv";
                //Form_CoarseTuning form_Coarse = new Form_CoarseTuning(CoarseTuningPath);
                //form_Coarse.Show();


                //this._plugin.LocalResource.fWM.RST();
                //this._plugin.LocalResource.fWM.SetAutomaticExposure(Auto.On);
                //this._plugin.LocalResource.fWM.SetSource(Source.EXTernal);
                //var datas = _plugin.LocalResource.fWM.EXTernalStart();
                //_plugin.LocalResource.fWM.EXTernalStop();





                //this._plugin.LocalResource.fWM.SetTriggerSource(Source.INTernal);
                //DataBook<double, double> datas_1 = new DataBook<double, double>();
                //Action FWM_1 = new Action(() =>
                //{
                //    datas_1 = _plugin.LocalResource.fWM.INTernalStart();
                //});
                //var datas_ = _plugin.LocalResource.fWM.INTernalStart();
                //_plugin.LocalResource.fWM.INTernalStop();


                //var a = this._plugin.LocalResource.fWM.GetBand();

                //var b = this._plugin.LocalResource.fWM.GetPowUnit();
                //var c = this._plugin.LocalResource.fWM.GetPower();
                //var d = this._plugin.LocalResource.fWM.GetWaveUnit();
                //var f = this._plugin.LocalResource.fWM.GetWavelenth();

                //var qq = this._plugin.LocalResource.oSA.Sensitivity;
                //var ww = this._plugin.LocalResource.oSA.ReadWavelengthAtPeak_nm();
                //var pp = this._plugin.LocalResource.oSA.ReadPowerAtPeak_dbm();


                this._plugin.LocalResource.oSA.Reset();
                //var q = this._plugin.LocalResource.oSA.InstrumentIDN;
                //var d = this._plugin.LocalResource.oSA.PeakExcursion_dB;
                //this._plugin.LocalResource.oSA.PowerUnit = PowerUnit.dBm;
                //this._plugin.LocalResource.oSA.ResolutionBandwidth_nm = 0.07;
                this._plugin.LocalResource.oSA.CenterWavelength_nm = 1550;
                this._plugin.LocalResource.oSA.WavelengthSpan_nm = 100;
                this._plugin.LocalResource.oSA.TraceLength = 1001;
                var waveandpower = this._plugin.LocalResource.oSA.GetOpticalSpectrumTrace(true);
                List<double> wave = new List<double>();
                List<double> power = new List<double>();
                for (int i = 0; i < waveandpower.Count; i++)
                {
                    wave.Add(waveandpower[i].Wavelength_nm);
                }
                for (int i = 0; i < waveandpower.Count; i++)
                {
                    power.Add(waveandpower[i].Power_dBm);
                }

                var Power_dBM = power.Max();

                int max_index = 0;
                int min_index = 0;
                ArrayMath.GetMaxAndMinIndex(power.ToArray(), out max_index, out min_index);
                var Wavelength_nm = wave[max_index];

            }
            catch (Exception ex)
            {

            }
        }

        private void bt_choose_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog()
                {
                    InitialDirectory = Application.StartupPath + $@"\Data",
                };
                var dr = ofd.ShowDialog();
                if (dr != DialogResult.OK)
                {
                    return;

                }
                string FileName = ofd.FileName;

                if (panel_coarse.Controls.Count > 0)
                {
                    foreach (Control item in panel_coarse.Controls)
                    {
                        item.Dispose();
                    }
                }
                panel_coarse.Controls.Clear();
                //string CoarseTuningPath = @"D:\CT-3103\LaserX_TesterLibrary\Data\Coarse_tuning\DO123\TM346\T7891\(SW_EXAMPLE)\CoarseTuning\DO123_TM346_T7891_CoarseTuning#Deviations@55.00C_2024-04-11_14-50-22.csv";
                Form_CoarseTuning form_Coarse = new Form_CoarseTuning(FileName);
                form_Coarse.TopLevel = false;
                form_Coarse.FormBorderStyle = FormBorderStyle.None;
                form_Coarse.Dock = DockStyle.Fill;
                panel_coarse.Controls.Add(form_Coarse);
                form_Coarse.Show();

            }
            catch (Exception ex)
            {
            }
        }

        #region Oswitch

        private void btn_SwitchCh_Click(object sender, EventArgs e)
        {
            try
            {
                var ch = Convert.ToByte(txt_OpticalChannel.Text.Trim()) - 1;
                if (ch < 0) ch = 0;


                if (this._plugin.LocalResource.OSwitch.SetCH((byte)ch))
                {
                    MessageBox.Show("切换成功！");
                }
                else
                {
                    MessageBox.Show("切换失败！");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作异常！！！\r\n异常原因:[{ex.Message}-{ex.StackTrace}]!");
            }
        }

        #endregion

        private void btn_AnalyzeFileData_Deviations_Click(object sender, EventArgs e)
        {
            txt_SelectedFileList_Deviations.Text = "";

            string ManuConvertExcelDataPath = "Data\\ManualConvertExcelData";

            int Ch = 0; //需要提取的通道
            string ResultCommon = txt_ResultFileCommon_Deviations.Text; //结果注释

            try
            {
                Ch = int.Parse(txt_SelectedChannel_Deviations.Text);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return;
            }

            try
            {
                CommonOpenFileDialog ofd = new CommonOpenFileDialog();
                string paths = Path.Combine(Application.StartupPath, "Data");
                if (Directory.Exists(paths) == false)
                {
                    Directory.CreateDirectory(paths);
                }

                ofd.InitialDirectory = System.IO.Path.GetFullPath(paths);// Application.StartupPath + @"\BurnInData\XML";
                ofd.IsFolderPicker = true;
                ofd.Title = "选择文件夹";
                //ofd.Filter = "文件夹|*.xxx";
                //ofd.ValidateNames = false;
                ofd.Multiselect = true;

                if (ofd.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    var sourFiles = ofd.FileNames;


                    if (sourFiles.Count() > 1)
                    {
                        bool Head = false;
                        string strHead = "目录名,产品SN,";

                        //选择的目录
                        List<string> selectedpath = new List<string>();

                        //建立输出文件
                        string fullpath = System.IO.Path.GetFullPath(ManuConvertExcelDataPath);
                        if (Directory.Exists(fullpath) == false)
                        {
                            Directory.CreateDirectory(fullpath);
                        }

                        //目录名, 行字符串
                        Dictionary<string, List<string>> lstfileline = new Dictionary<string, List<string>>();

                        //遍历文件
                        foreach (var pathitem in sourFiles)
                        {
                            string CurrentFile = Path.Combine(pathitem, "Coarse_tuning", "Deviations.csv");

                            if (File.Exists(CurrentFile))
                            {
                                //提取目录名, 作为第1列
                                string PathName = Path.GetFileName(pathitem);

                                //增加
                                selectedpath.Add(PathName);


                                //提取目录名中的有效SN, 作为第2列
                                string[] splstring = PathName.Split('_');

                                string ProductSN = "NULL";
                                if (splstring.Length > 3)
                                {
                                    ProductSN = splstring[2];
                                }

                                //读取文件行
                                List<string> fileline = new List<string>();

                                using (StreamReader sr = new StreamReader(CurrentFile, Encoding.Default))
                                {
                                    //逐行读取文件处理至文件结束
                                    string str = "";

                                    while ((str = sr.ReadLine()) != null)
                                    {
                                        var col = str.Split(',');

                                        int tCh = 0;

                                        if (Head == false)
                                        {
                                            //输出文件头
                                            if (col[0].ToUpper() == "CH")
                                            {
                                                strHead += str;
                                                Head = true;
                                            }
                                        }

                                        if (int.TryParse(col[0], out tCh) == true)
                                        {
                                            if (tCh == Ch)//发现目标通道
                                            {
                                                fileline.Add($"{PathName},{ProductSN},{str}");
                                            }
                                        }
                                    }

                                }

                                if (fileline.Count == 0)
                                {
                                    fileline.Add($"{PathName},{ProductSN},目标通道不存在");
                                }

                                lstfileline.Add(PathName, fileline);

                            }
                        }



                        //整理为输出文件
                        string newfile = Path.Combine(ManuConvertExcelDataPath, string.Format(@"{0}_CH{1}_{2:yyyyMMdd_hhmmss}.csv", ResultCommon, Ch, DateTime.Now));


                        //信息窗口
                        this.Invoke((EventHandler)delegate
                        {
                            string str = "输入目录:\r\n";
                            foreach (var item in selectedpath)
                            {
                                str += item + "\r\n";
                            }

                            str += "输出文件:\r\n";
                            str += newfile + "\r\n";

                            txt_SelectedFileList_Deviations.Text = str;
                        });

                        newfile = System.IO.Path.GetFullPath(newfile);
                        using (StreamWriter sw = new StreamWriter(newfile, false, Encoding.Default))
                        {
                            sw.WriteLine(strHead);

                            foreach (var item in lstfileline)
                            {
                                foreach (var item2 in item.Value)
                                {
                                    sw.WriteLine(item2);
                                }
                            }

                        }

                        //资源管理器打开目录

                        System.Diagnostics.Process.Start(fullpath);

                    }
                    else
                    {
                        MessageBox.Show("选择目录少于2个");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btn_AnalyzeFileData_AlternativeQWLT_Click(object sender, EventArgs e)
        {

            txt_SelectedFileList_AlternativeQWLT.Text = "";

            string ManuConvertExcelDataPath = "Data\\ManualConvertExcelData";

            string ResultCommon = txt_ResultFileCommon_AlternativeQWLT.Text; //结果注释

            try
            {
                CommonOpenFileDialog ofd = new CommonOpenFileDialog();
                string paths = Path.Combine(Application.StartupPath, "Data");
                if (Directory.Exists(paths) == false)
                {
                    Directory.CreateDirectory(paths);
                }

                ofd.InitialDirectory = System.IO.Path.GetFullPath(paths);// Application.StartupPath + @"\BurnInData\XML";
                ofd.IsFolderPicker = true;
                ofd.Title = "选择文件夹";
                //ofd.Filter = "文件夹|*.xxx";
                //ofd.ValidateNames = false;
                ofd.Multiselect = true;

                if (ofd.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    var sourFiles = ofd.FileNames;


                    if (sourFiles.Count() >= 1)
                    {
                        bool Head = false;
                        string strHead = "目录名,产品SN,";

                        //选择的目录
                        List<string> selectedpath = new List<string>();

                        //建立输出文件
                        string fullpath = System.IO.Path.GetFullPath(ManuConvertExcelDataPath);
                        if (Directory.Exists(fullpath) == false)
                        {
                            Directory.CreateDirectory(fullpath);
                        }

                        //目录名, 行字符串
                        List<string> lstfileline = new List<string>();

                        //遍历文件
                        foreach (var pathitem in sourFiles)
                        {
                            //提取目录名, 作为第1列
                            string PathName = Path.GetFileName(pathitem);
                            //增加
                            selectedpath.Add(PathName);

                            string[] files = Directory.GetFiles(Path.Combine(pathitem, "AlternativeQWLT"), "AlternativeQWLT*.csv", SearchOption.TopDirectoryOnly);

                            foreach (string CurrentFile in files)
                            {

                                if (File.Exists(CurrentFile))
                                {

                                    //提取目录名中的有效SN, 作为第2列
                                    string[] splstring = PathName.Split('_');

                                    string ProductSN = "NULL";
                                    if (splstring.Length > 3)
                                    {
                                        ProductSN = splstring[2];
                                    }

                                    //读取文件行
                                    List<string> fileline = new List<string>();

                                    using (StreamReader sr = new StreamReader(CurrentFile, Encoding.Default))
                                    {
                                        //逐行读取文件处理至文件结束
                                        string str = "";

                                        while ((str = sr.ReadLine()) != null)
                                        {
                                            var col = str.Split(',');

                                            int tCh = 0;

                                            if (Head == false)
                                            {
                                                //输出文件头
                                                if (col.Length > 2 && col[1].Trim().ToUpper() == "CH")
                                                {
                                                    strHead += str;
                                                    Head = true;
                                                }
                                            }

                                            if (col[0].ToUpper() == "NEWVALUE")
                                            {
                                                fileline.Add($"{PathName},{ProductSN},{str}");
                                                break;
                                            }
                                        }

                                    }

                                    if (fileline.Count == 0)
                                    {
                                        fileline.Add($"{PathName},{ProductSN},目标通道不存在");
                                    }


                                    lstfileline.AddRange(fileline);

                                }
                            }
                        }



                        //整理为输出文件
                        string newfile = Path.Combine(ManuConvertExcelDataPath, string.Format(@"{0}_{1:yyyyMMdd_hhmmss}.csv", ResultCommon, DateTime.Now));


                        //信息窗口
                        this.Invoke((EventHandler)delegate
                        {
                            string str = "输入目录:\r\n";
                            foreach (var item in selectedpath)
                            {
                                str += item + "\r\n";
                            }

                            str += "输出文件:\r\n";
                            str += newfile + "\r\n";

                            txt_SelectedFileList_AlternativeQWLT.Text = str;
                        });

                        newfile = System.IO.Path.GetFullPath(newfile);
                        using (StreamWriter sw = new StreamWriter(newfile, false, Encoding.Default))
                        {
                            sw.WriteLine(strHead);

                            foreach (var item in lstfileline)
                            {
                                sw.WriteLine(item);
                            }

                        }

                        //资源管理器打开目录

                        System.Diagnostics.Process.Start(fullpath);

                    }
                    else
                    {
                        MessageBox.Show("选择目录少于1个");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bt_TC_start_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.IOs[IONameEnum_CT3103.TEC_Right].TurnOn(false);
                this._plugin.LocalResource.IOs[IONameEnum_CT3103.TEC_Left].TurnOn(false);

                float start = 0;
                if (!float.TryParse(this.tb_temp.Text, out start))
                {
                    return;
                }

                if (this.rb_left.Checked)
                {
                    this._plugin.LocalResource.TC_1.TemperatureSetPoint_DegreeC = start;
                    this._plugin.LocalResource.TC_1.IsOutputEnabled = true;
                }
                else if (this.rb_right.Checked)
                {
                    this._plugin.LocalResource.TC_2.TemperatureSetPoint_DegreeC = start;
                    this._plugin.LocalResource.TC_2.IsOutputEnabled = true;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void bt_TC_stop_Click(object sender, EventArgs e)
        {
            try
            {
                this._plugin.LocalResource.IOs[IONameEnum_CT3103.TEC_Right].TurnOn(false);
                this._plugin.LocalResource.IOs[IONameEnum_CT3103.TEC_Left].TurnOn(false);

                float start = 0;
                if (!float.TryParse(this.tb_temp.Text, out start))
                {
                    return;
                }

                if (this.rb_left.Checked)
                {
                    this._plugin.LocalResource.TC_1.IsOutputEnabled = false;
                }
                else if (this.rb_right.Checked)
                {
                    this._plugin.LocalResource.TC_2.IsOutputEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void btnDisConnectAllInstruments_Click(object sender, EventArgs e)
        {
            try
            {

                DialogResult result = MessageBox.Show("Are you sure you want to close all instrument connections on the LaserX Test platform?", "Tips", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    this._core.TryDisConnectAllInstruments();

                    MessageBox.Show($"All instrument connections on the platform have been closed");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }

        private void btnConnectAllInstruments_Click(object sender, EventArgs e)
        {
            try
            {

                DialogResult result = MessageBox.Show("Are you sure you want to connect all instrument connections on the LaserX Test platform?", "Tips", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    this._core.TryConnectAllInstruments();

                    MessageBox.Show($"All instrument connections on the platform have been connected");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }
         
        private void btnRunOESTest_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are you sure you want to run OES TestModule_OES?", "Tips", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    this._plugin.ShowOESMainForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }
    }


}