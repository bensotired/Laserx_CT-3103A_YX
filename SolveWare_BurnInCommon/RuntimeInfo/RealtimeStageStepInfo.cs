using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInCommon
{
    public class RealtimeStageStepInfo
    {
        public string StageStepIDInfo { get; private set; }
        public string StageStepStartTimeInfo { get; private set; }
        public string StageStepRestTimeInfo { get; private set; }
        public string StageStepElapsedTimeInfo { get; private set; }
        public string StageStepEstimateEndTimeInfo { get; private set; }


        public virtual void ModifyStageStepIDInfo(string info)
        {
            this.StageStepIDInfo = info;
        }

        public virtual void ModifyStageStepStartTimeInfo(DateTime info)
        {
            this.StageStepStartTimeInfo = info.ToString("yyyy/MM/dd HH:mm:ss");
        }
        public virtual void ModifyStageStepRestTimeInfo(TimeSpan info)
        {
            this.StageStepRestTimeInfo = $"[{info.Days * 24 + info.Hours}]小时[{ info.Minutes}]分钟[{ info.Seconds}]秒";
        }
        public virtual void ModifyStageStepElapsedTimeInfo(TimeSpan info)
        {
            this.StageStepElapsedTimeInfo = $"[{info.Days * 24 + info.Hours}]小时[{ info.Minutes}]分钟[{ info.Seconds}]秒";
        }
        public virtual void ModifyStageStepEstimateEndTimeInfo(DateTime info)
        {
            this.StageStepEstimateEndTimeInfo = info.ToString("yyyy/MM/dd HH:mm:ss");
        }

        public void Clear()
        {
            this.StageStepIDInfo = string.Empty;
            this.StageStepStartTimeInfo = string.Empty;
            this.StageStepRestTimeInfo = string.Empty;
            this.StageStepEstimateEndTimeInfo = string.Empty;
            this.StageStepElapsedTimeInfo = string.Empty;
        }
    }
}