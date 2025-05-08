using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thorlabs.TLPM_32.Interop;

namespace SolveWare_BurnInInstruments
{
    public class TMPL_Controllers : InstrumentBase, IInstrumentBase
    {
        public TMPL_Controllers(string name, string address, IInstrumentChassis chassis)
       : base(name, address, chassis)
        {
        }
        public TLPM tlpm { get; set; } = null;

        /// <summary>
        /// 索雷博探头初始化
        /// </summary>
        /// <returns></returns>
        public bool InitializePM()
        {
            try
            {
                HandleRef Instrument_Handle = new HandleRef();

                TLPM searchDevice = new TLPM(Instrument_Handle.Handle);

                uint count = 0;

                string firstPowermeterFound = "";

                try
                {
                    int pInvokeResult = searchDevice.findRsrc(out count);

                    if (count > 0)
                    {
                        StringBuilder descr = new StringBuilder(1024);

                        searchDevice.getRsrcName(0, descr);

                        firstPowermeterFound = descr.ToString();
                    }
                }
                catch (Exception ex)
                {

                }

                if (count == 0)
                {
                    searchDevice.Dispose();
                    return false;
                }

                tlpm = new TLPM(firstPowermeterFound, false, false);
                return true;
            }
            catch
            {
                return false;

            }
        }


        public bool InitializePM(ref TLPM tlpm)
        {
            try
            {
                HandleRef Instrument_Handle = new HandleRef();

                TLPM searchDevice = new TLPM(Instrument_Handle.Handle);

                uint count = 0;

                string firstPowermeterFound = "";

                try
                {
                    int pInvokeResult = searchDevice.findRsrc(out count);

                    if (count > 0)
                    {
                        StringBuilder descr = new StringBuilder(1024);

                        searchDevice.getRsrcName(0, descr);

                        firstPowermeterFound = descr.ToString();
                    }
                }
                catch (Exception ex)
                {

                }

                if (count == 0)
                {
                    searchDevice.Dispose();
                    return false;
                }

                tlpm = new TLPM(firstPowermeterFound, false, false);
                return true;
            }
            catch
            {
                return false;

            }
        }



        public void setWavelength(double value)
        {
            try
            {
                tlpm.setWavelength(value);
            }
            catch (Exception ex)
            {
            }
        }

        public void setPowerAutoRange(bool isT)
        {
            try
            {
                tlpm.setPowerAutoRange(true);
            }
            catch (Exception ex)
            {
            }
        }

        public void measPower(out double CalibPower)
        {

            try
            {
                tlpm.measPower(out CalibPower);
            }
            catch (Exception ex)
            {
                CalibPower = 0;
            }
        }

        public void Dispose()
        {
            try
            {
                tlpm.Dispose();
            }
            catch (Exception ex)
            {

            }
        }










        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }



    }
}
