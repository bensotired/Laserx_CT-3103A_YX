using SolveWare_TestDatabase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_TestDatabase
{
    /// <summary>
    /// 底层库直接配置： 数据库机型可配置
    /// 1、数据库配置文件
    /// 2、ini配置文件配置-不安全
    /// </summary>
    public interface ITestDatabaseManager
    {

        /// <summary>
        /// 自动创建测试阶段数据存储表:PreBI、PostBI、Golden等
        /// </summary>
        /// <returns></returns>
        bool ICreateTestPurposeTable(string testPurpose);

        /// <summary>
        /// 通过指定测试阶段表获取指定参数值
        /// </summary>
        /// <returns></returns>
        string ISelectParmByTestPurpose(string testPurpose);

        /// <summary>
        /// 查找当前参数名称是否存在表字段中
        /// </summary>
        /// <returns></returns>
        bool IExistsParmName(string parmName);

        /// <summary>
        /// 对比两个参数并返回对比值delta
        /// </summary>  
        double CalculateDelta(ParmaData data1, ParmaData data2);



        
    }
}
