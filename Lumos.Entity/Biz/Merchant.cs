﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("Merchant")]
    public class Merchant
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactAddress { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? MendTime { get; set; }
        public string ApiHost { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public int PayTimeout { get; set; }
        public string SimpleCode { get; set; }
        public string Currency { get; set; }
        public string CurrencySymbol { get; set; }

        public string WechatPayMchId { get; set; }
        public string WechatPayKey { get; set; }
        public string WechatPayResultNotifyUrl { get; set; }
        public string WechatPaNotifyEventUrlToken { get; set; }
        public string WechatPaOauth2RedirectUrl { get; set; }
        public string WechatPaAppId { get; set; }
        public string WechatPaAppSecret { get; set; }
        public string WechatMpAppId { get; set; }
        public string WechatMpAppSecret { get; set; }
    }
}
