using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMerch.Models.ProductKind
{
    public class DetailsViewModel : OwnBaseViewModel
    {
        private Lumos.Entity.ProductKind _productKind = new Lumos.Entity.ProductKind();

        public Lumos.Entity.ProductKind ProductKind
        {
            get
            {
                return _productKind;
            }
            set
            {
                _productKind = value;
            }
        }

        public DetailsViewModel()
        {

        }
        public DetailsViewModel(string id)
        {
            var productKind = CurrentDb.ProductKind.Where(m => m.Id == id).FirstOrDefault();
            if (productKind != null)
            {
                _productKind = productKind;
            }


        }



    }
}