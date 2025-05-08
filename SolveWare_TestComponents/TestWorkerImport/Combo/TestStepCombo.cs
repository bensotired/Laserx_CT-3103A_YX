using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;

namespace SolveWare_TestComponents.Data
{

    public class TestExecutorCombo
    {
        public static string INFO_NODE_NAME = "测试链信息";
        public static string PRE_COMBO_EXECUTOR_ROOT_NODE_NAME = "[前置]测试项信息";
        public static string MAIN_COMBO_EXECUTOR_ROOT_NODE_NAME = "[主要]测试项信息";
        public static string POST_COMBO_EXECUTOR_ROOT_NODE_NAME = "[后置]测试项信息";
        public virtual string Name { get; set; }
        public virtual string CreateTime { get; set; }
        public virtual string LastModifyTime { get; set; }
        public virtual string ProductType { get; set; }
        public List<ExecutorConfigItem> Pre_ExecutorConfigCollection { get; set; }
        public List<ExecutorConfigItem> Main_ExecutorConfigCollection { get; set; }
        public List<ExecutorConfigItem> Post_ExecutorConfigCollection { get; set; }
        public TestExecutorCombo()
        {
            this.Name = "TestExecutorCombo";
            this.ProductType = "NoUse";
            this.CreateTime = BaseDataConverter.ConvertDateTimeToCommentString(DateTime.Now);
            this.LastModifyTime = BaseDataConverter.ConvertDateTimeToCommentString(DateTime.Now);
            this.Pre_ExecutorConfigCollection = new List<ExecutorConfigItem>();
            this.Main_ExecutorConfigCollection = new List<ExecutorConfigItem>();
            this.Post_ExecutorConfigCollection = new List<ExecutorConfigItem>();
        }
    
        //public virtual string ApplicableTestPlugin { get; set; }
        public virtual bool Check(ref string checkLog, string destTestPlugin, bool checkPreExecutor, bool checkMainExecutor, bool checkPostExecutor)
        {
            bool isApplicableTestPlugin = true;

            bool isPreExecutorOk = true;
            bool isMainExecutorOk = true;
            bool isPostExecutorOk = true;

            //if (this.ApplicableTestPlugin.ToLower() != destTestPlugin.ToLower())
            //{
            //    checkLog += $"测试项目链适用插件为[{ this.ApplicableTestPlugin}]与当前所需插件类型[{destTestPlugin}]不一致\r\n";
            //    isApplicableTestPlugin = false;
            //}

            if (checkPreExecutor == true)
            {
                if (this.Pre_ExecutorConfigCollection.Count <= 0)
                {
                    isPreExecutorOk = false;
                    checkLog += $"测试项目链未配置任何 [前置]测试项\r\n";
                }
                else
                {
                    List<bool> temp = new List<bool>();
                    string checkExeLog = string.Empty;
                    for (int i = 0; i < this.Pre_ExecutorConfigCollection.Count; i++)
                    {
                        checkExeLog = string.Empty;
                        var isOk = this.Pre_ExecutorConfigCollection[i].Check(ref checkExeLog);

                        temp.Add(isOk);
                        if (isOk == false)
                        {
                            checkExeLog = $"[前置]测试项 索引[{i}] {checkExeLog}";
                            checkLog += checkExeLog;
                        }
                    }
                    isPreExecutorOk = !temp.Contains(false);
                }
            }

            if (checkMainExecutor == true)
            {
                if (this.Main_ExecutorConfigCollection.Count <= 0)
                {
                    isMainExecutorOk = false;
                    checkLog += $"测试项目链未配置任何 [主要]测试项\r\n";
                }
                else
                {
                    List<bool> temp = new List<bool>();
                    string checkExeLog = string.Empty;
                    for (int i = 0; i < this.Main_ExecutorConfigCollection.Count; i++)
                    {
                        checkExeLog = string.Empty;
                        var isOk = this.Main_ExecutorConfigCollection[i].Check(ref checkExeLog);

                        temp.Add(isOk);
                        if (isOk == false)
                        {
                            checkExeLog = $"[主要]测试项 索引[{i}] {checkExeLog}";
                            checkLog += checkExeLog;
                        }
                    }
                 
                    isMainExecutorOk = !temp.Contains(false);
                }
            }

            if (checkPostExecutor == true)
            {
                if (this.Post_ExecutorConfigCollection.Count <= 0)
                {
                    checkLog += $"测试项目链未配置任何 [后置]测试项\r\n";
                }
                else
                {
                    List<bool> temp = new List<bool>();
                    string checkExeLog = string.Empty;
                    for (int i = 0; i < this.Post_ExecutorConfigCollection.Count; i++)
                    {
                        checkExeLog = string.Empty;
                        var isOk = this.Post_ExecutorConfigCollection[i].Check(ref checkExeLog);

                        temp.Add(isOk);
                        if (isOk == false)
                        {
                            checkExeLog = $"[后置]测试项 索引[{i}] {checkExeLog}";
                            checkLog += checkExeLog;
                        }
                    }
                    isPostExecutorOk = !temp.Contains(false);
                }
            }

            return  isApplicableTestPlugin &&
                    isPreExecutorOk &&
                    isPostExecutorOk &&
                    isMainExecutorOk;
        }
     
        public virtual void Save(string path)
        {
            this.LastModifyTime = BaseDataConverter.ConvertDateTimeToCommentString(DateTime.Now);
            XmlHelper.SerializeFile<TestExecutorCombo>(path, this);
        }
    }
}