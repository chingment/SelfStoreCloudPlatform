using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk.MsgPush
{
    public class UserConsumeCardMsg : BaseEventMsg
    {
        public string CardId { get; set; }
        public string UserCardCode { get; set; }
        public string ConsumeSource { get; set; }
        public string LocationName { get; set; }
        public string StaffOpenId { get; set; }
        public string VerifyCode { get; set; }
        public string RemarkAmount { get; set; }
        public string OuterStr { get; set; }
    }
}
