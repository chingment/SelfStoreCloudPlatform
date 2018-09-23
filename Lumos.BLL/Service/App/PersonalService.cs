using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class PersonalService : BaseProvider
    {
        public PersonalPageModel GetPageData(string pOperater, string pUserId, string pStoreId)
        {
            var pageModel = new PersonalPageModel();

            var user = CurrentDb.SysUser.Where(m => m.Id == pUserId).FirstOrDefault();
            if (user != null)
            {
                var userInfo = new UserInfoModel();
                userInfo.UserId = user.Id;
                userInfo.NickName = user.UserName;
                userInfo.Phone = user.PhoneNumber;
                userInfo.HeadImg = "http://thirdwx.qlogo.cn/mmopen/vi_32/6zcicmSoM5yjdWG9MoHydE6suFUGaHsKATFUPU7yU4d7PhLcsKWj51NhxA4PichkuY5uWvbEvXZWBGBpJSd48GNA/132";
                pageModel.UserInfo = userInfo;
            }


            return pageModel;
        }
    }
}
