using Lumos.DAL;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Lumos.BLL
{
    public abstract class BaseProvider
    {
        private LumosDbContext _CurrentDb;
        private DateTime _dateNow;

        public BaseProvider()
        {
            _dateNow = DateTime.Now;
        }

        protected LumosDbContext CurrentDb
        {
            get
            {
                if (_CurrentDb == null)
                {
                    _CurrentDb = new LumosDbContext();
                }

                return _CurrentDb;
            }
        }

        protected DateTime DateTime
        {
            get
            {
                return _dateNow;
            }
        }

        public object CloneObject(object o)
        {
            Type t = o.GetType();
            PropertyInfo[] properties = t.GetProperties();
            Object p = t.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, o, null);
            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    object value = pi.GetValue(o, null);
                    pi.SetValue(p, value, null);
                }
            }
            return p;
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
    }
}
