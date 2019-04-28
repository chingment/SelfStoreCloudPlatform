using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class FsBlockByField
    {
        public FsBlockByField()
        {
            this.Tag = new FsTag();
            this.Data = new List<FsField>();
        }

        public FsTag Tag { get; set; }
        public List<FsField> Data { get; set; }
    }
}
