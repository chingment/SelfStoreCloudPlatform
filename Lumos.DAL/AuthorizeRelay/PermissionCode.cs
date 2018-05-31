using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumos.DAL.AuthorizeRelay;

namespace System
{

    /// <summary>
    /// 权限代码
    /// </summary>
    public class PermissionCode
    {
        public const string 所有用户管理 = "Sys1000";
        public const string 后台用户管理 = "Sys2000";
        public const string 角色管理 = "Sys4000";
        public const string 菜单管理 = "Sys5000";

        public const string 订单查询 = "Biz2001";
        public const string POS机登记信息 = "Biz4003";
        public const string POS机更换 = "Biz4004";
        public const string 商户资料维护 = "Biz5001";
        public const string 商户资料初审 = "Biz5002";
        public const string 商户资料复审 = "Biz5003";
        public const string 车险订单报价 = "Biz6001";
        public const string 订单支付确认 = "Biz6002";
        public const string 理赔需求核实 = "Biz7001";
        public const string 理赔金额核实 = "Biz7002";
        public const string 扩展应用添加 = "Biz9001";
        public const string 人才需求核实 = "Biz10001";
        public const string 定损点申请 = "Biz20001";
        public const string 违章处理 = "Biz30001";
        public const string 贷款申请 = "Biz40001";
        public const string 保险购买 = "Biz50001";
        public const string 保险公司设置 = "BizA001";
        public const string 车险保险公司设置 = "BizA002";
        public const string 业务人员设置 = "BizB001";
        public const string 业务人员申领POS机登记 = "BizB002";
        public const string 商品设置 = "BizC001";
        public const string 广告设置 = "BizD001";
    }

}
