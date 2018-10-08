using Lumos.DAL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Task
{
    public class Task4GlobalProvider : BaseProvider, ITask
    {
        public CustomJsonResult Run()
        {
            CustomJsonResult result = new CustomJsonResult();

            #region 检查支付状态
            var lists = Task4GlobalTimerUtil.GetList();
            LogUtil.Info(string.Format("共有{0}条记录需要检查状态", lists.Count));

            LogUtil.Info(string.Format("开始执行订单查询,时间：{0}", this.DateTime));
            foreach (var m in lists)
            {
                if (m.ExpireTime != null)
                {
                    if (m.ExpireTime.AddMinutes(1) >= DateTime.Now)
                    {
                        switch (m.Type)
                        {
                            case Task4GlobalTimerType.CheckOrderPay:

                                var chData = (Order)m.Data;
                                LogUtil.Info(string.Format("查询订单号：{0}", chData.Sn));
                                string xml = SdkFactory.Wx.Instance().OrderQuery(chData.Sn);

                                LogUtil.Info(string.Format("订单号：{0},结果文件:{1}", chData.Sn, xml));

                                bool isPaySuccessed = false;
                                BizFactory.Order.PayResultNotify(GuidUtil.Empty(), Entity.Enumeration.OrderNotifyLogNotifyFrom.OrderQuery, xml, chData.Sn, out isPaySuccessed);

                                if (isPaySuccessed)
                                {
                                    Task4GlobalTimerUtil.Exit(m.Id);

                                    LogUtil.Info(string.Format("订单号：{0},支付成功,删除缓存", chData.Sn));
                                }
                                break;

                        }
                    }
                    else
                    {
                        switch (m.Type)
                        {
                            case Task4GlobalTimerType.CheckOrderPay:

                                var chData = (Order)m.Data;
                                var order = CurrentDb.Order.Where(q => q.Sn == chData.Sn).FirstOrDefault();
                                if (order != null)
                                {
                                    order.Status = Enumeration.OrderStatus.Cancled;
                                    order.Mender = GuidUtil.Empty();
                                    order.MendTime = this.DateTime;
                                    order.CancelReason = "订单支付有效时间过期";
                                    CurrentDb.SaveChanges();
                                    Task4GlobalTimerUtil.Exit(m.Id);
                                    LogUtil.Info(string.Format("订单号：{0},已经过期,删除缓存", chData.Sn));
                                }
                                break;
                        }
                    }
                }
            }


            LogUtil.Info(string.Format("结束执行订单查询,时间:{0}", this.DateTime));
            #endregion


            return result;
        }
    }
}
