using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SolveWare_Motion
{
    //[Serializable]
    //[StructLayout(LayoutKind.Sequential)]
    public class MotorRuntimeInteration
    {
     
        public MotionDel2 pIsInhibitToHome = null;
    
        public MotionDel2 pIsInhibitToMove = null;

        protected double planPosition;
     
        protected double currentPosition = 2105.0;

        //protected string _AxisTag = string.Empty;
        //protected bool isServoOn;
        //protected bool isInPosition;
        //protected bool isAlarm;
        //protected bool isOrg;
        //protected bool isPosLimit;
        //protected bool isNegLimit;
        //protected bool isProhibitActivated = false;
        //protected bool isMoving;
        //protected bool isStopReq = false;
        //protected double currentPulse;
        //protected double currentPhysicalPos;
        //protected double analogInputValue;
        //protected string interlockWaringMsg;
        //protected bool isSimulate;
        //protected bool hasHome;
        #region ctor
        public MotorRuntimeInteration()
        {

        }
        public string AxisName
        {
            get;  set;
        }
        public string AxisTag
        {
            get;  set; 
        }
        public double PlanPosition
        {
            get;  set;
        }
        public double CurrentPulse
        {
            get;  set;
        }
        public double CurrentPosition
        {
            get;  set;
        }
        public bool IsSimulation
        {
            get;  set;
        }
        public bool IsServoOn
        {
            get;  set;
        }
        public bool IsInPosition
        {
            get;  set;
        }
        public bool IsAlarm
        {
            get;  set;
        }
        public bool IsOrg
        {
            get;  set;
        }
        public bool IsPosLimit
        {
            get;  set;
        }
        public bool IsNegLimit
        {
            get;  set;
        }
        public bool HasHome
        {
            get;  set;
        }
        public bool IsMoving
        {
            get;  set;
        }
 
        public double AnalogInputValue
        {
            get;  set;
        }
        public string InterlockWaringMsg
        {
            get;  set;
        }
        #endregion
    }
}
