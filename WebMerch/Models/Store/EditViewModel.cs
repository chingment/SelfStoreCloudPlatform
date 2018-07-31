using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMerch.Models.Store
{
    public class EditViewModel : OwnBaseViewModel
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

        public void LoadData(string id)
        {
            var store = CurrentDb.Store.Where(m => m.Id == id).FirstOrDefault();
            if (store != null)
            {
                _store = store;
            }
        }
    }
}