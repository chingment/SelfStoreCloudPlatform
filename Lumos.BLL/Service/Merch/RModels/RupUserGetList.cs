using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Merch
{
    public class RupUserGetList : RupBaseGetList
    {
        public string UserName { get; set; }
        public string FullName { get; set; }

        public string OrganizationId { get; set; }

        public bool IsDataAllocater { get; set; }
    }
}
