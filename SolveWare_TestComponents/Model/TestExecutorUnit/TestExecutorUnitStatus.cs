using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_TestComponents.Model
{

    public enum TestExecutorUnitStatus//单元运行状态
    {
        Idle,                  //空闲
        Ready,                 //已正确加载老化任务
        Running,               //在进行老化任务
        Finished,              //老化任务已完成
        Error,                 //运行异常 
        Stopped,               //老化任务已经停止
        Invalid,
    }
}