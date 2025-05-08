using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using System.Drawing;
using System.IO;

namespace SolveWare_Vision
{
    
    public class DeltaPix2Point_ProviderBase
    {
        public const string ConfigFileName = @"dp2p.xml";
        public DeltaPix2Point_ProviderBase()
        {
            this.Dp2pDict = new DataBook<string, DeltaPix2PointDistance>();
        }

        public DataBook<string, DeltaPix2PointDistance> Dp2pDict { get; set; }

        protected virtual void Add(string dp2pName, DeltaPix2PointDistance distance )
        {
            if (this.Dp2pDict.ContainsKey(dp2pName))
            {
                this.Dp2pDict[dp2pName] = distance;
            }
            else
            {
                this.Dp2pDict.Add(dp2pName, distance);
            }
        }
        protected virtual DeltaPix2PointDistance this[string dp2pName]
        {
            get
            {
                if (this.Dp2pDict.ContainsKey(dp2pName))
                {
                    return this.Dp2pDict[dp2pName];
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
        public static TDeltaPix2Point_Provider Load<TDeltaPix2Point_Provider>(string path) where TDeltaPix2Point_Provider : DeltaPix2Point_ProviderBase
        {
            return XmlHelper.DeserializeFile<TDeltaPix2Point_Provider>(path);
        }
      
    }
}