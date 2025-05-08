using LX_BurnInSolution.Utilities;
using SolveWare_Motion;
using SolveWare_Vision;
using System;
using System.IO;
using System.Windows.Forms;

namespace TestPlugin_Demo
{

    public class InPutSettings_Provider_CT3103
    {
        public const string ConfigFileName = @"InPutSettings.xml";

        public InPutSettings _InPutSettings { get; set; }

        public InPutSettings_Provider_CT3103()
        {
            this._InPutSettings = new InPutSettings();
        }
        public virtual void Save(string path)
        {
            XmlHelper.SerializeFile(path, this);
        }

        public static TInPutSettings_Provider_CT3103 Load<TInPutSettings_Provider_CT3103>(string path) where TInPutSettings_Provider_CT3103 : InPutSettings_Provider_CT3103
        {
            return XmlHelper.DeserializeFile<TInPutSettings_Provider_CT3103>(path);
        }

    }

}