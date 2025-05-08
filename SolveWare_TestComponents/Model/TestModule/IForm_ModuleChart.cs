using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace SolveWare_TestComponents.Model
{
    public interface IForm_ModuleChart
    {
        void UpdateTitle(string title);
        void ClearChart();
        //void Rest_AxisX();
        void UpdateChartSeries(AxisType axisType, string legendName, IEnumerable<object> xData, IEnumerable<object> yData);
    }
}