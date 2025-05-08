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
    public class MtrSpeed
    {
        [Browsable(false)]
        public string AxisName { get; set; }

        [DisplayName("Long Distance Threshold")]
        [Description("Long Distance Threshold for Profile Selection")]
        [Category("Distance Threshold")]
        public double LongDistanceThreshold { get; set; }

        [DisplayName("Tacc")]
        [Description("Accelation time for Trapezoidal & S-Curve profile motion")]
        [Category("Normal Speed")]
        public double Tacc { get; set; }

        [Category("Normal Speed")]
        [DisplayName("Tdec")]
        [Description("Decelation time for Trapezoidal & S-Curve profle motion")]
        public double Tdec { get; set; }

        [Description("Accelation time for S-Curve profle motion")]
        [DisplayName("VSacc")]
        [Category("Normal Speed")]
        public double VSacc { get; set; }

        [Description("Decelation time for S-Curve profle motion")]
        [Category("Normal Speed")]
        [DisplayName("VSdec")]
        public double VSdec { get; set; }

        [DisplayName("MaxVel")]
        [Category("Normal Speed")]
        [Description("Maximum / constant velocity")]
        public double MaxVel { get; set; }

        [Category("Normal Speed")]
        [DisplayName("StrVel")]
        [Description("Start velocity")]
        public double StrVel { get; set; }

        [Category("Interpolate Speed")]
        [Description("Accelation time for Trapezoidal & S-Curve profile motion (Interpolation)")]
        [DisplayName("IntTacc")]
        public double IntTacc { get; set; }

        [Description("Decelation time for Trapezoidal & S-Curve profle motion (Interpolation)")]
        [Category("Interpolate Speed")]
        [DisplayName("IntTdec")]
        public double IntTdec { get; set; }

        [Category("Interpolate Speed")]
        [DisplayName("IntVSacc")]
        [Description("Accelation time for S-Curve profle motion (Interpolation)")]
        public double IntVSacc { get; set; }

        [Description("Decelation time for S-Curve profle motion (Interpolation)")]
        [Category("Interpolate Speed")]
        [DisplayName("IntVSdec")]
        public double IntVSdec { get; set; }

        [Description("Maximum / constant velocity (Interpolation)")]
        [DisplayName("IntMaxVel")]
        [Category("Interpolate Speed")]
        public double IntMaxVel { get; set; }

        [DisplayName("IntStrVel")]
        [Category("Interpolate Speed")]
        [Description("Start velocity (Interpolation)")]
        public double IntStrVel { get; set; }

        [Description("Circular interpolation max velocity")]
        [Category("Interpolate Speed")]
        [DisplayName("ArcIntStrVel")]
        public double ArcVel { get; set; }

        [Description("Kill Decelatoin")]
        [Category("ACS Profile")]
        [DisplayName("KillDec")]
        public double AcsKillDec { get; set; }

        [Description("Jerk")]
        [Category("ACS Profile")]
        [DisplayName("Jerk")]
        public double AcsJerk { get; set; }

        public MtrSpeed()
        {
            this.Tacc = 1.0;
            this.Tdec = 1.0;
            this.VSacc = 1.0;
            this.VSdec = 1.0;
            this.MaxVel = 10.0;
            this.StrVel = 10.0;
            this.IntTacc = 1.0;
            this.IntVSacc = 1.0;
            this.IntVSdec = 1.0;
            this.IntMaxVel = 1.0;
            this.IntStrVel = 1.0;
            this.IntMaxVel = 1.0;
            this.ArcVel = 1.0;
            this.AcsKillDec = 100.0;
            this.AcsJerk = 100.0;
            this.LongDistanceThreshold = 30.0;
        }
    }
}
