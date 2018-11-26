using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("ProductSubject")]
    public class ProductSubject
    {
        [Key]
        public string Id { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength(128)]
        public string MerchantId { get; set; }

        public string PId { get; set; }
        public string IconImg { get; set; }
        public string MainImg { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }
        public bool IsDelete { get; set; }
        public Lumos.Entity.Enumeration.ProductSubjectStatus Status { get; set; }
        public int Priority { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? MendTime { get; set; }

        public int Dept { get; set; }
    }
}
