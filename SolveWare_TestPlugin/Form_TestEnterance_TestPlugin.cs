using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System.Windows.Forms;
using System;

namespace SolveWare_TestPlugin
{
    public   partial class Form_TestEnterance_TestPlugin<TTestPlugin> : Form, ITesterAppUI, IAccessPermissionLevel where TTestPlugin : class, ITesterAppPluginInteration
    {
        protected ITesterCoreInteration _core;
        protected TTestPlugin _plugin;
        protected Form _TestProfileEditorUI;
        public Form_TestEnterance_TestPlugin()
        {
            InitializeComponent();
        }
        private void Form_TestEnterance_TestPlugin_Load(object sender, System.EventArgs e)
        {
            //this.CreateTestProfileEditorUI();
        }

        protected virtual void CreateTestProfileEditorUI() { throw new NotImplementedException(); }

        public virtual AccessPermissionLevel APL
        {
            get { return AccessPermissionLevel.None; }
        }

        public virtual void ConnectToAppInteration(ITesterAppPluginInteration app)
        {
            _plugin = (TTestPlugin)app;
        }

        public virtual void ConnectToCore(ITesterCoreInteration core)
        {
            _core = core as ITesterCoreInteration;
            _core.SendOutFormCoreToGUIEvent -= ReceiveMessageFromCore;
            _core.SendOutFormCoreToGUIEvent += ReceiveMessageFromCore; 
        }

        public virtual void DisconnectFromCore(ITesterCoreInteration core)
        {
            _core.SendOutFormCoreToGUIEvent -= ReceiveMessageFromCore;
            _core = null;
        }

        public virtual void RefreshOnce()
        {
             
        }

        protected virtual void ReceiveMessageFromCore(IMessage message)
        {
        }

    
    }
}
