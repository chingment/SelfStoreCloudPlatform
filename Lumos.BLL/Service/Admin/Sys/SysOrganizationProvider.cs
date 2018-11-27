﻿using Lumos.Entity;
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

        private List<SysOrganization> GetFathers(Enumeration.BelongSite belongSite, string id)
        {
            var sysOrganizations = CurrentDb.SysOrganization.Where(m => m.BelongSite == belongSite).ToList();

            var list = new List<SysOrganization>();
            var list2 = list.Concat(GetFatherList(sysOrganizations, id));
            return list2.ToList();
        }


        public IEnumerable<SysOrganization> GetFatherList(IList<SysOrganization> list, string pId)
        {
            var query = list.Where(p => p.Id == pId).ToList();
            return query.ToList().Concat(query.ToList().SelectMany(t => GetFatherList(list, t.PId)));
        }

        private List<SysOrganization> GetSons(string id)
        {
            var sysOrganizations = CurrentDb.SysOrganization.ToList();
            var sysOrganization = sysOrganizations.Where(p => p.Id == id).ToList();
            var list2 = sysOrganization.Concat(GetSonList(sysOrganizations, id));
            return list2.ToList();
        }

        private IEnumerable<SysOrganization> GetSonList(IList<SysOrganization> list, string pId)
        {
            var query = list.Where(p => p.PId == pId).ToList();
            return query.ToList().Concat(query.ToList().SelectMany(t => GetSonList(list, t.Id)));
        }

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
                var fathter = GetFathers(rop.BelongSite, rop.PId);
                int dept = fathter.Count;
                var isExists = CurrentDb.SysOrganization.Where(m => m.PId == rop.PId && m.Name == rop.Name && m.Dept == dept).FirstOrDefault();
                if (isExists != null)
                {
                    return new CustomJsonResult(ResultType.Failure, "该名称在同一级别已经存在");
                }

                var organization = new SysOrganization();
                organization.Id = GuidUtil.New();
                organization.BelongSite = Enumeration.BelongSite.Admin;
                organization.PId = rop.PId;
                organization.Name = rop.Name;
                organization.Description = rop.Description;
                organization.Status = Enumeration.SysOrganizationStatus.Valid;
                organization.Creator = operater;
                organization.CreateTime = DateTime.Now;
                organization.Dept = dept;
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
            if (organization == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "数据为空");
            }

            var fathter = GetFathers(organization.BelongSite, organization.PId);
            int dept = fathter.Count;
            var isExists = CurrentDb.SysOrganization.Where(m => m.PId == organization.PId && m.Name == rop.Name && m.Dept == dept && m.Id != rop.Id).FirstOrDefault();
            if (isExists != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("保存失败，该名称({0})已被同一级别使用", rop.Name));
            }


            organization.Name = rop.Name;
            organization.Status = rop.Status;
            organization.Description = rop.Description;
            organization.Mender = operater;
            organization.MendTime = DateTime.Now;
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");

        }

        public CustomJsonResult Delete(string operater, string id)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var sysOrganizations = GetSons(id).ToList();

                if (sysOrganizations.Count == 0)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "请选择要删除的数据");
                }



                foreach (var sysOrganization in sysOrganizations)
                {

                    if (sysOrganization.Dept == 0)
                    {
                        return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("所选机构（{0}）不允许删除", sysOrganization.Name));
                    }

                    sysOrganization.IsDelete = true;

                }


                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功"); ;
            }

            return result;
        }

        public CustomJsonResult EditSort(string operater, RopSysOrganizationEditSort rop)
        {
            if (rop != null)
            {
                if (rop.Dics != null)
                {
                    foreach (var item in rop.Dics)
                    {
                        string id = item.Id;
                        int priority = item.Priority;
                        var sysOrganization = CurrentDb.SysOrganization.Where(m => m.Id == id).FirstOrDefault();
                        if (sysOrganization != null)
                        {
                            sysOrganization.Priority = priority;
                            CurrentDb.SaveChanges();
                        }
                    }
                }
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");

        }
    }
}
