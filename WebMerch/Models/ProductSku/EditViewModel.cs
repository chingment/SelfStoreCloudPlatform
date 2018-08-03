using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMerch.Models.ProductSku
{
    public class EditViewModel : OwnBaseViewModel
    {

        private Lumos.Entity.ProductSku _productSku = new Lumos.Entity.ProductSku();

        private List<Lumos.Entity.ImgSet> _dispalyImgs = new List<Lumos.Entity.ImgSet>();

        public Lumos.Entity.ProductSku ProductSku
        {
            get
            {
                return _productSku;
            }
            set
            {
                _productSku = value;
            }
        }

        public List<Lumos.Entity.ImgSet> DispalyImgs
        {
            get
            {
                return _dispalyImgs;
            }
            set
            {
                _dispalyImgs = value;
            }
        }

        public void LoadData(int id)
        {


        
        }

        public EditViewModel()
        {

        }
    }
}