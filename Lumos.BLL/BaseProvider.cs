using Lumos.DAL;
using System;
using System.Reflection;


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

    }
}
