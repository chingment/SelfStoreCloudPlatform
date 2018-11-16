using Lumos;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;


namespace WebAppApi
{

    public class OwnApiHttpResult : IResult
    {
        private ResultType _result = ResultType.Unknown;
        private ResultCode _code = ResultCode.Unknown;
        private string _message = "";
        private object _data = null;

        public OwnApiHttpResult()
        {

        }

        public OwnApiHttpResult(ResultType result, ResultCode code, string message, object data = null)
        {
            _result = result;
            _code = code;
            _message = message;
            _data = data;
        }


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

        public object Data
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

        public override string ToString()
        {
            StringBuilder json = new StringBuilder();
            json.Append("{");
            json.Append("\"result\":" + (int)_result + ",");
            json.Append("\"code\":" + (int)_code +",");
            json.Append("\"message\":" + JsonConvert.SerializeObject(_message) + "");

            if (_data != null)
            {
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                jsonSerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                jsonSerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";


                json.Append(",\"data\":" + JsonConvert.SerializeObject(_data, jsonSerializerSettings) + "");
            }

            json.Append("}");

            return json.ToString();
        }
    }
}