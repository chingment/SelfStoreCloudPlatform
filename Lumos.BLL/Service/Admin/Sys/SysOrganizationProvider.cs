using Lumos.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.Admin
{
    public class SysOrganizationProvider : BaseProvider
    {
        public CustomJsonResult GetDetails(string operater, string id)
        {
            var ret = new RetSysOrganizationGetDetails();
            var organization = CurrentDb.SysOrganization.Where(m => m.Id == id).FirstOrDefault();

            if (organization == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "操作失败", ret);
            }


            ret.Id = organization.Id ?? "";
            ret.Name = organization.Name ?? "";
            ret.Description = organization.Description ?? "";
            ret.Status = organization.Status;


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }

        public CustomJsonResult Add(string operater, RopSysOrganizationAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var organization = new SysOrganization();
                organization.Id = GuidUtil.New();
                organization.MerchantId = null;
                organization.BelongSite = Enumeration.BelongSite.Admin;
                organization.PId = rop.PId;
                organization.Name = rop.Name;
                organization.Description = rop.Description;
                organization.Status = Enumeration.SysOrganizationStatus.Valid;
                organization.Creator = operater;
                organization.CreateTime = DateTime.Now;
                organization.IsCanDelete = true;
                CurrentDb.SysOrganization.Add(organization);
                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");
            }

            return result;
        }

        public CustomJsonResult Edit(string operater, RopSysOrganizationEdit rop)
        {
            var organization = CurrentDb.SysOrganization.Where(m => m.Id == rop.Id).FirstOrDefault();

            organization.Name = rop.Name;
            organization.Status = rop.Status;
            organization.Description = rop.Description;
            organization.Mender = operater;
            organization.MendTime = DateTime.Now;
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");

        }

        public CustomJsonResult Delete(string operater, string[] ids)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                if (ids == null || ids.Length == 0)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "请选择要删除的数据");
                }


                foreach (var id in ids)
                {
                    var organization = CurrentDb.SysOrganization.Where(m => m.Id == id).FirstOrDefault();
                    if (organization != null)
                    {
                        if (!organization.IsCanDelete)
                        {
                            return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("所选机构（{0}）不允许删除", organization.Name));
                        }

                        organization.IsDelete = true;

                        CurrentDb.SaveChanges();
                    }
                }


                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功"); ;
            }

            return result;
        }

    }
}
