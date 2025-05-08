using System;
using System.Collections.Generic;

namespace SolveWare_BurnInCommon
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public class PropEditableAttribute : Attribute
    {
        //
        // 摘要:
        //     初始化 System.Xml.Serialization.XmlIgnoreAttribute 类的新实例。
        public PropEditableAttribute(bool canEdit)
        {
            this.CanEdit = canEdit;
        }
        public bool CanEdit { get; protected set; }
    }
   
}