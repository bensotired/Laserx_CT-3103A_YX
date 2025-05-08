using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LX_BurnInSolution.Utilities
{
    public static class GuiExternFuns
    {
        public static void ModifyBooleanValueCell(bool sourceObj, DataGridView dgv, int rowIndex, int colIndex)
        {
            dgv.Rows[rowIndex].Cells[colIndex] = new DataGridViewComboBoxCell();
            ((DataGridViewComboBoxCell)dgv.Rows[rowIndex].Cells[colIndex]).Items.AddRange(true, false);
            ((DataGridViewComboBoxCell)dgv.Rows[rowIndex].Cells[colIndex]).Value = sourceObj;
        }
        public static void ModifyBooleanValueCell(object sourceObj, DataGridView dgv, PropertyInfo prop, int rowIndex, int colIndex)
        {
            var defaultValue = prop.GetValue(sourceObj);

            dgv.Rows[rowIndex].Cells[colIndex] = new DataGridViewComboBoxCell();
            dgv.Rows[rowIndex].Cells[colIndex].Tag = prop;
            ((DataGridViewComboBoxCell)dgv.Rows[rowIndex].Cells[colIndex]).Items.AddRange(true, false);
            ((DataGridViewComboBoxCell)dgv.Rows[rowIndex].Cells[colIndex]).Value = defaultValue;
        }
        public static void ModifyBooleanValueCell(object sourceObj, DataGridView dgv, PropertyInfo prop, int rowIndex, string colName)
        {
            var defaultValue = prop.GetValue(sourceObj);

            dgv.Rows[rowIndex].Cells[colName] = new DataGridViewComboBoxCell();
            dgv.Rows[rowIndex].Cells[colName].Tag = prop;
            ((DataGridViewComboBoxCell)dgv.Rows[rowIndex].Cells[colName]).Items.AddRange(true, false);
            ((DataGridViewComboBoxCell)dgv.Rows[rowIndex].Cells[colName]).Value = defaultValue;
        }
        public static void ModifyEnumValueCell(object sourceObj, DataGridView dgv, PropertyInfo prop, int rowIndex, int colIndex)
        {
            var defaultValue = prop.GetValue(sourceObj).ToString();

            dgv.Rows[rowIndex].Cells[colIndex] = new DataGridViewComboBoxCell();
            dgv.Rows[rowIndex].Cells[colIndex].Tag = prop;
            var eItems = Enum.GetValues(prop.PropertyType);
            List<string> cellItems = new List<string>();
            foreach (var item in eItems)
            {
                cellItems.Add(item.ToString());
            }
            ((DataGridViewComboBoxCell)dgv.Rows[rowIndex].Cells[colIndex]).Items.AddRange(cellItems.ToArray());
            ((DataGridViewComboBoxCell)dgv.Rows[rowIndex].Cells[colIndex]).Value = defaultValue;
   
        }

        public static void ModifyEnumValueCell(object sourceObj, DataGridView dgv, PropertyInfo prop, int rowIndex, string colName)
        {
            var defaultValue = prop.GetValue(sourceObj).ToString();

            dgv.Rows[rowIndex].Cells[colName] = new DataGridViewComboBoxCell();
            dgv.Rows[rowIndex].Cells[colName].Tag = prop;
            var eItems = Enum.GetValues(prop.PropertyType);
            List<string> cellItems = new List<string>();
            foreach (var item in eItems)
            {
                cellItems.Add(item.ToString());
            }
            ((DataGridViewComboBoxCell)dgv.Rows[rowIndex].Cells[colName]).Items.AddRange(cellItems.ToArray());
            ((DataGridViewComboBoxCell)dgv.Rows[rowIndex].Cells[colName]).Value = defaultValue;
        }

        public static Dictionary<string, Control> LX_GetFormControls(this Control ctrl)
        {
            List<Control> ctrlFamily = new List<Control>();

            Dictionary<string, Control> ctrlDict = new Dictionary<string, Control>();
            try
            {
                foreach (Control item in ctrl.Controls)
                {
                    GetControlFamily(item, ref ctrlFamily);
                }


                foreach (var item in ctrlFamily)
                {

                    if (item.Name != null && item.Name.Length > 0)
                    {
                        if (ctrlDict.ContainsKey(item.Name))
                        {
                            throw new Exception($"LX_GetFormControls :已经添加相同的控件[{item.Name}]");
                        }
                        else
                        {
                            ctrlDict.Add(item.Name, item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ctrlDict;
        }
        /// <summary>
        /// 拿control全家
        /// </summary>
        /// <param name="ctrl"></param>
        static void GetControlFamily(Control ctrl, ref List<Control> ctrlFamily)
        {
            if (ctrl.Controls.Count > 0)
            {
                foreach (Control subCtrl in ctrl.Controls)
                {
                    GetControlFamily(subCtrl, ref ctrlFamily);
                }
            }
            else
            {
                ctrlFamily.Add(ctrl);
            }
        }
        /// <summary>
        /// 拿control真身
        /// </summary>
        /// <typeparam name="TCtrl"></typeparam>
        /// <param name="ctrlName"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static TCtrl LX_GetCtrl<TCtrl>(string ctrlName, Dictionary<string, Control> dict)
        {
            if (dict.ContainsKey(ctrlName))
            {
                return (TCtrl)((object)dict[ctrlName]);
            }
            return default(TCtrl);
        }
        public static DataGridViewComboBoxColumn CreateEnumValueComboBoxColumn(PropertyInfo prop)
        {
            DataGridViewComboBoxColumn col = new DataGridViewComboBoxColumn();
            var eItems = Enum.GetValues(prop.PropertyType);
            foreach (var item in eItems)
            { 
                col.Items.Add(item.ToString());
            }
            return col;
        }
        public static DataGridViewComboBoxColumn CreateEnumValueComboBoxColumn(Type valueType)
        {
            DataGridViewComboBoxColumn col = new DataGridViewComboBoxColumn();
            var eItems = Enum.GetValues(valueType);
            foreach (var item in eItems)
            {
                col.Items.Add(item.ToString());
            }
            return col;
        }
        public static DataGridViewComboBoxColumn CreateBooleanValueComboBoxColumnl()
        {
            DataGridViewComboBoxColumn col = new DataGridViewComboBoxColumn();

            col.Items.AddRange(true, false);
          
            return col;
        }
     
    }
}