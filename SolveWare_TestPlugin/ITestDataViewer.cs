using SolveWare_BurnInAppInterface;
using SolveWare_TestComponents.Data;

namespace SolveWare_TestPlugin
{

    public interface ITestDataViewer : ITesterCoreLink, IAccessPermissionLevel
    {
        void UpdateMainStreamData(IMajorStreamData majorData, string targetDeviceSn);
        void UpdateMainStreamData(IMajorStreamData majorData);
        void Clear();
    }
   
 
}