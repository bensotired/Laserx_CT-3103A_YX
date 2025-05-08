using SolveWare_BurnInCommon;
using System;
 
namespace SolveWare_TestComponents.Attributes
{
    [AttributeUsage(AttributeTargets.Class , AllowMultiple = true)]
   
    public class StaticResourceAttribute : Attribute
    {
        public StaticResourceAttribute(ResourceItemType resourceType, string resourceName, string usageDescription)
        {
            ResourceType = resourceType;
            ResourceName = resourceName;
            UsageDescription = usageDescription;
        }
        public ResourceItemType ResourceType { get; set; }
        public string ResourceName { get; set; }
        public string UsageDescription { get; set; }
    }
}