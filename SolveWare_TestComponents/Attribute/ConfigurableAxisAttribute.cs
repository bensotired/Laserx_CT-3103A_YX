using System;

namespace SolveWare_TestComponents.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]

    public class ConfigurableAxisAttribute : Attribute
    {
        public ConfigurableAxisAttribute(string moduleDefineAxisName, string usageDescription)
        {
            ModuleDefineAxisName = moduleDefineAxisName;
            UsageDescription = usageDescription;
        }

        public string ModuleDefineAxisName { get; set; }
        public string UsageDescription { get; set; }
    }
}