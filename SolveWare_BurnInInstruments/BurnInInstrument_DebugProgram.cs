using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_BurnInInstruments
{

    public class BurnInInstrument_DebugProgram
    {
        [STAThread]
        public static void Main(params string[] args)
        {
            try
            {
                //var resource = "PXI1Slot5";
                //var address = "0";
                ////var resource = "PXI1Slot5@0";
                //PXIe_NIDCPower_Chassis chas = new PXIe_NIDCPower_Chassis("", resource, true);
                //chas.Initialize();
                //PXISourceMeter_4143 sourceLD = new PXISourceMeter_4143("", address, chas);
                //sourceLD.TurnOnline(true); 
                //sourceLD.Initialize();


                //sourceLD.Reset();
                ////sourceLD.IsOutputOn = true;
                //sourceLD.SetupMaster_Sequence_SourceCurrent_SenseVoltage(0, 0.1f, 20f, 3.0f, 0.001f, true);

                //Merged_PXIe_4143.ConfigureMultiChannelSynchronization(sourceLD, null);
                //Merged_PXIe_4143.Trigger(sourceLD, null, 101, 10);

                //var data = sourceLD.Fetch_MeasureVals(100, 10 * 1000.0);
                ////var current = sourceLD.Fetch_SenseCurrents(100, 10 * 1000.0);
                //var volt = data.VoltageMeasurements;
                //var curr = data.CurrentMeasurements;


                //sourceLD.IsOutputOn = false;
                //sourceLD.Reset();







                //NiVisaChassis Chassis2401 = new NiVisaChassis("2401Chassis", "PXI16::13::INSTR", true);
                //Chassis2401.Initialize();

                //var idn = Chassis2401.Query("*IDN?",50);

                //NiVisaChassis Chassis6485 = new NiVisaChassis("6485Chassis", "GPIB0::14::INSTR", true);

                //Keithley_24xx K2401 = new Keithley_24xx("2401", "1", Chassis2401);
                //Keithley_6485 K6485 = new Keithley_6485("6485", "1", Chassis6485);
                //K2401.TurnOnline(true);
                //K6485.TurnOnline(true);
                //K2401.Reset();
                //K6485.Reset();
                //var idn1 = K2401.InstrumentIDN;
                //var idn2 = K6485.InstrumentIDN;

                //EthernetChassis echas = new EthernetChassis("", "127.0.0.1:13897", true);
                //echas.Initialize();
                //VisionJsonCmdRunner runner = new VisionJsonCmdRunner("", "", echas);
                //runner.OCR_1("");

                //map<ViBoolean, int> MapINVERT =
                //{
                //{0, NISYNC_VAL_DONT_INVERT}, {1, NISYNC_VAL_INVERT},
                //};
                //map<ViBoolean, int> MapEDGE =
                //{
                //{0, NISYNC_VAL_UPDATE_EDGE_RISING}, {1, NISYNC_VAL_UPDATE_EDGE_FALLING}
                //};


                //NiVisaChassis ted = new NiVisaChassis("", "USB0::0x1313::0x8048::M00984272::INSTR", true);
                //ted.Initialize();
                //TED4015 tED4015 = new TED4015("", "0", ted);
                //tED4015.TurnOnline(true);
                //tED4015.Reset();
                //var a = tED4015.IDN;
                //tED4015.SetRTB(10000, 25, 3930);
                //tED4015.SetPID(1, 0.1, 0, 1);

                //tED4015.SetTemperatureSetpoint(25);
                //var b = tED4015.GetTemperatureSetpoint();

                //var t = tED4015.GetTempTemperature();

                //tED4015.IsOutPut(true);
                //tED4015.IsOutPut(false);

                PXIe_NIDCPower_Chassis gainchas = new PXIe_NIDCPower_Chassis("", "PXI1Slot5", true);
                gainchas.Initialize();
                PXISourceMeter_4143 gain = new PXISourceMeter_4143("", "0", gainchas);
                gain.TurnOnline(true);
                gain.Initialize();

                PXIe_PxiSession_Chassis chas = new PXIe_PxiSession_Chassis("", "PXI7::13::INSTR", true);
                chas.Initialize();

                PXISourceMeter_6683H p6683h = new PXISourceMeter_6683H("", "0", chas);
                p6683h.TurnOnline(true);







                //try
                //{
                //    string serverIp = "192.168.170.50";
                //    int port = 8807;
                //    TcpClient client = new TcpClient(serverIp, port);
                //    Console.WriteLine("Connected to the server.");

                //    NetworkStream stream = client.GetStream();

                //    //查询当前机器idn信息
                //    string command0 = "*IDN?\n";
                //    byte[] data0 = Encoding.ASCII.GetBytes(command0);
                //    stream.Write(data0, 0, data0.Length);

                //    byte[] responseData0 = new byte[1024];
                //    int bytesRead0 = stream.Read(responseData0, 0, responseData0.Length);
                //    string response0 = Encoding.ASCII.GetString(responseData0, 0, bytesRead0);
                //    Console.WriteLine("Response from server: " + response0);
                //    Thread.Sleep(1);
                //    //配置触发模式为外触发
                //    string command_ext = ":TRIG:SOUR EXT\n";
                //    byte[] data_ext = Encoding.ASCII.GetBytes(command_ext);
                //    stream.Write(data_ext, 0, data_ext.Length);
                //    Thread.Sleep(1);

                //    //查询当前触发模式
                //    string command1 = ":TRIG:SOUR?\n";
                //    byte[] data1 = Encoding.ASCII.GetBytes(command1);
                //    stream.Write(data1, 0, data1.Length);

                //    byte[] responseData1 = new byte[2048];
                //    int bytesRead1 = stream.Read(responseData1, 0, responseData1.Length);
                //    string response1 = Encoding.ASCII.GetString(responseData1, 0, bytesRead1);
                //    Console.WriteLine("Response from server: " + response1);

                //    //开始外触发
                //    string command = "READ:WL:EXTernalStart\n";
                //    byte[] data = Encoding.ASCII.GetBytes(command);
                //    stream.Write(data, 0, data.Length);


                gain.Reset();

                var name = gain.BuildTermialName();
                gain.SetDefaultTermialName(name);
                p6683h.TrigTerminalsStart(name);

                EthernetChassis ethernet = new EthernetChassis("", "192.168.170.50:8807", true);
                ethernet.Initialize();
                FWM8612 fWM = new FWM8612("bcj", "0", ethernet);

                fWM.TurnOnline(true);
                fWM.Initialize();
                string idn = fWM.InstrumentIDN;
                fWM.SetTriggerSource(Source.SINGle);
                var b = fWM.GetTriggerSource();
                fWM.EXTernalStop();
                Thread.Sleep(50);
                fWM.SetTriggerSource(Source.EXTernal);
                var a = fWM.GetTriggerSource();
                Thread.Sleep(50);//Thread.Sleep(1);
                //fWM.EXTernalStart2();      //Thread.Sleep(1);
                Thread.Sleep(50);


                //这里发脉冲
                try
                {

                    float I_Start_mA = 0;
                    float I_Step_mA = 0.1f;
                    float I_Stop_mA = 19.9f;
                    float complianceVoltage_V = 2.5f;
                    float ApertureTime_s = 0.001f;
                    int sweepPoints = Convert.ToInt32(
                        (
                         I_Stop_mA -
                         I_Start_mA)
                        /
                         I_Step_mA) + 1;
                    //fWM.RST();
                    //fWM.SetTriggerSource(Source.EXTernal);

                    //var tMode = fWM.GetTriggerSource();


                    //gain.SetupMaster_Sequence_SourceCurrent_SenseVoltage
                    //(
                    //I_Start_mA,
                    //I_Step_mA,
                    //I_Stop_mA,
                    //complianceVoltage_V,
                    //ApertureTime_s,
                    //true
                    //);


                    Merged_PXIe_4143.ConfigureMultiChannelSynchronization(gain, null);

                    fWM.EXTernalStart2();      //Thread.Sleep(1);

                    Merged_PXIe_4143.Trigger(gain, null);

                    var resut = gain.Fetch_MeasureVals(sweepPoints, 10 * 1000.0);
                    double[] curr = resut.CurrentMeasurements;//pXISource.Fetch_SenseCurrents(sweepPoints, 10 * 1000.0);
                    Console.WriteLine("curr count: " + curr.Length);
                    //var dataBook = fWM.EXTernalStart();
                    //Thread.Sleep(5000);
                    //var respon = fWM.FethEXTernalData();
                    //var response111 = respon.Trim(';');
                    //var resut111 = response111.Split(';');
                }
                catch
                {

                }
                finally
                {
                    fWM.EXTernalStop();

                    p6683h.TrigTerminalsStop(name);
                    //Thread.Sleep(500);
                    gain.Reset();
                    gain.ClearTriggerTermialName();
                }

                //Thread.Sleep(3000);
                //var respon = fWM.debug();
                //var response111 = respon.Trim(';');
                //var resut111 = response111.Split(';');

                //fWM.EXTernalStop();
                return;




                //    byte[] responseData = new byte[28000];
                //    string response = "";
                //    bool stopRequested = false;
                //    int count = 0;
                //    while (!stopRequested)
                //    {
                //        try
                //        {
                //            int bytesRead = stream.Read(responseData, 0, responseData.Length);
                //            response = Encoding.ASCII.GetString(responseData, 0, bytesRead);
                //            Console.WriteLine("Response from server: " + response);
                //            response = response.Trim(';');
                //            var resut = response.Split(';');
                //            var resutlist = resut.ToList();
                //            resutlist.RemoveAt(resutlist.Count - 1);
                //            count += resutlist.Count;
                //            Console.WriteLine("count: " + count);
                //        }
                //        catch (Exception ex)
                //        {

                //        }

                //    }

                //    //client.Close();
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine("Error: " + e);
                //}



                //return;




                //string serverIp = "192.168.170.50";
                //int port = 8807;
                //TcpClient client = new TcpClient(serverIp, port);
                //Console.WriteLine("Connected to the server.");

                //NetworkStream stream = client.GetStream();




                //PXIe_NIDCPower_Chassis gainchas = new PXIe_NIDCPower_Chassis("", "PXI1Slot5", true);
                //gainchas.Initialize();
                //PXISourceMeter_4143 gain = new PXISourceMeter_4143("", "0", gainchas);
                //gain.TurnOnline(true);
                //gain.Initialize();

                //PXIe_PxiSession_Chassis chas = new PXIe_PxiSession_Chassis("", "PXI7::13::INSTR", true);
                //chas.Initialize();

                //PXISourceMeter_6683H p6683h = new PXISourceMeter_6683H("", "0", chas);
                //p6683h.TurnOnline(true);

                //gain.Reset();
                //var name = gain.BuildTermialName();
                //gain.SetDefaultTermialName(name);

                //p6683h.TrigTerminalsStart(name);
                //try
                //{

                //    float I_Start_mA = 0.1F;
                //    float I_Step_mA = 0.1f;
                //    float I_Stop_mA = 20F;
                //    float complianceVoltage_V = 2.5f;
                //    float ApertureTime_s = 0.001f;
                //    int sweepPoints = Convert.ToInt32(
                //        (
                //         I_Stop_mA -
                //         I_Start_mA)
                //        /
                //         I_Step_mA) + 1;



                //    gain.SetupMaster_Sequence_SourceCurrent_SenseVoltage
                //        (
                //        I_Start_mA,
                //        I_Step_mA,
                //        I_Stop_mA,
                //        complianceVoltage_V,
                //        ApertureTime_s,
                //        true
                //        );


                //    Merged_PXIe_4143.ConfigureMultiChannelSynchronization(gain, null);


                //    //string command_ext = ":TRIG:SOUR EXT\n";
                //    //byte[] data_ext = Encoding.ASCII.GetBytes(command_ext);
                //    //stream.Write(data_ext, 0, data_ext.Length);
                //    //Thread.Sleep(1);
                //    //string command = "READ:WL:EXTernalStart\n";
                //    //byte[] data = Encoding.ASCII.GetBytes(command);
                //    //stream.Write(data, 0, data.Length);



                //    Merged_PXIe_4143.Trigger(gain, null);

                //    var resut = gain.Fetch_MeasureVals(sweepPoints, 10 * 1000.0);
                //    double[] curr = resut.CurrentMeasurements;//pXISource.Fetch_SenseCurrents(sweepPoints, 10 * 1000.0);

                //    //byte[] responseData = new byte[28000];
                //    //string response = "";
                //    //bool stopRequested = false;
                //    //int count = 0;
                //    //while (!stopRequested)
                //    //{
                //    //    try
                //    //    {
                //    //        Thread.Sleep(100);
                //    //        int bytesRead = stream.Read(responseData, 0, responseData.Length);
                //    //        response = Encoding.ASCII.GetString(responseData, 0, bytesRead);
                //    //        Console.WriteLine("Response from server: " + response);
                //    //        response = response.Trim(';');
                //    //        var resut1 = response.Split(';');
                //    //        var resutlist = resut1.ToList();
                //    //        resutlist.RemoveAt(resutlist.Count - 1);
                //    //        count += resutlist.Count;
                //    //        Console.WriteLine("count: " + count);
                //    //    }
                //    //    catch (Exception ex)
                //    //    {

                //    //    }

                //    //}

                //    //client.Close();
                //}
                //catch
                //{

                //}
                //finally
                //{

                //    p6683h.TrigTerminalsStop(name);
                //}
            }
            catch (Exception ex)
            {

            }

            return;

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Instrument_DebugForm debugForm = new Instrument_DebugForm();
            //debugForm.Show();
            //Application.Run(debugForm);
            //Environment.Exit(0);
        }
    }
}
