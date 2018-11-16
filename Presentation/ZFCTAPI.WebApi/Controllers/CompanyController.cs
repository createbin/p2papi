using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ZFCTAPI.Core.Caching;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Core.Security;
using ZFCTAPI.Data.ApiModels;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Data.BoHai.ReturnModels;
using ZFCTAPI.Data.BoHai.SubmitModels;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.Customers;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Services.BoHai;
using ZFCTAPI.Services.LoanInfo;
using ZFCTAPI.Services.UserInfo;
using ZFCTAPI.WebApi.RequestAttribute;
using ZFCTAPI.WebApi.Validates;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZFCTAPI.WebApi.Controllers
{
    /// <summary>
    /// 企业开户
    /// </summary>
    [Route("api/[controller]/[action]")]
    [RequestLog]
    public class CompanyController : Controller
    {
        private readonly ICompanyInfoService _companyInfoService;
        private readonly IAccountInfoService _accountInfoService;
        private readonly IBHUserService _bhUserService;
        private readonly ZfctWebConfig _zfctWebConfig;
        private readonly ICacheManager _cacheManager;
        private readonly ICustomerService _customerService;
        private readonly ICstUserInfoService _cstUserInfoService;
        private readonly IBHAccountService _bhAccountService;
        private readonly ILoanInfoService _loanInfoService;
        private readonly ILoanPlanService _loanPlanService;

        public CompanyController(ICompanyInfoService companyInfoService,
            IAccountInfoService accountInfoService,
            IBHUserService bhUserService,
            ZfctWebConfig zfctWebConfig,
            ICacheManager cacheManager,
            ICustomerService customerService,
            ICstUserInfoService cstUserInfoService,
            IBHAccountService bhAccountService,
            ILoanInfoService loanInfoService,
            ILoanPlanService loanPlanService)
        {
            _companyInfoService = companyInfoService;
            _accountInfoService = accountInfoService;
            _bhUserService = bhUserService;
            _zfctWebConfig = zfctWebConfig;
            _cacheManager = cacheManager;
            _customerService = customerService;
            _cstUserInfoService = cstUserInfoService;
            _bhAccountService = bhAccountService;
            _loanInfoService = loanInfoService;
            _loanPlanService = loanPlanService;
        }

        /// <summary>
        /// 企业户注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RRegisterModel, string> CompanyRegister([FromBody] SCompanyRegisterModel model)
        {
            var registerModel = new ReturnModel<RRegisterModel, string>();

            #region 验签
            var signResult = VerifyBase.Sign<SRegisterModel>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                registerModel.Message = "签名错误";
                registerModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                registerModel.ReturnData = null;
                registerModel.Token = model.Token;
                return registerModel;
            }
            #endregion
            //手机验证码
            var existPhone = _cacheManager.Get<String>(model.Phone);
            if (existPhone == null || existPhone != model.VerCode)
            {
                registerModel.Message = "手机验证码过期或错误！";
                registerModel.ReturnCode = (int)ReturnCode.RegisterEorr;
                return registerModel;
            }
            //用户名不存在
            if (string.IsNullOrEmpty(model.UserName))
            {
                model.UserName = model.Phone;
            }
            #region 校验信息是否满足注册条件
            if (!Regex.IsMatch(model.Phone, RegularExpression.Mobile))
            {
                registerModel.Message = "手机号码格式不正确！";
                registerModel.ReturnCode = (int)ReturnCode.RegisterEorr;
                return registerModel;
            }
            if (!Regex.IsMatch(model.Password, RegularExpression.PassWordRegu))
            {
                registerModel.Message = "密码必须为6~16位以上的数字或字母！";
                registerModel.ReturnCode = (int)ReturnCode.RegisterEorr;
                return registerModel;
            }
            #endregion

            #region 判断手机号和用户名的可用性

            var isExist = _customerService.GetCustomerByCondition(phone: model.Phone);
            var resgisterInfo = _cstUserInfoService.GetUserInfo(phone: model.Phone);
            if (isExist != null && isExist.IsAdminAccount && resgisterInfo != null)
            {
                registerModel.Message = "用户手机在系统中已存在！";
                registerModel.ReturnCode = (int)ReturnCode.RegisterEorr;
                return registerModel;
            }
            #endregion

            var customer = _customerService.InsertGuestCustomer();
            customer.Phone = model.Phone;
            customer.cst_invitation_code = _customerService.CreateYqm();
            customer.Username = CommonHelper.CreateCompanyName();
            var inviter = new Customer();
            if (!string.IsNullOrEmpty(model.RecommendCode) && Regex.IsMatch(model.RecommendCode, RegularExpression.Mobile))
            {
                inviter = _customerService.GetCustomerByCondition(phone: model.RecommendCode);
                if (inviter.cst_invitation_code != null)
                {
                    model.RecommendCode = inviter.cst_invitation_code;
                }
            }

            var isApproved = CustomerSettings.UserRegistrationType == UserRegistrationType.Standard;
            var registrationRequest = new CustomerRegistrationRequest(customer, model.Email,
                model.UserName, customer.Phone, model.Password, CustomerSettings.DefaultPasswordFormat, model.RecommendCode, model, isApproved);
            var registerResult = _customerService.RegisterCompany(registrationRequest);
            if (registerResult.Success)
            {
                var token = new TokenModel(customer.Id);
                var register = new RRegisterModel { UserId = customer.Id, UserName = customer.Username };
                registerModel.ReturnData = register;
                registerModel.Message = "注册成功！";
                registerModel.ReturnCode = (int)ReturnCode.Success;
                registerModel.Token = CryptHelper.Encrypt(JsonConvert.SerializeObject(token));
                return registerModel;
            }
            registerModel.Message = "注册失败！";
            registerModel.ReturnCode = (int)ReturnCode.RegisterEorr;
            return registerModel;
        }

        /// <summary>
        /// 返回企业用户是否已经结算和大额充值户开户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RCompanyState, string> CompanyState([FromBody] BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RCompanyState, string>();

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
            #endregion

            if (userInfo.CompanyId == null || userInfo.CompanyId.Value == 0)
            {
                retModel.Message = "该用户不是企业户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userInfo.Id);
            var state = new RCompanyState
            {
                JieSuan = accountInfo.JieSuan,
                ChargeAccount = accountInfo.BoHai
            };
            retModel.Message = "成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = state;
            retModel.Token = model.Token;
            return retModel;
        }
        /// <summary>
        /// 返回企业用户现有的信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RCompanyRealInfo, string> CompanyRealInfo([FromBody] BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RCompanyRealInfo, string>();

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
            #endregion
            if (userInfo.CompanyId == null || userInfo.CompanyId.Value == 0)
            {
                retModel.Message = "该用户不是企业户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            //获取企业信息
            var companyInfo = _companyInfoService.Find(userInfo.CompanyId.Value);
            //获取企业账户信息
            var accountInfo = _accountInfoService.Find(userInfo.cst_account_id.Value);
            var result = new RCompanyRealInfo
            {
                CompanyName = companyInfo.CompanyName,
                ContactPhone = companyInfo.ContactPhone,
                ContactUser = companyInfo.ContactUser,
                CorperationIdCard = companyInfo.CorperationIdCard,
                CorperationName = companyInfo.RealName,
                IdCard = companyInfo.InstuCode,
                LicenseCode = companyInfo.BusiCode,
                TaxId = companyInfo.TaxCode,
                AccountName = companyInfo.CompanyName
            };
            result.JieSuanMsg = accountInfo.JieSuanMsg;
            result.BoHaiMsg = accountInfo.BhMsg;
            if (accountInfo.JieSuan)
            {
                result.JieSuan = "1";
                result.OnceJieSuan = "1";
                result.CompanyType = accountInfo.act_business_property == 4 ? "1" : "2";
            }
            if (accountInfo.BoHai)
            {
                result.BoHai = "1";
                result.OnceBoHai = "1";
            }
            if (!accountInfo.JieSuan && !string.IsNullOrEmpty(accountInfo.JieSuanCode))
            {
                result.OnceJieSuan = "1";
            }
            if (!accountInfo.BoHai && !string.IsNullOrEmpty(accountInfo.BhCode))
            {
                result.OnceBoHai = "1";
            }
            var chargeAccount = _companyInfoService.GetChargeAccount(companyId: companyInfo.Id);
            if (chargeAccount != null)
            {
                result.AccountNo = chargeAccount.AccountNo;
                result.AccountBk = chargeAccount.AccountBk;
                result.ChargeAccountName = chargeAccount.AccountName;
                result.ChargeAccountNo = chargeAccount.ChargeAccountNo;
                result.RealNameState = chargeAccount.RealNameState ? "1" : "0";
                result.RealNameMoney = chargeAccount.CertificationMoney.ToString("0.00");
            }
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = result;
            retModel.Token = model.Token;
            return retModel;
        }
        /// <summary>
        /// 企业用户结算账户注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<string, string> JieSuanCoRegister([FromBody] SJsCoRegisterModel model)
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
            #endregion

            if (userInfo.CompanyId == null || userInfo.CompanyId.Value == 0)
            {
                retModel.Message = "该用户不是企业户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            //获取企业信息
            var companyInfo = _companyInfoService.Find(userInfo.CompanyId.Value);

            //获取企业账户信息 企业用户注册时连带账户信息输入
            var accountInfo = _accountInfoService.Find(userInfo.cst_account_id.Value);

            //更新开户类型
            accountInfo.act_business_property = model.IsGuarantee == 1 ? 4 : 5;
            _accountInfoService.Update(accountInfo);

            #region 重新处理账户信息
            companyInfo.ChargeAccount = false;
            companyInfo.CompanyName = model.CompanyName;
            companyInfo.CorperationIdCard = model.CorperationIdCard;
            companyInfo.RealName = model.CorperationName;
            companyInfo.UpdateTime = DateTime.Now;
            companyInfo.UserName = model.AccountName == "" ? model.CompanyName : model.AccountName;
            //处理注册信息
            companyInfo.InstuCode = model.IdCard;
            companyInfo.BusiCode = model.LicenseCode;
            companyInfo.TaxCode = model.TaxId;
            companyInfo.ContactUser = model.ContactUser;
            companyInfo.ContactPhone = model.ContactPhone;
            _companyInfoService.Update(companyInfo);
            #endregion
            #region 处理账户信息

            accountInfo.act_legal_name = model.CompanyName;
            _accountInfoService.Update(accountInfo);
            #endregion
            //数据处理完成后操作企业开户
            //构造请求注册中心数据 
            var postData = new SBHUserAdd
            {
                SvcBody =
            {
                idcard = companyInfo.InstuCode,
                identityType = IdentityType.OrganizationCodeNumber.ToString(),
                businessType =UserType.BusinessUser.ToString(),
                //platformUidInvestment = CommonHelper.CreatePlatFormId(userInfo.Id,UserAttributes.Invester),
                //platformUidFinance = CommonHelper.CreatePlatFormId(userInfo.Id,UserAttributes.Financer),
                platformUid =  CommonHelper.CreatePlatFormId(userInfo.Id,UserAttributes.Financer),
                businessProperty="2",
                accountTyp=model.IsGuarantee.ToString(),
                truename = companyInfo.ContactUser,
                phonenum = companyInfo.ContactPhone,
                companyName = model.CompanyName,
                corperationName = companyInfo.RealName,
                corperationIdentityType = IdentityType.IdCard.ToString(),
                corperationIdcard = companyInfo.CorperationIdCard,
                accountName = model.AccountName==""?model.CompanyName:model.AccountName,
                //渤海银行不允许机构投资
                companyInvestmentPermit = "0"
            }
            };
            //如果非三证合一
            if (!string.IsNullOrEmpty(companyInfo.BusiCode))
            {
                postData.SvcBody.licenseCode = companyInfo.BusiCode;
            }
            if (!string.IsNullOrEmpty(companyInfo.TaxCode))
            {
                postData.SvcBody.taxId = companyInfo.TaxCode;
            }
            if (string.IsNullOrEmpty(companyInfo.BusiCode) && string.IsNullOrEmpty(companyInfo.TaxCode))
            {
                postData.SvcBody.identityType = IdentityType.ThreeCardNumber;
            }


            var result = _bhUserService.UserAdd(postData, UserType.BusinessUser);
            retModel.Message = "成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = result;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 企业用户前往渤海开户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RToPage, string> OpenChargeAccount([FromBody] SJsOpenChargeModel model)
        {
            var retModel = new ReturnModel<RToPage, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SJsOpenChargeModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion

            if (userInfo.CompanyId == null || userInfo.CompanyId.Value == 0)
            {
                retModel.Message = "该用户不是企业户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            //获取企业信息
            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userInfo.Id);

            var companyInfo = _companyInfoService.Find(userInfo.CompanyId.Value);
            if (!accountInfo.JieSuan)
            {
                retModel.Message = "企业户尚未完成开户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            //操作充值户开户
            var chargeAccount = _companyInfoService.GetChargeAccount(companyId: companyInfo.Id);
            if (chargeAccount == null)
            {
                chargeAccount = new ChargeAccount
                {
                    CompanyId = companyInfo.Id,
                    AccountBk = model.AccountBk,
                    AccountName = model.AccountName,
                    AccountType = accountInfo.act_business_property == 4 ? "1" : "2",
                    AccountNo = model.AccountNo,
                    Success = false
                };
                _companyInfoService.AddChargeAccount(chargeAccount);
            }
            else
            {
                chargeAccount.AccountBk = model.AccountBk;
                chargeAccount.AccountName = model.AccountName;
                chargeAccount.AccountNo = model.AccountNo;
                _companyInfoService.UpdateChargeAccount(chargeAccount);
            }
            var returnModel = new RToPage
            {
                Url = $"{_zfctWebConfig.LocalUrl}Page/CompanyOpenAccount?token={System.Net.WebUtility.UrlEncode(model.Token)}&type={model.Type}"
            };
            retModel.Message = "成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = returnModel;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 企业户开户结果查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<string, string> QueryChargeAccountResult([FromBody] BaseSubmitModel model)
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
            #endregion

            if (userInfo.CompanyId == null || userInfo.CompanyId.Value == 0)
            {
                retModel.Message = "该用户不是企业户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userInfo.Id);
            if (!accountInfo.JieSuan)
            {
                retModel.Message = "该企业户尚未完成开户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            if (!accountInfo.BoHai)
            {
                retModel.Message = "该企业户尚未绑卡";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            //获取大额充值户信息
            var chargeInfo = _companyInfoService.GetChargeAccount(companyId: userInfo.CompanyId.Value);
            if (chargeInfo == null || !chargeInfo.Success)
            {
                retModel.Message = "该企业户尚未大额充值开户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            //查询大额充值户信息
            var queryModel = new SBHQueryChargeAccountResult
            {
                SvcBody =
                {
                    accountNo = chargeInfo.AccountNo,
                    accountTyp = chargeInfo.AccountType
                }
            };
            var info = _bhUserService.QueryChargeAccountResult(queryModel);
            if (info != null)
            {
                if (info.SvcBody.realNameFlg == "03")
                {
                    //修改企业开户状态
                    chargeInfo.RealNameState = true;
                    _companyInfoService.UpdateChargeAccount(chargeInfo);
                    if (accountInfo != null)
                    {
                        accountInfo.BoHai = true;
                        //accountInfo.act_account_no = info.SvcBody.plaCustId;
                        accountInfo.cst_plaCustId = info.SvcBody.plaCustId;
                    }
                    _accountInfoService.Update(accountInfo);
                    var companyInfo = _companyInfoService.Find(chargeInfo.CompanyId);
                    if (companyInfo != null)
                    {
                        companyInfo.AuditState = 8;
                        companyInfo.AuditDesc = "SUCCESS";
                        _companyInfoService.Update(companyInfo);
                    }

                    retModel.Message = "成功";
                    retModel.ReturnCode = (int)ReturnCode.Success;
                    retModel.ReturnData = "实名认证成功";
                    retModel.Token = model.Token;
                    return retModel;
                }
                else
                {
                    retModel.Message = "失败";
                    retModel.ReturnCode = (int)ReturnCode.Success;
                    retModel.ReturnData = "暂未通过渤海实名认证";
                    retModel.Token = model.Token;
                    return retModel;
                }

            }
            retModel.Message = "失败";
            retModel.ReturnCode = (int)ReturnCode.DataEorr;
            retModel.ReturnData = "";
            retModel.Token = model.Token;
            return retModel;

        }

        /// <summary>
        /// 查询企业户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RBHQueryChargeAccountBody, string> QueryChargeAccount([FromBody]SJsQueryChargeAccount model)
        {
            var retModel = new ReturnModel<RBHQueryChargeAccountBody, string>();

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
            #endregion

            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userInfo.Id);
            if (!accountInfo.JieSuan)
            {
                retModel.Message = "该用户尚未完成开户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            if (!accountInfo.BoHai)
            {
                retModel.Message = "该用户尚未绑卡";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            var platformUid = model.UserAttribute == 1
                ? accountInfo.invest_platform_id
                : accountInfo.financing_platform_id;
            //查询大额充值户信息
            var queryModel = new SBHQueryChargeAccount
            {
                SvcBody =
                {
                    platformUid = platformUid
                }
            };
            var info = _bhUserService.QueryChargeAccount(queryModel);

            retModel.Message = "成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = info;
            retModel.Token = model.Token;
            return retModel;
        }


        /// <summary>
        /// 大额充值记录查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[HttpPost]
        //public ReturnModel<RBHQueryChargeDetailBody, string> JieSuanChargeRecord([FromBody]SJsChargeRecord model)
        //{
        //    var retModel = new ReturnModel<RBHQueryChargeDetailBody, string>();

        //    #region 校验签名

        //    var signResult = VerifyBase.SignAndToken<RBHQueryChargeDetailBody>(model, out CST_user_info userInfo);
        //    if (signResult == ReturnCode.SignatureFailure)
        //    {
        //        retModel.Message = "签名错误";
        //        retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
        //        retModel.ReturnData = null;
        //        retModel.Token = model.Token;
        //        return retModel;
        //    }
        //    #endregion

        //    if (userInfo.CompanyId == null || userInfo.CompanyId.Value == 0)
        //    {
        //        retModel.Message = "该用户不是企业户";
        //        retModel.ReturnCode = (int)ReturnCode.DataEorr;
        //        retModel.ReturnData = null;
        //        retModel.Token = model.Token;
        //        return retModel;
        //    }
        //    //获取企业信息
        //    var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userInfo.Id);
        //    if (!accountInfo.JieSuan)
        //    {
        //        retModel.Message = "该企业户尚未注册结算中心";
        //        retModel.ReturnCode = (int)ReturnCode.DataEorr;
        //        retModel.ReturnData = null;
        //        retModel.Token = model.Token;
        //        return retModel;
        //    }
        //    //获取大额充值户信息
        //    if (!accountInfo.BoHai)
        //    {
        //        retModel.Message = "该企业户尚未大额充值开户";
        //        retModel.ReturnCode = (int)ReturnCode.DataEorr;
        //        retModel.ReturnData = null;
        //        retModel.Token = model.Token;
        //        return retModel;
        //    }
        //    //获取大额充值户信息
        //    var chargeInfo = _companyInfoService.GetChargeAccount(companyId: userInfo.CompanyId.Value);
        //    //查询大额充值户信息
        //    var queryModel = new SBHQueryChargeDetail
        //    {
        //        SvcBody =
        //        {
        //            accountTyp = "2",
        //            accountNo = chargeInfo.AccountNo,
        //            queryTyp = model.QueryType
        //        }
        //    };
        //    if (model.QueryType == "1")//历史记录查询
        //    {
        //        queryModel.SvcBody.startDate = model.StartDate;
        //        queryModel.SvcBody.endDate = model.EndDate;
        //    }
        //    else
        //    {
        //        queryModel.SvcBody.transId = "0";
        //    }
        //    queryModel.SvcBody.pageNo = model.PageNo;
        //    var info = _bhUserService.QueryChargeDetail(queryModel);

        //    retModel.Message = "成功";
        //    retModel.ReturnCode = (int)ReturnCode.Success;
        //    retModel.ReturnData = info;
        //    retModel.Token = model.Token;
        //    return retModel;
        //}

        /// <summary>
        /// 企业户余额同步
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<string, string> RechargeSyn([FromBody] SUserType model)
        {
            var retModel = new ReturnModel<string, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SUserType>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion

            //获取用户信息
            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userInfo.Id);
            if (!accountInfo.JieSuan)
            {
                retModel.Message = "该企业户尚未完成开户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            if (!accountInfo.BoHai)
            {
                retModel.Message = "该企业户尚未完成绑卡";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            var requestModel = new SBHRechargeSyn
            {
                SvcBody = new SBHRechargeSynBody()
            };
            if (model.UserType == "0")
            {
                requestModel.SvcBody.platformUid = accountInfo.invest_platform_id;
            }
            else
            {
                requestModel.SvcBody.platformUid = accountInfo.financing_platform_id;
            }
            var result = _bhAccountService.RechargeSyn(requestModel);
            if (result != null && result.RspSvcHeader.returnCode == JSReturnCode.Success)
            {
                retModel.Message = "成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = result.SvcBody.avlBal;
                retModel.Token = model.Token;
                return retModel;
            }
            else
            {
                retModel.Message = "同步余额失败";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
        }

        /// <summary>
        /// 企业户融资统计
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RCompanyStatics, string> LoanStatics([FromBody] BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RCompanyStatics, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SUserType>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion

            if (userInfo.CompanyId == null || userInfo.CompanyId.Value == 0)
            {
                retModel.Message = "该用户不是企业户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            //获取企业信息
            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userInfo.Id);
            if (!accountInfo.JieSuan)
            {
                retModel.Message = "该企业户尚未完成开户";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            //获取大额充值户信息
            if (!accountInfo.BoHai)
            {
                retModel.Message = "该企业户尚未完成绑卡";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            var loanInfos = _loanInfoService.LoansByPeriods(userInfo.Id);
            var loanPlans = _loanPlanService.GetLoanPlansByLoaner(userInfo.Id);

            var result = DealCompanyStatics(loanInfos, loanPlans);
            retModel.Message = "成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = result;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 企业户忘记密码验证输入三证号和手机号码是否匹配
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> CompanyForgetPwd([FromBody]CompanyForgetPwd model)
        {
            var rModel = new ReturnModel<bool, string>();
            rModel.ReturnData = false;

            #region 校验签名

            var signResult = VerifyBase.Sign<CompanyForgetPwd>(model);
            if (signResult != ReturnCode.Success)
            {
                rModel.Message = "签名错误";
                rModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                rModel.ReturnData = false;
                rModel.Token = model.Token;
                return rModel;
            }

            #endregion

            //手机验证码验证
            var existPhone = _cacheManager.Get<String>(model.PhoneNumber);
            if (existPhone == null || existPhone != model.VerCode)
            {
                rModel.ReturnData = false;
                rModel.Message = "手机验证码错误或过期";
                return rModel;
            }

            var companyInfo = _companyInfoService.GetCompanyInfoByPhone(model.PhoneNumber);
            if (companyInfo == null)
            {
                rModel.Message = "手机号不存在";
                return rModel;
            }

            //三证合一
            if (model.VerifyType && !string.IsNullOrEmpty(companyInfo.InstuCode)
                &&string.IsNullOrEmpty(companyInfo.BusiCode)
                && string.IsNullOrEmpty(companyInfo.TaxCode)
                && companyInfo.InstuCode.Equals(model.InstuCode, StringComparison.OrdinalIgnoreCase))
            {
                rModel.ReturnData = true;
            }
            else if (model.InstuCode == companyInfo.InstuCode
                && model.BusiCode == companyInfo.BusiCode
                && model.TaxCode == companyInfo.TaxCode)
            {
                rModel.ReturnData = true;
            }
            else
            {
                rModel.ReturnData = false;
                rModel.Message = "手机号与与证件信息不匹配";
            }

            return rModel;
        }


        #region 处理数据


        private RCompanyStatics DealCompanyStatics(List<PRO_loan_info> loans, List<PRO_loan_plan> plans)
        {
            var result = new RCompanyStatics();
            if (loans != null)
            {
                result.AllLoanCount = loans.Count().ToString();
                result.AllLoanMoney = loans.Sum(p => p.pro_loan_money.Value).ToString("0.00");
                var periods = new List<LoanStatics>();
                for (int i = 0; i < 4; i++)
                {
                    var period = new LoanStatics();
                    switch (i)
                    {
                        case 0:
                            period.MonthType = "1";
                            period.MonthCount = loans.Count(p => p.pro_loan_period == 1).ToString();
                            period.MonthMoney = loans.Where(p => p.pro_loan_period == 1).Sum(p => p.pro_loan_money).ToString();
                            break;
                        case 1:
                            period.MonthType = "3";
                            period.MonthCount = loans.Count(p => p.pro_loan_period == 3).ToString();
                            period.MonthMoney = loans.Where(p => p.pro_loan_period == 3).Sum(p => p.pro_loan_money).ToString();
                            break;
                        case 2:
                            period.MonthType = "6";
                            period.MonthCount = loans.Count(p => p.pro_loan_period == 6).ToString();
                            period.MonthMoney = loans.Where(p => p.pro_loan_period == 6).Sum(p => p.pro_loan_money).ToString();
                            break;
                        case 3:
                            period.MonthType = "12";
                            period.MonthCount = loans.Count(p => p.pro_loan_period == 12).ToString();
                            period.MonthMoney = loans.Where(p => p.pro_loan_period == 12).Sum(p => p.pro_loan_money).ToString();
                            break;
                    }
                    periods.Add(period);
                }
                if (plans != null)
                {
                    result.OverduePlanCount = plans.Count(p => !p.pro_is_clear && p.pro_pay_date < DateTime.Now).ToString();
                    result.RepayedPlanCount = plans.Count(p => p.pro_is_clear).ToString();
                    result.OverduePlanMoney = plans.Where(p => !p.pro_is_clear && p.pro_pay_date < DateTime.Now).Sum(p => p.pro_pay_total).ToString();
                    result.RepayedPlanMoney = plans.Where(p => p.pro_is_clear).Sum(p => p.pro_collect_total).ToString();
                    result.RepayingPlanCount = plans.Count(p => !p.pro_is_clear).ToString();
                    result.RepayingPlanMoney = plans.Where(p => !p.pro_is_clear).Sum(p => p.pro_pay_total).ToString();
                }
                result.LoanStatics = periods;
            }

            return result;
        }

        #endregion
    }
}
