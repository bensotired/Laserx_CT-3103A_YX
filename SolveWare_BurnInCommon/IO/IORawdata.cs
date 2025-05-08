using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInCommon
{
    public class IORawdata
    {
        Dictionary<string, bool> ioRawdataDict = new Dictionary<string, bool>();
        public IORawdata()
        {

        }
           
        public void ModifyIoStatus(string ioKey, bool ioStatus)
        {
            if (ioRawdataDict.ContainsKey(ioKey))
            {
                ioRawdataDict[ioKey] = ioStatus;
            }
            else
            {
                ioRawdataDict.Add(ioKey, ioStatus);
            }
        }
        public bool GetIoStatus(string ioKey)
        {
            if (ioRawdataDict.ContainsKey(ioKey))
            {
                return ioRawdataDict[ioKey];
            }
            else
            {
                return false;
            }
        }
        public void Clear()
        {
            this.ioRawdataDict.Clear();
        }
    }
}