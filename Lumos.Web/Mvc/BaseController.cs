using Lumos.DAL;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace Lumos.Web.Mvc
{
    public abstract class BaseController : Controller
    {
        #region JsonResult 扩展

        protected internal CustomJsonResult Json(ResultType type)
        {
            return Json(type, null, null);
        }

        protected internal CustomJsonResult Json(ResultType type, string message)
        {
            return Json(type, null, message);
        }

        protected internal CustomJsonResult Json(ResultType type, object content)
        {
            return Json(type, content, null);
        }

        protected CustomJsonResult Json(ResultType type, object content, string message, params JsonConverter[] converters)
        {
            return Json(null, type, ResultCode.Unknown, message, content, null, converters);
        }

        protected CustomJsonResult Json(ResultType type, object content, string message, JsonSerializerSettings settings)
        {
            return Json(null, type, ResultCode.Unknown, message, content, settings);
        }

        protected CustomJsonResult Json(string contenttype, ResultType type, string message)
        {
            return Json(contenttype, type, null, message);
        }

        protected CustomJsonResult Json(string contenttype, ResultType type, object content, string message, params JsonConverter[] converters)
        {
            return Json(contenttype, type, ResultCode.Unknown, message, content, null);
        }

        protected CustomJsonResult Json(string contenttype, ResultType type, ResultCode code, string message, object content, JsonSerializerSettings settings, params JsonConverter[] converters)
        {
            return new CustomJsonResult(contenttype, type, code, message, content, settings, converters);
        }
        #endregion

        #region 公共的方法
        public string ConvertToZTreeJson(object obj, string idField, string pIdField, string nameField, string IconSkinField, params string[] isCheckedIds)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("[");
            Type t = obj.GetType();
            foreach (var model in (object[])obj)
            {
                Type t1 = model.GetType();
                Json.Append("{");
                foreach (PropertyInfo p in t1.GetProperties())
                {
                    string name = p.Name.Trim().ToLower();
                    object value = p.GetValue(model, null);
                    if (name == idField.ToLower())
                    {
                        Json.Append("\"id\":" + JsonConvert.SerializeObject(value) + ",\"open\":true,");
                        string v = value.ToString();
                        if (isCheckedIds.Contains(v))
                        {
                            Json.Append("\"checked\":true,");
                        }
                    }
                    else if (name == pIdField.Trim().ToLower())
                    {
                        Json.Append("\"pId\":" + JsonConvert.SerializeObject(value) + ",");

                        if (value == null || value.ToString() == "")
                        {
                            Json.Append("\"iconSkin\":\"" + IconSkinField + "\" ");
                            Json.Append(",");
                        }
                        else
                        {
                            Json.Append("\"iconSkin\":\"" + IconSkinField + "s\" ");
                            Json.Append(",");
                        }

                    }
                    else if (name == nameField.Trim().ToLower())
                    {
                        Json.Append("\"name\":" + JsonConvert.SerializeObject(value) + ",");

                    }
                    else
                    {
                        Json.Append("\"" + p.Name + "\":" + JsonConvert.SerializeObject(value) + ",");
                    }
                }
                if (Json.Length > 2)
                {
                    Json.Remove(Json.Length - 1, 1);
                }
                Json.Append("},");
            }
            if (Json.Length > 2)
            {
                Json.Remove(Json.Length - 1, 1);
            }
            Json.Append("]");
            return Json.ToString();
        }

        public string ConvertToZTreeJson2(object obj, string idField, string pIdField, string nameField, string IconSkinField, params string[] isCheckedIds)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("[");
            Type t = obj.GetType();
            foreach (var model in (object[])obj)
            {
                Type t1 = model.GetType();
                Json.Append("{");
                foreach (PropertyInfo p in t1.GetProperties())
                {
                    string name = p.Name.Trim().ToLower();
                    object value = p.GetValue(model, null);
                    if (name == idField.ToLower())
                    {
                        Json.Append("\"id\":" + JsonConvert.SerializeObject(value) + ",");
                        string v = value.ToString();
                        if (isCheckedIds.Contains(v))
                        {
                            Json.Append("\"checked\":true,");
                        }
                    }
                    else if (name == pIdField.Trim().ToLower())
                    {
                        Json.Append("\"pId\":0,");

                        if (value == null || value.ToString() == "")
                        {
                            Json.Append("\"iconSkin\":\"" + IconSkinField + "\" ");
                            Json.Append(",");
                        }
                        else
                        {
                            Json.Append("\"iconSkin\":\"" + IconSkinField + "s\" ");
                            Json.Append(",");
                        }

                    }
                    else if (name == nameField.Trim().ToLower())
                    {
                        Json.Append("\"name\":" + JsonConvert.SerializeObject(value) + ",");

                    }
                    else
                    {
                        Json.Append("\"" + p.Name + "\":" + JsonConvert.SerializeObject(value) + ",");
                    }
                }
                if (Json.Length > 2)
                {
                    Json.Remove(Json.Length - 1, 1);
                }
                Json.Append("},");
            }
            if (Json.Length > 2)
            {
                Json.Remove(Json.Length - 1, 1);
            }
            Json.Append("]");
            return Json.ToString();
        }


        #endregion 公共的方法


        public virtual string CurrentUserId { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            LogUtil.SetTrackId();
        }

        private LumosDbContext _currentDb;

        public LumosDbContext CurrentDb
        {
            get
            {
                return _currentDb;
            }
        }

        public BaseController()
        {
            _currentDb = new LumosDbContext();
        }
    }
}
