using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using IdeaOptics;
using NationalInstruments.VisaNS;


namespace SolveWare_BurnInInstruments
{
    public class OSA_PC4000 : InstrumentBase, IInstrumentBase, IOSAUSB
    {
        public Wrapper wrapper { get; set; }
        public OSA_PC4000(string name, string address, IInstrumentChassis chassis)
         : base(name, address, chassis)
        {


        }
        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            // throw new NotImplementedException();
        }

        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
           // throw new NotImplementedException();
        }
        //初始化 
        public override void Initialize()
        {
            this.wrapper = new Wrapper();
        }


        //获取设备的数目
        public int GetSpectrometersCount()
        {
           int opticalCount = this.wrapper.OpenAllSpectrometers();
            return opticalCount;
        }

        /// <summary>
        /// 支持重连5次
        /// </summary>
        public void InitialAndSupprtReconnct()
        {
            double opticalCount = 0;
            try
            {
                this.wrapper = new Wrapper();
                opticalCount = this.wrapper.OpenAllSpectrometers();
            }
            catch (Exception ex)
            {
                int rountTimes = 0;
                do
                {
                    Thread.Sleep(100);
                    this.wrapper = new Wrapper();
                    opticalCount = this.wrapper.OpenAllSpectrometers();
                    if (opticalCount > 0)
                    {
                        break;
                    }
                    rountTimes++;
                } while (rountTimes <= 5);
                return;
            }
            if (opticalCount < 1)
            {
                int rountTimes = 0;
                do
                {
                    Thread.Sleep(100);
                    this.wrapper = new Wrapper();
                    opticalCount = this.wrapper.OpenAllSpectrometers();
                    if (opticalCount > 0)
                    {
                        break;
                    }
                    rountTimes++;
                } while (rountTimes<=5);
            }
        }


        public void SetParameters(int integrationTime, int BoxcarWidth, int ScansToAverage)
        {
            this.wrapper.setIntegrationTime(0, integrationTime);
            this.wrapper.setBoxcarWidth(0, BoxcarWidth);
            this.wrapper.setScansToAverage(0, ScansToAverage);
        }
        public double[] GetOpticalData()
        {
            int Pixels = this.wrapper.getNumberOfPixels(0);
            return this.wrapper.getSpectrum(0);
        }

        public double[] GetWaveLength()
        {
            return this.wrapper.getWavelengths(0);
        }
    }
}