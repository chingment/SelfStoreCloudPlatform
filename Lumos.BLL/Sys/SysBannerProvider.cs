using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL
{
    public class SysBannerProvider : BaseProvider
    {
        public string GetLinkUrl(int id)
        {
            string linkUrl = "";

            //linkUrl = string.Format("{0}/Resource/BannerDetails/{1}", BizFactory.AppSettings.WebAppServerUrl, id);

            return linkUrl;
        }


        public CustomJsonResult Add(int operater, Enumeration.OperateType operate, SysBanner sysBanner)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                sysBanner.CreateTime = this.DateTime;
                sysBanner.Creator = operater;


                switch (operate)
                {
                    case Enumeration.OperateType.Save:

                        sysBanner.Status = Enumeration.SysBannerStatus.Save;

                        result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功", sysBanner);
                        break;
                    case Enumeration.OperateType.Submit:

                        sysBanner.Status = Enumeration.SysBannerStatus.Release;

                        result = new CustomJsonResult(ResultType.Success, "发布成功");

                        break;
                }


                CurrentDb.SysBanner.Add(sysBanner);
                CurrentDb.SaveChanges();

                //SysFactory.SysItemCacheUpdateTime.Update(Enumeration.SysItemCacheType.Banner);

                ts.Complete();
            }

            return result;
        }

        public CustomJsonResult Edit(int operater, Enumeration.OperateType operate, SysBanner pSysBanner)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var sysBanner = CurrentDb.SysBanner.Where(m => m.Id == pSysBanner.Id).FirstOrDefault();



                switch (operate)
                {
                    case Enumeration.OperateType.Save:
                        sysBanner.Title = pSysBanner.Title;
                        sysBanner.ImgUrl = pSysBanner.ImgUrl;
                        sysBanner.Content = pSysBanner.Content;
                        sysBanner.Source = pSysBanner.Source;
                        sysBanner.LastUpdateTime = this.DateTime;
                        sysBanner.Mender = operater;
                        if (sysBanner.Status != Enumeration.SysBannerStatus.Release)
                        {
                            sysBanner.Status = Enumeration.SysBannerStatus.Save;
                        }

                        result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功", sysBanner);
                        break;
                    case Enumeration.OperateType.Cancle:

                        sysBanner.Status = Enumeration.SysBannerStatus.Cancle;

                        result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "撤销成功", sysBanner);
                        break;
                    case Enumeration.OperateType.Submit:
                        sysBanner.Title = pSysBanner.Title;
                        sysBanner.ImgUrl = pSysBanner.ImgUrl;
                        sysBanner.Content = pSysBanner.Content;
                        sysBanner.Source = pSysBanner.Source;
                        sysBanner.LastUpdateTime = this.DateTime;
                        sysBanner.Mender = operater;
                        sysBanner.Status = Enumeration.SysBannerStatus.Release;

                        result = new CustomJsonResult(ResultType.Success, "发布成功");

                        break;
                }


                CurrentDb.SaveChanges();

                //SysFactory.SysItemCacheUpdateTime.Update(Enumeration.SysItemCacheType.Banner);

                ts.Complete();
            }

            return result;
        }
    }
}
