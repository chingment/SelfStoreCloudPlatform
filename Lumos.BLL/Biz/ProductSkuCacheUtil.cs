using Lumos.Entity;
using Lumos.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Biz
{
    public static class ProductSkuCacheUtil
    {
        private static readonly string redis_key_productsku_list = "entity:Lumos.Entity.ProductSku";

        public static void Add(ProductSku model)
        {
            RedisManager.Db.HashSetAsync(redis_key_productsku_list, model.Id, Newtonsoft.Json.JsonConvert.SerializeObject(model), StackExchange.Redis.When.Always);
        }

        public static ProductSku GetOne(string id)
        {
            ProductSku model = RedisHashUtil.Get<ProductSku>(redis_key_productsku_list, id);
            return model;
        }
    }
}
