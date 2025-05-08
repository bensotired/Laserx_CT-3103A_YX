using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SolveWare_BurnInAppInterface
{

    public interface ITesterCoreHandleUIAction : /*ITesterAssembly,*/ ILogHandle, IExceptionHandle
    {
        void SendToUI(IMessage messageToUI);
        void GUIRunUIInvokeAction(Action guiInvokeAction);
        void GUIRunUIInvokeActionSYNC(Action guiInvokeAction);
        object GUIRunUIInvokeFunction(Func<object> guiInvokeAction);
        //void PopUI(GUIType gT);
        void PopUI(object uiObject);
        void PopUI_DefaultSize(object uiObject);
        void DockingMessageBoard();
        //void DockingNanoTrakBoard();
        void DockingStationBoard();
        void DockingTestFrameBoard();
        void PopUIForAppPlugin(ITesterAppPluginInteration plugin);
        void DockUIForAppPlugin(ITesterAppPluginInteration plugin);
        void ModifyDockableUI(Form _UI, bool isDockable);
        void RefreshListView(ListView listView, Dictionary<string, string> files);
        TreeNode Convert_TestExecutorComboToTreeNode(object comboConfig);
    }
}