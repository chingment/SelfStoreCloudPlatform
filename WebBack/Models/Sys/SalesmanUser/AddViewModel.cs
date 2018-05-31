using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Sys.SalesmanUser
{
    public class AddViewModel:OwnViewModel
    {
        private SysSalesmanUser _sysSalesmanUser = new SysSalesmanUser();

        public SysSalesmanUser SysSalesmanUser
        {
            get
            {
                return _sysSalesmanUser;
            }
            set
            {
                _sysSalesmanUser = value;
            }
        }
    }
}