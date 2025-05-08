using System;
 
namespace SolveWare_TestComponents.Attributes
{
    [AttributeUsage(AttributeTargets.Class , AllowMultiple = true)]
   
    public class ConfigurableInstrumentAttribute : Attribute
    {
        public ConfigurableInstrumentAttribute(string instrType, string moduleDefineInstrName, string usageDescription)
        {
            InstrType = instrType;
            ModuleDefineInstrName = moduleDefineInstrName;
            UsageDescription = usageDescription;
        }
        public string InstrType { get; set; }
        public string ModuleDefineInstrName { get; set; }
        public string UsageDescription { get; set; }
    }
}