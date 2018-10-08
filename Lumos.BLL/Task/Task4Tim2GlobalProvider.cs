using Lumos.DAL;
using Lumos.Entity;
using Lumos.Mvc;
using Lumos.Redis;
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

    public enum TimerTaskType
    {
        [Remark("未知")]
        Unknow = 0,
        [Remark("检查订单支付状态")]
        CheckOrderPay = 1
    }


    public class Task4Tim2GlobalData
    {
        public string Id { get; set; }
        public TimerTaskType Type { get; set; }
        public DateTime ExpireTime { get; set; }
        public object Data { get; set; }
    }

    public class Task4Tim2GlobalProvider : BaseProvider, ITask
    {

        private static readonly string key = "task4GlobalTimer";

        public void Enter(TimerTaskType type, DateTime expireTime, object data)
        {
            var d = new Task4Tim2GlobalData();
            d.Id = GuidUtil.New();
            d.Type = type;
            d.ExpireTime = expireTime;
            d.Data = data;
            RedisManager.Db.HashSetAsync(key, d.Id, Newtonsoft.Json.JsonConvert.SerializeObject(d), StackExchange.Redis.When.Always);
        }

        public void Exit(string id)
        {
            RedisManager.Db.HashDelete(key, id);
        }

        public static List<Task4Tim2GlobalData> GetList()
        {
            List<Task4Tim2GlobalData> list = new List<Task4Tim2GlobalData>();
            var hs = RedisManager.Db.HashGetAll(key);

            var d = (from i in hs select i).ToList();

            foreach (var item in d)
            {
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Task4Tim2GlobalData>(item.Value);
                list.Add(obj);
            }
            return list;
        }

        public CustomJsonResult Run()
        {
            CustomJsonResult result = new CustomJsonResult();

            #region 检查支付状态
            var lists = GetList();
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
                            case TimerTaskType.CheckOrderPay:

                                var chData = m.Data.ToJsonObject<Order>();
                                LogUtil.Info(string.Format("查询订单号：{0}", chData.Sn));
                                string xml = SdkFactory.Wx.Instance().OrderQuery(chData.Sn);

                                LogUtil.Info(string.Format("订单号：{0},结果文件:{1}", chData.Sn, xml));

                                bool isPaySuccessed = false;
                                BizFactory.Order.PayResultNotify(GuidUtil.Empty(), Entity.Enumeration.OrderNotifyLogNotifyFrom.OrderQuery, xml, chData.Sn, out isPaySuccessed);

                                if (isPaySuccessed)
                                {
                                    Exit(m.Id);

                                    LogUtil.Info(string.Format("订单号：{0},支付成功,删除缓存", chData.Sn));
                                }
                                break;

                        }
                    }
                    else
                    {
                        switch (m.Type)
                        {
                            case TimerTaskType.CheckOrderPay:

                                var chData = (Order)m.Data;
                                var order = CurrentDb.Order.Where(q => q.Sn == chData.Sn).FirstOrDefault();
                                if (order != null)
                                {
                                    order.Status = Enumeration.OrderStatus.Cancled;
                                    order.Mender = GuidUtil.Empty();
                                    order.MendTime = this.DateTime;
                                    order.CancelReason = "订单支付有效时间过期";
                                    CurrentDb.SaveChanges();
                                    Exit(m.Id);
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
