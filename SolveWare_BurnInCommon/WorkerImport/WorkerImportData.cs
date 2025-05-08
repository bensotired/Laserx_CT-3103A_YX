

using MessagePack;
using System;

namespace SolveWare_BurnInCommon
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class WorkerImportData : IWorkerImportData
    {
        public string TaskNo { get; set; }
        /// <summary>
        /// 导入数据目标单元名称
        /// </summary>
         public string UnitName { get; set; }
        /// <summary>
        /// 导入数据目标单元类型
        /// </summary>
        public string UnitType { get; set; }
        public int UnitSlotCount { get; set; }
        public int UnitCarrierCount { get; set; }
        public int UnitFixtureCount { get; set; }
 
        /// <summary>
        /// 导入老化计划类型
        /// </summary>
        public string PlanType { get; set; }
        /// <summary>
        /// 老化产品信息原始备份 - 作为原始老化槽位信息记录 老化任务运行过程中不应改变此备份任何状态,若有退出老化预设动作需求可以调用此备份产品信息进行退出动作
        /// 如（降温到退出温度等)
        /// </summary>
         public UnitSlotsInfo UnitSlotsInfo_Original { get; set; }//
        /// <summary>
        /// 老化产品信息运行时备份 - 老化任务运行过程中改变老化槽位isValid状态来控制该槽位是否继续运行接下来的运行状态
        /// </summary>
         public UnitSlotsInfo UnitSlotsInfo_RunTime { get; set; }//
        /// <summary>
        /// 老化计划轻量级实例 - 作为泛型老化计划的句柄
        /// </summary>
        public BurnInPlanLite ImportPlan { get; set; }
        //public object ImportPlan { get; set; }
        /// <summary>
        /// 老化步骤链表导入实例 - 为加载老化运行链表提供信息
        /// </summary>
        public BurnInStepCombo StepCombo { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkerImportData()
        {
            this.TaskNo = "DefaultTaskNo";
            this.ImportPlan = new BurnInPlanLite();
            this.UnitSlotsInfo_Original = new UnitSlotsInfo();
            this.UnitSlotsInfo_RunTime = new UnitSlotsInfo();
            this.StepCombo = new BurnInStepCombo();
        }

        /// <summary>
        /// 更新内容方法 包括老化计划/产品信息/老化步骤链表信息
        /// </summary>
        /// <param name="importPlan"></param>
        /// <param name="slotsInfo"></param>
        /// <param name="stepCombo"></param>
        public void Update(IBurnInPlanLite importPlan, UnitSlotsInfo slotsInfo, BurnInStepCombo stepCombo)
        {
            this.ImportPlan = importPlan as BurnInPlanLite;

            this.StepCombo = stepCombo;
            this.UnitSlotsInfo_Original.Clear();
            this.UnitSlotsInfo_RunTime.Clear();
            foreach (var sinfo in slotsInfo)
            {
                this.UnitSlotsInfo_Original.Add(new SlotInfoBaseItem()
                {
                    Slot = sinfo.Slot,
                    SerialNumber = sinfo.SerialNumber,
                    IsValid = sinfo.IsValid,
                    CarrierNumber = sinfo.CarrierNumber?.ToString(),
                    FixtureNumber = sinfo.FixtureNumber?.ToString()
                });
                this.UnitSlotsInfo_RunTime.Add(new SlotInfoBaseItem()
                {
                    Slot = sinfo.Slot,
                    SerialNumber = sinfo.SerialNumber,
                    IsValid = sinfo.IsValid,
                    CarrierNumber = sinfo.CarrierNumber?.ToString(),
                    FixtureNumber = sinfo.FixtureNumber?.ToString()
                });
            }
        }
        /// <summary>
        /// 设置对应老化槽位有效状态
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="isValid"></param>
        public void SetSlotValidStatus(int slot, bool isValid)
        {
            this.UnitSlotsInfo_RunTime.SetSlotValidStatus(slot, isValid);
        }
        /// <summary>
        /// 设置对应老化槽位有效状态
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="isValid"></param>
        public void SetSlotValidStatus(int slot, string carrierNumber, bool isValid)
        {
            this.UnitSlotsInfo_RunTime.SetSlotValidStatus(slot, carrierNumber, isValid);
        }
        /// <summary>
        /// 设置对应老化槽位有效状态
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="isValid"></param> 
        public void SetSlotValidStatus(int slot, string carrierNumber, string fixtureNumber, bool isValid)
        {
            this.UnitSlotsInfo_RunTime.SetSlotValidStatus(slot, carrierNumber, fixtureNumber, isValid);
        }
        [Obsolete("no use")]
        public WorkerImportData Clone()
        {
            return (WorkerImportData)this.MemberwiseClone();
        }
    }
}