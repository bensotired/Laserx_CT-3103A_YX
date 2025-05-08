using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_Motion
{
    public partial class Form_PositionNameSetting : Form, ITesterCoreLink
    {
        ITesterCoreInteration _core;
        AxesPositionCollection Main_AxesPositionCollection;
        List<MotorAxisBase> SelectedAxes;
        public void ConnectToCore(ITesterCoreInteration core)
        {
            _core = core as ITesterCoreInteration;
        }

        public void DisconnectFromCore(ITesterCoreInteration core)
        {
            _core = null;
        }
        public Form_PositionNameSetting()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void Form_PositionNameSetting_Load(object sender, EventArgs e)
        {
            DisplaySelectAxisInfo(SelectedAxes);
            Diaplay_SingleAxisControl();
        }

        /// <summary>
        /// 确认按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxPositionName.Text))
            {
                messageboxShow("输入的名称不能为空");
                return;
            }
            DialogResult dialogResult = messageboxShow("是否保存新坐标");
            if (dialogResult == DialogResult.OK)
            {
                string positionName = textBoxPositionName.Text.Trim();
                if (checkNameIn_MotorPositionCollection(positionName))
                {
                    var dr = MessageBox.Show($"已存在相同名称的坐标[{positionName}],是否替换?", $"坐标[{positionName}]替换提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        Update_MotorPositionCollection(this.SelectedAxes, positionName);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                this.Add_MotorPositionCollection(this.SelectedAxes, positionName);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        /// <summary>
        /// 将选中的轴写入
        /// </summary>
        public void Insert_SelectAxes(List<MotorAxisBase> AxesSelect)
        {
            this.SelectedAxes = AxesSelect;
        }



        public void Insert_AxesPositionCollection(AxesPositionCollection axesPositionCollection)
        {
            this.Main_AxesPositionCollection = axesPositionCollection;
        }
        public void Enable_AxesPositionNameEditor(bool isEnable)
        {
            this.textBoxPositionName.Enabled = isEnable;
        }
        public void SetDefault_AxesPositionNameEditor(string AxesPositionName)
        {
            this.textBoxPositionName.Text = AxesPositionName;
        }

        /// <summary>
        /// 检查名字是否在集合中
        /// </summary>
        /// <param name="name">名字是文本中输入的</param>
        /// <returns></returns>
        public bool checkNameIn_MotorPositionCollection(string name)
        {
            return this.Main_AxesPositionCollection.ItemCollection.Exists(item => item.Name == name);
        }



        /// <summary>
        /// 增加新的点集合 点集合中包含了轴 位置和运动排序
        /// </summary>
        /// <param name="AxesSelect">输入的轴</param>
        /// <param name="positionName">输入的名字</param>
        public void Add_MotorPositionCollection(List<MotorAxisBase> AxesSelect, string positionName)
        {
            AxesPosition axesPosition = new AxesPosition();
            axesPosition.Name = positionName;
            //axesPosition.ID = this.SetID();
            for (int i = 0; i < AxesSelect.Count; i++)
            {
                AxisPosition axisPosition = new AxisPosition();
                axisPosition.Name = AxesSelect[i].MotorGeneralSetting.Name;
                axisPosition.CardNo = AxesSelect[i].MotorGeneralSetting.MotorTable.CardNo.ToString();
                axisPosition.AxisNo = AxesSelect[i].MotorGeneralSetting.MotorTable.AxisNo.ToString();
                axisPosition.Position = AxesSelect[i].Get_CurUnitPos();
                axesPosition.AddSingleItem(axisPosition);
            }
            //总集合中
            this.Main_AxesPositionCollection.AddSingleItem(axesPosition);
        }


        /// <summary>
        /// 增加新的点集合 点集合中包含了轴 位置和运动排序
        /// </summary>
        /// <param name="AxesSelect">输入的轴</param>
        /// <param name="positionName">输入的名字</param>
        public void Update_MotorPositionCollection(List<MotorAxisBase> AxesSelect, string positionName)
        {
            AxesPosition axesPosition = new AxesPosition();
            axesPosition.Name = positionName;
            //axesPosition.ID = this.SetID();
            for (int i = 0; i < AxesSelect.Count; i++)
            {
                AxisPosition axisPosition = new AxisPosition();
                axisPosition.Name = AxesSelect[i].MotorGeneralSetting.Name;
                axisPosition.CardNo = AxesSelect[i].MotorGeneralSetting.MotorTable.CardNo.ToString();
                axisPosition.AxisNo = AxesSelect[i].MotorGeneralSetting.MotorTable.AxisNo.ToString();
                axisPosition.Position = AxesSelect[i].Get_CurUnitPos();
                axesPosition.AddSingleItem(axisPosition);
            }
            string errMsg = string.Empty;
            //总集合中
            this.Main_AxesPositionCollection.UpdateSingleItem(axesPosition,ref errMsg);
        }
        /// <summary>
        /// 显示信息 返回信息的值
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 展现选中轴的信息
        /// </summary>
        /// <param name="Axes"></param>
        private void DisplaySelectAxisInfo(List<MotorAxisBase> Axes)
        {
            string Info = string.Empty;
            for (int i = 0; i < Axes.Count; i++)
            {
                Info += $"axisInfo:{Axes[i].MotorGeneralSetting.Name}";
                Info += "\t";
                Info += $"axisNo  :{Axes[i].MotorGeneralSetting.MotorTable.AxisNo.ToString()}";
                Info += "\n";
            }
            this.richTextBox1.Text = Info;
        }

        private void Diaplay_SingleAxisControl()
        {
            Task.Factory.StartNew(() =>
            {
                this._core.GUIRunUIInvokeAction(() =>
                {
                    this.SuspendLayout();
                    Form_TLP_layer layer = new Form_TLP_layer();
                    List<Form> frms = new List<Form>();
                    for (int i = 0; i < SelectedAxes.Count; i++)
                    {
                        Form_SingleAxisControl form_SingleAxisControl = new Form_SingleAxisControl();
                        form_SingleAxisControl.AssignAxis(SelectedAxes[i]);
                        form_SingleAxisControl.gb_info.Text = GetAxisInfo(SelectedAxes[i]);
                        frms.Add(form_SingleAxisControl);
                    }
                    if (frms.Count > 4)
                    {
                        layer.LayoutSubForms(frms.ToArray(), 2);
                    }
                    else
                    {
                        layer.LayoutSubForms(frms.ToArray(), 2, 2);
                    }
                    layer.Show();
                    gb_axesMotion.Controls.Clear();
                    gb_axesMotion.Controls.Add(layer);
                    this.ResumeLayout(false);
                });
            });
        }


        /// <summary>
        /// 这个将单轴的信息显示到界面上，不然不知道是哪个轴
        /// </summary>
        /// <param name="axisBase"></param>
        /// <returns></returns>
        private string GetAxisInfo(MotorAxisBase axisBase)
        {
            string retInfo = string.Empty;
            string name = axisBase.MotorGeneralSetting.Name;
            string cardNo = axisBase.MotorGeneralSetting.MotorTable.CardNo.ToString();
            string axisNo = axisBase.MotorGeneralSetting.MotorTable.AxisNo.ToString();
            retInfo = $"Name:{name}     CardNo:{cardNo}     AxisNo:{axisNo}";
            return retInfo;
        }
        MotionActionV2 _motorActionForAllAxesHoming = new MotionActionV2();
        private void btn_homeAllAxes_Click(object sender, EventArgs e)
        {
            if (SelectedAxes?.Count <= 0)
            {
                MessageBox.Show($"轴组内不包含任何轴!", "顺序复位错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var axis in SelectedAxes)
            {
                if (axis.Interation.IsSimulation == false)
                {
                    if (axis.Interation.IsMoving)
                    {
                        MessageBox.Show($"轴[{axis.Name}]正在移动!", "顺序复位错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            if (MessageBox.Show($"按顺序复位所有轴?", "顺序复位确认", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (MessageBox.Show($"确认轴组顺序复位将在没有运动干涉情况下进行?", "顺序复位确认二次确认", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _motorActionForAllAxesHoming.Reset();
                    Task.Run(() =>
                    {
                        foreach (var axis in SelectedAxes)
                        {
                            try
                            {
                                var isSucceeded = this._motorActionForAllAxesHoming.SingleAxisHome(axis);
                                if (isSucceeded == false)
                                {
                                    if (_motorActionForAllAxesHoming.TokenIsCancellationRequested())
                                    {
                                        MessageBox.Show($"轴回零取消!");
                                        break;
                                    }
                                    else
                                    {
                                        MessageBox.Show($"轴回零错误!");
                                        break;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show($"轴回零完成!");
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"轴回零错误:[{ex.Message}-{ex.StackTrace}]!");
                            }
                        }
                    });
                }
            }
        }

        private void btn_stophomingAllAxes_Click(object sender, EventArgs e)
        {
            _motorActionForAllAxesHoming.Cancel();
        }
    }
}