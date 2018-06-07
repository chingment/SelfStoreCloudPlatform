﻿using Lumos.BLL;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMerch.Models.ProductKind
{
    public class AddViewModel : BaseViewModel
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


        public AddViewModel()
        {

        }
    }
}