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

        public enum MerchantPosMachineStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("正常")]
            Normal = 1,
            [Remark("未激活")]
            NoActive = 2,
            [Remark("到期")]
            Expiry = 3,
            [Remark("已解绑")]
            Unbind = 4
        }

        public enum ExtendedAppType
        {
            Unknow = 0,
            HaoYiLianService = 1,
            ThirdPartyApp = 2,
            CarInsService = 3
        }

        public enum ExtendedAppStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("正常")]
            Normal = 1,
            [Remark("停用")]
            Disable = 2
        }

        public enum CarInsureAuditStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("提交订单")]
            Sumbit = 1,
            [Remark("等待报价")]
            WaitOffer = 2,
            [Remark("报价中")]
            InOffer = 3,
            [Remark("报价完成")]
            OfferComplete = 4,
            [Remark("客户跟进")]
            ClientFllow = 5,
            [Remark("报价时取消订单")]
            CancleOffer = 6,
            [Remark("等待核保")]
            WaitCheckInsure = 7,
            [Remark("核保中")]
            InCheckInsure = 8,
            [Remark("核保完成")]
            CheckInsureComplete = 9
        }

        //public enum CarInsureAuditStep
        //{
        //    [Remark("未知")]
        //    Unknow = 0,
        //    [Remark("提交")]
        //    Submit = 1,
        //    [Remark("报价")]
        //    Offer = 2,
        //    [Remark("跟进")]
        //    Fllow = 3,
        //    [Remark("完成")]
        //    Complete = 4,
        //    [Remark("取消")]
        //    Cancle = 5
        //}

        //public enum CarInsureOfferAutoDealtStatus
        //{
        //    [Remark("未知")]
        //    Unknow = 0,
        //    [Remark("提交基础数据")]
        //    WaitOffer = 1,
        //    [Remark("等待自动报价")]
        //    InOffer = 2,
        //    [Remark("自动报价成功")]
        //    OfferComplete = 3,
        //    [Remark("自动报价失败")]
        //    ClientFllow = 3,
        //    [Remark("等待人工报价")]
        //    StaffCancle = 4,
        //    [Remark("人工报价成功")]
        //    ClientCancle = 5,
        //    [Remark("人工报价失败")]
        //    Complete = 6
        //}

        public enum CarClaimDealtStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("等待核实")]
            WaitVerifyOrder = 1,
            [Remark("核实需求中")]
            InVerifyOrder = 2,
            [Remark("跟进待上传定损单")]
            FllowUploadEstimateListImg = 3,
            [Remark("等待核实金额")]
            WaitVerifyAmount = 4,
            [Remark("核实金额中")]
            InVerifyAmount = 5,
            [Remark("后台取消订单")]
            StaffCancle = 6,
            [Remark("客户取消订单")]
            ClientCancle = 7,
            [Remark("完成")]
            Complete = 8
        }

        public enum CarClaimDealtStep
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("提交订单")]
            Submit = 1,
            [Remark("核实需求")]
            VerifyOrder = 2,
            [Remark("上传定损单")]
            UploadEstimateListImg = 3,
            [Remark("核实金额")]
            VerifyAmount = 4,
            [Remark("支付完成")]
            Complete = 5

        }

        public enum AuditFlowV1Status
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("提交订单")]
            Submit = 1,
            [Remark("等待核实")]
            WaitVerify = 2,
            [Remark("核实中")]
            InVerify = 3,
            [Remark("核实正确")]
            VerifyCorrect = 5,
            [Remark("核实不正确")]
            VerifyIncorrect = 6,
            [Remark("等待进入待处理")]
            waitGoDealt = 7,
            [Remark("等待处理")]
            WaitDealt = 8,
            [Remark("处理中")]
            InDealt = 9,
            [Remark("处理驳回")]
            DealtReject = 10,
            [Remark("处理成功")]
            DealtSuccess = 11,
            [Remark("处理失败")]
            DealtFailure = 12
        }


        public enum AuditFlowV1Step
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("提交订单")]
            Submit = 1,
            [Remark("核实中")]
            InVerify = 2,
            [Remark("核实完成")]
            VerifyedComplete = 3,
            [Remark("处理驳回")]
            VerifyedReject = 4,
            [Remark("处理订单中")]
            InDealt = 5,
            [Remark("处理完成")]
            DealtedComplete = 6,
            [Remark("处理驳回")]
            DealtedReject = 7
        }


        public enum MerchantAuditStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("提交订单")]
            Submit = 1,
            [Remark("等待初审")]
            WaitPrimaryAudit = 2,
            [Remark("初审中")]
            InPrimaryAudit = 3,
            [Remark("等待复审")]
            WaitSeniorAudit = 4,
            [Remark("复审中")]
            InSeniorAudit = 5,
            [Remark("复审通过")]
            SeniorAuditPass = 6,
            [Remark("复审驳回")]
            SeniorAuditReject = 7
        }


        public enum MerchantAuditStep
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("提交")]
            Submit = 1,
            [Remark("初审")]
            PrimaryAudit = 2,
            [Remark("复审")]
            SeniorAudit = 3
        }


        public enum CarInsurePlanStatus
        {
            Unknow = 0,
            CarService = 1
        }

        public enum CarKindInputType
        {
            Unknow = 0,
            None = 1,
            Text = 2,
            DropDownList = 3
        }
        public enum CarKindType
        {
            Unknow = 0,
            Compulsory = 1,
            Commercial = 2,
            AdditionalRisk = 3
        }

        public enum ProductType
        {
            Unknow = 0,
            [Remark("普通商品")]
            Goods = 1,
            [Remark("保险产品")]
            Insure = 2,
            [Remark("意外险")]
            InsureForYiWaiXian = 200001
        }


        public enum OrderType
        {
            Unknow = 0,
            [Remark("商品")]
            Goods = 1,
            [Remark("保险")]
            Insure = 2,
            [Remark("业务")]
            Biz = 3,
            [Remark("车险投保")]
            InsureForCarForInsure = 300001,
            [Remark("车险理赔")]
            InsureForCarForClaim = 300002,
            [Remark("POS机服务费")]
            PosMachineServiceFee = 300003,
            [Remark("人才输送")]
            TalentDemand = 300004,
            [Remark("申请定损点")]
            ApplyLossAssess = 300005,
            [Remark("违章查询积分充值")]
            LllegalQueryRecharge = 300006,
            [Remark("违章处理")]
            LllegalDealt = 300007,
            [Remark("贷款申请")]
            Credit = 300008

        }

        public enum ProductStatus
        {
            Unknow = 0,
            [Remark("上架")]
            OnLine = 1,
            [Remark("下架")]
            OffLine = 2
        }

        public enum DealtStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("待处理")]
            Wait = 1,
            [Remark("处理中")]
            Handle = 2,
            [Remark("已处理")]
            Complete = 3
        }


        public enum OrderToCarClaimDealtStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("定损中")]
            Wait = 1,
            [Remark("复核中")]
            Handle = 2,
            [Remark("待支付")]
            Complete = 3
        }

        public enum CarInsuranceClaimResult
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("派单")]
            Dispatch = 1,
            [Remark("撤销")]
            Cancel = 2,
        }

        public enum OrderStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("已提交")]
            Submitted = 1,
            [Remark("跟进中")]
            Follow = 2,
            [Remark("待支付")]
            WaitPay = 3,
            [Remark("已完成")]
            Completed = 4,
            [Remark("已取消")]
            Cancled = 5
        }

        public enum OrderChildDetailsStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("待支付")]
            WaitPay = 1,
            //[Remark("已支付")]
            //Payed = 2,
            [Remark("正在生成取货码")]
            PickCodeBuilding = 3,
            //[Remark("生成取货码失败")]
            //PickCodeBuildFailure = 4,
            [Remark("待提货")]
            WaitTake = 5,
            //[Remark("退款处理中")]
            //ReFunding = 6,
            //[Remark("已退款")]
            //ReFunded = 7,
            [Remark("已取货")]
            Picked = 8,
            [Remark("已取消")]
            Cancled = 9,
            [Remark("待发货")]
            WaitSend = 10,
            [Remark("已发货")]
            Sent = 11,
            [Remark("已签收")]
            Received = 12,
            [Remark("取货失败")]
            PickedFailure = 13
        }


        public enum OrderToCarInsureFollowStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("待提交")]
            WaitSubmit = 1,
            [Remark("已提交")]
            Submitted = 2
        }

        public enum OrderToCarClaimFollowStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("定损中")]
            WaitEstimate = 1,
            [Remark("待上传定损单")]
            WaitUploadEstimateList = 2,
            [Remark("等待核实定损金额")]
            VerifyEstimateAmount = 3,
            [Remark("等待支付佣金")]
            WaitPayCommission = 6,
            [Remark("支付完成")]
            PayedCommission = 7
        }


        public enum OrderPayWay
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("POS")]
            POS = 1,
            [Remark("微信")]
            Wechat = 2,
            [Remark("支付宝")]
            Alipay = 3,
            [Remark("现金")]
            Cash = 4,
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

        public enum MerchantStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("待完善资料")]
            WaitFill = 1,
            [Remark("完善资料中")]
            InFill = 2,
            [Remark("已完善")]
            Filled = 3
        }

        public enum MerchantType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("汽车用品")]
            CarGoods = 1,
            [Remark("维修厂")]
            CarRepair = 2,
            [Remark("美容快保")]
            CarBeauty = 3,
            [Remark("其他")]
            Else = 4
        }

        public enum HandMerchantType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("需求方")]
            Demand = 1,
            [Remark("维修方")]
            Supply = 2,
        }


        public enum RepairCapacity
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("无维修能力")]
            NoRepair = 1,
            [Remark("无定损权但有维修能力")]
            NoEstimateButRepair = 2,
            [Remark("有定损权也有维修能力")]
            EstimateAndRepair = 3,
        }

        public enum RepairsType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("维修和定损")]
            EstimateRepair = 1,
            [Remark("只定损")]
            Estimate = 2
        }

        public enum TransactionsType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("服务费")]
            ServiceFee = 1,
            [Remark("违章积分充值")]
            LllegalQueryRecharg = 2,
            [Remark("违章处理")]
            LllegalDealt = 3
        }

        public enum LllegalQueryScoreTransType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("初始增加积分")]
            IncreaseByInit = 1,
            [Remark("充值增加积分")]
            IncreaseByRecharge = 2,
            [Remark("查询扣除积分")]
            DecreaseByQuery = 3
        }

        public enum CarUserCharacter
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("家庭自用汽车")]
            JTZYQC = 1,
            [Remark("非营业企业客车")]
            FYYQYKC = 2,
            [Remark("非营业机关事业团体客车")]
            FYYJGSYTTKC = 3,
            [Remark("非营业货车")]
            FYYHC = 5,
            [Remark("非营业挂车")]
            FYYGC = 6,
            [Remark("兼用型拖拉机")]
            JYXTTLJ = 7,
            [Remark("其他非营业车辆")]
            QTFYYCL = 8
        }

        public enum CarVechicheType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("客车")]
            KC = 1,
            [Remark("货车")]
            HC = 2,
            [Remark("特种车")]
            TZC = 3,
            [Remark("拖拉机")]
            TLJ = 4
        }

        public enum CarSeat
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("六座以下客车")]
            S6 = 1,
            [Remark("六座至十座以下客车")]
            S10 = 2,
            [Remark("十座至十二座以下客车")]
            S12 = 3,
            [Remark("二十座至三十座以下客车")]
            S30 = 4,
            [Remark("三十座及三十六座以上客车")]
            S36 = 5
        }

        public enum CarInsuranceCompanyStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("正常")]
            Normal = 2,
            [Remark("停用")]
            Disable = 3
        }

        public enum OpenIdType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("车管家")]
            Cgj = 1
        }

        public enum WorkJob
        {
            [Remark("未知")]
            Unknow = 0,
            //[Remark("储备店长")]
            //储备店长 = 1,
            //[Remark("机修")]
            //机修 = 2,
            //[Remark("钣喷")]
            //钣喷 = 3,
            //[Remark("美容")]
            //美容 = 4,
            //[Remark("销售顾问")]
            //销售顾问 = 5,
            //[Remark("电销专员")]
            //电销专员 = 6,
            //[Remark("前台接待")]
            //前台接待 = 7,
            //[Remark("理赔专员")]
            //理赔专员 = 8,
            //[Remark("配件销售")]
            //配件销售 = 9,
            //[Remark("仓管员")]
            //仓管员 = 10,
            //[Remark("财务出纳")]
            //财务出纳 = 11,
            //[Remark("行政人事专员")]
            //行政人事专员 = 12,
            [Remark("周末兼职")]
            周末兼职 = 13,
            [Remark("实习生派遣")]
            实习生派遣 = 14
        }

        public enum PayResultNotifyType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("合作方支付接口主动通知")]
            PartnerPayOrgNotifyUrl = 1,
            [Remark("定时任务查询合作方支付接口")]
            PartnerPayOrgOrderQueryApi = 2,
            [Remark("后台确认")]
            Staff = 3,
            [Remark("App主动通知")]
            AppNotify = 4
        }

        public enum SysItemCacheType
        {
            Unknow = 0,
            User = 1,
            Banner = 2,
            CarInsCompanys = 3,
            CarKinds = 4,
            TalentDemandWorkJob = 5,
            ExtendedApp = 6,
        }


        public enum OrderToLllegalDealtDetailsStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("待支付")]
            WaitPay = 1,
            [Remark("处理中")]
            InDealt = 2,
            [Remark("已完成")]
            Completed = 3
        }

        public enum ProductCategoryStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("正常")]
            Valid = 1,
            [Remark("停用")]
            Invalid = 2
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

        public enum CompanyStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("正常")]
            Valid = 1,
            [Remark("停用")]
            Invalid = 2
        }

        public enum CompanyType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("保险公司")]
            InsuranceCompany = 1,
            [Remark("供应商")]
            Supplier = 2
        }

        public enum InventoryOperateType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("增加")]
            Increase = 1,
            [Remark("减少")]
            Decrease = 2
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

        public enum ChannelType
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("快递商品")]
            Express = 1,
            [Remark("自助商品")]
            SelfPick = 2
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

        public enum ReceptionMode
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("机器自取")]
            Machine = 1,
            [Remark("快递专送")]
            Express = 2
        }

        public enum OfferResult
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("等待自动报价")]
            WaitAutoOffer = 1,
            [Remark("自动报价成功")]
            AutoOfferSuccess = 2,
            [Remark("自动报价失败")]
            AutoOfferFailure = 3,
            [Remark("等待人工报价")]
            WaitArtificialOffer = 4,
            [Remark("人工报价成功")]
            ArtificialOfferSuccess = 5,
            [Remark("人工报价失败")]
            ArtificialOfferFailure = 6,
            [Remark("等待人工报价")]
            WaitStaffOffer = 7,
            [Remark("人工报价成功")]
            StaffOfferSuccess = 8,
            [Remark("人工报价失败")]
            StaffOfferFailure = 9
        }
    }
}
