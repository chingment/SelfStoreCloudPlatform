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
        private AppInfoConfig _config = null;

        public AppInfoConfig Config
        {
            get
            {
                return _config;
            }
        }

        public TenpayUtil(AppInfoConfig config)
        {
            this._config = config;
        }
        //public UnifiedOrderResult UnifiedOrder(UnifiedOrder order)
        //{
        //    var rt = new UnifiedOrderResult();
        //    TenpayUnifiedOrderApi api = new TenpayUnifiedOrderApi(_config, order);

        //    var result = _request.DoPost(_config, api);

        //    string prepayId = null;

        //    result.TryGetValue("prepay_id", out prepayId);

        //    rt.PrepayId = prepayId;
        //    return rt;
        //}

        public UnifiedOrderResult UnifiedOrder(UnifiedOrder order)
        {
            var rt = new UnifiedOrderResult();

            TenpayUnifiedOrderApi api = new TenpayUnifiedOrderApi(_config, order);

            var result = _request.DoPost(_config, api);
            string prepayId = null;
            string code_url = null;
            result.TryGetValue("prepay_id", out prepayId);
            result.TryGetValue("code_url", out code_url);
            rt.PrepayId = prepayId;
            rt.CodeUrl = code_url;
            return rt;
        }

        public string OrderQuery(string out_trade_no)
        {
            TenpayOrderQueryApi api = new TenpayOrderQueryApi(_config, out_trade_no);
            var result = _request.DoPost(_config, api);


            return _request.ReturnContent;
        }

    }
}
