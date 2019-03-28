using Lumos.BLL.Biz;
using Lumos.BLL.Service.Admin;
using Lumos.BLL.Service.Merch;
using Lumos.Entity;
using Lumos.Redis;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;

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

    public class Task4Tim2GlobalProvider : BaseProvider, IJob
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

        public void Execute(IJobExecutionContext context)
        {
            #region 检查支付状态
            try
            {
                var lists = GetList();
                LogUtil.Info(string.Format("共有{0}条记录需要检查状态", lists.Count));
                if (lists.Count > 0)
                {
                    LogUtil.Info(string.Format("开始执行订单查询,时间：{0}", this.DateTime));
                    foreach (var m in lists)
                    {
                        switch (m.Type)
                        {
                            case TimerTaskType.CheckOrderPay:
                                #region 检查支付状态
                                if (m.ExpireTime.AddMinutes(1) >= DateTime.Now)//未过期
                                {
                                    var chData = m.Data.ToJsonObject<Order>();

                                    LogUtil.Info(string.Format("查询订单号：{0}", chData.Sn));

                                    bool isPaySuccessed = false;

                                    if (!string.IsNullOrEmpty(chData.AppId))
                                    {
                                        var appInfo = BizFactory.Merchant.GetWxAppInfoConfig(chData.MerchantId);

                                        string xml = SdkFactory.Wx.OrderQuery(appInfo, chData.Sn);
                                        LogUtil.Info(string.Format("订单号：{0},结果文件:{1}", chData.Sn, xml));
                                        BizFactory.Order.PayResultNotify(GuidUtil.Empty(), Entity.Enumeration.OrderNotifyLogNotifyFrom.OrderQuery, xml, chData.Sn, out isPaySuccessed);
                                    }

                                    if (isPaySuccessed)
                                    {
                                        Task4Factory.Global.Exit(m.Id);
                                        LogUtil.Info(string.Format("订单号：{0},支付成功,删除缓存", chData.Sn));
                                    }
                                }
                                else
                                {
                                    var chData = m.Data.ToJsonObject<Order>();
                                    var rt = BizFactory.Order.Cancle(GuidUtil.Empty(), chData.Id, "订单支付有效时间过期");
                                    if (rt.Result == ResultType.Success)
                                    {
                                        Task4Factory.Global.Exit(m.Id);
                                    }
                                }
                                #endregion 
                                break;
                        }
                    }

                    LogUtil.Info(string.Format("结束执行订单查询,时间:{0}", this.DateTime));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error("全局定时任务发生异常", ex);
            }
            #endregion
        }
    }
}
