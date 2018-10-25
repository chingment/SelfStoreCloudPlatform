using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
    //{ "openId":"ol1Iu5VfH-UocXdmU2SDtLNO8W88","nickName":"邱庆文","gender":1,"language":"zh_CN","city":"Guangzhou","province":"Guangdong","country":"China","avatarUrl":"https://wx.qlogo.cn/mmopen/vi_32/KDXdSp0mgZPfFu81OicXewUnDaYVGNkpwwSDqbyFE6m8BRv8OevTamibicBzvSzNWDAsdDIaaU9xS9r2ZvLBBjoSA/132","watermark":{ "timestamp":1538208985,"appid":"wxb01e0e16d57bd762"} }

    public class UserInfoModelByMinProramJsCode
    {
        public string openId { get; set; }
        public string nickName { get; set; }
        public string gender { get; set; }
        public string language { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string avatarUrl { get; set; }
        public UserInfoModelByMinProramJsCode.Watermark watermark { get; set; }

        public class Watermark
        {
            public string timestamp { get; set; }

            public string appid { get; set; }
        }
    }
}
