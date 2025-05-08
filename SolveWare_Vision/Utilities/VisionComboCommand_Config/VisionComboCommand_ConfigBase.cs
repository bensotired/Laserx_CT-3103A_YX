using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using System.Collections.Generic;
using System.IO;

namespace SolveWare_Vision
{
    public class VisionComboCommand_ConfigBase
    {
        public const string ConfigFileName = @"VisCboCmd.xml";
        public VisionComboCommand_ConfigBase()
        {
            this.CmdDict = new DataBook<string, List<string>>();
        }

        public DataBook<string, List<string>> CmdDict { get; set; }

        public virtual void Add(string cmdPurpose, string oneTemplateCmd)
        {
            if (this.CmdDict.ContainsKey(cmdPurpose))
            {
            } 
            else
            {
                List<string> cmdList = new List<string>();
                this.CmdDict.Add(cmdPurpose, cmdList);
            }
            if (this.CmdDict[cmdPurpose].Contains(oneTemplateCmd) == false)
            {
                this.CmdDict[cmdPurpose].Add(oneTemplateCmd);
            }
        }
        public virtual List<string> this[string cmdPurpose]
        {
            get
            {
                if (this.CmdDict.ContainsKey(cmdPurpose))
                {
                    return this.CmdDict[cmdPurpose];
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
        public static TVisionComboCommand_Config  Load<TVisionComboCommand_Config>(string path)where TVisionComboCommand_Config : VisionComboCommand_ConfigBase
        {
            return XmlHelper.DeserializeFile<TVisionComboCommand_Config>(path);
        }
    }
}
