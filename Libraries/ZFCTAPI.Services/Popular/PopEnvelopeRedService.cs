using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using ZFCTAPI.Core;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Data.BoHai.SubmitModels;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.POP;
using ZFCTAPI.Services.LoanInfo;
using ZFCTAPI.Services.Repositorys;
using ZFCTAPI.Data.Popular;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Services.BoHai;
using ZFCTAPI.Services.Transaction;
using ZFCTAPI.Services.UserInfo;

namespace ZFCTAPI.Services.Popular
{
    public partial interface IPopEnvelopeRedService : IRepository<POP_envelope_red>
    {
        /// <summary>
        /// 根据类型获取的红包信息
        /// </summary>
        /// <returns></returns>
        List<POP_envelope_red> GetRedType(string type);

        /// <summary>
        /// 获取产品红包信息
        /// </summary>
        /// <returns></returns>
        List<POP_envelope_red> GetRedTypeProduct();

        /// <summary>
        ///
        /// </summary>
        /// <param name="redId"></param>
        /// <returns></returns>
        List<POP_envelopeRed_Product_Mapping> GetRedProductsByRedId(int redId);

        /// <summary>
        /// 投资人使用红包
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="loanId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        List<InterfaceRedEnvelope> GetRedEnvelopeLists(int userId, int loanId, decimal amount = 0m);

        /// <summary>
        /// 可用红包
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userId"></param>
        /// <param name="loanId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        List<AvailableRedEnvelope> GetRedEnvelopeIsAvailable(GebruiksType type, int userId, int loanId,
            decimal amount = 0m);

        /// <summary>
        /// 获取当前红包信息
        /// </summary>
        /// <param name="envelopeId"></param>
        /// <returns></returns>
        RedEnvelopeListInfo GetEnvelopeListInfo(int envelopeId);
        /// <summary>
        /// 渤海发送红包
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RedEnvelopesResult TransferRed(SRedEnvelopeInfo model);
        /// <summary>
        ///像用户发红包不划转
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string ToUserRed(SRedEnvelopeInfo model);

