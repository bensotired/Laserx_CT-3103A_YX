using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments 
{
    public static class VisionJsonDataConverter
    {
        public static TResult ConvertJsonDataTo<TResult>(string jsonStringData)
        {
            if (string.IsNullOrEmpty(jsonStringData))
            {
                return default(TResult);
            }
            var data = JsonConvert.DeserializeObject(jsonStringData, typeof(TResult));
            return (TResult)data;
        }
    }
}
