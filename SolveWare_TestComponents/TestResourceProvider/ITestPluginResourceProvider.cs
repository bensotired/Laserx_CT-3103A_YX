using System.Collections.Generic;
using SolveWare_BurnInAppInterface;

namespace SolveWare_TestComponents.ResourceProvider
{
    public interface ITestPluginResourceProvider
    {
        IAxesPositionResourceProvider Local_AxesPosition_ResourceProvider { get; }
        IAxisResourceProvider Local_Axis_ResourceProvider { get; }
        IIOResourceProvider Local_IO_ResourceProvider { get; }
        ISpecResourceProvider Local_Spec_ResourceProvider { get; }
        IVisionResourceProvider Local_Vision_ResourceProvider { get; }
        Dictionary<string, object> Resource_Axes { get; }
        Dictionary<string, object> Resource_Instruments { get; }
        Dictionary<string, object> Resource_IO { get; }
        Dictionary<string, object> Resource_Posititon { get; }
        Dictionary<string, object> Resource_VisionController { get; }
    }
}