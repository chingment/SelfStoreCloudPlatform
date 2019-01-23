using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("ProductKindSku")]
    public class ProductKindSku
    {
        [Key]
        public string Id { get; set; }

        public string ProductKindId { get; set; }
        public string ProductSkuId { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
