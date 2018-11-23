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
        public CustomJsonResult GetDetails(string pOperater, string pOrganizationId)
        {
            var ret = new RetSysOrganizationGetDetails();
            var organization = CurrentDb.SysOrganization.Where(m => m.Id == pOrganizationId).FirstOrDefault();
            if (organization != null)
            {
                ret.OrganizationId = organization.Id ?? "";
                ret.Name = organization.Name ?? "";
                ret.Description = organization.Description ?? "";
                ret.Status = organization.Status;
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }


        public CustomJsonResult Add(string pOperater, RopSysOrganizationAdd rop)
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
                organization.Creator = pOperater;
                organization.CreateTime = DateTime.Now;
                organization.IsCanDelete = true;
                CurrentDb.SysOrganization.Add(organization);
                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");
            }

            return result;
        }

        public CustomJsonResult Edit(string pOperater, RopSysOrganizationEdit rop)
        {
            var organization = CurrentDb.SysOrganization.Where(m => m.Id == rop.OrganizationId).FirstOrDefault();

            organization.Name = rop.Name;
            organization.Status = rop.Status;
            organization.Description = rop.Description;
            organization.Mender = pOperater;
            organization.MendTime = DateTime.Now;
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");

        }

        public CustomJsonResult Delete(string pOperater, string[] pOrganizationIds)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                if (pOrganizationIds == null || pOrganizationIds.Length == 0)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "请选择要删除的数据");
                }


                foreach (var id in pOrganizationIds)
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

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "删除成功"); ;
            }

            return result;
        }

    }
}
