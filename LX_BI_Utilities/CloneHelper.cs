using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LX_BurnInSolution.Utilities
{
    public static class CloneHelper
    {
        public static TObject Clone<TObject>(TObject obj)
        {
            TObject destObj;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, obj);
                    ms.Seek(0, SeekOrigin.Begin);
                    destObj = (TObject)bf.Deserialize(ms);
                }
                return destObj;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"EX[{ex}]_\r\\r\nnex.StackTrace[{ex.StackTrace}]");
                return default(TObject);
            }
        }
    }
}