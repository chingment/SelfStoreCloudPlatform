
using System;

namespace Lumos.Entity
{

    /// <summary>
    /// 系统的枚举
    /// </summary>
    public partial class Enumeration
    {
        public enum SmsSendResult
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

        public enum AppType
        {
            Unknow = 0,
            NativeApp = 1,
            PublicNumber = 2,
            MinProgram = 3
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

        public enum LoginType
        {
            Unknow = 0,
            Website = 1,
            AndroidApp = 2,
            IosApp = 3,
            Wechat = 4
        }

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

        public enum InputType
        {
            CheckBox = 0,
            Hidden = 1,
            Password = 2,
            Radio = 3,
            Text = 4,
            Select = 5
        }

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

        public enum BackgroundJobStatus
        {
            [Remark("未知")]
            Unknow = 0,
            [Remark("停止")]
            Stoped = 1,
            [Remark("运行中")]
            Runing = 2,
            [Remark("启动中")]
            Starting = 3,
            [Remark("停止中")]
            Stoping = 4
        }
    }
}
