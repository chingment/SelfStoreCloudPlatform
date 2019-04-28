using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Web.Http;
using Lumos;
using Lumos.BLL.Service.ApiApp;
using Lumos.Session;
using Lumos.BLL.Biz;
using Lumos.BLL.Service.Merch;

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

            var userInfo = SdkFactory.Wx.GetUserInfoByMinProramJsCode(this.CurrentWxMpAppInfo, rop.EncryptedData, rop.Iv, rop.Code);

            if (userInfo == null)
            {
                result = new OwnApiHttpResult() { Result = ResultType.Failure, Code = ResultCode.Failure, Message = "获取用户信息失败" };
                return new OwnApiHttpResponse(result);
            }

            RopWxUserCheckedUser ropWxCheckedUser = new RopWxUserCheckedUser();
            ropWxCheckedUser.OpenId = userInfo.openId;
            ropWxCheckedUser.Nickname = userInfo.nickName;
            ropWxCheckedUser.Sex = userInfo.gender;
            ropWxCheckedUser.Province = userInfo.province;
            ropWxCheckedUser.City = userInfo.city;
            ropWxCheckedUser.Country = userInfo.country;
            ropWxCheckedUser.HeadImgUrl = userInfo.avatarUrl;


            var retWxCheckedUser = BizFactory.WxUser.CheckedUser(GuidUtil.Empty(), ropWxCheckedUser);


            if (retWxCheckedUser == null)
            {
                result = new OwnApiHttpResult() { Result = ResultType.Failure, Code = ResultCode.Failure, Message = "保存用户失败" };
                return new OwnApiHttpResponse(result);
            }

            var ret = new RetLoginByMinProgram();
            
            ret.AccessToken = GuidUtil.New();

            SSOUtil.SetUserInfo(ret.AccessToken, new UserInfo { UserId = retWxCheckedUser.ClientUserId, UserName = retWxCheckedUser.Nickname, WxOpenId = retWxCheckedUser.OpenId }, new TimeSpan(30, 0, 0, 0, 0));

            result = new OwnApiHttpResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "登录成功", Data = ret };

            return new OwnApiHttpResponse(result);

        }


    }
}