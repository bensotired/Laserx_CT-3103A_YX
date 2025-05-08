using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Specification;
using System.IO;

namespace SolveWare_TestSpecification
{
    public class TestSpecManagerConfig : ITesterAppConfig
    {
        static TestSpecManagerConfig _instance;
        static object _mutex = new object();
        public static TestSpecManagerConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_mutex)
                    {
                        if (_instance == null)
                        {
                            _instance = new TestSpecManagerConfig();
                        }
                    }
                }
                return _instance;
            }
        }
        //规格文件读取不到，才会调用该方法
        //创建文件，然后在读取
        public void CreateDefaultInstance()
        {
            _instance = new TestSpecManagerConfig();
            _instance.TestSpecCollection = new TestSpecCollection();

            TestSpecification newSpec = new TestSpecification();
            newSpec.Name = "SampleSpec_1";
            newSpec.Version = "2022.01.01"; 

            for(int specIndex = 0; specIndex < 5; specIndex++)
            {
                newSpec.AddSingleItem(new TestSpecificationItem
                {
                    Max = "99",
                    Min = "0",
                    //IsCirtcal = true,
                    Name = $"SampleSpecItem_{specIndex}",
                    FailureCode = $"FC_{specIndex:0000}",
                    Unit = "NA",
                    DataType = SpecDataType.Double,
                });
            }
      
            _instance.TestSpecCollection.AddSingleItem(newSpec);
            //const int defaultMasterId = 1;
            //const int defaultAxesCount = 10;

            //string errMsg = string.Empty;

            //for (short axisNo = 1; axisNo <= defaultAxesCount; axisNo++)
            //{
            //    var motorSettingItem = new MotorSetting();
            //    motorSettingItem.MotorTable.AxisNo = axisNo;
            //    motorSettingItem.MotorTable.CardNo = defaultMasterId;
            //    motorSettingItem.Name = $"AXIS_{axisNo}";
            //    _instance.MotorGeneralSetting.AddSingleItem(motorSettingItem, ref errMsg);
            //}
        }
 


        public TestSpecCollection TestSpecCollection { get; set; }

         
        //加载
        public void Load(string configFile)
        {

            if (File.Exists(configFile) == false)
            {
                if (Directory.Exists(Path.GetDirectoryName(configFile)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(configFile));
                }
                this.CreateDefaultInstance();
                //写入
                XmlHelper.SerializeFile<TestSpecManagerConfig>(configFile, _instance);
            }
            else
            {
                //读取
                _instance = XmlHelper.DeserializeFile<TestSpecManagerConfig>(configFile);
            }
        }

        //保存
        public void Save(string configFile)
        {
            if (File.Exists(configFile) == false)
            {//文件不存在
                if (Directory.Exists(Path.GetDirectoryName(configFile)) == false)
                {//目录不存在
                    Directory.CreateDirectory(Path.GetDirectoryName(configFile));
                }
                this.CreateDefaultInstance();
            }
            //写入
            XmlHelper.SerializeFile<TestSpecManagerConfig>(configFile, _instance);
        }
    }

    //    public class TestSpecCollection : CURDBase<TestSpecification>//, IEnumerable<MotorSetting>
    //{
    //    public TestSpecCollection()
    //    {

    //    }
    //}
}
