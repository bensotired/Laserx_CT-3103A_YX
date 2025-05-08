using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin_Demo
{
    public class RunSettings_Provider_CT3103
    {
        public const string ConfigFileName = @"runParamSettings.xml";

        public RunParamSettings _RunParamSettings { get; set; }

        public RunSettings_Provider_CT3103()
        {
            this._RunParamSettings = new RunParamSettings();
        }
        public virtual void Save(string path)
        {
            XmlHelper.SerializeFile(path, this);
        }

        public static TRunSettings_Provider_CT3103 Load<TRunSettings_Provider_CT3103>(string path) where TRunSettings_Provider_CT3103 : RunSettings_Provider_CT3103
        {
            return XmlHelper.DeserializeFile<TRunSettings_Provider_CT3103>(path);
        }

    }
}
