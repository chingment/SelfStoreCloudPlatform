using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{

    //角色信息表AspNetRoles
    //通过一个类的继承来扩展IdentityRole的属性对应的表是AspNetRoles表
    //在这里测试 添加了Description属性
    [Table("SysRole")]
    public class SysRole 
    {
        [Key]
        public string Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public string PId { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }

        public string Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public string Mender { get; set; }

        public DateTime? MendTime { get; set; }
    }
}
