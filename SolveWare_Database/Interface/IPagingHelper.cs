using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_TestDatabase.Interface
{
    public interface IPagingHelper
    {
        /// <summary>
        /// 获取分页SQL语句，默认row_number为关健字，所有表不允许使用该字段名
        /// </summary>
        /// <param name="recordCount">记录总数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="safeSql">SQL查询语句</param>
        /// <param name="orderField">排序字段，多个则用“,”隔开</param>
        /// <returns>分页SQL语句</returns>
        string CreatePagingSql(int recordCount, int pageSize, int pageIndex, string safeSql, string orderField);

        /// <summary>
        /// 获取记录总数SQL语句
        /// </summary>
        /// <param name="safeSql">SQL查询语句</param>
        /// <returns>记录总数SQL语句</returns>
        string CreateCountingSql(string safeSql);
    }
}
