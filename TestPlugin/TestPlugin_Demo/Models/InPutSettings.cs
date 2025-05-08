using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestPlugin_Demo
{
    [Serializable]
    public class InPutSettings
    {
        public InPutSettings()
        {
            
            this.Feed = new DataBook<FeedBox, DataBook<ProductPosition, double>>();
            DataBook<ProductPosition, double> dataPairs = new DataBook<ProductPosition, double>();
            foreach (string name in Enum.GetNames(typeof(ProductPosition)))
            {
                dataPairs.Add((ProductPosition)Enum.Parse(typeof(ProductPosition), name), 0);
            }
            foreach (string name in Enum.GetNames(typeof(FeedBox)))
            {
                this.Feed.Add((FeedBox)Enum.Parse(typeof(FeedBox), name), dataPairs);
            }

        }

        public DataBook<FeedBox, DataBook<ProductPosition, double>> Feed { get; set; }


        
    }
}
