using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_Business_Motion.Base
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class MtrMisc
    {
        [Browsable(false)]
        public string AxisName { get; set; }

        [Category("Axis Panel Control")]
        [DisplayName("RightButtonPositive")]
        [Description("True:Right Button Positive, False:Right Button Negevite")]
        public bool RightButtonPositive { get; set; }

        [Description("True:Down Button Positive, False:Down Button Negevite")]
        [Category("Axis Panel Control")]
        [DisplayName("DownButtonPositive")]
        public bool DownButtonPositive { get; set; }

        [Description("Unit name to show")]
        [Category("Axis Panel GUI")]
        [DisplayName("UnitName")]
        public string UnitName
        {
            get
            {
                return this._unitName;
            }
            set
            {
                this._unitName = value;
            }
        }

        [DisplayName("VerticalButtons")]
        [Description("True: Show Vertical Buttons")]
        [Category("Axis Panel GUI")]
        public bool VerticalButtons { get; set; }

        [Description("True: CW-CCW Rotation")]
        [Category("Axis Panel GUI")]
        [DisplayName("Rotation Axis")]
        public bool RotationAxis { get; set; }

        [Description("True: Show Alarm Reset Buttons")]
        [DisplayName("ShowAlarmReset")]
        [Category("Axis Panel GUI")]
        public bool ShowAlarmReset
        {
            get
            {
                return this._showAlarmReset;
            }
            set
            {
                this._showAlarmReset = value;
            }
        }

        [Description("True: Show Brake Buttons")]
        [Category("Axis Panel GUI")]
        [DisplayName("ShowBrake")]
        public bool ShowBrake { get; set; }

        [DisplayName("ShowServoOn")]
        [Description("True: Show ServoOn Buttons")]
        [Category("Axis Panel GUI")]
        public bool ShowServoOn { get; set; }

        [Description("True: Hide MotionBtn Buttons")]
        [DisplayName("HideMotionBtn")]
        [Category("Axis Panel GUI")]
        public bool HideMotionBtn { get; set; }

        [Category("Axis Panel GUI")]
        [DisplayName("HideLimitWarning")]
        [Description("True: Hide Limit Waring")]
        public bool HideLimitWarning { get; set; }

        [Description("True: Show HomeBtn Buttons")]
        [DisplayName("ShowHomeBtn")]
        [Category("Axis Panel GUI")]
        public bool ShowHomeBtn
        {
            get
            {
                return this._showHomeBtn;
            }
            set
            {
                this._showHomeBtn = value;
            }
        }

        [DisplayName("ShowContCb")]
        [Category("Axis Panel GUI")]
        [Description("True: Show Cont Checkbox")]
        public bool ShowContCb
        {
            get
            {
                return this._showContCb;
            }
            set
            {
                this._showContCb = value;
            }
        }

        [DisplayName("ShowOriginSignal")]
        [Description("True: Show Origin Signal")]
        [Category("Axis Panel GUI")]
        public bool ShowOriginSignal
        {
            get
            {
                return this._showOriginSignal;
            }
            set
            {
                this._showOriginSignal = value;
            }
        }

        [Category("Axis Panel GUI")]
        [Description("True: Show Alarm Signal")]
        [DisplayName("ShowAlarmSignal")]
        public bool ShowAlarmSignal
        {
            get
            {
                return this._showAlarmSignal;
            }
            set
            {
                this._showAlarmSignal = value;
            }
        }

        [Category("Axis Panel GUI")]
        [DisplayName("ShowInPosSignal")]
        [Description("True: Show InPos Signal")]
        public bool ShowInPosSignal
        {
            get
            {
                return this._showInPosSignal;
            }
            set
            {
                this._showInPosSignal = value;
            }
        }

        [Description("True: Show Positive Signal")]
        [Category("Axis Panel GUI")]
        [DisplayName("ShowPositiveSignal")]
        public bool ShowPositiveSignal
        {
            get
            {
                return this._showPositivieSignal;
            }
            set
            {
                this._showPositivieSignal = value;
            }
        }

        [DisplayName("ShowNegativeSignal")]
        [Description("True: Show Negative Signal")]
        [Category("Axis Panel GUI")]
        public bool ShowNegativeSignal
        {
            get
            {
                return this._showNegativeSignal;
            }
            set
            {
                this._showNegativeSignal = value;
            }
        }

        [DisplayName("ShowGoto")]
        [Description("True: Show Goto Signal")]
        [Category("Axis Panel GUI")]
        public bool ShowGoto
        {
            get
            {
                return this._showGoto;
            }
            set
            {
                this._showGoto = value;
            }
        }

        [DisplayName("CheckHomeForMove")]
        [Category("Axis Panel Other Settings")]
        [Description("Check axis for Home done before motion")]
        public bool CheckHomeForMove { get; set; }

        [DisplayName("Speed")]
        [Description("Speed in %")]
        [Category("Axis Panel Other Settings")]
        public double Speed
        {
            get
            {
                return this._speed;
            }
            set
            {
                this._speed = value;
            }
        }

        private string _unitName = "mm";
        private bool _showAlarmReset = false;
        private bool _showHomeBtn = true;
        private bool _showContCb = true;
        private bool _showOriginSignal = true;
        private bool _showAlarmSignal = true;
        private bool _showInPosSignal = true;
        private bool _showPositivieSignal = true;
        private bool _showNegativeSignal = true;
        private bool _showGoto = true;
        private double _speed = 50.0;
    }
}
