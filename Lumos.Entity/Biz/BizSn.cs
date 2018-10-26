using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("BizSn")]
    public class BizSn
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int IncrNum { get; set; }

        [Key]
        public string IncrDate { get; set; }

    }
}
