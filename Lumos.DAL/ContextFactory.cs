using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.DAL
{
    ///// <summary>
    ///// 上下文简单工厂
    ///// <remarks>
    ///// 创建：2014.02.05
    ///// </remarks>
    ///// </summary>
    //public class DbContextFactory
    //{
    //    /// <summary>
    //    /// 获取当前数据上下文
    //    /// </summary>
    //    /// <returns></returns>
    //    public static TestDbContext GetTestDb()
    //    {
    //        TestDbContext _nContext = CallContext.GetData("TestContext") as TestDbContext;
    //        if (_nContext == null)
    //        {
    //            _nContext = new TestDbContext();
    //            CallContext.SetData("TestContext", _nContext);
    //        }
    //        return _nContext;
    //    }
    //}
}
