using LX_BurnInSolution.Utilities;

namespace TestPlugin_Demo
{
    public class DelayTimeSettings_Provider_CT3103
    {
        public const string ConfigFileName = @"DelayTimeSettings.xml";

        public DelayTimeSettings _DelayTimeSettings { get; set; }

        public DelayTimeSettings_Provider_CT3103()
        {
            this._DelayTimeSettings = new DelayTimeSettings();
        }
        public virtual void Save(string path)
        {
            XmlHelper.SerializeFile(path, this);
        }

        public static TDelayTimeSettings_Provider_CT3103 Load<TDelayTimeSettings_Provider_CT3103>(string path) where TDelayTimeSettings_Provider_CT3103 : DelayTimeSettings_Provider_CT3103
        {
            return XmlHelper.DeserializeFile<TDelayTimeSettings_Provider_CT3103>(path);
        }
    }
}
