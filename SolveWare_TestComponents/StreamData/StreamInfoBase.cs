using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SolveWare_TestComponents.Data
{
    public abstract class StreamInfoBase : IStreamInfoBase
    {
        public StreamInfoBase()
        {
            CreateTime =BaseDataConverter.ConvertDateTimeToCommentString( DateTime.Now);
            this.IncludingTypes = new List<string>();
        }

        public List<string> IncludingTypes { get; set; }
        public string CreateTime { get; set; }
        public virtual string Information { get; set; } = string.Empty;
        public abstract Type[] GetIncludingTypes();
        public virtual void Save(string filePath)
        {
            try
            {
                var dir = Path.GetDirectoryName(filePath);
                if (Directory.Exists(dir) == false)
                {
                    Directory.CreateDirectory(dir);
                }
                var types = GetIncludingTypes();
                this.IncludingTypes = new List<string>();
                foreach (var t in types)
                {
                    this.IncludingTypes.Add(t.FullName);
                }
              
                XmlHelper.SerializeFile(filePath, this, types);
            }
            catch (Exception ex)
            {

            }
        }
        public virtual object Load(string path, Func<List<string>, List<Type>> convertFunc)
        {
            var temp = XElement.Load(path);
            var typesEle = temp.GetElement("IncludingTypes") ;
            List<string> typeNames = new List<string>();
            foreach(var node in typesEle.Nodes())
            {
                typeNames.Add(((XElement)node).Value);
            }
 
            var types = convertFunc.Invoke(typeNames);
            var dataObj = XmlHelper.DeserializeFile(path, this.GetType(), types.ToArray());
            return dataObj;
        }
    }
}