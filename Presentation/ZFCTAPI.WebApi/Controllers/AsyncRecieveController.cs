using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Data.BoHai.ReturnModels;
using ZFCTAPI.Data.RabbitMQ;
using ZFCTAPI.Services.RabbitMQ;
using ZFCTAPI.Services.UserInfo;
using ZFCTAPI.Services.Transaction;
using ZFCTAPI.Services.BoHai;
using ZFCTAPI.Core;
using ZFCTAPI.Services.LoanInfo;
using ZFCTAPI.Services.InvestInfo;
using ZFCTAPI.Services.Repayment;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Data.BoHai.SubmitModels;
using ZFCTAPI.Data.WeChat;
using ZFCTAPI.Services.Helpers;
using ZFCTAPI.Services.Sys;
using ZFCTAPI.Services.Messages;
using ZFCTAPI.Data.BoHai.TransferData;

namespace ZFCTAPI.WebApi.Controllers
{
    /// <summary>
    /// 渤海回调
    /// </summary>
    public class AsyncRecieveController : Controller
    {
        #region fields ctor

        private readonly IRabbitMQEvent _rabbitMQEvent;
        private readonly ICstUserInfoService _userInfoService;
        private readonly IAccountInfoService _accountInfoService;
        private readonly ICstTransactionService _cstTransactionService;
        private readonly IBHAccountService _iBHAccountService;
        private readonly ILoanInfoService _iloanInfoService;
        private readonly ILoanPlanService _iloanPlanService;
        private readonly ICapitalTransferService _capitalTransferService;
        private readonly IBHUserService _iBHUserService;
        private readonly ICompanyInfoService _companyInfoService;
        private readonly ICustomerService _customerService;
        private readonly ItbWechatService _itbWechatService;
        private readonly ICST_suspicious_transactionService _cST_Suspicious_TransactionService;
        private readonly IUserAgentHelper _userAgentHelper;
        private readonly ZfctWebConfig _zfctWebConfig;
        private readonly IInvestInfoService _iinvestInfoService;
        private readonly IMessageNoticeService _messageNoticeService;

        public AsyncRecieveController(IRabbitMQEvent rabbitMQEvent,
            ICstUserInfoService userInfoService,
            IAccountInfoService accountInfoService,
            ICstTransactionService cstTransactionService,
            IBHAccountService iBHAccountService,
            ILoanPlanService iloanPlanService,
            ILoanInfoService iloanInfoService,
            ICapitalTransferService capitalTransferService,
            IBHUserService iBHUserService,
            ICompanyInfoService companyInfoService,
            ICustomerService customerService,
            ItbWechatService itbWechatService,
            ICST_suspicious_transactionService cST_Suspicious_TransactionService,
            IUserAgentHelper userAgentHelper,
            ZfctWebConfig zfctWebConfig,
            IInvestInfoService iinvestInfoService,
            IMessageNoticeService messageNoticeService)
        {
            _rabbitMQEvent = rabbitMQEvent;
            _userInfoService = userInfoService;
            _accountInfoService = accountInfoService;
            _cstTransactionService = cstTransactionService;
            _iBHAccountService = iBHAccountService;
            _iloanPlanService = iloanPlanService;
            _iloanInfoService = iloanInfoService;
            _capitalTransferService = capitalTransferService;
            _iBHUserService = iBHUserService;
            _companyInfoService = companyInfoService;
            _customerService = customerService;
            _itbWechatService = itbWechatService;
            _cST_Suspicious_TransactionService = cST_Suspicious_TransactionService;
            _userAgentHelper = userAgentHelper;
            _zfctWebConfig = zfctWebConfig;
            _iinvestInfoService = iinvestInfoService;
            _messageNoticeService = messageNoticeService;
        }

        #endregion

        #region 异步接收渤海返回信息

        public ActionResult Index([FromBody]RBHAsyncRecieve model)
        {
            var jsonResult = JsonConvert.SerializeObject(model);
            LogsHelper.WriteLog(jsonResult);
            var type = model.bizType;

            switch (type)
            {
                case "RealNameWebResult"://注册绑卡
                    return RedirectToAction("AsyncRealNameWeb", model);

                case "DrawingsResult"://提现
                    return RedirectToAction("AysncDrawings", model);

                case "WebRechargeResult"://充值
                    return RedirectToAction("AysncWebRecharge", model);

                case "BindCardWebResult"://修改绑定银行卡
                    return RedirectToAction("AsyncBindCardWeb", model);

                case "BindMobileNoResult"://修改绑定手机号
                    return RedirectToAction("AsyncBindMobileNo", model);

                case "BindPassResult"://修改密码
                    return RedirectToAction("AsyncUserBindPass", model);

                case "FileRelease"://放款通知
                    return RedirectToAction("AsyncRelease", model);

                case "FileRepayment"://还款通知
                    return RedirectToAction("AsyncRepayment", model);

                case "BatInvestCancle":
                    return RedirectToAction("AsyncBatInvestCancle", model);

                case "submitCreditProjectResult"://新增投资
                    return RedirectToAction("AysncSubmitCreditProjectResult", model);

                case "OpenChargeAccountWebResult":
                    return RedirectToAction("AsyncOpenChargeAccount", model);

                case "UserAddResult":  //结算中心注册通知
                    return RedirectToAction("AsyncUserAddResult", model);

                case "CloseAccountResult":  //结算中心注册通知
                    return RedirectToAction("AsyncCloseAccount", model);
            }
            return Content("Success");
        }

        #endregion 异步接收渤海返回信息

        #region 销户
        /// <summary>
        /// 用户销户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AsyncCloseAccount(RBHAsyncCloseAccount model)
        {
            var jsonResult = JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.CallbackParameters.ToString(),
                ITF_req_parameters = jsonResult,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_CloseAccount.ToString()
            });

