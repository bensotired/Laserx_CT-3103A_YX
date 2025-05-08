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
    public class AxisMotionProfile : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public ObservableCollection<MtrMotionProfile> ProfileList
        {
            get
            {
                return this.profileList;
            }
        }

        public void UpdateAxisName(string axName, string axUnit)
        {
            List<IMotionProfile> allCopyableProfileList = this.GetAllCopyableProfileList();
            for (int i = 0; i < allCopyableProfileList.Count; i++)
            {
                allCopyableProfileList[i].AxisName = axName;
                allCopyableProfileList[i].MtrUnit = axUnit;
            }
        }

        public MtrMotionProfile GetBaseProfileByName(string name)
        {
            for (int i = 0; i < this.profileList.Count; i++)
            {
                if (this.profileList[i].Name == name)
                {
                    return this.profileList[i];
                }
            }
            return null;
        }

        public List<IMotionProfile> GetAllCopyableProfileList()
        {
            List<IMotionProfile> list = new List<IMotionProfile>();
            for (int i = 0; i < this.profileList.Count; i++)
            {
                MtrMotionProfile mtrMotionProfile = this.profileList[i];
                List<IMotionProfile> allProfileList = mtrMotionProfile.GetAllProfileList();
                if (allProfileList != null && allProfileList.Count > 0)
                {
                    for (int j = 0; j < allProfileList.Count; j++)
                    {
                        list.Add(allProfileList[j]);
                    }
                }
            }
            return list;
        }

        [XmlIgnore]
        public AxisBase Axis
        {
            get
            {
                return this.axis;
            }
        }

        public string AssignAxisID
        {
            get
            {
                return this.assignAxisID;
            }
            set
            {
                this.assignAxisID = value;
            }
        }

        public string AxisID
        {
            get
            {
                string result;
                if (this.axis == null)
                {
                    result = "";
                }
                else
                {
                    result = this.axis.AxisID;
                }
                return result;
            }
        }

        [XmlIgnore]
        public bool HasAssignAxis
        {
            get
            {
                return this.axis != null;
            }
        }

        public void RegisterProfile(string name)
        {
            if (this.HasProfileWithName(name))
            {
                throw new Exception("Duplicate Name!");
            }
            MtrMotionProfile mtrMotionProfile = new MtrMotionProfile();
            mtrMotionProfile.SerialNo = this.profileList.Count + 1;
            mtrMotionProfile.Name = name;
            mtrMotionProfile.AxisID = this.assignAxisID;
            this.UpdateUnitNameForAxisProfile(mtrMotionProfile);
            this.profileList.Add(mtrMotionProfile);
        }

        public MtrMotionProfile[] GetCustomProfileList()
        {
            List<MtrMotionProfile> list = new List<MtrMotionProfile>();
            for (int i = 0; i < this.profileList.Count; i++)
            {
                if (!AxisMotionProfile.DefaultProfileNames.Contains(this.profileList[i].Name))
                {
                    list.Add(this.profileList[i]);
                }
            }
            return list.ToArray();
        }

        [XmlIgnore]
        public bool IsProfileSelected
        {
            get
            {
                return this.selectedProfile != null;
            }
        }

        [XmlIgnore]
        public IMotionProfile SelectedProfile
        {
            get
            {
                return this.selectedProfile;
            }
            set
            {
                this.selectedProfile = value;
                this.OnPropertyChanged("SelectedProfile");
                this.OnPropertyChanged("IsProfileSelected");
            }
        }

        private MtrMotionProfile HasProfileWithName(string pfName, ref bool found)
        {
            found = false;
            MtrMotionProfile profileByName = this.GetProfileByName(pfName);
            found = (profileByName != null);
            return profileByName;
        }

        private bool HasProfileWithName(string pfName)
        {
            for (int i = 0; i < this.profileList.Count; i++)
            {
                if (this.profileList[i].Name == pfName)
                {
                    return true;
                }
            }
            return false;
        }

        public MtrMotionProfile GetProfileByName(string pfName)
        {
            for (int i = 0; i < this.profileList.Count; i++)
            {
                if (this.profileList[i].Name.ToLower() == pfName.ToLower())
                {
                    return this.profileList[i];
                }
            }
            return null;
        }

        public MtrMotionProfile GetDefaultManualProfile()
        {
            bool flag = false;
            MtrMotionProfile mtrMotionProfile = this.HasProfileWithName("ManualMode", ref flag);
            if (!flag)
            {
                this.RegisterProfile("ManualMode");
            }
            return this.HasProfileWithName("ManualMode", ref flag);
        }

        public MtrMotionProfile GetDefaultAutoProfile()
        {
            bool flag = false;
            MtrMotionProfile mtrMotionProfile = this.HasProfileWithName("AutoMode", ref flag);
            if (!flag)
            {
                this.RegisterProfile("AutoMode");
            }
            return this.HasProfileWithName("AutoMode", ref flag);
        }

        public MtrMotionProfile GetDefaultProfile()
        {
            bool flag = false;
            MtrMotionProfile mtrMotionProfile = this.HasProfileWithName("Default", ref flag);
            if (!flag)
            {
                this.RegisterProfile("Default");
            }
            return this.HasProfileWithName("Default", ref flag);
        }

        public void RegisterNewProfile(MtrMotionProfile pf)
        {
            if (pf != null)
            {
                if (this.HasProfileWithName(pf.Name))
                {
                    throw new Exception("Duplicate Name");
                }
                pf.SerialNo = this.ProfileList.Count + 1;
                pf.AxisID = this.assignAxisID;
                this.profileList.Add(pf);
                this.UpdateUnitNameForAxisProfile(pf);
                this.SelectedProfile = this.ProfileList.Last<MtrMotionProfile>();
            }
        }

        public bool HasDefaultProfile()
        {
            return this.HasProfileWithName("Default");
        }

        public MtrMotionProfile RegisterNewProfile(string name)
        {
            if (this.HasProfileWithName(name))
            {
                throw new Exception("Duplicate Name");
            }
            MtrMotionProfile mtrMotionProfile = new MtrMotionProfile();
            MtrMotionProfile pf = this.GetDefaultProfile();
            mtrMotionProfile.Name = name;
            mtrMotionProfile.CopyValueFrom(pf);
            mtrMotionProfile.SerialNo = this.ProfileList.Count + 1;
            mtrMotionProfile.AxisID = this.assignAxisID;
            this.profileList.Add(mtrMotionProfile);
            this.SelectedProfile = this.ProfileList.Last<MtrMotionProfile>();
            return mtrMotionProfile;
        }

        public void AssignAxis(AxisBase ax, bool resetDefault = true)
        {
            this.axis = ax;
            this.AssignAxisID = this.AxisID;
            if (ax != null && resetDefault)
            {
                this.RegisterDefaultProfiles(ax.MtrSpeed.StrVel, ax.MtrSpeed.MaxVel, ax.MtrSpeed.Tacc, ax.MtrSpeed.Tdec);
            }
            this.UpdateUnitNameForAxisProfile(null);
        }

        private void UpdateUnitNameForAxisProfile(MtrMotionProfile prf = null)
        {
            if (this.axis != null)
            {
                if (this.axis.MtrMisc != null)
                {
                    if (prf != null)
                    {
                        prf.MtrUnit = this.axis.MtrMisc.UnitName;
                    }
                    else
                    {
                        for (int i = 0; i < this.profileList.Count; i++)
                        {
                            this.profileList[i].MtrUnit = this.axis.MtrMisc.UnitName;
                        }
                    }
                }
            }
        }

        public void RegisterDefaultProfiles(double startVel, double maxVel, double defaultAcc, double defaultDcc)
        {
            this.defaultProfile = this.GetDefaultProfile();
            this.defaultProfile.StartVel = (float)startVel;
            this.defaultProfile.MaxVel = (float)maxVel;
            this.defaultProfile.Acc = defaultAcc;
            this.defaultProfile.Dec = defaultDcc;
            this.autoModeProfile = this.GetDefaultAutoProfile();
            this.autoModeProfile.StartVel = (float)startVel;
            this.autoModeProfile.MaxVel = (float)maxVel;
            this.autoModeProfile.Acc = defaultAcc;
            this.autoModeProfile.Dec = defaultDcc;
            this.manualModeProfile = this.GetDefaultManualProfile();
            this.manualModeProfile.StartVel = (float)startVel;
            this.manualModeProfile.MaxVel = (float)maxVel;
            this.manualModeProfile.Acc = defaultAcc;
            this.manualModeProfile.Dec = defaultDcc;
            this.SelectedProfile = this.ProfileList.Last<MtrMotionProfile>();
        }

        public void UpdateDefaultProfileValues()
        {
            MtrMotionProfile mtrMotionProfile = this.GetDefaultProfile();
            mtrMotionProfile.StartVel = (float)((int)this.axis.MtrSpeed.StrVel);
            mtrMotionProfile.MaxVel = (float)((int)this.axis.MtrSpeed.MaxVel);
            mtrMotionProfile.Acc = this.axis.MtrSpeed.Tacc;
            mtrMotionProfile.Dec = this.axis.MtrSpeed.Tdec;
            this.SelectedProfile = this.ProfileList.Last<MtrMotionProfile>();
        }

        public void CopyProfileListData(List<MtrMotionProfile> srcProfileList)
        {
            List<MtrMotionProfile> list = new List<MtrMotionProfile>();
            for (int i = 0; i < srcProfileList.Count; i++)
            {
                bool flag = false;
                for (int j = 0; j < this.profileList.Count; j++)
                {
                    if (!(this.profileList[j].Name != srcProfileList[i].Name))
                    {
                        this.profileList[j].CopyValueFrom(srcProfileList[i]);
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    try
                    {
                        this.RegisterNewProfile(srcProfileList[i]);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        private void RemoveProfile(IMotionProfile pf)
        {
            if (pf != null)
            {
                if (!(pf.Name == "Default"))
                {
                    if (!(pf.Name == "AutoMode"))
                    {
                        if (!(pf.Name == "ManualMode"))
                        {
                            if (this.profileList.Contains(pf))
                            {
                                this.profileList.Remove(pf as MtrMotionProfile);
                            }
                            if (pf == this.selectedProfile)
                            {
                                if (this.profileList.Count > 0)
                                {
                                    this.SelectedProfile = this.profileList.Last<MtrMotionProfile>();
                                }
                                else
                                {
                                    this.SelectedProfile = null;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void RemoveSelectedProfile()
        {
            this.RemoveProfile(this.selectedProfile);
        }

        public const string DefaultProfileName = "Default";
        public const string AutoModeProfileName = "AutoMode";
        public const string ManualModeProfileName = "ManualMode";

        public static string[] DefaultProfileNames = new string[]
        {
            "Default",
            "AutoMode",
            "ManualMode"
        };

        private ObservableCollection<MtrMotionProfile> profileList = new ObservableCollection<MtrMotionProfile>();
        private AxisBase axis = null;
        private string assignAxisID = "";
        private IMotionProfile selectedProfile = null;
        private MtrMotionProfile defaultProfile = null;
        private MtrMotionProfile autoModeProfile = null;
        private MtrMotionProfile manualModeProfile = null;
    }
}
