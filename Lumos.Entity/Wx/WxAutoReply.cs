using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("WxAutoReply")]
    public class WxAutoReply
    {
        [Key]
        public string Id { get; set; }

        public Enumeration.WxAutoReplyType Type { get; set; }

        public string Keyword { get; set; }

        public string ReplyContent { get; set; }

        public bool IsDelete { get; set; }

        public string Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public string Mender { get; set; }

        public DateTime? MendTime { get; set; }
    }
}
