using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using LX_BurnInSolution.Utilities;

namespace SolveWare_BurnInLog
{
    public class CommonLogger 
    {
        public string PlatformLogFile { get; private set; }
        StreamWriter Writer { get; set; }
        int LogCounter { get; set; }
        public bool HighlightException { get; set; }
        public CommonLogger()
        {
            this.Writer = null;
        }
        public void Initialize(string rootFolder, string platformLogFile, bool isHighlightException)
        {

            if (string.IsNullOrEmpty(rootFolder) == false)
            {
                if (string.IsNullOrEmpty( platformLogFile))
                {
                    this.PlatformLogFile = Path.Combine(rootFolder + @"\Log\TestStationLog.txt");
                }
                else
                {
                    this.PlatformLogFile = Path.Combine(rootFolder + platformLogFile);
                }
            }

            this.HighlightException = isHighlightException;

            string fileDir = Path.GetDirectoryName(this.PlatformLogFile);
            if (!Directory.Exists(fileDir))
            {
                Directory.CreateDirectory(fileDir);
            }
            if (!File.Exists(this.PlatformLogFile))
            {
                File.Create(this.PlatformLogFile).Close();
            }
            else
            {
                this.IsLogBackupDone();
            }
            this.Writer = new StreamWriter(this.PlatformLogFile, true,Encoding.Default);
            this.Writer.WriteLine();
            this.Writer.WriteLine("##===================================================================##");
            this.Writer.Flush ();
        }
        //public void Initialize(XElement configuration, string rootFolder)
        //{
        //    this.PlatformLogFile = Path.Combine(rootFolder + @"\Log\TestStationLog.txt");
        //    if(configuration.Attribute("LogFileName") != null)
        //    {
        //        this.PlatformLogFile = Path.Combine(rootFolder + configuration.GetAttribute("LogFileName").Value);
        //    }
        //    this.HighlightException = true;
        //    if (configuration.Attribute("HighlightException") != null)
        //    {
        //        this.HighlightException = Convert.ToBoolean(configuration.GetAttribute("HighlightException").Value);
        //    }
        //    string fileDir = Path.GetDirectoryName(this.PlatformLogFile);
        //    if (!Directory.Exists(fileDir))
        //    {
        //        Directory.CreateDirectory(fileDir);
        //    }
        //    if (!File.Exists(this.PlatformLogFile))
        //    {
        //        File.Create(this.PlatformLogFile).Close();
        //    }
        //    else
        //    {
        //        this.IsLogBackupDone();
        //    }
        //    this.Writer = new StreamWriter(this.PlatformLogFile, true);
        //    this.Writer.WriteLine();
        //    this.Writer.WriteLine("##===================================================================##");
        //    this.Writer.Flush();
        //}

        private bool IsLogBackupDone()
        {
            bool isBackup = false;
            FileInfo fileInfo = new FileInfo(this.PlatformLogFile);
            if (fileInfo.Length > 1024 * 1024 * 5)//5m
            {
                if (this.Writer != null)
                {
                    this.Writer.Close();
                }
                this.LogCounter = 0;
                string backupFileName = fileInfo.FullName;
                backupFileName = backupFileName.Remove(backupFileName.Length - fileInfo.Extension.Length);
                backupFileName = backupFileName + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".BK.txt";
                fileInfo.MoveTo(backupFileName);
                isBackup = true;
            }
            return isBackup;
        }
        public void Log(string message)
        {
           
            if (message.ToLower().Contains("exception") && this.HighlightException)
            {
                this.Writer.WriteLine("************************************EXCEPTION START************************************");
                this.Writer.WriteLine(message);
                this.Writer.WriteLine("************************************EXCEPTION END**************************************");
            }
            else
            {
                this.Writer.WriteLine(message);
            }
         
            this.LogCounter++;
            if (this.LogCounter > 1000)
            {
                if (this.IsLogBackupDone())
                {
                    this.Writer = new StreamWriter(this.PlatformLogFile, true, Encoding.Default);
                    //this.Writer = new StreamWriter(this.PlatformLogFile, true);
                    this.Writer.WriteLine();
                    this.Writer.WriteLine("##===================================================================##");
                    //this.Writer.FlushAsync();
                }
            }  
            this.Writer.Flush ();
            Thread.Sleep(0);
        }

        public void Dispose()
        {
            this.Writer.Close();
        }
    }
}
