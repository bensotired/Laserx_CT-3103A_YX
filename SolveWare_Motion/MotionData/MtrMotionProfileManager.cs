using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Utilities;

namespace SolveWare_Business_Motion.Base
{
    public class MtrMotionProfileManager
    {
        public List<AxisMotionProfile> AllAxisProfile
        {
            get
            {
                return this.allAxisProfile;
            }
        }

        public AxisMotionProfile GetAxisProfileByAxis(AxisBase ax)
        {
            for (int i = 0; i < this.allAxisProfile.Count; i++)
            {
                if (this.allAxisProfile[i].Axis == ax)
                {
                    return this.allAxisProfile[i];
                }
            }
            return null;
        }

        public AxisMotionProfile GetAxisProfileByAxisID(string axid)
        {
            for (int i = 0; i < this.allAxisProfile.Count; i++)
            {
                if (!(axid == ""))
                {
                    if (this.allAxisProfile[i].AssignAxisID == axid)
                    {
                        return this.allAxisProfile[i];
                    }
                }
            }
            return null;
        }

        public MtrMotionProfile[] GetCustomProfileByAxis(AxisBase ax)
        {
            MtrMotionProfile[] result = null;
            AxisMotionProfile axisProfileByAxis = this.GetAxisProfileByAxis(ax);
            if (axisProfileByAxis != null)
            {
                result = axisProfileByAxis.GetCustomProfileList();
            }
            return result;
        }

        public void Init(string dir, List<AxisBase> motors)
        {
            this.motors = motors;
            this.systemDir = dir;
            this.allAxisProfile.Clear();
            for (int i = 0; i < motors.Count; i++)
            {
                AxisMotionProfile axisMotionProfile = new AxisMotionProfile();
                axisMotionProfile.AssignAxis(motors[i], true);
                this.allAxisProfile.Add(axisMotionProfile);
            }
        }

        public MtrMotionProfile GetProfileWithMode(AxisBase ax, bool autoMode)
        {
            string name = autoMode ? "AutoMode" : "ManualMode";
            return this.GetProfileWithName(ax, name);
        }

        public MtrMotionProfile GetMotionProfile(AxisBase ax, MtrPosSpeed pos)
        {
            MtrMotionProfile mtrMotionProfile = ax.TryGetLongDistanceProf(pos.Pos);
            if (mtrMotionProfile == null)
            {
                if (pos == null)
                {
                    mtrMotionProfile = this.GetDefaultProfile(ax);
                }
                else if (pos.UseCustomProfile)
                {
                    mtrMotionProfile = this.GetProfileWithName(ax, pos.CustomProfileName);
                }
                else
                {
                    mtrMotionProfile = this.GetManualModeProfile(ax);
                }
            }
            return mtrMotionProfile;
        }

        public MtrMotionProfile GetProfileWithName(AxisBase ax, string name)
        {
            MtrMotionProfile result = null;
            AxisMotionProfile axisProfileByAxis = this.GetAxisProfileByAxis(ax);
            if (axisProfileByAxis != null)
            {
                result = axisProfileByAxis.GetProfileByName(name);
            }
            return result;
        }

        public MtrMotionProfile GetAutoModeProfile(AxisBase ax)
        {
            return this.GetProfileWithMode(ax, true);
        }

        public MtrMotionProfile GetManualModeProfile(AxisBase ax)
        {
            return this.GetProfileWithMode(ax, false);
        }

        public void AssignCustonProfile(AxisBase ax, MtrPosSpeed pos, string profileName)
        {
            MtrMotionProfile profileWithName = this.GetProfileWithName(ax, "Default");
            if (profileWithName == null)
            {
                pos.CustomProfileName = "";
            }
            else
            {
                pos.CustomProfileName = profileWithName.Name;
            }
        }

        public MtrMotionProfile GetDefaultProfile(AxisBase ax)
        {
            return this.GetProfileWithName(ax, "Default");
        }

        public bool Load()
        {
            string text = Path.Combine(this.systemDir, this.ProfileFileName + ".xml");
            bool result;
            if (!File.Exists(text))
            {
                result = false;
            }
            else
            {
                bool flag = false;
                var targetObj = XElement.Load(text);
                List<AxisMotionProfile> list = (List<AxisMotionProfile>)Serializer.LoadXml(typeof(List<AxisMotionProfile>), text);
                if (list != null && list.Count == this.motors.Count)
                {
                    List<AxisBase> list2 = new List<AxisBase>();
                    for (int i = 0; i < list.Count; i++)
                    {
                        for (int j = 0; j < this.motors.Count; j++)
                        {
                            if (!(list[i].AssignAxisID != this.motors[i].AxisID))
                            {
                                if (!list2.Contains(this.motors[i]))
                                {
                                    list[i].AssignAxis(this.motors[i], false);
                                    list2.Add(this.motors[i]);
                                }
                            }
                        }
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].HasAssignAxis)
                        {
                            AxisMotionProfile axisProfileByAxis = this.GetAxisProfileByAxis(list[i].Axis);
                            axisProfileByAxis.CopyProfileListData(list[i].ProfileList.ToList<MtrMotionProfile>());
                        }
                    }
                    flag = true;
                }
                result = flag;
            }
            return result;
        }

        public bool Save(List<AxisMotionProfile> profiles)
        {
            string fileName = Path.Combine(this.systemDir, this.ProfileFileName + ".xml");
            //Serializer.SerializeFile<AxisMotionProfile>(fileName, profiles);
            return Serializer.SaveXml(typeof(List<AxisMotionProfile>), this.allAxisProfile, fileName);
        }

        private string systemDir = "";
        private string ProfileFileName = "AxisMotionProfile";
        private List<AxisMotionProfile> allAxisProfile = new List<AxisMotionProfile>();
        private List<AxisBase> motors = null;
    }
}
