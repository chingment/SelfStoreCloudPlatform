using Lumos.DAL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Lumos.Mvc;
using Newtonsoft.Json;

namespace Lumos.BLL
{
    public class WxUserProvider : BaseProvider
    {
        private static readonly object goSettlelock = new object();

        public WxUserInfo CheckedUser(string pOperater, WxUserInfo pWxUserInfo)
        {
            WxUserInfo mod_UserInfo = null;
            LogUtil.Info(string.Format("开始检测用户信息：{0}", JsonConvert.SerializeObject(pWxUserInfo)));
            lock (goSettlelock)
            {
                try
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        mod_UserInfo = CurrentDb.WxUserInfo.Where(m => m.OpenId == pWxUserInfo.OpenId).FirstOrDefault();
                        if (mod_UserInfo == null)
                        {
                            var sysClientUser = new SysClientUser();
                            sysClientUser.Id = GuidUtil.New();
                            sysClientUser.UserName = string.Format("wx{0}", Guid.NewGuid().ToString().Replace("-", ""));
                            sysClientUser.PasswordHash = PassWordHelper.HashPassword("888888");
                            sysClientUser.SecurityStamp = Guid.NewGuid().ToString();
                            sysClientUser.RegisterTime = this.DateTime;
                            sysClientUser.CreateTime = this.DateTime;
                            sysClientUser.Creator = pOperater;
                            sysClientUser.Type = Enumeration.UserType.Client;
                            sysClientUser.Status = Enumeration.UserStatus.Normal;
                            CurrentDb.SysClientUser.Add(sysClientUser);
                            CurrentDb.SaveChanges();

                            mod_UserInfo = new WxUserInfo();
                            mod_UserInfo.Id = GuidUtil.New();
                            mod_UserInfo.ClientId = sysClientUser.Id;
                            mod_UserInfo.OpenId = pWxUserInfo.OpenId;
                            mod_UserInfo.AccessToken = pWxUserInfo.AccessToken;
                            mod_UserInfo.ExpiresIn = pWxUserInfo.ExpiresIn;
                            if (pWxUserInfo.Nickname != null)
                            {
                                mod_UserInfo.Nickname = pWxUserInfo.Nickname;
                            }
                            if (pWxUserInfo.Sex != null)
                            {
                                mod_UserInfo.Sex = pWxUserInfo.Sex;
                            }

                            if (pWxUserInfo.Province != null)
                            {
                                mod_UserInfo.Province = pWxUserInfo.Province;
                            }

                            if (pWxUserInfo.City != null)
                            {
                                mod_UserInfo.City = pWxUserInfo.City;
                            }

                            if (pWxUserInfo.Country != null)
                            {
                                mod_UserInfo.Country = pWxUserInfo.Country;
                            }

                            if (pWxUserInfo.HeadImgUrl != null)
                            {
                                mod_UserInfo.HeadImgUrl = pWxUserInfo.HeadImgUrl;
                            }

                            if (pWxUserInfo.UnionId != null)
                            {
                                mod_UserInfo.UnionId = pWxUserInfo.UnionId;
                            }
                            mod_UserInfo.CreateTime = this.DateTime;
                            mod_UserInfo.Creator = pOperater;
                            CurrentDb.WxUserInfo.Add(mod_UserInfo);
                            CurrentDb.SaveChanges();

                        }
                        else
                        {
                            mod_UserInfo.AccessToken = pWxUserInfo.AccessToken;
                            mod_UserInfo.ExpiresIn = pWxUserInfo.ExpiresIn;

                            if (pWxUserInfo.Nickname != null)
                            {
                                mod_UserInfo.Nickname = pWxUserInfo.Nickname;
                            }
                            if (pWxUserInfo.Sex != null)
                            {
                                mod_UserInfo.Sex = pWxUserInfo.Sex;
                            }

                            if (pWxUserInfo.Province != null)
                            {
                                mod_UserInfo.Province = pWxUserInfo.Province;
                            }

                            if (pWxUserInfo.City != null)
                            {
                                mod_UserInfo.City = pWxUserInfo.City;
                            }

                            if (pWxUserInfo.Country != null)
                            {
                                mod_UserInfo.Country = pWxUserInfo.Country;
                            }

                            if (pWxUserInfo.HeadImgUrl != null)
                            {
                                mod_UserInfo.HeadImgUrl = pWxUserInfo.HeadImgUrl;
                            }

                            if (pWxUserInfo.UnionId != null)
                            {
                                mod_UserInfo.UnionId = pWxUserInfo.UnionId;
                            }
                            mod_UserInfo.MendTime = this.DateTime;
                            mod_UserInfo.Mender = pOperater;
                        }



                        CurrentDb.SaveChanges();

                        ts.Complete();

                    }
                    LogUtil.Info(string.Format("结束检测用户信息：{0}", pWxUserInfo.OpenId));
                }
                catch (Exception ex)
                {
                    mod_UserInfo = null;
                    LogUtil.Error("检查微信用户系统发生异常", ex);
                }
            }

            return mod_UserInfo;
        }
    }
}
