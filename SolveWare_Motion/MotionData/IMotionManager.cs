using SolveWare_Business_Manager_Motion.Business;
using SolveWare_Business_Motion.Base;
using SolveWare_Service_Data.Interface;
using SolveWare_Service_Manager;
using SolveWare_Service_Manager.Interface;
using SolveWare_Service_Tool.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_Business_Manager_Motion.Base
{
    public interface IMotionManager : IManagerConfig<MotionManagerConfig>, IManagerTool<AxisBase2>, IManagerPoolData<Motion_Data>,IManagerMonitor,  IManagerAction, IManager
    {
        void StopAllMotors();
        void DisableAllMotors();
    }
}
