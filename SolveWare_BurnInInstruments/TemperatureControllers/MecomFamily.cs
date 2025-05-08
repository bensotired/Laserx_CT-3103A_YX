
using MeSoft.MeCom.Core;
using MeSoft.MeCom.PhyWrapper;
using System;
using System.Threading;

namespace SolveWare_BurnInInstruments
{
    public class MecomFamily
    {
        public MecomFamily(IMeComPhy seed, byte deviceId)
        {
            //MeComPhyTcp = new MeComPhyTcp();
            //MeComPhySerialPort = new MeComPhySerialPort();
            this.IMeComPhySeed = seed;
            this.MeComQuerySet = new MeComQuerySet(seed);
            this.MeComQuerySet.SetDefaultDeviceAddress(deviceId);
            this.MeComQuerySet.SetIsReady(true);  //5020启动的时候将IsReady = ON

            this.MeComBasicCmd = new MeComBasicCmd(MeComQuerySet);
            this.MeComG2Cmd = new MeComG2Cmd(MeComQuerySet);
        }
        public IMeComPhy IMeComPhySeed { get; protected set; }
        public MeComQuerySet MeComQuerySet { get; protected set; } //通讯核心
        public MeComBasicCmd MeComBasicCmd { get; protected set; } //G1指令
        public MeComG2Cmd MeComG2Cmd { get; protected set; }   //G2指令
    }
 
}