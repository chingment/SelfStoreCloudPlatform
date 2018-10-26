using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Lumos.Entity
{
    [Table("ClientCart")]
    public class ClientCart
    {
        [Key]
        public string Id { get; set; }

        public string MerchantId { get; set; }
        public string ClientId { get; set; }
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
        public Enumeration.ReceptionMode ReceptionMode { get; set; }
    }
}
