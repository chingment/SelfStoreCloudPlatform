using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Lumos.Entity
{
    public class BizSn
    {
        [Key]
        public string Id { get; set; }

        public Enumeration.BizSnType Type { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Prefix { get; set; }

        public string CurrentSn { get; set; }

    }
}
