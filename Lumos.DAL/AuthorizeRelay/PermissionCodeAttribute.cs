using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.DAL.AuthorizeRelay
{
    /// <summary>
    /// 权限属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class PermissionCodeAttribute : Attribute
    {
        public string PId { get; set; }
    }
}
