using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class DecimalExtensions
    {
        public static string ToPrice(this decimal d)
        {
            return d.ToString("#,##0.00");
        }

        public static string ToF2Price(this decimal d)
        {
            return string.Format("{0:0.##}", d);
        }

        public static string ToPrice(this decimal? d)
        {

            if (d == null)
            {
                return "0.00";
            }


            return d.Value.ToString("#,##0.00");
        }

        public static string ToF2Price(this decimal? d)
        {

            if (d == null)
            {
                return "0.00";
            }


            return d.Value.ToString("f2");
        }
    }
}