            //不做操作
            return Content("Success");
        }
        #endregion

        #region 接受结算开户回调

        public ActionResult AsyncUserAddResult(RBHAsyncUserAdd model)
        {
            if (model?.respCode == null)
                return Content("model is null");
            if (model.respCode == BHReturnCode.Success.ToString())
            {
                var accountInfo = _accountInfoService.GetAccountInfoByUserId(formId: model.platformUid);
                if (accountInfo != null && accountInfo.act_user_id != null)
                {
                    var userInfo = _userInfoService.GetUserInfo(id: accountInfo.act_user_id.Value);
                    if (userInfo != null)
                    {
                        accountInfo.JieSuan = true;
                        accountInfo.JieSuanCode = model.respCode;
                        accountInfo.JieSuanMsg = model.respDesc;
                        accountInfo.JieSuanTime = DateTime.Now;
                        _accountInfoService.Update(accountInfo);
                    }
                }

            }
            else
            {
                var accountInfo = _accountInfoService.GetAccountInfoByUserId(formId: model.platformUid);
                if (accountInfo != null && accountInfo.act_user_id != null)
                {
                    var userInfo = _userInfoService.GetUserInfo(id: accountInfo.act_user_id.Value);
                    if (userInfo != null)
                    {
                        accountInfo.JieSuanCode = model.respCode;
                        accountInfo.JieSuanMsg = model.respDesc;
                        _accountInfoService.Update(accountInfo);
                    }
                }
            }
            return Content("Success");
        }

        #endregion

        #region 接收渤海开户开卡回调

        /// <summary>
        /// 页面跳转
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult RealNameWeb(RBHAsyncRealNameWeb model)
        {
            #region 返回结果json化保存

            var jsonResult = JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.CallbackParameters.ToString(),
                ITF_req_parameters = jsonResult,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_RealNameWeb.ToString()
            });

            #endregion 返回结果json化保存

            if (model == null || (string.IsNullOrEmpty(model.respCode) && string.IsNullOrEmpty(model.rpCode)))
                return Content("model is null");
            var msg = "";
            if ((!string.IsNullOrEmpty(model.respCode) && model.respCode == BHReturnCode.Success.ToString()) || (!string.IsNullOrEmpty(model.rpCode) && model.rpCode == BHReturnCode.Success.ToString()))
            {
                msg = "开户成功";
                model.merPriv = model.merPriv.Split("*")[0];
                //获取用户账户信息
                var accountInfo = _accountInfoService.GetAccountInfoByUserId(formId: model.merPriv);
                if (!accountInfo.BoHai)
                {
                    accountInfo.cst_plaCustId = model.plaCustId;
                    accountInfo.BoHai = true;
                    accountInfo.BhCode = model.respCode;
                    accountInfo.BhMsg = model.respDesc;
                    _accountInfoService.Update(accountInfo);
                    //查询个人线下充值账户
                    var queryModel = new SBHQueryChargeAccount
                    {
                        SvcBody =
                        {
                            platformUid = accountInfo.invest_platform_id
                        }
                    };
                    var info = _iBHUserService.QueryChargeAccount(queryModel);
                    if (info != null)
                    {
                        accountInfo.personal_charge_account = info.chargeAccount;
                        _accountInfoService.Update(accountInfo);
                    }
                }
            }
            else
            {
                msg = "开户失败,返回页面查看原因";
                model.merPriv = model.merPriv.Split("*")[0];
                //获取用户账户信息
                var accountInfo = _accountInfoService.GetAccountInfoByUserId(formId: model.merPriv);
                if (_userAgentHelper.IsMobileDevice())
                {
                    accountInfo.BhCode = model.rpCode;
                    accountInfo.BhMsg = model.rpDesc;
                }
                else
                {
                    accountInfo.BhCode = model.respCode;
                    accountInfo.BhMsg = model.respDesc;
                }

                _accountInfoService.Update(accountInfo);
            }
            if (_userAgentHelper.IsApp())
            {
                return RedirectToAction("DefaultPage", "App", new { msg = msg, type = ReturnPageType.OpenAccount });
            }
            else if (_userAgentHelper.IsWeChat())
            {
                return Redirect($"{_zfctWebConfig.WeChatUrl}/MyAccount/BankDepository?msg={WebUtility.UrlEncode(msg)}");
            }
            else
            {
                //pc
                return Redirect($"{_zfctWebConfig.PcUrl}/MyAccount/BankCardManage?msg={WebUtility.UrlEncode(msg)}");
            }
        }

        /// <summary>
        /// 异步结果处理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AsyncRealNameWeb(RBHAsyncRealNameWeb model)
        {
            if (model?.respCode == null)
            {
                return Content("model is null");
            }
            #region 返回结果json化保存

            var jsonResult = JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.CallbackParameters.ToString(),
                ITF_req_parameters = jsonResult,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_RealNameWeb.ToString()
            });

            #endregion 返回结果json化保存


            if (model.respCode == BHReturnCode.Success.ToString())
            {
                model.merPriv = model.merPriv.Split("*")[0];
                //获取用户账户信息
                var accountInfo = _accountInfoService.GetAccountInfoByUserId(formId: model.merPriv);
                if (!accountInfo.BoHai)
                {
                    accountInfo.cst_plaCustId = model.plaCustId;
                    accountInfo.BoHai = true;
                    accountInfo.BhCode = model.respCode;
                    accountInfo.BhMsg = model.respDesc;
                    _accountInfoService.Update(accountInfo);
                    //查询个人线下充值账户
                    var queryModel = new SBHQueryChargeAccount
                    {
                        SvcBody =
                        {
                            platformUid = accountInfo.invest_platform_id
                        }
                    };
                    var info = _iBHUserService.QueryChargeAccount(queryModel);
                    if (info != null)
                    {
                        accountInfo.personal_charge_account = info.chargeAccount;
                        _accountInfoService.Update(accountInfo);
                    }
                }
                return Content("Success");
            }
            else
            {
                model.merPriv = model.merPriv.Split("*")[0];
                //获取用户账户信息
                var accountInfo = _accountInfoService.GetAccountInfoByUserId(formId: model.merPriv);
                accountInfo.BhCode = model.respCode;
                accountInfo.BhMsg = model.respDesc;
                _accountInfoService.Update(accountInfo);
                return Content("error：" + model.respCode);
            }

        }

        #endregion 接收渤海开户开卡回调

        #region 接收修改绑定银行卡回调

        /// <summary>
        /// 页面跳转
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult BindCardWeb(RBHAsyncBindCardWeb model)
        {
            #region 返回结果json化保存

            var jsonResult = JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.PageReturnParameters.ToString(),
                ITF_req_parameters = jsonResult,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_BindCardWeb.ToString()
            });
            var msg = "";
            try
            {
                if (string.IsNullOrEmpty(model.respCode) && string.IsNullOrEmpty(model.rpCode))
                {
                    msg = "银行卡绑定处理中";
                }
                else
                {
                    if ((!string.IsNullOrEmpty(model.respCode) && model.respCode == BHReturnCode.Success.ToString()) || (!string.IsNullOrEmpty(model.rpCode) && model.rpCode == BHReturnCode.Success.ToString()))
                    {
                        msg = "银行卡绑定成功";
                    }
                    else
                    {
                        msg = "银行卡绑定失败";
                    }
                }
            }
            catch
            {
                msg = "银行卡绑定处理中";
            }
            if (_userAgentHelper.IsApp())
            {
                return RedirectToAction("DefaultPage", "App", new { msg = msg, type = ReturnPageType.BindCard });
            }
            else if (_userAgentHelper.IsWeChat())
            {
                return Redirect($"{_zfctWebConfig.WeChatUrl}/MyAccount/BankCardManagement?msg={WebUtility.UrlEncode(msg)}");
            }
            else
            {
                //pc
                msg = WebUtility.UrlEncode(msg);
                return Redirect($"{_zfctWebConfig.PcUrl}/MyAccount/BankCardManageNext?msg={WebUtility.UrlEncode(msg)}");
            }
            #endregion 返回结果json化保存
        }

        /// <summary>
        /// 异步结果处理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AsyncBindCardWeb(RBHAsyncBindCardWeb model)
        {
            #region 返回结果json化保存

            var jsonResult = JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.CallbackParameters.ToString(),
                ITF_req_parameters = jsonResult,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_BindCardWeb.ToString()
            });

            #endregion 返回结果json化保存
            try
            {
                if (model.respCode == BHReturnCode.Success.ToString())
                {
                    var queryUserInfo = _iBHUserService.QueryUserInfo(new Data.BoHai.SubmitModels.SBHQueryUserInfo
                    {
                        SvcBody = new Data.BoHai.SubmitModels.SBHQueryUserInfoBody
                        {
                            plaCustId = model.plaCustId,
                            mblNo=model.mobileNo
                            
                        }
                    });

                    if (queryUserInfo != null)
                    {
                        model.merPriv = model.merPriv.Split("*")[0];
                        var accountInfo = _accountInfoService.GetAccountInfoByUserId(formId: model.merPriv);
                        if (accountInfo != null)
                        {
                            _accountInfoService.DeleteUserBankCard(accountInfo.Id);
                            foreach (var bankinfo in queryUserInfo.items)
                            {
                                _accountInfoService.AddBankCardInfo(new Data.CST.CST_bankcard_info
                                {
                                    bank_code = bankinfo.capCorg,
                                    bank_no = bankinfo.capCrdNo,
                                    mon_account_id = accountInfo.Id,
                                    bank_datetime = DateTime.Now,
                                    IsBoHai = true
                                });
                            }
                        }

                    }
                }
            }
            catch (Exception e)
            {
                _rabbitMQEvent.Publish(new SYS_Interface_Info
                {
                    ITF_info_type = LogsEnum.CallbackParameters.ToString(),
                    ITF_req_parameters = e.ToString(),
                    ITF_Info_addtime = DateTime.Now,
                    cmdid = InterfaceName.p_BindCardWeb.ToString()
                });
            }

            return Content("Success");
        }

        #endregion 接收修改绑定银行卡回调

        #region 接收渤海对账文件通知

        /// <summary>
        ///没用了
        /// </summary>
        /// <param name="model"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public ActionResult FileNotice(RBHFileNotice model, List<IFormFile> file)
        {
            if (file.Count > 0)
            {
                var filePath = LogsHelper.AppPath() + "\\bohaiduizhang\\" + DateTime.Now.ToString("yyyy-MM-dd");
                var fileName = DateTime.Now.ToString("ddHHmmss") + model.fileName;
                var fileInfo = filePath + "\\" + fileName;
                if (!Directory.Exists(filePath))//验证路径是否存在
                {
                    //不存在则创建
                    Directory.CreateDirectory(filePath);
                }
                using (var fs = System.IO.File.Create(fileInfo))
                {
                    file[0].CopyTo(fs);
                    fs.Flush();
                }
            }
            return Content("Success");
        }

        #endregion 接收渤海对账文件通知

        #region 接收充值回调

        /// <summary>
        /// 页面返回
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult WebRecharge(RBHAsyncWebRecharge model)
        {
            #region 返回结果json化保存

            var jsonResult = JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.PageReturnParameters.ToString(),
                ITF_req_parameters = jsonResult,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_WebRecharge.ToString()
            });
            var msg = "";
            try
            {
                if (string.IsNullOrEmpty(model.respCode) && string.IsNullOrEmpty(model.rpCode))
                {
                    msg = "充值处理中";
                }
                else
                {
                    if ((model.respCode != null && model.respCode.Equals(BHReturnCode.Success)) || (model.rpCode != null && model.rpCode == BHReturnCode.Success.ToString()))
                    {
                        msg = "充值成功";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(model.respDesc))
                        {
                            msg = "充值失败:" + model.respDesc;
                        }
                        else
                        {
                            msg = "充值失败:" + model.rpDesc;
                        }
                        return RedirectToAction("DefaultPage", "App", new { msg = msg, type = ReturnPageType.RechargeFailed });
                    }
                }

            }
            catch
            {
                msg = "充值处理中";
            }
            if (_userAgentHelper.IsApp())
            {
                return RedirectToAction("DefaultPage", "App", new { msg = msg, type = ReturnPageType.Recharge });
            }
            else if (_userAgentHelper.IsWeChat())
            {
                return Redirect($"{_zfctWebConfig.WeChatUrl}/MyAccount/RechargeSuccOrFail?msg={WebUtility.UrlEncode(msg)}");
            }
            else
            {
                msg = WebUtility.UrlEncode(msg);
                return Redirect($"{_zfctWebConfig.PcUrl}/MyAccount/RechargeState?msg={WebUtility.UrlEncode(msg)}");
            }
            #endregion 返回结果json化保存
        }

        /// <summary>
        /// 异步结果处理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AysncWebRecharge(RBHAsyncWebRecharge model)
        {
            #region 返回结果json化保存

            var jsonResult = JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.CallbackParameters.ToString(),
                ITF_req_parameters = jsonResult,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_WebRecharge.ToString()
            });

            #endregion 返回结果json化保存

            try
            {
                if (model.respCode.Equals(BHReturnCode.Success))
                {
                    //更新交易记录
                    var transactions = _cstTransactionService.GetListByOrderNo(model.merBillNo);
                    if (transactions.Any())
                    {
                        var transaction = transactions.FirstOrDefault();
                        if (transaction.pro_transaction_status != DataDictionary.transactionstatus_success)
                        {
                            var accountInfo = _accountInfoService.GetAccountInfoByUserId(transaction.pro_user_id.Value);
                            var iAccBalance = _iBHAccountService.AccountQueryAccBalance(new SBHAccountQueryAccBalance
                            {
                                SvcBody = new SBHAccountQueryAccBalanceBody
                                {
                                    platformUid = accountInfo.invest_platform_id
                                }
                            });
                            var fAccBalance = _iBHAccountService.AccountQueryAccBalance(new SBHAccountQueryAccBalance
                            {
                                SvcBody = new SBHAccountQueryAccBalanceBody
                                {
                                    platformUid = accountInfo.financing_platform_id
                                }
                            });
                            transaction.pro_transaction_money = Convert.ToDecimal(model.transAmt);
                            transaction.pro_complete_time = DateTime.Now;
                            transaction.pro_transaction_remarks = model.respDesc + model.respCode;
                            transaction.pro_transaction_status = DataDictionary.transactionstatus_success;
                            if (iAccBalance.RspSvcHeader.returnCode.Equals(JSReturnCode.Success))
                            {
                                transaction.pro_balance_money = decimal.Parse(iAccBalance.SvcBody.withdrawAmount);
                            }
                            if (fAccBalance.RspSvcHeader.returnCode.Equals(JSReturnCode.Success))
                            {
                                transaction.pro_finance_money = decimal.Parse(fAccBalance.SvcBody.withdrawAmount);
                            }
                            _cstTransactionService.Update(transaction);

                            //发送微信消息
                            _itbWechatService.SendReChargeNotice(_userInfoService.Find(transaction.pro_user_id.Value), new Dictionary<string, MessageData> {
                                    { "keyword1",new MessageData{ value ="充值" } },
                                    { "keyword2",new MessageData{ value =transaction.pro_transaction_time.GetValueOrDefault().ToString("MM月dd日 HH:mm")} },
                                    { "keyword3",new MessageData{ value ="+"+string.Format("{0:N}",transaction.pro_transaction_money.GetValueOrDefault()),color="#FF0000"} }
                                });
                            //危险交易
                            if (transaction.pro_transaction_money.GetValueOrDefault() > 50000)
                            {
                                _cST_Suspicious_TransactionService.Add(new CST_suspicious_transaction
                                {
                                    pro_user_id = transaction.pro_user_id.Value,
                                    pro_transaction_id = transaction.Id,
                                    suspicious_transaction_type = (int)SuspiciousTransactionType.LargeRecharge,
                                    suspicious_transaction_description = "单笔充值金额大于5万",
                                    pro_create_time = DateTime.Now
                                });
                            }
                            if (_cstTransactionService.ExistSuspiciousTransactionInfo(transaction))
                            {
                                _cST_Suspicious_TransactionService.Add(new CST_suspicious_transaction
                                {
                                    pro_user_id = transaction.pro_user_id.Value,
                                    pro_transaction_id = transaction.Id,
                                    suspicious_transaction_type = (int)SuspiciousTransactionType.RepeatTrade,
                                    suspicious_transaction_description = "3个工作日内交易金额超过10万",
                                    pro_create_time = DateTime.Now
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _rabbitMQEvent.Publish(new SYS_Interface_Info
                {
                    ITF_info_type = LogsEnum.CallbackParameters.ToString(),
                    ITF_req_parameters = e.ToString(),
                    ITF_Info_addtime = DateTime.Now,
                    cmdid = InterfaceName.p_WebRecharge.ToString()
                });
            }

            return Content("Success");
        }

        #endregion 接收充值回调

        #region 接收提现回调

        /// <summary>
        /// 页面返回
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Drawings(RBHAsyncDrawings model)
        {
            #region 返回结果json化保存

            var jsonResult = JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.PageReturnParameters.ToString(),
                ITF_req_parameters = jsonResult,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_Drawings.ToString()
            });

            #endregion 返回结果json化保存
            var msg = "";
            try
            {
                if (string.IsNullOrEmpty(model.respCode) && string.IsNullOrEmpty(model.rpCode))
                {
                    msg = "提现处理中";
                }
                else
                {

                    if ((model.respCode != null && model.respCode.Equals(BHReturnCode.Success)) || (model.rpCode != null && model.rpCode == BHReturnCode.Success.ToString()))
                    {
                        msg = "提现成功";
                    }
                    else
                    {
                        msg = "提现失败:" + model.respDesc;
                        return RedirectToAction("DefaultPage", "App", new { msg = msg, type = ReturnPageType.WithdrawFailed });
                    }
                }

            }
            catch
            {
                msg = "提现处理中";
            }
            if (_userAgentHelper.IsApp())
            {
                return RedirectToAction("DefaultPage", "App", new { msg = msg, type = ReturnPageType.Withdraw });
            }
            else if (_userAgentHelper.IsWeChat())
            {
                return Redirect($"{_zfctWebConfig.WeChatUrl}/MyAccount/WithdrawSuccOrFail?msg={ WebUtility.UrlEncode(msg)}");
            }
            else
            {
                //pc
                return Redirect($"{_zfctWebConfig.PcUrl}/MyAccount/WidtdrawState?msg={WebUtility.UrlEncode(msg)}");

            }
        }

        /// <summary>
        /// 异步结果处理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AysncDrawings(RBHAsyncDrawings model)
        {
            #region 返回结果json化保存

            var jsonResult = JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.CallbackParameters.ToString(),
                ITF_req_parameters = jsonResult,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_Drawings.ToString()
            });

            #endregion 返回结果json化保存

            try
            {
                if (model.respCode.Equals(BHReturnCode.Success))
                {
                    //更新交易记录
                    var transactions = _cstTransactionService.GetListByOrderNo(model.merBillNo);
                    if (transactions.Any())
                    {
                        var transaction = transactions.FirstOrDefault();
                        if (transaction.pro_transaction_status != DataDictionary.transactionstatus_success)
                        {
                            var accountInfo = _accountInfoService.GetAccountInfoByUserId(transaction.pro_user_id.Value);
                            var iAccBalance = _iBHAccountService.AccountQueryAccBalance(new SBHAccountQueryAccBalance
                            {
                                SvcBody = new SBHAccountQueryAccBalanceBody
                                {
                                    platformUid = accountInfo.invest_platform_id
                                }
                            });
                            var fAccBalance = _iBHAccountService.AccountQueryAccBalance(new SBHAccountQueryAccBalance
                            {
                                SvcBody = new SBHAccountQueryAccBalanceBody
                                {
                                    platformUid = accountInfo.financing_platform_id
                                }
                            });
                            transaction.pro_transaction_money = Convert.ToDecimal(model.transAmt);
                            transaction.pro_complete_time = DateTime.Now;
                            transaction.pro_transaction_remarks = model.respDesc + model.respCode;
                            transaction.pro_transaction_status = DataDictionary.transactionstatus_success;
                            if (iAccBalance.RspSvcHeader.returnCode.Equals(JSReturnCode.Success))
                            {
                                transaction.pro_balance_money = decimal.Parse(iAccBalance.SvcBody.withdrawAmount);
                            }
                            if (fAccBalance.RspSvcHeader.returnCode.Equals(JSReturnCode.Success))
                            {
                                transaction.pro_finance_money = decimal.Parse(fAccBalance.SvcBody.withdrawAmount);
                            }
                            _cstTransactionService.Update(transaction);
                            //发送微信消息
                            var userInfo = _userInfoService.Find(transaction.pro_user_id.Value);
                            _itbWechatService.SendWithdrawalsNotice(userInfo, new Dictionary<string, MessageData> {
                                    { "keyword1",new MessageData{ value ="提现" } },
                                    { "keyword2",new MessageData{ value =transaction.pro_transaction_time.GetValueOrDefault().ToString("MM月dd日 HH:mm")} },
                                    { "keyword3",new MessageData{ value =string.Format("{0:N}",transaction.pro_transaction_money.GetValueOrDefault()),color="#00CC00"} }
                                });
                            //发送短信
                            _messageNoticeService.SendWithdrawalMsg(Convert.ToDecimal(model.transAmt).ToString("0.00"), userInfo.cst_user_phone);
                            //危险交易
                            if (transaction.pro_transaction_money.GetValueOrDefault() > 50000)
                            {
                                _cST_Suspicious_TransactionService.Add(new CST_suspicious_transaction
                                {
                                    pro_user_id = transaction.pro_user_id.Value,
                                    pro_transaction_id = transaction.Id,
                                    suspicious_transaction_type = (int)SuspiciousTransactionType.LargeWithdraw,
                                    suspicious_transaction_description = "单笔提现金额大于5万",
                                    pro_create_time = DateTime.Now
                                });
                            }
                            if (_cstTransactionService.ExistSuspiciousTransactionInfo(transaction))
                            {
                                _cST_Suspicious_TransactionService.Add(new CST_suspicious_transaction
                                {
                                    pro_user_id = transaction.pro_user_id.Value,
                                    pro_transaction_id = transaction.Id,
                                    suspicious_transaction_type = (int)SuspiciousTransactionType.RepeatTrade,
                                    suspicious_transaction_description = "3个工作日内交易金额超过10万",
                                    pro_create_time = DateTime.Now
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _rabbitMQEvent.Publish(new SYS_Interface_Info
                {
                    ITF_info_type = LogsEnum.CallbackParameters.ToString(),
                    ITF_req_parameters = e.ToString(),
                    ITF_Info_addtime = DateTime.Now,
                    cmdid = InterfaceName.p_Drawings.ToString()
                });
            }

            return Content("Success");
        }

        #endregion 接收提现回调

        #region 放还款通知

        /// <summary>
        /// 还款通知(还款明细异步回调)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AsyncRepayment(RBHRepayment model)
        {
            var jsonReuslt = JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.CallbackParameters.ToString(),
                ITF_req_parameters = jsonReuslt,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_repaymentExecuteEx.ToString()
            });

            if (model.respCode == BHReturnCode.Success)
            {
                _capitalTransferService.RepaymentSuccess(model.merBillNo, model.respDesc);
            }
            else
            {
                _capitalTransferService.RepaymentFail(model.merBillNo, model.respDesc);
            }

            return Ok();
        }

        /// <summary>
        /// 放款通知(满标划转异步回调)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AsyncRelease(RBHRepayment model)
        {
            var jsonReuslt = JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.CallbackParameters.ToString(),
                ITF_req_parameters = jsonReuslt,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_raiseResultNoticeEx.ToString()
            });

            if (model.respCode == BHReturnCode.Success)
            {
                _capitalTransferService.ReleaseSuccess(model.merBillNo, model.respDesc);
            }
            else
            {
                _capitalTransferService.ReleaseFail(model.merBillNo, model.respDesc);
            }

            return Ok();
        }

        /// <summary>
        /// 流标异步回调
        /// </summary>
        /// <returns></returns>
        public ActionResult AsyncBatInvestCancle(RBHRepayment model)
        {
            var jsonReuslt = JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.CallbackParameters.ToString(),
                ITF_req_parameters = jsonReuslt,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_BatInvestCancle.ToString()
            });

            var investInfo = _iinvestInfoService.GetInvestInfoByMerBillNo(model.merBillNo);
            if (investInfo != null && investInfo.Id > 0)
            {
                //单个解冻投资金额
                if (model.respCode == BHReturnCode.Success)
                {
                    _capitalTransferService.CancelInvest(model.merBillNo);
                }
            }
            else
            {
                //流标
                if (model.respCode == BHReturnCode.Success)
                {
                    _capitalTransferService.CancelSuccess(model.merBillNo, model.respDesc);
                }
                else
                {
                    _capitalTransferService.CancelFail(model.merBillNo, model.respDesc);
                }
            }

            return Ok();
        }

        #endregion

        #region 接收修改手机号回调

        /// <summary>
        /// 页面跳转
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult BindMobileNo(RBHAsyncBindMobileNo model)
        {
            #region 返回结果json化保存

            var jsonResult = "修改手机号回调,页面跳转" + JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.PageReturnParameters.ToString(),
                ITF_req_parameters = jsonResult,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_BindMobileNo.ToString()
            });
            #endregion 返回结果json化保存

            var msg = "";
            try
            {
                if (string.IsNullOrEmpty(model.respCode) && string.IsNullOrEmpty(model.rpCode))
                {
                    msg = "修改手机号处理中";
                }
                else
                {
                    if ((model.respCode != null && model.respCode.Equals(BHReturnCode.Success)) || (model.rpCode != null && model.rpCode == BHReturnCode.Success.ToString()))
                    {
                        msg = "修改手机号成功";
                    }
                    else
                    {
                        if (_userAgentHelper.IsMobileDevice())
                        {
                            msg = "修改手机号失败:" + model.rpDesc;
                        }
                        else
                        {
                            msg = "修改手机号失败:" + model.respDesc;
                        }

                    }
                }
            }
            catch
            {
                msg = "修改手机号处理中";
            }
            if (_userAgentHelper.IsApp())
            {
                return RedirectToAction("DefaultPage", "App", new { msg = msg, type = ReturnPageType.BindMobileNo });
            }
            else if (_userAgentHelper.IsWeChat())
            {
                return Redirect($"{_zfctWebConfig.WeChatUrl}/MyAccount/BankDepository?msg={WebUtility.UrlEncode(msg)}");
            }
            else
            {
                //pc
                return Redirect($"{_zfctWebConfig.PcUrl}/MyAccount/BankCardManage?msg={WebUtility.UrlEncode(msg)}");

            }
        }

        /// <summary>
        /// 异步结果处理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AsyncBindMobileNo(RBHAsyncBindMobileNo model)
        {
            #region 返回结果json化保存

            var jsonResult = JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.CallbackParameters.ToString(),
                ITF_req_parameters = jsonResult,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_BindMobileNo.ToString()
            });

            #endregion 返回结果json化保存

            //用户修改手机号之后我们就拿不到真实的银行卡了 这就非常的尴尬导致我们只能把渤海加*之后的银行卡保存到我们的数据里
            if (model.respCode == BHReturnCode.Success.ToString())
            {
                var accountInfo = _accountInfoService.GetAccountInfoByUserId(platId: model.plaCustId);
                if (accountInfo != null)
                {
                    //获取用户账户信息
                    var queryUserInfo = _iBHUserService.QueryUserInfo(new Data.BoHai.SubmitModels.SBHQueryUserInfo
                    {
                        SvcBody = new SBHQueryUserInfoBody
                        {
                            plaCustId = model.plaCustId,
                            mblNo=model.mobileNo
                        }
                    });
                    //银行卡重新绑定
                    if (queryUserInfo != null)
                    {
                        _accountInfoService.DeleteUserBankCard(accountInfo.Id);
                        foreach (var bankinfo in queryUserInfo.items)
                        {
                            _accountInfoService.AddBankCardInfo(new Data.CST.CST_bankcard_info
                            {
                                bank_code = bankinfo.capCorg,
                                bank_no = bankinfo.capCrdNo,
                                mon_account_id = accountInfo.Id,
                                bank_datetime = DateTime.Now,
                                IsBoHai = true
                            });
                        }
                    }

                    if (!string.IsNullOrEmpty(model.mobileNo))
                    {

                        accountInfo.act_user_phone = model.mobileNo;
                        _accountInfoService.Update(accountInfo);
                    }
                }

            }
            return Content("Success");
        }

        #endregion

        #region 接收修改、找回支付密码通知回调

        /// <summary>
        /// 页面返回结果
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult UserBindPass(RBHAsyncBindPass model)
        {
            #region 返回结果json化保存

            var jsonResult = "修改手机号回调,页面返回结果" + JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.PageReturnParameters.ToString(),
                ITF_req_parameters = jsonResult,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_BindPass.ToString()
            });

            #endregion 返回结果json化保存
            var msg = "";
            try
            {
                if (string.IsNullOrEmpty(model.respCode) && string.IsNullOrEmpty(model.rpCode))
                {
                    msg = "修改交易密码处理中";
                }
                else
                {
                    if ((model.respCode != null && model.respCode.Equals(BHReturnCode.Success)) || (model.rpCode != null && model.rpCode == BHReturnCode.Success.ToString()))
                    {
                        msg = "修改交易密码成功";
                    }
                    else
                    {
                        if (_userAgentHelper.IsMobileDevice())
                        {
                            msg = "修改交易密码失败:" + model.rpDesc;
                        }
                        else
                        {
                            msg = "修改交易密码失败:" + model.respDesc;
                        }

                    }
                }
            }
            catch
            {
                msg = "修改交易密码处理中";
            }
            if (_userAgentHelper.IsApp())
            {
                return RedirectToAction("DefaultPage", "App", new { msg = msg, type = ReturnPageType.BindPass });
            }
            else if (_userAgentHelper.IsWeChat())
            {
                return Redirect($"{_zfctWebConfig.WeChatUrl}/MyAccount/BankDepository?msg={WebUtility.UrlEncode(msg)}");
            }
            else
            {
                //pc
                return Redirect($"{_zfctWebConfig.PcUrl}/MyAccount/BankCardManage?msg={WebUtility.UrlEncode(msg)}");
            }
        }

        /// <summary>
        /// 异步结果处理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AsyncUserBindPass(RBHAsyncBindPass model)
        {
            #region 返回结果json化保存

            var jsonResult = JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.CallbackParameters.ToString(),
                ITF_req_parameters = jsonResult,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_BindPass.ToString()
            });

            #endregion 返回结果json化保存

            //不做操作
            return Content("Success");
        }

        #endregion

        #region 接收新增、修改项目通知回调
        /// <summary>
        /// 异步结果处理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AysncSubmitCreditProjectResult(RBHAsyncSubmitCreditProject model)
        {
            #region 返回结果json化保存
            var jsonResult = JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.CallbackParameters.ToString(),
                ITF_req_parameters = jsonResult,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_submitCreditProject.ToString()
            });

            #endregion 返回结果json化保存

            try
            {
                var loanInfo = _iloanInfoService.Find(Convert.ToInt16(model.projectCode));
                if (model.respCode.Equals(BHReturnCode.Success))
                {
                    loanInfo.pro_loan_state = DataDictionary.projectstate_hfpass;
                }
                else
                {
                    loanInfo.pro_loan_state = DataDictionary.projectstate_hfrefuse;
                }
                _iloanInfoService.Update(loanInfo);
            }
            catch (Exception e)
            {
                _rabbitMQEvent.Publish(new SYS_Interface_Info
                {
                    ITF_info_type = LogsEnum.CallbackParameters.ToString(),
                    ITF_req_parameters = e.ToString(),
                    ITF_Info_addtime = DateTime.Now,
                    cmdid = InterfaceName.p_submitCreditProject.ToString()
                });
            }



            return Content("Success");
        }
        #endregion

        #region 接收开设对公账户通知回调
        /// <summary>
        /// 页面返回结果
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult OpenChargeAccount(RBHAsyncOpenChargeAccount model)
        {
            #region 返回结果json化保存

            var jsonResult = JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.PageReturnParameters.ToString(),
                ITF_req_parameters = jsonResult,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_OpenChargeAccount.ToString()
            });

            #endregion

            string msg = "";
            //保存异步结果数据
            if (model.respCode == BHReturnCode.Success.ToString())
            {
                msg = "开户成功";
                //获取用户信息
                var chargeInfo = _companyInfoService.GetChargeAccount(billNo: model.merBillNo);
                if (chargeInfo != null)
                {
                    if (Convert.ToDecimal(model.chargeAmt) > 100)
                    {
                        model.chargeAmt = (Convert.ToDecimal(model.chargeAmt) / 100).ToString("0.00");
                    }
                    //操作chargeInfo
                    chargeInfo.AccountBk = model.accountBk;
                    chargeInfo.Success = true;
                    chargeInfo.ChargeAccountNo = model.chargeAccount;
                    chargeInfo.CertificationMoney = Convert.ToDecimal(model.chargeAmt);
                    _companyInfoService.UpdateChargeAccount(chargeInfo);
                    //获取用户账户信息
                    var accountInfo = _accountInfoService.GetAccountInfoByCompanyId(chargeInfo.CompanyId);
                    if (accountInfo != null)
                    {
                        accountInfo.BoHai = true;
                        accountInfo.cst_plaCustId = model.plaCustId;
                        _accountInfoService.Update(accountInfo);
                    }
                    if (model.chargeAmt == "0.00")
                    {
                        //修改企业开户状态
                        chargeInfo.RealNameState = true;
                        _companyInfoService.UpdateChargeAccount(chargeInfo);
                        var companyInfo = _companyInfoService.Find(chargeInfo.CompanyId);
                        if (companyInfo != null)
                        {
                            companyInfo.AuditState = 8;
                            companyInfo.AuditDesc = "SUCCESS";
                            _companyInfoService.Update(companyInfo);
                        }
                    }
                }
            }
            else
            {
                msg = "开户失败了" + model.respDesc;
            }
            msg = WebUtility.UrlEncode(msg);
            //直接返回pc
            return Redirect($"{_zfctWebConfig.PcUrl}/Enterprise/CompanyAccount/EnterprisepBankDepository?msg={msg}");
        }

        /// <summary>
        /// 异步结果处理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AsyncOpenChargeAccount(RBHAsyncOpenChargeAccount model)
        {
            #region 返回结果json化保存

            var jsonResult = JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.PageReturnParameters.ToString(),
                ITF_req_parameters = jsonResult,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_OpenChargeAccount.ToString()
            });

            #endregion 返回结果json化保存
            //保存异步结果数据
            if (model.respCode == BHReturnCode.Success.ToString())
            {
                //获取用户信息
                if (Convert.ToDecimal(model.chargeAmt) > 100)
                {
                    model.chargeAmt = (Convert.ToDecimal(model.chargeAmt) / 100).ToString("0.00");
                }
                var chargeInfo = _companyInfoService.GetChargeAccount(billNo: model.merBillNo);
                if (chargeInfo != null)
                {
                    //操作chargeInfo
                    chargeInfo.AccountBk = model.accountBk;
                    chargeInfo.Success = true;
                    chargeInfo.ChargeAccountNo = model.chargeAccount;
                    chargeInfo.CertificationMoney = Convert.ToDecimal(model.chargeAmt);
                    _companyInfoService.UpdateChargeAccount(chargeInfo);
                    //获取用户账户信息
                    var accountInfo = _accountInfoService.GetAccountInfoByCompanyId(chargeInfo.CompanyId);
                    if (accountInfo != null)
                    {
                        accountInfo.BoHai = true;
                        accountInfo.cst_plaCustId = model.plaCustId;
                        _accountInfoService.Update(accountInfo);
                    }

                    if (model.chargeAmt == "0.00")
                    {
                        //修改企业开户状态
                        chargeInfo.RealNameState = true;
                        _companyInfoService.UpdateChargeAccount(chargeInfo);
                        var companyInfo = _companyInfoService.Find(chargeInfo.CompanyId);
                        if (companyInfo != null)
                        {
                            companyInfo.AuditState = 8;
                            companyInfo.AuditDesc = "SUCCESS";
                            _companyInfoService.Update(companyInfo);
                        }
                    }

                }
            }
            return Content("Success");
        }
        #endregion

        #region 接受授权信息

        public IActionResult AutoAuth(RBHAsyncAutoInvestAuthWeb model)
        {
            #region 返回结果json化保存

            var jsonResult = JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.CallbackParameters.ToString(),
                ITF_req_parameters = jsonResult,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_AutoInvestAuth.ToString()
            });

            #endregion 返回结果json化保存

            var msg = "";
            if (!string.IsNullOrEmpty(model.merBillNo))
            {
                msg = "授权操作处理中,以实际为准";
                try
                {
                    if ((!string.IsNullOrEmpty(model.respCode) && model.respCode == "000000") && (!string.IsNullOrEmpty(model.rpCode) && model.rpCode == "000000"))
                    {
                        var authInfo = _accountInfoService.GetUserAuthorized(mebillNo: model.merBillNo);
                        var requestModel = new SBHQueryAuthInf { SvcBody = { plaCustId = authInfo.PlanCustId } };
                        var authQuery = _iBHUserService.AuthResult(requestModel);
                        if (authQuery != null && authQuery.RspSvcHeader.returnCode.Equals(JSReturnCode.Success))
                        {
                            foreach (var auth in authQuery.SvcBody.items)
                            {
                                if (auth.end_dt.Length == 8)
                                {
                                    var endDate = CommonHelper.HandleStringTime(auth.end_dt);
                                    switch (auth.auth_typ)
                                    {
                                        case "11":
                                            authInfo.InvestAuth = true;
                                            authInfo.InvestAuthStartTime = DateTime.Now;
                                            authInfo.InvestAuthEndTime = endDate;
                                            break;
                                        case "59":
                                            authInfo.PaymentAuth = true;
                                            authInfo.PaymentAuthStartTime = DateTime.Now;
                                            authInfo.PaymentAuthEndTime = endDate;
                                            break;
                                        case "60":
                                            authInfo.RepaymentAuth = true;
                                            authInfo.RepaymentAuthStratTime = DateTime.Now;
                                            authInfo.RepaymentAuthEndTime = endDate;
                                            break;
                                    }
                                }
                            }
                            authInfo.UpdateTime = DateTime.Now;
                            _accountInfoService.UpdateUserAuth(authInfo);
                        }
                        var accountInfo = _accountInfoService.GetAccountInfoByUserId(platId: authInfo.PlanCustId);
                        if (accountInfo.act_business_property == 4 || accountInfo.act_business_property == 5)
                        {
                            //企业户
                            //直接返回pc
                            msg = WebUtility.UrlEncode(msg);
                            return Redirect(
                                $"{_zfctWebConfig.PcUrl}/Enterprise/CompanyAccount/EnterpriseAManagement?msg={msg}");
                        }
                    }
                    else
                    {
                        msg = "授权操作处理中,以实际为准";
                    }

                }
                catch
                {
                    msg = "授权操作处理中,以实际为准";
                }

            }
            else
            {
                msg = "授权操作处理中,以实际为准";
            }
            if (_userAgentHelper.IsApp())
            {
                return RedirectToAction("DefaultPage", "App", new { msg = msg, type = ReturnPageType.AutoAuth });
            }
            else if (_userAgentHelper.IsWeChat())
            {
                return Redirect($"{_zfctWebConfig.WeChatUrl}/MyAccount/Authorized?msg={WebUtility.UrlEncode(msg)}");
            }
            else
            {
                //个人pc
                return Redirect($"{_zfctWebConfig.PcUrl}/MyAccount/BankCardManageThird?msg={WebUtility.UrlEncode(msg)}");
            }
        }
        public IActionResult AsyncAutoAuth(RBHAsyncAutoInvestAuthWeb model)
        {
            #region 返回结果json化保存

            var jsonResult = JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.CallbackParameters.ToString(),
                ITF_req_parameters = jsonResult,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_AutoInvestAuth.ToString()
            });

            #endregion 返回结果json化保存
            if (!string.IsNullOrEmpty(model.merBillNo))
            {
                var authInfo = _accountInfoService.GetUserAuthorized(mebillNo: model.merBillNo);
                var requestModel = new SBHQueryAuthInf { SvcBody = { plaCustId = authInfo.PlanCustId } };
                var authQuery = _iBHUserService.AuthResult(requestModel);
                if (authQuery != null && authQuery.RspSvcHeader.returnCode.Equals(JSReturnCode.Success))
                {
                    foreach (var auth in authQuery.SvcBody.items)
                    {
                        if (auth.end_dt.Length == 8)
                        {
                            var endDate = CommonHelper.HandleStringTime(auth.end_dt);
                            switch (auth.auth_typ)
                            {
                                case "11":
                                    authInfo.InvestAuth = true;
                                    authInfo.InvestAuthStartTime = DateTime.Now;
                                    authInfo.InvestAuthEndTime = endDate;
                                    break;
                                case "59":
                                    authInfo.PaymentAuth = true;
                                    authInfo.PaymentAuthStartTime = DateTime.Now;
                                    authInfo.PaymentAuthEndTime = endDate;
                                    break;
                                case "60":
                                    authInfo.RepaymentAuth = true;
                                    authInfo.RepaymentAuthStratTime = DateTime.Now;
                                    authInfo.RepaymentAuthEndTime = endDate;
                                    break;
                            }
                        }
                    }
                    authInfo.UpdateTime = DateTime.Now;
                    _accountInfoService.UpdateUserAuth(authInfo);
                }
                return Content("200");
            }
            return Content("error");
        }
        #endregion

        #region 线下充值结果通知
        public IActionResult AsyncOfflineRecharge(RBHAsyncOfflineRecharge model)
        {
            #region 返回结果json化保存

            var jsonResult = JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.CallbackParameters.ToString(),
                ITF_req_parameters = jsonResult,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_AutoInvestAuth.ToString()
            });

            #endregion 返回结果json化保存
            if (!String.IsNullOrEmpty(model.respCode) && model.respCode.Equals(BHReturnCode.Success))
            {
                //调用充值余额同步接口



                return Content("200");
            }
            return Content("error");
        }
        #endregion

        #region 存量用户迁移
        public ActionResult AsyncExistUserRegisterNotice(UserRegisterAsyncRecieve model)
        {
            
            if (model == null)
                return Content("no data");
            var jsonResult = JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.CallbackParameters.ToString(),
                ITF_req_parameters = jsonResult,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_AutoInvestAuth.ToString()
            });
            if (model.RespCode=="000000")
            {
                var merId = model.partner_id;
                var fileCreateDate = DateTime.Now.ToString("yyyyMMdd");
                var fileName = "RESULT_" + merId + "_" + fileCreateDate + "_ExistUserRegister_" + model.BatchNo + ".txt";
                var sFilePath = LogsHelper.AppPath() + "\\BoHaiFile\\" + "\\ImportUser\\" + DateTime.Now.ToString("yyyyMMdd");
                var path = sFilePath + "\\" + fileName;
                var fileByte = FtpUploadHelper.GetFileImport(fileName);//读取结算文件服务器文件
                if (fileByte.Length > 0)
                {
                    if (!Directory.Exists(sFilePath))//验证路径是否存在
                    {
                        //不存在则创建
                        Directory.CreateDirectory(sFilePath);
                    }
                    //创建新文件若存在则删除
                    if (!System.IO.File.Exists(path))
                    {
                        var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                        fs.Seek(0, SeekOrigin.Begin);
                        fs.Write(fileByte, 0, fileByte.Length);
                        fs.Close();
                        #region 处理文件
                        HandleText(path, fileName);
                        #endregion
                    }

                }
                return Content("Success");
            }
            else
            {
                //记录错误
                return Content("error");
            }
        }
        private void HandleText(string path, string fileName)
        {
            var sR = new StreamReader(path, Encoding.GetEncoding("gb2312"));
            string nextLine;
            var importInfos = new RBHImportUserResult();
            int line = 1;
            while ((nextLine = sR.ReadLine()) != null)
            {
                var textInfo = nextLine.Split("|");
                if (line == 1)
                {
                    importInfos.partner_id = textInfo[0];
                    importInfos.BatchNo = textInfo[1];
                    importInfos.TransDate = textInfo[2];
                    importInfos.TotalNum = textInfo[3];
                }
                else
                {
                    var importInfo = new ResultUser
                    {
                        TransId = textInfo[0],
                        MobileNo = textInfo[1],
                        RespCode = textInfo[2],
                        PlaCustId = textInfo[3],
                        RespDesc = textInfo[4],
                    };
                    importInfos.ResultUsers.Add(importInfo);
                }
                line++;
            }
            if (importInfos.ResultUsers.Any())
            {
                foreach (var importInfosResultUser in importInfos.ResultUsers)
                {
                    if (importInfosResultUser.RespCode == BHReturnCode.Success)
                    {
                        //处理用户数据
                        var accountInfo = _accountInfoService.GetUserAccountInfoByPhone(importInfosResultUser.MobileNo);
                        accountInfo.cst_plaCustId = importInfosResultUser.PlaCustId;
                        accountInfo.BoHai = true;
                        accountInfo.JieSuan = true;
                        accountInfo.JieSuanTime = DateTime.Now;
                        //accountInfo.act_business_property = int.Parse(importInfosResultUser.TransTyp);//账户类型 1投资户 2 融资户
                        _accountInfoService.Update(accountInfo);
                    }
                }
            }

        }
        #endregion


        #region 存量标的迁移
        public ActionResult AsyncTransferLoan(LoanReturnModel model)
        {
            if (model == null)
                return Content("no data");
            var jsonResult = JsonConvert.SerializeObject(model);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.CallbackParameters.ToString(),
                ITF_req_parameters = jsonResult,
                ITF_Info_addtime = DateTime.Now,
                cmdid = InterfaceName.p_ExistLoanTransfer.ToString()
            });
            if (model.RespCode == "000000")
            {
                var merId = model.partner_id;
                var fileCreateDate = DateTime.Now.ToString("yyyyMMdd");
                var fileName = "RESULT_" + merId + "_" + fileCreateDate + "_FileLoanTransfer_" + model.MerBillNo + ".txt";
                //读取结算文件服务器文件
                var fileByte = FtpUploadHelper.GetFileImport(fileName);
                if (fileByte.Length > 0)
                {
                    var sFilePath = LogsHelper.AppPath() + "\\BoHaiFile\\" + "\\ImportLoan\\" + DateTime.Now.ToString("yyyyMMdd");
                    var path = sFilePath + "\\" + fileName;
                    if (!Directory.Exists(sFilePath))//验证路径是否存在
                    {
                        //不存在则创建
                        Directory.CreateDirectory(sFilePath);
                    }
                    //创建新文件若存在则删除
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                    fs.Seek(0, SeekOrigin.Begin);
                    fs.Write(fileByte, 0, fileByte.Length);
                    fs.Close();

                    //处理相关回调业务  1、更新标的信息表的Bohai字段
                    LoanRegisterHandle(path, fileName);
                }
                return Content("success");
                
            }
            else
            {
                return Content("error");
            }
        }
        /// <summary>
        /// 处理回调业务，更新标的信息
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="fileName">文件名称</param>
        private void LoanRegisterHandle(string path, string fileName)
        {
            var sR = new StreamReader(path, Encoding.GetEncoding("gb2312"));
            string nextLine;
            var importInfos = new LoanFileReturnModel();
            int line = 1;
            while ((nextLine = sR.ReadLine()) != null)
            {
                var textInfo = nextLine.Split("|");
                if (line == 1)
                {
                    importInfos.partner_id = textInfo[0];
                    importInfos.MerBillNo = textInfo[1];
                    importInfos.BorrowId = textInfo[2];
                    importInfos.BorrowerAmt = textInfo[3];
                    importInfos.BorrCustId = textInfo[4];
                    importInfos.TotalNum = textInfo[5];
                }
                else
                {
                    var importInfo = new LoanFileDetailsReturnModel
                    {
                        id = textInfo[0]                        
                    };
                    importInfos.LoanDetailsLsit.Add(importInfo);
                }
                line++;
            }
            if (importInfos.LoanDetailsLsit.Any())
            {

                //处理用户数据
                var loanInfo = _iloanInfoService.GetLoanInfoByID(importInfos.BorrowId);
                loanInfo.Bohai = true;
                _iloanInfoService.Update(loanInfo);
            }

        }

        #endregion

    }
}