using System;


namespace Lumos.Entity
{
    /// <summary>
    /// 业务的枚举
    /// </summary>
    public partial class Enumeration
    {
        public enum Order2StockOutTargetType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("客户")]
            Tartget = 1,
            [Remark("店铺")]
            Store = 2
        }

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

        public enum StoreBannerStatus
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

        public enum ProductSubjectStatus
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
            StockIn = 1,
            [Remark("出库单")]
            StockOut = 2
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
            [Remark("订单号")]
            Order = 1,
            [Remark("入库单号")]
            Order2StockIn = 2,
            [Remark("出库单号")]
            Order2StockOut = 3
        }

        public enum BizSnOrderChannel
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("机器")]
            Machine = 1,
            [Remark("小程序")]
            MicroProgram = 2
        }


        public enum OrderNotifyLogNotifyFrom
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("网站")]
            WebApp = 1,
            [Remark("微信支付配置通知链接")]
            NotifyUrl = 2,
            [Remark("微信订单查询接口")]
            OrderQuery = 3
        }

        public enum OrderNotifyLogNotifyType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("支付通知")]
            Pay = 1,
            [Remark("退款通知")]
            ReFund = 2
        }

        public enum WxUserInfoFrom
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("授权")]
            Authorize = 1,
            [Remark("订阅")]
            Subscribe = 2
        }

        public enum WxAutoReplyType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("订阅")]
            Subscribe = 1,
            [Remark("关键字")]
            Keyword = 2,
            [Remark("菜单点击")]
            MenuClick = 3,
            [Remark("不是关键字")]
            NotKeyword = 4
        }

        public enum OrderStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("已提交")]
            Submitted = 1000,
            [Remark("待支付")]
            WaitPay = 2000,
            [Remark("已支付")]
            Payed = 3000,
            [Remark("已完成")]
            Completed = 4000,
            [Remark("已失效")]
            Cancled = 5000

        }


        public enum OrderDetailsChildSonStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("已提交")]
            Submitted = 1000,
            [Remark("待支付")]
            WaitPay = 2000,
            [Remark("已支付")]
            Payed = 30000,
            [Remark("待取货")]
            WaitPick = 3001,
            [Remark("已取货")]
            Picked = 3002,
            [Remark("已完成")]
            Completed = 4000,
            [Remark("已失效")]
            Cancled = 5000
        }

        public enum OrderSource
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("机器")]
            Machine = 1,
            [Remark("接口")]
            Api = 2,
            [Remark("小程序")]
            MiniProgram = 3

        }

        public enum OrderPayWay
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("微信")]
            Wechat = 1,
            [Remark("支付宝")]
            AliPay = 2
        }

        public enum ChannelType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("自助商品")]
            Machine = 1,
            [Remark("快递商品")]
            Express = 2
        }

        public enum ReserveMode
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("线下")]
            OffLine = 1,
            [Remark("线上")]
            Online = 2
        }


        public enum CouponStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("待使用")]
            WaitUse = 1,
            [Remark("已使用")]
            Used = 2,
            [Remark("已过期")]
            Expired = 3,
            [Remark("已删除")]
            Delete = 4,
            [Remark("冻结")]
            Frozen = 5
        }

        public enum CouponSourceType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("领取")]
            Receive = 1,
            [Remark("派送")]
            Give = 2
        }

        public enum CartStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("等待结算")]
            WaitSettle = 1,
            [Remark("结算中")]
            Settling = 2,
            [Remark("结算完成")]
            SettleCompleted = 3,
            [Remark("已删除")]
            Deleted = 4
        }

        public enum CouponType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("满减券")]
            FullCut = 1,
            [Remark("立减券")]
            UnLimitedCut = 2,
            [Remark("折扣券")]
            Discount = 3
        }

        public enum CartOperateType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("选择")]
            Selected = 1,
            [Remark("加")]
            Increase = 2,
            [Remark("减")]
            Decrease = 3,
            [Remark("删除")]
            Delete = 4
        }

        public enum StoreBannerType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("首页Banner")]
            IndexBanner = 1,
        }
    }
}
