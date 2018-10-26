using Lumos.Entity;
using Lumos.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMerch.Models.ProductSubject
{
    public class EditViewModel: BaseViewModel
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


        public EditViewModel()
        {

        }
    }
}