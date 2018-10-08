using Lumos.Entity;
using Lumos.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{

    public enum Task4GlobalTimerType
    {
        [Remark("未知")]
        Unknow = 0,
        [Remark("检查订单支付状态")]
        CheckOrderPay = 1
    }


    public class Task4GlobalTimerData
    {
        public string Id { get; set; }
        public Task4GlobalTimerType Type { get; set; }
        public DateTime ExpireTime { get; set; }
        public object Data { get; set; }
    }


    public static class Task4GlobalTimerUtil
    {
        private static readonly string key = "task4GlobalTimer";

        public static void Enter(Task4GlobalTimerType type, DateTime expireTime, object data)
        {
            var d = new Task4GlobalTimerData();
            d.Id = GuidUtil.New();
            d.Type = type;
            d.ExpireTime = expireTime;
            d.Data = data;
            RedisManager.Db.HashSetAsync(key, d.Id, Newtonsoft.Json.JsonConvert.SerializeObject(d), StackExchange.Redis.When.Always);
        }

        public static void Exit(string id)
        {
            RedisManager.Db.HashDelete(key, id);
        }

        public static List<Task4GlobalTimerData> GetList()
        {
            List<Task4GlobalTimerData> list = new List<Task4GlobalTimerData>();
            var hs = RedisManager.Db.HashGetAll(key);

            var d = (from i in hs select i).ToList();

            foreach (var item in d)
            {
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Task4GlobalTimerData>(item.Value);
                list.Add(obj);
            }
            return list;
        }
    }
}
