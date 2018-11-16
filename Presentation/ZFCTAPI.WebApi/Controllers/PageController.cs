using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.Configuration.DataBase;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Core.Infrastructure;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Data.BoHai.SubmitModels;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.RabbitMQ;
using ZFCTAPI.Services.BoHai;
using ZFCTAPI.Services.Helpers;
using ZFCTAPI.Services.RabbitMQ;
using ZFCTAPI.Services.Settings;
using ZFCTAPI.Services.Transaction;
using ZFCTAPI.Services.UserInfo;
using ZFCTAPI.WebApi.RequestAttribute;
using ZFCTAPI.WebApi.Validates;

namespace ZFCTAPI.WebApi.Controllers
{
    public class PageController : Controller
    {
        private readonly IBoHaiService _boHaiService;
        private readonly IBHUserService _bhUserService;
        private readonly IBHAccountService _bhAccountService;
        private readonly IRabbitMQEvent _rabbitMQEvent;
        private readonly ISettingService _settingService;
        private readonly IAccountInfoService _accountInfoService;
        private readonly ILoanTextHelper _loanTextHelper;
        private readonly IBoHaiReconciliationService _reconciliationService;
        private readonly IUserAgentHelper _userAgentHelper;
        private readonly BoHaiApiConfig _boHaiApiConfig;
        private readonly ZfctWebConfig _zfctWebConfig;
        private readonly ICompanyInfoService _companyInfoService;
        private readonly IFeeService _feeService;

        public PageController(IBoHaiService boHaiService, IRabbitMQEvent rabbitMQEvent,
            IBHUserService bhUserService,
            ISettingService settingService,
            IAccountInfoService accountInfoService,
            IBHAccountService bhAccountService,
            ILoanTextHelper loanTextHelper,
            IBoHaiReconciliationService reconciliationService,
            IUserAgentHelper userAgentHelper,
            BoHaiApiConfig boHaiApiConfig,
            ZfctWebConfig zfctWebConfig,
            ICompanyInfoService companyInfoService,
            IFeeService feeService)
        {
            _boHaiService = boHaiService;
            _rabbitMQEvent = rabbitMQEvent;
            _bhUserService = bhUserService;
            _settingService = settingService;
            _accountInfoService = accountInfoService;
            _bhAccountService = bhAccountService;
            _loanTextHelper = loanTextHelper;
            _reconciliationService = reconciliationService;
            _userAgentHelper = userAgentHelper;
            _boHaiApiConfig = boHaiApiConfig;
            _zfctWebConfig = zfctWebConfig;
            _companyInfoService = companyInfoService;
            _feeService = feeService;
        }


        #region Test 
        public IActionResult Index()
        {
            //var model = new SBHQueryUserInfo();
            //_bhUserService.QueryUserInfo(model);
            var model = new SBHUserCancel();
            model.SvcBody.platformUid = "10000603";
            _bhUserService.UserCancel(model);
            return View();
        }



        public IActionResult TestHandleText()
        {
            _reconciliationService.DownloadHandleInvest();
            _reconciliationService.DownloadHandelRecharge();
            _reconciliationService.DownloadHandelWithDraw();
            return Content("Success");
        }

