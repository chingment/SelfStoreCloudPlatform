using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class SnUtil
    {

        public static string Build(int id)
        {
            string prefix = "";

            string dateTime = DateTime.Now.ToString("yyMMddHHmm");

            string sId = id.ToString().PadLeft(10, '0');

            string sn = prefix + dateTime + sId;
            return sn;
        }
    }
}
