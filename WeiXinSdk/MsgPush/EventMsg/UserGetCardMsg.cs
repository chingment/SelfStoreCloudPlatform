using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk.MsgPush
{
    public class UserGetCardMsg: BaseEventMsg
    {
        public string CardId { get; set; }
        public int IsGiveByFriend { get; set; }
        public string UserCardCode { get; set; }
        public string FriendUserName { get; set; }
        public string OuterId { get; set; }
        public string OldUserCardCode { get; set; }
        public string OuterStr { get; set; }
        public int IsRestoreMemberCard { get; set; }
        public string IsRecommendByFriend { get; set; }
        public string UnionId { get; set; }
    }
}
