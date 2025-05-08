namespace SolveWare_TestPlugin
{

    public enum PluginRunStatus//单元运行状态
    {
        NotHomeYet,              //整机未复位
        Idle,                  //空闲
        Ready,                 //已正确加载老化任务
        Running,               //在进行老化任务
        Finished,              //老化任务已完成
        Error,                 //运行异常 
        Stopped,               //老化任务已经停止
        Invalid,
    }
    public enum PluginOnlineStatus//单元在线状态 
    {
        Online,
        Offline,
    }
 
}