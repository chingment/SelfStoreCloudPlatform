using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lumos.WeiXinSdk.MsgPush
{
    public class WxMsgFactory
    {

        private static T ConvertObj<T>(string xmlstr)
        {
            XElement xdoc = XElement.Parse(xmlstr);
            var type = typeof(T);
            var t = Activator.CreateInstance<T>();
            foreach (XElement element in xdoc.Elements())
            {
                var pr = type.GetProperty(element.Name.ToString());
                if (element.HasElements)
                {//这里主要是兼容微信新添加的菜单类型。nnd，竟然有子属性，所以这里就做了个子属性的处理
                    foreach (var ele in element.Elements())
                    {
                        pr = type.GetProperty(ele.Name.ToString());
                        pr.SetValue(t, Convert.ChangeType(ele.Value, pr.PropertyType), null);
                    }
                    continue;
                }
                if (pr != null)
                {
                    if (pr.PropertyType.Name == "MsgType")//获取消息模型
                    {
                        pr.SetValue(t, (MsgType)Enum.Parse(typeof(MsgType), element.Value.ToUpper()), null);
                        continue;
                    }
                    if (pr.PropertyType.Name == "EventType")//获取事件类型。
                    {
                        pr.SetValue(t, (EventType)Enum.Parse(typeof(EventType), element.Value.ToUpper()), null);
                        continue;
                    }
                    pr.SetValue(t, Convert.ChangeType(element.Value, pr.PropertyType), null);
                }
            }
            return t;
        }

        private static List<BaseMsg> _queue;
        public static BaseEventMsg CreateMessage(string xml)
        {
            if (_queue == null)
            {
                _queue = new List<BaseMsg>();
            }
            else if (_queue.Count >= 50)
            {
                _queue = _queue.Where(q => { return q.CreateTime.AddSeconds(20) > DateTime.Now; }).ToList();//保留20秒内未响应的消息
            }
            XElement xdoc = XElement.Parse(xml);
            var msgtype = xdoc.Element("MsgType").Value.ToUpper();
            //var FromUserName = xdoc.Element("FromUserName").Value;
            //var MsgId = xdoc.Element("MsgId").Value;
            //var CreateTime = xdoc.Element("CreateTime").Value;
            MsgType type = (MsgType)Enum.Parse(typeof(MsgType), msgtype);
            //if (type != MsgType.EVENT)
            //{
            //    if (_queue.FirstOrDefault(m => { return m.MsgFlag == MsgId; }) == null)
            //    {
            //        _queue.Add(new BaseMsg
            //        {
            //            CreateTime = DateTime.Now,
            //            FromUser = FromUserName,
            //            MsgFlag = MsgId
            //        });
            //    }
            //    else
            //    {
            //        return null;
            //    }

            //}
            //else
            //{
            //    if (_queue.FirstOrDefault(m => { return m.MsgFlag == CreateTime; }) == null)
            //    {
            //        _queue.Add(new BaseMsg
            //        {
            //            CreateTime = DateTime.Now,
            //            FromUser = FromUserName,
            //            MsgFlag = CreateTime
            //        });
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //}

            switch (type)
            {
                case MsgType.TEXT:
                    return ConvertObj<TextMsg>(xml);
                case MsgType.EVENT://事件类型

                    var eventtype = (EventType)Enum.Parse(typeof(EventType), xdoc.Element("Event").Value.ToUpper());
                    switch (eventtype)
                    {
                        case EventType.SUBSCRIBE:
                            return ConvertObj<SubEventMsg>(xml);
                        case EventType.UNSUBSCRIBE:
                            return ConvertObj<UnSubEventMsg>(xml);
                        case EventType.SCAN:
                        case EventType.CLICK:
                        case EventType.VIEW:
                            return ConvertObj<LinkEventMsg>(xml);
                        case EventType.USER_GET_CARD:
                            return ConvertObj<UserGetCardMsg>(xml);
                        case EventType.USER_CONSUME_CARD:
                            return ConvertObj<UserConsumeCardMsg>(xml);
                    }

                    break;

            }

            return null;
        }

        public static string CreateReplyText(string toUserName, string fromUserName, string content)
        {
            string replyTextFormat = @"<xml>
<ToUserName><![CDATA[{0}]]></ToUserName>
<FromUserName><![CDATA[{1}]]></FromUserName>
<CreateTime>{2}</CreateTime>
<MsgType><![CDATA[text]]></MsgType>
<Content><![CDATA[{3}]]></Content>
</xml>";

            string replyText = string.Format(replyTextFormat, toUserName, fromUserName, DateTime.Now.ToBinary(), content);

            return replyText;
        }

        public static string CreateReplyImage(string toUserName, string fromUserName, string media_id)
        {
            string replyTextFormat = @"<xml>
<ToUserName><![CDATA[{0}]]></ToUserName>
<FromUserName><![CDATA[{1}]]></FromUserName>
<CreateTime>{2}</CreateTime>
<MsgType><![CDATA[image]]></MsgType>
<Image><MediaId><![CDATA[{3}]]></MediaId></Image>
</xml>";
            string replyText = string.Format(replyTextFormat, toUserName, fromUserName, DateTime.Now.ToBinary(), media_id);

            return replyText;
        }
    }
}
