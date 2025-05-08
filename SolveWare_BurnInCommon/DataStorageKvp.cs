using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInCommon
{
    public class DataStorageKvp:IDisposable
    {

        public string StorageFile { get; set; }
        public object DataObject { get; set; }

        public void Dispose()
        {
            this.StorageFile = string.Empty;
            this.DataObject = null;
        }
    }
}