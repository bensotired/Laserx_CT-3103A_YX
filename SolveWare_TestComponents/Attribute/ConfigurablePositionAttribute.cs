using System;

namespace SolveWare_TestComponents.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]

    public class ConfigurablePositionAttribute : Attribute
    {
        public ConfigurablePositionAttribute(string moduleDefinePositionName, string usageDescription)
        {
            ModuleDefinePositionName = moduleDefinePositionName;
            UsageDescription = usageDescription;
        }

        public string ModuleDefinePositionName { get; set; }
        public string UsageDescription { get; set; }
    }
}