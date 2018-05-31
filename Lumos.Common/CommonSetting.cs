using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Common
{
    public class CommonSetting
    {

        public static readonly string DefaultPassword = "888888";

        public static readonly string LoggerAccessWeb = "AccessWebLogger";
        public static readonly string LoggerLoginWeb = "LoginWebLogger";
        public static readonly string LoggerStatisticsTracker = "StatisticsTrackerLogger";


        public static readonly string CartProductsCookiesName = "cartProducts";

        public static string GetUploadPath(string path)
        {
            string rootPath = "/Upload/";
            rootPath += path;
            return rootPath;
        }
    }
}
