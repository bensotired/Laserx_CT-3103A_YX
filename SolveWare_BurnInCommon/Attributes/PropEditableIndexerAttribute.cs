using System;
using System.Collections.Generic;

namespace SolveWare_BurnInCommon
{
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public class PropEditableIndexerAttribute : PropEditableAttribute
    {
        //
        // 摘要:
        //     初始化 System.Xml.Serialization.XmlIgnoreAttribute 类的新实例。
        public PropEditableIndexerAttribute(bool canEdit,int index):base(canEdit)
        {
            this.Index = index;
        }
        public int Index { get; protected set; }
    }
}