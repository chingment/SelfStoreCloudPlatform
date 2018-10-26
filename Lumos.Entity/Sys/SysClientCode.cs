using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
     [Table("SysClientCode")]
    public class SysClientCode
    {
         [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         public int Id { get; set; }

        [MaxLength(128)]
        public string Code { get; set; }
    }
}
