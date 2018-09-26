using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
        public string Nickname { get; set; }
        [MaxLength(128)]
        public string Sex { get; set; }
        [MaxLength(128)]
        public string Province { get; set; }
        [MaxLength(128)]
        public string City { get; set; }
        [MaxLength(128)]
        public string Country { get; set; }
        [MaxLength(256)]
        public string HeadImgUrl { get; set; }
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
