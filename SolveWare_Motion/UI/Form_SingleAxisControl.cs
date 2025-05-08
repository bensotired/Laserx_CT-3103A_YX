using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_Motion
{
    public partial class Form_SingleAxisControl : Form
    {
        Color YES_COLOR = Color.LimeGreen;
        Color NO_COLOR = Color.DarkGreen;
        Color EMPTY_COLOR = Color.DimGray;
        const string STA_SIM = "使能状态:模拟";
        const string STA_SERVO_ON = "使能状态:ON";
        const string STA_SERVO_OFF = "使能状态:OFF";
        const string STA_EMPTY = "使能状态:未加载任何轴";
        const string EMPTY_POS = "0.0";
        Task UpdateInfoTask = null;
        MotorAxisBase _motor = null;
        //MotionAction _motorAction = new MotionAction();
        MotionActionV2 _motorAction_V2 = new MotionActionV2();
        CancellationTokenSource _updateTaskTokenSource = new CancellationTokenSource();
        Dictionary<Control, string> _sizeDict = new Dictionary<Control, string>();
        int _orgFormWidth;//获取窗体的宽度
        int _orgFormHeight;//获取窗体的宽度

        public Form_SingleAxisControl()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void Form_SingleAxisControl_Load(object sender, EventArgs e)
        {
            this._orgFormWidth = this.Width;
            this._orgFormHeight = this.Height;
            this.CreateSizeDict(this, ref _sizeDict);
            UpdateInfoTask = Task.Factory.StartNew(() => UpdateRun(ref this._updateTaskTokenSource), TaskCreationOptions.LongRunning);
        }
        void UpdateRun(ref CancellationTokenSource tokenSource)
        {
            do
            {
                try
                {
                    Thread.Sleep(100);
                    if (this._motor == null)
                    {
                        this.Invoke((EventHandler)delegate
                        {
                            this.lbl_servoStatus.Text = STA_EMPTY;
                            this.tb_currentPulse.Text = EMPTY_POS;
                            this.tb_currentAbsPos.Text = EMPTY_POS;
                            this.btn_PosLimtSignal.BackColor = EMPTY_COLOR;
                            this.btn_NegLimtSignal.BackColor = EMPTY_COLOR;
                            this.btn_OrgSignal.BackColor = EMPTY_COLOR;
                            this.chk_AxisHomed.Checked = true;
                        });
                        continue;
                    }
                    else
                    {
                        this.Invoke((EventHandler)delegate
                        {

                            if (this._motor.Interation.IsSimulation)
                            {
                                this.lbl_servoStatus.Text = STA_SIM;

                            }
                            else
                            {
                                this.lbl_servoStatus.Text = this._motor.Interation.IsServoOn ? STA_SERVO_ON : STA_SERVO_OFF;
                            }
                            this.tb_currentPulse.Text = this._motor.Interation.CurrentPulse.ToString();
                            this.tb_currentAbsPos.Text = this._motor.Interation.CurrentPosition.ToString();
                            this.btn_PosLimtSignal.BackColor = this._motor.Interation.IsPosLimit ? YES_COLOR : NO_COLOR;
                            this.btn_NegLimtSignal.BackColor = this._motor.Interation.IsNegLimit ? YES_COLOR : NO_COLOR;
                            this.btn_OrgSignal.BackColor = this._motor.Interation.IsOrg ? YES_COLOR : NO_COLOR;
                            this.chk_AxisHomed.Checked = _motor.Interation.HasHome;
                        });
                    }
                }
                catch (Exception ex)
                {

                }
            } while (true);
        }
        public void AssignAxis(MotorAxisBase motor)
        {
            if (this._motor != null)
            {
                lock (this._motor)
                {
                    if (this._motor.Interation.IsMoving)
                    {
                        this._motor.Stop();
                        this._motor.WaitMotionDone();
                        MessageBox.Show($"自动停止当前马达[{this._motor.Name}]动作!");
                    }
                }
                _motor = motor;
            }
            else
            {
                _motor = motor;
            }
        }
        public void UninstallAxis()
        {

            if (this._motor != null)
            {
                lock (this._motor)
                {
                    if (this._motor.Interation.IsMoving)
                    {
                        this._motor.Stop();
                        this._motor.WaitMotionDone();
                        MessageBox.Show($"自动停止当前马达[{this._motor.Name}]动作!");
                    }
                }
                _motor = null;
            }
        }
        private void btn_PosRelMove_Click(object sender, EventArgs e)
        {

            if (this._motor == null ||
            this._motor.Interation.IsMoving == true)
            {
                return;
            }
            if (this._motor.Interation.IsSimulation == true)
            {


            }
            else
            {
                Task.Run(() =>
                {
                    try
                    {
                        //如果停止需要重新reset一下 用cancel来进行停止 执行内部代码
                        this._motorAction_V2.Reset();
                        int errorCode = ErrorCodes.NoError;
                        double relativePos = Convert.ToDouble(this.tb_planRelPos.Text);
                        //if (!this._Motor.HasHome) throw new Exception("Motor Has Not Homed");

                        double curPos = this._motor.Interation.CurrentPosition;
                        double targetPos = curPos + relativePos;

                        errorCode = this._motorAction_V2.SingleAxisMotion(this._motor, targetPos );

                        //if (errorCode != ErrorCodes.NoError) throw new Exception($"Motor Moved Failed");
                        if (errorCode != ErrorCodes.NoError)
                            MessageBox.Show($"轴运行错误:[{ErrorCodes.GetErrorDescription(errorCode)}]!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"轴运行错误:[{ex.Message}-{ex.StackTrace}]!");
                    }
                });
            }
        }

        private void btn_NegRelMove_Click(object sender, EventArgs e)
        {
            if (this._motor == null ||
          this._motor.Interation.IsMoving == true)
            {
                return;
            }
            if (this._motor.Interation.IsSimulation == true)
            {


            }
            else
            {
                Task.Run(() =>
                {
                    try
                    {
                        //如果停止需要重新reset一下 用cancel来进行停止 执行内部代码
                        this._motorAction_V2.Reset();
                        int errorCode = ErrorCodes.NoError;
                        double relativePos = Convert.ToDouble(this.tb_planRelPos.Text);
                        //if (!this._Motor.HasHome) throw new Exception("Motor Has Not Homed");

                        double curPos = this._motor.Interation.CurrentPosition;
                        double targetPos = curPos - relativePos;

                        errorCode = this._motorAction_V2.SingleAxisMotion (this._motor, targetPos);

                        //if (errorCode != ErrorCodes.NoError) throw new Exception($"Motor Moved Failed");
                        if (errorCode != ErrorCodes.NoError)
                            MessageBox.Show($"轴运行错误:[{ErrorCodes.GetErrorDescription(errorCode)}]!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"轴运行错误:[{ex.Message}-{ex.StackTrace}]!");
                    }
                });
            }
        }

        private void btn_AbsMove_Click(object sender, EventArgs e)
        {
            if (this._motor == null ||
           this._motor.Interation.IsMoving == true)
            {
                return;
            }
            if (this._motor.Interation.IsSimulation == true)
            {


            }
            else
            {
                Task.Run(() =>
                           {

                               try
                               {
                                   //如果停止需要重新reset一下 用cancel来进行停止 执行内部代码
                                   this._motorAction_V2.Reset();

                                   int errorCode = ErrorCodes.NoError;
                                   double absolutePos = Convert.ToDouble(this.tb_planAbsPos.Text);



                                   double targetPos = absolutePos;
                                   string sErr = string.Empty;
                                   errorCode = this._motorAction_V2.SingleAxisMotion (this._motor, targetPos);

                                   if (errorCode != ErrorCodes.NoError)
                                       MessageBox.Show($"轴运行错误:[{ErrorCodes.GetErrorDescription(errorCode)}]!");

                               }
                               catch (Exception ex)
                               {
                                   MessageBox.Show($"轴运行错误:[{ex.Message}-{ex.StackTrace}]!");
                               }
                           });
            }
        }
        private void btn_PosJogMove_MouseDown(object sender, MouseEventArgs e)
        {
            if (this._motor == null ||
               this._motor.Interation.IsMoving == true)
            {
                return;
            }
            if (this._motor.Interation.IsSimulation == true)
            {
                //模拟运动
            }
            else
            {
                this._motor.Jog(true);
            }
        }

        private void btn_NegJogMove_MouseDown(object sender, MouseEventArgs e)
        {
            if (this._motor == null ||
                this._motor.Interation.IsMoving == true)
            {
                return;
            }
            if (this._motor.Interation.IsSimulation == true)
            {
                //模拟运动
            }
            else
            {
                this._motor.Jog(false);
            }
        }

        private void btn_PosJogMove_MouseUp(object sender, MouseEventArgs e)
        {
            if (this._motor == null)
            {
                return;
            }
            if (this._motor.Interation.IsSimulation == true)
            {
                //模拟运动
            }
            else
            {
                this._motor.Stop();
            }
        }

        private void btn_NegJogMove_MouseUp(object sender, MouseEventArgs e)
        {
            if (this._motor == null)
            {
                return;
            }
            if (this._motor.Interation.IsSimulation == true)
            {
                //模拟运动
            }
            else
            {
                this._motor.Stop();
            }
        }

        private void btn_HomeMove_Click(object sender, EventArgs e)
        {
            if (this._motor == null ||
                this._motor.Interation.IsMoving == true)
            {
                return;
            }
            if (this._motor.Interation.IsSimulation == true)
            {
            }
            else
            {
                Task.Run(() =>
                {

                    try
                    {
                        //如果停止需要重新reset一下 用cancel来进行停止 执行内部代码
                        this._motorAction_V2.Reset();
                        int errorCode = ErrorCodes.NoError;


                        var isSucceeded = this._motorAction_V2.SingleAxisHome(this._motor);//.MotorHomeAndWait(this._motor);
                        //errorCode = this._motorAction.MotorHomeAndWait(this._motor);
                        if (isSucceeded == false)
                        {
                            errorCode = ErrorCodes.MotorHomingError;
                        }
                        else
                        {
                            //20231218 关闭提示,烦死了
                            //MessageBox.Show($"轴回零完成!");
                        }
                        if (errorCode != ErrorCodes.NoError)
                            MessageBox.Show($"轴回零错误:[{ErrorCodes.GetErrorDescription(errorCode)}]!");

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"轴回零错误:[{ex.Message}-{ex.StackTrace}]!");
                    }
                });
            }
        }

        private void btn_servoOn_Click(object sender, EventArgs e)
        {
            if (this._motor == null)
            {
                MessageBox.Show($"未加载任何轴!"); return;
            }
            if (this._motor.Interation.IsMoving == true)
            {
                MessageBox.Show($"当前轴正在运动!");
                return;
            }
            if (this._motor.Interation.IsSimulation == true)
            {
                MessageBox.Show($"当前轴正在模拟模式!");
                return;
            }
            else
            {
                this._motor.Set_Servo(true);
                if (this._motor.MotorGeneralSetting.MotorTable.IsPhaseSearchNeeded)
                {
                    Thread.Sleep(1000);
                    MessageBox.Show($"当前轴正在寻相!");
                    if (this._motor.PhaseSearching(60) == true)
                    {
                        MessageBox.Show($"当前轴寻相完成!");

                    }
                    else
                    {
                        MessageBox.Show($"当前轴寻相失败!");
                    }
                }
            }
        }
        private void btn_servoOff_Click(object sender, EventArgs e)
        {
            if (this._motor == null)
            {
                MessageBox.Show($"未加载任何轴!"); return;
            }
            if (this._motor.Interation.IsMoving == true)
            {
                MessageBox.Show($"当轴正在运动!");
                return;
            }
            if (this._motor.Interation.IsSimulation == true)
            {
                MessageBox.Show($"当轴正在模拟模式!");
                return;
            }
            else
            {
                this._motor.Set_Servo(false);
            }
        }

        private void btn_servoSim_Click(object sender, EventArgs e)
        {

        }

        private void btn_AxisStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._motor == null)
                {
                    MessageBox.Show($"未加载任何轴!"); return;
                }

                if (this._motor.Interation.IsSimulation == true)
                {
                    MessageBox.Show($"当轴正在模拟模式!");
                    return;
                }
                else
                {
                    this._motorAction_V2.Cancel();
                    //this._motor.Stop();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"轴停止错误:[{ex.Message}-{ex.StackTrace}]!");
            }
        }


        private void CreateSizeDict(Control sourceControl, ref Dictionary<Control, string> SizeDict)
        {
            foreach (Control ctrl in sourceControl.Controls)//循环窗体中的控件
            {
                SizeDict.Add(ctrl, $"{ctrl.Width}:{ ctrl.Height}:{ctrl.Left}:{ ctrl.Top}:{ ctrl.Font.Size}");

                if (ctrl.Controls.Count > 0)
                    CreateSizeDict(ctrl, ref SizeDict);
            }
        }
        private void ResizeControls(float newx, float newy)
        {
            return;
            try
            {
                //遍历窗体中的控件，重新设置控件的值
                foreach (var ctrlKvp in _sizeDict)
                {
                    var ctrl = ctrlKvp.Key;
                    string[] sizeStrips = ctrlKvp.Value.Split(new char[] { ':' });//获取控件的Tag属性值，并分割后存储字符串数组
                    float newSizeValue = Convert.ToSingle(sizeStrips[0]) * newx;//根据窗体缩放比例确定控件的值，宽度
                    ctrl.Width = (int)newSizeValue;//宽度
                    newSizeValue = Convert.ToSingle(sizeStrips[1]) * newy;//高度
                    ctrl.Height = (int)(newSizeValue);
                    newSizeValue = Convert.ToSingle(sizeStrips[2]) * newx;//左边距离
                    ctrl.Left = (int)(newSizeValue);
                    newSizeValue = Convert.ToSingle(sizeStrips[3]) * newy;//上边缘距离
                    ctrl.Top = (int)(newSizeValue);
                    float currentSize = Convert.ToSingle(sizeStrips[4]) * newy;//字体大小
                    ctrl.Font = new Font(ctrl.Font.Name, currentSize, ctrl.Font.Style, ctrl.Font.Unit);

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Form_SingleAxisControl_Resize(object sender, EventArgs e)
        {
            float newx = Convert.ToSingle(this.Width) / Convert.ToSingle(this._orgFormWidth); //窗体宽度缩放比例
            float newy = Convert.ToSingle(this.Height) / Convert.ToSingle(this._orgFormHeight);//窗体高度缩放比例

            ResizeControls(newx, newy);
        }

        private void btn_moveRelMicroStep_positive_Click(object sender, EventArgs e)
        {
            if (this._motor == null ||
            this._motor.Interation.IsMoving == true)
            {
                return;
            }
            if (this._motor.Interation.IsSimulation == true)
            {


            }
            else
            {
                Task.Run(() =>
                {
                    try
                    {
                        double relativePos = 0.0;

                        if(rb_stp_0_0001.Checked)
                        {
                            relativePos = 0.0001;
                        }
                        if (rb_stp_0_001.Checked)
                        {
                            relativePos = 0.001;
                        }
                        else if (rb_stp_0_01.Checked)
                        {
                            relativePos = 0.01;
                        }
                        else if (rb_stp_0_1.Checked)
                        {
                            relativePos = 0.1;
                        }
                        else if (rb_stp_1.Checked)
                        {
                            relativePos = 1;
                        }

                        //如果停止需要重新reset一下 用cancel来进行停止 执行内部代码
                        this._motorAction_V2.Reset();

                        int errorCode = ErrorCodes.NoError;
                        //double relativePos = Convert.ToDouble(this.tb_planRelPos.Text);
                        //if (!this._Motor.HasHome) throw new Exception("Motor Has Not Homed");

                        double curPos = this._motor.Interation.CurrentPosition;
                        double targetPos = curPos + relativePos;

                        errorCode = this._motorAction_V2.SingleAxisMotion (this._motor, targetPos);

                        //if (errorCode != ErrorCodes.NoError) throw new Exception($"Motor Moved Failed");
                        if (errorCode != ErrorCodes.NoError)
                            MessageBox.Show($"轴运行错误:[{ErrorCodes.GetErrorDescription(errorCode)}]!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"轴运行错误:[{ex.Message}-{ex.StackTrace}]!");
                    }
                });
            }
        }

        private void btn_moveRelMicroStep_negative_Click(object sender, EventArgs e)
        {
            if (this._motor == null ||
      this._motor.Interation.IsMoving == true)
            {
                return;
            }
            if (this._motor.Interation.IsSimulation == true)
            {


            }
            else
            {
                Task.Run(() =>
                {
                    try
                    {
                        double relativePos = 0.0;

                        if (rb_stp_0_0001.Checked)
                        {
                            relativePos = 0.0001;
                        }
                        if (rb_stp_0_001.Checked)
                        {
                            relativePos = 0.001;
                        }
                        else if (rb_stp_0_01.Checked)
                        {
                            relativePos = 0.01;
                        }
                        else if (rb_stp_0_1.Checked)
                        {
                            relativePos = 0.1;
                        }
                        else if (rb_stp_1.Checked)
                        {
                            relativePos = 1;
                        }
                        //如果停止需要重新reset一下 用cancel来进行停止 执行内部代码
                        this._motorAction_V2.Reset();

                        int errorCode = ErrorCodes.NoError;
                        //double relativePos = Convert.ToDouble(this.tb_planRelPos.Text);
                        //if (!this._Motor.HasHome) throw new Exception("Motor Has Not Homed");

                        double curPos = this._motor.Interation.CurrentPosition;
                        double targetPos = curPos - relativePos;

                        errorCode = this._motorAction_V2.SingleAxisMotion (this._motor, targetPos);

                        //if (errorCode != ErrorCodes.NoError) throw new Exception($"Motor Moved Failed");
                        if (errorCode != ErrorCodes.NoError)
                            MessageBox.Show($"轴运行错误:[{ErrorCodes.GetErrorDescription(errorCode)}]!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"轴运行错误:[{ex.Message}-{ex.StackTrace}]!");
                    }
                });
            }
        }
    }
}