using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    /// <summary>
    /// 业务的枚举
    /// </summary>
    public partial class Enumeration
    {

        public enum MerchantMachineStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("已绑定")]
            Bind = 1,
            [Remark("已解绑")]
            Unbind = 2
        }

        public enum StoreMachineStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("已绑定")]
            Bind = 1,
            [Remark("已解绑")]
            Unbind = 2
        }

        public enum StoreStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("设置中")]
            Setting = 1,
            [Remark("维护中")]
            Maintain = 2,
            [Remark("正常")]
            Opened = 3,
            [Remark("关闭")]
            Closed = 4
        }

        public enum BizProcessesAuditType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("车险订单")]
            OrderToCarInsure = 1,
            [Remark("车险理赔")]
            OrderToCarClaim = 2,
            [Remark("商户资料审核")]
            MerchantAudit = 3,
            [Remark("人才输送")]
            OrderToTalentDemand = 4,
            [Remark("定损点申请")]
            OrderToApplyLossAssess = 5,
            [Remark("违章处理")]
            OrderToLllegalDealt = 6,
            [Remark("贷款")]
            OrderToCredit = 7,
            [Remark("保险产品订单")]
            OrderToInsurance = 8
        }

        public enum MachineBannerType
        {
            Unknow = 0,
            MainHomeTop = 1
        }

        public enum MachineBannerStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("正常")]
            Normal = 1,
            [Remark("已删除")]
            Deleted = 2
        }

        public enum ProductKindStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("正常")]
            Valid = 1,
            [Remark("停用")]
            Invalid = 2
        }


        public enum MachineStockLogChangeTpye
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("销售")]
            Sales = 1,
            [Remark("锁定")]
            Lock = 2,
            [Remark("解锁")]
            UnLock = 3
        }
    }
}
