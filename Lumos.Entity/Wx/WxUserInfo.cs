using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Lumos.Entity
{
    [Table("WxUserInfo")]
    public class WxUserInfo
    {
        [Key]
        public string Id { get; set; }
        public string ClientId { get; set; }
        [MaxLength(128)]
        public string OpenId { get; set; }
        [MaxLength(128)]
        public string UnionId { get; set; }
        [MaxLength(128)]
        public string AccessToken { get; set; }
        public DateTime? ExpiresIn { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? MendTime { get; set; }
    }

}
