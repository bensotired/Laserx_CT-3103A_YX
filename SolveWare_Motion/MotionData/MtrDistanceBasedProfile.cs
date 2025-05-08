using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SolveWare_Business_Motion.Base
{
    public enum MotionProfileType
    {
        Trapezoida,
        SCurve
    }

    public class MtrDistanceBasedProfile : IMotionProfile, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion                  

        [XmlIgnore]
        public bool IsEnableDistanceProf
        {
            get
            {
                return false;
            }
            set
            {
            }
        }
        [XmlIgnore]
        public bool IsBasedProf
        {
            get
            {
                return false;
            }
        }
        [XmlIgnore]
        public bool IsDistanceProf
        {
            get
            {
                return true;
            }
        }
        [XmlIgnore]
        public int SerialNo
        {
            get
            {
                return this.serialNo;
            }
            set
            {
                this.serialNo = value;
                this.OnPropertyChanged("SerialNo");
            }
        }
        [XmlIgnore]
        public string MtrUnit
        {
            get
            {
                return this.mtrUnit;
            }
            set
            {
                this.mtrUnit = value;
                this.OnPropertyChanged("MtrUnit");
                this.NotifyInfoChanged();
            }
        }
        [XmlIgnore]
        public string AxisName
        {
            get
            {
                return this.axisName;
            }
            set
            {
                this.axisName = value;
                this.OnPropertyChanged("AxisName");
            }
        }
        [XmlIgnore]
        public string FullName
        {
            get
            {
                return string.Concat(new object[]
                {
                    this.axisName,
                    " ",
                    this.basedProfName,
                    " ",
                    this.name,
                    "-",
                    this.serialNo
                });
            }
        }
        [XmlIgnore]
        public string Info
        {
            get
            {
                string text = "[{5}] {0} ";
                text = text + ", D <= {7} " + this.mtrUnit;
                text += (this.isRequiredStartVelocity ? "{1}, {2}, {3}, {4}" : " {2}, {3}, {4}");
                text += (this.isRequiredJerk ? ", {6}" : "");
                return string.Format(text, new object[]
                {
                    this.name,
                    this._startVel.ToString("F2"),
                    this._maxVel.ToString("F2"),
                    this._acc.ToString("F2"),
                    this._dec.ToString("F2"),
                    this.serialNo.ToString().PadLeft(2, '0'),
                    this._jerk.ToString("F2"),
                    this._distance.ToString("F4")
                });
            }
        }
        [XmlIgnore]
        public string InfoDetail
        {
            get
            {
                string text = "[{5}] {0} ";
                text = text + ", Dis =< {7} " + this.mtrUnit;
                text += (this.isRequiredStartVelocity ? "StartVel={1}, MaxVel={2}, Acc={3}, Dec={4}" : " Vel={2}, Acc={3}, Dec={4}");
                text += (this.isRequiredJerk ? ", Jrk={6}" : "");
                return string.Format(text, new object[]
                {
                    this.name,
                    this._startVel.ToString("F2"),
                    this._maxVel.ToString("F2"),
                    this._acc.ToString("F2"),
                    this._dec.ToString("F2"),
                    this.serialNo.ToString().PadLeft(2, '0'),
                    this._jerk.ToString("F2"),
                    this._distance.ToString("F4")
                });
            }
        }

        public MotionProfileType MotionType
        {
            get
            {
                return this.motionType;
            }
            set
            {
                this.motionType = value;
                this.OnPropertyChanged("MotionType");
            }
        }
        public string AxisID
        {
            get
            {
                return this.axisID;
            }
            set
            {
                this.axisID = value;
                this.OnPropertyChanged("AxisID");
                this.NotifyInfoChanged();
            }
        }
        public string BasedProfName
        {
            get
            {
                return this.basedProfName;
            }
            set
            {
                this.basedProfName = value;
                this.OnPropertyChanged("BasedProfName");
                this.NotifyInfoChanged();
            }
        }
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                this.OnPropertyChanged("Name");
                this.NotifyInfoChanged();
            }
        }
        public string BasedID
        {
            get
            {
                return this.basedID;
            }
            set
            {
                this.basedID = value;
            }
        }
        public override string ToString()
        {
            return this.Info;
        }

        [Description("Distance Up To")]
        [DisplayName("Distance")]
        [Category("Profile")]
        public float Distance
        {
            get
            {
                return this._distance;
            }
            set
            {
                this._distance = value;
                this.OnPropertyChanged("Distance");
                this.NotifyInfoChanged();
            }
        }

        [Category("Profile")]
        [Description("Start Speed/Velocity")]
        [DisplayName(".StartVel")]
        public float StartVel
        {
            get
            {
                return this._startVel;
            }
            set
            {
                this._startVel = value;
                this.OnPropertyChanged("StartVel");
                this.NotifyInfoChanged();
            }
        }

        [Category("Profile")]
        [Description("Maximum Speed/Velocity")]
        public float MaxVel
        {
            get
            {
                return this._maxVel;
            }
            set
            {
                this._maxVel = value;
                this.OnPropertyChanged("MaxVel");
                this.NotifyInfoChanged();
            }
        }

        [Category("Profile")]
        [Description("Acceralation")]
        public double Acc
        {
            get
            {
                return this._acc;
            }
            set
            {
                this._acc = value;
                this.OnPropertyChanged("Acc");
                this.NotifyInfoChanged();
            }
        }

        [Description("Deceralation")]
        [Category("Profile")]
        public double Jerk
        {
            get
            {
                return this._jerk;
            }
            set
            {
                this._jerk = value;
                this.OnPropertyChanged("Jerk");
                this.NotifyInfoChanged();
            }
        }

        [Description("Deceralation")]
        [Category("Profile")]
        public double Dec
        {
            get
            {
                return this._dec;
            }
            set
            {
                this._dec = value;
                this.OnPropertyChanged("Dec");
                this.NotifyInfoChanged();
            }
        }

        private void NotifyInfoChanged()
        {
            this.OnPropertyChanged("Info");
            this.OnPropertyChanged("InfoDetail");
        }
        public void CopyValueFrom(IMotionProfile pf)
        {
            this._startVel = pf.StartVel;
            this._maxVel = pf.MaxVel;
            this._acc = pf.Acc;
            this._dec = pf.Dec;
            this._jerk = pf.Jerk;
            this._distance = pf.Distance;
            this.axisID = pf.AxisID;
            this.NotifyInfoChanged();
        }
        public bool IsRequiredJerk
        {
            get
            {
                return this.isRequiredJerk;
            }
            set
            {
                this.isRequiredJerk = value;
                this.OnPropertyChanged("IsRequiredJerk");
                this.NotifyInfoChanged();
            }
        }
        public bool IsRequiredStartVelocity
        {
            get
            {
                return this.isRequiredStartVelocity;
            }
            set
            {
                this.isRequiredStartVelocity = value;
                this.OnPropertyChanged("IsRequiredStartVelocity");
                this.NotifyInfoChanged();
            }
        }


        private MotionProfileType motionType = MotionProfileType.Trapezoida;
        private int serialNo = 0;
        private string axisID = "";
        private string basedProfName = "";
        private string mtrUnit = "";
        private string name = "";
        private string basedID = "";
        private string axisName = "";
        private bool isRequiredJerk = false;
        private bool isRequiredStartVelocity = true;
        protected float _distance = 0f;
        protected float _startVel = 10f;
        protected float _maxVel = 100f;
        protected double _acc = 1.0;
        protected double _jerk = 1.0;
        protected double _dec = 1.0;
    }
}
