﻿using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Security.Cryptography;
using WebMerch;
using Lumos;

namespace System.Web
{
    public static class OwnHmlExtension
    {
        public static IHtmlString initEnumeration<T>(this HtmlHelper helper, Enumeration.InputType inputType, string name, object htmlAttributes = null)
        {
            IDictionary<string, object> HtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            StringBuilder sb = new StringBuilder();

            var hidValue = new T[] { };
            if (HtmlAttributes["hidevalue"] != null)
            {
                hidValue = (T[])HtmlAttributes["hidevalue"];
            }

            var classname = "";
            if (HtmlAttributes["class"] != null)
            {
                classname = "class=\"" + HtmlAttributes["class"].ToString() + "\"";
            }

            var selectValue = new T[] { };
            if (HtmlAttributes["selectedvalue"] != null)
            {
                selectValue = (T[])HtmlAttributes["selectedvalue"];
            }

            string defaulttext = "";
            if (HtmlAttributes["defaulttext"] != null)
            {
                defaulttext = HtmlAttributes["defaulttext"].ToString();
            }

            if (inputType == Enumeration.InputType.Select)
            {
                int i = 0;
                string id = name.Replace(".", "_");
                sb.Append("<select name=\"" + name + "\" id=\"" + id + "\"  " + classname + ">");
                if (defaulttext != "")
                {
                    sb.Append("<option value=\"\"  >" + defaulttext + "</option>");
                }
                foreach (T t in Enum.GetValues(typeof(T)))
                {
                    string strKey = Convert.ToInt32(t).ToString();
                    Enum en = (Enum)Enum.Parse(t.GetType(), strKey);
                    string strValue = en.GetCnName();
                    string checkeds = "";
                    if (selectValue != null)
                    {
                        if (selectValue.Length > 0)
                        {
                            if (selectValue.Contains(t))
                            {
                                checkeds = "selected";
                            }
                        }
                    }

                    bool isHide = false;
                    if (hidValue != null)
                    {
                        if (hidValue.Length > 0)
                        {
                            if (hidValue.Contains(t))
                            {
                                isHide = true;
                            }
                        }
                    }
                    if (!isHide)
                    {
                        sb.Append(" <option value=\"" + strKey + "\" " + checkeds + ">" + strValue + "</option>");
                    }
                    i++;

                }
                sb.Append("/<select>");
            }
            else if (inputType == Enumeration.InputType.CheckBox)
            {

                int i = 0;
                foreach (T t in Enum.GetValues(typeof(T)))
                {
                    string strKey = Convert.ToInt32(t).ToString();
                    string strValue = Enum.GetName(typeof(T), t);
                    string id = name + i;

                    string checkeds = "";
                    if (selectValue != null)
                    {
                        for (int j = 0; j < selectValue.Length; j++)
                        {
                            string key1 = strValue;
                            string key2 = selectValue[j].ToString();
                            if (key1 == key2)
                            {
                                checkeds = "checked";
                                break;
                            }
                        }
                    }

                    sb.Append(" <input type=\"checkbox\" name=\"" + name + "\" id=\"" + id + "\"  value=\"" + strKey + "\" " + checkeds + " /><label for=\"" + id + "\">" + strValue + "</label>");



                    i++;

                }
            }
            else if (inputType == Enumeration.InputType.Radio)
            {
                int i = 0;
                foreach (T t in Enum.GetValues(typeof(T)))
                {
                    string strKey = Convert.ToInt32(t).ToString();
                    string strValue = Enum.GetName(typeof(T), t);
                    string id = name + i;
                    string checkeds = "";
                    if (selectValue != null)
                    {
                        if (selectValue.Length > 0)
                        {
                            string key1 = strValue;
                            string key2 = selectValue[0].ToString();
                            if (key1 == key2)
                            {
                                checkeds = "checked";
                            }
                        }
                    }
                    sb.Append(" <input type=\"radio\" name=\"" + name + "\" id=\"" + name + "\" value=\"" + strKey + "\" " + checkeds + "  /><label for=\"" + id + "\">" + strValue + "</label>");
                    i++;
                }
            }


            return new MvcHtmlString(sb.ToString());
        }

        public static IHtmlString IsInPermission(this HtmlHelper helper, object value, params string[] permissions)
        {
            if (permissions == null)
                return helper.Raw(value);

            if (permissions.Length == 0)
                return helper.Raw(value);

            bool isHas = OwnRequest.IsInPermission(permissions);
            if (isHas)
            {
                return helper.Raw(value);
            }
            else
            {
                return helper.Raw("");
            }
        }


        public static MvcHtmlString initProductKind(this HtmlHelper helper, string name, List<ProductKind> productKind, string selectval = null)
        {

            StringBuilder sb = new StringBuilder();

            string id = name.Replace('.', '_');
            sb.Append("<select multiple='multiple' id=\"" + id + "\" data-placeholder=\"选择分类模块\" name =\"" + name + "\" class=\"chosen-select\" style=\"width: 400px;\" >");
            sb.Append("<option value=\"-1\"></option>");

            var p_category = productKind.Where(m => m.PId == GuidUtil.Empty()).ToList();

            string[] arr_selectval = null;

            if (selectval != null)
            {
                arr_selectval = selectval.Split(',');
            }

            foreach (var m in p_category)
            {
                var catalogList1 = productKind.Where(c => c.PId == m.Id).ToList();

                string disabled = "";

                if (catalogList1.Count > 0)
                {
                    disabled = "disabled";
                }

                string selected = "";

                if (selectval != null)
                {
                    if (arr_selectval.Contains(m.Id.ToString()))
                    {
                        selected = "selected";
                    }
                }

                sb.Append("<option value=\"" + m.Id + "\"  " + disabled + " " + selected + "   >&nbsp;" + m.Name + "</option>");

                GetProductKindNodes(sb, productKind, m.Id, 1, arr_selectval);
            }

            sb.Append("</select>");
            return new MvcHtmlString(sb.ToString());
        }

        private static void GetProductKindNodes(StringBuilder sb, List<ProductKind> productKind, string pId, int nLevel, string[] arr_selectval)
        {

            var catalogList = productKind.Where(m => m.PId == pId).ToList();

            string _s = "";
            for (int i = 0; i <= nLevel; i++)
            {
                _s += "&nbsp;&nbsp;";
            }
            _s += "&nbsp;&nbsp;";
            nLevel += 1;



            foreach (var m in catalogList)
            {
                var catalogList1 = productKind.Where(c => c.PId == m.Id).ToList();

                string disabled = "";

                if (catalogList1.Count > 0)
                {
                    disabled = "disabled";
                }

                string selected = "";
                if (arr_selectval != null)
                {
                    if (arr_selectval.Contains(m.Id.ToString()))
                    {
                        selected = "selected";
                    }
                }

                sb.Append("<option value=\"" + m.Id + "\" " + disabled + " " + selected + "  >" + _s + m.Name + "</option>");

                GetProductKindNodes(sb, productKind, m.Id, nLevel, arr_selectval);
            }
        }
    }
}