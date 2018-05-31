using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Common
{
    public static class FormatUtils
    {

        public static string AccessWeb(int userId,string userName)
        {
            return string.Format("[Visit the website]-> UserID:{0},UserName:{1},IP:{2},CreateTime:{3},Path：{4}", userId, userName, CommonUtils.GetIP(), DateTime.Now.ToString("yyyy-MM-dd hh:mm:dd"), CommonUtils.GetScriptUrl);
        }

        public static string LoginInWeb(int userId, string userName,string message=null)
        {
            return string.Format("[Login website]-> UserID:{0},UserName:{1},IP:{2},CreateTime:{3},Path：{4},Tip:{5}", userId, userName, CommonUtils.GetIP(), DateTime.Now.ToString("yyyy-MM-dd hh:mm:dd"), CommonUtils.GetScriptUrl, message);
        }

        public static string LoginOffWeb(int userId, string userName)
        {
            return string.Format("[Logout website]]-> UserID:{0},UserName:{1},IP:{2},CreateTime:{3},Path：{4}", userId, userName, CommonUtils.GetIP(), DateTime.Now.ToString("yyyy-MM-dd hh:mm:dd"), CommonUtils.GetScriptUrl);
        }

        public static string Operation(int userId, string userName, string operation)
        {
            return string.Format("[Operation log]-> UserID:{0},UserName:{1},IP:{2},CreateTime:{3},Path:{4},Tip:{5}", userId, userName, CommonUtils.GetIP(), DateTime.Now.ToString("yyyy-MM-dd hh:mm:dd"), CommonUtils.GetScriptUrl, operation);
        }
    }
}
