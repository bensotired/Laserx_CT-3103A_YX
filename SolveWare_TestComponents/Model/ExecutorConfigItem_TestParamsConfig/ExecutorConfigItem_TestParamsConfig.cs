using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SolveWare_TestComponents.Data
{
   [Serializable]
    public class ExecutorConfigItem_TestParamsConfig
    {
        public string Name { get; set; }
        public List<string> IncludingTypes { get; set; }
        public TestRecipeBase TestRecipe { get; set; }
        public DataBook<string, CalcRecipe> CalcRecipeBook { get; set; }
        public ExecutorConfigItem_TestParamsConfig()
        {
            CalcRecipeBook = new DataBook<string, CalcRecipe>();
            IncludingTypes = new List<string>();
        }
        public List<string> GetCalculatorParamNames()
        {
            List<string> names = new List<string>();
            foreach (var kvp in this.CalcRecipeBook)
            {
                names.Add(kvp.Key);
            }
            return names;
        }
        public bool Decode(ITesterAssembly core, ExecutorConfigItem exeItemObj)
        {
            try
            {
                Name = exeItemObj.TestExecutorName;
                TestRecipe = (TestRecipeBase)core.CreateExeItem_TestRecipeInstance(exeItemObj);
                exeItemObj.TestRecipeFileName = TestRecipe.GetType().Name;

                DataBook<string, object> tempBook = core.CreateExeItem_CalcRecipeInstances(exeItemObj);

                foreach (var item in tempBook)
                {
                    var calcRcp = item.Value as CalcRecipe;
                    calcRcp.CalcData_Rename = item.Key;
                    CalcRecipeBook.Add(item.Key, calcRcp);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public virtual Type[] GetIncludingTypes()
        {
            List<Type> includingTypes = new List<Type>();

            includingTypes.Add(this.TestRecipe.GetType());

            foreach (var kvp in this.CalcRecipeBook)
            {
                if (includingTypes.Contains(kvp.Value.GetType()) == false)
                {
                    includingTypes.Add(kvp.Value.GetType());
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