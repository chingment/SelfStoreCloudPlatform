using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.Admin
{
    public class SysPositionProvider : BaseProvider
    {
        public CustomJsonResult GetDetails(string pOperater, string pPositionId)
        {
            var ret = new RetSysPositionGetDetails();
            var position = CurrentDb.SysPosition.Where(m => m.Id == pPositionId).FirstOrDefault();
            if (position != null)
            {
                var roleIds = CurrentDb.SysPositionRole.Where(x => x.PositionId == pPositionId).Select(x => x.RoleId).ToArray();

                ret.Name = position.Name ?? "";

                ret.RoleIds = roleIds;
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }

        public CustomJsonResult Add(string pOperater, RopSysPositionAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var position = new SysPosition();
                position.Id = GuidUtil.New();
                position.OrganizationId = rop.OrganizationId;
                position.Name = rop.Name;
                position.Creator = pOperater;
                position.CreateTime = DateTime.Now;
                position.IsCanDelete = true;
                CurrentDb.SysPosition.Add(position);

                if (rop.RoleIds != null)
                {
                    if (rop.RoleIds.Length > 0)
                    {
                        foreach (string roleId in rop.RoleIds)
                        {
                            CurrentDb.SysPositionRole.Add(new SysPositionRole { Id = GuidUtil.New(), PositionId = position.Id, RoleId = roleId, Creator = pOperater, CreateTime = DateTime.Now });
                        }
                    }
                }


                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");
            }

            return result;
        }

        public CustomJsonResult Edit(string pOperater, RopSysPositionEdit rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var sysPosition = CurrentDb.SysPosition.Where(m => m.Id == rop.PositionId).FirstOrDefault();

                sysPosition.Name = rop.Name;
                sysPosition.MendTime = DateTime.Now;
                sysPosition.Mender = pOperater;
                CurrentDb.SaveChanges();


                var sysPositionRoles = CurrentDb.SysPositionRole.Where(m => m.PositionId == rop.PositionId).ToList();

                foreach (var sysPositionRole in sysPositionRoles)
                {

                    CurrentDb.SysPositionRole.Remove(sysPositionRole);
                }

                if (rop.RoleIds != null)
                {
                    if (rop.RoleIds.Length > 0)
                    {
                        foreach (string roleId in rop.RoleIds)
                        {
                            CurrentDb.SysPositionRole.Add(new SysPositionRole { Id = GuidUtil.New(), PositionId = sysPosition.Id, RoleId = roleId, Creator = pOperater, CreateTime = DateTime.Now });
                        }
                    }
                }

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");

            }
            return result;

        }

        public CustomJsonResult Delete(string pOperater, string[] pPositionIds)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                if (pPositionIds == null || pPositionIds.Length == 0)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "请选择要删除的数据");
                }


                foreach (var id in pPositionIds)
                {
                    var sysPosition = CurrentDb.SysPosition.Where(m => m.Id == id).FirstOrDefault();
                    if (sysPosition != null)
                    {
                        if (!sysPosition.IsCanDelete)
                        {
                            return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("所选机构（{0}）不允许删除", sysPosition.Name));
                        }

                        sysPosition.IsDelete = true;

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
