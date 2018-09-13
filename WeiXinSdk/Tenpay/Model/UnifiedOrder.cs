using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
    /// <summary>
    /// 微信统一接口请求实体对象
    /// </summary>
    [Serializable]
    public class UnifiedOrder
    {
        public string body = "";
        public string attach = "";
        public string out_trade_no = "";
        public int total_fee = 0;
        public string spbill_create_ip = "";
        public string time_start = "";
        public string time_expire = "";
        public string goods_tag = "";
        public string openid = "";
        public string trade_type = "";
    }

}
