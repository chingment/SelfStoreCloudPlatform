using Lumos.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Lumos.Mvc
{
    #region CustomJsonResult 自定义JsonResult
    /// <summary>
    /// 扩展JsonResult
    /// </summary>
    /// 



    public class CustomJsonResult<T> : ActionResult, IResult<T>
    {
        private ResultType _result = ResultType.Unknown;
        private ResultCode _code = ResultCode.Unknown;
        private string _message = "";
        private T _data;

        /// <summary>
        /// 结果状态默认为Unknown
        /// </summary>
        public ResultType Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }

        /// <summary>
        /// 信息默认返回空字符串
        /// </summary>
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        /// <summary>
        /// 信息默认返回空字符串
        /// </summary>
        public ResultCode Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
            }
        }

        /// <summary>
        /// 内容默认为null
        /// </summary>
        public T Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        public JsonConverter[] JsonConverter
        {
            get;
            set;
        }

        public JsonSerializerSettings JsonSerializerSettings
        {
            get;
            set;
        }

        public CustomJsonResult()
        {
            this.JsonSerializerSettings = new JsonSerializerSettings();
        }

        private void SetCustomJsonResult(string contenttype, ResultType type, ResultCode code, string message, T data, JsonSerializerSettings settings, params JsonConverter[] converters)
        {
            //this.ContentType = contenttype;
            this._result = type;
            this._code = code;
            this._message = message;
            this._data = data;
            this.JsonSerializerSettings = settings;
            this.JsonConverter = converters;
        }

        public CustomJsonResult(string contenttype, ResultType type, ResultCode code, string message, T content, JsonSerializerSettings settings, params JsonConverter[] converters)
        {
            SetCustomJsonResult(contenttype, type, code, message, content, settings, converters);
        }

        public CustomJsonResult(string contenttype, ResultType type, string message, T content, JsonSerializerSettings settings, params JsonConverter[] converters)
        {
            SetCustomJsonResult(contenttype, type, ResultCode.Unknown, message, content, settings, converters);
        }

        public CustomJsonResult(string contenttype, ResultType type, string message, T content, params JsonConverter[] converters)
        {
            SetCustomJsonResult(contenttype, type, ResultCode.Unknown, message, content, null, converters);
        }

        public CustomJsonResult(string contenttype, ResultType type, string message, T content, JsonSerializerSettings settings)
        {
            SetCustomJsonResult(contenttype, type, ResultCode.Unknown, message, content, settings);
        }

        public CustomJsonResult(ResultType type, ResultCode code, string message, T content, params JsonConverter[] converters)
        {
            SetCustomJsonResult(null, type, code, message, content, null, converters);
        }


        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            //if (!String.IsNullOrEmpty(ContentType))
            //{
            //    response.ContentType = ContentType;
            //}
            //else
            //{
            //    response.ContentType = "application/json";
            //}

            //if (ContentEncoding != null)
            //{
            //    response.ContentEncoding = ContentEncoding;
            //}

            response.Write(GetResultJson());

        }

        public override string ToString()
        {
            return GetResultJson();
        }


        public string GetResultJson()
        {
            StringBuilder json = new StringBuilder();
            json.Append("{");

            try
            {
                json.Append("\"result\": " + (int)this._result + ",");

                if (this._data != null)
                {
                    if (this._data is string)
                    {
                        if (!string.IsNullOrWhiteSpace(this._data.ToString()))
                        {
                            json.Append("\"data\":" + this._data + ",");
                        }
                    }
                    else
                    {

                        if (this.JsonSerializerSettings == null)
                        {
                            this.JsonSerializerSettings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };

                        }

                        JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
                        {
                            //日期类型默认格式化处理
                            this.JsonSerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                            this.JsonSerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                            return this.JsonSerializerSettings;
                        });
                        this.JsonSerializerSettings.Converters = this.JsonConverter;
                        this.JsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        json.Append("\"data\":" + JsonConvert.SerializeObject(this._data, Formatting.None, this.JsonSerializerSettings) + ",");
                    }
                }

                if (this._code != ResultCode.Unknown)
                {
                    json.Append("\"code\": \"" + ((int)this._code).ToString() + "\",");
                }


                json.Append("\"message\":" + JsonConvert.SerializeObject(this._message) + "");


            }
            catch (Exception ex)
            {

                json.Append("\"result\": " + (int)ResultType.Exception + ",");
                json.Append("\"message\":\"" + string.Format("CustomJsonResult转换发生异常:{0}", ex.Message) + "\"");
                //转换失败记录日志
            }
            json.Append("}");

            string s = json.ToString();

            return s;
        }
    }

    public class CustomJsonResult : CustomJsonResult<object>, IResult
    {

        public CustomJsonResult()
        {

        }

        public CustomJsonResult(ResultType type, string message) : base(type, ResultCode.Unknown, message, null, null)
        {

        }

        public CustomJsonResult(ResultType type, ResultCode code, string message) : base(type, code, message, null, null)
        {

        }

        public CustomJsonResult(ResultType type, ResultCode code, string message, object content) : base(type, code, message, content, null)
        {

        }

        public CustomJsonResult(ResultType type, ResultCode code, string message, object content, params JsonConverter[] converters) : base(type, code, message, content, converters)
        {

        }

        public CustomJsonResult(string contenttype, ResultType type, ResultCode code, string message, object content, JsonSerializerSettings settings, params JsonConverter[] converters) : base(contenttype, type, code, message, content, settings, converters)
        {

        }

        public CustomJsonResult(string contenttype, ResultType type, string message, object content, JsonSerializerSettings settings, params JsonConverter[] converters) : base(contenttype, type, message, content, settings, converters)
        {

        }

        public CustomJsonResult(string contenttype, ResultType type, string message, object content, params JsonConverter[] converters) : base(contenttype, type, message, content, converters)
        {

        }

        public CustomJsonResult(string contenttype, ResultType type, string message, object content, JsonSerializerSettings settings) : base(contenttype, type, message, content, settings)
        {

        }


    }

    #endregion
}
