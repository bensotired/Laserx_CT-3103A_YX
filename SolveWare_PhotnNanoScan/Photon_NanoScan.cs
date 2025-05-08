using SolveWare_BurnInInstruments;
using System;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics;

namespace SolveWare_BurnInInstruments
{
    public partial class Photon_NanoScan : InstrumentBase, IInstrumentBase
    {
        public enum NanoScan_AxisNameEnum_20050A
        {
            X = 0,
            Y = 1,
        }
        Photon_NanoScanChassis MyChassis
        {
            get
            {
                return this._chassis as Photon_NanoScanChassis;
            }
        }
        public Photon_NanoScan(string name, string address, IInstrumentChassis chassis) : base(name, address, chassis)
        {

        }
        public static bool DevicesOnline { get; set; }
        //调用步骤1
        public  void NsInteropSetDataAcquisition()
        {
            Photon_NanoScanChassis.NsInteropSetDataAcquisition(true);
        }
        public void NsInteropGetDeviceList()
        {
            // retrieve device list
            object deviceList = new ulong[10];
            Photon_NanoScanChassis.NsInteropGetDeviceList(ref deviceList);
        }
        //调用步骤2
        #region old
        //public void WaitForFirstData()
        //{
        //    /* A valid method of determining whether data has been processed yet is
        //     * to evaluate whether any Results (Parameters per NS1) have yet been computed.
        //     * In this example the Centroid position result is used due to its benign
        //     * nature, i.e. usually enabled and not affected by other settings or results.
        //     */

        //    //Enable 13.5% Percent of Peak, D4σ, and Centroid Position results
        //    Photon_NanoScanChassis.NsInteropSelectParameters(0x1 | 0x10 | 0x20);

        //    bool daqState = false;
        //    int count = 0;
        //    float centroidValue = 0;
        //    do
        //    {
        //        //Sleep for a moment while daq initializes
        //        Thread.Sleep(50);

        //        //Get the Centroid position result
        //        Photon_NanoScanChassis.NsInteropGetCentroidPosition(0, 0, ref centroidValue);

        //        //Is the value greater than the default of 0.0?
        //        if (centroidValue > 0)
        //            daqState = true;

        //        //How many times did we iterate before we had data?
        //        count++;
        //    }
        //    while (daqState == false);

