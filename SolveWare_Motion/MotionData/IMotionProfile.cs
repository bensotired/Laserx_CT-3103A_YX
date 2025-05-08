using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_Business_Motion.Base
{
    public interface IMotionProfile
    {
        int SerialNo { get; }
        float StartVel { get; set; }
        float MaxVel { get; set; }
        float Distance { get; set; }
        bool IsBasedProf { get; }
        bool IsDistanceProf { get; }
        bool IsRequiredJerk { get; set; }
        bool IsRequiredStartVelocity { get; set; }
        bool IsEnableDistanceProf { get; set; }
        string FullName { get; }
        string AxisName { get; set; }
        string Name { get; }
        string AxisID { get; set; }
        string MtrUnit { get; set; }
        double Acc { get; set; }
        double Dec { get; set; }
        double Jerk { get; set; }
        void CopyValueFrom(IMotionProfile src);
    }
}
