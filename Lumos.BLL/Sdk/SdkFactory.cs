using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public static class SdkFactory
    {
        private static Lazy<WxSdkProvider> _wx = new Lazy<WxSdkProvider>(() => new WxSdkProvider());

        public static WxSdkProvider Wx
        {
            get
            {
                return _wx.Value;
            }
        }


    }
}
