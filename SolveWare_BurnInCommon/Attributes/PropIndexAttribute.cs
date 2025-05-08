using System;
using System.Collections.Generic;

namespace SolveWare_BurnInCommon
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public class PropIndexAttribute : Attribute
    {
        //
        // 摘要:
        //     初始化 System.Xml.Serialization.XmlIgnoreAttribute 类的新实例。
        public PropIndexAttribute(int index)
        {
            this.Index = index;
        }
        public int Index { get; protected set; }
    }

    public class PropIndexComparer : IComparer<PropIndexAttribute>  
    {
        public int Compare(PropIndexAttribute x, PropIndexAttribute y)
        {
            return x.Index.CompareTo(y.Index);
        }
    }
}