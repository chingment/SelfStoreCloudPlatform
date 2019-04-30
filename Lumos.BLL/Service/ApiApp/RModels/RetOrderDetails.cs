using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.ApiApp
{
    public class RetOrderDetails
    {
        public RetOrderDetails()
        {
            this.Blocks = new List<FsBlock>();
            this.FieldBlocks = new List<FsBlockByField>();
        }
        public Enumeration.OrderStatus Status { get; set; }
        public FsTag Tag { get; set; }
        public List<FsBlock> Blocks { get; set; }
        public List<FsBlockByField> FieldBlocks { get; set; }
    }
}
