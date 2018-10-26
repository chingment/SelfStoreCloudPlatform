using Lumos;
using System.Net.Http;
using System.Text;


namespace WebAppApi
{
    public class OwnApiHttpResponse : HttpResponseMessage
    {
        public OwnApiHttpResponse(IResult apiResult)
        {
            StringContent content=new StringContent(apiResult.ToString(), Encoding.GetEncoding("UTF-8"), "application/json");
            this.Content=content;
        }
    }

    public class OwnApiHttpResponse<T> : HttpResponseMessage
    {
        public OwnApiHttpResponse(IResult<T> apiResult)
        {
            StringContent content = new StringContent(apiResult.ToString(), Encoding.GetEncoding("UTF-8"), "application/json");
            this.Content = content;
        }
    }
}