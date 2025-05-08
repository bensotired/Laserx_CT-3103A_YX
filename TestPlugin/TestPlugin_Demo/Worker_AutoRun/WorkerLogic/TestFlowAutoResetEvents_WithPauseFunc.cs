using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using SolveWare_TestPlugin;
using System;
using System.Threading;

namespace TestPlugin_Demo
{
 
    public class TestFlowAutoResetEvents_WithPauseFunc_CT3103 : TestFlowAutoResetEvents_WithPauseCheckFunc
    {
        public TestFlowAutoResetEvents_WithPauseFunc_CT3103() : base() { }
        public Func<bool> PauseFunc { get; set; }
        public override void Initialize()
        {
            if (PauseFunc == null)
            {
                throw new Exception("TestFlowAutoResetEvents_WithPauseFunc 中 PasueFunc不能为空!");
            }
            var areTypes = Enum.GetNames(typeof(Action3103));
            foreach (var areT in areTypes)
            {
                this.AddSingleItem(new AutoResetEventItem_WithPauseCheckFunc(PauseFunc)
                {
                    Name = areT
                });
            }
        }
        internal AutoResetEventItem_WithPauseCheckFunc this[Action3103 eName]
        {
            get
            {
                return this.GetSingleItem(eName.ToString());
            }
        }

    }

}
