using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMerch.Models.Warehouse
{
    public class EditViewModel : OwnBaseViewModel
    {
        public Lumos.Entity.Warehouse _warehouse = new Lumos.Entity.Warehouse();

        public Lumos.Entity.Warehouse Warehouse
        {
            get
            {
                return _warehouse;
            }
            set
            {
                _warehouse = value;
            }
        }

        public void LoadData(string id)
        {
            var warehouse = CurrentDb.Warehouse.Where(m => m.Id == id).FirstOrDefault();
            if (warehouse != null)
            {
                _warehouse = warehouse;
            }
        }
    }
}