        RedEnvelopesResult TransferLoanRed(PRO_loan_info loanInfo);
    }

    public partial class PopEnvelopeRedService : Repository<POP_envelope_red>, IPopEnvelopeRedService
    {
        private static ILoanInfoService _loanInfoService;
        private static ICstRedInfoService _redInfoService;
        private static IBHUserService _bhUserService;
        private static ICstTransactionService _transactionService;
        private static IAccountInfoService _accountInfoService;
        private static ICstUserInfoService _userInfoService;

        public PopEnvelopeRedService(ILoanInfoService loanInfoService,
            ICstRedInfoService redInfoService,
            IBHUserService bhUserService,
            ICstTransactionService transactionService,
            IAccountInfoService accountInfoService,
            ICstUserInfoService userInfoService)
        {
            _loanInfoService = loanInfoService;
            _redInfoService = redInfoService;
            _bhUserService = bhUserService;
            _transactionService = transactionService;
            _accountInfoService = accountInfoService;
            _userInfoService = userInfoService;
        }

        public List<POP_envelope_red> GetRedType(string type)
        {
            var sqlStence = new StringBuilder();
            sqlStence.Append("select * from POP_envelope_red");
            sqlStence.Append(" where pop_red_type='" + RedEnvelopeType.System.ToString() + "'");
            sqlStence.Append(" and pop_red_grant='true'");
            sqlStence.Append(" and pop_red_systemType='" + type + "'");
            sqlStence.Append(" and pop_red_IsDelete='false'");
            return _Conn.Query<POP_envelope_red>(sqlStence.ToString()).ToList();
        }

        public List<POP_envelope_red> GetRedTypeProduct()
        {
            var sqlStence = new StringBuilder();
            sqlStence.Append("select * from POP_envelope_red");
            sqlStence.Append(" where pop_red_type='" + RedEnvelopeType.Product.ToString() + "'");
            sqlStence.Append(" and pop_red_grant='true'");
            sqlStence.Append(" and pop_red_IsDelete='false'");
            return _Conn.Query<POP_envelope_red>(sqlStence.ToString()).ToList();
        }

        public List<POP_envelopeRed_Product_Mapping> GetRedProductsByRedId(int redId)
        {
            var sqlStence = new StringBuilder();
            sqlStence.Append("select * from POP_envelopeRed_Product_Mapping");
            sqlStence.Append(" where POP_envelope_red_Id='" + redId + "'");
            return _Conn.Query<POP_envelopeRed_Product_Mapping>(sqlStence.ToString()).ToList();
        }

        public List<InterfaceRedEnvelope> GetRedEnvelopeLists(int userId, int loanId, decimal amount = 0m)
        {
            var redResult = new List<InterfaceRedEnvelope>();
            var proInfo = _loanInfoService.Find(loanId);
            var enableReds = _redInfoService.GeListEnableRedInfos(userId);
            foreach (var enableRed in enableReds)
            {
                if (enableRed.cst_red_endDate != null && enableRed.cst_red_endDate.Value <= DateTime.Now.Date)
                    continue;
                var avaliableRed = new InterfaceRedEnvelope();
                var red = Find(Convert.ToInt32(enableRed.cst_red_id));
                //定向。查询出当前红包所有使用规则
                var usageRules = GetRedUserules(Convert.ToInt32(enableRed.cst_red_id));
                string[] popType = { GebruiksType.Invest.ToString(), RedEnvelopeCash.AtLeastAmount.ToString() };
                bool flg = !popType.Except(usageRules.Select(p => p.pop_type)).Any();
                if (flg)
                {
                    List<Decimal?> proid = new List<Decimal?>()
                    {
                        Convert.ToDecimal(proInfo.pro_prod_typeId)
                    };
                    bool atLeastAmount = (amount >=
                                          usageRules.Find(p => p.pop_type == RedEnvelopeCash.AtLeastAmount.ToString())
                                              .pop_red_term);
                    bool invest =
                    usageRules.FindAll(p => p.pop_type == GebruiksType.Invest.ToString())
                        .Select(p => p.pop_red_term)
                        .Intersect(proid)
                        .Any();

                    avaliableRed.Id = enableRed.Id;
                    avaliableRed.Amount = enableRed.cst_red_money.Value;
                    avaliableRed.EnableAmount = enableRed.cst_red_money.Value;
                    avaliableRed.Name = red.pop_red_name;
                    if (enableRed.cst_red_endDate != null)
                    {
                        avaliableRed.ExpiryDate = (DateTime)enableRed.cst_red_endDate;
                    }
                    avaliableRed.Status = "未使用";
                    avaliableRed.IsCanUse = atLeastAmount && invest;
                    avaliableRed.Introduction = EmployIntroductions(enableRed.cst_red_id.Value);
                    redResult.Add(avaliableRed);
                    continue;
                }
                else
                {
                    foreach (var usageRule in usageRules)
                    {
                        if (usageRule.pop_type == GebruiksType.Invest.ToString())
                        {
                            avaliableRed.Id = enableRed.Id;
                            avaliableRed.Amount = enableRed.cst_red_money.Value;
                            avaliableRed.EnableAmount = enableRed.cst_red_money.Value;
                            avaliableRed.Name = red.pop_red_name;
                            avaliableRed.Introduction = EmployIntroductions(enableRed.cst_red_id.Value);
                            if (enableRed.cst_red_endDate != null)
                            {
                                avaliableRed.ExpiryDate = (DateTime)enableRed.cst_red_endDate;
                            }
                            avaliableRed.Status = "未使用";
                            avaliableRed.IsCanUse = proInfo.pro_prod_typeId == Convert.ToInt32(usageRule.pop_red_term);
                            redResult.Add(avaliableRed);
                            break;
                        }
                        else if (usageRule.pop_type == RedEnvelopeCash.AtLeastAmount.ToString())
                        {
                            avaliableRed.Id = enableRed.Id;
                            avaliableRed.Amount = enableRed.cst_red_money.Value;
                            avaliableRed.EnableAmount = enableRed.cst_red_money.Value;
                            avaliableRed.Name = red.pop_red_name;
                            avaliableRed.Introduction = EmployIntroductions(enableRed.cst_red_id.Value);
                            if (enableRed.cst_red_endDate != null)
                            {
                                avaliableRed.ExpiryDate = (DateTime)enableRed.cst_red_endDate;
                            }
                            avaliableRed.Status = "未使用";
                            avaliableRed.IsCanUse = amount >= usageRule.pop_red_term;
                            redResult.Add(avaliableRed);
                            break;
                        }
                    }
                    if (red.pop_red_useAstrictType == GebruiksType.Unlimited.ToString())
                    {
                        avaliableRed.Id = enableRed.Id;
                        avaliableRed.Amount = enableRed.cst_red_money.Value;
                        avaliableRed.EnableAmount = enableRed.cst_red_money.Value;
                        avaliableRed.Name = red.pop_red_name;
                        if (enableRed.cst_red_endDate != null)
                        {
                            avaliableRed.ExpiryDate = (DateTime)enableRed.cst_red_endDate;
                        }
                        avaliableRed.Status = "未使用";
                        avaliableRed.Introduction = EmployIntroductions(enableRed.cst_red_id.Value);
                        avaliableRed.IsCanUse = true;
                        redResult.Add(avaliableRed);
                    }
                }
            }
            return redResult;
        }

        /// <summary>
        /// 用户可使用的红包
        /// </summary>
        /// <param name="type">业务类型</param>
        /// <param name="userId">用户Id</param>
        /// <param name="loanId">项目Id</param>
        /// <param name="amount">当前业务的金额（如：投资金额、还款金额）</param>
        /// <returns></returns>
        public List<AvailableRedEnvelope> GetRedEnvelopeIsAvailable(GebruiksType type, int userId, int loanId, decimal amount = 0m)
        {
            var availableRedEnvelopes = new List<AvailableRedEnvelope>();
            try
            {
                //项目信息
                var projectInfo = _loanInfoService.Find(loanId);
                switch (type)
                {
                    case GebruiksType.Invest://投资
                        //1、查询用户所有可用红包
                        //2、根据红包查询出所有使用条件
                        //3、根据传入条件匹配使用条件
                        //用户可用投资红包信息
                        var enableReds = _redInfoService.GeListEnableRedInfos(userId);
                        foreach (var enableRed in enableReds)
                        {
                            //过期的红包不显示
                            if (enableRed.cst_red_endDate != null && enableRed.cst_red_endDate.Value <= DateTime.Now.Date)
                                continue;
                            //未领取的红包不显示
                            //if(!enableRed.cst_red_recive)
                            //    continue;
                            var availableRed = new AvailableRedEnvelope();
                            //获取对应的红包信息
                            var red = Find(Convert.ToInt32(enableRed.cst_red_id));

                            //定向。查询出当前红包所有使用规则
                            var usageRules = GetRedUserules(Convert.ToInt32(enableRed.cst_red_id));
                            string[] popType = { GebruiksType.Invest.ToString(), RedEnvelopeCash.AtLeastAmount.ToString() };
                            bool flg = !popType.Except(usageRules.Select(p => p.pop_type)).Any();
                            if (flg)
                            {
                                List<Decimal?> proid = new List<Decimal?>()
                                {
                                   Convert.ToDecimal(projectInfo.pro_prod_typeId)
                                };
                                bool atLeastAmount = (amount >= usageRules.Find(p => p.pop_type == RedEnvelopeCash.AtLeastAmount.ToString()).pop_red_term);
                                bool invest = usageRules.FindAll(p => p.pop_type == GebruiksType.Invest.ToString()).Select(p => p.pop_red_term).Intersect(proid).Any();
                                if (atLeastAmount && invest)
                                {
                                    availableRed.Id = enableRed.Id;
                                    availableRed.Amount = enableRed.cst_red_money.Value;
                                    availableRed.EnableAmount = enableRed.cst_red_money.Value;
                                    availableRed.Name = red.pop_red_name;
                                    if (enableRed.cst_red_endDate != null)
                                    {
                                        availableRed.ExpiryDate = (DateTime)enableRed.cst_red_endDate;
                                    }
                                    availableRed.Status = !enableRed.cst_red_employ ? "未使用" : "已使用";
                                    availableRedEnvelopes.Add(availableRed);
                                    continue;
                                }
                            }
                            else
                            {
                                foreach (var usageRule in usageRules)
                                {
                                    if (usageRule.pop_type == GebruiksType.Invest.ToString())
                                    {
                                        if (projectInfo.pro_prod_typeId == Convert.ToInt32(usageRule.pop_red_term))
                                        {
                                            availableRed.Id = enableRed.Id;
                                            availableRed.Amount = enableRed.cst_red_money.Value;
                                            availableRed.EnableAmount = enableRed.cst_red_money.Value;
                                            availableRed.Name = red.pop_red_name;
                                            if (enableRed.cst_red_endDate != null)
                                            {
                                                availableRed.ExpiryDate = (DateTime)enableRed.cst_red_endDate;
                                            }
                                            if (!enableRed.cst_red_employ)
                                            {
                                                availableRed.Status = "未使用";
                                            }
                                            else
                                            {
                                                availableRed.Status = "已使用";
                                            }
                                            availableRedEnvelopes.Add(availableRed);
                                            break;
                                        }
                                    }
                                    else if (usageRule.pop_type == RedEnvelopeCash.AtLeastAmount.ToString())
                                    {
                                        if (amount >= usageRule.pop_red_term)
                                        {
                                            availableRed.Id = enableRed.Id;
                                            availableRed.Amount = enableRed.cst_red_money.Value;
                                            availableRed.EnableAmount = enableRed.cst_red_money.Value;
                                            availableRed.Name = red.pop_red_name;
                                            if (enableRed.cst_red_endDate != null)
                                            {
                                                availableRed.ExpiryDate = (DateTime)enableRed.cst_red_endDate;
                                            }
                                            if (!enableRed.cst_red_employ)
                                            {
                                                availableRed.Status = "未使用";
                                            }
                                            else
                                            {
                                                availableRed.Status = "已使用";
                                            }
                                            availableRedEnvelopes.Add(availableRed);
                                            break;
                                        }
                                    }
                                }
                                if (red.pop_red_useAstrictType == GebruiksType.Unlimited.ToString())
                                {
                                    availableRed.Id = enableRed.Id;
                                    availableRed.Amount = enableRed.cst_red_money.Value;
                                    availableRed.EnableAmount = enableRed.cst_red_money.Value;
                                    availableRed.Name = red.pop_red_name;
                                    if (enableRed.cst_red_endDate != null)
                                    {
                                        availableRed.ExpiryDate = (DateTime)enableRed.cst_red_endDate;
                                    }

                                    if (!enableRed.cst_red_employ)
                                    {
                                        availableRed.Status = "未使用";
                                    }
                                    else
                                    {
                                        availableRed.Status = "已使用";
                                    }
                                    availableRedEnvelopes.Add(availableRed);
                                }
                            }
                        }

                        break;
                }
            }
            catch (Exception)
            {
            }

            return availableRedEnvelopes;
        }

        #region 辅助

        public List<POP_red_userule> GetRedUserules(int redId)
        {
            var sqlStence = $"select * from POP_red_userule where pop_envelope_id='{redId}'";
            return _Conn.Query<POP_red_userule>(sqlStence).ToList();
        }

        /// <summary>
        /// 获取当前红包基本信息
        /// </summary>
        /// <param name="envelopeId">红包Id</param>
        /// <returns></returns>
        public RedEnvelopeListInfo GetEnvelopeListInfo(int envelopeId)
        {
            RedEnvelopeListInfo redEnvelopeinfo = new RedEnvelopeListInfo();

            var redinfo = Find(envelopeId);
            redEnvelopeinfo.Name = redinfo.pop_red_name;
            if (redinfo.pop_red_type == RedEnvelopeType.Orientation.ToString())
            {
                redEnvelopeinfo.Type = "定向红包";
            }
            else if (redinfo.pop_red_type == RedEnvelopeType.System.ToString())
            {
                redEnvelopeinfo.Type = EnumValueExchange.ConvertGrantTypeValueStr(redinfo.pop_red_systemType);
            }
            else if (redinfo.pop_red_type == RedEnvelopeType.Product.ToString())
            {
                redEnvelopeinfo.Type = "项目红包";
            }
            if (redinfo.pop_red_useAstrictType == GebruiksType.Invest.ToString())
            {
                redEnvelopeinfo.EmployIntroductions = EmployIntroductions(envelopeId);
            }
            else if (redinfo.pop_red_useAstrictType == GebruiksType.Repayment.ToString())
                redEnvelopeinfo.EmployIntroductions = EmployIntroductions(envelopeId);
            else if (redinfo.pop_red_useAstrictType == GebruiksType.Cash.ToString())
                redEnvelopeinfo.EmployIntroductions = EmployIntroductions(envelopeId);
            else if (redinfo.pop_red_useAstrictType == GebruiksType.Unlimited.ToString())
                redEnvelopeinfo.EmployIntroductions = "无限制使用";
            return redEnvelopeinfo;
        }

        public RedEnvelopesResult TransferRed(SRedEnvelopeInfo model)
        {
            //获取商户账户信息
            var mModel = new SBHBaseModel();
            var merchantInfo=_bhUserService.QueryMerchantAccts(mModel);
            if (Convert.ToDecimal(merchantInfo.items.First(p => p.acTyp == "810").avlBal) >
    Convert.ToDecimal(model.CampaignMoney))
            {
                //商户营销户金额是否大于红包发放金额
                var experModel = new SBHExperBonus
                {
                    SvcBody =
                    {
                        campaignCode = model.LoanId.ToString(),
                        campaignInfo = model.LoanName,
                        userPlatformCode = model.UserPlatformCode,
                        campaignMoney = model.CampaignMoney,
                        orderNo =CommonHelper.RedBactOrderNo()
                    }
                };
                var redInfo = _bhUserService.ExperBonus(experModel);
                if (redInfo.RspSvcHeader.returnCode == JSReturnCode.Success.ToString())
                {
                    //保存红包流水号
                    var redTrans = new CST_transaction_info
                    {
                        pro_transaction_money = Convert.ToDecimal(model.CampaignMoney),
                        pro_transaction_no = experModel.ReqSvcHeader.tranSeqNo,
                        pro_transaction_time = DateTime.Now,
                        pro_transaction_type = DataDictionary.transactiontype_InvestRed,
                        pro_user_id =Convert.ToInt32(CommonHelper.SurplusPlatFormId(model.UserPlatformCode)),
                        pro_user_type = 9,
                        pro_transaction_remarks = "实时红包",
                        pro_transaction_resource = model.RequestSource
                    };
                    _transactionService.Add(redTrans);

                    //如果红包发放成功直接发放至用户账户
                    var redModel = new SBHCampaignTransfer
                    {
                        SvcBody =
                        {
                            platformUid = model.UserPlatformCode,
                            amount = model.CampaignMoney,
                            comment = model.LoanName
                        }
                    };
                    var result = _bhUserService.CampaignTransfer(redModel);
                    if (result.RspSvcHeader.returnCode == JSReturnCode.Success.ToString())
                    {
                        //成功红包记录添加
                        var redTrans2 = new CST_transaction_info
                        {
                            pro_loan_id = model.LoanId,
                            pro_transaction_money = Convert.ToDecimal(model.CampaignMoney),
                            pro_transaction_no = redModel.ReqSvcHeader.tranSeqNo,
                            pro_transaction_time = DateTime.Now,
                            pro_transaction_type = DataDictionary.transactiontype_InvestRed,
                            pro_user_id = Convert.ToInt32(CommonHelper.SurplusPlatFormId(model.UserPlatformCode)),
                            pro_user_type = 9,
                            pro_transaction_remarks = "红包转账",
                            pro_transaction_status = DataDictionary.transactionstatus_success
                        };
                        _transactionService.Add(redTrans2);
                        return RedEnvelopesResult.Success;
                    }
                    return RedEnvelopesResult.TransferRedError;
                }
                else
                {
                    return RedEnvelopesResult.ExperBonusError;
                }
            }
            else
            {
                return RedEnvelopesResult.InsufficientBalance;
            }
        }

        public string ToUserRed(SRedEnvelopeInfo model)
        {
            //获取商户账户信息
            var mModel = new SBHBaseModel();
            var merchantInfo = _bhUserService.QueryMerchantAccts(mModel);
            if (Convert.ToDecimal(merchantInfo.items.First(p => p.acTyp == "810").avlBal) >
    Convert.ToDecimal(model.CampaignMoney))
            {
                //商户营销户金额是否大于红包发放金额
                var experModel = new SBHExperBonus
                {
                    SvcBody =
                    {
                        campaignCode = model.LoanId.ToString(),
                        campaignInfo = model.LoanName,
                        userPlatformCode = model.UserPlatformCode,
                        campaignMoney = model.CampaignMoney,
                        orderNo =CommonHelper.RedBactOrderNo()
                    }
                };
                var redInfo = _bhUserService.ExperBonus(experModel);
                if (redInfo.RspSvcHeader.returnCode == JSReturnCode.Success.ToString())
                {
                    //保存红包流水号
                    var redTrans = new CST_transaction_info
                    {
                        pro_transaction_money = Convert.ToDecimal(model.CampaignMoney),
                        pro_transaction_no = experModel.ReqSvcHeader.tranSeqNo,
                        pro_transaction_time = DateTime.Now,
                        pro_transaction_type = DataDictionary.transactiontype_InvestRed,
                        pro_user_id = Convert.ToInt32(CommonHelper.SurplusPlatFormId(model.UserPlatformCode)),
                        pro_user_type = 9,
                        pro_transaction_remarks = "实时红包",
                        pro_transaction_resource = model.RequestSource
                    };
                    _transactionService.Add(redTrans);
                    return redInfo.SvcBody.transId;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public RedEnvelopesResult TransferLoanRed(PRO_loan_info loanInfo)
        {
            var redList = _redInfoService.LoanRedList(loanInfo.Id);
            if (redList.Any())
            {
                var isSuceess = true;
                //获取商户账户信息
                var mModel = new SBHBaseModel();
                var merchantInfo = _bhUserService.QueryMerchantAccts(mModel);
                if (Convert.ToDecimal(merchantInfo.items.First(p => p.acTyp == "810").avlBal) <
                    Convert.ToDecimal(redList.Sum(p=>(decimal)p.cst_red_money)))
                {
                    return RedEnvelopesResult.ExperBonusError;
                }
                foreach (var cstRedInfo in redList)
                {
                    if (cstRedInfo.cst_isTransfer)
                    {
                        continue;
                    }
                    var userInfo = _userInfoService.GetUserInfo(customerId: cstRedInfo.cst_user_id.Value);
                    if (userInfo != null&&userInfo.cst_account_id!=null)
                    {
                        var account = _accountInfoService.GetAccountInfoByUserId(userId: userInfo.Id);
                        //商户营销户金额是否大于红包发放金额
                        var experModel = new SBHExperBonus
                        {
                            SvcBody =
                        {
                            campaignCode = loanInfo.Id.ToString(),
                            campaignInfo = loanInfo.pro_loan_no,
                            userPlatformCode = account.invest_platform_id,
                            campaignMoney = cstRedInfo.cst_red_money.ToString(),
                            orderNo = CommonHelper.RedBactOrderNo()
                        }
                        };
                        var redInfo = _bhUserService.ExperBonus(experModel);
                        if (redInfo.RspSvcHeader.returnCode == JSReturnCode.Success.ToString())
                        {
                            //保存红包流水号
                            var redTrans = new CST_transaction_info
                            {
                                pro_loan_id = loanInfo.Id,
                                pro_transaction_money = Convert.ToDecimal(cstRedInfo.cst_red_money),
                                pro_transaction_no = experModel.ReqSvcHeader.tranSeqNo,
                                pro_transaction_time = DateTime.Now,
                                pro_transaction_type = DataDictionary.transactiontype_InvestRed,
                                pro_user_id = Convert.ToInt32(CommonHelper.SurplusPlatFormId(account.invest_platform_id)),
                                pro_user_type = 9,
                                pro_transaction_remarks = "实时红包",
                                pro_transaction_status = DataDictionary.transactionstatus_success
                            };
                            _transactionService.Add(redTrans);

                            //如果红包发放成功直接发放至用户账户
                            var redModel = new SBHCampaignTransfer
                            {
                                SvcBody =
                            {
                                platformUid = account.invest_platform_id,
                                amount = cstRedInfo.cst_red_money.ToString(),
                                comment = loanInfo.pro_loan_no
                            }
                            };
                            var result = _bhUserService.CampaignTransfer(redModel);
                            if (result.RspSvcHeader.returnCode == JSReturnCode.Success.ToString())
                            {
                                //成功红包记录添加
                                var redTrans2 = new CST_transaction_info
                                {
                                    pro_loan_id = loanInfo.Id,
                                    pro_transaction_money = Convert.ToDecimal(cstRedInfo.cst_red_money),
                                    pro_transaction_no = redModel.ReqSvcHeader.tranSeqNo,
                                    pro_transaction_time = DateTime.Now,
                                    pro_transaction_type = DataDictionary.transactiontype_InvestRed,
                                    pro_user_id = Convert.ToInt32(CommonHelper.SurplusPlatFormId(account.invest_platform_id)),
                                    pro_user_type = 9,
                                    pro_transaction_remarks = "红包转账",
                                    pro_transaction_status = DataDictionary.transactionstatus_success
                                };
                                _transactionService.Add(redTrans2);
                                cstRedInfo.cst_isTransfer = true;
                                _redInfoService.Update(cstRedInfo);
                            }
                            else
                            {
                                isSuceess = false;
                            }
                        }
                        else
                        {
                            isSuceess = false;
                        }
                    }                    
                }
                if (isSuceess == true)
                {
                    return RedEnvelopesResult.Success;
                }
                else
                {
                    return RedEnvelopesResult.PartError;
                }

            }
            else
            {
                return RedEnvelopesResult.Error;
            }
        }

        /// <summary>
        /// 红包使用说明
        /// </summary>
        /// <param name="redid">红包表主键</param>
        /// <returns></returns>
        public string EmployIntroductions(int redid)
        {
            var ues = GetRedUserules(redid);
            var sb = new StringBuilder();
            var invsts = ues.FindAll(p => p.pop_type == GebruiksType.Invest.ToString());
            var atLeasts = ues.FindAll(p => p.pop_type == RedEnvelopeCash.AtLeastAmount.ToString());
            var product = new List<string>();
            foreach (var invst in invsts)
            {
                var proProduct = _loanInfoService.GetProductInfo(Convert.ToInt32(invst.pop_red_term));
                product.Add(proProduct != null ? proProduct.pdo_product_name : "-");
            }
            if (product.Count > 0)
                sb.Append("投资“" + string.Join("、", product) + "”可使用.");
            foreach (var atLeast in atLeasts)
            {
                sb.Append("并且投资至少" + string.Format("{0:C}", atLeast.pop_red_term) + "元可使用.");
            }

            return sb.ToString();
        }

        #endregion 辅助
    }
}