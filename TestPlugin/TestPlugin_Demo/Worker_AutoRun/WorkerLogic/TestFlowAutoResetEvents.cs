using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using SolveWare_TestPlugin;
using System;
using System.Threading;

namespace TestPlugin_Demo
{
    
    public class TestFlowAutoResetEvents_CT3103 : TestFlowAutoResetEvents
    {
        public TestFlowAutoResetEvents_CT3103() : base() { }
        public override void Initialize()
        {
            var areTypes = Enum.GetNames(typeof(Action3103));
            foreach (var areT in areTypes)
            {
                this.AddSingleItem(new AutoResetEventItem()
                {
                    Name = areT
                });
            }
        }
        internal AutoResetEventItem this[Action3103 eName]
        {
            get
            {
                return this.GetSingleItem(eName.ToString());
            }
        }

    }
 

}
