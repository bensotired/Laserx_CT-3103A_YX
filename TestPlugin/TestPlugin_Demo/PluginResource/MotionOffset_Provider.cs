using LX_BurnInSolution.Utilities;

namespace TestPlugin_Demo
{

    public class MotionOffset_Provider_CT3103
    {
        public const string ConfigFileName = @"MotionOffsetSettings.xml";

        public MotionOffsetSettings _MotionOffsetSettings { get; set; }

        public MotionOffset_Provider_CT3103()
        {
            this._MotionOffsetSettings = new MotionOffsetSettings();
        }
        public virtual void Save(string path)
        {
            XmlHelper.SerializeFile(path, this);
        }

        public static TMotionOffset_Provider_CT3103 Load<TMotionOffset_Provider_CT3103>(string path) where TMotionOffset_Provider_CT3103 : MotionOffset_Provider_CT3103
        {
            return XmlHelper.DeserializeFile<TMotionOffset_Provider_CT3103>(path);
        }
    }
}