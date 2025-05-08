using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class ExecutorConfigItem
    {
        public static string REQ_STATIC_RESOURCE_LIST_NODE_NAME = "[静态资源]列表";
        public static string REQ_INSTR_LIST_NODE_NAME = "[测试项]所需<仪器>列表";
        public static string REQ_AXIS_LIST_NODE_NAME = "[测试项]所需<轴>列表";
        public static string REQ_POS_LIST_NODE_NAME = "[测试项]所需<运动位置>列表";
        public static string USER_INSTR_LIST_NODE_NAME = "[用户配置]<仪器>列表";
        public static string USER_AXIS_LIST_NODE_NAME = "[用户配置]<轴>列表";
        public static string USER_POS_LIST_NODE_NAME = "[用户配置]<运动位置>列表";
        public ExecutorConfigItem()
        {
            this.UserDefineInstrumentConfig = new DataBook<string, string>();
            this.UserDefinePositionConfig = new DataBook<string, string>();
            this.UserDefineAxisConfig = new DataBook<string, string>();
            this.CalculatorCollection = new DataBook<string, string>();
        }
        public string TestExecutorName { get; set; }
        public string TestModuleClassName { get; set; }
        public string TestRecipeFileName { get; set; }
        public DataBook<string, string> UserDefineInstrumentConfig { get; set; }
        public DataBook<string, string> UserDefineAxisConfig { get; set; }
        public DataBook<string, string> UserDefinePositionConfig { get; set; }
        public DataBook<string, string> CalculatorCollection { get; set; }
        public virtual bool Check(ref string checkLog)
        {
            bool isOk = true;
            if (string.IsNullOrEmpty(TestExecutorName))
            {
                isOk = false;
                checkLog += $"测试项未命名\r\n";
            }
            if (string.IsNullOrEmpty(TestModuleClassName))
            {
                isOk = false;
                checkLog += $"未指定测试模块\r\n";
            }
            if (string.IsNullOrEmpty(TestRecipeFileName))
            {
                isOk = false;
                checkLog += $"未指定测试Recipe文件\r\n";
            }
            for (int calcIndex = 0; calcIndex <  this.CalculatorCollection.Count; calcIndex++)
            {
                if (string.IsNullOrEmpty(this.CalculatorCollection.Book[calcIndex].Key))
                {
                    isOk = false;
                    checkLog += $"未指定算子索引[{calcIndex}] 上的算子类型\r\n";
                }
                if (string.IsNullOrEmpty(this.CalculatorCollection.Book[calcIndex].Value))
                {
                    isOk = false;
                    checkLog += $"未指定算子索引[{calcIndex}] 上的算子条件\r\n";
                }
            }
            return isOk;
        }
        public void Clear()
        {
            this.TestExecutorName = string.Empty;
            this.TestModuleClassName = string.Empty;
            this.TestRecipeFileName = string.Empty;
            this.CalculatorCollection.Clear();
        }
        public virtual void Save(string path)
        {
            XmlHelper.SerializeFile<ExecutorConfigItem>(path, this);
        }
    }
}