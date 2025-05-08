using System;

namespace SolveWare_BurnInCommon
{
    public interface ICURDLite<ICURDItemLite>
    {
        bool AddSingleItem(ICURDItemLite item, ref string sErr);
        bool DeleteSingleItem(ICURDItemLite item, ref string sErr);
        bool UpdateSingleItem(ICURDItemLite item, ref string sErr);
        ICURDItemLite GetSingleItem(string name);
    }
}