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

        public enum StoreStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("正常")]
            Opened = 1,
            [Remark("关闭")]
            Closed = 2
        }

        public enum CompanyClass
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("供应商")]
            Supplier = 1
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

        public enum WarehouseStockLogChangeTpye
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("入库单")]
            StockIn = 1
        }



        public enum ReceptionMode
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("机器自取")]
            Machine = 1,
            [Remark("快递专送")]
            Express = 2
        }

        public enum BizSnType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("入库单号")]
            Order2StockIn = 1
        }
    }
}
