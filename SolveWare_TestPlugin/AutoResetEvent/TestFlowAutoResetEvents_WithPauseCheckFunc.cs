using SolveWare_BurnInCommon;
using System.Collections.Generic;

namespace SolveWare_TestPlugin
{
    public abstract class TestFlowAutoResetEvents_WithPauseCheckFunc : CURDBaseLite<AutoResetEventItem_WithPauseCheckFunc>
    {
        public TestFlowAutoResetEvents_WithPauseCheckFunc()
        {
            this.ItemCollection = new List<AutoResetEventItem_WithPauseCheckFunc>();
        }
        public abstract void Initialize();
        public virtual void Clear()
        {
            this.ItemCollection.Clear();
        }
        public AutoResetEventItem_WithPauseCheckFunc this[string name]
        {
            get
            {
                return this.GetSingleItem(name);
            }
        }

    }

}