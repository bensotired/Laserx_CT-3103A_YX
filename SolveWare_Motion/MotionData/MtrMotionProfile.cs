using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SolveWare_Business_Motion.Base
{
    [Serializable]
    public class MtrMotionProfile : IMotionProfile, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(info));
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

        [XmlIgnore]
        public bool IsBasedProf
        {
            get
            {
                return true;
            }
        }

        [XmlIgnore]
        public bool IsDistanceProf
        {
            get
            {
                return false;
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

        [XmlIgnore]
        public string ID
        {
            get
            {
                return this.axisID + this.name;
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
                return this.axisName + " " + this.name;
            }
        }

        [XmlIgnore]
        public float Distance
        {
            get
            {
                return 0f;
            }
            set
            {
            }
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
                this.UpdateJertAndStartVelForDisProf();
                this.NotifyInfoChanged();
            }
        }

        public IMotionProfile GetDistanceProf(double dist)
        {
            IMotionProfile result;
            if (!this.isEnableDistanceProf)
            {
                result = null;
            }
            else if (this.disProfileList == null || this.disProfileList.Count <= 0)
            {
                result = null;
            }
            else
            {
                for (int i = 0; i < this.disProfileList.Count; i++)
                {
                    if (Math.Abs(dist) <= (double)Math.Abs(this.disProfileList[i].Distance))
                    {
                        return this.disProfileList[i];
                    }
                }
                result = null;
            }
            return result;
        }

        private void UpdateJertAndStartVelForDisProf()
        {
            if (this.disProfileList != null && this.disProfileList.Count > 0)
            {
                for (int i = 0; i < this.disProfileList.Count; i++)
                {
                    this.disProfileList[i].IsRequiredJerk = this.isRequiredJerk;
                    this.disProfileList[i].IsRequiredStartVelocity = this.isRequiredStartVelocity;
                }
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
                this.UpdateJertAndStartVelForDisProf();
                this.NotifyInfoChanged();
            }
        }

        public bool IsEnableDistanceProf
        {
            get
            {
                return this.isEnableDistanceProf;
            }
            set
            {
                this.isEnableDistanceProf = value;
                this.OnPropertyChanged("IsEnableDistanceProf");
                this.NotifyInfoChanged();
            }
        }

        public bool IsUseDefaultAccDec
        {
            get
            {
                return this.isUseDefaultAccDec;
            }
            set
            {
                this.isUseDefaultAccDec = value;
                this.OnPropertyChanged("IsUseDefaultAccDec");
            }
        }

        [XmlIgnore]
        public bool IsMotionTypeChangable
        {
            get
            {
                return this.isMotionTypeChangable;
            }
            set
            {
                this.isMotionTypeChangable = value;
                this.OnPropertyChanged("IsMotionTypeChangable");
            }
        }

        public ObservableCollection<MtrDistanceBasedProfile> DisProfileList
        {
            get
            {
                return this.disProfileList;
            }
            set
            {
                this.disProfileList = value;
            }
        }

        public List<IMotionProfile> GetAllProfileList()
        {
            List<IMotionProfile> list = new List<IMotionProfile>();
            list.Add(this);
            for (int i = 0; i < this.disProfileList.Count; i++)
            {
                list.Add(this.disProfileList[i]);
            }
            return list;
        }

        [XmlIgnore]
        public MtrDistanceBasedProfile SelectedDistanceProf
        {
            get
            {
                return this.selectedDistanceProf;
            }
            set
            {
                this.selectedDistanceProf = value;
                this.OnPropertyChanged("SelectedDistanceProf");
            }
        }

        public void RegisterNewProf(MtrDistanceBasedProfile prof)
        {
            if (this.disProfileList != null)
            {
                if (this.disProfileList.Count >= 10)
                {
                    throw new Exception("Not Allow to add anymore");
                }
                this.disProfileList.Add(prof);
                prof.SerialNo = this.disProfileList.Count + 1;
                prof.Name = "Dis-Prof";
                prof.BasedProfName = this.name;
                prof.AxisName = this.AxisName;
                prof.AxisID = this.axisID;
                prof.IsRequiredJerk = this.isRequiredJerk;
                prof.IsRequiredStartVelocity = this.isRequiredStartVelocity;
                this.SortDistanceBasedProf();
                this.SelectedDistanceProf = prof;
                this.NotifyInfoChanged();
            }
        }

        public void RemoveSelected()
        {
            if (this.selectedDistanceProf != null)
            {
                if (this.disProfileList != null)
                {
                    if (!this.disProfileList.Contains(this.selectedDistanceProf))
                    {
                        this.SelectedDistanceProf = null;
                    }
                    else
                    {
                        this.disProfileList.Remove(this.selectedDistanceProf);
                        this.SelectedDistanceProf = ((this.disProfileList.Count > 0) ? this.disProfileList.First<MtrDistanceBasedProfile>() : null);
                        this.NotifyInfoChanged();
                    }
                }
            }
        }

        public void RemoveAll()
        {
            if (this.disProfileList != null)
            {
                this.disProfileList.Clear();
                this.SelectedDistanceProf = null;
                this.NotifyInfoChanged();
            }
        }

        public void SortDistanceBasedProf()
        {
            int index = 0;
            for (int i = 0; i < this.disProfileList.Count; i++)
            {
                MtrDistanceBasedProfile mtrDistanceBasedProfile = this.disProfileList[i];
                MtrDistanceBasedProfile mtrDistanceBasedProfile2 = this.disProfileList[i];
                if (mtrDistanceBasedProfile.Distance > 0f)
                {
                    for (int j = i + 1; j < this.disProfileList.Count; j++)
                    {
                        if (this.disProfileList[j].Distance < mtrDistanceBasedProfile2.Distance)
                        {
                            mtrDistanceBasedProfile2 = this.disProfileList[j];
                            index = j;
                        }
                    }
                    if (mtrDistanceBasedProfile != mtrDistanceBasedProfile2)
                    {
                        this.disProfileList[i] = mtrDistanceBasedProfile2;
                        this.disProfileList[index] = mtrDistanceBasedProfile;
                    }
                }
            }
            for (int i = 0; i < this.disProfileList.Count; i++)
            {
                this.disProfileList[i].SerialNo = i + 1;
            }
        }

        [DisplayName(".StartVel")]
        [Category("Profile")]
        [Description("Start Speed/Velocity")]
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

        [Description("Acceralation")]
        [Category("Profile")]
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

        [Category("Profile")]
        [Description("Deceralation")]
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
            this.isRequiredJerk = pf.IsRequiredJerk;
            this.isRequiredStartVelocity = pf.IsRequiredStartVelocity;
            if (pf is MtrMotionProfile)
            {
                MtrMotionProfile mtrMotionProfile = pf as MtrMotionProfile;
                this.IsEnableDistanceProf = mtrMotionProfile.IsEnableDistanceProf;
                this.isUseDefaultAccDec = mtrMotionProfile.IsUseDefaultAccDec;
                this.disProfileList.Clear();
                for (int i = 0; i < mtrMotionProfile.disProfileList.Count; i++)
                {
                    this.RegisterNewProf(mtrMotionProfile.disProfileList[i]);
                }
            }
            this.axisID = pf.AxisID;
            this.NotifyInfoChanged();
        }

        [XmlIgnore]
        public string Info
        {
            get
            {
                string text = "[{5}] {0} ";
                text += (this.isRequiredStartVelocity ? "{1}, {2}, {3}, {4}" : " {2}, {3}, {4}");
                text += (this.isRequiredJerk ? ", {6}" : "");
                int num = (this.disProfileList == null) ? 0 : this.disProfileList.Count;
                return string.Format(text, new object[]
                {
                    this.name,
                    this._startVel.ToString("F2"),
                    this._maxVel.ToString("F2"),
                    this._acc.ToString("F2"),
                    this._dec.ToString("F2"),
                    this.serialNo.ToString().PadLeft(2, '0'),
                    this._jerk.ToString("F2")
                });
            }
        }

        public override string ToString()
        {
            return this.Info;
        }

        [XmlIgnore]
        public string InfoDetail
        {
            get
            {
                string text = "[{5}] {0} ";
                text += (this.isRequiredStartVelocity ? "StartVel={1}, MaxVel={2}, Acc={3}, Dec={4}" : " Vel={2}, Acc={3}, Dec={4}");
                text += (this.isRequiredJerk ? ", Jrk={6}" : "");
                text += ((!this.isEnableDistanceProf && (this.disProfileList == null || this.disProfileList.Count < 1)) ? "" : ", [DisPorf={7}]");
                int num = (this.disProfileList == null) ? 0 : this.disProfileList.Count;
                return string.Format(text, new object[]
                {
                    this.name,
                    this._startVel.ToString("F2"),
                    this._maxVel.ToString("F2"),
                    this._acc.ToString("F2"),
                    this._dec.ToString("F2"),
                    this.serialNo.ToString().PadLeft(2, '0'),
                    this._jerk.ToString("F2"),
                    num
                });
            }
        }

        private const int MAX_DISTANCE_RANGE = 10;
        private MotionProfileType motionType = MotionProfileType.Trapezoida;
        private int serialNo = 0;
        private string axisID = "";
        private string mtrUnit = "";
        private string name = "";
        private string axisName = "";
        private bool isRequiredJerk = false;
        private bool isRequiredStartVelocity = true;
        private bool isEnableDistanceProf = false;
        private bool isUseDefaultAccDec = true;
        private bool isMotionTypeChangable = false;
        private ObservableCollection<MtrDistanceBasedProfile> disProfileList = new ObservableCollection<MtrDistanceBasedProfile>();
        private MtrDistanceBasedProfile selectedDistanceProf = null;
        protected float _startVel = 10f;
        protected float _maxVel = 100f;
        protected double _acc = 1.0;
        protected double _jerk = 1.0;
        protected double _dec = 1.0;
    }
}
