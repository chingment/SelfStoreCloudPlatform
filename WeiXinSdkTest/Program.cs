using Lumos.WeiXinSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeiXinSdkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //1Vi0WyupqfNSpUx22KCUYVVIPHQaa2Dnnjt65IKoLPKOIaoDwwBQZrlnal71nPC1

            WxApi c = new WxApi();
            WxApiAccessToken apiAccessToken = new WxApiAccessToken("client_credential", "wx60443bbfd97f6aa5", "dfd062f723c38519092dfd7646af2f76");
            var result = c.DoGet(apiAccessToken);



            WxApiUserInfoTest(result.access_token, "oUWrIjoJpzcbnwpcQ2IG4C1172To");
            WxApiMessageTemplateSendTest(result.access_token, "oUWrIjoJpzcbnwpcQ2IG4C1172To");
          //  WxApiMediaUploadNewsTest(result.access_token);
          // WxApiMessageMassSendText(result.access_token,"1Vi0WyupqfNSpUx22KCUYVVIPHQaa2Dnnjt65IKoLPKOIaoDwwBQZrlnal71nPC1");

            //postData = "{\"touser\":[\"oUWrIjoJpzcbnwpcQ2IG4C1172To\",\"oUWrIjoJpzcbnwpcQ2IG4C1172To\"], \"mpnews\":{\"media_id\":\"" + result3.media_id + "\" },\"msgtype\":\"mpnews\"}";

            //WebApiMessageMassSend messageMassSend = new WebApiMessageMassSend(result.access_token, WxPostDataType.Text, postData);

            //var result4 = c.DoPost(messageMassSend);

        }


        public static void WxApiUserInfoTest(string access_token, string open_id)
        {
            WxApi c = new WxApi();
            WxApiUserInfo apiUserInfo = new WxApiUserInfo(access_token, open_id);
            var result = c.DoGet(apiUserInfo);

        }

        public static void WxApiMediaUploadNewsTest(string access_token)
        {
            string postData = "{\"articles\": [{\"thumb_media_id\":\"1Vi0WyupqfNSpUx22KCUYVVIPHQaa2Dnnjt65IKoLPKOIaoDwwBQZrlnal71nPC1\",\"author\":\"xxx\",\"title\":\"Happy Day\",\"content_source_url\":\"www.qq.com\",\"content\":\"content\",\"digest\":\"digest\",\"show_cover_pic\":1 } ]}";

            WxApiMediaUploadNewsModel model = new WxApiMediaUploadNewsModel();

            Article article = new Article();

            article.show_cover_pic = "1";
            article.thumb_media_id = "1Vi0WyupqfNSpUx22KCUYVVIPHQaa2Dnnjt65IKoLPKOIaoDwwBQZrlnal71nPC1";


            Author author = new Author();
            author.title = "Happy Day";
            author.content_source_url = "www.qq.com";
            author.content = "content";
            author.digest = "digest";
            article.author = author;

            model.articles.Add(article);

            WxApiMediaUploadNews mediaUploadNews = new WxApiMediaUploadNews(access_token, WxPostDataType.Json, model);
            WxApi c = new WxApi();
            var result = c.DoPost(mediaUploadNews);

        }

        public static void WxApiMessageMassSendText(string access_token, string media_id)
        {
            string postData = "{\"touser\":[\"oUWrIjoJpzcbnwpcQ2IG4C1172To\",\"oUWrIjoJpzcbnwpcQ2IG4C1172To\"], \"mpnews\":{\"media_id\":\"" + media_id + "\" },\"msgtype\":\"mpnews\"}";
            WxApi c = new WxApi();
            WxApiMessageMassSend messageMassSend = new WxApiMessageMassSend(access_token, WxPostDataType.Text, postData);

            var result4 = c.DoPost(messageMassSend);
        }

        public static void WxApiMessageTemplateSendTest(string access_token,string openid)
        {
            //http://www.thinksaas.cn/topics/0/348/348237.html
            string postData = "{\"touser\":\"" + openid + "\",\"template_id\":\"HNnR_Yp2ZcjC0-vj_jfi2wLTlYwYl8QnFx6vx0dr7cE\",\"url\":\"http://www.feytil.com\",\"data\":{\"first\":{ \"value\":\"学生迟到\",\"color\":\"#173177\" },\"student\":{ \"value\":\"巧克力\",\"color\":\"#173177\"}, \"date\":{\"value\":\"2016-05-16\",\"color\":\"#173177\" },\"remark\":{\"value\":\"欢迎再次购买！\",\"color\":\"#173177\"}}}";

         

            WxApiMessageTemplateSend mediaUploadNews = new WxApiMessageTemplateSend(access_token, WxPostDataType.Text, postData);
            WxApi c = new WxApi();
            var result = c.DoPost(mediaUploadNews);

        }
    }
}
