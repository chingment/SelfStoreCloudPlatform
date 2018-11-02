using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class MachineService : BaseProvider
    {
        public CustomJsonResult LoginByQrCode(string pOperater, string pClientId, RopMachineLoginByQrCode rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            Biz.RModels.RopMachineLoginByQrCode bizRop = new Biz.RModels.RopMachineLoginByQrCode();

            bizRop.LoginUserId = pClientId;
            bizRop.Token = rop.Token;
            bizRop.MerchantId = rop.MerchantId;
            bizRop.StoreId = rop.StoreId;
            bizRop.MachineId = rop.MachineId;


            result = BizFactory.Machine.LoginByQrCode(pOperater, bizRop);

            return result;
        }


        public CustomJsonResult GetLoginConfirmInfo(string pOperater, string pClientId, RupMachineGetLoginConfirmInfo rup)
        {
            var result = new CustomJsonResult();

            var ret = new RetOperateResult();

            var merchant = CurrentDb.SysMerchantUser.Where(m => m.Id == rup.MerchantId).FirstOrDefault();

            if (merchant == null)
            {
                ret.Result = RetOperateResult.ResultType.Failure;
                ret.Remarks = "";
                ret.Message = "商户不存在";
                ret.IsComplete = true;

                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "", ret);
            }

            var store = CurrentDb.Store.Where(m => m.Id == rup.StoreId).FirstOrDefault();

            if (store == null)
            {
                ret.Result = RetOperateResult.ResultType.Failure;
                ret.Remarks = "";
                ret.Message = "店铺不存在";
                ret.IsComplete = true;

                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "", ret);
            }

            var machine = CurrentDb.MerchantMachine.Where(m => m.MerchantId == rup.MerchantId && m.MachineId == rup.MachineId && m.IsBind == true).FirstOrDefault();

            if (machine == null)
            {
                ret.Result = RetOperateResult.ResultType.Failure;
                ret.Remarks = "";
                ret.Message = "机器不存在";
                ret.IsComplete = true;

                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "", ret);
            }

            ret.Result = RetOperateResult.ResultType.Tips;
            ret.Remarks = "";
            ret.Message = "您确定要登录机器吗？";
            ret.IsComplete = true;

            ret.Buttons.Add(new RetOperateResult.Button() { Name = "登录", Color = "red", Url = "" });
            ret.Buttons.Add(new RetOperateResult.Button() { Name = "取消", Color = "green", Url = "" });

            ret.Fields.Add(new RetOperateResult.Field() { Name = "商户", Value = merchant.MerchantName });
            ret.Fields.Add(new RetOperateResult.Field() { Name = "店铺", Value = store.Name });
            ret.Fields.Add(new RetOperateResult.Field() { Name = "机器编码", Value = machine.MachineName });



            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
            return result;
        }
    }
}
