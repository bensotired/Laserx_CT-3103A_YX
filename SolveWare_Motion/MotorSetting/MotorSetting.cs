using SolveWare_BurnInCommon;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SolveWare_Motion
{
    [Serializable]
    public class MotorSetting : CURDItem, ICURDItem
    {
        public MotorSetting()
        {
            MotorTable = new MotorTable();
            MotorSpeed = new MotorSpeed();
            MotorModes = new MotorModes();
        }
        public MotorTable MotorTable { get; set; }
        public MotorSpeed MotorSpeed { get; set; }
        public MotorModes MotorModes { get; set; }
 
    }
}