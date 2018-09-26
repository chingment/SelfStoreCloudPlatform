using Lumos;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMerch.Models.ProductSku
{
    public class AddViewModel : OwnBaseViewModel
    {
        private List<Lumos.Entity.ProductKind> _kind = new List<Lumos.Entity.ProductKind>();
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

        public List<Lumos.Entity.ProductKind> Kind
        {
            get
            {
                return _kind;
            }
            set
            {
                _kind = value;
            }
        }

        public void LoadData()
        {



        }

        public AddViewModel()
        {
            string id = GuidUtil.Empty();
            var kind = CurrentDb.ProductKind.Where(m => m.Id != id && m.MerchantId == this.Operater && m.IsDelete == false).ToList();

            if (kind != null)
            {
                _kind = kind;
            }
        }

    }
}