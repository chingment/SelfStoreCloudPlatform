using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Machine
{
    public class AddViewModel:OwnBaseViewModel
    {
        private Lumos.Entity.Machine _machine=new Lumos.Entity.Machine();

        public Lumos.Entity.Machine Machine
        {
            get
            {
                return _machine;
            }
            set
            {
                _machine = value;
            }
        }
    }
}