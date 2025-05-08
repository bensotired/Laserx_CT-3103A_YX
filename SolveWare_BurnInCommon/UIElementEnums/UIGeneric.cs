using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace SolveWare_BurnInCommon
{
    public static class UIGeneric
    {


        public static void FillListDGV_InstrumentChassisConfigItem(DataGridView dgv, List<InstrumentChassisConfigItem> sourceObject,
            int nameColIndex, int resourceColIndex, int isOnlineColIndex, int initTimeout_msColIndex, int chassisTypeColIndex)
        {
            //var props = PropHelper.GetEditableProperties(sourceObject);

            if (sourceObject.Count <= 0) { return; }

            dgv.Rows.Clear();

            foreach (var item in sourceObject)
            {
                int rowIndex = dgv.Rows.Add();
                var props = item.GetType().GetProperties();

                dgv.Rows[rowIndex].Cells[nameColIndex].Value = item.Name;
                dgv.Rows[rowIndex].Cells[resourceColIndex].Value = item.Resource;
                GuiExternFuns.ModifyBooleanValueCell(item.IsOnline, dgv, rowIndex, isOnlineColIndex);
                dgv.Rows[rowIndex].Cells[initTimeout_msColIndex].Value = item.InitTimeout_ms;
                dgv.Rows[rowIndex].Cells[chassisTypeColIndex].Value = item.ChassisType;

            }
        }
        public static void FillListDGV_InstrumentConfigItem(DataGridView dgv, List<InstrumentConfigItem> sourceObject,
         int nameColIndex, int addressColIndex, int isOnlineColIndex,/* int isSimulationColIndex,*/ int chassisNameColIndex, int instrumentTypeColIndex)
        {
            //var props = PropHelper.GetEditableProperties(sourceObject);

            if (sourceObject.Count <= 0) { return; }

            dgv.Rows.Clear();

            foreach (var item in sourceObject)
            {
                int rowIndex = dgv.Rows.Add();
                var props = item.GetType().GetProperties();

                dgv.Rows[rowIndex].Cells[nameColIndex].Value = item.Name;
                dgv.Rows[rowIndex].Cells[addressColIndex].Value = item.Address;
                GuiExternFuns.ModifyBooleanValueCell(item.IsOnline, dgv, rowIndex, isOnlineColIndex);
                //GuiExternFuns.ModifyBooleanValueCell(item.IsSimulation, dgv, rowIndex, isSimulationColIndex);
                dgv.Rows[rowIndex].Cells[chassisNameColIndex].Value = item.ChassisName;
                dgv.Rows[rowIndex].Cells[instrumentTypeColIndex].Value = item.InstrumentType;

            }
        }
        public static void FillListDGV_InfoKeyValue(DataGridView dgv, object sourceObject, int infoColIndex, int keyColIndex, int valColIndex)
        {
            //获取可以编辑的属性
            var props = PropHelper.GetEditableProperties(sourceObject);

            if (props.Length <= 0) { return; }

            dgv.Rows.Clear();

            for (int i = 0; i < props.Length; i++)
            {
                var prop = props[i];
                var pName = prop.Name;
                var pType = prop.PropertyType;
                //展示名称
                var dName = string.Empty;

                //标签是否应用在属性上面
                if (PropHelper.IsPropertyBelongs<DisplayNameAttribute>(prop))
                {
                    dName = PropHelper.GetAttributeValue<DisplayNameAttribute>(prop).DisplayName;
                }
                //从数据源中获取属性的值
                var val = prop.GetValue(sourceObject);

                //添加行
                int rowIndex = dgv.Rows.Add();
                //添加行的单元格
                dgv.Rows[rowIndex].Cells[infoColIndex].Value = dName;
                dgv.Rows[rowIndex].Cells[keyColIndex].Value = pName;

                if (pType.IsEnum)
                {
                    //修改枚举类型单元格，并添加数据
                    GuiExternFuns.ModifyEnumValueCell(sourceObject, dgv, prop, rowIndex, valColIndex);
                }
                else if (pType == typeof(bool))
                {
                    //修改枚举类型单元格，并添加数据
                    GuiExternFuns.ModifyBooleanValueCell(sourceObject, dgv, prop, rowIndex, valColIndex);
                }
                else
                {
                    dgv.Rows[rowIndex].Cells[valColIndex].Tag = prop;
                    dgv.Rows[rowIndex].Cells[valColIndex].Value = val;
                }
            }
        }

        //public static Dictionary<string, object> Grab_DGV_KeyValueDict(DataGridView dgv, int keyColIndex, int valColIndex)
        //{
        //    Dictionary<string, object> dict = new Dictionary<string, object>();
        //    if ((dgv.ColumnCount - 1) < keyColIndex ||
        //        (dgv.ColumnCount - 1) < valColIndex ||
        //        dgv.RowCount <= 0)
        //    {
        //        //空的dgv什么都不干
        //    }
        //    else
        //    {
        //        for (int rIndex = 0; rIndex < dgv.RowCount; rIndex++)
        //        {
        //            var key = dgv.Rows[rIndex].Cells[keyColIndex].Value.ToString();

        //            var pInfo = dgv.Rows[rIndex].Cells[valColIndex].Tag as PropertyInfo;
        //            var valObj = dgv.Rows[rIndex].Cells[valColIndex].Value;

        //            var val = Converter.ConvertObjectTo(valObj, pInfo.PropertyType);

        //            dict.Add(key, val);
        //        }
        //    }
        //    return dict;
        //}
        public static Dictionary<string, object> Grab_DGV_KeyValueDict(DataGridView dgv, int keyColIndex, int valColIndex)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                if ((dgv.ColumnCount - 1) < keyColIndex ||
                    (dgv.ColumnCount - 1) < valColIndex ||
                    dgv.RowCount <= 0)
                {
                    //空的dgv什么都不干
                }
                else
                {
                    for (int rIndex = 0; rIndex < dgv.RowCount; rIndex++)
                    {
                        var key = dgv.Rows[rIndex].Cells[keyColIndex].Value.ToString();

                        var pInfo = dgv.Rows[rIndex].Cells[valColIndex].Tag as PropertyInfo;
                        var valObj = dgv.Rows[rIndex].Cells[valColIndex].Value;

                        try
                        {
                            var val = Converter.ConvertObjectTo(valObj, pInfo.PropertyType);
                            dict.Add(key, val);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($" 属性{pInfo.Name}项内容转换为[{pInfo.PropertyType}]失败!");
                            //throw new Exception($"当前第{rIndex + 1}行，{pInfo.Name}项输入的数据类型有误！");
                        }
                    }
                }
                return dict;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void ModifyDockableUI(Form _UI, bool isDockable)
        {
            if (isDockable)
            {
                _UI.Hide();
                _UI.TopLevel = false;
                _UI.FormBorderStyle = FormBorderStyle.None;
                _UI.Dock = DockStyle.Fill;
            }
            else
            {
                //不知道
                _UI.Hide();
                _UI.TopLevel = true;
                _UI.FormBorderStyle = FormBorderStyle.Sizable;
                _UI.Dock = DockStyle.None;
            }
        }
        public static void RefreshListView(ListView listView, Dictionary<string, string> files)
        {//文件名(无扩展名), 完整目录
            //while (!this.IsHandleCreated)
            //{
            //    Application.DoEvents();
            //}

            listView.Invoke((EventHandler)delegate
            {
                try
                {
                    string b4Plan = string.Empty;
                    bool needReSelected = false;
                    if (listView.SelectedItems.Count > 0)
                    {
                        b4Plan = listView.SelectedItems[0].Text;
                        needReSelected = true;
                    }

                    int newIndex = 0;
                    int i = -1;
                    listView.Clear();
                    listView.Columns.Add("文件名", listView.Width, HorizontalAlignment.Left);
                    listView.BeginUpdate();
                    foreach (var kvp in files)
                    {
                        i++;
                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = kvp.Key;
                        lvi.Tag = kvp.Value; //用Tag存路径
                        listView.Items.Add(lvi);

                        if (needReSelected)
                        {
                            if (kvp.Key == b4Plan)
                            {
                                newIndex = i;
                            }
                        }

                    }
                    listView.EndUpdate();

                    if (needReSelected)
                    {
                        listView.Items[newIndex].Selected = true;
                    }
                }
                catch (Exception ex)
                {

                }
            });
        }
    }
}