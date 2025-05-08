using LX_BurnInSolution.Utilities;
using System;

namespace SolveWare_TestComponents.Data
{

    public class TestPluginImportProfileBase : ITestPluginImportProfileBase
    {
        public TestPluginImportProfileBase()
        {
            this.Name = "TestPluginImportProfile";
            this.CreateTime = BaseDataConverter.ConvertDateTimeToCommentString(DateTime.Now);
            this.LastModifyTime = BaseDataConverter.ConvertDateTimeToCommentString(DateTime.Now);
        }
        public string Name { get; set; }
        public virtual string CreateTime { get; set; }
        public virtual string LastModifyTime { get; set; }
        public virtual bool Check(out string checkLog,params object[] args)
        {
            throw new NotImplementedException();
        }
        public virtual void Save(string filePath)
        {
            this.LastModifyTime = BaseDataConverter.ConvertDateTimeToCommentString(DateTime.Now);
            XmlHelper.SerializeFile(filePath, this);
        }

        public virtual void Clear()
        {
            throw new NotImplementedException(); 
        }
    }
}