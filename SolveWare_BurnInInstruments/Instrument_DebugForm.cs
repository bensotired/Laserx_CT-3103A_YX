using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LX_Utilities;

namespace SolveWare_BurnInInstruments
{
    public partial class Instrument_DebugForm : Form
    {
        List<IInstrumentBase> list = new List<IInstrumentBase>();
        IInstrumentChassis _sampleChassis;
        IInstrumentChassis _realEthernetChassis;
        public Instrument_DebugForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            _sampleChassis.TurnOnline(false);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _sampleChassis.TurnOnline(true);
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void Instrument_DebugForm_Load(object sender, EventArgs e)
        {

            //this._sampleChassis = new SampleInstrumentChassis_SMU("sampleChassisName", "sampleChassisSource", true, (str) => { });
            //for (int instIndex = 1; instIndex <= 4; instIndex++)
            //{
            //    var _sampleInstrument = new SampleInstrument_SMU($"SMU{instIndex}", $"{instIndex}:d2", this._sampleChassis);
            //    list.Add(_sampleInstrument);
            //    _sampleInstrument.Initialize();

            //}
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                _realEthernetChassis = new EthernetChassis("EthernetChassis", "192.168.1.113:4196", true);
                _realEthernetChassis.Initialize();
                Keithley_24xx smu1 = new Keithley_24xx("LDDriver", "1", _realEthernetChassis);
             
                //IInstrumentBase source = new SMU_LXHM_8CHs_HighPow("8chsSmu", "1", _realEthernetChassis);
                //LaserX_1010Series_DualMode_SMU source1 = new LaserX_1010Series_DualMode_SMU("dm_smu", "1", _realEthernetChassis);
                //LaserX_1010Series_DualMode_SMU source2 = new LaserX_1010Series_DualMode_SMU("dm_smu", "2", _realEthernetChassis);
                //source1.TurnOnline(true);
                //source1.Initialize();
                //source2.TurnOnline(true);
                //source2.Initialize();
            }
            catch
            {

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            _realEthernetChassis.TurnOnline(false);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            _realEthernetChassis.TurnOnline(true);
        }
        List<TEC_Controller_4CH> temp = new List<TEC_Controller_4CH>();
        private void button8_Click(object sender, EventArgs e)
        {
            _realEthernetChassis = new MeCom_TCPChassis("MeCom_TCPChassis", "192.168.1.101:4196", true);
            _realEthernetChassis.Initialize();
            temp.Clear();
            for (int id = 1; id <=4; id++)
            {
                TEC_Controller_4CH tec = new TEC_Controller_4CH($"tec_{id}", id.ToString(), _realEthernetChassis);
                tec.TurnOnline(true);
                tec.Initialize();
                temp.Add(tec);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            _realEthernetChassis.TurnOnline(false);
            //temp.ForEach(tec =>
            //{
            //    tec.GetTargetObjectPID(1);
            //    tec.GetTargetObjectPID(2);
            //    tec.GetTargetObjectPID(3);
            //    tec.GetTargetObjectPID(4);
            //});
        }

        private void button10_Click(object sender, EventArgs e)
        {
            _realEthernetChassis.TurnOnline(true);
            //temp.ForEach(tec =>
            //{
            //    tec.SetTargetObjectTemperature(1, 25);
            //    tec.SetTargetObjectTemperature(2, 25);
            //    tec.SetTargetObjectTemperature(3, 25);
            //    tec.SetTargetObjectTemperature(4, 25);
            //});
        }
        /// <summary>
        /// Demo for K2401 & K6485 Triggered LIV Sweep
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            NiVisaChassis Chassis2401 = new NiVisaChassis("2401Chassis", "GPIB0::4::INSTR", true);
            NiVisaChassis Chassis6485 = new NiVisaChassis("6485Chassis", "GPIB0::14::INSTR", true);

            Keithley_24xx K2401 = new Keithley_24xx("2401", "1", Chassis2401);
            Keithley_6485 K6485 = new Keithley_6485("6485", "1", Chassis6485);
            K2401.TurnOnline(true);
            K6485.TurnOnline(true);
            K2401.Reset();
            K6485.Reset();            
            K6485.ZeroCorrection();

            double ldComplianceCurrent = 2.0;
            double nplc = 0.1;
            double start = 0;
            double stop = 0.1;
            double step = 0.001;

            double[] currs = LX_BurnInSolution.Utilities.ArrayMath.CalculateArray(start, stop, step);
            Stopwatch sw = new Stopwatch();
            K6485.IsCurrentSenseAutoRangeOn = false;
            K6485.CurrentSenseRange_A = 0.01;
            K2401.SetupCurrentStairSweep(true, ldComplianceCurrent, nplc, TriggerLine.Line3, TriggerLine.Line4, currs.Length, false, SelectTerminal.Rear, start, stop, step);
            K6485.SetupTrigger(false, TriggerLine.Line4, TriggerLine.Line3, currs.Length, nplc);

            K6485.Trigger();
            K2401.Trigger();
            sw.Start();
            //K6485.WaitForComplete();
            //K2401.WaitForComplete();
            
            double[] pdCurrs = K6485.FetchRealData();
            double[] volts = K2401.FetchRealData();

            K2401.IsOutputOn = false;
            sw.Stop();
            MessageBox.Show(sw.Elapsed.TotalSeconds.ToString());

            StreamWriter writer = new StreamWriter("LIV.CSV");
            for (int i = 0; i < currs.Length; i++)
            {
                writer.WriteLine($"{currs[i]},{volts[i]},{pdCurrs[i]}");
            }
            writer.Close();

        }
    }
}