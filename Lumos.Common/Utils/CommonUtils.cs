using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Lumos.Common
{

    /// <summary>
    /// 密码强度
    /// </summary>
    public enum PasswordStrength
    {
        Invalid = 0, //无效密码
        Weak = 1,    //低强度密码
        Normal = 2,  //中强度密码
        Strong = 3   //高强度密码
    };


    public static class CommonUtils
    {
        #region "数组转字符串"
        public static string ArraryToString(long[] l)
        {

            if (l == null)
                return "";
            String s = "";
            for (int i = 0; i < l.Length; i++)
            {
                s += l[i] + ",";
            }
            if (s != "")
            {
                s = s.Substring(0, s.Length - 1);
            }
            return s;
        }
        #endregion 

        #region "返回网页路径名称"
        /// <summary>
        /// 返回网页路径名称
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string SubUrlNameHandler(string url)
        {
            string str = url;
            if (str.LastIndexOf("/") > 0)
            {
                str = str.Substring(str.LastIndexOf("/") + 1);
            }
            return str;
        }
        #endregion

        #region"判断是否是Int类型"
        /// <summary>
        /// 判断是否为Int类型
        /// </summary>
        /// <param name="strDate">字符串</param>
        /// <returns></returns>
        public static bool IsInt(string strInt)
        {
            int dtInt;
            bool bValid = true;
            try
            {
                dtInt = int.Parse(strInt);
            }
            catch (FormatException)
            {
                // 如果解析方法失败则表示不是Int
                bValid = false;
            }
            return bValid;
        }
        #endregion

        #region"判断是否是Double类型"
        /// <summary>
        /// 判断是否为Int类型
        /// </summary>
        /// <param name="strDate">字符串</param>
        /// <returns></returns>
        public static bool IsDouble(string strInt)
        {
            double dtInt;
            bool bValid = true;
            try
            {
                dtInt = double.Parse(strInt);
            }
            catch (FormatException)
            {
                // 如果解析方法失败则表示不是Int
                bValid = false;
            }
            return bValid;
        }
        #endregion

        #region"判断是否是Decimal类型"
        /// <summary>
        /// 判断是否为Int类型
        /// </summary>
        /// <param name="strDate">字符串</param>
        /// <returns></returns>
        public static bool IsDecimal(string strInt)
        {
            decimal dtInt;
            bool bValid = true;
            try
            {
                dtInt = decimal.Parse(strInt);
            }
            catch (FormatException)
            {
                // 如果解析方法失败则表示不是Int
                bValid = false;
            }
            return bValid;
        }


        public static bool IsDecimal(string s, int precision, int scale)
        {
            if ((precision == 0) && (scale == 0))
            {
                return false;
            }
            string pattern = @"(^\d{1," + precision + "}";
            if (scale > 0)
            {
                pattern += @"\.\d{0," + scale + "}$)|" + pattern;
            }
            pattern += "$)";
            return Regex.IsMatch(s, pattern);
        }

        #endregion

        #region 判断是否为日期数
        /// <summary>
        /// 判断是否为日期数
        /// </summary>
        /// <param name="strDate">日期字符串</param>
        /// <returns></returns>
        /// 
        public static bool IsDateTime(string strDate)
        {
            if (strDate == null)
            {
                return false;
            }
            else
            {
                DateTime dtDate;
                bool bValid = true;
                try
                {
                    dtDate = DateTime.Parse(strDate);
                }
                catch (FormatException)
                {
                    // 如果解析方法失败则表示不是日期性数据
                    bValid = false;
                }
                return bValid;
            }
        }


        public static bool IsDateTime(object strDate)
        {
            if (strDate == null)
            {
                return false;
            }
            else
            {
                return CommonUtils.IsDateTime(strDate.ToString());
            }
        }
        #endregion

        #region 计算两个日期之间的天数差（t2-t1）
        public static int DiffDays(DateTime t1, DateTime t2)
        {
            t1 = DateTime.Parse(t1.ToShortDateString() + " 00:00:00.000");
            t2 = DateTime.Parse(t2.ToShortDateString() + " 00:00:00.000");

            return (int)(t2 - t1).TotalDays;
        }
        #endregion

        #region 转换时间的开始和结束时间

        public static DateTime? ConverToShortDateTime(string strDate)
        {
            DateTime? d = null;
            try
            {
                d = DateTime.Parse(strDate);
            }
            catch
            {

            }
            return d;

        }

        public static DateTime? ConverToStartTime(string strDate)
        {
            DateTime? d = null;
            try
            {
                if (strDate.Trim() != "")
                {
                    d = DateTime.Parse(strDate + " 00:00:00.000");
                }
            }
            catch
            {

            }
            return d;

        }

        public static DateTime? ConverToEndTime(string strDate)
        {
            DateTime? d = null;
            try
            {
                if (strDate.Trim() != "")
                {
                    d = DateTime.Parse(strDate + " 23:59:59");
                }
            }
            catch
            {

            }
            return d;

        }




        public static string ConverToShortDateStart(DateTime strDate)
        {
            return strDate.ToShortDateString() + " 00:00:00.000";
        }

        public static string ConverToShortDateStart(string strDate)
        {
            if (CommonUtils.IsDateTime(strDate))
            {
                return DateTime.Parse(strDate).ToShortDateString() + " 00:00:00.000";
            }
            else
            {
                return "";
            }
        }

        public static string ConverToShortDateEnd(DateTime strDate)
        {
            return strDate.ToShortDateString() + " 23:59:59";
        }

        public static string ConverToShortDateEnd(string strDate)
        {
            if (CommonUtils.IsDateTime(strDate))
            {
                return DateTime.Parse(strDate).ToShortDateString() + " 23:59:59";
            }
            else
            {
                return "";
            }
        }

        #endregion

        #region "获取Ip"
        /// <summary>
        /// 获取Ip
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        public static string GetIP()
        {
            string userIP = "";
            try
            {
                HttpContext rq = HttpContext.Current;
                HttpRequest Request = HttpContext.Current.Request;
                // 如果使用代理，获取真实IP
                if (rq.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "")
                {
                    userIP = rq.Request.ServerVariables["REMOTE_ADDR"];
                }
                else
                {
                    userIP = rq.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }
                if (userIP == null || userIP == "")
                {
                    userIP = rq.Request.UserHostAddress;
                }
            }
            catch
            {
                userIP = "error ip";
            }
            return userIP;

        }
        #endregion

        #region 生成0-9随机数
        /// <summary>
        /// 生成0-9随机数
        /// </summary>
        /// <param name="VcodeNum">生成长度</param>
        /// <returns></returns>
        public static string RndNum(int VcodeNum)
        {
            StringBuilder sb = new StringBuilder(VcodeNum);
            Random rand = new Random();
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                int t = rand.Next(9);
                sb.AppendFormat("{0}", t);
            }
            return sb.ToString();

        }
        #endregion

        #region 截取字符串
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="str_value"></param>
        /// <param name="str_len"></param>
        /// <returns></returns>
        public static string SubString(string str_value, int str_len, string lue)
        {
            int p_num = 0;
            int i;
            string New_Str_value = "";

            if (str_value == "")
            {
                New_Str_value = "";
            }
            else
            {
                int Len_Num = str_value.Length;
                for (i = 0; i <= Len_Num - 1; i++)
                {
                    if (i > Len_Num) break;
                    char c = Convert.ToChar(str_value.Substring(i, 1));
                    if (((int)c > 255) || ((int)c < 0))
                        p_num = p_num + 2;
                    else
                        p_num = p_num + 1;



                    if (p_num >= str_len)
                    {

                        New_Str_value = str_value.Substring(0, i + 1);
                        break;
                    }
                    else
                    {
                        New_Str_value = str_value;
                    }

                }

            }
            return New_Str_value + lue;
        }


        public static string InterceptString(string str, int maxlegnth)
        {
            string newstr = str.Trim();
            if (str.Trim() == "")
            {
                int len = str.Length;
                if (len > maxlegnth)
                {
                    string thestr = newstr.Substring(0, maxlegnth);
                    return thestr;
                }
                else
                {
                    return newstr;
                }
            }
            else
            {
                return newstr;
            }
        }


        #endregion

        #region "获得sessionid"
        /// <summary>
        /// 获得sessionid
        /// </summary>
        public static string GetSessionID
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Session.SessionID;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region "获取页面url"
        /// <summary>
        /// 获取当前访问页面地址
        /// </summary>
        public static string GetScriptName
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString();
            }
        }

        /// <summary>
        /// 检测当前url是否包含指定的字符
        /// </summary>
        /// <param name="sChar">要检测的字符</param>
        /// <returns></returns>
        public static bool CheckScriptNameChar(string sChar)
        {
            bool rBool = false;
            if (GetScriptName.ToLower().LastIndexOf(sChar) >= 0)
                rBool = true;
            return rBool;
        }

        /// <summary>
        /// 获取当前页面的扩展名
        /// </summary>
        public static string GetScriptNameExt
        {
            get
            {
                return GetScriptName.Substring(GetScriptName.LastIndexOf(".") + 1);
            }
        }

        /// <summary>
        /// 获取当前访问页面地址参数
        /// </summary>
        public static string GetScriptNameQueryString
        {
            get
            {
                if (HttpContext.Current.Request.ServerVariables["QUERY_STRING"] != null)
                {
                    return HttpContext.Current.Request.ServerVariables["QUERY_STRING"].ToString();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获得页面文件名和参数名
        /// </summary>
        public static string GetScriptNameUrl
        {
            get
            {
                string Script_Name = CommonUtils.GetScriptName;
                Script_Name = Script_Name.Substring(Script_Name.LastIndexOf("/") + 1);
                if (GetScriptNameQueryString != "")
                {
                    Script_Name += "?" + GetScriptNameQueryString;
                }
                return Script_Name;
            }
        }

        /// <summary>
        /// 获得当前页面的文件名
        /// </summary>
        public static string GetScriptFileName
        {
            get
            {
                string Script_Name = CommonUtils.GetScriptName;
                Script_Name = Script_Name.Substring(Script_Name.LastIndexOf("/") + 1);
                return Script_Name;
            }
        }

        /// <summary>
        /// 获取当前访问页面Url
        /// </summary>
        public static string GetScriptUrl
        {
            get
            {
                return CommonUtils.GetScriptNameQueryString == "" ? CommonUtils.GetScriptName : string.Format("{0}?{1}", CommonUtils.GetScriptName, CommonUtils.GetScriptNameQueryString);
            }
        }

        /// <summary>
        /// 返回当前页面目录的url
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <returns></returns>
        public static string GetHomeBaseUrl(string FileName)
        {
            string Script_Name = CommonUtils.GetScriptName;
            return string.Format("{0}/{1}", Script_Name.Remove(Script_Name.LastIndexOf("/")), FileName);
        }

        /// <summary>
        /// 返回当前网站网址
        /// </summary>
        /// <returns></returns>
        public static string GetHomeUrl()
        {
            return HttpContext.Current.Request.Url.Authority;
        }

        /// <summary>
        /// 获取当前访问文件物理目录
        /// </summary>
        /// <returns>路径</returns>
        public static string GetScriptPath
        {
            get
            {
                string Paths = HttpContext.Current.Request.ServerVariables["PATH_TRANSLATED"].ToString();
                return Paths.Remove(Paths.LastIndexOf("\\"));
            }
        }
        #endregion

        #region "格式化日期24小时制为字符串如:2008/12/12 21:22:33"
        /// <summary>
        /// 格式化日期24小时制为字符串如:2008/12/12 21:22:33
        /// </summary>
        /// <param name="d">日期</param>
        /// <returns>字符</returns>
        public static string FormatDateToString(DateTime d)
        {
            return d.ToString("yyyy/MM/dd HH:mm:ss");
        }
        #endregion

        #region "格式化日期显示为字符"
        /// <summary>
        /// 格式化日期显示为字符
        /// </summary>
        /// <param name="d">日期</param>
        /// <returns></returns>
        public static string FormatDateToDispString(DateTime d)
        {
            return d.ToString("yyyy/MM/dd HH:mm:ss");
        }
        #endregion

        #region "将错误列表转为Html"
        public static string ListErrorToHtml(List<string> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var s in list)
            {
                sb.Append(s.ToString() + "<BR>");
            }
            return sb.ToString();
        }
        #endregion

        #region "SQL 组合"
        public static string WhereAdd(string sql, object param, bool isselectnull)
        {
            string _where = "";
            if (param != null)
            {
                if (isselectnull)
                {

                    _where += string.Format(sql.ToString(), param.ToString().Trim());
                }
                else
                {
                    if (param.ToString().Trim() != "")
                    {
                        _where += string.Format(sql.ToString(), param.ToString().Trim());
                    }

                }
            }
            return _where;

        }
        public static string WhereAdd(string sql, object param)
        {
            return WhereAdd(sql, param, false);
        }
        #endregion

        #region "组合新的CombGuid"
        public static Guid NewCombId()
        {
            byte[] guidArray = System.Guid.NewGuid().ToByteArray();

            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;
            // Get the days and milliseconds which will be used to build the byte string 
            TimeSpan days = new TimeSpan(now.Date.Ticks - baseDate.Ticks);
            TimeSpan msecs = new TimeSpan(now.Ticks - (now.Date.Ticks));

            // Convert to a byte array 
            // SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333 
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            for (int i = 15; i >= 6; i--)
            {
                guidArray[i] = guidArray[i - 6];
            }

            Array.Copy(daysArray, daysArray.Length - 2, guidArray, 0, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, 2, 4);

            return new System.Guid(guidArray);
        }
        #endregion

        #region "判断密码强度"
        /// <summary>
        /// 计算密码强度
        /// </summary>
        /// <param name="password">密码字符串</param>
        /// <returns></returns>
        private static PasswordStrength GetPasswordStrength(string password)
        {
            //空字符串强度值为0
            if (password == "") return PasswordStrength.Invalid;

            //字符统计
            int iNum = 0, iLtt = 0, iSym = 0;
            foreach (char c in password)
            {
                if (c >= '0' && c <= '9') iNum++;
                else if (c >= 'a' && c <= 'z') iLtt++;
                else if (c >= 'A' && c <= 'Z') iLtt++;
                else iSym++;
            }

            if (iLtt == 0 && iSym == 0) return PasswordStrength.Weak; //纯数字密码
            if (iNum == 0 && iLtt == 0) return PasswordStrength.Weak; //纯符号密码
            if (iNum == 0 && iSym == 0) return PasswordStrength.Weak; //纯字母密码

            if (password.Length <= 6) return PasswordStrength.Weak;   //长度不大于6的密码

            if (iLtt == 0) return PasswordStrength.Normal; //数字和符号构成的密码
            if (iSym == 0) return PasswordStrength.Normal; //数字和字母构成的密码
            if (iNum == 0) return PasswordStrength.Normal; //字母和符号构成的密码

            if (password.Length <= 10) return PasswordStrength.Normal; //长度不大于10的密码

            return PasswordStrength.Strong; //由数字、字母、符号构成的密码
        }
        #endregion

        #region  "去除空格"
        public static string ToTrim(string str)
        {
            if (str == null)
                return null;
            return Regex.Replace(str.Trim(), @"\s", "");
        }
        #endregion

        #region "打乱List的顺序"
        /// <summary>
        /// 打乱List的顺序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputList"></param>
        /// <returns></returns>
        public static List<T> GetRandomList<T>(List<T> inputList)
        {
            //Copy to a array
            T[] copyArray = new T[inputList.Count];
            inputList.CopyTo(copyArray);

            //Add range
            List<T> copyList = new List<T>();
            copyList.AddRange(copyArray);

            //Set outputList and random
            List<T> outputList = new List<T>();
            Random rd = new Random(DateTime.Now.Millisecond);

            while (copyList.Count > 0)
            {
                //Select an index and item
                int rdIndex = rd.Next(0, copyList.Count - 1);
                T remove = copyList[rdIndex];

                //remove it from copyList and add it to output
                copyList.Remove(remove);
                outputList.Add(remove);
            }
            return outputList;
        }
        #endregion



        public static string ToHtml(string Text)
        {

            return Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace("<p>" + Text + "</p>", "\r\n", "</p><p>"), "\r", "</p><p>"), "\n", "<br />"), "\t", "    "), "  ", "  ");

        }

        public static string GetBankAccountTail(string bankAccount)
        {
            if (string.IsNullOrEmpty(bankAccount))
                return null;

            return bankAccount.Length > 3 ? bankAccount.Substring(bankAccount.Length - 4) : bankAccount;
        }

        public static string GetBankLogo(string bankName)
        {
            string url = "http://112.74.179.185:8085/upload/BankLogo";
            string logo = "";
            if (string.IsNullOrEmpty(bankName))
            {
                bankName = "";
            }

            if (bankName.IndexOf("广州农村信用合作社") > -1)
            {
                logo = "logo_GZNCXYHZS.png";
            }
            else if (bankName.IndexOf("建设银行") > -1)
            {
                logo = "logo_JIANSHE.png";
            }
            else if (bankName.IndexOf("农业银行") > -1)
            {
                logo = "logo_NONGYE.png";
            }
            else if (bankName.IndexOf("邮政银行") > -1)
            {
                logo = "logo_YOUZHENG.png";
            }
            else if (bankName.IndexOf("招商银行") > -1)
            {
                logo = "logo_ZHAOSHANG.png";
            }
            else if (bankName.IndexOf("中国银行") > -1)
            {
                logo = "logo_ZHONGGUO.png";
            }
            else
            {
                logo = "logo_Default.png";
            }

            return string.Format("{0}/{1}", url, logo);

        }


        public static bool CanViewErrorStackTrace()
        {
            string[] canViewIp = new string[] { "127.0.0.1", "::1" };


            string ip = CommonUtils.GetIP();

            if (canViewIp.Contains(ip))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }


}
