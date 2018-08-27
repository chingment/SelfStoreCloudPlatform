using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMerch.Models.Supplier
{
    public class AddViewModel : OwnBaseViewModel
    {
        public Lumos.Entity.Company _company = new Lumos.Entity.Company();

        public Lumos.Entity.Company Company
        {
            get
            {
                return _company;
            }
            set
            {
                _company = value;
            }
        }

        public AddViewModel()
        {

        }
    }
}