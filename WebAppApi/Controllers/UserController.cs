using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Web.Http;
using Lumos;
using Lumos.BLL.Service.App;
using Lumos.Session;

namespace WebAppApi.Controllers
{
    public class UserController : OwnApiBaseController
    {
        [AllowAnonymous]
        [HttpPost]
        public OwnApiHttpResponse Authorize()
        {
            return null;
        }

        [AllowAnonymous]
        [HttpPost]
        public OwnApiHttpResponse LoginByMinProgram(RopLoginByMinProgram rop)
        {
            OwnApiHttpResult result;

            var userInfo = SdkFactory.Wx.GetUserInfoByMinProramJsCode(this.CurrentAppInfo, rop.EncryptedData, rop.Iv, rop.Code);

            if (userInfo == null)
            {
                result = new OwnApiHttpResult() { Result = ResultType.Failure, Code = ResultCode.Failure, Message = "获取用户信息失败" };
                return new OwnApiHttpResponse(result);
            }

            WxUserInfo wxUserInfo = new WxUserInfo();
            wxUserInfo.OpenId = userInfo.openId;
            wxUserInfo.Nickname = userInfo.nickName;
            wxUserInfo.Sex = userInfo.gender;
            wxUserInfo.Province = userInfo.province;
            wxUserInfo.City = userInfo.city;
            wxUserInfo.Country = userInfo.country;
            wxUserInfo.HeadImgUrl = userInfo.avatarUrl;


            wxUserInfo = BizFactory.WxUser.CheckedUser(GuidUtil.Empty(), wxUserInfo);


            if (wxUserInfo == null)
            {
                result = new OwnApiHttpResult() { Result = ResultType.Failure, Code = ResultCode.Failure, Message = "保存用户失败" };
                return new OwnApiHttpResponse(result);
            }

            var ret = new RetLoginByMinProgram();

            ret.AccessToken = GuidUtil.New();


            SSOUtil.SetUserInfo(ret.AccessToken, new UserInfo { UserId = wxUserInfo.ClientId, UserName = wxUserInfo.Nickname, WxOpenId = wxUserInfo.OpenId }, new TimeSpan(30, 0, 0, 0, 0));

            result = new OwnApiHttpResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "登录成功", Data = ret };

            return new OwnApiHttpResponse(result);

        }


    }
}