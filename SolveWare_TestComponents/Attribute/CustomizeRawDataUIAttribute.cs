using System;

namespace SolveWare_TestComponents.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]

    public class CustomizeRawDataUIAttribute : Attribute
    {
        public CustomizeRawDataUIAttribute(string rawDataUI_TypeName )
        {
            RawDataUI_TypeName = rawDataUI_TypeName;
 
        }

        public string RawDataUI_TypeName { get; set; }
      
    }
}