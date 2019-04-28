using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.ApiApp
{
    public class RetOrderDetails
    {
        public string Id { get; set; }
        public string Sn { get; set; }
        public string Status { get; set; }
        public string StatusName { get; set; }
        public List<FsBlock> Blocks { get; set; }
        public List<FsBlockByField> FieldBlocks { get; set; }
    }
}
