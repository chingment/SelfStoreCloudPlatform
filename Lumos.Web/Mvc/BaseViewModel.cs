using Lumos.Entity;
using System;


namespace Lumos.Web.Mvc
{


    public abstract class BaseViewModel
    {
        public virtual string CurrentUserId { get; set; }

        public bool IsHasOperater { get; set; }

        public string OperaterName { get; set; }

        public Enumeration.OperateType Operate { get; set; }
    }
}