        public IActionResult Test()
        {
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = "11"
            });
            var projectConig = _settingService.LoadSetting<ProjectSettings>();
            var v1 = projectConig.sys_borrow_count;
            return Content("Success");
        }

        public IActionResult Test2()
        {
            _bhUserService.UserAdd(new SBHUserAdd(), UserType.PersonalUser);
            return Content("Success");
        }

        public IActionResult TestAddLoan()
        {
            _bhAccountService.SubmitCreditProject(new SBHSubmitCreditProject
            {
                SvcBody = new SBHSubmitCreditProjectBody
                {
                    projectCode = "120",
                    projectName = "test120",
                    projectType = ProjectType.Creditor,
                    loanType = ((int)BHLoanType.Credit).ToString(),
                    loanAmount = "10000.00",
                    transferFlag = "1",
                    interestType = "1",
                    valueDate = DateTime.Now.AddDays(10).ToString("yyyyMMdd"),
                    repayType = "2",
                    repayTypeComment="定期付息",
                    numberOfPayments = "3",
                    repayFrequency = "2",
                    //projectInterestPeriod = "30",
                    //principalRepayRate = "0.3,0.3,0.4",
                    interestPayMode = "2",
                    annualPeriod = "1",
                    APR = "0.1",
                    startTimeOfRaising = DateTime.Now.AddMinutes(5).ToString("yyyy-MM-dd HH:mm:ss"),
                    endTimeOfRaising = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd HH:mm:ss"),
                    fundList = new System.Collections.Generic.List<SBHFund> {
                        new SBHFund{
                            collectDate = DateTime.Now.AddDays(7).AddMonths(3).ToString("yyyy-MM-dd"),
                            commissionFeeMode  ="1",
                            commissionFee ="10.00"
                        }
                    },
                    //guaranteeList = new List<SBHGuarantee> {
                    //    new SBHGuarantee{
                    //        guaranteeUserCode = "20000523",
                    //        guaranteeUserName ="担保人啊",
                    //        guaranteeUserProperty ="2",
                    //        guaranteeUserIDType="04",
                    //        guaranteeUserID ="91320106MA1MQ5HJ85",
                    //        guaranteeLRName = "担保人",
                    //        guaranteeLRID = "320124198909022226"
                    //    }
                    //},
                    borrowerUserCode = "20000519",
                    borrowerUserName = "施耐德",
                    borrowerUserProperty = "1",
                    borrowerUserIDType = "01",
                    borrowerUserID = "320104199006012886",
                    borrowerPhone = "13785858585",
                    collateralInformation = "车辆",
                    purpose = "买车",
                    //guaranteeNo = "6225801238811412"
                }
            });

            return Content("Success");
        }

        public IActionResult TestCompanyLoan()
        {
            _bhAccountService.SubmitCreditProject(new SBHSubmitCreditProject
            {
                SvcBody = new SBHSubmitCreditProjectBody
                {
                    projectCode = "204",
                    projectName = "test204",
                    projectType = ProjectType.Stock,
                    loanType = ((int)BHLoanType.Credit).ToString(),
                    loanAmount = "10000.00",
                    transferFlag = "0",
                    interestType = "1",
                    valueDate = DateTime.Now.AddDays(10).ToString("yyyyMMdd"),
                    repayType = "1",
                    numberOfPayments = "1",
                    repayFrequency = "1",
                    projectInterestPeriod = "30",
                    principalRepayRate = "1",
                    interestPayMode = "2",
                    annualPeriod = "1",
                    APR = "0.1",
                    startTimeOfRaising = DateTime.Now.AddMinutes(5).ToString("yyyy-MM-dd HH:mm:ss"),
                    endTimeOfRaising = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd HH:mm:ss"),
                    fundList = new System.Collections.Generic.List<SBHFund> {
                        new SBHFund{
                            collectDate = DateTime.Now.AddDays(7).AddMonths(1).ToString("yyyy-MM-dd"),
                            commissionFeeMode  ="1",
                            commissionFee ="10.00"
                        }
                    },
                    borrowerUserCode = "20000521",
                    borrowerUserName = "梦昼初啊",
                    borrowerUserProperty = "2",
                    borrowerUserIDType = "04",
                    borrowerUserID = "91320505MA1UUL9UXB",
                    borrowerPhone = "18868884543",
                    borrowerLRName = "梦昼初",
                    borrowerLRID = "321002196504240936",
                    collateralInformation = "车辆",
                    purpose = "买车",
                    borrowerAccountNo = "6225801238811472",
                    borrowerAccountName = "梦昼初科技",
                }
            });

            return Content("Success");
        }

        public IActionResult TestTransfer(string vCode)
        {
            _bhAccountService.ClaimsTransferDeal(new SBHClaimsTransferDeal
            {
                SvcBody = new SBHClaimsTransferDealBody
                {
                    projectCode = "612",
                    projectType = ProjectType.Creditor,
                    transferApplyNumber = "20180529163259309751",
                    transferDealNumber = CommonHelper.GetMchntTxnSsn(),
                    transferorPlatformCode = "0003100000049196",
                    transfereePlatformCode = "0003100000060043",
                    dealMoney = "99.00",
                    dealPrice = "99.00",
                    transferorFee = "10",
                    mobileCode = vCode
                }
            });
            return Content("Success");
        }

        public IActionResult TestInvest(string vCode)
        {
            _bhAccountService.ClaimsPurchase(new SBHClaimsPurchase
            {
                SvcBody = new SBHClaimsPurchaseBody
                {
                    projectCode = "120",
                    projectType = ProjectType.Creditor,
                    purchaseNumber = CommonHelper.GetMchntTxnSsn(),
                    investorPlatformCode = "10000517",
                    platformAgent = "0",
                    purchaseMoney = "10000.00",
                    purchasePrice = "10000.00",
                    purchaseIP = ZfctWebEngineToConfiguration.GetIPAddress(),
                    mobileCode = vCode
                }
            });

            return Content("Success");
        }

        public IActionResult TestSendUapMsg(string mobile)
        {
            var vCode = _bhUserService.SendUapMsg(new SBHSendUapMsg
            {
                SvcBody = new SBHSendUapMsgBody
                {
                    mobileNo = mobile
                }
            });

            return Content(vCode);
        }
        #endregion

        #region 开户绑卡

        /// <summary>
        /// 用户渤海开户绑卡
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public IActionResult UserRealName([FromQuery]string token)
        {
            VerifyBase.AnalysisToken(token, out CST_user_info userInfo);
            //获取用户账户信息
            if (userInfo.cst_account_id == null)
            {
                return Content("用户账户信息获取失败");
            }
            var accountInfo = _accountInfoService.Find(userInfo.cst_account_id.Value);
            //判断用户是否已经在结算中心注册
            if (!accountInfo.JieSuan)
            {
                return Content("用户尚未开户");
            }
            var realProve = _accountInfoService.GetRealNameInfo(userInfo.Id);
            var bankInfo = _accountInfoService.GetBankInfo(userInfo.cst_account_id.Value);
            var isMobile = _userAgentHelper.IsMobileDevice();
            var bohaiModel = new SBHRealNameWeb
            {
                SvcBody = new SBHRealNameWebBody
                {
                    clientType = isMobile ? "2" : "1",
                    //platformUidInvestment = accountInfo.invest_platform_id,
                    //platformUidFinance = accountInfo.financing_platform_id,
                    identType = IdentityType.IdCard.ToString(),
                    identNo = realProve.cst_card_num,
                    usrName = realProve.cst_user_realname,
                    mobileNo = accountInfo.act_user_phone,
                    openBankId = bankInfo.bank_code,
                    openAcctId = bankInfo.bank_no,
                    pageReturnUrl = ZfctWebEngineToConfiguration.GetWebReturnUrl("OpenAccount")
                }
            };
            if (accountInfo.act_business_property.Value == 1)
            {
                bohaiModel.SvcBody.platformUid = accountInfo.invest_platform_id;
            }
            else
            {
                bohaiModel.SvcBody.platformUid = accountInfo.financing_platform_id;
            }

            var model = _bhUserService.RealNameWeb(bohaiModel, UserOpenType.NewUser);
            
            if (model == null)
                return Content("交换数据时发生错误");
            if (model.RspSvcHeader.returnCode != JSReturnCode.Success.ToString())
            {
                return Content(model.RspSvcHeader.returnMsg);
            }
            if (isMobile)
            {
                var type = MobileAddressType.CBHBNetLoanRegister.ToString();
                //跳转至渤海手机端
                var url = BoHaiApiEngineToConfiguration.BHMobileAddress() + "?Transid=" + type + "&NetLoanInfo=" +
                          model.SvcBody.NetLoanInfo;
                return Redirect(url);
            }

            return View(model.SvcBody);
        }

        /// <summary>
        /// 用户渤海换绑卡
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public IActionResult UserBindCard([FromQuery] string token)
        {
            VerifyBase.AnalysisToken(token, out CST_user_info userInfo);
            //获取用户账户信息
            if (userInfo.cst_account_id == null)
            {
                return Content("获取用户账户信息失败");
            }
            //判断用户是否已经结算开户渤海开户
            var accountInfo = _accountInfoService.Find(userInfo.cst_account_id.Value);
            if (!accountInfo.BoHai || !accountInfo.JieSuan)
                return Content("用户尚未开户绑卡");

            var isMobile = _userAgentHelper.IsMobileDevice();

            //跳转渤海换绑卡页面
            var rbhBindCardWeb = _bhUserService.BindCardWeb(new SBHBindCardWeb
            {
                SvcBody = new SBHBindCardWebBody
                {
                    clientType = isMobile == true ? "2" : "1",
                    //platformUidInvestment = accountInfo.invest_platform_id,
                    //platformUidFinance = accountInfo.financing_platform_id,
                    platformUid = accountInfo.act_business_property.Value==1? accountInfo.invest_platform_id: accountInfo.financing_platform_id,
                    plaCustId = accountInfo.cst_plaCustId,
                    pageReturnUrl = ZfctWebEngineToConfiguration.GetWebReturnUrl("BindCardWeb")
                }
            });


            if (rbhBindCardWeb != null)
            {
                if (isMobile)
                    return Redirect($"{_boHaiApiConfig.BoHaiMobileAddress}?Transid={MobileAddressType.CBHBNetLoanBindCardMessage}&NetLoanInfo={rbhBindCardWeb.NetLoanInfo}");
                else
                    return RedirectToAction("RedirectBH", CommonHelper.ObjectToDictionary(rbhBindCardWeb, true));
            }
            return Content("发生错误");
        }

        #endregion

        #region 修改手机号码,支付密码

        /// <summary>
        /// 用户渤海更换手机号
        /// </summary>
        /// <param name="token"></param>
        /// <param name="newPhone">新手机号</param>
        /// <returns></returns>
        public IActionResult UserBindMobile(string token, string newPhone)
        {
            VerifyBase.AnalysisToken(token, out CST_user_info userInfo);
            //获取用户账户信息
            if (userInfo.cst_account_id == null)
            {
                return Content("获取用户账户信息失败");
            }
            //判断用户是否已经结算开户渤海开户
            var accountInfo = _accountInfoService.Find(userInfo.cst_account_id.Value);
            if (!accountInfo.BoHai || !accountInfo.JieSuan)
                return Content("用户尚未开户绑卡");

            if (accountInfo.act_user_phone.Equals(newPhone))
                return Content("修改手机号与当前手机号相同");

            //请求渤海接口模型
            var bhModel = new SBHBindMobileNo
            {
                SvcBody = new SBHBindMobileNoBody
                {
                    clientType = ClientType.PC,
                    //platformUid = accountInfo.invest_platform_id.ToString(),//"10000517"
                    plaCustId = accountInfo.cst_plaCustId,//"0003100000047121",//
                    mobileNo = newPhone,
                    transTyp = accountInfo.act_business_property.Value.ToString(),
                    pageReturnUrl = ZfctWebEngineToConfiguration.GetWebReturnUrl("BindMobileNo")
                }
            };
            var isMobile = _userAgentHelper.IsMobileDevice();
            if (isMobile)
            {
                bhModel.SvcBody.clientType = ClientType.WEB;
                bhModel.SvcBody.usrName = accountInfo.act_user_name;
                bhModel.SvcBody.oldMobileNo = accountInfo.act_user_phone;
                bhModel.SvcBody.identType = IdentityType.IdCard;
                bhModel.SvcBody.identNo = accountInfo.act_user_card;
            }

            //请求渤海接口
            var model = _bhUserService.BindMobileNo(bhModel);

            //页面跳转
            if (isMobile)
            {
                var type = MobileAddressType.CBHBNetLoanUpdatePhone.ToString();
                var url = BoHaiApiEngineToConfiguration.BHMobileAddress() + "?Transid=" + type + "&NetLoanInfo=" +
                          model.NetLoanInfo;
                return Redirect(url);
            }
            else
            {
                return View(model);
            }
        }

        /// <summary>
        /// 修改找回支付密码
        /// </summary>
        /// <param name="token"></param>
        /// <param name="operationType">移动端必填，1修改支付密码,0找回支付密码</param>
        /// <returns></returns>
        public IActionResult UserBindPass(string token, int operationType=1)
        {
            var verifyToken = VerifyBase.Token(token, out CST_user_info userInfo);

            //验证Token
            if (verifyToken == ReturnCode.TokenEorr)
                return Content("token错误");

            //获取用户账户信息
            if (userInfo.cst_account_id == null)
            {
                return Content("获取用户账户信息错误");
            }
            //判断用户是否已经结算开户渤海开户
            var accountInfo = _accountInfoService.Find(userInfo.cst_account_id.Value);
            if (!accountInfo.BoHai || !accountInfo.JieSuan)
                return NoContent();


            //请求渤海接口模型
            var bhModel = new SBHBindPass
            {
                SvcBody = new SBHBindPassBody
                {
                    clientType = ClientType.PC,
                    plaCustId = accountInfo.cst_plaCustId,
                    accountTyp = userInfo.CompanyId != null ? "2" : "1",
                    pageReturnUrl = ZfctWebEngineToConfiguration.GetWebReturnUrl("UserBindPass"),
                    transTyp = "1"
                }
            };
            if (accountInfo.act_business_property.Value != 1)
            {
                bhModel.SvcBody.transTyp = "2";
            }

            var isMobile = _userAgentHelper.IsMobileDevice();
            if(isMobile)
            {
                bhModel.SvcBody.clientType = ClientType.WEB;
                bhModel.SvcBody.mobileNo = accountInfo.act_user_phone;
                bhModel.SvcBody.txDesc = operationType == 1 ? "修改支付密码" : "找回支付密码";
            }
            //请求渤海
            var model = _bhUserService.BindPass(bhModel);
            //页面跳转
            if (isMobile)
            {
                var type = "";
                if (userInfo.CompanyId == null)
                {
                    type = operationType == 1 ?
                    MobileAddressType.CBHBNetLoanUpdatePassword.ToString() :
                    MobileAddressType.CBHBNetLoanGetPassword.ToString();
                    
                }
                else
                {
                    type = operationType == 1 ?
                        MobileAddressType.CBHBNetLoanUpdatePwdPublic.ToString() :
                        MobileAddressType.CBHBNetLoanGetPwdPublic.ToString();
                }
                var url = BoHaiApiEngineToConfiguration.BHMobileAddress() + "?Transid=" + type + "&NetLoanInfo=" +
                              model.NetLoanInfo;
                return Redirect(url);
            }
            else
            {
                return View(model);
            }
        }

        /// <summary>
        /// 跳转渤海
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult RedirectBH(Dictionary<string, string> model)
        {
            return View(model);
        }

        #endregion

        #region 跳转渤海提现

        public IActionResult Drawings([FromQuery]string token, [FromQuery]decimal money, [FromQuery]string orderno, [FromQuery]int userattribute)
        {
            var verifyToken = VerifyBase.Token(token, out int userId);

            //验证Token
            if (verifyToken == ReturnCode.TokenEorr)
                return Content("token错误");
            //验证开户
            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userId);
            if (accountInfo == null || !accountInfo.BoHai || !accountInfo.JieSuan)
                return Content("尚未完成开户");

            var isMobile = _userAgentHelper.IsMobileDevice();
            var fee = _feeService.CalcWithdrawFee(accountInfo.act_user_id.Value, money,out bool isFirst);
            if (isFirst) fee = 0.00m;

            //发送结算中心
            var rbhDrawings = _bhAccountService.Drawings(new SBHDrawings
            {
                ReqSvcHeader = new SBHBaseHeaderModel
                {
                    tranSeqNo = orderno
                },
                SvcBody = new SBHDrawingsBody
                {
                    clientType = isMobile == true ? "2" : "1",
                    platformUid = userattribute == 1 ? accountInfo.invest_platform_id : accountInfo.financing_platform_id,
                    plaCustId = accountInfo.cst_plaCustId,
                    transAmt = money.ToString("0.00"),
                    feeType = "1",
                    merFeeAmt = fee.ToString("0.00"),
                    pageReturnUrl = _zfctWebConfig.LocalUrl + _zfctWebConfig.WebUrl.Where(p => p.Name == "Drawings").First().ReturnUrl
                }
            });

            //Post渤海
            if (rbhDrawings != null && rbhDrawings.RspSvcHeader.returnCode == JSReturnCode.Success)
            {
                if (isMobile)
                    return Redirect($"{_boHaiApiConfig.BoHaiMobileAddress}?Transid={MobileAddressType.CBHBNetLoanWithdraw}&NetLoanInfo={rbhDrawings.SvcBody.NetLoanInfo}");
                else
                    return RedirectToAction("RedirectBH", CommonHelper.ObjectToDictionary(rbhDrawings.SvcBody, true));
            }

            return Content("发生错误");
        }

        #endregion 跳转渤海提现

        public IActionResult CloseAccount([FromQuery]string token, [FromQuery]string orderno, [FromQuery]int userattribute)
        {
            var verifyToken = VerifyBase.Token(token, out int userId);

            //验证Token
            if (verifyToken == ReturnCode.TokenEorr)
                return Content("token错误");
            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userId);
            var isMobile = _userAgentHelper.IsMobileDevice();
            //发送结算中心
            var rbhCloseAccount = _bhAccountService.CloseAcount(new SBBCloseAccount
            {
                ReqSvcHeader = new SBHBaseHeaderModel
                {
                    tranSeqNo = orderno
                },
                SvcBody = new SBBCloseAccountBody
                {
                    clientType = isMobile == true ? "2" : "1",
                    platformUid = userattribute == 1 ? accountInfo.invest_platform_id : accountInfo.financing_platform_id,
                    plaCustId = accountInfo.cst_plaCustId,                    
                    pageReturnUrl = _zfctWebConfig.LocalUrl + _zfctWebConfig.WebUrl.Where(p => p.Name == "CloseAccount").First().ReturnUrl
                }
            });
            //Post渤海
            if (rbhCloseAccount != null && rbhCloseAccount.RspSvcHeader.returnCode == JSReturnCode.Success)
            {
                if (isMobile)
                    return Redirect($"{_boHaiApiConfig.BoHaiMobileAddress}?Transid={MobileAddressType.CBHBNetLoanWithdraw}&NetLoanInfo={rbhCloseAccount.SvcBody.NetLoanInfo}");
                else
                    return RedirectToAction("RedirectBH", CommonHelper.ObjectToDictionary(rbhCloseAccount.SvcBody, true));
            }
            return Content("发生错误");
        }

        #region 跳转渤海充值

        public IActionResult WebRecharge([FromQuery]string token, [FromQuery]decimal money, [FromQuery]string orderno, [FromQuery]int userattribute)
        {
            var verifyToken = VerifyBase.Token(token, out int userId);

            //验证Token
            if (verifyToken == ReturnCode.TokenEorr)
                return Content("token错误");

            //验证开户 
            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userId);
            if (accountInfo == null || !accountInfo.BoHai || !accountInfo.JieSuan)
                return Content("尚未开户绑卡");
            var isMobile = _userAgentHelper.IsMobileDevice();

            //获取请求参数
            var sbhWebRecharge = new SBHWebRecharge
            {
                ReqSvcHeader = new SBHBaseHeaderModel
                {
                    tranSeqNo = orderno
                },
                SvcBody = new SBHWebRechargeBody
                {
                    clientType = isMobile == true ? "2" : "1",
                    //platformUid = userattribute == 1 ? accountInfo.invest_platform_id : accountInfo.financing_platform_id,
                    platformUid = accountInfo.act_business_property.Value==1? accountInfo.invest_platform_id: accountInfo.financing_platform_id,
                    plaCustId = accountInfo.cst_plaCustId,
                    transAmt = money.ToString("0.00"),
                    feeType = "0",
                    merFeeAmt = "0",
                    pageReturnUrl = _zfctWebConfig.LocalUrl + _zfctWebConfig.WebUrl.Where(p => p.Name == "WebRecharge").First().ReturnUrl
                }
            };

            //发送结算中心
            var rbhWebRecharge = _bhAccountService.WebRecharge(sbhWebRecharge);

            //Post渤海
            if (rbhWebRecharge != null && rbhWebRecharge.RspSvcHeader.returnCode == JSReturnCode.Success)
            {
                if (isMobile)
                    return Redirect($"{_boHaiApiConfig.BoHaiMobileAddress}?Transid={MobileAddressType.CBHBNetLoanRecharge}&NetLoanInfo={rbhWebRecharge.SvcBody.NetLoanInfo}");
                else
                    return RedirectToAction("RedirectBH", CommonHelper.ObjectToDictionary(rbhWebRecharge.SvcBody, true));
            }

            return Content("发生错误");
        }

        #endregion 跳转渤海充值

        #region 渤海企业开户

        public IActionResult CompanyOpenAccount([FromQuery] string token,string type="1")
        {
            VerifyBase.AnalysisToken(token, out CST_user_info userInfo);
            if (userInfo.CompanyId == null || userInfo.CompanyId.Value == 0)
            {
                return Content("该用户非企业用户");
            }
            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId: userInfo.Id);
            if (!accountInfo.JieSuan)
            {
                return Content("改用户尚未企业开户");
            }
            var chargeAccount = _companyInfoService.GetChargeAccount(companyId: userInfo.CompanyId.Value);
            var isMobile = _userAgentHelper.IsMobileDevice();
            var bohaiModel = new SBHOpenChargeAccount
            {
                SvcBody = new SBHOpenChargeAccountBody
                {
                    clientType = isMobile ? ClientType.WEB : ClientType.PC,
                    //platformUidInvestment = accountInfo.invest_platform_id,
                    //platformUidFinance = accountInfo.financing_platform_id,
                    platformUid = accountInfo.financing_platform_id,
                    txnTyp =type=="1"?TxnType.Add:TxnType.Update,
                    //txnTyp = TxnType.Update,
                    accountTyp = chargeAccount.AccountType,
                    accountNo = chargeAccount.AccountNo,
                    accountName = chargeAccount.AccountName,
                    //accountName = "非渤海企业修改二",
                    accountBk = chargeAccount.AccountBk,
                    //accountBk = "309523015014",
                    pageReturnUrl = ZfctWebEngineToConfiguration.GetWebReturnUrl("OpenCompanyAccount")
                }
            };
            var model = _bhUserService.OpenChargeAccount(bohaiModel);
            if (model == null)
                return Content("data error");
            if (model.RspSvcHeader.returnCode != JSReturnCode.Success.ToString())
                return Content(model.RspSvcHeader.returnMsg);
            chargeAccount.MerBillNo = bohaiModel.ReqSvcHeader.tranSeqNo;
            _companyInfoService.UpdateChargeAccount(chargeAccount);
            if (isMobile)
            {
                var transid = MobileAddressType.CBHBNetLoanRegisterPublic.ToString();
                //跳转至渤海手机端
                var url = BoHaiApiEngineToConfiguration.BHMobileAddress() + "?Transid=" + transid + "&NetLoanInfo=" +
                          model.SvcBody.NetLoanInfo;
                return Redirect(url);
            }
            return View(model.SvcBody);
        }

        #endregion

        #region 用户账户授权

        public IActionResult AutoInvestAuth([FromQuery] string token,string authType="1")
        {
            VerifyBase.AnalysisToken(token, out CST_user_info userInfo);
            //获取用户账户信息
            if (userInfo.cst_account_id == null)
            {
                return Content("签名error");
            }
            var accountInfo = _accountInfoService.Find(userInfo.cst_account_id.Value);
            //判断用户是否已经在结算中心注册
            if (!accountInfo.JieSuan)
            {
                return Content("尚未结算注册");
            }
            //判断用户是否已经在渤海开户
            if (!accountInfo.BoHai)
            {
                return Content("尚未渤海开户");
            }
            var isMobile = _userAgentHelper.IsMobileDevice();
            var bohaiModel = new SBHAutoInvestAuth()
            {
                SvcBody = new SBHAutoInvestAuthBody
                {
                    clientType = isMobile ? "2" : "1",
                    plaCustId = accountInfo.cst_plaCustId,
                    txnTyp= authType,
                    pageReturnUrl = ZfctWebEngineToConfiguration.GetWebReturnUrl("AutoInvestAuth"),
                    transTyp="1"
                }
            };
            if (accountInfo.act_business_property.Value != 1)
            {
                bohaiModel.SvcBody.transTyp = "2";
            }

            var model = _bhUserService.AutoInvestAuth(bohaiModel);

            if (model == null)
                return Content("授权error");
            //插入授权信息
            var authInfo = _accountInfoService.GetUserAuthorized(accountId: accountInfo.Id);
            if (authInfo == null)
            {
                var newAuth = new UserAuthorized
                {
                    AccountId = accountInfo.Id,
                    PlanCustId = accountInfo.cst_plaCustId,
                    MerBillNo = bohaiModel.ReqSvcHeader.tranSeqNo,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                _accountInfoService.AddUserAuth(newAuth);
            }
            else
            {
                authInfo.MerBillNo = bohaiModel.ReqSvcHeader.tranSeqNo;
                authInfo.UpdateTime = DateTime.Now;
                _accountInfoService.UpdateUserAuth(authInfo);
            }
            if (isMobile)
            {
                var type = MobileAddressType.CBHBNetLoanAuthTrs.ToString();
                //跳转至渤海手机端
                var url = BoHaiApiEngineToConfiguration.BHMobileAddress() + "?Transid=" + type + "&NetLoanInfo=" +
                          model.NetLoanInfo;
                return Redirect(url);
            }
            return View(model);
        }

        #endregion

        #region 批量帮助用户结算中心注册

        public IActionResult BatchRegistration()
        {

            return Content("success");
        }

        #endregion



    }
}