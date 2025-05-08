using SolveWare_BurnInCommon;
using System;

namespace SolveWare_Motion
{
    [Serializable]
    public class  AxisPosition : CURDItem, ICURDItem
    {
        //可以通过位置模块来确定轴
        //序列化的时候已经有ID 和
        public AxisPosition()
        {
           
        }
        public string CardNo { get; set; }//卡号
        public string AxisNo { get; set; }//轴号
        public double Position { get; set;}

    }
   
}