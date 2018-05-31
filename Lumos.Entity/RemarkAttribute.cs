using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 备注特性
    /// </summary>
    public class RemarkAttribute : Attribute
    {
        private string _CnName;
        public RemarkAttribute(string cnname)
        {
            this._CnName = cnname;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string CnName
        {
            get { return _CnName; }
            set { _CnName = value; }
        }
        /// <summary>
        /// 获取枚举的备注信息
        /// </summary>
        /// <param name="val">枚举值</param>
        /// <returns></returns>
        public static string GetEnumRemark(Enum val)
        {
            Type type = val.GetType();
            FieldInfo fd = type.GetField(val.ToString());
            if (fd == null)
                return string.Empty;
            object[] attrs = fd.GetCustomAttributes(typeof(RemarkAttribute), false);
            string name = string.Empty;
            foreach (RemarkAttribute attr in attrs)
            {
                name = attr._CnName;
            }
            return name;
        }

    }

    /// <summary>
    /// 枚举扩展类
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举的备注信息
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        public static string GetCnName(this Enum em)
        {

            Type type = em.GetType();
            FieldInfo fd = type.GetField(em.ToString());
            if (fd == null)
                return string.Empty;
            object[] attrs = fd.GetCustomAttributes(typeof(RemarkAttribute), false);
            string name = string.Empty;
            foreach (RemarkAttribute attr in attrs)
            {
                name = attr.CnName;
            }
            return name;
        }



        public static string ToEnumCnName(this int v, string typname)
        {
            try
            {
                if (string.IsNullOrEmpty(typname))
                    return "";

                Type ot = Type.GetType(typname);
                if (ot == null)
                    return "";

                object a = Enum.ToObject(ot, v);
                if (a == null)
                    return "";
                string b = ((Enum)a).GetCnName();

                return b;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

    }
}
