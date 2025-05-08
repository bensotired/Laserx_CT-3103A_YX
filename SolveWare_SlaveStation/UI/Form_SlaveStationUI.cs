using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_IO;
using SolveWare_Motion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_SlaveStation
{

    public partial class Form_SlaveStationUI : Form
    {
        Color YES_COLOR = Color.LimeGreen;
        Color NO_COLOR = Color.DarkGreen;
        //dgv1
        private const int Index_LocalName = 0;
        private const int Index_LocalPosition = 1;
        private const int Index_LocalOriginalName = 2;
        //dgv2
        private const int INFO_COL_INDEX = 0;
        private const int KEY_COL_INDEX = 1;
        private const int VAL_COL_INDEX = 2;
        IAxesPositionResourceProvider _positionResourceProvider = null;
        //进行线程的开始
        Task statusTask = null;
        CancellationTokenSource cancellationTokenSource = null;
        //本地绑定的
        List<SingleAxisBindingUIControl> singleAxisBindingUIControls = null;
        //轴运行
        MotionActionV2 _motorAction_V2 = new MotionActionV2();
        //轴关联的坐标们和轴的集合
        List<MotorAxisBase> localMotorAxisBaseList = new List<MotorAxisBase>();
        List<AxesPosition> localAxesPositionsList = new List<AxesPosition>();
        List<IOBase> localIOsList = new List<IOBase>();
        //名称对应 
        string[] xyzr = new string[] { "X", "Y", "Z", "R" };

        Dictionary<string, string> localName_OriginalName = new Dictionary<string, string>();
        Dictionary<string, string> originalName_LocalName = new Dictionary<string, string>();
        Dictionary<string, AxesPosition> dic_AxesPositions = new Dictionary<string, AxesPosition>();//根据点的名字得到点的所有信息

        Action<AxesPosition> _updatePluginAxesPositionAction { get; set; }
        Form frm_calibration = null;
        //IO操作
        Label[] input_labels = null;
        Label[] output_labels = null;
        Button[] output_buttons = null;
        Effective_IOs effective_IOs = new Effective_IOs();
        string _tempText = string.Empty;

        protected AutoResetEvent cancelDoneFlag = new AutoResetEvent(false);
        public Form_SlaveStationUI()
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            //构造结束后  应该label和button进行赋值
            input_labels = new Label[] 
            { 
                lbl_IN_1,
                lbl_IN_2, 
                lbl_IN_3, 
                lbl_IN_4,
                lbl_IN_5,
                lbl_IN_6,
                lbl_IN_7,
                lbl_IN_8,
                lbl_IN_9,
            };
            output_labels = new Label[]
            {
                lbl_OUT_1,
                lbl_OUT_2,
                lbl_OUT_3,
                lbl_OUT_4,
                lbl_OUT_5,
                lbl_OUT_6,
                lbl_OUT_7,
                lbl_OUT_8,
                lbl_OUT_9,
            };
            output_buttons = new Button[]
            {
                btn_output_1, 
                btn_output_2, 
                btn_output_3,
                btn_output_4,
                btn_output_5,
                btn_output_6,
                btn_output_7,
                btn_output_8,
                btn_output_9,
            };
            //将所有的都默认为不显示的状态  只有enable的时候才显示
            Pre_InitialLocalControlCollection();

        }

        private void Form_AxisGroupAction_Load(object sender, EventArgs e)
        {
            Load_SetAxisOtherName();
            Load_InitialComboBox();
            Load_SetIOBundingUI();
            cancellationTokenSource = new CancellationTokenSource();
            this.Invoke((EventHandler)delegate
            {
                if (this.frm_calibration != null)
                {
                    this.frm_calibration.Show();
                    this.pnl_calibration.Controls.Add(this.frm_calibration);
                }
                this.Text = this._tempText;
            });
            statusTask = Task.Factory.StartNew(() =>
            {
                UpdateUIStatus(cancellationTokenSource);
            }, cancellationTokenSource.Token);
        }


        //界面初始化的时候就是不行
        private void Pre_InitialLocalControlCollection()
        {
            singleAxisBindingUIControls = new List<SingleAxisBindingUIControl>();

            SingleAxisBindingUIControl singleAxisBindingUIControl_X = new SingleAxisBindingUIControl();
            singleAxisBindingUIControl_X.Lbl_Org = this.lbl_ORG_X;
            singleAxisBindingUIControl_X.Lbl_Plimit = this.lbl_PLimit_X;
            singleAxisBindingUIControl_X.Lbl_Nlimit = this.lbl_NLimit_X;
            singleAxisBindingUIControl_X.Lbl_Pos = this.lbl_Pos_X;
            singleAxisBindingUIControl_X.Lbl_OtherName = this.lbl_RealName_X;
            singleAxisBindingUIControl_X.Btn_Up = this.btnXUp;
            singleAxisBindingUIControl_X.Btn_Down = this.btnXDown;
            singleAxisBindingUIControl_X.Lbl_Logo = this.lbl_LOGO_X;
            singleAxisBindingUIControl_X.IsEnable = false;

            SingleAxisBindingUIControl singleAxisBindingUIControl_Y = new SingleAxisBindingUIControl();
            singleAxisBindingUIControl_Y.Lbl_Org = this.lbl_ORG_Y;
            singleAxisBindingUIControl_Y.Lbl_Plimit = this.lbl_PLimit_Y;
            singleAxisBindingUIControl_Y.Lbl_Nlimit = this.lbl_NLimit_Y;
            singleAxisBindingUIControl_Y.Lbl_Pos = this.lbl_Pos_Y;
            singleAxisBindingUIControl_Y.Lbl_OtherName = this.lbl_RealName_Y;
            singleAxisBindingUIControl_Y.Btn_Up = this.btnYUp;
            singleAxisBindingUIControl_Y.Btn_Down = this.btnYDown;
            singleAxisBindingUIControl_Y.Lbl_Logo = this.lbl_LOGO_Y;
            singleAxisBindingUIControl_Y.IsEnable = false;

            SingleAxisBindingUIControl singleAxisBindingUIControl_Z = new SingleAxisBindingUIControl();
            singleAxisBindingUIControl_Z.Lbl_Org = this.lbl_ORG_Z;
            singleAxisBindingUIControl_Z.Lbl_Plimit = this.lbl_PLimit_Z;
            singleAxisBindingUIControl_Z.Lbl_Nlimit = this.lbl_NLimit_Z;
            singleAxisBindingUIControl_Z.Lbl_Pos = this.lbl_Pos_Z;
            singleAxisBindingUIControl_Z.Lbl_OtherName = this.lbl_RealName_Z;
            singleAxisBindingUIControl_Z.Btn_Up = this.btnZUp;
            singleAxisBindingUIControl_Z.Btn_Down = this.btnZDown;
            singleAxisBindingUIControl_Z.Lbl_Logo = this.lbl_LOGO_Z;
            singleAxisBindingUIControl_Z.IsEnable = false;

            SingleAxisBindingUIControl singleAxisBindingUIControl_R = new SingleAxisBindingUIControl();
            singleAxisBindingUIControl_R.Lbl_Org = this.lbl_ORG_R;
            singleAxisBindingUIControl_R.Lbl_Plimit = this.lbl_PLimit_R;
            singleAxisBindingUIControl_R.Lbl_Nlimit = this.lbl_NLimit_R;
            singleAxisBindingUIControl_R.Lbl_Pos = this.lbl_Pos_R;
            singleAxisBindingUIControl_R.Lbl_OtherName = this.lbl_RealName_R;
            singleAxisBindingUIControl_R.Btn_Up = this.btnRUp;
            singleAxisBindingUIControl_R.Btn_Down = this.btnRDown;
            singleAxisBindingUIControl_R.Lbl_Logo = this.lbl_LOGO_R;
            singleAxisBindingUIControl_R.IsEnable = false;

            singleAxisBindingUIControls.Add(singleAxisBindingUIControl_X);
            singleAxisBindingUIControls.Add(singleAxisBindingUIControl_Y);
            singleAxisBindingUIControls.Add(singleAxisBindingUIControl_Z);
            singleAxisBindingUIControls.Add(singleAxisBindingUIControl_R);

            //全都不显示   只有在setconfig之后选择性显示
            foreach (var item in input_labels)
            {
                item.Visible = false;
            }
            foreach (var item in output_labels)
            {
                item.Visible = false;
            }
            foreach (var item in output_buttons)
            {
                item.Visible = false;
            }

        }

        //将combobox填充
        private void Load_InitialComboBox()
        {
            try
            {
                comboBox_Positions.Items.Clear();
                for (int i = 0; i < localAxesPositionsList.Count; i++)
                {
                    comboBox_Positions.Items.Add(localAxesPositionsList[i].Name);//点的名字
                    dic_AxesPositions.Add(localAxesPositionsList[i].Name, localAxesPositionsList[i]);//点的名字  点的信息
                }
                comboBox_Positions.Text = comboBox_Positions.Items[0].ToString();
                Update_DataGridView_Positions(comboBox_Positions.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //初始化 将坐标输入到界面上  触发事件吗?
        private void Load_InitialDataGridView_Positions()
        {

        }

        private void Update_DataGridView_Positions(string axesPositionName)
        {
            try
            {
                AxesPosition axesPosition = dic_AxesPositions[axesPositionName];
                Update_DataGridView_Positions(axesPosition);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Update_DataGridView_Positions(AxesPosition axesPosition)
        {
            try
            {
                dataGridView_Positions.Rows.Clear();
                for (int i = 0; i < axesPosition.ItemCollection.Count; i++)
                {
                    int rowIndex = dataGridView_Positions.Rows.Add();
                    dataGridView_Positions.Rows[rowIndex].Cells[Index_LocalName].Value = originalName_LocalName[axesPosition.ItemCollection[i].Name].ToString();
                    dataGridView_Positions.Rows[rowIndex].Cells[Index_LocalPosition].Value = axesPosition.ItemCollection[i].Position.ToString();
                    dataGridView_Positions.Rows[rowIndex].Cells[Index_LocalOriginalName].Value = axesPosition.ItemCollection[i].Name.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //转换成本地
        public void RunInitializer(
                              string slaveStationName,
                              List<MotorAxisBase> motorAxisBases,
                              List<bool> directionReverseList,
                              List<AxesPosition> axesPositions,
                              List<IOBase> iOBases,
                               IAxesPositionResourceProvider positionResourceProvider,
                            Form  frm_calibration
                              )
        {
            try
            {
                if (frm_calibration != null)
                {
                    this.frm_calibration = frm_calibration;
                }
                this._tempText = slaveStationName;
                this._positionResourceProvider = positionResourceProvider;
                //this._updatePluginAxesPositionAction = updatePluginAxesPositionAction;
                for (int i = 0; i < motorAxisBases.Count; i++)
                {
                    singleAxisBindingUIControls[i].MotorAxis = motorAxisBases[i];
                    singleAxisBindingUIControls[i].IsEnable = true;
                    singleAxisBindingUIControls[i].IsDirectionReverse = directionReverseList[i];
                    localName_OriginalName.Add(xyzr[i], motorAxisBases[i].Name);
                    originalName_LocalName.Add(motorAxisBases[i].Name, xyzr[i]);
                    localMotorAxisBaseList.Add(motorAxisBases[i]);
                }
                localAxesPositionsList = axesPositions;
                localIOsList = iOBases;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //用本地  一次就好
        private void Load_SetAxisOtherName()
        {
            for (int i = 0; i < singleAxisBindingUIControls.Count; i++)
            {
                if (!singleAxisBindingUIControls[i].IsEnable) continue;
                singleAxisBindingUIControls[i].Lbl_OtherName.Text = singleAxisBindingUIControls[i].MotorAxis.Name;//更新名字
            }
        }

        //设定IO绑定UI
        private void Load_SetIOBundingUI()
        {
            //限定最大的数目
            int LimitCount = 9;
            int inputCount = 0;
            int outputCount = 0;
            foreach (IOBase item in localIOsList)//最多支持4个
            {
                Effective_IO effective_IO = new Effective_IO();
                if (inputCount < LimitCount && item.IOSetting.IOType == IOType.INPUT)
                {
                    effective_IO.iOBase = item;
                    effective_IO.StatusLabel = input_labels[inputCount];
                    effective_IO.StatusLabel.Visible = true;
                    effective_IO.SetStatusLabelName();
                    inputCount++;
                }
                if (outputCount < LimitCount && item.IOSetting.IOType == IOType.OUTPUT)
                {
                    effective_IO.iOBase = item;
                    effective_IO.StatusLabel = output_labels[outputCount];
                    effective_IO.OnOffButton = output_buttons[outputCount];
                    effective_IO.StatusLabel.Visible = true;
                    effective_IO.OnOffButton.Visible = true;
                    effective_IO.SetStatusLabelName();
                    outputCount++;
                }
                effective_IOs.Add(effective_IO);
            }
        }



        private void UpdateUIStatus(CancellationTokenSource cancellationTokenSource)
        {
            try
            {
                do
                {
                    this.BeginInvoke((EventHandler)delegate
                    {
                        for (int i = 0; i < singleAxisBindingUIControls.Count; i++)
                        {
                            if (!singleAxisBindingUIControls[i].IsEnable)
                            {
                                continue;
                            }
                            try
                            {

                                //singleAxisBundingUIControls[i].Lbl_Org.BackColor = singleAxisBundingUIControls[i].MotorAxis.Interation.IsOrg ? Color.Tomato : Color.Lime;
                                //singleAxisBundingUIControls[i].Lbl_Plimit.BackColor = singleAxisBundingUIControls[i].MotorAxis.Interation.IsPosLimit ? Color.Tomato : Color.Lime;
                                //singleAxisBundingUIControls[i].Lbl_Nlimit.BackColor = singleAxisBundingUIControls[i].MotorAxis.Interation.IsNegLimit ? Color.Tomato : Color.Lime;

                                singleAxisBindingUIControls[i].Lbl_Org.BackColor = singleAxisBindingUIControls[i].MotorAxis.Interation.IsOrg ? YES_COLOR : NO_COLOR;
                                singleAxisBindingUIControls[i].Lbl_Plimit.BackColor = singleAxisBindingUIControls[i].MotorAxis.Interation.IsPosLimit ? YES_COLOR : NO_COLOR;
                                singleAxisBindingUIControls[i].Lbl_Nlimit.BackColor = singleAxisBindingUIControls[i].MotorAxis.Interation.IsNegLimit ? YES_COLOR : NO_COLOR;

                                singleAxisBindingUIControls[i].Lbl_Pos.Text = singleAxisBindingUIControls[i].MotorAxis.Interation.CurrentPosition.ToString();
                            }
                            catch
                            {

                            }
                            if (cancellationTokenSource.IsCancellationRequested == true)
                            {
                                break;
                            }
                        }

                        for (int i = 0; i < effective_IOs.collection.Count; i++)
                        {
                            //effective_IOs.collection[i].StatusLabel.BackColor = effective_IOs.collection[i].iOBase.Interation.IsActive ? Color.Tomato : Color.Lime;
                            effective_IOs.collection[i].StatusLabel.BackColor = effective_IOs.collection[i].iOBase.Interation.IsActive ? YES_COLOR : NO_COLOR;
                        }


                    });
                    Thread.Sleep(250);//不要刷新也太快了
                    if (cancellationTokenSource.IsCancellationRequested == true)
                    {
                        break;
                    }
                }
                while (true);
                cancelDoneFlag.Set();
            }
            catch
            {

            }
        }
        private void Form_SlaveStationUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.cancellationTokenSource.Cancel();
                this.cancelDoneFlag.WaitOne();
            }
            catch
            {

            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (rbtnE_Jog.Checked)
            {
                return;
            }

            int tag = Convert.ToInt32((sender as Button).Tag.ToString());
            int axisIndex = tag / 2;
            bool direction = tag % 2 == 0 ? false : true;//根据按钮先判断方向
            MotorAxisBase motorAxis = singleAxisBindingUIControls[axisIndex].MotorAxis;

            bool newDirection = direction;
            if (singleAxisBindingUIControls[axisIndex].IsDirectionReverse)
            {
                newDirection = !direction;
            }


            if (motorAxis == null || motorAxis.Interation.IsMoving == true)
            {
                return;
            }
            if (motorAxis.Interation.IsSimulation == true)
            {
            }
            else
            {
                //Task.Run(() =>
                //{
                    distanceRun(motorAxis, newDirection);
                //});
            }
        }

        private void distanceRun(MotorAxisBase motorAxis, bool direction)
        {
            try
            {
                double distance = 0;
                if (rbtnA_um.Checked)
                {
                    distance = 0.001;
                }
                else if (rbtnB_10um.Checked)
                {
                    distance = 0.01;
                }
                else if (rbtnC_100um.Checked)
                {
                    distance = 0.1;
                }
                else if (rbtnD_1mm.Checked)
                {
                    distance = 1.0;
                }

                this._motorAction_V2.Reset();
                int errorCode = ErrorCodes.NoError;
                double curPos = motorAxis.Interation.CurrentPosition;
                double targetPos = 0;
                if (direction)
                {
                    targetPos = curPos + distance;
                }
                else
                {
                    targetPos = curPos - distance;
                }
                errorCode = this._motorAction_V2.SingleAxisMotion(motorAxis, targetPos);

                if (errorCode != ErrorCodes.NoError)
                    MessageBox.Show($"轴运行错误:[{ErrorCodes.GetErrorDescription(errorCode)}]!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"轴运行错误:[{ex.Message}-{ex.StackTrace}]!");
            }
        }
        private void btnRun_MouseDown(object sender, MouseEventArgs e)
        {
            MotorAxisBase motorAxis = null;
            try
            {
                if (!rbtnE_Jog.Checked)
                {
                    return;
                }

                int tag = Convert.ToInt32((sender as Button).Tag.ToString());
                int axisIndex = tag / 2;
                bool direction = tag % 2 == 0 ? false : true;
                bool newDirection = direction;
                motorAxis = singleAxisBindingUIControls[axisIndex].MotorAxis;
                if (singleAxisBindingUIControls[axisIndex].IsDirectionReverse)
                {
                    newDirection = !direction;
                }


                if (!singleAxisBindingUIControls[axisIndex].IsEnable)
                {
                    return;
                }

                if (motorAxis == null || motorAxis.Interation.IsMoving == true)
                {
                    return;
                }
                if (motorAxis.Interation.IsSimulation == true)
                {
                    //模拟运动
                }
                else
                {
                    motorAxis.Jog(newDirection);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRun_MouseUp(object sender, MouseEventArgs e)
        {
            MotorAxisBase motorAxis = null;
            try
            {
                if (!rbtnE_Jog.Checked)
                {
                    return;
                }
                int tag = Convert.ToInt32((sender as Button).Tag.ToString());
                int axisIndex = tag / 2;
                motorAxis = singleAxisBindingUIControls[axisIndex].MotorAxis;
                if (!singleAxisBindingUIControls[axisIndex].IsEnable)
                {
                    return;
                }
                if (motorAxis == null)
                {
                    return;
                }
                if (motorAxis.Interation.IsSimulation == true)
                {
                    //模拟运动
                }
                else
                {
                    motorAxis.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btnEmergencyStop_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < singleAxisBindingUIControls.Count; i++)
                {
                    if (singleAxisBindingUIControls[i].IsEnable)
                    {
                        singleAxisBindingUIControls[i].MotorAxis.Stop();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox_Positions_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                var cb = sender as ComboBox;
                string erroeName = cb.Text;//这是错的
                var positionName = cb.SelectedItem.ToString();
                Update_DataGridView_Positions(positionName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public bool Update_MotorPositionCollection(AxesPosition tarAp)
        {
            bool isok = true;
            try
            {
                foreach (var axisPos in tarAp.ItemCollection)
                {
                    var axisInstance = localMotorAxisBaseList.Find
                        (
                            axis =>
                            axis.Name == axisPos.Name &&
                            axis.MotorGeneralSetting.MotorTable.CardNo == Convert.ToInt16(axisPos.CardNo) &&
                            axis.MotorGeneralSetting.MotorTable.AxisNo == Convert.ToInt16(axisPos.AxisNo)
                        );
                    if (axisInstance == null)
                    {
                        isok = false;
                        break;
                    }
                    else
                    {
                        axisPos.Position = axisInstance.Get_CurUnitPos();
                    }
                }
            }
            catch (Exception ex)
            {
                isok = false;
            }
            return isok;
        }

        private void btnSavePosition_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox_Positions.Text))
            {
                MessageBox.Show($"未选择任何需要保存的坐标!");
                return;
            }

            string positionName = comboBox_Positions.Text.Trim();
           

            DialogResult dialogResult = messageboxShow($"是否更新坐标[{positionName}]?");
            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            var pos = this._positionResourceProvider.GetAxesPosition_Object(positionName) as AxesPosition;
            if (pos == null)
            {
                MessageBox.Show($"保存AxesPosition失败:  {positionName}位置不存在!");
                return;
            }
            //更新本窗本地备份点位 与 
            //插件运行时资源点位 与 
            //平台资源点位  
            //AxesPosition为引用类型指向同一片内存地址
            var isOk = this.Update_MotorPositionCollection(pos);
            if (isOk == false)
            {
                MessageBox.Show($"更新AxesPosition失败!");
                return;
            }
            // 更新本窗UI
            this.Update_DataGridView_Positions(pos);
            // 保存到实体配置文件
            this._positionResourceProvider.SavePositionConfig();
            (this._positionResourceProvider as ITesterAppPluginInteration).RefreshOnceUI();
        }

        private void btnRunPosition_Click(object sender, EventArgs e)
        {
            if (comboBox_Positions.Items.Count < 1)
            {
                MessageBox.Show("没有点坐标，无法运行");
                return;
            }
            if (_motorAction_V2?.GetSTATUS() != STATUS.FREE)
            {
                MessageBox.Show("轴正在运行");
                return;
            }
            _motorAction_V2?.Reset();
            string axesPositionName = comboBox_Positions.Text;
            AxesPosition axesPosition = dic_AxesPositions[axesPositionName];
            Task.Factory.StartNew(() =>
            {
                _motorAction_V2?.MultipleAxisMotion(localMotorAxisBaseList, axesPosition);
            });
        }
        private DialogResult messageboxShow(string message)
        {
            DialogResult dialogResult = MessageBox.Show(
                  message,
                  "提示",
                  MessageBoxButtons.OKCancel,
                  MessageBoxIcon.Information,
                  MessageBoxDefaultButton.Button1,
                  MessageBoxOptions.DefaultDesktopOnly);
            return dialogResult;
        }
     

        private void btn_output_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button.Text.Trim() == "开")
            {
                button.Text = "关";
                Effective_IO effective_IO = effective_IOs[button.Name];
                effective_IO.iOBase.TurnOn(true);
            }
            else if (button.Text.Trim() == "关")
            {
                button.Text = "开";
                effective_IOs[button.Name].iOBase.TurnOn(false);
            }
        }
    }
}