using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{

    public class ActionInfo
    {
        public ActionInfo()
        {
            this.scene = new ActionInfoScene();
        }

        public ActionInfoScene scene { get; set; }
    }

    public class ActionInfoScene
    {
        public string scene_str { get; set; }
    }

    public class WxApiQrCodeCreatePostData
    {
        public WxApiQrCodeCreatePostData()
        {
            this.action_info = new ActionInfo();
        }

        public int expire_seconds { get; set; }

        public string action_name { get; set; }

        public ActionInfo action_info { get; set; }

    }

    public class WxApiQrCodeCreatePostData2
    {
        public WxApiQrCodeCreatePostData2()
        {
            this.action_info = new ActionInfo();
        }

        public string action_name { get; set; }

        public ActionInfo action_info { get; set; }

    }
}
