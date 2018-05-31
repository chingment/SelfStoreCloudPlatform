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
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public SysRole()
        {

        }

        public SysRole(string name) : this() { Name = name; }

        /// <summary>
        /// 父角色ID
        /// </summary>
        public virtual int PId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(512)]
        public virtual string Description { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }
    }
}
