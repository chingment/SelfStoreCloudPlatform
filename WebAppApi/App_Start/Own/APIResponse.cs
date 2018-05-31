using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace WebAppApi
{
    public class APIResponse : HttpResponseMessage
    {
        public APIResponse(IResult apiResult)
        {
            StringContent content=new StringContent(apiResult.ToString(), Encoding.GetEncoding("UTF-8"), "application/json");
            this.Content=content;
        }
    }

    public class APIResponse<T> : HttpResponseMessage
    {
        public APIResponse(IResult<T> apiResult)
        {
            StringContent content = new StringContent(apiResult.ToString(), Encoding.GetEncoding("UTF-8"), "application/json");
            this.Content = content;
        }
    }
}