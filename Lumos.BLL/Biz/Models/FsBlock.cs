using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class FsBlock
    {
        public FsBlock()
        {
            this.Tag = new FsTag();
            this.Fields = new List<FsField>();
        }

        public FsTag Tag { get; set; }
        public List<FsField> Fields { get; set; }
    }
}
