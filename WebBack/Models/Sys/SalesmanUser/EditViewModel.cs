using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Sys.SalesmanUser
{
    public class EditViewModel : OwnBaseViewModel
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

        public EditViewModel()
        {

        }

        public EditViewModel(int id)
        {
            var sysSalesmanUser = CurrentDb.SysSalesmanUser.Where(m => m.Id == id).FirstOrDefault();
            if (sysSalesmanUser != null)
            {
                _sysSalesmanUser = sysSalesmanUser;
            }
        }
    }
}