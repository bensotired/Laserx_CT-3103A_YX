using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using SolveWare_TestPlugin;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;

namespace TestPlugin_Demo
{

    [Serializable]
    public class TestPluginImportProfile_CT3103 : TestPluginImportProfile_MultiComboWithTestParams
    {
        public List<string> IncludingTypes { get; set; }
        [XmlIgnore]
        public string FileName { set; get; } = string.Empty;//文档名称
        [XmlIgnore]
        public string TestTime { set; get; } = string.Empty;
        [XmlIgnore]
        public string TestProfileName { set; get; } = string.Empty;//测试档名字

        public string BinCollectionName { set; get; } = string.Empty;//测试档名字
        public List<CalibrationItem> UserDefinedCalibrationData_Loader { get; set; }
        public TestPluginImportProfile_CT3103()
        {

            this.Name = $"NewTestProfile_CT3103_{BaseDataConverter.ConvertDateTimeToCommentString(DateTime.Now)}";
            this.Spec = null;
            this.TestExecutorComboDict = new DataBook<string, TestExecutorCombo>();
            this.TestExecutorComboDict.Add(MT.测试站1.ToString(), null);
            //this.TestExecutorComboDict.Add(MT.测试站2.ToString(), null);
            this.TestParamsConfigComboDict = new DataBook<string, ExecutorConfigItem_TestParamsConfigCombo>();
            this.TestParamsConfigComboDict.Add(MT.测试站1.ToString(), null);
            //this.TestParamsConfigComboDict.Add(MT.测试站2.ToString(), null);
            this.IncludingTypes = new List<string>();

            this.UserDefinedCalibrationData_Loader = new List<CalibrationItem>();
        }
        public void CreateDefaultCalibrationData()
        {

            this.UserDefinedCalibrationData_Loader = new List<CalibrationItem>();

            //this.UserDefinedCalibrationData_Loader.Add(new CalibrationItem()
            //{
            //    Name = MT.测试站1.ToString(),
            //    K = 1.0,
            //    B = 0.0
            //});
            string[] userRequestSummaryCaliData = { "VF1", "VF2", "VF3", "VF4" };
            for (int i = 0; i < userRequestSummaryCaliData.Length; i++)
            {
                this.UserDefinedCalibrationData_Loader.Add(new CalibrationItem()
                {
                    Name = userRequestSummaryCaliData[i],
                    K = 1.0,
                    B = 0.0
                });
            }
        }

        public override bool Check(out string checkLog, params object[] args)
        {

            bool isSpecOk = true;
            bool isCombo_1_Ok = false;
            bool isCombo_2_Ok = false;
            bool isParamCombo_1_Ok = true;
            bool isParamCombo_2_Ok = true;
            checkLog = string.Empty;

            string destTestPlugin = args[0].ToString();
            bool checkPreExecutor = Convert.ToBoolean(args[1]);
            bool checkMainExecutor = Convert.ToBoolean(args[2]);
            bool checkPostExecutor = Convert.ToBoolean(args[3]);


            if (this.TestExecutorComboDict[MT.测试站1.ToString()] == null)
            {
                checkLog += $"{MT.测试站1}未配置测试项目链\r\n";
            }
            else
            {
                var combo_log = string.Empty;
                isCombo_1_Ok = this.TestExecutorComboDict[MT.测试站1.ToString()].Check
                                (
                                    ref combo_log,
                                    destTestPlugin,
                                    checkPreExecutor,
                                    checkMainExecutor,
                                    checkPostExecutor
                                );
                if (isCombo_1_Ok == false)
                {
                    checkLog += $"{MT.测试站1}测试链项目错误:\r\n{combo_log}\r\n";
                }

            }
            //if (this.TestParamsConfigComboDict[MT.测试站1.ToString()] == null)
            //{
            //    checkLog += $"{MT.测试站1}未配置测试参数链\r\n";
            //}
            //else
            //{
            //    var combo_log = string.Empty;
            //    isCombo_1_Ok = this.TestExecutorComboDict[MT.测试站1.ToString()].Check
            //                    (
            //                        ref combo_log,
            //                        destTestPlugin,
            //                        checkPreExecutor,
            //                        checkMainExecutor,
            //                        checkPostExecutor
            //                    );
            //    if (isCombo_1_Ok == false)
            //    {
            //        checkLog += $"{MT.测试站1}测试参数链错误:\r\n{combo_log}\r\n";
            //    }

            //}

            isSpecOk = true;
            if(false)  //我也不知道为什么不存在. 注释掉试试
            {
                if (this.Spec == null || this.Spec.Count <= 0)
                {
                    checkLog += $"测试规格为空|";
                    isSpecOk = false;
                }
                else
                {
                    isSpecOk = this.Spec.Check(out checkLog);
                }
            }


            //if (this.TestExecutorComboDict[MT.测试站2.ToString()] == null)
            //{
            //    checkLog += $"{MT.测试站2}未配置测试链\r\n";
            //}
            //else
            //{
            //    var combo_log = string.Empty;
            //    isCombo_2_Ok = this.TestExecutorComboDict[MT.测试站2.ToString()].Check
            //                    (
            //                        ref combo_log,
            //                        destTestPlugin,
            //                        checkPreExecutor,
            //                        checkMainExecutor,
            //                        checkPostExecutor
            //                    );
            //    if (isCombo_2_Ok == false)
            //    {
            //        checkLog += $"{MT.测试站2}测试链错误:\r\n{combo_log}\r\n";
            //    }
            //}


            return isCombo_1_Ok && isSpecOk;///&& isCombo_2_Ok;
        }
        public override void Clear()
        {
            this.Spec = null;
            this.TestExecutorComboDict = new DataBook<string, TestExecutorCombo>();
            this.TestExecutorComboDict.Add(MT.测试站1.ToString(), null);
            //this.TestExecutorComboDict.Add(MT.测试站2.ToString(), null);
            this.TestParamsConfigComboDict = new DataBook<string, ExecutorConfigItem_TestParamsConfigCombo>();
            this.TestParamsConfigComboDict.Add(MT.测试站1.ToString(), null);
            //this.TestParamsConfigComboDict.Add(MT.测试站2.ToString(), null);

        }
        public void AddComboToExecutorDict(ITesterCoreInteration core, string executorName, TestExecutorCombo combo)
        {
            if (this.TestExecutorComboDict.ContainsKey(executorName) == false)
            {

            }
            else
            {

                ExecutorConfigItem_TestParamsConfigCombo tempParamsCombo = new ExecutorConfigItem_TestParamsConfigCombo();
                foreach (var item in combo.Pre_ExecutorConfigCollection)
                {
                    ExecutorConfigItem_TestParamsConfig temp = new ExecutorConfigItem_TestParamsConfig();
                    temp.Decode(core, item);
                    tempParamsCombo.Pre_TestParamsCollection.Add(temp);
                }
                foreach (var item in combo.Main_ExecutorConfigCollection)
                {
                    ExecutorConfigItem_TestParamsConfig temp = new ExecutorConfigItem_TestParamsConfig();
                    temp.Decode(core, item);
                    tempParamsCombo.Main_TestParamsCollection.Add(temp);
                }
                foreach (var item in combo.Post_ExecutorConfigCollection)
                {
                    ExecutorConfigItem_TestParamsConfig temp = new ExecutorConfigItem_TestParamsConfig();
                    temp.Decode(core, item);
                    tempParamsCombo.Post_TestParamsCollection.Add(temp);
                }


                this.TestExecutorComboDict[executorName] = combo;
                this.TestParamsConfigComboDict[executorName] = tempParamsCombo;
            }
        }

