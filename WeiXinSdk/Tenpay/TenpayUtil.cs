using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk.Tenpay
{
    public class TenpayUtil
    {
        private TenpayRequest _request = new TenpayRequest();
        private IWxConfig _config = null;

        public IWxConfig Config
        {
            get
            {
                return _config;
            }
        }

        public TenpayUtil(IWxConfig config)
        {
            this._config = config;
        }
        public string GetPrepayId(UnifiedOrder order)
        {

            TenpayUnifiedOrderApi api = new TenpayUnifiedOrderApi(_config, order);

            var result = _request.DoPost(_config, api);

            string prepayId = null;

            result.TryGetValue("prepay_id", out prepayId);

            return prepayId;
        }

        public string OrderQuery(string out_trade_no)
        {
            TenpayOrderQueryApi api = new TenpayOrderQueryApi(_config, out_trade_no);
            var result = _request.DoPost(_config, api);


            return _request.ReturnContent;
        }

    }
}
