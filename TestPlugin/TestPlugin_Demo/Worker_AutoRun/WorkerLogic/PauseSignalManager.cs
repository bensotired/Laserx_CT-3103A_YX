using System;
using System.Collections.Generic;
using System.Text;

namespace TestPlugin_Demo
{

    internal class PauseSignalManager
    {
        static PauseSignalManager _instance;
        static object _mutex = new object();
        Dictionary<MT, bool> SignalDict = new Dictionary<MT, bool>();
        internal PauseSignalManager()
        {

        }
        public static PauseSignalManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_mutex)
                    {
                        if (_instance == null)
                        {
                            _instance = new PauseSignalManager();
                        }
                    }
                }
                return _instance;
            }
        }
        internal void Initialize()
        {
            SignalDict.Clear();
            var mts = Enum.GetValues(typeof(MT));
            foreach (var mt in mts)
            {
                SignalDict.Add((MT)mt, false); ;
            }
        }
        internal bool IsSignalOwnerPausing(MT signalOwner)
        {
          return  SignalDict[signalOwner] ;
        }
        internal void Request_Pause(MT signalOwner)
        {
            SignalDict[signalOwner] = true;
        }
        internal void Request_Resume(MT signalOwner)
        {
            SignalDict[signalOwner] = false;
        }

        internal bool IsAnyOwnerPaused
        {
            get
            {
                if (SignalDict.ContainsValue(true))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        internal void Force_Clear_AllPauseSignal()
        {
            var mts = Enum.GetValues(typeof(MT));
            foreach (var mt in mts)
            {
                SignalDict[(MT)mt] = false;
            }

        }
        internal string PrintStatus()
        {
            StringBuilder strb = new StringBuilder();
            try
            {

                var mts = Enum.GetValues(typeof(MT));
                strb.AppendLine($"机台是否处于暂停状态 [{IsAnyOwnerPaused}]"); ;
                strb.AppendLine($"各模组暂停状态如下:");
                foreach (var mt in mts)
                {
                    strb.AppendLine($"[{mt}] 暂停信号 [{  SignalDict[(MT)mt] }]");

                }

            }
            catch (Exception ex)
            {

            }
            return strb.ToString();
        }
    }
}













 