using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SolveWare_TestComponents.UI
{
    public class PropertyDataGirdView : DataGridView
    {
        public PropertyDataGirdView() : base()
        {
            this.DataError -= Dgv_DataError;
            this.DataError += Dgv_DataError;
        }
        private void Dgv_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                var dgv = sender as DataGridView;
                if (e.Exception is System.ArgumentException)
                {
                    if (dgv[e.ColumnIndex, e.RowIndex] is DataGridViewComboBoxCell)
                    {
                        var value = dgv[e.ColumnIndex, e.RowIndex].Value;

                        var valueType = (dgv.Columns[e.ColumnIndex].Tag as PropertyInfo).PropertyType;

                        if (valueType.IsEnum)
                        {
                            dgv[e.ColumnIndex, e.RowIndex].Value = Converter.ConvertObjectTo(value, valueType).ToString();
                        }
                        else
                        {
                            dgv[e.ColumnIndex, e.RowIndex].Value = Converter.ConvertObjectTo(value, valueType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        public virtual void ImportSourceData<TCURDItemLite>(CURDBaseLite<TCURDItemLite> sourceData , bool autoSizeColumn) where TCURDItemLite : class, ICURDItemLite
        {
            if(sourceData.ItemCollection.Count <= 0)
            {
                this.Rows.Clear();
                this.Columns.Clear();
                return;
            }
            //拿到item所有属性
            var curdItemProps = typeof(TCURDItemLite).GetProperties();
            //拿到所有带PropEditableAttribute标签的属性
            PropertyInfo[] editableProps = PropHelper.GetEditableProperties(curdItemProps.ToArray());
            //建立<未排序>的带PropEditableIndexerAttribute标签的属性的字典 
            Dictionary<PropEditableIndexerAttribute, PropertyInfo> sortProps = new Dictionary<PropEditableIndexerAttribute, PropertyInfo>();
            //从所有带PropEditableAttribute标签的属性中进行遍历
            foreach (var itemProp in curdItemProps)
            {
                //拿到带PropEditableIndexerAttribute标签的属性
                if (PropHelper.IsPropertyBelongs<PropEditableIndexerAttribute>(itemProp))
                {
                    //拿到属性上的PropEditableIndexerAttribute标签
                    var editIdxAtrb = PropHelper.GetAttributeValue<PropEditableIndexerAttribute>(itemProp);
                    //拿到属性上的PropEditableIndexerAttribute标签如果被标记为可编辑
                    if (editIdxAtrb.CanEdit)
                    {
                        //加入<未排序>的带PropEditableIndexerAttribute标签的属性的字典 
                        if (sortProps.ContainsKey(editIdxAtrb))
                        {
                            throw new ArgumentException($"已经存在相同的 PropEditableIndexerAttribute    在添加属性{itemProp.Name}!");
                        }
                        sortProps.Add(editIdxAtrb, itemProp);
                    }
                }
            }

            var keyList = sortProps.Keys.ToList();
            keyList.Sort((item1, item2) => { return item1.Index.CompareTo(item2.Index); });

            //建立<排序后>的属性对于dataGridViewColumn
            
            this.Columns.Clear();
            foreach (var atrb in keyList)
            {
                DataGridViewColumn newCol = null;

                var colPropType = sortProps[atrb].PropertyType;

                if (colPropType.IsEnum)
                {
                    newCol = GuiExternFuns.CreateEnumValueComboBoxColumn(colPropType);
                }
                else if (colPropType == typeof(bool))
                {
                    newCol = GuiExternFuns.CreateBooleanValueComboBoxColumnl();
                }
                else
                {
                    newCol = new DataGridViewTextBoxColumn();
                }
                newCol.HeaderText = sortProps[atrb].Name;
                newCol.Tag = sortProps[atrb];
                this.Columns.Add(newCol);
            }
            int colWidth = this.Width / this.ColumnCount;
            foreach(DataGridViewColumn col in this.Columns)
            {
                col.Width = colWidth;
            }

            this.Rows.Clear();

            foreach (var curdItem in sourceData.ItemCollection)
            {
                var rIndex = this.Rows.Add();
                foreach (var atrb in keyList)
                {
                    var colProp = sortProps[atrb];
                    var defaultValue = colProp.GetValue(curdItem).ToString();
                    this.Rows[rIndex].Cells[atrb.Index].Value = defaultValue;
                }
            }
        }
   
        public virtual void Clear()
        {
           
            this.Rows.Clear();
            this.Columns.Clear();
        }
    }
}