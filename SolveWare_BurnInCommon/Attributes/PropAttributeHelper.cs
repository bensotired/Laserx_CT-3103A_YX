using System;
using System.Collections.Generic;
using System.Reflection;

namespace SolveWare_BurnInCommon
{
    public static class PropHelper
    {
        public static bool IsPropertySweepChartElement(PropertyInfo prop, ref CEAxisXY ceAxisXY, ref CEClass ceClass /*,ref int seriesID*/)
        {
            var cAtt = prop.GetCustomAttributes(typeof(PropSweepChartElementAttribute), false);
            if (cAtt != null && cAtt.Length > 0)
            {
                var attr = (cAtt[0] as PropSweepChartElementAttribute);
                ceAxisXY = attr.ChartAxisXY;
                ceClass = attr.ChartClass;
                //seriesID = attr.ChartSeriesID;
                return true;

            }
            else
            {
                return false;
            }
        }
        public static bool IsPropertyChartElement(PropertyInfo prop ,ref CEAxisXY ceAxisXY,ref CEClass ceClass /*,ref int seriesID*/)
        {
            var cAtt = prop.GetCustomAttributes(typeof(PropChartElementAttribute), false);
            if (cAtt != null && cAtt.Length > 0)
            {
                var attr = (cAtt[0] as PropChartElementAttribute);
                ceAxisXY = attr.ChartAxisXY;
                ceClass = attr.ChartClass;
                //seriesID = attr.ChartSeriesID;
                return true;

            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 属性是否可编辑
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static bool IsPropertyCanEdit(PropertyInfo prop)
        {
            var cAtt = prop.GetCustomAttributes(typeof(PropEditableAttribute), false);
            if (cAtt != null && cAtt.Length > 0)
            {
                return (cAtt[0] as PropEditableAttribute).CanEdit;

            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取可以编辑的属性
        /// </summary>
        /// <param name="props"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetEditableProperties(PropertyInfo[] props)
        {
            List<PropertyInfo> retProps = new List<PropertyInfo>();
            foreach (var prop in props)
            {
                var cAtt = prop.GetCustomAttributes(typeof(PropEditableAttribute), false);
                if (cAtt != null && cAtt.Length > 0)
                {

                   if( (cAtt[0] as PropEditableAttribute).CanEdit)
                    {
                        retProps.Add(prop);
                    }
                }
                else
                {
                 
                }
            }
            return retProps.ToArray();
        }

        /// <summary>
        /// 获取对象中可以编辑的属性
        /// </summary>
        /// <param name="objectItem"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetEditableProperties(object objectItem)
        {
            List<PropertyInfo> retProps = new List<PropertyInfo>();
            var allProps = objectItem.GetType().GetProperties();
            foreach (var prop in allProps) 
            {
                var cAtt = prop.GetCustomAttributes(typeof(PropEditableAttribute), false);
                if (cAtt != null && cAtt.Length > 0)
                {

                    if ((cAtt[0] as PropEditableAttribute).CanEdit)
                    {
                        retProps.Add(prop);
                    }
                }
                else
                {

                }
            }
            return retProps.ToArray();
        }


        public static bool IsPropertyCanIndex(PropertyInfo prop, ref int index)
        {
            var cAtt = prop.GetCustomAttributes(typeof(PropIndexAttribute), false);
            if (cAtt != null && cAtt.Length > 0)
            {
                index = ((PropIndexAttribute)cAtt[0]).Index;
                return true;
            }
            else
            {
                index = -1;
                return false;
            }
        }

        /// <summary>
        /// 判断类是否定义指定特性
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="clsT"></param>
        /// <returns></returns>
        public static bool IsClassTypeBelongs<TAttribute>(Type clsT)
        {
            return clsT.IsDefined(typeof(TAttribute), true);
        }
        /// <summary>
        /// 判断方法是否定义指定特性
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="mthd"></param>
        /// <returns></returns>
        public static bool IsMethodBelongs<TAttribute>(MethodInfo mthd)
        {
            return mthd.IsDefined(typeof(TAttribute), true);

        }
        /// <summary>
        /// 判断属性是否定义指定特性
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static bool IsPropertyBelongs<TAttribute>(PropertyInfo prop)
        {
            return prop.IsDefined(typeof(TAttribute), true);
        }

        /// <summary>
        /// 从类上面获取特性类的内容
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="clsT"></param>
        /// <returns></returns>
        public static TAttribute GetAttributeValue<TAttribute>(Type clsT)
        {
            var cAtt = clsT.GetCustomAttributes(typeof(TAttribute), false);
            if (cAtt != null && cAtt.Length > 0)
            {
                return (TAttribute)cAtt[0];
            }
            else
            {
                return default(TAttribute);
            }
        }
        public static List<TAttribute> GetAttributeValueCollection<TAttribute>(Type clsT)
        {
            var cAtt = clsT.GetCustomAttributes(typeof(TAttribute), false);
            List<TAttribute> temp = new List<TAttribute>();
            foreach (var att in cAtt)
            {
                temp.Add((TAttribute)att);
            }
            return temp;
        }
        /// <summary>
        /// 从方法上面获取特性类的内容
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static TAttribute GetAttributeValue<TAttribute>(MethodInfo methodInfo)
        {
            var cAtt = methodInfo.GetCustomAttributes(typeof(TAttribute), false);
            if (cAtt != null && cAtt.Length > 0)
            {
                return (TAttribute)cAtt[0];
            }
            else
            {
                return default(TAttribute);
            }
        }

        /// <summary>
        /// 从属性上面获取特性类的内容
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static TAttribute GetAttributeValue<TAttribute>(PropertyInfo propertyInfo)
        {
            var cAtt = propertyInfo.GetCustomAttributes(typeof(TAttribute), false);
            if (cAtt != null && cAtt.Length > 0)
            {
                return (TAttribute)cAtt[0];
            }
            else
            {
                return default(TAttribute);
            }
        }


        /*----------------------------20220901------------------------------*/


        /// <summary>
        /// 从类数组中获取定义指定特性的类数组
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="clsTypes"></param>
        /// <returns></returns>
        public static Type[] GetAttributeTypes<TAttribute>(Type[] clsTypes)
        {
            List<object> list = new List<object>();
            if (clsTypes != null && clsTypes.Length > 0)
            {
                foreach (var item in clsTypes)
                {
                    if (IsClassTypeBelongs<TAttribute>(item))
                    {
                        list.Add(item);
                    }
                }
                return (Type[])list.ToArray();
            }
            return null;
        }


        /// <summary>
        /// 从属性数组中获取定义指定特性的属性数组
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="props"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetAttributeProps<TAttribute>(PropertyInfo[] props)
        {
            List<PropertyInfo> list = new List<PropertyInfo>();
            if (props != null && props.Length > 0)
            {
                foreach (var item in props)
                {
                    if (IsPropertyBelongs<TAttribute>(item))
                    {
                        list.Add(item);
                    }
                }
                return list.ToArray();
            }
            return list.ToArray();
        }





















    }
}