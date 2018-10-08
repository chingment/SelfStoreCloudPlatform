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
        private static readonly string redis_key_order_list_check_pay = "order_list_check_pay";

        public static void EnterQueue4CheckPay(string orderSn, Order order)
        {
            RedisManager.Db.HashSetAsync(redis_key_order_list_check_pay, orderSn, Newtonsoft.Json.JsonConvert.SerializeObject(order), StackExchange.Redis.When.Always);
        }

        public static void ExitQueue4CheckPay(string orderSn)
        {
            RedisManager.Db.HashDelete(redis_key_order_list_check_pay, orderSn);
        }

        public static List<Order> GetLisy4CheckPay()
        {
            List<Order> list = new List<Order>();
            var hs = RedisManager.Db.HashGetAll(redis_key_order_list_check_pay);

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
