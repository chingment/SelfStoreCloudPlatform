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
        public CustomJsonResult GetDetails(string operater, Enumeration.SysPositionId id)
        {
            var ret = new RetSysPositionGetDetails();
            var sysPosition = CurrentDb.SysPosition.Where(m => m.Id == id).FirstOrDefault();
            if (sysPosition != null)
            {
                var roleIds = CurrentDb.SysPositionRole.Where(x => x.PositionId == id).Select(x => x.RoleId).ToArray();

                ret.Id = sysPosition.Id;
                ret.Name = sysPosition.Name ?? ""; ;
                ret.RoleIds = roleIds;

            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }

        public CustomJsonResult Add(string operater, RopSysPositionAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();
            var isExist = CurrentDb.SysPosition.Where(m => m.Name == rop.Name).FirstOrDefault();
            if (isExist != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("该名称（{0}）已被使用", rop.Name));
            }

            using (TransactionScope ts = new TransactionScope())
            {
                var sysPosition = new SysPosition();
                //sysPosition.Id = GuidUtil.New();
                sysPosition.Name = rop.Name;
                sysPosition.Description = rop.Description;
                sysPosition.Creator = operater;
                sysPosition.CreateTime = DateTime.Now;
                sysPosition.IsCanDelete = true;


                CurrentDb.SysPosition.Add(sysPosition);


                CurrentDb.SaveChanges();

                List<SysPositionRole> sysPositionRoles = CurrentDb.SysPositionRole.Where(m => m.PositionId == sysPosition.Id).ToList();
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
                            CurrentDb.SysPositionRole.Add(new SysPositionRole { Id = GuidUtil.New(), PositionId = sysPosition.Id, RoleId = roleId, Creator = operater, CreateTime = DateTime.Now, IsCanDelete = true });
                        }
                    }
                }

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");

                CurrentDb.SaveChanges();
                ts.Complete();
            }

            return result;
        }

        public CustomJsonResult Edit(string operater, RopSysPositionEdit rop)
        {

            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var sysPosition = CurrentDb.SysPosition.Where(m => m.Id == rop.Id).FirstOrDefault();
                sysPosition.Description = rop.Description;
                sysPosition.MendTime = DateTime.Now;
                sysPosition.Mender = operater;
                CurrentDb.SaveChanges();


                List<SysPositionRole> sysPositionRoles = CurrentDb.SysPositionRole.Where(m => m.PositionId == rop.Id).ToList();

                foreach (var sysPositionRole in sysPositionRoles)
                {
                    if (!sysPositionRole.IsCanDelete)
                    {
                        var role = CurrentDb.SysRole.Where(m => m.Id == sysPositionRole.Id).FirstOrDefault();
                        return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("不能去掉机构（{0}）的角色（{1}）", sysPosition.Name, role.Name));
                    }

                    CurrentDb.SysPositionRole.Remove(sysPositionRole);
                }

                if (rop.RoleIds != null)
                {
                    if (rop.RoleIds.Length > 0)
                    {
                        foreach (string roleId in rop.RoleIds)
                        {
                            CurrentDb.SysPositionRole.Add(new SysPositionRole { Id = GuidUtil.New(), PositionId = rop.Id, RoleId = roleId, Creator = operater, CreateTime = DateTime.Now, IsCanDelete = true });
                        }
                    }
                }

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");

            }
            return result;


        }

        public CustomJsonResult Delete(string operater, string id)
        {


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
        }
    }
}
