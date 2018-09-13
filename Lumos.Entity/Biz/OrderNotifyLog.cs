using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{
    [Table("OrderNotifyLog")]
    public class OrderNotifyLog
    {
        [Key]
        public string Id { get; set; }

        public string UserId { get; set; }

        public string OrderId { get; set; }

        public string OrderSn { get; set; }

        public Enumeration.OrderNotifyLogNotifyType NotifyType { get; set; }

        public Enumeration.OrderNotifyLogNotifyFrom NotifyFrom { get; set; }

        public string NotifyContent { get; set; }

        public string Creator { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
