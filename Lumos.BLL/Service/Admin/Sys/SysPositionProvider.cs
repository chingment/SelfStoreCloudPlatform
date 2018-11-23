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
        public CustomJsonResult GetDetails(string operater, string id)
        {
            var ret = new RetSysPositionGetDetails();
            var sysPosition = CurrentDb.SysPosition.Where(m => m.Id == id).FirstOrDefault();
            if (sysPosition != null)
            {
                var sysRoleIds = CurrentDb.SysPositionRole.Where(x => x.PositionId == id).Select(x => x.RoleId).ToArray();

                ret.Id = sysPosition.Id ?? "";
                ret.Name = sysPosition.Name ?? "";
                ret.RoleIds = sysRoleIds;
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }

        public CustomJsonResult Add(string operater, RopSysPositionAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var position = new SysPosition();
                position.Id = GuidUtil.New();
                position.OrganizationId = rop.OrganizationId;
                position.Name = rop.Name;
                position.Creator = operater;
                position.CreateTime = DateTime.Now;
                position.IsCanDelete = true;
                CurrentDb.SysPosition.Add(position);

                if (rop.RoleIds != null)
                {
                    if (rop.RoleIds.Length > 0)
                    {
                        foreach (string roleId in rop.RoleIds)
                        {
                            CurrentDb.SysPositionRole.Add(new SysPositionRole { Id = GuidUtil.New(), PositionId = position.Id, RoleId = roleId, Creator = operater, CreateTime = DateTime.Now });
                        }
                    }
                }


                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
            }

            return result;
        }

        public CustomJsonResult Edit(string operater, RopSysPositionEdit rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var sysPosition = CurrentDb.SysPosition.Where(m => m.Id == rop.Id).FirstOrDefault();

                sysPosition.Name = rop.Name;
                sysPosition.MendTime = DateTime.Now;
                sysPosition.Mender = operater;
                CurrentDb.SaveChanges();


                var sysPositionRoles = CurrentDb.SysPositionRole.Where(m => m.PositionId == rop.Id).ToList();

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
                            CurrentDb.SysPositionRole.Add(new SysPositionRole { Id = GuidUtil.New(), PositionId = sysPosition.Id, RoleId = roleId, Creator = operater, CreateTime = DateTime.Now });
                        }
                    }
                }

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");

            }
            return result;

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
                    var sysPosition = CurrentDb.SysPosition.Where(m => m.Id == id).FirstOrDefault();
                    if (sysPosition != null)
                    {
                        if (!sysPosition.IsCanDelete)
                        {
                            return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("所选机构（{0}）不允许删除", sysPosition.Name));
                        }

                        sysPosition.IsDelete = true;
                        sysPosition.Mender = operater;
                        sysPosition.MendTime = this.DateTime;
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
