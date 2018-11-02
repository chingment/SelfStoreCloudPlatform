using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    public class MerchantReplenishStaff
    {
        [Key]
        public string Id { get; set; }

        public string MerchantId { get; set; }

        public string UserId { get; set; }

        public string WxUserId { get; set; }
    }
}
