using LX_BurnInSolution.Utilities;
using SolveWare_BinSorter;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using SolveWare_TestPlugin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestPlugin_Demo
{
    public partial class Form_TestEnterance_CT3103 : Form_TestEnterance_TestPlugin<TestPluginWorker_CT3103>
    {
        Form _binUI;

        System.Timers.Timer m_refreshTimer;


        public Form_TestEnterance_CT3103()
        {
            InitializeComponent();
        }
        private void Form_TestEnterance_CT3103_Load(object sender, EventArgs e)
        {
            try
            {
                this.RefreshOnce();
                m_refreshTimer = new System.Timers.Timer(2000);
                m_refreshTimer.Elapsed += M_refreshTimer_Elapsed;

                this.RefreshUserCustomizedInformation();
            }
            catch (Exception ex)
            {
            }
        }

        private void M_refreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            m_refreshTimer.Stop();
            try
            {
                if (this._plugin.Interation.RunStatus == PluginRunStatus.Running)
                {
                    //启动后不能再次点击  与开始一样
                    btn_startTest.Enabled = false;
                    //相反
                    btn_All_RequesResumeTest.Enabled = true;
                    btn_All_RequesPauseTest.Enabled = true;

                    //急停不参与?考虑做一个单按钮尝试Enable?



                }
                else
                {




                    btn_startTest.Enabled = true;

                    btn_All_RequesPauseTest.Enabled = true;

                    btn_All_RequesResumeTest.Enabled = true;

                }
            }
            catch
            {

            }
            m_refreshTimer.Start();

        }




        protected override void CreateTestProfileEditorUI()
        {
            if (this._TestProfileEditorUI == null || this._TestProfileEditorUI.IsDisposed == true)
            {
                this._TestProfileEditorUI = new Form_TestProfileEditor_CT3103();
                this._core.LinkToCore(_TestProfileEditorUI as ITesterAppUI);
                (this._TestProfileEditorUI as ITesterAppUI).ConnectToAppInteration(this._plugin);
            }
        }


        private void cb_testProfileSelector_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                var fileDict = this._core.GetLocalTestProfileFiles();

                var solutionName = cmb_TestProfile_Selector.SelectedItem.ToString();
                var testprofileFullName = fileDict[solutionName];
                var profileInstance = this._core.LoadTestProfileWithExtraTypes<TestPluginImportProfile_CT3103>(testprofileFullName) as TestPluginImportProfile_CT3103;


                //20241121 客户要求对"老化前"/"老化后" 进行名称匹配进行选择
                string[] strBefore = new string[] { "老化前", "Before" };
                string[] strAfter = new string[] { "老化后", "After" };

                bool bBeforeExist = false;
                bool bAterExist = false;

                string solutionName_B = solutionName.ToUpper();

                foreach (var item in strBefore)
                {
                    if(solutionName_B.Contains(item.ToUpper()))
                    {
                        bBeforeExist = true;
                        break;
                    }
                }

                foreach (var item in strAfter)
                {
                    if(solutionName_B.Contains(item.ToUpper()))
                    {
                        bAterExist = true;
                        break;
                    }
                }

                if(bBeforeExist)
                {
                    rb_PreBI.Checked = true;
                }
                else if(bAterExist)
                {
                    rb_PostBI.Checked = true;
                }

                //var binCollectionName = profileInstance.BinCollectionName;

                //this._plugin.binCollectionName = binCollectionName;//获取测试链中Bin的名字

                //var BinSettingCollection = this._plugin.LocalResource.Local_BinSortList_ResourceProvider.GetBinSettingCollectionObject(binCollectionName) as BinSettingCollection;
                //var binSettingNames = BinSettingCollection.GetDataListByPropName<string>("Name");


            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作异常！异常原因:{ex.Message} - {ex.StackTrace}!");
            }

        }


        private void btn_editTestProfile_Click(object sender, EventArgs e)
        {
            try
            {

                this.CreateTestProfileEditorUI();
                this._core.ModifyDockableUI(this._TestProfileEditorUI, false);

                this._TestProfileEditorUI.ShowDialog();

                RefreshOnce();
            }
            catch (Exception ex)
            {

            }
        }


        private void btn_startTest_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    //检测TestData.csv是否认可以正常打开，不然应该数据写入
                    string dirpath = Path.GetFullPath(Application.StartupPath) + @"\TestData\TestData.csv";
                    using (StreamWriter sw = new StreamWriter(dirpath, false, Encoding.GetEncoding("gb2312")))
                    {
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show(@"\TestData\TestData.csv  文件打开异常！！！");
                    return;
                }

                //20241111 模拟量板卡功能存在的确认, 1#板卡必须要有模拟量功能
                //这种方法不好 先这样用.
                try
                {
                    if (SolveWare_BurnInInstruments.LaserX_9078_Utilities.ResIsAdjusted[1] == false)
                    {
                        MessageBox.Show("模拟量卡 功能/档位调节器运行 异常");
                        return;
                    }

                }
                catch (Exception ex1)
                {
                    MessageBox.Show($"模拟量卡 数量/功能 异常\r\nEx:{ex1}");
                    return;
                }

                //this._plugin.LocalResource.



                Form_ShowCSV cSV = new Form_ShowCSV();
                cSV.ShowDialog();
                if (cSV.DialogResult != DialogResult.OK)
                {
                    return;
                }
                if (!this._plugin.LocalResource.IOs[IONameEnum_CT3103.SEN_FLOW].Interation.IsActive)
                {
                    MessageBox.Show("流量计检测异常！！！\r\n水冷机未打开！！！");
                    return;
                }

                if (this.rb_PreBI.Checked)
                {
                    this._plugin.Purpose = "Pre BI";
                }
                if (this.rb_PostBI.Checked)
                {
                    this._plugin.Purpose = "Post BI";
                }

                this._plugin.MaskName = this.tb_MaskName.Text;
                this._plugin.WaferName = this.tb_WaferName.Text;
                if (string.IsNullOrEmpty(this.tb_ChipName.Text) || this.tb_ChipName.Text.Contains("Demo"))
                {

                }
                else
                {
                    this._plugin.ChipName = this.tb_ChipName.Text + "_";
                }

                this._plugin.WorkOrder = this.tb_WorkOrder.Text;

                double templeft = 25;
                if (!double.TryParse(this.textBox_Left_TempList.Text, out templeft))
                {
                    MessageBox.Show("左平台温度参数错误！");
                    return;
                }
                double tempright = 25;
                if (!double.TryParse(this.textBox_Right_TempList.Text, out tempright))
                {
                    MessageBox.Show("右平台温度参数错误！");
                    return;
                }
                double Tolerance = 1;
                if (!double.TryParse(this.textBox_TempTolerance.Text, out Tolerance))
                {
                    MessageBox.Show("温度容差参数错误！");
                    return;
                }

                List<double> tmpleft = new List<double>();
                tmpleft.Add(templeft);
                List<double> tmpright = new List<double>();
                tmpright.Add(tempright);
                this._plugin.parameter.TemperatureListLeft = tmpleft;
                this._plugin.parameter.TemperatureListRight = tmpright;
                this._plugin.parameter.TemperatureTolerance = Tolerance;


                List<string> CarrierNumberlist = new List<string>();
                int count = 0;
                var ispass = this._plugin.GetCarrierNumber(out CarrierNumberlist, out count);
                if (!ispass)
                {
                    MessageBox.Show("夹具编号信息获取失败！");
                    return;
                }
                if (count == 0)
                {
                    MessageBox.Show("无可用夹具！");
                    return;
                }

                this._plugin.parameter.CarrierNumberlist = CarrierNumberlist;
                this._plugin.parameter.CarrierNumber = count;
                this._plugin.parameter.UnLoadCarrierNumber = count;
                this._plugin.Status.LoadLeftStation = true;
                this._plugin.Status.LoadRightStation = true;
                this._plugin.Status.UnLoadLeftStation = false;
                this._plugin.Status.UnLoadRightStation = false;
                //this._plugin.LocalResource.IOs[IONameEnum_CT3103.TWR_GRN].TurnOn(true);
                //this._plugin.Device_DryRun();

                //return;

                if (this._plugin.Interation.CanImportTestProfileOrResetPluginStatus == false)
                {
                    var msg = $"测试组件状态为[{this._plugin.Interation.PluginStatusInfo}]!测试组件状态将不会重置!";
                    this._core.Log_Global(msg);
                    MessageBox.Show(msg, "测试组件状态重置失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    this._plugin.ResetWorker();
                    this._core.Log_Global($"重置测试状态完成.");
                }


                this._core.Log_Global($"导入测试档..");

                if (this.cmb_TestProfile_Selector.SelectedItem == null ||
                    string.IsNullOrEmpty(this.cmb_TestProfile_Selector.SelectedItem.ToString()))
                {
                    var msg = $"未选择需要的测试档!测试任务将不会执行!";
                    this._core.Log_Global(msg);
                    MessageBox.Show(msg);
                    return;
                }

                var profileKey = this.cmb_TestProfile_Selector.SelectedItem.ToString();
                var fileDict = this._core.GetLocalTestProfileFiles();
                if (fileDict.ContainsKey(profileKey) == false ||
                    File.Exists(fileDict[profileKey]) == false)
                {
                    var msg = $"所选测试档[{profileKey}]本地文件不存在!";
                    this._core.Log_Global(msg);
                    MessageBox.Show(msg);
                    return;
                }

                var tempProfile = this._core.LoadTestProfileWithExtraTypes<TestPluginImportProfile_CT3103>(fileDict[profileKey]) as ITestPluginImportProfileBase;
                if (tempProfile == null)
                {
                    var msg = $"解析测试档[{profileKey}]本地文件失败!";
                    this._core.Log_Global(msg);
                    MessageBox.Show(msg);
                    return;
                }

                //var profileInstance = this._core.LoadTestProfileWithExtraTypes<TestPluginImportProfile_CT3103>(fileDict[profileKey]) as TestPluginImportProfile_CT3103;
                //var binCollectionName = profileInstance.BinCollectionName;

                //this._plugin.binCollectionName = binCollectionName;//获取测试链中Bin的名字

                if (string.IsNullOrEmpty(txt_OperatorID.Text) == true)
                {
                    MessageBox.Show("请输入操作员名称!!!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    return;
                }
                else
                {
                    this._plugin.OperatorID = txt_OperatorID.Text.Trim();
                }

                string checkProfileLog = string.Empty;
                //导入到测试组件
                bool isProfileContextOk = this._plugin.CheckTestProfileContext(tempProfile, out checkProfileLog);
                if (isProfileContextOk == false)
                {
                    MessageBox.Show(checkProfileLog, "测试档检查结果", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string importLog = string.Empty;

                bool isProfileImported = this._plugin.ImportTestProfile(tempProfile, out importLog);
                if (isProfileImported == false)
                {
                    MessageBox.Show(importLog, "测试档导入结果", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this._core.Log_Global($"测试档导入成功!");

                this._plugin.ResetAllMotionActionToken();
                var isStartedTest = this._plugin.StartTest();

                if (isStartedTest == false)
                {
                    this._plugin.Paused_IO(1);
                    MessageBox.Show(" 测试不会开始！");
                    return;
                }
                

                UserCustomizedInformation userCustomized = new UserCustomizedInformation();

                userCustomized.Left_Temp = this.textBox_Left_TempList.Text;
                userCustomized.Right_Temp = this.textBox_Right_TempList.Text;
                userCustomized.Tolerance_Temp = this.textBox_TempTolerance.Text;


                userCustomized.ChipName = this.tb_ChipName.Text;
                userCustomized.MaskName = this.tb_MaskName.Text;
                userCustomized.WaferName = this.tb_WaferName.Text;
                userCustomized.WorkOrder = this.tb_WorkOrder.Text;
                this._plugin.SerUserCustomized(userCustomized);

                this._plugin.Resume_IO();
                Controls_Enable_False();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"测试档导入错误:[{ex.Message}-{ex.StackTrace}]!");
                return;
            }
        }


        private void btn_All_RequesPauseTest_Click(object sender, EventArgs e)
        {
            if (IsMachineRunning() == true)
            {

                if (this._plugin.IsAnyonePause() == true)
                {
                    var msg = $"设备已暂停，请勿重复点击操作！";
                    this._core.Log_Global(msg);
                    MessageBox.Show(msg);
                    return;
                }


                this._plugin.UserRequest_MasterControl_Pause();
            }
            else
            {

            }
        }

        private void btn_All_RequesResumeTest_Click(object sender, EventArgs e)
        {
            if (IsMachineRunning() == false)
            {
                return;
            }

            if (this._plugin.IsAnyonePause() == false)
            {
                var msg = $"设备正在运行中，请在设备暂停后使用该功能！";
                this._core.Log_Global(msg);
                MessageBox.Show(msg);
                return;
            }
            //继续测试前可加判定

            this._plugin.UserRequest_MasterControl_Resume();
        }

        private void btn_Normal_StopTest_Click(object sender, EventArgs e)
        {
            if (IsMachineRunning() == false)
            {
                return;
            }

            Controls_Enable_False();


            this._plugin.UserRequest_MasterControl_Resume();

            btn_startTest.Enabled = false;

            btn_All_RequesPauseTest.Enabled = false;

            btn_All_RequesResumeTest.Enabled = false;

            btn_Normal_StopTest.Enabled = false;

        }

        private void btn_Ex_StopTest_Click(object sender, EventArgs e)
        {

            if (IsMachineRunning() == false)
            {
                Controls_Enable_True();
                return;
            }

            var dr = MessageBox.Show($"请确认是否执行当前操作！\r\n" +
                $"执行当前操作将导致自动测试流程异常退出！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            if (dr != DialogResult.Yes)
            {
                return;
            }


            this._plugin.StopTest();

            Controls_Enable_True();
        }



        public override void RefreshOnce()
        {
            try
            {
                this.Invoke((EventHandler)delegate
                {
                    var fileDict = this._core.GetLocalTestProfileFiles();
                    cmb_TestProfile_Selector.Items.Clear();
                    if (fileDict.Count > 0)
                    {
                        cmb_TestProfile_Selector.Items.AddRange(fileDict.Keys.ToArray());
                        //cmb_TestProfile_Selector.SelectedIndex = 0;

                        //var solutionName = cmb_TestProfile_Selector.SelectedItem.ToString();

                        //var testprofileFullName = fileDict[solutionName];
                        //var profileInstance = this._core.LoadTestProfileWithExtraTypes<TestPluginImportProfile_CT3103>(testprofileFullName) as TestPluginImportProfile_CT3103;

                    }

                });
            }
            catch (Exception ex)
            {

            }
        }
        private void Controls_Enable_False()
        {

            cmb_TestProfile_Selector.Enabled = false;
            btn_EditTestProfile.Enabled = false;
            txt_OperatorID.Enabled = false;

            bt_Left_enable.Enabled = false;
            bt_Right_enable.Enabled = false;

            rb_PreBI.Enabled = false;
            rb_PostBI.Enabled = false;
        }
        public void Controls_Enable_True()
        {

            if (this.InvokeRequired)
            {
                this.Invoke((EventHandler)delegate
                {

                    cmb_TestProfile_Selector.Enabled = true;
                    btn_EditTestProfile.Enabled = true;
                    txt_OperatorID.Enabled = true;



                    btn_startTest.Enabled = true;

                    btn_All_RequesPauseTest.Enabled = true;

                    btn_All_RequesResumeTest.Enabled = true;

                    btn_Normal_StopTest.Enabled = true;

                    bt_Left_enable.Enabled = true;
                    bt_Right_enable.Enabled = true;
                    rb_PreBI.Enabled = true;
                    rb_PostBI.Enabled = true;
                });
            }
            else
            {
                cmb_TestProfile_Selector.Enabled = true;
                btn_EditTestProfile.Enabled = true;
                txt_OperatorID.Enabled = true;


                btn_startTest.Enabled = true;

                btn_All_RequesPauseTest.Enabled = true;

                btn_All_RequesResumeTest.Enabled = true;

                btn_Normal_StopTest.Enabled = true;

                bt_Left_enable.Enabled = true;
                bt_Right_enable.Enabled = true;
                rb_PreBI.Enabled = true;
                rb_PostBI.Enabled = true;
            }

        }
        private bool IsMachineRunning()
        {

            if (this._plugin.Interation.RunStatus == PluginRunStatus.Running)
            {
                return true;
            }
            else
            {
                var msg = $"设备未运行！该功能不可用！";
                MessageBox.Show(msg);
                return false;
            }

        }
        #region TEC
        //private void bt_TC_1_Click(object sender, EventArgs e)
        //{
        //    double wavelen = 1;
        //    if (double.TryParse(this.textBox_TC_1.Text, out wavelen) == false)
        //    {
        //        MessageBox.Show("目标温度值格式错误!");
        //        return;
        //    }
        //    this._plugin.TEC_Controller_1(wavelen);
        //}

        //private void bt_TC_3_Click(object sender, EventArgs e)
        //{
        //    double wavelen = 1;
        //    if (double.TryParse(this.textBox_TC_3.Text, out wavelen) == false)
        //    {
        //        MessageBox.Show("目标温度值格式错误!");
        //        return;
        //    }
        //    this._plugin.TEC_Controller_3(wavelen);

        //}

        //private void bt_TC_2_Click(object sender, EventArgs e)
        //{
        //    double wavelen = 1;
        //    if (double.TryParse(this.textBox_TC_2.Text, out wavelen) == false)
        //    {
        //        MessageBox.Show("目标温度值格式错误!");
        //        return;
        //    }
        //    this._plugin.TEC_Controller_2(wavelen);
        //}

        //private void bt_TC_4_Click(object sender, EventArgs e)
        //{
        //    double wavelen = 1;
        //    if (double.TryParse(this.textBox_TC_4.Text, out wavelen) == false)
        //    {
        //        MessageBox.Show("目标温度值格式错误!");
        //        return;
        //    }
        //    this._plugin.TEC_Controller_4(wavelen);
        //}
        #endregion
        private void bt_Left_enable_Click(object sender, EventArgs e)
        {
            if (this._plugin.Status.LeftStation)
            {
                this._plugin.Status.LeftStation = false;
                bt_Left_enable.Text = "左载台禁用";
                bt_Left_enable.BackColor = Color.Red;
            }
            else
            {
                this._plugin.Status.LeftStation = true;
                bt_Left_enable.Text = "左载台启用";
                bt_Left_enable.BackColor = Color.LightGreen;
            }
        }

        private void bt_Right_enable_Click(object sender, EventArgs e)
        {
            if (this._plugin.Status.RightStation)
            {
                this._plugin.Status.RightStation = false;
                bt_Right_enable.Text = "右载台禁用";
                bt_Right_enable.BackColor = Color.Red;
            }
            else
            {
                this._plugin.Status.RightStation = true;
                bt_Right_enable.Text = "右载台启用";
                bt_Right_enable.BackColor = Color.LightGreen;
            }
        }

        private void rb_PostBI_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rb_PostBI.Checked)
            {
                this.cb_PostBIColumn.Enabled = true;
            }
            else
            {
                this.cb_PostBIColumn.Enabled = false;
            }
        }

        private void rb_PostBI_Compare_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rb_PostBI_Compare.Checked)
            {
                this.cb_PostBIColumn_Compare.Enabled = true;
            }
            else
            {
                this.cb_PostBIColumn_Compare.Enabled = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.groupBox1.Enabled = true;
            }
            else
            {
                this.groupBox1.Enabled = false;
            }
        }

        private void RefreshUserCustomizedInformation()
        {
            var Information = this._plugin.DserUserCustomized();

            this.textBox_Left_TempList.Text = Information.Left_Temp;
            this.textBox_Right_TempList.Text = Information.Right_Temp;
            this.textBox_TempTolerance.Text = Information.Tolerance_Temp;

            this.tb_MaskName.Text = Information.MaskName;
            this.tb_WaferName.Text = Information.WaferName;
            this.tb_ChipName.Text = Information.ChipName;
            this.tb_WorkOrder.Text = Information.WorkOrder;
        }
        private void bt_Test_Click(object sender, EventArgs e)
        {
            if (this._plugin.Interation.CanImportTestProfileOrResetPluginStatus == false)
            {
                var msg = $"测试组件状态为[{this._plugin.Interation.PluginStatusInfo}]!测试组件状态将不会重置!";
                this._core.Log_Global(msg);
                MessageBox.Show(msg, "测试组件状态重置失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                this._plugin.ResetWorker();
                this._core.Log_Global($"重置测试状态完成.");
            }


            this._core.Log_Global($"导入测试档..");

            if (this.cmb_TestProfile_Selector.SelectedItem == null ||
                string.IsNullOrEmpty(this.cmb_TestProfile_Selector.SelectedItem.ToString()))
            {
                var msg = $"未选择需要的测试档!测试任务将不会执行!";
                this._core.Log_Global(msg);
                MessageBox.Show(msg);
                return;
            }

            var profileKey = this.cmb_TestProfile_Selector.SelectedItem.ToString();
            var fileDict = this._core.GetLocalTestProfileFiles();
            if (fileDict.ContainsKey(profileKey) == false ||
                File.Exists(fileDict[profileKey]) == false)
            {
                var msg = $"所选测试档[{profileKey}]本地文件不存在!";
                this._core.Log_Global(msg);
                MessageBox.Show(msg);
                return;
            }

            var tempProfile = this._core.LoadTestProfileWithExtraTypes<TestPluginImportProfile_CT3103>(fileDict[profileKey]) as ITestPluginImportProfileBase;
            if (tempProfile == null)
            {
                var msg = $"解析测试档[{profileKey}]本地文件失败!";
                this._core.Log_Global(msg);
                MessageBox.Show(msg);
                return;
            }
            string checkProfileLog = string.Empty;
            //导入到测试组件
            bool isProfileContextOk = this._plugin.CheckTestProfileContext(tempProfile, out checkProfileLog);
            if (isProfileContextOk == false)
            {
                MessageBox.Show(checkProfileLog, "测试档检查结果", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string importLog = string.Empty;

            bool isProfileImported = this._plugin.ImportTestProfile(tempProfile, out importLog);
            if (isProfileImported == false)
            {
                MessageBox.Show(importLog, "测试档导入结果", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this._core.Log_Global($"测试档导入成功!");

            DeviceStreamData_CT3103 data_demo = new DeviceStreamData_CT3103();
            data_demo.Information = "Demo_1";
            data_demo.SerialNumber = "111111111";
            this._plugin._mainStreamData.AddToDataCollection(data_demo);
            tokenSource = new System.Threading.CancellationTokenSource();
            Task task1 = Task.Factory.StartNew(() =>
            {
                this._plugin.test(data_demo, tokenSource);
                var a = data_demo.RawDataCollection;
            }, TaskCreationOptions.LongRunning);

        }
        System.Threading.CancellationTokenSource tokenSource = new System.Threading.CancellationTokenSource();

        private void button1_Click(object sender, EventArgs e)
        {
            tokenSource.Cancel();
        }

        public void RefreshCarrierID(string carrID)
        {
            this.Invoke((EventHandler)delegate
            {
                tb_CarrierID.Text = carrID;
            });
        }
        public void RefreshOeskID(string oeskID)
        {
            this.Invoke((EventHandler)delegate
            {
                tb_OeskID.Text = oeskID;
            });
        }
    }
}
