using Lumos.Entity;
using Lumos.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public static class OrderCacheUtil
    {
        private static readonly string redis_key_order_check_pay_status = "order_check_pay_status";


        public static void EnterQueue4CheckPayStatus(string orderSn, Order order)
        {
            RedisManager.Db.HashSetAsync(redis_key_order_check_pay_status, orderSn, Newtonsoft.Json.JsonConvert.SerializeObject(order), StackExchange.Redis.When.Always);
        }

        public static void ExitQueue4CheckPayStatus(string orderSn)
        {
            RedisManager.Db.HashDelete(redis_key_order_check_pay_status, orderSn);
        }

        public static List<Order> GetCheckPayStatusQueue()
        {
            List<Order> list = new List<Order>();
            var hs = RedisManager.Db.HashGetAll(redis_key_order_check_pay_status);

            var d = (from i in hs select i).ToList();

            foreach (var item in d)
            {
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Order>(item.Value);
                list.Add(obj);
            }
            return list;
        }
    }
}
