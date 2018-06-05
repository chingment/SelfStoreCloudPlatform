using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{

    /// <summary>
    /// 系统的枚举
    /// </summary>
    public partial class Enumeration
    {
        public enum SysSmsSendResult
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("成功")]
            Success = 1,
            [Remark("失败")]
            Failure = 2,
            [Remark("异常")]
            Exception = 2,
        }

        public enum BannerType
        {
            Unknow = 0,
            MainHomeTop = 1
        }

        public enum SysBannerStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("暂存")]
            Save = 1,
            [Remark("已发布")]
            Release = 2,
            [Remark("撤销")]
            Cancle = 3
        }

        public enum AppKeySecretStatus
        {
            Unknow = 0,
            Normal = 1,
            Disable = 2
        }

        public enum ClientAccountType
        {
            Unknow = 0,
            MasterAccount = 1,
            SubAccount = 2
        }

        public enum UserStatus
        {
            Unknow = 0,
            Normal = 1,
            Disable = 2
        }

        public enum UserType
        {
            Unknow = 0,
            Staff = 1,
            Client = 2,
            Agent = 3,
            Salesman = 4,
            Merchant = 5
        }

        /// <summary>
        /// 登录类型
        /// </summary>
        public enum LoginType
        {
            Unknow = 0,
            Website = 1,
            AndroidApp = 2,
            IosApp = 3,
            Wechat = 4
        }

        /// <summary>
        /// 登录结果
        /// </summary>
        public enum LoginResult
        {

            Unknow = 0,
            Success = 1,
            Failure = 2
        }

        public enum LoginResultTip
        {
            Unknow = 0,
            VerifyPass = 1,
            UserNotExist = 2,
            UserPasswordIncorrect = 3,
            UserDisabled = 4,
            UserDeleted = 5,
            UserAccessFailedMaxCount = 6
        }


        /// <summary>
        /// 控件类型
        /// </summary>
        public enum InputType
        {
            CheckBox = 0,
            Hidden = 1,
            Password = 2,
            Radio = 3,
            Text = 4,
            Select = 5
        }


        /// <summary>
        /// 是否有效
        /// </summary>
        public enum Disable
        {
            /// <summary>
            /// 是
            /// </summary>
            Yes = 0,
            /// <summary>
            /// 否
            /// </summary>
            No = 1
        }

        /// <summary>
        /// 是否有效
        /// </summary>
        public enum YesOrNo
        {
            /// <summary>
            /// 是
            /// </summary>
            [Remark("否")]
            No = 0,
            /// <summary>
            /// 否
            /// </summary>
            [Remark("是")]
            Yes = 1
        }


        /// <summary>
        /// 性别
        /// </summary>
        public enum Sex
        {
            /// <summary>
            /// 未知
            /// </summary>
            [Remark("未知")]
            Unknow = 0,
            /// <summary>
            /// 男
            /// </summary>
            [Remark("男")]
            Male = 1,
            /// <summary>
            /// 女
            /// </summary>
            [Remark("女")]
            Female = 2
        }

        /// <summary>
        /// 证件类型
        /// </summary>
        public enum DocumentType
        {
            /// <summary>
            /// 未知
            /// </summary>
            Unknow = 0,
            /// <summary>
            /// 身份证
            /// </summary>
            IdCard = 1,
            /// <summary>
            /// 护照
            /// </summary>
            Passport = 2
        }

        /// <summary>
        /// 操作类型
        /// </summary>
        public enum OperateType
        {
            /// <summary>
            /// 未知
            /// </summary>
            Unknow = 0,
            /// <summary>
            /// 新增
            /// </summary>
            New = 1,
            /// <summary>
            /// 修改
            /// </summary>
            Update = 2,
            /// <summary>
            /// 删除
            /// </summary>
            Delete = 3,
            /// <summary>
            /// 暂存
            /// </summary>
            Save = 4,
            /// <summary>
            /// 确定
            /// </summary>
            Submit = 5,
            /// <summary>
            /// 通过
            /// </summary>
            Pass = 6,
            /// <summary>
            /// 暂存
            /// </summary>
            Reject = 7,
            /// <summary>
            /// 拒绝
            /// </summary>
            Refuse = 8,
            /// <summary>
            /// 取消
            /// </summary>
            Cancle = 9,

            /// <summary>
            /// 查询
            /// </summary>
            Serach = 101,
            /// <summary>
            /// 导出Excel
            /// </summary>
            ExportExcel = 102
        }

        public enum SysItemCacheType
        {
            Unknow = 0,
            User = 1
        }
    }
}
