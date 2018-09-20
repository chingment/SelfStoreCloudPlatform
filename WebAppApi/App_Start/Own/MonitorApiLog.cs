using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebAppApi
{

    public class SignatureData
    {
        public string Key
        {
            get;
            set;
        }

        public string Sign
        {
            get;
            set;
        }

        public string TimeStamp
        {
            get;
            set;
        }

        public string Data
        {
            get;
            set;
        }

        public string Secret
        {
            get;
            set;
        }
    }

    public class MonitorApiLog
    {

        public MonitorApiLog()
        {
            this.SignatureData = new SignatureData();
        }

        public SignatureData SignatureData { get; set; }

        public string RequestUrl
        {
            get;
            set;
        }

        public DateTime RequestTime
        {
            get;
            set;
        }
        public DateTime? ResponseTime
        {
            get;
            set;
        }

        public string ResponseData
        {
            get;
            set;
        }

        public override string ToString()
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            return json;
        }
    }
}