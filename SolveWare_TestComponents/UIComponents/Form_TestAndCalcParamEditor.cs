using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SolveWare_TestComponents.UIComponents
{
    public partial class Form_TestAndCalcParamEditor : Form
    {
        const int INFO_COL_INDEX = 0;
        const int KEY_COL_INDEX = 1;
        const int VAL_COL_INDEX = 2;
        public Form_TestAndCalcParamEditor()
        {
            InitializeComponent();
            dgv_TestParamEditor.DataError += Dgv_TestParamEditor_DataError;
            dgv_CalcParamEditor.DataError += Dgv_TestParamEditor_DataError;
        }

        private void Dgv_TestParamEditor_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            
        }

        ExecutorConfigItem_TestParamsConfig _local_ExecutorConfigItem_TestParamsConfig;
        public void Import(  ExecutorConfigItem_TestParamsConfig item)
        {
            _local_ExecutorConfigItem_TestParamsConfig = item;
            this.ImportTestParamsConfig(item);
            this.ImportCalcParamsConfig(item);
        }
        void ImportTestParamsConfig(ExecutorConfigItem_TestParamsConfig item)
        {
            try
            {
                dgv_TestParamEditor.Rows.Clear();
                UIGeneric.FillListDGV_InfoKeyValue(dgv_TestParamEditor, item.TestRecipe, INFO_COL_INDEX, KEY_COL_INDEX, VAL_COL_INDEX);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        void ImportCalcParamsConfig(ExecutorConfigItem_TestParamsConfig item)
        {
            try
            {
                int maxParamsCount = 0;
                //遍历所有算子  找出参数最多的算子
                foreach (var calcItem in item.CalcRecipeBook)
                {
                    var props = PropHelper.GetEditableProperties(calcItem.Value);
                    if (maxParamsCount < props.Length)
                    {
                        maxParamsCount = props.Length;
                    }
                }
                //}
                dgv_CalcParamEditor.Rows.Clear();
                dgv_CalcParamEditor.Columns.Clear();
                int fixColIndex = 0;
                fixColIndex = dgv_CalcParamEditor.Columns.Add("Col_Calculator", "算子");
                dgv_CalcParamEditor.Columns[fixColIndex].ReadOnly = true;
                for (int calcParamIndex = 1; calcParamIndex <= maxParamsCount; calcParamIndex++)
                {
                    fixColIndex = dgv_CalcParamEditor.Columns.Add($"Col_CalcParam[{calcParamIndex}]", $"参数[{calcParamIndex}]");
                    dgv_CalcParamEditor.Columns[fixColIndex].ReadOnly = true;
                    dgv_CalcParamEditor.Columns[fixColIndex].DefaultCellStyle.BackColor = Color.FromKnownColor(KnownColor.Info);
                    dgv_CalcParamEditor.Columns.Add($"Col_CalcParamName[{calcParamIndex}]", $"值[{calcParamIndex}]");
                }

                foreach (var calcItem in item.CalcRecipeBook)
                {
                    var calcName = calcItem.Key;
                    var calcRcp = calcItem.Value;
                    var props = PropHelper.GetEditableProperties(calcRcp);
                    List<object> calcRowObjs = new List<object>();
                    calcRowObjs.Add(calcName);
                    foreach (var prop in props)
                    {
                        calcRowObjs.Add(prop.Name);
                        calcRowObjs.Add(prop.GetValue(calcRcp));
                    }

                    dgv_CalcParamEditor.Rows.Add(calcRowObjs.ToArray());
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Update()
        {
  
            this.UpdateTestParamsConfig(_local_ExecutorConfigItem_TestParamsConfig);
            this.UpdateCalcParamsConfig(_local_ExecutorConfigItem_TestParamsConfig);
        }
        void UpdateTestParamsConfig(ExecutorConfigItem_TestParamsConfig item)
        {
            try
            {
                var valueDict = UIGeneric.Grab_DGV_KeyValueDict(dgv_TestParamEditor, KEY_COL_INDEX, VAL_COL_INDEX);

                ReflectionTool.SetPropertyValues(item.TestRecipe, valueDict); 
            }
            catch (Exception ex)
            {
                throw new Exception($"当前界面获取数据异常，异常原因：[{ex.Message}]");
            }
        }
        void UpdateCalcParamsConfig(ExecutorConfigItem_TestParamsConfig item)
        {
            try
            {
                const int CalcNameColIndex = 0;
                const int CalcValueOffset = 1;
                foreach (var calcItem in item.CalcRecipeBook)
                {
                    string calcName = calcItem.Key;
                    CalcRecipe calcRcp = calcItem.Value;
                    foreach (DataGridViewRow row in dgv_CalcParamEditor.Rows)
                    {
                        if (calcName == row.Cells[CalcNameColIndex].Value.ToString())
                        {
                            var props = PropHelper.GetEditableProperties(calcRcp);
                            foreach (var prop in props)
                            {
                                for (int colIndex = 1; colIndex < row.Cells.Count; colIndex++)
                                {
                                    if (colIndex % 2 == 0) { continue; }
                                    var cellValue = row.Cells[colIndex].Value?.ToString();
                                    if (cellValue == prop.Name)
                                    {
                                        var propValueObj = row.Cells[colIndex + CalcValueOffset].Value;
                                        var propValue = Converter.ConvertObjectTo(propValueObj, prop.PropertyType);
                                        prop.SetValue(calcRcp, propValue);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"当前界面获取数据异常，异常原因：[{ex.Message}]");
            }
        }
        private void dgv_TestParamEditor_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}