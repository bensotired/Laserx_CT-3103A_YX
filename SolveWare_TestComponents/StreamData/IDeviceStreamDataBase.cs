using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;

namespace SolveWare_TestComponents.Data
{
    public interface IDeviceStreamDataBase : IStreamInfoBase
    {
        string SerialNumber { get;   }
        IDeviceInfoBase DeviceInfoBase { get;  }
        string GetSQL_HeaderValues();
        string GetSQL_Header();
        SummaryDataCollection SummaryDataCollection { get; set; }
        int RawDataCollecetionCount { get; }
        List<RawDataBaseLite> RawDataCollection { get; set; }
        void AddRawData(IRawDataBaseLite datum);
        void AddSummaryDataCollection(List<SummaryDatumItemBase> summaryDataCollection);
        void AddSingleSummaryData(SummaryDatumItemBase summaryData);
        void Clear();
        IEnumerator<RawDataBaseLite> GetEnumerator();



        string MaskName { get; set; }
        string WaferName { get; set; }
        string ChipName { get; set; }
        string OeskID { get; set; }
        double Tec1ActualTemp { get; set; }
        DateTime CurrentDateTime { get; set; }
        string CoarseTuningPath { get; set; }


    }
}