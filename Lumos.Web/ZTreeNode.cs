using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Web
{
    public class ZTreeNode
    {
        public string Id { get; set; }

        public string PId { get; set; }

        public string Name { get; set; }

        public bool Checked { get; set; }

        public string IconSkin { get; set; }

        public bool Open { get; set; }

    }
}
