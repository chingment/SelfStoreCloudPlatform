using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Merch
{
    public class RetOrderGetDetails
    {
        public RetOrderGetDetails()
        {
            this.Blocks = new List<Block>();
        }

        public string OrderId { get; set; }
        public string Sn { get; set; }
        public string StoreName { get; set; }
        public string ClientName { get; set; }
        public string ClientId { get; set; }
        public string SourceName { get; set; }
        public string ChargeAmount { get; set; }
        public string DiscountAmount { get; set; }
        public string OriginalAmount { get; set; }
        public string SubmitTime { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public List<Block> Blocks { get; set; }

        public class Block
        {
            public Block()
            {
                this.Skus = new List<BlockSku>();
            }
            public string Name { get; set; }

            public List<BlockSku> Skus { get; set; }

        }

        public class BlockSku
        {
            public BlockSku()
            {
                this.BlockSubSkus = new List<BlockSubSku>();
            }
            public string ProductSkuName { get; set; }
            public int Quantity { get; set; }
            public string ChargeAmount { get; set; }
            public string DiscountAmount { get; set; }
            public string OriginalAmount { get; set; }
            public string StatusName { get; set; }
            public List<BlockSubSku> BlockSubSkus { get; set; }
            public class BlockSubSku
            {
                public int Quantity { get; set; }
                public string StatusName { get; set; }
            }
        }
    }
}
