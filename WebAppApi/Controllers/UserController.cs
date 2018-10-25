﻿using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using Lumos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lumos.BLL.Service;
using System.Web;
using Lumos;
using Lumos.BLL.Service.App;
using System.Security.Cryptography;
using System.IO;
using System.Text;
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
            //"wxb01e0e16d57bd762", "4acf13ebe601a5b13029bd74bed3de1a"
            OwnApiHttpResult result;
            var userInfo = SdkFactory.Wx.Instance().GetUserInfoByMinProramJsCode("NativeMiniProgram", rop.EncryptedData, rop.Iv, rop.Code);

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