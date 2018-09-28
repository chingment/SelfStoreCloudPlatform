using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMerch.Models.ProductSubject
{
    public class DetailsViewModel : OwnBaseViewModel
    {
        private Lumos.Entity.ProductSubject _productSubject = new Lumos.Entity.ProductSubject();

        public Lumos.Entity.ProductSubject ProductSubject
        {
            get
            {
                return _productSubject;
            }
            set
            {
                _productSubject = value;
            }
        }

        public DetailsViewModel()
        {

        }
        public DetailsViewModel(string id)
        {
            var productSubject = CurrentDb.ProductSubject.Where(m => m.Id == id).FirstOrDefault();
            if (productSubject != null)
            {
                _productSubject = productSubject;
            }


        }



    }
}