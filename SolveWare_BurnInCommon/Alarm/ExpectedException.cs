using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInCommon
{
    public enum ExpectedException
    {
        RACK_WATER_TEMPERATURE_ALARM,
        RACK_AIR_PRESSURE_ALARM,
        RACK_FAN_STOP_ALARM,
        RACK_LEAKAGE_ALARM,
        RACK_WATER_STABLE_ALARM,
        RACK_VALVE_ALARM,
        RACK_CDAIR_PRESSURE_ALARM,
        RACK_INIT_ALARM,
        GM_ALARM,
        UNIT_ALARM,
        BURNIN_RUNTIME_ALARM,
        IM_ALARM,
        INSTU_CHASSIS_ALRAM,
        USER_ALARM,
        DATA_HANDLER_ALARM,
        GUI_ELEMENT_ALARM,
        MES_UPDATE_ALARM,
        UNEXPECTED
    }
}