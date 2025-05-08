using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SolveWare_TestComponents.Data
{

    public class TestExecutorComboWithParams
    {
        public virtual string Name { get; set; }
        public List<string> IncludingTypes { get; set; }
        public TestExecutorCombo Combo { get; set; }
        public ExecutorConfigItem_TestParamsConfigCombo ComboParams { get; set; }
        public TestExecutorComboWithParams()
        {
            Name = string.Empty;
            IncludingTypes = new List<string>();
            Combo = new TestExecutorCombo();
            ComboParams = new ExecutorConfigItem_TestParamsConfigCombo();
        }
        public void AddExecutorConfigItemTo_Pre_List(ITesterCoreInteration core, ExecutorConfigItem eci)
        {
            ExecutorConfigItem_TestParamsConfig eci_param = new ExecutorConfigItem_TestParamsConfig();
            eci_param.Decode(core, eci);

            this.Combo.Pre_ExecutorConfigCollection.Add(eci);
            this.ComboParams.Pre_TestParamsCollection.Add(eci_param);
        }
        public void AddExecutorConfigItemTo_Main_List(ITesterCoreInteration core, ExecutorConfigItem eci)
        {

            ExecutorConfigItem_TestParamsConfig eci_param = new ExecutorConfigItem_TestParamsConfig();
            eci_param.Decode(core, eci);

            this.Combo.Main_ExecutorConfigCollection.Add(eci);
            this.ComboParams.Main_TestParamsCollection.Add(eci_param);
        }
        public void AddExecutorConfigItemTo_Post_List(ITesterCoreInteration core, ExecutorConfigItem eci)
        {
            ExecutorConfigItem_TestParamsConfig eci_param = new ExecutorConfigItem_TestParamsConfig();
            eci_param.Decode(core, eci);
            this.Combo.Post_ExecutorConfigCollection.Add(eci);
            this.ComboParams.Post_TestParamsCollection.Add(eci_param);
        }
        public void RemoveCalculatorAt_ExecutorConfigItem_Of_Pre_List(int eciIndex, int destCalcIndex )
        {
            this.Combo.Pre_ExecutorConfigCollection[eciIndex].CalculatorCollection.RemoveAtIndex(destCalcIndex);
            this.ComboParams.Pre_TestParamsCollection[eciIndex].CalcRecipeBook.RemoveAtIndex(destCalcIndex);
        }
        public void RemoveCalculatorAt_ExecutorConfigItem_Of_Main_List(int eciIndex, int destCalcIndex)
        {
            this.Combo.Main_ExecutorConfigCollection[eciIndex].CalculatorCollection.RemoveAtIndex(destCalcIndex);
            this.ComboParams.Main_TestParamsCollection[eciIndex].CalcRecipeBook.RemoveAtIndex(destCalcIndex);
        }
        public void RemoveCalculatorAt_ExecutorConfigItem_Of_Post_List(int eciIndex, int destCalcIndex)
        {
            this.Combo.Post_ExecutorConfigCollection[eciIndex].CalculatorCollection.RemoveAtIndex(destCalcIndex);
            this.ComboParams.Post_TestParamsCollection[eciIndex].CalcRecipeBook.RemoveAtIndex(destCalcIndex);
        }

        public void InsertCalculatorInto_ExecutorConfigItem_Of_Pre_List(int eciIndex, int destCalcIndex, string calculatorTypeName, string calculatorName, CalcRecipe calcRecipeInstance)
        {
            this.Combo.Pre_ExecutorConfigCollection[eciIndex].CalculatorCollection.InsertItem(destCalcIndex, calculatorTypeName, calculatorName);
            this.ComboParams.Pre_TestParamsCollection[eciIndex].CalcRecipeBook.InsertItem(destCalcIndex, calculatorName, calcRecipeInstance);
        }
        public void InsertCalculatorInto_ExecutorConfigItem_Of_Main_List(int eciIndex, int destCalcIndex, string calculatorTypeName, string calculatorName, CalcRecipe calcRecipeInstance)
        {
            this.Combo.Main_ExecutorConfigCollection[eciIndex].CalculatorCollection.InsertItem(destCalcIndex, calculatorTypeName, calculatorName);
            this.ComboParams.Main_TestParamsCollection[eciIndex].CalcRecipeBook.InsertItem(destCalcIndex, calculatorName, calcRecipeInstance);
        }
        public void InsertCalculatorInto_ExecutorConfigItem_Of_Post_List(int eciIndex, int destCalcIndex, string calculatorTypeName, string calculatorName, CalcRecipe calcRecipeInstance)
        {
            this.Combo.Post_ExecutorConfigCollection[eciIndex].CalculatorCollection.InsertItem(destCalcIndex, calculatorTypeName, calculatorName);
            this.ComboParams.Post_TestParamsCollection[eciIndex].CalcRecipeBook.InsertItem(destCalcIndex, calculatorName, calcRecipeInstance);
        }

        //优化：插入算子功能
        public void InsertCalculatorInto_ExecutorConfigItem_Of_Pre_List_V2(int eciIndex, int destCalcIndex, string calculatorTypeName, string calculatorName, CalcRecipe calcRecipeInstance)
        {
            this.Combo.Pre_ExecutorConfigCollection[eciIndex].CalculatorCollection.InsertItem_V2(destCalcIndex, calculatorTypeName, calculatorName);
            this.ComboParams.Pre_TestParamsCollection[eciIndex].CalcRecipeBook.InsertItem_V2(destCalcIndex, calculatorName, calcRecipeInstance);
        }
        public void InsertCalculatorInto_ExecutorConfigItem_Of_Main_List_V2(int eciIndex, int destCalcIndex, string calculatorTypeName, string calculatorName, CalcRecipe calcRecipeInstance)
        {
            this.Combo.Main_ExecutorConfigCollection[eciIndex].CalculatorCollection.InsertItem_V2(destCalcIndex, calculatorTypeName, calculatorName);
            this.ComboParams.Main_TestParamsCollection[eciIndex].CalcRecipeBook.InsertItem_V2(destCalcIndex, calculatorName, calcRecipeInstance);
        }
        public void InsertCalculatorInto_ExecutorConfigItem_Of_Post_List_V2(int eciIndex, int destCalcIndex, string calculatorTypeName, string calculatorName, CalcRecipe calcRecipeInstance)
        {
            this.Combo.Post_ExecutorConfigCollection[eciIndex].CalculatorCollection.InsertItem_V2(destCalcIndex, calculatorTypeName, calculatorName);
            this.ComboParams.Post_TestParamsCollection[eciIndex].CalcRecipeBook.InsertItem_V2(destCalcIndex, calculatorName, calcRecipeInstance);
        }

        public void RemoveAt_ExecutorConfigItem_Of_Pre_List(int Index)
        {
            this.Combo.Pre_ExecutorConfigCollection.RemoveAt(Index);
            this.ComboParams.Pre_TestParamsCollection.RemoveAt(Index);
        }
        public void RemoveAt_ExecutorConfigItem_Of_Main_List(int Index)
        {
            this.Combo.Main_ExecutorConfigCollection.RemoveAt(Index);
            this.ComboParams.Main_TestParamsCollection.RemoveAt(Index);
        }
        public void RemoveAt_ExecutorConfigItem_Of_Post_List(int Index)
        {
            this.Combo.Post_ExecutorConfigCollection.RemoveAt(Index);
            this.ComboParams.Post_TestParamsCollection.RemoveAt(Index);
        }

        public void Switch_ExecutorConfigItem_Of_Pre_List(int sourceIndex, int destIndex)
        {

            var sourComboVal = this.Combo.Pre_ExecutorConfigCollection[sourceIndex];
            var destComboVal = this.Combo.Pre_ExecutorConfigCollection[destIndex];

            this.Combo.Pre_ExecutorConfigCollection[sourceIndex] = destComboVal;
            this.Combo.Pre_ExecutorConfigCollection[destIndex] = sourComboVal;

            var sourParamVal = this.ComboParams.Pre_TestParamsCollection[sourceIndex];
            var destParamVal = this.ComboParams.Pre_TestParamsCollection[destIndex];

            this.ComboParams.Pre_TestParamsCollection[sourceIndex] = destParamVal;
            this.ComboParams.Pre_TestParamsCollection[destIndex] = sourParamVal;

        }
        public void Switch_ExecutorConfigItem_Of_Main_List(int sourceIndex, int destIndex)
        {

            var sourComboVal = this.Combo.Main_ExecutorConfigCollection[sourceIndex];
            var destComboVal = this.Combo.Main_ExecutorConfigCollection[destIndex];

            this.Combo.Main_ExecutorConfigCollection[sourceIndex] = destComboVal;
            this.Combo.Main_ExecutorConfigCollection[destIndex] = sourComboVal;

            var sourParamVal = this.ComboParams.Main_TestParamsCollection[sourceIndex];
            var destParamVal = this.ComboParams.Main_TestParamsCollection[destIndex];

            this.ComboParams.Main_TestParamsCollection[sourceIndex] = destParamVal;
            this.ComboParams.Main_TestParamsCollection[destIndex] = sourParamVal;

        }

        public void Switch_ExecutorConfigItem_Of_Post_List(int sourceIndex, int destIndex)
        {

            var sourComboVal = this.Combo.Post_ExecutorConfigCollection[sourceIndex];
            var destComboVal = this.Combo.Post_ExecutorConfigCollection[destIndex];

            this.Combo.Post_ExecutorConfigCollection[sourceIndex] = destComboVal;
            this.Combo.Post_ExecutorConfigCollection[destIndex] = sourComboVal;

            var sourParamVal = this.ComboParams.Post_TestParamsCollection[sourceIndex];
            var destParamVal = this.ComboParams.Post_TestParamsCollection[destIndex];

            this.ComboParams.Post_TestParamsCollection[sourceIndex] = destParamVal;
            this.ComboParams.Post_TestParamsCollection[destIndex] = sourParamVal;
        }
 

        public virtual Type[] GetIncludingTypes()
        {
            List<Type> includingTypes = new List<Type>();
            includingTypes.AddRange(ComboParams.GetIncludingTypes());
            var biggerThanOneElementList = includingTypes.GroupBy(x => x).Where(g => g.Count() > 1).Select(m => m.Key).ToList();
            foreach (var mulEle in biggerThanOneElementList)
            {
                includingTypes.RemoveAll(item => item == mulEle);
                includingTypes.Add(mulEle);
            }

            return includingTypes.ToArray();
        }
        public string CheckDuplicateCalcParamName()
        {
            string dupCalcName = string.Empty;
            List<string> orgNameList = new List<string>();
            foreach (var testParams in this.ComboParams.Pre_TestParamsCollection)
            {
                orgNameList.AddRange(testParams.GetCalculatorParamNames());
            }
            foreach (var testParams in this.ComboParams.Main_TestParamsCollection)
            {
                orgNameList.AddRange(testParams.GetCalculatorParamNames());
            }
            foreach (var testParams in this.ComboParams.Post_TestParamsCollection)
            {
                orgNameList.AddRange(testParams.GetCalculatorParamNames());
            }

            var biggerThanOneElementList = orgNameList.GroupBy(x => x).Where(g => g.Count() > 1).Select(m => m.Key).ToList();

            foreach (var ele in biggerThanOneElementList)
            {
                dupCalcName += $"[{ele}],";
            }
            dupCalcName = dupCalcName.TrimEnd(',');
 
            return dupCalcName;
        }
        public bool CheckEmptyCalcParamName()
        {
            string dupCalcName = string.Empty;
            List<string> orgNameList = new List<string>();
            foreach (var testParams in this.ComboParams.Pre_TestParamsCollection)
            {
                orgNameList.AddRange(testParams.GetCalculatorParamNames());
            }
            foreach (var testParams in this.ComboParams.Main_TestParamsCollection)
            {
                orgNameList.AddRange(testParams.GetCalculatorParamNames());
            }
            foreach (var testParams in this.ComboParams.Post_TestParamsCollection)
            {
                orgNameList.AddRange(testParams.GetCalculatorParamNames());
            }

            var isAnyEmpty = orgNameList.Contains(string.Empty);

            return isAnyEmpty;
        }

        public void Save(string filePath)
        {
            try
            {
                var tempTypes = this.GetIncludingTypes();
                this.IncludingTypes = new List<string>();
                foreach (var t in tempTypes)
                {
                    this.IncludingTypes.Add(t.FullName);
                }

                XmlHelper.SerializeFile(filePath, this, tempTypes.ToArray());
            }
            catch
            {

            }

        }
        public static TestExecutorComboWithParams Load(ITesterCoreInteration core, string path)
        {
            TestExecutorComboWithParams dataObj = null;
            try
            {
                var temp = XElement.Load(path);
                var typesEle = temp.GetElement("IncludingTypes");
                List<string> typeNames = new List<string>();
                foreach (var node in typesEle.Nodes())
                {
                    typeNames.Add(((XElement)node).Value);
                }

                var types = core.GetTypeFromClassInPreLoadDlls(typeNames);
                dataObj = XmlHelper.DeserializeFile(path, typeof(TestExecutorComboWithParams), types.ToArray()) as TestExecutorComboWithParams;
                return dataObj;
            }
            catch
            {

            }
            return dataObj;
        }
    }
}