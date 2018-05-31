using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
    public interface IWxApiPostRequest<T> where T : WxApiBaseResult
    {
        string ApiName { get; }

        IDictionary<string, string> GetUrlParameters();

        WxPostDataType PostDataTpye { get; set; }

        object PostData { get; set; }

    }
}
