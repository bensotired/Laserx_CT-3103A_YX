using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace SolveWare_Business_Motion.Base
{
    [TypeConverter(typeof(MtrPosSpeedConverter))]
    [Serializable]
    public class MtrPosSpeed : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(info));
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
            }
        }

        [XmlIgnore]
        public string Category
        {
            get
            {
                return this.category;
            }
            set
            {
                this.category = value;
            }
        }

        [XmlIgnore]
        public string PositionGroup
        {
            get
            {
                return this.positionGroup;
            }
            set
            {
                this.positionGroup = value;
            }
        }

        [XmlIgnore]
        public string ConfiguredFeatureID
        {
            get
            {
                return this.configuredFeatureID;
            }
            set
            {
                this.configuredFeatureID = value;
            }
        }

        public string CustomProfileName
        {
            get
            {
                return this.customProfileName;
            }
            set
            {
                this.customProfileName = value;
                this.OnPropertyChanged("CustomProfileName");
            }
        }

        public bool UseCustomProfile
        {
            get
            {
                return this.useCustomProfile;
            }
            set
            {
                this.useCustomProfile = value;
                this.OnPropertyChanged("UseCustomProfile");
            }
        }

        private void customAssignedProfile_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "InfoDetail")
            {
                this.NotifyInfoChanged();
            }
        }

        private void SetDevalutVal()
        {
            this._startVel = 10;
            this._maxVel = 100;
            this._pos = 0.0;
        }

        public MtrPosSpeed()
        {
            this.SetDevalutVal();
        }

        public MtrPosSpeed(string name)
        {
            this.name = name;
            this.SetDevalutVal();
        }

        public MtrPosSpeed(string name, string group, string category, string configuredAccess)
        {
            this.name = name;
            this.positionGroup = group;
            this.category = category;
            this.configuredFeatureID = configuredAccess;
            this.SetDevalutVal();
        }

        public void CopyFrom(MtrPosSpeed posSp)
        {
            this._startVel = posSp._startVel;
            this._maxVel = posSp._maxVel;
            this._pos = posSp.Pos;
            this.name = posSp.name;
            this.customProfileName = posSp.customProfileName;
            this.useCustomProfile = posSp.useCustomProfile;
            this.axisID = posSp.axisID;
        }

        public MtrPosSpeed(MtrPosSpeed posSp)
        {
            this.CopyFrom(posSp);
        }

        [DisplayName(".StartVel")]
        [Category("Pos Speed")]
        [Description("Start Speed/Velocity")]
        public int StartVel
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

        [Category("Pos Speed")]
        [Description("Maximum Speed/Velocity")]
        public int MaxVel
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

        [Category("Pos Speed")]
        [Description("Position in mm/degree")]
        [DisplayName(".Pos")]
        public double Pos
        {
            get
            {
                return this._pos;
            }
            set
            {
                this._pos = value;
                this.OnPropertyChanged("Pos");
                this.NotifyInfoChanged();
            }
        }

        private void NotifyInfoChanged()
        {
            this.OnPropertyChanged("Info");
            this.OnPropertyChanged("InfoDetail");
            this.OnPropertyChanged("Info2");
            this.OnPropertyChanged("InfoDetail2");
        }

        [XmlIgnore]
        public string Info
        {
            get
            {
                return string.Format("{0}, {1}, {2}", this._pos.ToString("F4"), this._startVel, this._maxVel);
            }
        }

        [XmlIgnore]
        public string Info2
        {
            get
            {
                return string.Format("{0}", this._pos.ToString("F4"), this.GetProfileInfo);
            }
        }

        [XmlIgnore]
        public string InfoDetail
        {
            get
            {
                return string.Format("Pos={0}, StartVel={1}, MaxVel={2}", this._pos.ToString("F4"), this._startVel, this._maxVel);
            }
        }

        [XmlIgnore]
        public string InfoDetail2
        {
            get
            {
                return string.Format("Pos={0} {1}", this._pos.ToString("F4"), this.GetProfileInfo);
            }
        }

        private string GetProfileInfo
        {
            get
            {
                string text = (this.customProfileName == null) ? "" : this.customProfileName;
                if (text != "")
                {
                    text = ", " + text;
                }
                return text;
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(this._pos.ToString("F6"));
            stringBuilder.Append(", ");
            stringBuilder.Append(this._startVel);
            stringBuilder.Append(", ");
            stringBuilder.Append(this._maxVel);
            return stringBuilder.ToString();
        }

        private string name = "";
        private string axisID = "";
        private string category = "";
        private string positionGroup = "";
        private string configuredFeatureID = "";
        private string customProfileName = "";
        private bool useCustomProfile = false;
        protected int _startVel = 10;
        protected int _maxVel = 100;
        protected double _pos;
    }
}
