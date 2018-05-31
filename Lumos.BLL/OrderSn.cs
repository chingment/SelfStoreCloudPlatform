using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public enum SnType
    {
        Unknow = 0

    }

    public class SnModel
    {
        public string Sn { get; set; }
    }

    public class Sn
    {

        public static SnModel Build(SnType type, int id)
        {
            SnModel model = new SnModel();
            string prefix = "";

            string dateTime = DateTime.Now.ToString("yyMMddHHmm");

            string sId = id.ToString().PadLeft(10, '0');

            string sn = prefix + dateTime + sId;

            model.Sn = sn;
            return model;
        }
    }
}
