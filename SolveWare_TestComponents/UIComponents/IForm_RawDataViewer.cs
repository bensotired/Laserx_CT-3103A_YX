using SolveWare_TestComponents.Data;

namespace SolveWare_TestComponents.UIComponents
{
    public interface IForm_RawDataViewer
    {
        bool FFChartSaveImage_SW { get; set; }
        string SaveImagePath { get; set; }
        string SN { get; set; }

        void ClearContext();
        void ImportRawData(IRawDataBaseLite rawData);
    }
}