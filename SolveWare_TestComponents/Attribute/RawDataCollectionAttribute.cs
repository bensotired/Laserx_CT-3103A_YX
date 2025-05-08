using SolveWare_BurnInCommon;
using System;

namespace SolveWare_TestComponents.Attributes
{
 
    [AttributeUsage(AttributeTargets.Property)]
    public class RawDataCollectionAttribute : Attribute
    {
        public RawDataCollectionAttribute( ) 
        {
    
        }

    }
    [AttributeUsage(AttributeTargets.Property)]
    public class RawDataCollectionItemElementAttribute : Attribute
    {
        public RawDataCollectionItemElementAttribute(string elementTag)
        {
            this.ElementTag = elementTag;
        }
        public string ElementTag { get; private set; }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class RawDataChartAxisElementAttribute : Attribute
    {
        //
        // 摘要:
        //     初始化 System.Xml.Serialization.XmlIgnoreAttribute 类的新实例。
        public RawDataChartAxisElementAttribute(CEAxisXY chartAxisXY)//,int chartSeriesID)
        {
            this.ChartAxisXY = chartAxisXY;
        }
 
        public CEAxisXY ChartAxisXY { get; protected set; }
     
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class RawDataBrowsableElementAttribute : Attribute
    {
        //
        // 摘要:
        //     初始化 System.Xml.Serialization.XmlIgnoreAttribute 类的新实例。
        public RawDataBrowsableElementAttribute( )//,int chartSeriesID)
        {
          
        }

    }
    [AttributeUsage(AttributeTargets.Property)]
    public class RawDataPrintableElementAttribute : Attribute
    {
        //
        // 摘要:
        //     初始化 System.Xml.Serialization.XmlIgnoreAttribute 类的新实例。
        public RawDataPrintableElementAttribute()//,int chartSeriesID)
        {

        }

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class RawDataDrawMulitipeLinesElementAttribute : Attribute
    {
        /// <summary>
        /// ZedGraphChart  多线条绘图
        /// </summary>
        public RawDataDrawMulitipeLinesElementAttribute()
        {

        }
    }
}