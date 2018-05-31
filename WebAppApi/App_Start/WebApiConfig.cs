using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using WebAppApi.Controllers;

namespace WebAppApi
{
    public class UploadMultipartMediaTypeFormatter : MediaTypeFormatter
    {
        public UploadMultipartMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }
        //

        public override bool CanWriteType(Type type)
        {
            return true;
        }
    }

    public class WebImageMediaFormatter : MediaTypeFormatter
    {
        public WebImageMediaFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
        }
        public override bool CanReadType(Type type)
        {
            return type == typeof(WebImage);
        }
        public override bool CanWriteType(Type type)
        {
            return false;
        }
        public async override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger, CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[content.Headers.ContentLength.Value];
            while (await readStream.ReadAsync(buffer, (int)readStream.Position, buffer.Length - (int)readStream.Position) > 0) { }
            string stringData = Encoding.Default.GetString(buffer);
            JObject myJson = JObject.Parse(stringData);
            JToken myJToken = myJson.GetValue("imageBytes");
            byte[] myBytes = myJToken.Values().Select(x => (byte)x).ToArray();
            return new WebImage(myBytes);
        }
    }

    public class UnsupportedMediaTypeConnegHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            var contentType = request.Content.Headers.ContentType;
            var formatters = request.GetConfiguration().Formatters;
            var hasFormetterForContentType = formatters //
                .Any(formatter => formatter.SupportedMediaTypes.Contains(contentType));

            if (!hasFormetterForContentType)
            {
                return Task<HttpResponseMessage>.Factory //
                    .StartNew(() => new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType));
            }

            return base.SendAsync(request, cancellationToken);
        }
    }

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.Filters.Add(new APIExceptionAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            // 干掉XML序列化器
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // 解决json序列化时的循环引用问题
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            // 对 JSON 数据使用混合大小写。驼峰式,但是是javascript 首字母小写形式.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            // 对 JSON 数据使用混合大小写。跟属性名同样的大小.输出
            // config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new DefaultContractResolver()

        }
    }
}
