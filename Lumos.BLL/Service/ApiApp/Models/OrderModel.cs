using Lumos.BLL.Biz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.ApiApp
{
    public class OrderModel
    {
        public OrderModel()
        {
            this.Tag = new TagModel();
            this.Tip = new FontModel();
            this.Blocks = new List<BlockModel>();
            this.Buttons = new List<ButtonModel>();
        }
        public string Id { get; set; }

        public string Sn { get; set; }

        public TagModel Tag { get; set; }

        public List<BlockModel> Blocks { get; set; }

        public string ChargeAmount { get; set; }

        public FontModel Tip { get; set; }

        public class BlockModel
        {
            public BlockModel()
            {
                this.Tag = new TagModel();
                this.Fields = new List<FieldModel>();
            }

            public TagModel Tag { get; set; }
            public List<FieldModel> Fields { get; set; }
        }

        public List<ButtonModel> Buttons { get; set; }

        public class FieldModel
        {
            public string Type { get; set; }

            public object Value { get; set; }
        }

        public class SkuModel
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string ImgUrl { get; set; }
            public int Quantity { get; set; }
            public decimal ChargeAmount { get; set; }
        }

        public class ButtonModel
        {
            public FontModel Name { get; set; }
            public string Url { get; set; }
            public string Operate { get; set; }
        }

        public class TagModel
        {
            public TagModel()
            {
                this.Name = new FontModel();
                this.Desc = new FontModel();
            }

            public FontModel Name { get; set; }
            public FontModel Desc { get; set; }
        }

        public class FontModel
        {
            public string Content { get; set; }
            public string Color { get; set; }
            public string BackgroundColor { get; set; }

        }
    }
}
