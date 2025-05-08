using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInCommon
{
    public class SectionItem
    {
        /// <summary>
        /// gai
        /// </summary>
        public string Name { get; set; }
        public string InstKey { get; set; }
        public int InstChannel { get; set; }
        public SectionItem()
        {

        }
        public SectionItem(SectionItem sourceItem)
        {
            this.Name = sourceItem.Name;
            this.InstKey = sourceItem.InstKey;
            this.InstChannel = sourceItem.InstChannel;
        }
        public SectionItem(string name, string instName, int instChannel)
        {
            this.Name = name;
            this.InstKey = instName;
            this.InstChannel = instChannel;
        }
    }
}