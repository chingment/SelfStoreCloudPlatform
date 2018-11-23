using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppMobile
{
    public class PersonalService : BaseProvider
    {
        public PersonalPageModel GetPageData(string operater, string clientId, string storeId)
        {
            var pageModel = new PersonalPageModel();

            var user = CurrentDb.SysClientUser.Where(m => m.Id == clientId).FirstOrDefault();
            if (user != null)
            {
                var userInfo = new UserInfoModel();
                userInfo.UserId = user.Id;
                userInfo.NickName = user.Nickname;
                userInfo.PhoneNumber = user.PhoneNumber;
                userInfo.HeadImgUrl = user.HeadImgUrl;
                userInfo.IsVip = user.IsVip;
                pageModel.UserInfo = userInfo;
            }


            return pageModel;
        }
    }
}