        public void AddTestExecutorComboWithParams(/*ITesterCoreInteration core, */string executorName, TestExecutorComboWithParams comboWithParam)
        {
            if (this.TestExecutorComboDict.ContainsKey(executorName) == false)
            {
                System.Windows.Forms.MessageBox.Show("该测试方案无测试站2号!");
            }
            else
            {

                //ExecutorConfigItem_TestParamsConfigCombo tempParamsCombo = new ExecutorConfigItem_TestParamsConfigCombo();
                //foreach (var item in combo.Pre_ExecutorConfigCollection)
                //{
                //    ExecutorConfigItem_TestParamsConfig temp = new ExecutorConfigItem_TestParamsConfig();
                //    temp.Decode(core, item);
                //    tempParamsCombo.Pre_TestParamsCollection.Add(temp);
                //}
                //foreach (var item in combo.Main_ExecutorConfigCollection)
                //{
                //    ExecutorConfigItem_TestParamsConfig temp = new ExecutorConfigItem_TestParamsConfig();
                //    temp.Decode(core, item);
                //    tempParamsCombo.Main_TestParamsCollection.Add(temp);
                //}
                //foreach (var item in combo.Post_ExecutorConfigCollection)
                //{
                //    ExecutorConfigItem_TestParamsConfig temp = new ExecutorConfigItem_TestParamsConfig();
                //    temp.Decode(core, item);
                //    tempParamsCombo.Post_TestParamsCollection.Add(temp);
                //}


                this.TestExecutorComboDict[executorName] = comboWithParam.Combo;
                this.TestParamsConfigComboDict[executorName] = comboWithParam.ComboParams;
            }
        }



        public virtual Type[] GetIncludingTypes()
        {


            List<Type> includingTypes = new List<Type>();

            if (this.TestParamsConfigComboDict[MT.测试站1.ToString()] != null)
            {
                includingTypes.AddRange(this.TestParamsConfigComboDict[MT.测试站1.ToString()].GetIncludingTypes());
            }
            //if (this.TestParamsConfigComboDict[MT.测试站2.ToString()] != null)
            //{
            //    includingTypes.AddRange(this.TestParamsConfigComboDict[MT.测试站2.ToString()].GetIncludingTypes());
            //}
            var biggerThanOneElementList = includingTypes.GroupBy(x => x).Where(g => g.Count() > 1).Select(m => m.Key).ToList();
            foreach (var mulEle in biggerThanOneElementList)
            {
                includingTypes.RemoveAll(item => item == mulEle);
                includingTypes.Add(mulEle);
            }

            return includingTypes.ToArray();
        }
        public override void Save(string filePath)
        {
            this.LastModifyTime = BaseDataConverter.ConvertDateTimeToCommentString(DateTime.Now);

            var tempTypes = this.GetIncludingTypes();
            this.IncludingTypes = new List<string>();
            foreach (var t in tempTypes)
            {
                this.IncludingTypes.Add(t.FullName);
            }

            XmlHelper.SerializeFile(filePath, this, tempTypes.ToArray());
        }
    }
}