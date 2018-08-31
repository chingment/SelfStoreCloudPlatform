using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class SnUtil
    {

        public static string Build(Entity.Enumeration.BizSnType type)
        {

            string prefix = "";
            Random ran = new Random();
            string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss") + ran.Next(1000, 9999);


            string sn = prefix + dateTime;
            return sn;
        }
    }
}
