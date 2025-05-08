using SolveWare_SlaveStation;

namespace TestPlugin_Demo
{
    public class SlaveStation_Provider : SlaveStation_ProviderBase
    {
        public SlaveStation_Provider() : base()
        {
           
        }
        public SlaveStation_Config this[string key]
        {
            get
            {
                return Configs[key];
            }
        }
        
    }
}