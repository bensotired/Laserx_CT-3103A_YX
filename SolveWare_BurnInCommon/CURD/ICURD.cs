using System;

namespace SolveWare_BurnInCommon
{
    public interface ICURD<ICURDItem>  
    {
        bool AddSingleItem(ICURDItem item, ref string sErr);
        bool DeleteSingleItem(ICURDItem item, ref string sErr);
        bool DeleteSingleItem(long Id, ref string sErr);
        bool UpdateSingleItem(ICURDItem item, ref string sErr);
        ICURDItem GetSingleItem(string name);
        ICURDItem GetSingleItem(long id);
        void Clear();
    }
}