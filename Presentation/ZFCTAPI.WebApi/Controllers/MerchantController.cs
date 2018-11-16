using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZFCTAPI.Core;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Data.BoHai.ReturnModels;
using ZFCTAPI.Data.BoHai.SubmitModels;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Services.BoHai;
using ZFCTAPI.Services.Transaction;
using ZFCTAPI.Services.UserInfo;
using ZFCTAPI.WebApi.RequestAttribute;
using ZFCTAPI.WebApi.Validates;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZFCTAPI.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [RequestLog]
    public class MerchantController : Controller
    {
        private static IBHUserService _bhUserService;
        private static ICstTransactionService _transactionService;
        private static IAccountInfoService _accountInfoService;
        public MerchantController(IBHUserService bhUserService,ICstTransactionService transactionService,IAccountInfoService accountInfoService)
        {
            _bhUserService = bhUserService;
            _transactionService = transactionService;
            _accountInfoService = accountInfoService;
        }
        /// <summary>
        /// 商户账户充值
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RBHMercRechargeBody, string> Recharge([FromBody]SMerchantRecharge model)
        {
            var retModel = new ReturnModel<RBHMercRechargeBody, string>();

            #region 校验签名

            var signResult = VerifyBase.Sign<SMerchantRecharge>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion
            var rechargeModel = new SBHMercRecharge
            {
                SvcBody =
                {
                    accountType = model.AccountType,
                    amount = model.TranMoney,
                    remark = model.Remark
                }
            };
           var info= _bhUserService.MercRecharge(rechargeModel);
           if (info.RspSvcHeader.returnCode == JSReturnCode.Success.ToString())
           {
               retModel.Message = "操作成功";
               retModel.ReturnCode = (int)ReturnCode.Success;
               retModel.ReturnData = info.SvcBody;
               retModel.Token = model.Token;
               return retModel;
           }
            retModel.Message = info.RspSvcHeader.returnMsg;
            retModel.ReturnCode = (int)ReturnCode.DataEorr;
            retModel.ReturnData = null;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 商户在账户提现
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RBHMercWithdrawBody, string> Withdraw([FromBody] SMerchantRecharge model)
        {
            var retModel = new ReturnModel<RBHMercWithdrawBody, string>();

            #region 校验签名

            var signResult = VerifyBase.Sign<SMerchantRecharge>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion
            var rechargeModel = new SBHMercWithdraw
            {
                SvcBody =
                {
                    accountType = model.AccountType,
                    amount = model.TranMoney,
                    remark = model.Remark
                }
            };
            var info = _bhUserService.MercWithdraw(rechargeModel);
            if (info.RspSvcHeader.returnCode == JSReturnCode.Success.ToString())
            {
                retModel.Message = "操作成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = info.SvcBody;
                retModel.Token = model.Token;
                return retModel;
            }
            retModel.Message = info.RspSvcHeader.returnMsg;
            retModel.ReturnCode = (int)ReturnCode.DataEorr;
            retModel.ReturnData = null;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 商户账户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RBHQueryMerchantAcctsBody, string> MerchantInfo([FromBody]BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RBHQueryMerchantAcctsBody, string>();

            #region 校验签名

            var signResult = VerifyBase.Sign<SMerchantRecharge>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion
            //获取商户账户信息
            var mModel = new SBHBaseModel();
            var merchantInfo = _bhUserService.QueryMerchantAccts(mModel);
            if (merchantInfo != null)
            {
                retModel.Message = "查询成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = merchantInfo;
                retModel.Token = model.Token;
                return retModel;
            }
            else
            {
                retModel.Message = "查询失败";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
        }

        /// <summary>
        /// 交易状态查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RBHQueryTransStatBody, string> QueryTransState([FromBody]SQueryTransState model)
        {
            var retModel = new ReturnModel<RBHQueryTransStatBody, string>();

            #region 校验签名

            var signResult = VerifyBase.Sign<SMerchantRecharge>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion

            var tranInfo = _transactionService.Find(model.TranscationId);
            if (tranInfo == null)
            {
                retModel.Message = "交易数据不存在";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            //查询交易数据
            var qmodel=new SBHQueryTransStat
            {
                SvcBody =
                {
                    merBillNo = tranInfo.pro_transaction_no,
                    queryTransType = ProjectHelper.GenerateBHTransType(tranInfo.pro_transaction_type.Value)
                }
            };
            var info = _bhUserService.QueryTransStat(qmodel);
            if (info == null)
            {
                retModel.Message = "查询数据出错";
                retModel.ReturnCode = (int) ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            else
            {
                retModel.Message = "操作成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = info;
                retModel.Token = model.Token;
                return retModel;
            }
        }

        /// <summary>
        /// 商户交易记录查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<string, string> MerchantTransQuery([FromBody] SQueryMerchantTrans model)
        {
            var retModel = new ReturnModel<string, string>();
            //生成交易数据
            var qmodel = new SBHQueryMerchantTrans
            {
                SvcBody =
                {
                    startDate = model.StartDate,
                    endDate =model.EndDate
                }
            };
            var info = _bhUserService.QueryMerchantTrans(qmodel);
            if (info == "")
            {
                retModel.Message = "生成交易数据失败";
                retModel.ReturnCode = (int) ReturnCode.DataFormatError;
                retModel.ReturnData = info;
                retModel.Token = model.Token;
                return retModel;
            }
            else
            {
                retModel.Message = "生成交易数据成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = info;
                retModel.Token = model.Token;
                return retModel;
            }
        }

        /// <summary>
        /// 联调时 商户发红包的测试
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        private ReturnModel<string, string> MerchantExperBonus([FromBody] BaseSubmitModel model)
        {
            var retModel = new ReturnModel<string, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            var account = _accountInfoService.GetAccountInfoByUserId(userId: userInfo.Id);
            //商户营销户金额是否大于红包发放金额
            var experModel = new SBHExperBonus
            {
                SvcBody =
                {
                    campaignCode = "99999999",
                    campaignInfo = "渤海联调测试",
                    userPlatformCode = account.invest_platform_id,
                    campaignMoney = "50",
                    orderNo = CommonHelper.RedBactOrderNo()
                }
            };
            var redInfo = _bhUserService.ExperBonus(experModel);
            if (redInfo.RspSvcHeader.returnCode == JSReturnCode.Success.ToString())
            {
                //保存红包流水号
                var redTrans = new CST_transaction_info
                {
                    pro_loan_id = 99999,
                    pro_transaction_money = Convert.ToDecimal(50),
                    pro_transaction_no = experModel.ReqSvcHeader.tranSeqNo,
                    pro_transaction_time = DateTime.Now,
                    pro_transaction_type = DataDictionary.transactiontype_InvestRed,
                    pro_user_id = Convert.ToInt32(CommonHelper.SurplusPlatFormId(account.invest_platform_id)),
                    pro_user_type = 9,
                    pro_transaction_remarks = "实时红包",
                    pro_transaction_status = DataDictionary.transactionstatus_success
                };
                _transactionService.Add(redTrans);
                retModel.ReturnData = "成功";
                return retModel;
            }
            return null;
        }
        /// <summary>
        /// 商户像用户转账
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RMerchantToUserMoney, string> MerchantToUserMoney([FromBody]MerchantToUserMoney model)
        {
            var retModel = new ReturnModel<RMerchantToUserMoney, string>();
            var accountInfos = _accountInfoService.GetAccountInfos(model.AccountIds);
            if (accountInfos.Any())
            {
                //获取商户账户信息
                var mModel = new SBHBaseModel();
                var merchantInfo = _bhUserService.QueryMerchantAccts(mModel);
                var sumMoney = model.CampaignMoney * accountInfos.Count;
                if (Convert.ToDecimal(merchantInfo.items.First(p => p.acTyp == "810").avlBal) <sumMoney)
                {
                    retModel.Message = "商户营销户余额小于应发金额";
                    retModel.ReturnCode = (int)ReturnCode.Success;
                    retModel.ReturnData = null;
                    retModel.Token = model.Token;
                    return retModel;
                }
                var resultData=new RMerchantToUserMoney();
                var resultInfos=new List<TransResult>();
                foreach (var accountInfo in accountInfos)
                {
                    var resultInfo=new TransResult();
                    //商户营销户金额是否大于红包发放金额
                    var experModel = new SBHExperBonus
                    {
                        SvcBody =
                        {
                            campaignCode = model.CampaignCode,
                            campaignInfo = model.CampaignInfo,
                            userPlatformCode = accountInfo.InvestId,
                            campaignMoney = model.CampaignMoney.ToString("0.00"),
                            orderNo = CommonHelper.RedBactOrderNo()
                        }
                    };
                    var result = _bhUserService.ToUserMoney(experModel,accountInfo.InvestId);
                    resultInfo.Success = result == "费用退回成功" ? "1" : "0";
                    resultInfo.ErrorInfo = result;
                    resultInfo.UserId = accountInfo.UserId;
                    resultInfo.UserName = accountInfo.PlanCustName;
                    resultInfo.UserPhone = accountInfo.PlanCustPhone;
                    resultInfos.Add(resultInfo);
                }
                resultData.ErrorCount = resultInfos.Count(p => p.Success == "0");
                resultData.SuccessCount = resultInfos.Count(p => p.Success == "1");
                resultData.TransResults = resultInfos;
                retModel.Message = "发放成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = resultData;
                retModel.Token = model.Token;
                return retModel;

            }
            return null;
        }

        /// <summary>
        /// 获取商户退款记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<RefundRecond>, string> RefundRecords([FromBody] RefundRecords model)
        {
            var retModel = new ReturnModel<ReturnPageData<RefundRecond>, string>();
            //
            ReturnPageData<RefundRecond> result = new ReturnPageData<RefundRecond>();
            try
            {
                var transcationInfo = _transactionService.GetRefundRecords(model.UserPhone,model.UserName,model.StartDate,model.EndDate,model.Page, model.PageSize);
                if (transcationInfo.Items.Any())
                {
                    foreach (var transcationInfoItem in transcationInfo.Items)
                    {
                        transcationInfoItem.OperateStateDesc =
                            transcationInfoItem.OperateState == DataDictionary.transactionstatus_success.ToString()
                                ? "成功"
                                : "失败";
                    }
                    result.PagingData = transcationInfo.Items;
                    result.Total = transcationInfo.TotalNum;
                    result.TotalPageCount = transcationInfo.TotalPageCount;
                    retModel.ReturnData = result;
                    retModel.Message = "查询成功";
                    retModel.ReturnCode = (int)ReturnCode.Success;
                    retModel.Token = model.Token;
                    return retModel;
                }
                else
                {
                    result.Total = transcationInfo.TotalNum;
                    result.TotalPageCount = transcationInfo.TotalPageCount;
                    retModel.ReturnData = null;
                    retModel.Message = "查询成功";
                    retModel.ReturnCode = (int)ReturnCode.Success;
                    retModel.Token = model.Token;
                    return retModel;
                }
            }
            catch
            {
                result.Total = 0;
                result.TotalPageCount = 0;
                retModel.ReturnData = null;
                retModel.Message = "查询失败";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.Token = model.Token;
                return retModel; ;
            }

        }

    }
}
