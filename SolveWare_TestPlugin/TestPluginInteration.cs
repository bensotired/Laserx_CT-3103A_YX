namespace SolveWare_TestPlugin
{
    public class TestPluginInteration
    {
        public bool CanExecuteEngineeringAction
        {
            get
            {
                if (this.OnlineStatus == PluginOnlineStatus.Online)
                {
                    switch (RunStatus)
                    {
                        case PluginRunStatus.Idle:
                            {
                                return true;
                            }
                            break;
                        default:
                            return false;
                            break;
                    }
                }
                return false;
            }
        }
        public bool CanImportTestProfileOrResetPluginStatus
        {
            get
            {
                if(this.OnlineStatus == PluginOnlineStatus.Online)
                {
                    switch (RunStatus)
                    {
                        case PluginRunStatus.Running:
                            {
                                return false;
                            }
                            break;
                        default:
                            return true;
                            break;
                    }
                }
                return false;
            }
        }
        public bool NeedToNotifyUserRiskBeforeImportTestProfile
        {
            get
            {
                if (this.OnlineStatus == PluginOnlineStatus.Online)
                {
                    switch (RunStatus)
                    {
                        case PluginRunStatus.Error:
                        case PluginRunStatus.Invalid:
                        case PluginRunStatus.Stopped:
                            {
                                return true;
                            }
                            break;
                        default:
                            return false;
                            break;
                    }
                }
                return false;
            }
        }
        public string PluginStatusInfo
        {
            get
            {
                var info = $"[{this.Name}][{OnlineStatus}][{RunStatus}]";
                return info;
            }
        }
        public string Name { get; internal set; }
        public PluginRunStatus RunStatus { get;   set; }
        public PluginOnlineStatus OnlineStatus { get; internal set; }

    
    }
}