using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public delegate void DPMonitorUpdatedEventHandler(float Temp, float RH, float AH, float DP);

    //显通电子 HT20-B
    public class Humidity_XT_HT20B : InstrumentBase, IInstrumentBase
    {

        uint RegAddr_Start = 0;
        //public enum TempMonitor_RegAddr : uint
        //{
        //    GetTemperature = 0,
        //    SetTempCoeff = 974
        //    //T_High = 974,
        //    //R_High = 976,
        //    //T_Mid = 978,
        //    //R_Mid = 980,
        //    //T_Low = 982,
        //    //R_Low = 984
        //}
        //protected const int dChannels = 16;
        //protected const int SHORT_ARRAY_DATA_LEN = 2;
        //protected const int BYTE_ARRAY_DATA_LEN = 4;
        //private Modbus modbus;
        public Modbus Modbus
        {
            get
            {
                return this._chassis.Modbus;
            }
        }
        protected IInstrumentChassis myChassis = null;
        public int ID { get; set; }
        //public string Name { get; set; }

        float _Temperatures;        //温度        DegC
        float _RelativeHumidity;    //相对湿度      %
        float _AbsoluteHumidity;    //绝对湿度      g/m3
        float _DewPoint;            //露点        DegC


        public Humidity_XT_HT20B(string name, string address, IInstrumentChassis chassis)
                : base(name, address, chassis)
        {
            this.ID = Convert.ToInt16(this.Address);
            this.Name = name;
            this.myChassis = chassis;
        }
        public override void Initialize()
        {
            if (this._isOnline)
            {
                //if (this._isSimulation)
                //{

                //}
                //else
                //{
                    this._Temperatures = 0.0f;
                    this._RelativeHumidity = 0.0f;
                    this._AbsoluteHumidity = 0.0f;
                    this._DewPoint = 0.0f;
                //}
            }
            base.Initialize();
        }
        
        public virtual float Tempertures
        {
            get { return _Temperatures; }
            protected set { _Temperatures = value; }
        }
        public virtual float RelativeHumidity
        {
            get { return _RelativeHumidity; }
            protected set { _RelativeHumidity = value; }
        }
        public virtual float AbsoluteHumidity
        {
            get { return _AbsoluteHumidity; }
            protected set { _AbsoluteHumidity = value; }
        }
        public virtual float DewPoint
        {
            get { return _DewPoint; }
            protected set { _DewPoint = value; }
        }
        public override void RefreshDataLoop(CancellationToken token)
        {
            //return;
            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                    token.ThrowIfCancellationRequested();
                    this.RefreshDataOnceCycle(token);
                    Thread.Sleep(2000);
                }
                catch (OperationCanceledException ocex)
                {
                    //响应取消操作前把喂狗功能关掉
                    //this.EnableFeedDog(false);
                    return;
                }
                catch (Exception ex)
                {
                    //非取消操作不退出循环
                }
            }
        }
        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
            this._Temperatures = 25;
            this._RelativeHumidity = 0.5f;
            //绝对湿度的计算
            bool ok = Calc(_Temperatures, _RelativeHumidity, ref _AbsoluteHumidity, ref _DewPoint);
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            try
            {
                if (this.IsOnline == false)
                {
                    return;
                }
                //[0]温度 TEMP
                //[1]湿度 RH
                short[] values = new short[2];
                bool isOk = this.Modbus.Function_3(this.ID, (int)RegAddr_Start, 2, ref values);

                if (isOk == false)
                {
                    isOk = this.Modbus.Function_3(this.ID, (int)RegAddr_Start, 2, ref values);
                }

                if (isOk)
                {
                    _Temperatures = (float)(values[0] / 10.0);
                    _RelativeHumidity = (float)(values[1] / 10.0);

                    //绝对湿度和露点的计算
                    bool ok = Calc(_Temperatures, _RelativeHumidity, ref _AbsoluteHumidity, ref _DewPoint);

                }
                return;
            }
            catch (Exception ex)
            {
                string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis.Name}] resource [{this._chassis.Resource}]- RefreshDataOnceCycle exception:{ex.Message}-{ex.StackTrace}.";
                throw new Exception(msg, ex);
            }
        }
 

        //根据温度和湿度计算绝对湿度和露点
        //转换方法不明, 就这么算
        bool Calc(double Temp, double Rh, ref float Ah, ref float Dp)
        {
            double Tk = Temp + 273.15;

            double ptot = 1013.25; //hPa单位  if (ptot > 300000 || ptot <= 0)
            if (Tk > 647.096 || Tk < 173)
            {
                return false;
            }
            if (Rh > 100 || Rh < 0)
            {
                return false;
            }

            double pws = Pww(Tk, ptot);
            //============

            double pw = Rh / 100 * pws; //不知道算了个啥
            
            //计算绝对湿度
            Ah = (float)A_to_outputunit(pw, Tk, ptot);   //绝对湿度

            //计算露点
            Dp = (float)(Tf_find(pw, ptot) - 273.15);

            return true;
        }





        #region 算法支持函数, 我也不懂, 这么用就好
        double A_to_outputunit(double pw, double tk, double ptot)
        {
            double __reg1 = 0;
            //if ((__reg0 = document.Pwsat.aunit.value) === "gm3")
            {
                __reg1 = 216.679 * pw / tk;
            }
            //else if (__reg0 === "grft")
            //{
            //    __reg1 = 94.68779 * pw / tk;
            //}
            //else
            //{
            //    __reg1 = 216.679 * pw / tk;
            //}
            return __reg1;
        }


        //露点计算
        double Tf_find(double Pwfind, double ptot)
        {
            double __reg7 = 0;
            double __reg1 = 0;
            double __reg3 = 0;
            double __reg2 = 0;
            double __reg4 = 0;
            __reg7 = Math.Log(Pwfind / 6.1134) / 2.30258509299;
            __reg1 = 273.47 / (9.7911 / __reg7 - 1) + 273.15;
            __reg3 = 0.01;
            do
            {
                __reg2 = Pwi(__reg1, ptot);
                __reg4 = (Pwi(__reg1 + __reg3, ptot) - __reg2) / __reg3;
                __reg1 = __reg1 + (Pwfind - __reg2) / __reg4;
            }
            while (Math.Abs(Pwfind - __reg2) > 1e-006);
            return __reg1;
        }

        double Td_find(double Pwfind, double ptot)
        {
            double __reg7 = 0;
            double __reg1 = 0;
            double __reg3 = 0;
            double __reg2 = 0;
            double __reg4 = 0;
            __reg7 = Math.Log(Pwfind / 6.1078) / 2.30258509299;
            __reg1 = 237.3 / (7.5 / __reg7 - 1) + 273.15;
            __reg3 = 0.01;
            do
            {
                __reg2 = Pww(__reg1, ptot);
                __reg4 = (Pww(__reg1 + __reg3, ptot) - __reg2) / __reg3;
                __reg1 = __reg1 + (Pwfind - __reg2) / __reg4;
            }
            while (Math.Abs(Pwfind - __reg2) > 1e-005);
            return __reg1;
        }





        double Pwi(double T, double ptot)
        {
            double __reg1 = 0;
            double __reg5 = 0;
            double __reg2 = 0;
            double __reg3 = 0;
            double[] Ai = new double[] { -5674.5359, 6.3925247, -0.009677843, 6.2215701e-007, 2.0747825e-009, -9.484024e-013, 4.1635019 };

            __reg1 = T * 1;
            if (__reg1 > 273.15)
            {
                __reg3 = Pww(T, ptot);
            }
            else
            {
                __reg5 = Ai[0] / __reg1 + Ai[1] + Ai[2] * __reg1 + Ai[3] * __reg1 * __reg1 + Ai[4] * __reg1 * __reg1 * __reg1 + Ai[5] * __reg1 * __reg1 * __reg1 * __reg1 + Ai[6] * Math.Log(__reg1);
                __reg2 = Math.Exp(__reg5) / 100;
                __reg3 = __reg2;// * getenha(__reg2, ptot, T);
            }
            return __reg3;
        }


        //返回pws
        //T=温度(DegC)
        //ptot=1013.25(hPa)
        double Pww(double T, double ptot)
        {
            double __reg2 = 0;
            double __reg1 = 0;
            double __reg4 = 0;
            double __reg3 = 0;
            double[] Ci = new double[] { -7.85951783, 1.84408259, -11.7866497, 22.6807411, -15.9618719, 1.80122502 };
            __reg2 = T * 1;
            __reg1 = 1 - __reg2 / 647.096;
            __reg4 = 647.096 / __reg2 * (Ci[0] * __reg1 + Ci[1] * Math.Pow(__reg1, 1.5) + Ci[2] * __reg1 * __reg1 * __reg1 + Ci[3] * Math.Pow(__reg1, 3.5) + Ci[4] * __reg1 * __reg1 * __reg1 * __reg1 + Ci[5] * Math.Pow(__reg1, 7.5));
            __reg3 = Math.Exp(__reg4) * 220640;
            return __reg3;// * getenha(__reg3, ptot, T);
        }

        #endregion

    }
}