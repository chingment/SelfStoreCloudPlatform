using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{
    [Table("UserCart")]
    public class UserCart
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string StoreId { get; set; }
        public string ProductSkuId { get; set; }
        [MaxLength(128)]
        public string ProductSkuName { get; set; }
        [MaxLength(256)]
        public string ProductSkuImgUrl { get; set; }
        public int Quantity { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? MendTime { get; set; }
        public bool Selected { get; set; }
        public Enumeration.CartStatus Status { get; set; }
        public int ChannelId { get; set; }
        public Enumeration.ChannelType ChannelType { get; set; }
    }
}