        //    //Console.WriteLine(@"DAQ state: {0} : {1}", daqState, count);
        //}
        #endregion
        public void SetAveragAndRotation(short finite, short rolling, float rotationFrequency)
        {
            if (true)
            {
                short _finite = 0;
                short _rolling = 0;
                Photon_NanoScanChassis.NsInteropGetAveraging(ref _finite, ref _rolling);
                //short finite = 5;
                //short rolling = 2;
                Photon_NanoScanChassis.NsInteropSetAveraging(finite, rolling);
            }


            float _rotationFrequency = 0;
            Photon_NanoScanChassis.NsInteropGetRotationFrequency(ref _rotationFrequency);
            //float rotationFrequency = 20;
            Photon_NanoScanChassis.NsInteropSetRotationFrequency(rotationFrequency);
        }
        public void WaitForFirstData()
        {
            /* A valid method of determining whether data has been processed yet is
             * to evaluate whether any Results (Parameters per NS1) have yet been computed.
             * In this example the Centroid position result is used due to its benign
             * nature, i.e. usually enabled and not affected by other settings or results.
             */

            //Enable 13.5% Percent of Peak, D4σ, and Centroid Position results
            Photon_NanoScanChassis.NsInteropSelectParameters(0x00000020);

            //ulong p_135 = 0x1;
            //ulong p_50 = 0x2;
            //ulong p_100 = 0x4;


            bool AutoROI = Photon_NanoScanChassis.NsInteropGetAutoROI();
            if (!AutoROI)
            {
                Photon_NanoScanChassis.NsInteropSetAutoROI(true);
            }


            //NsInteropSelectParameters(0x1 | 0x2 | 0x4);
            //NsInteropSelectParameters(0x1 | 0x10 | 0x20);
            //NsInteropGetSelectedParameters(ref p_135);
            //NsInteropGetSelectedParameters(ref p_50);
            //NsInteropGetSelectedParameters(ref p_100);

            bool daqState = false;
            int count = 0;
            float centroidValue = 0;
            float beamWidth4Sigma = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            do
            {
                //Sleep for a moment while daq initializes
                Thread.Sleep(50);

                //Get the Centroid position result
                Photon_NanoScanChassis.NsInteropGetCentroidPosition(0, 0, ref centroidValue);

                //Is the value greater than the default of 0.0?
                if (centroidValue > 0)
                    daqState = true;

                //How many times did we iterate before we had data?
                count++;
                if (count > 10)
                {
                    //if (MessageBox.Show("确定可以TEC平台可以正常进入测试位置?", "进料位置确认", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.Yes)
                    MessageBox.Show("Photon_NanoScan WaitForFirstData TimeOut?", "Photon_NanoScan Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
            while (daqState == false);
            sw.Stop();
            // Console.WriteLine(@"DAQ state: {0} : {1}", daqState, count);
        }

        //调用步骤3
        #region old
        //public void ReadProfile(NanoScan_AxisNameEnum_20050A AxisName, float leftBound, float rightBound, float samplingRes, short decimation, int numOfPoints,
        //    ref  float[] _Amplitude, ref float[] _Position) 
        //{
        //    //currently we are only reading data from Aperture 1 (or X axis) to simplify the code.
        //    short aperture = (short)AxisName;

        //    //short aperture = 0;
        //    //float leftBound = 0;
        //    //float rightBound = 0;
        //    //float samplingRes = 0;
        //    //short decimation = 1;
        //    //int numOfPoints = 10;

        //    /* Initialize new arrays that will be marshaled through the COM As.  The As
        //     * will handle conversion from VARIANT to .NET Array.  In this case, they will come
        //     * back as an array of floats.
        //     */
        //    object amplitude = new float[numOfPoints];
        //    object position = new float[numOfPoints];

        //    //Calculate the decimation factor to only retrieve numOfPoints
        //    Photon_NanoScanChassis.NsInteropGetApertureLimits(aperture, ref leftBound, ref rightBound);
        //    Photon_NanoScanChassis.NsInteropGetSamplingResolution(aperture, ref samplingRes);
        //    decimation = (short)((rightBound - leftBound) / samplingRes / numOfPoints);

        //    //Read the profile data
        //    Photon_NanoScanChassis.NsInteropReadProfile(aperture, leftBound, rightBound, decimation, ref amplitude, ref position);

        //    //Cast the returned objects to an IEnumerable of type float and then to an array of floats to access the data.
        //    //float[] fAmplitude = ((float[])((IEnumerable<float>)amplitude));
        //    //float[] fPosition = ((float[])((IEnumerable<float>)position));

        //    _Amplitude = ((float[])((IEnumerable<float>)amplitude));
        //    _Position = ((float[])((IEnumerable<float>)position));

        //    //Output data values to the console
        //    //Console.WriteLine(@"Values: (Amplitude, Position)");
        //    //for (var i = 0; i < numOfPoints; i++)
        //    //    Console.WriteLine(@"({0}, {1})", fAmplitude[i], fPosition[i]);
        //}
        #endregion
        public class dataKvp
        {
            public float pos = 0;
            public float adc = 0;
        }
        public List<dataKvp>  ReadProfile(NanoScan_AxisNameEnum_20050A AxisName, float leftBound, float rightBound, float samplingRes, 
            short decimation, int numOfPoints,ref double  BeamWidth_13p5, ref double BeamWidth_50, ref double BeamWidth_5)
        {
            short aperture = (short)AxisName;
            //currently we are only reading data from Aperture 1 (or X axis) to simplify the code.

            /* Initialize new arrays that will be marshaled through the COM interop.  The interop
             * will handle conversion from VARIANT to .NET Array.  In this case, they will come
             * back as an array of floats.
             */
            object amplitude = new float[numOfPoints];
            object position = new float[numOfPoints];
            var cl_1 = 13.5f;
            var cl_2 = 50.0f;
            var cl_3 = 5.0f;
            var bw_13p5 = 0.0f;
            var bw_50 = 0.0f;
            var bw_5 = 0.0f;

            //Calculate the decimation factor to only retrieve numOfPoints
            Photon_NanoScanChassis.NsInteropGetApertureLimits(aperture, ref leftBound, ref rightBound);
            Photon_NanoScanChassis.NsInteropGetSamplingResolution(aperture, ref samplingRes);


            //NsInteropGetMaxSamplingResolution(ref max_samplingRes);
            //NsInteropSetSamplingResolution(max_samplingRes);


            //decimation = (short)((rightBound - leftBound) / max_samplingRes / numOfPoints);
            decimation = (short)((rightBound - leftBound) / samplingRes / numOfPoints);
            //NewMethod(aperture);
            Photon_NanoScanChassis.NsInteropSelectParameters(0x01 | 0x02);

            Photon_NanoScanChassis.NsInteropGetBeamWidth(aperture, 0, cl_1, ref bw_13p5);
            Photon_NanoScanChassis.NsInteropGetBeamWidth(aperture, 0, cl_2, ref bw_50);
            Photon_NanoScanChassis.NsInteropGetBeamWidth(aperture, 0, cl_3, ref bw_5);
            BeamWidth_13p5 = Convert.ToDouble(bw_13p5);
            BeamWidth_50 = Convert.ToDouble(bw_50);
            BeamWidth_5 = Convert.ToDouble(bw_5);
            //Read the profile data
            Photon_NanoScanChassis.NsInteropReadProfile(aperture, leftBound, rightBound, decimation, ref amplitude, ref position);

            //Cast the returned objects to an IEnumerable of type float and then to an array of floats to access the data.
            float[] fAmplitude = ((float[])((IEnumerable<float>)amplitude));
            float[] fPosition = ((float[])((IEnumerable<float>)position));

            //Output data values to the console
            Console.WriteLine(@"Values: (Amplitude, Position)");
            for (var i = 0; i < numOfPoints; i++)
                Console.WriteLine(@"({0}, {1})", fAmplitude[i], fPosition[i]);

            //NsInteropSelectParameters(0x1 | 0x10 | 0x20);

            //p_135 = 0x1;
            //p_50 = 0x2;


            List<dataKvp> temp = new List<dataKvp>();
            for (int i = 0; i < fAmplitude.Length; i++)
            {
                temp.Add(new dataKvp() { pos = fPosition[i], adc = fAmplitude[i] });
            }
            return temp;
        }
        public  double[] GetBeamWidth(NanoScan_AxisNameEnum_20050A AxisName)
        {
            short aperture = (short)AxisName;
            var cl_1 = 13.5f;
            var cl_2 = 50.0f;
            var bw_13p5 = 0.0f;
            var bw_50 = 0.0f;

            double[] Temp = new double[2];
            //NsInteropSelectParameters( 0x10 | 0x20);
            //var cp = 0.0f;
            //NsInteropGetCentroidPosition(aperture, 0, ref cp);
            //var bw4_sig = 0.0f;
            //NsInteropGetBeamWidth4Sigma(aperture, 0, ref bw4_sig);

            Photon_NanoScanChassis.NsInteropSelectParameters(0x01 | 0x02);

            Photon_NanoScanChassis.NsInteropGetBeamWidth(aperture, 0, cl_1, ref bw_13p5);

            Photon_NanoScanChassis.NsInteropGetBeamWidth(aperture, 0, cl_2, ref bw_50);

            if (aperture == 0)
            {
                //bw_13p5_X = Convert.ToDouble(bw_13p5);
                //bw_50_X = Convert.ToDouble(bw_50);
                Temp[0] = Convert.ToDouble(bw_13p5);
                Temp[1] = Convert.ToDouble(bw_50);
            }
            else
            {
                //bw_13p5_Y = Convert.ToDouble(bw_13p5);
                //bw_50_Y = Convert.ToDouble(bw_50);
                Temp[0] = Convert.ToDouble(bw_13p5);
                Temp[1] = Convert.ToDouble(bw_50); 
            }

            //NsInteropGetBeamWidth(1, 0, cl_1, ref bw_13p5_x);

            //NsInteropGetBeamWidth(1, 0, cl_2, ref bw_50_x);
            return Temp;
        }
       
        public void StartControl()
        {
            if (!this.MyChassis.IsOnline|| DevicesOnline) return;
            if (1 == Photon_NanoScanChassis.InitNsInterop())
            {
                ////Display GUI.
                 Photon_NanoScanChassis.NsInteropSetShowWindow(true);
                //var ABC =Photon_NanoScanChassis.NsInteropGetShowWindow();


                //Retrieve number of connected scan heads.
                Int16 NumberOfDevices = -1;
                Int32 Scode = Photon_NanoScanChassis.NsInteropGetNumDevices(ref NumberOfDevices);
                if (NumberOfDevices == 1)
                {
                    DevicesOnline = true;
                }
                //Start DAQ so that we have meaningful profile data before reading the profile.
                Photon_NanoScanChassis.NsInteropSetDataAcquisition(true);
            }
        }
        public void StopControl()
        {
            if (!this.MyChassis.IsOnline|| !DevicesOnline) return;
            Photon_NanoScanChassis.ShutdownNsInterop();
            DevicesOnline = false;
        }      
        public override void RefreshDataOnceCycle(CancellationToken token)
        {

        }
        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {

        }
    }
}