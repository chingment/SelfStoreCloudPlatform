using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class FsTag
    {
        public FsTag()
        {
            this.Name = new FsText();
            this.Desc = new FsText();
        }

        public FsText Name { get; set; }
        public FsText Desc { get; set; }
    }
}
