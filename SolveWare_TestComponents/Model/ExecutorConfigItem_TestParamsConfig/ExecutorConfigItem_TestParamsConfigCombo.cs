using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SolveWare_TestComponents.Data
{
    public class ExecutorConfigItem_TestParamsConfigCombo
    {
        public List<string> IncludingTypes { get; set; }
        public List<ExecutorConfigItem_TestParamsConfig> Pre_TestParamsCollection { get; set; }
        public List<ExecutorConfigItem_TestParamsConfig> Main_TestParamsCollection { get; set; }
        public List<ExecutorConfigItem_TestParamsConfig> Post_TestParamsCollection { get; set; }
        public ExecutorConfigItem_TestParamsConfigCombo()
        {
            IncludingTypes = new List<string>();
            Pre_TestParamsCollection = new List<ExecutorConfigItem_TestParamsConfig>();
            Main_TestParamsCollection = new List<ExecutorConfigItem_TestParamsConfig>();
            Post_TestParamsCollection = new List<ExecutorConfigItem_TestParamsConfig>();
        }
        public virtual Type[] GetIncludingTypes()
        {
            List<Type> includingTypes = new List<Type>();

            foreach(var item in this.Pre_TestParamsCollection)
            {
                if (includingTypes.Contains(item.TestRecipe.GetType()) == false)
                {
                    includingTypes.Add(item.TestRecipe.GetType());
                }
                foreach (var kvp in item.CalcRecipeBook)
                {
                    if (includingTypes.Contains(kvp.Value.GetType()) == false)
                    {
                        includingTypes.Add(kvp.Value.GetType());
                    }
                }
            }
            foreach (var item in this.Main_TestParamsCollection)
            {
                if (includingTypes.Contains(item.TestRecipe.GetType()) == false)
                {
                    includingTypes.Add(item.TestRecipe.GetType());
                }
                foreach (var kvp in item.CalcRecipeBook)
                {
                    if (includingTypes.Contains(kvp.Value.GetType()) == false)
                    {
                        includingTypes.Add(kvp.Value.GetType());
                    }
                }
            }
            foreach (var item in this.Post_TestParamsCollection)
            {
                if (includingTypes.Contains(item.TestRecipe.GetType()) == false)
                {
                    includingTypes.Add(item.TestRecipe.GetType());
                }
                foreach (var kvp in item.CalcRecipeBook)
                {
                    if (includingTypes.Contains(kvp.Value.GetType()) == false)
                    {
                        includingTypes.Add(kvp.Value.GetType());
                    }
                }
            }

            return includingTypes.ToArray();
        }
        public virtual void Save(string filePath)
        {
            var types = GetIncludingTypes();
            this.IncludingTypes = new List<string>();
            foreach (var t in types)
            {
                this.IncludingTypes.Add(t.FullName);
            }
            XmlHelper.SerializeFile(filePath, this, GetIncludingTypes());
        }


        public static ExecutorConfigItem_TestParamsConfig Load(ITesterCoreInteration core, string path)
        {
            var temp = XElement.Load(path);
            var typesEle = temp.GetElement("IncludingTypes");
            List<string> typeNames = new List<string>();
            foreach (var node in typesEle.Nodes())
            {
                typeNames.Add(((XElement)node).Value);
            }

            var types = core.GetTypeFromClassInPreLoadDlls(typeNames);
            var dataObj = XmlHelper.DeserializeFile(path, typeof(ExecutorConfigItem_TestParamsConfig), types.ToArray()) as ExecutorConfigItem_TestParamsConfig;
            return dataObj;
        }
    }
}