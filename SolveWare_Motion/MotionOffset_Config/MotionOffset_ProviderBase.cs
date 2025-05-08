using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using System.Drawing;
using System.IO;

namespace SolveWare_Motion
{
    
    public class MotionOffset_ProviderBase
    {
        public const string ConfigFileName = @"motionOffset.xml";
        public MotionOffset_ProviderBase()
        {
            this.OffsetPosDict = new DataBook<string, MotionOffsetDistance>();
        }

        public DataBook<string, MotionOffsetDistance> OffsetPosDict { get; set; }

        protected virtual void Add(string offsetPosName, MotionOffsetDistance distance )
        {
            if (this.OffsetPosDict.ContainsKey(offsetPosName))
            {
                this.OffsetPosDict[offsetPosName] = distance;
            }
            else
            {
                this.OffsetPosDict.Add(offsetPosName, distance);
            }
        }
        protected virtual MotionOffsetDistance this[string offsetPosName]
        {
            get
            {
                if (this.OffsetPosDict.ContainsKey(offsetPosName))
                {
                    return this.OffsetPosDict[offsetPosName];
                }
                else
                {
                    return null;
                }
            }
        }
        public virtual void Save(string path)
        {
            if (File.Exists(path) == false)
            {
                if (Directory.Exists(Path.GetDirectoryName(path)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
               
            }
            XmlHelper.SerializeFile (path, this);
        }
        public static TMotionOffset_Provider Load<TMotionOffset_Provider>(string path) where TMotionOffset_Provider : MotionOffset_ProviderBase
        {
            return XmlHelper.DeserializeFile<TMotionOffset_Provider>(path);
        }
      
    }
}