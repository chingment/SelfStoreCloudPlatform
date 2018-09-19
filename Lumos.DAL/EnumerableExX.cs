using Lumos.Entity;
using Lumos.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace System
{

    public static class EnumerableExX
    {

        public static List<TSource> ToListByCache<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {

            string tpye_name = source.GetType().ToString();

            int start = tpye_name.IndexOf("[") + 1;

            string entity_name = tpye_name.Substring(start, tpye_name.Length - start - 1);


            List<TSource> list = RedisHashUtil.GetAll<TSource>(string.Format("entity:{0}", entity_name));

            list = list.Where(predicate).ToList();
            //IQueryable<TSource> c = list.Where(predicate);
            ////var c = source;
            ////string name = source.GetType().ToString();
            ////return default(TSource);

            //List<TSource> a = new List<TSource>();
            return list;
        }


        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, string id)
        {

            string tpye_name = source.GetType().ToString();

            int start = tpye_name.IndexOf("[") + 1;

            string entity_name = tpye_name.Substring(start, tpye_name.Length - start - 1);


            TSource model = RedisHashUtil.Get<TSource>(string.Format("entity:{0}", entity_name), id);


            return model;
        }

    }
}
