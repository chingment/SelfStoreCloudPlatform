using Lumos.DAL;
using System;
using System.Transactions;

namespace Lumos.BLL.Biz
{
    public enum RedisMqType
    {
        [Remark("未知")]
        Unknow = 0,
        [Remark("优惠卷核销")]
        CouponConsume = 1
    }

    public class RedisMq4GlobalModel
    {
        public RedisMqType Type { get; set; }

        public object Pms { get; set; }

        private static readonly object lock_Handle = new object();
        public void Handle()
        {
            lock (lock_Handle)
            {
                if (this.Pms != null)
                {
                    try
                    {
                        using (LumosDbContext CurrentDb = new LumosDbContext())
                        {
                            using (TransactionScope ts = new TransactionScope())
                            {
                                switch (this.Type)
                                {
                                    case RedisMqType.CouponConsume:

                                        
                                        break;

                                }

                                CurrentDb.SaveChanges();
                                ts.Complete();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error("消息队列，处理信息-佣金计算发生异常", ex);
                    }
                }
            }
        }
    }
}
