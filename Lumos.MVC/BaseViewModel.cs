using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Mvc
{


    public abstract class BaseViewModel
    {
        public virtual string Operater { get; set; }

        public bool IsHasOperater { get; set; }

        public string OperaterName { get; set; }

        public Enumeration.OperateType Operate { get; set; }
    }
}
