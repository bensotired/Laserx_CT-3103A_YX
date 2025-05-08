using System;
using System.Collections.Generic;

namespace SolveWare_BurnInCommon
{
    public enum CEAxisXY
    {
        /// <summary>
        /// 下x轴
        /// </summary>
        X,
        /// <summary>
        /// 上x轴
        /// </summary>
        X2,
        /// <summary>
        /// 左Y轴
        /// </summary>
        Y,
        /// <summary>
        /// 右Y轴
        /// </summary>
        Y2
    }
    
    public enum CEClass
    {    
        /// <summary>
        /// 扫描图区
        /// </summary>
        SWP_LIV,
        /// <summary>
        /// LD图区
        /// </summary>
        LD,
        /// <summary>
        /// EA图区
        /// </summary>
        EA,
        /// <summary>
        /// SOA图区
        /// </summary>
        SOA,
        /// <summary>
        /// 电流图区
        /// </summary>
        I,
        /// <summary>
        /// 电压图区
        /// </summary>
        V,
        /// <summary>
        /// 功率图区
        /// </summary>
        P,
        /// <summary>
        /// 温度图区
        /// </summary>
        T
    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public class PropChartElementAttribute : Attribute
    {
        //
        // 摘要:
        //     初始化 System.Xml.Serialization.XmlIgnoreAttribute 类的新实例。
        public PropChartElementAttribute(CEClass chartClass, CEAxisXY chartAxisXY)//,int chartSeriesID)
        {
            this.ChartAxisXY = chartAxisXY;
            this.ChartClass = chartClass;
            //this.ChartSeriesID = chartSeriesID;
        }
        public CEAxisXY ChartAxisXY { get; protected set; }
        public CEClass ChartClass { get; protected set; }
        //public int ChartSeriesID { get; protected set; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public class PropSweepChartElementAttribute : Attribute
    {
        //
        // 摘要:
        //     初始化 System.Xml.Serialization.XmlIgnoreAttribute 类的新实例。
        public PropSweepChartElementAttribute(CEClass chartClass, CEAxisXY chartAxisXY)//,int chartSeriesID)
        {
            this.ChartAxisXY = chartAxisXY;
            this.ChartClass = chartClass;
            //this.ChartSeriesID = chartSeriesID;
        }
        public CEAxisXY ChartAxisXY { get; protected set; }
        public CEClass ChartClass { get; protected set; }
        //public int ChartSeriesID { get; protected set; }
    }
}