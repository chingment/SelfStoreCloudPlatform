using Lumos.DAL;
using System;
using System.Linq;
using System.Transactions;

namespace Lumos.BLL
{
    public class SnUtil
    {

        private static readonly object lock_GetIncrNum = new object();

        private static int GetIncrNum()
        {

            lock (lock_GetIncrNum)
            {
                try
                {
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        LumosDbContext _dbContext = new LumosDbContext();

                        string date = DateTime.Now.ToString("yyyy-MM-dd");

                        var bizSn = _dbContext.BizSn.Where(m => m.IncrDate == date).FirstOrDefault();
                        if (bizSn == null)
                        {
                            bizSn = new Entity.BizSn();
                            bizSn.IncrNum = 0;
                            bizSn.IncrDate = date;
                            _dbContext.BizSn.Add(bizSn);
                            _dbContext.SaveChanges();
                        }

                        bizSn.IncrNum += 1;

                        _dbContext.SaveChanges();
                        ts.Complete();

                        return bizSn.IncrNum;
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error("业务流水号生成发生异常，错误编码：005", ex);

                    throw new Exception("业务流水号生成发生异常，错误编码：005");
                }
            }

        }

        public static string Build(Entity.Enumeration.BizSnType snType, string userId)
        {

            string prefix = "";

            switch (snType)
            {
                case Entity.Enumeration.BizSnType.Order:
                    prefix = "61";
                    break;
                //case Entity.Enumeration.BizSnType.FundTrans:
                //    prefix = "10";
                //    break;
                //case Entity.Enumeration.BizSnType.Withraw:
                //    prefix = "20";
                //    break;
                case Entity.Enumeration.BizSnType.Order2StockIn:
                    prefix = "30";
                    break;
                case Entity.Enumeration.BizSnType.Order2StockOut:
                    prefix = "31";
                    break;
            }

            ThreadSafeRandom ran = new ThreadSafeRandom();


            string part0 = ran.Next(100, 999).ToString();
            string part1 = DateTime.Now.ToString("yyyyMMddHHmmss");
            string part2 = GetIncrNum().ToString().PadLeft(5, '0');

            string sn = prefix + part2 + part1 + part0;
            return sn;
        }
    }
}
