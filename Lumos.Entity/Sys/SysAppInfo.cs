using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("SysAppInfo")]
    public class SysAppInfo
    {
        [Key]
        public string Id { get; set; }

        public Enumeration.AppType AppType { get; set; }
        [MaxLength(128)]
        public string AppMchId { get; set; }
        [MaxLength(128)]
        public string AppMchName { get; set; }
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string AppWxPayMchId { get; set; }
        public string AppWxPayKey { get; set; }
        public string AppWxPayResultNotifyUrl { get; set; }
        public string AppWxOauth2RedirectUrl { get; set; }

        public string AppWxNotifyEventUrlToken { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }

        public string Mender { get; set; }

        public DateTime? MendTime { get; set; }
    }
}
