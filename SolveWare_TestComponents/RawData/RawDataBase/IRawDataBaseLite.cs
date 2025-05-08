using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_TestComponents.Data
{
    public interface IRawDataBaseLite
    {
        string Name { get; set; }
        void SetRawDataFixFormat(string preFix, string postFix);
        string RawDataFixFormat { get; set; }
        DateTime TestStepStartTime { get; set; }
        DateTime TestStepEndTime { get; set; }
        TimeSpan TestCostTimeSpan { get; set; }
        void PrintToCSV(string filePath, bool append = false);
        //void PrintToCSV(string filePath);
        bool ReloadFormString(string reloadDataString);
        bool ReloadFormString(string[] reloadDataString);
    }
}
