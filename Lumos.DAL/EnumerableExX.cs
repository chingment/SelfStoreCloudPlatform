using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{

    public static class EnumerableExX
    {
        public static List<TSource> ToListByCache<TSource>(this IEnumerable<TSource> source)
        {
            string name = source.GetType().ToString();

            //var c = source;
            //string name = source.GetType().ToString();
            //return default(TSource);

            List<TSource> a = new List<TSource>();
            return a;
        }
    }
}
