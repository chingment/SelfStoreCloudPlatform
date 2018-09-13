using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{
    [Table("WxMsgPushLog")]
    public class WxMsgPushLog
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        [MaxLength(128)]
        public string ToUserName { get; set; }
        [MaxLength(128)]
        public string FromUserName { get; set; }
        public DateTime CreateTime { get; set; }
        [MaxLength(128)]
        public string MsgType { get; set; }
        public long MsgId { get; set; }
        [MaxLength(128)]
        public string Event { get; set; }
        public string EventKey { get; set; }
        public string ContentXml { get; set; }
    }
}
