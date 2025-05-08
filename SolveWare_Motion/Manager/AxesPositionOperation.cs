using System;
using System.Collections.Generic;

namespace SolveWare_Motion
{
    [Serializable]
    public partial class AxesPosition
    {

        public static AxesPosition operator + (AxesPosition ap_1, AxesPosition ap_2)
        {
            
            AxesPosition newAp = new AxesPosition();
            newAp.Name = $"{ap_1.Name} + {ap_2.Name}";
            List<string> subApNamesToOpera = new List<string>(); 
            foreach (var subAp_1 in ap_1)
            {
                if(ap_2.ItemCollection.Exists(item=>item.Name == subAp_1.Name))
                {

                    var subAp_2 = ap_2.ItemCollection.Find(item => item.Name == subAp_1.Name);
                    var newPosition = subAp_1.Position + subAp_2.Position;

                    AxisPosition newSubAp = new AxisPosition();
                    newSubAp.Name = subAp_1.Name;
                    newSubAp.Position = newPosition;
                    newSubAp.CardNo = subAp_1.CardNo;
                    newSubAp.AxisNo = subAp_1.AxisNo;

                    newAp.AddSingleItem(newSubAp);
                }
                else
                {
                    newAp.AddSingleItem(subAp_1);
                }
            }

            foreach (var subAp_2 in ap_2)
            {
                if (ap_1.ItemCollection.Exists(item => item.Name == subAp_2.Name))
                {
                }
                else
                {
                    newAp.AddSingleItem(subAp_2);
                }
            }


            return newAp;
        }
        public static AxesPosition operator -(AxesPosition ap_1, AxesPosition ap_2)
        {

            AxesPosition newAp = new AxesPosition();
            newAp.Name = $"{ap_1.Name} - {ap_2.Name}";
            List<string> subApNamesToOpera = new List<string>();
            foreach (var subAp_1 in ap_1)
            {
                if (ap_2.ItemCollection.Exists(item => item.Name == subAp_1.Name))
                {

                    var subAp_2 = ap_2.ItemCollection.Find(item => item.Name == subAp_1.Name);
                    var newPosition = subAp_1.Position - subAp_2.Position;

                    AxisPosition newSubAp = new AxisPosition();
                    newSubAp.Name = subAp_1.Name;
                    newSubAp.Position = newPosition;
                    newSubAp.CardNo = subAp_1.CardNo;
                    newSubAp.AxisNo = subAp_1.AxisNo;

                    newAp.AddSingleItem(newSubAp);
                }
                else
                {
                    newAp.AddSingleItem(subAp_1);
                }
            }

            foreach (var subAp_2 in ap_2)
            {
                if (ap_1.ItemCollection.Exists(item => item.Name == subAp_2.Name))
                {
                }
                else
                {
                    newAp.AddSingleItem(subAp_2);
                }
            }


            return newAp;
        }
    }
}