using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    public class OrderCarInsureKind
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int CarKindId { get; set; }

        public int CarKindValue { get; set; }

        public int CarKindDetails { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
