using Lumos.BLL.Biz;
using Lumos.DAL;
using Lumos.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Biz
{


    public class WxUserProvider : BaseProvider
    {
        private static readonly object goSettlelock = new object();

        public RetWxUserCheckedUser CheckedUser(string pOperater, RopWxUserCheckedUser rop)
        {
            RetWxUserCheckedUser ret = null;
            LogUtil.Info(string.Format("开始检测用户信息：{0}", JsonConvert.SerializeObject(rop)));
            lock (goSettlelock)
            {
                try
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        var wxUserInfo = CurrentDb.WxUserInfo.Where(m => m.OpenId == rop.OpenId).FirstOrDefault();
                        if (wxUserInfo == null)
                        {
                            var sysClientUser = new SysClientUser();
                            sysClientUser.Id = GuidUtil.New();
                            sysClientUser.UserName = string.Format("wx{0}", Guid.NewGuid().ToString().Replace("-", ""));
                            sysClientUser.PasswordHash = PassWordHelper.HashPassword("888888");
                            sysClientUser.SecurityStamp = Guid.NewGuid().ToString();
                            sysClientUser.RegisterTime = this.DateTime;
                            sysClientUser.CreateTime = this.DateTime;
                            sysClientUser.Creator = pOperater;
                            sysClientUser.BelongSite = Enumeration.BelongSite.Client;
                            sysClientUser.Status = Enumeration.UserStatus.Normal;
                            CurrentDb.SysClientUser.Add(sysClientUser);
                            CurrentDb.SaveChanges();

                            wxUserInfo = new WxUserInfo();
                            wxUserInfo.Id = GuidUtil.New();
                            wxUserInfo.ClientId = sysClientUser.Id;
                            wxUserInfo.OpenId = rop.OpenId;
                            wxUserInfo.AccessToken = rop.AccessToken;
                            wxUserInfo.ExpiresIn = rop.ExpiresIn;

                            if (rop.Nickname != null)
                            {
                                sysClientUser.Nickname = rop.Nickname;
                            }
                            if (rop.Sex != null)
                            {
                                sysClientUser.Sex = rop.Sex;
                            }

                            if (rop.Province != null)
                            {
                                sysClientUser.Province = rop.Province;
                            }

                            if (rop.City != null)
                            {
                                sysClientUser.City = rop.City;
                            }

                            if (rop.Country != null)
                            {
                                sysClientUser.Country = rop.Country;
                            }

                            if (rop.HeadImgUrl != null)
                            {
                                sysClientUser.HeadImgUrl = rop.HeadImgUrl;
                            }

                            if (rop.UnionId != null)
                            {
                                wxUserInfo.UnionId = rop.UnionId;
                            }
                            wxUserInfo.CreateTime = this.DateTime;
                            wxUserInfo.Creator = pOperater;
                            CurrentDb.WxUserInfo.Add(wxUserInfo);
                            CurrentDb.SaveChanges();

                        }
                        else
                        {
                            var sysClientUser = CurrentDb.SysClientUser.Where(m => m.Id == wxUserInfo.ClientId).FirstOrDefault();

                            wxUserInfo.AccessToken = rop.AccessToken;
                            wxUserInfo.ExpiresIn = rop.ExpiresIn;

                            if (rop.Nickname != null)
                            {
                                sysClientUser.Nickname = rop.Nickname;
                            }
                            if (rop.Sex != null)
                            {
                                sysClientUser.Sex = rop.Sex;
                            }

                            if (rop.Province != null)
                            {
                                sysClientUser.Province = rop.Province;
                            }

                            if (rop.City != null)
                            {
                                sysClientUser.City = rop.City;
                            }

                            if (rop.Country != null)
                            {
                                sysClientUser.Country = rop.Country;
                            }

                            if (rop.HeadImgUrl != null)
                            {
                                sysClientUser.HeadImgUrl = rop.HeadImgUrl;
                            }

                            if (rop.UnionId != null)
                            {
                                wxUserInfo.UnionId = rop.UnionId;
                            }
                            wxUserInfo.MendTime = this.DateTime;
                            wxUserInfo.Mender = pOperater;
                        }

                        CurrentDb.SaveChanges();
                        ts.Complete();

                        ret = new RetWxUserCheckedUser();
                        ret.OpenId = rop.OpenId;
                        ret.UnionId = rop.UnionId;
                        ret.AccessToken = rop.AccessToken;
                        ret.ExpiresIn = rop.ExpiresIn;
                        ret.PhoneNumber = rop.PhoneNumber;
                        ret.HeadImgUrl = rop.HeadImgUrl;
                        ret.Nickname = rop.Nickname;
                        ret.Sex = rop.Sex;
                        ret.Province = rop.Province;
                        ret.City = rop.City;
                        ret.Country = rop.Country;
                        ret.ClientId = wxUserInfo.ClientId;


                    }
                    LogUtil.Info(string.Format("结束检测用户信息：{0}", rop.OpenId));
                }
                catch (Exception ex)
                {
                    ret = null;
                    LogUtil.Error("检查微信用户系统发生异常", ex);
                }
            }

            return ret;
        }
    }
}
