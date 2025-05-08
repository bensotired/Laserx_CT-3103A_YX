using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LX_BurnInSolution.Utilities;
using SolveWare_TestComponents.Data;

namespace SolveWare_TestComponents
{
    static class Program
    {
  
        static void Main()
        {
            try
            {
                //string path = @"D:\tfs_tp\开发_BT-0632\LaserX_TesterLibrary\Playground\rawdata.xml";
                //RawDataBaseLite rd = new RawDataBaseLite();
                //rd.RawDataFixFormat = "heello{0}1123";
                //rd.Tag = "abc";

                //XmlHelper.SerializeFile<RawDataBaseLite>(path, rd);


                //string path1 = @"D:\tfs_tp\开发_BT-0632\LaserX_TesterLibrary\Playground\rawdata_col.xml";
                //RawDataCollectionSample rdCol = new RawDataCollectionSample();
                //rdCol.Add(new RawDatumItemSample() { ItemData_A = 11.0 });
                //rdCol.RawDataFixFormat = "col_{0}_sample";
                //rdCol.Tag = "liv";

                //XmlHelper.SerializeFile<RawDataCollectionSample>(path1, rdCol);

                //string path2 = @"D:\tfs_tp\开发_BT-0632\LaserX_TesterLibrary\Playground\rawdata_sig.xml";
                //RawDataSingleSample rdSig = new RawDataSingleSample();
                //rdSig.Single_A = 111;
                //rdSig.Single_B = 222;
                //rdSig.RawDataFixFormat = "sig_{0}_sample";
                //rdSig.Tag = "liv";

                //XmlHelper.SerializeFile<RawDataSingleSample>(path2, rdSig);


                //string path3 = @"D:\tfs_tp\开发_BT-0632\LaserX_TesterLibrary\Playground\rawdata_cplx.xml";
                //RawDataComplexSample rdCplx = new RawDataComplexSample();

                //rdCplx.Add(new RawDatumItemSample() { ItemData_A = 11.0 });
                //rdCplx.RawDataFixFormat = "cplx_{0}_sample";
                //rdCplx.Tag = "liv";
                //rdCplx.Single_A = 7777.0;

                //XmlHelper.SerializeFile<RawDataComplexSample>(path3, rdCplx);
                //string path4 = @"D:\tfs_tp\开发_BT-0632\LaserX_TesterLibrary\Playground\rawdata_td.xml";
                //DeviceStreamDataBase td = new DeviceStreamDataBase();
                //td.AddRawData(rdCol);
                //td.AddRawData(rdSig);
                //td.AddRawData(rdCplx);

                //XmlHelper.SerializeFile<DeviceStreamDataBase>(path4, td );

                

                //var deserialData = XmlHelper.DeserializeFile<DeviceStreamDataBase>(path4 );



            }
            catch (Exception ex)
            {


            }


        }
    }
}
