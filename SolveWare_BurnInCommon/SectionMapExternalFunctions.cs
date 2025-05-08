using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInCommon
{
    public static class SectionMapExternalFunctions
    {
        public static SectionItem GetSectionItem(this List<SectionItem> sectionList, string sectionName)
        {
            if (sectionList.Exists(item => item.Name == sectionName))
            {
                return sectionList.Find(item => item.Name == sectionName);
            }
            else
            {
                throw new Exception("Section item not found exception.");
            }
        }
        public static int GetSectionChannel(this List<SectionItem> sectionList, string sectionName)
        {
            if (sectionList.Exists(item => item.Name == sectionName))
            {
                return sectionList.Find(item => item.Name == sectionName).InstChannel;
            }
            return -1;
        }
        public static string GetSectionDriverName(this List<SectionItem> sectionList, string sectionName)
        {
            if (sectionList.Exists(item => item.Name == sectionName))
            {
                return sectionList.Find(item => item.Name == sectionName).InstKey;
            }
            return "NaN";
        }
    }
}