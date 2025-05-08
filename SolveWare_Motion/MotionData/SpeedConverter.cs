using SolveWare_Business_Manager_Motion.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_Business_Motion.Base
{



    public class SpeedConverter
    {
        // Start Velocity: Input => mm/s | unit/s = pulse/Ratio perMM | 10 mm/s =  10000(pulse)/ 1mm  * 10; 
        // Acc_s = (MaxSpeed - MinSpeed) / _CurrentAxisConfig.Acc_Unit;
        // Dec_s = Acc_s;


        public static void ConverToHomeMMPerSec(AxisBase ax, ref float startVel, ref float maxVel, ref double acc, ref double dec, bool isJog = false)
        {

            double unitPerSec = ax.MtrTable.StepPerRevolution / ax.MtrTable.mmPerRevolution;


            double acc_Unit = ax.MtrTable.HmAcceleration * unitPerSec;
            double dec_Unit = ax.MtrTable.HmDeceleration * unitPerSec;

            startVel = (float)(unitPerSec * ax.MtrTable.HmStrVel);
            maxVel = (float)(unitPerSec * ax.MtrTable.HmMaxVel);

            double factor = maxVel == startVel ? 1 : maxVel - startVel;
            acc = factor / acc_Unit;
            dec = factor / dec_Unit;
           
            //1
            //10
            //1
            //1


            //startVel = (float)(ax.MtrTable.HmStrVel * ax.MtrTable.StepPerRevolution / ax.MtrTable.mmPerRevolution);
            //maxVel = (float)(ax.MtrTable.HmMaxVel * ax.MtrTable.StepPerRevolution / ax.MtrTable.mmPerRevolution);
            //acc = (maxVel - startVel) / (ax.MtrTable.HmAcceleration * (ax.MtrTable.StepPerRevolution / ax.MtrTable.mmPerRevolution));
            //dec = (maxVel - startVel) / (ax.MtrTable.HmDeceleration * (ax.MtrTable.StepPerRevolution / ax.MtrTable.mmPerRevolution));
        }
        public static void ConverToAutoMMPerSec(AxisBase ax, ref float startVel, ref float maxVel, ref double acc, ref double dec, bool isJog = false)
        {
            double unitPerSec = ax.MtrTable.StepPerRevolution / ax.MtrTable.mmPerRevolution;
            double acc_Unit = ax.MtrTable.AutoAcceleration * unitPerSec;
            double dec_Unit = ax.MtrTable.AutoDeceleration * unitPerSec;

            startVel = (float)(unitPerSec * ax.MtrTable.AutoStrVel);
            maxVel = (float)(unitPerSec * ax.MtrTable.AutoMaxVel);


            double factor = maxVel == startVel ? 1 : maxVel - startVel;
            acc = factor / acc_Unit;
            dec = factor / dec_Unit;


            //startVel = (float)(ax.MtrTable.AutoStrVel * ax.MtrTable.StepPerRevolution / ax.MtrTable.mmPerRevolution);
            //maxVel = (float)(ax.MtrTable.AutoMaxVel * ax.MtrTable.StepPerRevolution / ax.MtrTable.mmPerRevolution);
            //acc = (maxVel - startVel) / (ax.MtrTable.AutoAcceleration * (ax.MtrTable.StepPerRevolution / ax.MtrTable.mmPerRevolution));
            //dec = (maxVel - startVel) / (ax.MtrTable.AutoDeceleration * (ax.MtrTable.StepPerRevolution / ax.MtrTable.mmPerRevolution));
        }

        public static void ConverToAutoMMPerSec2(AxisBase2 ax, ref float startVel, ref float maxVel, ref double acc, ref double dec, bool isJog = false)
        {
            double unitPerSec = ax.MtrTable.PulsePerRevolution / ax.MtrTable.UnitPerRevolution;
            double acc_Unit = ax.MtrSpeed.Auto_Acceleration * unitPerSec;
            double dec_Unit = ax.MtrSpeed.Auto_Deceleration * unitPerSec;

            startVel = (float)(unitPerSec * ax.MtrSpeed.Auto_Start_Velocity);
            maxVel = (float)(unitPerSec * ax.MtrSpeed.Auto_Max_Velocity);


            double factor = maxVel == startVel ? 1 : maxVel - startVel;
            acc = factor / acc_Unit;
            dec = factor / dec_Unit;


            //startVel = (float)(ax.MtrTable.AutoStrVel * ax.MtrTable.StepPerRevolution / ax.MtrTable.mmPerRevolution);
            //maxVel = (float)(ax.MtrTable.AutoMaxVel * ax.MtrTable.StepPerRevolution / ax.MtrTable.mmPerRevolution);
            //acc = (maxVel - startVel) / (ax.MtrTable.AutoAcceleration * (ax.MtrTable.StepPerRevolution / ax.MtrTable.mmPerRevolution));
            //dec = (maxVel - startVel) / (ax.MtrTable.AutoDeceleration * (ax.MtrTable.StepPerRevolution / ax.MtrTable.mmPerRevolution));
        }


        public static void ConverToJogMMPerSec(AxisBase ax, ref float startVel, ref float maxVel, ref double acc, ref double dec, bool isJog = false)
        {
            double unitPerSec = ax.MtrTable.StepPerRevolution / ax.MtrTable.mmPerRevolution;
            double acc_Unit = ax.MtrTable.JogAcceleration * unitPerSec;
            double dec_Unit = ax.MtrTable.JogDeceleration * unitPerSec;

            startVel = (float)(unitPerSec * ax.MtrTable.JogStrVel);
            maxVel = (float)(unitPerSec * ax.MtrTable.JogMaxVel);


            double factor = maxVel == startVel ? 1 : maxVel - startVel;
            acc = factor / acc_Unit;
            dec = factor / dec_Unit;

            //    startVel = (float)(ax.MtrTable.JogStrVel * ax.MtrTable.StepPerRevolution / ax.MtrTable.mmPerRevolution);
            //    maxVel = (float)(ax.MtrTable.JogMaxVel * ax.MtrTable.StepPerRevolution / ax.MtrTable.mmPerRevolution);
            //    acc = (maxVel - startVel) / (ax.MtrTable.JogAcceleration * (ax.MtrTable.StepPerRevolution / ax.MtrTable.mmPerRevolution));
            //    dec = (maxVel - startVel) / (ax.MtrTable.JogDeceleration * (ax.MtrTable.StepPerRevolution / ax.MtrTable.mmPerRevolution));
        }


    }

    
}
