using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using System.Windows.Forms;

namespace SolveWare_TestPlugin
{
    public interface ITestPluginWorkerBase : ITestPluginRuntimeOverview
    {
        Form GetRuntimeOverviewUI(bool isDockable);
        void UpdateMainStreamDataToUI();
        TestPluginResourceProvider GetPluginLocalizedResourceProvider();
        TestPluginInteration Interation { get; }

        //bool SwitchProductConfig(string prodName);
    }
}