using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMerch.Models.Store
{
    public class AddViewModel : OwnBaseViewModel
    {
        public Lumos.Entity.Store _store = new Lumos.Entity.Store();

        public Lumos.Entity.Store Store
        {
            get
            {
                return _store;
            }
            set
            {
                _store = value;
            }
        }

        public AddViewModel()
        {

        }
    }
}