using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace SolveWare_TestPlugin
{
    
    public abstract  class TestFlowAutoResetEvents : CURDBaseLite<AutoResetEventItem>
    {
        public TestFlowAutoResetEvents()
        {
            this.ItemCollection = new List<AutoResetEventItem>();
        }
        public abstract void Initialize();
        public virtual void Clear()
        {
            this.ItemCollection.Clear();
        }
        public  AutoResetEventItem this[string name]
        {
            get
            {
                return this.GetSingleItem(name);
            }
        }

    }
}