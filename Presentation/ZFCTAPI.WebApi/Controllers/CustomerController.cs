using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ZFCTAPI.Core;
using ZFCTAPI.Core.Caching;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Core.Security;
using ZFCTAPI.Data.ApiModels;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Data.BoHai.SubmitModels;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.Customers;
using ZFCTAPI.Services.BoHai;
using ZFCTAPI.Services.Popular;
using ZFCTAPI.Services.UserInfo;
using ZFCTAPI.WebApi.RequestAttribute;
using ZFCTAPI.WebApi.Validates;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.Infrastructure;
using ZFCTAPI.Services.Messages;
using ZFCTAPI.Data.SYS;
using ZFCTAPI.Services.Sys;
using ZFCTAPI.Services.RabbitMQ;
using ZFCTAPI.Data.Logs;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZFCTAPI.WebApi.Controllers
{
    /// <summary>
    /// 用户方面的功能
    /// </summary>
    [Route("api/[controller]/[action]")]
    [RequestLog]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ICacheManager _cacheManager;
        private readonly ICstUserInfoService _cstUserInfoService;
        private readonly IRedFunctionService _redFunctionService;
        private readonly IPopEnvelopeRedService _redService;
        private readonly IAccountInfoService _accountInfoService;
        private readonly IMessageNoticeService _messageNoticeService;
        private readonly ILoginLogInfoService _loginLogInfoService;
        private readonly IRabbitMQEvent _rabbitMQEvent;

        public CustomerController(ICacheManager cacheManager,
            ICustomerService customerService,
            ICstUserInfoService cstUserInfoService,
            IRedFunctionService redFunctionService,
            IPopEnvelopeRedService redService,
            IAccountInfoService accountInfoService,
            IMessageNoticeService messageNoticeService,
            ILoginLogInfoService loginLogInfoService, IRabbitMQEvent rabbitMQEvent)
        {
            _cacheManager = cacheManager;
            _customerService = customerService;
            _cstUserInfoService = cstUserInfoService;
            _redFunctionService = redFunctionService;
            _redService = redService;
            _accountInfoService = accountInfoService;
            _messageNoticeService = messageNoticeService;
            _loginLogInfoService = loginLogInfoService;
            _rabbitMQEvent = rabbitMQEvent;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RRegisterModel, string> Register([FromBody]SRegisterModel model)
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

            #endregion 验签

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

            if (!Regex.IsMatch(model.UserName, RegularExpression.UserNameRegu))
            {
                registerModel.Message = "用户名必须为5~15位以上的数字或字母！";
                registerModel.ReturnCode = (int)ReturnCode.RegisterEorr;
                return registerModel;
            }
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

            #endregion 校验信息是否满足注册条件

            #region 判断手机号和用户名的可用性

            var isExist = _customerService.GetCustomerByCondition(phone: model.Phone);
            var resgisterInfo = _cstUserInfoService.GetUserInfo(phone: model.Phone);
            if (_cstUserInfoService.CheckUserNameExist(model.UserName))
            {
                registerModel.Message = "用户名已经存在！";
                registerModel.ReturnCode = (int)ReturnCode.RegisterEorr;
                return registerModel;
            }
            if (isExist != null && isExist.IsAdminAccount && resgisterInfo != null)
            {
                registerModel.Message = "用户手机在系统中已存在！";
                registerModel.ReturnCode = (int)ReturnCode.RegisterEorr;
                return registerModel;
            }

            if (isExist != null && !isExist.IsAdminAccount)
            {
                registerModel.Message = "用户手机在系统中已存在！";
                return registerModel;
            }

            #endregion 判断手机号和用户名的可用性

            #region 注册用户信息到数据库

            var customer = _customerService.InsertGuestCustomer();
            customer.Phone = model.Phone;
            customer.cst_invitation_code = _customerService.CreateYqm();
            customer.cst_user_source = model.RequestSource;
            #region 如果邀请人的邀请码填写的是手机号处理

            var inviter = new Customer();
            if (!string.IsNullOrEmpty(model.RecommendCode) && Regex.IsMatch(model.RecommendCode, RegularExpression.Mobile))
            {
                inviter = _customerService.GetCustomerByCondition(phone: model.RecommendCode);
                if (inviter!=null&&inviter.cst_invitation_code != null)
                {
                    model.RecommendCode = inviter.cst_invitation_code;
                }
            }

            #endregion 如果邀请人的邀请码填写的是手机号处理

            var isApproved = CustomerSettings.UserRegistrationType == UserRegistrationType.Standard;
            var registrationRequest = new CustomerRegistrationRequest(customer, model.Email,
                model.UserName, customer.Phone, model.Password, CustomerSettings.DefaultPasswordFormat, model.RecommendCode,null ,isApproved);
            var registerResult = _customerService.RegisterCustomer(registrationRequest);

            #endregion 注册用户信息到数据库

            #region 注册成功后

            if (registerResult.Success)
            {
                customer = _customerService.GetCustomerByCondition(phone: customer.Phone);
                //插入注册信息到cst_user_info 表
                //插入邀请人
                int? recommentUserId = null;
                if (model.RecommendCode != null)
                {
                    var recommandUser = _customerService.GetCustomerByCondition(code: model.RecommendCode);
                    if (recommandUser != null)
                    {
                        recommentUserId = recommandUser.Id;
                    }
                }
                var cstInfo = new CST_user_info
                {
                    cst_recommend_userId = recommentUserId,
                    cst_customer_id = customer.Id, //Customer表ID
                    cst_user_phone = model.Phone, //手机号
                    cst_add_date = DateTime.Now, //添加时间
                    cst_mobile_date = DateTime.Now, //手机验证日期
                    cst_user_name = model.UserName, //用户名
                    cst_user_type = DataDictionary.projecttype_Individual, //用户类型（个人）
                    cst_user_email = null,
                };
                _cstUserInfoService.Add(cstInfo);

                #region 发放注册红包

                _redFunctionService.GrantRedEnvelope(GrantType.RegisteredUsers, customer.Id);
                //给邀请人发放红包
                if (inviter != null && !string.IsNullOrEmpty(inviter.cst_invitation_code))
                {
                    _redFunctionService.GrantRedEnvelope(GrantType.RegisteredUsers, inviter.Id, customer.Id);
                }

                #endregion 发放注册红包

                var token = new TokenModel(customer.Id);
                var register = new RRegisterModel { UserId = customer.Id, UserName = customer.Username };
                registerModel.ReturnData = register;
                registerModel.Message = "注册成功！";
                registerModel.ReturnCode = (int)ReturnCode.Success;
                registerModel.Token = CryptHelper.Encrypt(JsonConvert.SerializeObject(token));
                return registerModel;
            }

            #endregion 注册成功后

            registerModel.Message = "注册失败！";
            registerModel.ReturnCode = (int)ReturnCode.RegisterEorr;
            return registerModel;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RLoginModel, string> Login([FromBody]SLoginModel model)
        {
            var returnModel = new ReturnModel<RLoginModel, string>();

            #region 系统升级维护中,不允许登录
            var systemUpgrade = EngineContext.Current.Resolve<CommonConfig>().SystemUpgrade;
            if (systemUpgrade == "true")
            {
                returnModel.Message = "系统升级维护中...";
                returnModel.ReturnCode = (int)ReturnCode.SystemUpgrade;
                returnModel.ReturnData = null;
                returnModel.Token = model.Token;
                return returnModel;
            }
            #endregion

            #region 验签

            var signResult = VerifyBase.Sign<SLoginModel>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                returnModel.Message = "签名错误";
                returnModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                returnModel.ReturnData = null;
                returnModel.Token = model.Token;
                return returnModel;
            }

            #endregion 验签
            if (!string.IsNullOrEmpty(_cacheManager.Get<String>(model.UserName + "LoginUnavailable")))
            {
                returnModel.ReturnCode = (int)ReturnCode.LoginError;
                returnModel.Message = "账户已冻结！";
                returnModel.ReturnData = null;
                return returnModel;
            }
            var loginResult = _customerService.ValidateUser(model.UserName, model.Password);
            if (loginResult != CustomerLoginResults.Successful)
            {
                returnModel.ReturnCode = (int)ReturnCode.LoginError;
                returnModel.ReturnData = null;
                returnModel.Token = "";
                var cache = 10;
                if (_cacheManager.Get<int?>(model.UserName + "LoginNO") != null)
                {
                    int loginNo = _cacheManager.Get<int>(model.UserName + "LoginNO") + 1;
                    if (loginNo >= 5)
                    {
                        _cacheManager.Set(model.UserName + "LoginUnavailable", "Unavailable", 180);//密码十分钟内连续输入五次,冻结账号3个小时
                        returnModel.Message = "密码错误,账户已冻结！";
                        return returnModel;
                    }
                    _cacheManager.Set(model.UserName + "LoginNO", loginNo, cache);//加缓存记录十分钟内，密码输错的次数
                }
                else
                {
                    _cacheManager.Set(model.UserName + "LoginNO", 1, cache);
                }
                if(loginResult == CustomerLoginResults.CustomerNotExist)
                    returnModel.Message =  "用户名不存在！";
                else if(loginResult == CustomerLoginResults.WrongPassword)
                    returnModel.Message = "登录失败，密码错误！";
                else
                    returnModel.Message = "登录失败！";
                return returnModel;
            }
            else
            {
                #region 后台用户禁止登录

                var customer = _customerService.GetCustomerByCondition(model.UserName);
                if (customer.IsAdminAccount)
                {
                    returnModel.ReturnCode = (int)ReturnCode.LoginError;
                    returnModel.Message = "后台人员不允许登录！";
                    return returnModel;
                }

                var userInfo = _cstUserInfoService.GetUserInfo(phone: customer.Phone);

                if(model.RequestSource!= (int)UserSource.PC&& userInfo.CompanyId != null)
                {
                    returnModel.ReturnCode = (int)ReturnCode.LoginError;
                    returnModel.Message = "企业户请在PC登录";
                    return returnModel;
                }

                if (model.IsCompanyLogin&& userInfo.CompanyId == null)
                {
                    returnModel.ReturnCode = (int)ReturnCode.LoginError;
                    returnModel.Message = "请前往个人用户登录页面登录";
                    return returnModel;
                }

                if(!model.IsCompanyLogin&& userInfo.CompanyId != null)
                {
                    returnModel.ReturnCode = (int)ReturnCode.LoginError;
                    returnModel.Message = "请前往企业用户登录页面登录";
                    return returnModel;
                }


                #endregion 后台用户禁止登录

                #region 赋值Token

                var token = new TokenModel(customer.Id);

                #endregion 赋值Token

                returnModel.ReturnCode = (int)ReturnCode.Success;
                returnModel.Message = "登录成功";

                var loModel = new RLoginModel
                {
                    UserName = customer.Username,
                    UserId = customer.Id,
                    Phone = customer.Phone,
                    IsCompanyLogin = model.IsCompanyLogin ? true : false,
                    Password = ""
                };
                if (model.IsCompanyLogin)
                {
                    var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId:userInfo.Id);
                    loModel.IsGur = (accountInfo!=null&&accountInfo.act_business_property!=null&&accountInfo.act_business_property==5)?"1":"0";
                }
                var cstUserInfo = _cstUserInfoService.GetUserInfo(customer.Id);
                if (cstUserInfo != null)
                {
                    loModel.PicUrl = FastDFSHelper.GetImageAbsolutePath(cstUserInfo.cst_user_pic);
                }
                returnModel.ReturnData = loModel;
                //加密Token
                returnModel.Token = CryptHelper.Encrypt(JsonConvert.SerializeObject(token));

                //登录成功清除登录失败记录
                _cacheManager.Remove(model.UserName + "LoginUnavailable");
                _cacheManager.Remove(model.UserName + "LoginNO");
                _cacheManager.Remove(customer.Phone + "LoginUnavailable");
                _cacheManager.Remove(customer.Phone + "LoginNO");

                //登录日志
                LoginLogInfo loginLogModel = new LoginLogInfo()
                {
                    Operator = customer.Id,
                    OperatorName = customer.Username,
                    OperatingTime = DateTime.Now,
                    Type = (int)OperateType.LoginIn,
                    CreateDate = DateTime.Now,
                    Source = model.RequestSource,
                    BY2 = customer.Phone,
                    DeviceAddress = model.IPAddress,
                    DeviceType = model.PhoneInfo,
                    VersionNum = model.VersionNum
                };

                _loginLogInfoService.Add(loginLogModel);
                #region 推送MQ
                _rabbitMQEvent.Publish(new UserLoginLog()
                {
                    Operator = customer.Id,
                    OperatorName = customer.Username,
                    OperatingTime = DateTime.Now,
                    Type = (int)OperateType.LoginIn,
                    CreateDate = DateTime.Now,
                    Source = model.RequestSource,
                    BY2 = customer.Phone,
                    DeviceAddress = model.IPAddress,
                    DeviceType = model.PhoneInfo,
                    VersionNum = model.VersionNum
                });
                #endregion
                return returnModel;
            }
        }

        /// <summary>
        /// 用户更改手机号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> ChangePhone([FromBody]SChangePhoneModel model)
        {
            var retModel = new ReturnModel<bool, string>();

            #region 校验签名
            var signResult = VerifyBase.SignAndToken<SChangePhoneModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = false;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            #region 校验参数
           
            if (!Regex.IsMatch(model.PhoneNumber, RegularExpression.Mobile))
            {
                retModel.Message = "手机号码格式不正确！";
                retModel.ReturnCode = (int)ReturnCode.RegisterEorr;
                return retModel;
            }

            var verCode = _cacheManager.Get<String>(model.PhoneNumber);
            if (verCode == null || verCode != model.VerCode)
            {
                LogsHelper.WriteLog("手机号码：" + userInfo.cst_user_phone + "; 数据库验证码：" + verCode + "; 传过来的验证码：" + model.VerCode);
                retModel.Message = "手机验证码过期或错误！";
                retModel.ReturnCode = (int)ReturnCode.RegisterEorr;
                return retModel;
            }

            var isExist = _customerService.GetCustomerByCondition(phone: model.PhoneNumber);
            var resgisterInfo = _cstUserInfoService.GetUserInfo(phone: model.PhoneNumber);
            if (isExist != null && isExist.IsAdminAccount && resgisterInfo != null)
            {
                retModel.Message = "用户手机在系统中已存在！";
                retModel.ReturnCode = (int)ReturnCode.RegisterEorr;
                return retModel;
            }

            if (isExist != null && !isExist.IsAdminAccount)
            {
                retModel.Message = "用户手机在系统中已存在！";
                return retModel;
            }

            #endregion 校验参数

            var customer = _customerService.Find(userInfo.cst_customer_id.Value);
            userInfo.cst_user_phone = model.PhoneNumber;
            customer.Phone = model.PhoneNumber;
            _cstUserInfoService.Update(userInfo);
            _customerService.Update(customer);

            retModel.Message = "成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = true;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 用户更改邮箱
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> ChangeEmail([FromBody]SChangeEmailModel model)
        {
            var retModel = new ReturnModel<bool, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SChangeEmailModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = false;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            #region 校验参数

            var verCode = _cacheManager.Get<String>(model.EmailNumber);
            if (verCode == null || verCode != model.VerCode)
            {
                retModel.Message = "邮箱验证码过期或错误！";
                retModel.ReturnCode = (int)ReturnCode.RegisterEorr;
                return retModel;
            }

            var isExist = _customerService.GetCustomerByCondition(email: model.EmailNumber);
            var resgisterInfo = _cstUserInfoService.GetUserInfo(email: model.EmailNumber);
            if (isExist != null && isExist.IsAdminAccount && resgisterInfo != null)
            {
                retModel.Message = "用户邮箱在系统中已存在！";
                retModel.ReturnCode = (int)ReturnCode.RegisterEorr;
                return retModel;
            }

            if (isExist != null && !isExist.IsAdminAccount)
            {
                retModel.Message = "用户邮箱在系统中已存在！";
                return retModel;
            }

            #endregion 校验参数

            var customer = _customerService.Find(userInfo.cst_customer_id.Value);
            userInfo.cst_user_email = model.EmailNumber;
            customer.Email = model.EmailNumber;
            _cstUserInfoService.Update(userInfo);
            _customerService.Update(customer);

            retModel.Message = "成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = true;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 用户修改登陆密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> ChangeLoginPassword([FromBody]SChangeLoginPasswordModel model)
        {
            var retModel = new ReturnModel<bool, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SChangeLoginPasswordModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = false;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            #region 校验参数

            if (!Regex.IsMatch(model.Password, RegularExpression.PassWordRegu))
            {
                retModel.Message = "密码必须为6~16位以上的数字或字母！";
                retModel.ReturnCode = (int)ReturnCode.RegisterEorr;
                return retModel;
            }

            #endregion 校验参数

            var response = _customerService.ChangeLoginPassword(new ChangePasswordRequest(userInfo.cst_customer_id.Value,
                true, CustomerSettings.DefaultPasswordFormat, model.Password,model.OldPassword));
            if (response.Success)
            {
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = true;
                retModel.Message = "密码修改成功！";
                retModel.Token = model.Token;
                return retModel;
            }
            else
            {
                retModel.ReturnCode = (int)ReturnCode.ChangePasswordEorr;
                retModel.ReturnData = false;
                retModel.Message = response.Errors[0];
                retModel.Token = model.Token;
                return retModel;
            }
        }

        /// <summary>
        ///  用户基本信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RUseBaseInfoModel, string> UseBaseInfo([FromBody]BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RUseBaseInfoModel, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            var returnModel = _cstUserInfoService.GetUserBaseInfo(userId);
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = returnModel;
            retModel.Token = model.Token;
            return retModel;
        }


        /// <summary>
        /// 验证用户名是否存在接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> ValidUserNameExit([FromBody]ValidateInfo model)
        {
            ReturnModel<bool, string> reModel = new ReturnModel<bool, string>();

            #region 验签
            var signResult = VerifyBase.Sign<ValidateInfo>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                reModel.Message = "签名错误";
                reModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                reModel.ReturnData = false;
                reModel.Token = model.Token;
                return reModel;
            }
            #endregion

            if (_cstUserInfoService.CheckUserNameExist(model.Validate))
            {
                reModel.Message = "用户名已经存在！";
                reModel.ReturnCode = (int)ReturnCode.DataEorr;
                reModel.ReturnData = false;
                return reModel;
            }

            reModel.Message = "用户名不存在！";
            reModel.ReturnCode = (int)ReturnCode.Success;
            reModel.ReturnData = true;
            return reModel;
        }

        /// <summary>
        /// 验证手机号是否存在接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> ValidPhoneExit([FromBody]ValidateInfo model)
        {
            ReturnModel<bool, string> reModel = new ReturnModel<bool, string>();

            #region 验签
            var signResult = VerifyBase.Sign<ValidateInfo>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                reModel.Message = "签名错误";
                reModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                reModel.ReturnData = false;
                reModel.Token = model.Token;
                return reModel;
            }
            #endregion

            if (_cstUserInfoService.CheckPhoneExist(model.Validate))
            {
                reModel.Message = "手机号已经存在！";
                reModel.ReturnCode = (int)ReturnCode.DataEorr;
                reModel.ReturnData = false;
                return reModel;
            }

            reModel.Message = "手机号不存在！";
            reModel.ReturnCode = (int)ReturnCode.Success;
            reModel.ReturnData = true;
            return reModel;
        }

        /// <summary>
        /// 验证邀请码是否存在接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> ValidRecommandCodeExit([FromBody]ValidateInfo model)
        {
            ReturnModel<bool, string> reModel = new ReturnModel<bool, string>();

            #region 验签
            var signResult = VerifyBase.Sign<ValidateInfo>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                reModel.Message = "签名错误";
                reModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                reModel.ReturnData = false;
                reModel.Token = null;
                return reModel;
            }
            #endregion

            var userinfo3 = _customerService.GetCustomerByCondition(code:model.Validate);
            if (userinfo3 != null)
            {
                reModel.ReturnData = true;
            }
            else
            {
                if (model.Validate != null && Regex.IsMatch(model.Validate, RegularExpression.Mobile))//邀请码输入邀请人手机号
                {
                    var cust = _customerService.GetCustomerByCondition(phone:model.Validate);
                    if (cust != null)
                    {
                        reModel.ReturnData = true;
                        reModel.ReturnCode = (int)ReturnCode.Success;
                        return reModel;
                    }
                }
                reModel.ReturnData = false;
            }
            reModel.ReturnCode = (int)ReturnCode.Success;
            return reModel;
        }
        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> ForgetPwd([FromBody] ForgetPwd model)
        {
            var reModel = new ReturnModel<bool, string>();

            #region 验签
            var signResult = VerifyBase.Sign<ForgetPwd>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                reModel.Message = "签名错误";
                reModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                reModel.ReturnData = false;
                reModel.Token = null;
                return reModel;
            }
            #endregion

            if (String.IsNullOrEmpty(model?.Phone) || String.IsNullOrEmpty(model.VerCode) || String.IsNullOrEmpty(model.Password))
            {
                reModel.ReturnCode = (int)ReturnCode.DataEorr;
                reModel.ReturnData = false;
                reModel.Message = "提交数据错误！";
                reModel.Token = model.Token;
                //reModel.Signature = AddSignature<string>(reModel);
                return reModel;
            }

            if (!Regex.IsMatch(model.Password, RegularExpression.Password))
            {
                reModel.Message = "密码格式不正确！";
                reModel.ReturnCode = (int)ReturnCode.RegisterEorr;
                return reModel;
            }

            var customer = _customerService.GetCustomerByCondition(phone:model.Phone);
            if (_cacheManager.Get<String>(model.Phone) == null || !model.VerCode.Equals(_cacheManager.Get<String>(model.Phone)))
            {
                reModel.ReturnCode = (int)ReturnCode.CheckCodeEorr;
                reModel.ReturnData = false;
                reModel.Message = "验证码输入错误！";
                reModel.Token = model.Token;
                //reModel.Signature = AddSignature<string>(reModel);
                return reModel;
            }
            if (customer == null)
            {
                reModel.ReturnCode = (int)ReturnCode.DataEorr;
                reModel.ReturnData = false;
                reModel.Message = "系统不存在该手机号！";
                reModel.Token = model.Token;
                return reModel;
            }
            var response = _customerService.ChangeLoginPassword(new ChangePasswordRequest(customer.Id,
                   false, CustomerSettings.DefaultPasswordFormat,model.Password));
            if (response.Success)
            {
                //修改密码成功清除缓存
                _cacheManager.Remove(customer.Username + "LoginUnavailable");
                _cacheManager.Remove(customer.Username + "LoginNO");
                _cacheManager.Remove(model.Phone + "LoginUnavailable");
                _cacheManager.Remove(model.Phone + "LoginNO");

                reModel.ReturnCode = (int)ReturnCode.Success;
                reModel.ReturnData = true;
                reModel.Message = "密码修改成功！";
                reModel.Token = model.Token;
                return reModel;
            }
            else
            {
                reModel.ReturnCode = (int)ReturnCode.ChangePasswordEorr;
                reModel.ReturnData = false;
                reModel.Message = "密码修改失败！";
                reModel.Token = model.Token;
                //reModel.Signature = AddSignature<string>(reModel);
                return reModel;
            }
        }

        

        /// <summary>
        /// 验证手机验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<string, string> VerifyPhoneCode([FromBody]SChangePhoneModel model)
        {
            ReturnModel<string, string> reModel = new ReturnModel<string, string>();

            #region 验签
            var signResult = VerifyBase.Sign<SChangePhoneModel>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                reModel.Message = "签名错误";
                reModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                reModel.ReturnData = null;
                reModel.Token = null;
                return reModel;
            }
            #endregion

            if (!string.IsNullOrEmpty(model.VerCode) && _cacheManager.Get<string>(model.PhoneNumber).ToString() == model.VerCode)
            {
                reModel.Message = "手机验证通过！";
                reModel.ReturnCode = (int)ReturnCode.Success;
                return reModel;
            }
            else
            {
                reModel.Message = "手机验证码过期或不正确！";
                reModel.ReturnCode = (int)ReturnCode.DataEorr;
                return reModel;
            }
        }

        /// <summary>
        ///联调时 测试的方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        private ReturnModel<string, string> ToYangRed([FromBody]SRedEnvelopeInfo model)
        {
            var retModel = new ReturnModel<string, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SRedEnvelopeInfo>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            #endregion 校验签名
            //获取商户账户信息
            var envelopesResult = _redService.ToUserRed(model);
            if (envelopesResult == "")
            {
                retModel.Message = "发送错误";
                retModel.ReturnCode = (int) ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }
            else
            {
                retModel.Message = "发送成功";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = envelopesResult;
                retModel.Token = model.Token;
                return retModel;
            }

        }

        #region 微信

        /// <summary>
        /// 微信发送验证码,
        /// 返回判断手机号是否注册
        /// true 已注册 false未注册 null表示企业户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool?, string> WeChatSendPhoneCode([FromBody]SendPhoneCode model)
        {
            var reModel = new ReturnModel<bool?, string>();

            #region 校验签名

            var signResult = VerifyBase.Sign<SendPhoneCode>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                reModel.Message = "签名错误";
                reModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                reModel.ReturnData = false;
                reModel.Token = model.Token;
                return reModel;
            }
            #endregion 校验签名

            string validateCode = EngineContext.Current.Resolve<CommonConfig>().IsTest == "true" ? "888888" : CommonHelper.GenerateRandomDigitCode(6);

            if (Regex.IsMatch(model.Phone, RegularExpression.Mobile))
            {
                var customer = _customerService.GetCustomerByCondition(phone: model.Phone);
                //判断企业户
                if (customer != null)
                {
                    var userInfo = _cstUserInfoService.GetUserInfo(phone: customer.Phone);
                    if (userInfo!=null&&userInfo.CompanyId != null)
                    {
                        reModel.Message = "企业户请前往PC登录";
                        reModel.ReturnCode = (int)ReturnCode.DataEorr;
                        reModel.ReturnData = null;
                        reModel.Token = model.Token;
                        return reModel;
                    }
                }
                //发送SMS
                var result = _messageNoticeService.SendValidateCode(validateCode, model.Phone);
                if (result)
                {
                    _cacheManager.Set(model.Phone, validateCode, 5);

                    reModel.Message = "成功！";
                    reModel.ReturnCode = (int)ReturnCode.Success;
                    reModel.ReturnData = customer == null ? false: true;
                    reModel.Token = model.Token;
                    return reModel;
                }
            }

            reModel.Message = "短信发送失败，无效的手机号码或者短信服务器故障！";
            reModel.ReturnCode = (int)ReturnCode.SendMess;
            reModel.ReturnData = false;
            reModel.Token = model.Token;
            return reModel;
        }

        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RLoginReturnModel,string> WeChatLogin([FromBody]SWeChatLoginModel model)
        {
            var rModel = new ReturnModel<RLoginReturnModel, string>();

            #region 校验签名

            var signResult = VerifyBase.Sign<SWeChatLoginModel>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                rModel.Message = "签名错误";
                rModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                rModel.ReturnData = null;
                rModel.Token = model.Token;
                return rModel;
            }

            #endregion 

            if (!string.IsNullOrEmpty(_cacheManager.Get<String>(model.Phone + "LoginUnavailable")))
            {
                rModel.ReturnCode = (int)ReturnCode.LoginError;
                rModel.Message = "账户已冻结！";
                rModel.ReturnData = null;
                return rModel;
            }

            var customer = _customerService.GetCustomerByCondition(phone: model.Phone);
            if (customer == null)
            {
                rModel.ReturnCode = (int)ReturnCode.LoginError;
                rModel.Message = "用户不存在";
                return rModel;
            }

            //验证手机验证码
            var phoneCode = _cacheManager.Get<String>(model.Phone);
            if (string.IsNullOrEmpty(phoneCode) || !phoneCode.Equals(model.PhoneCode))
            {
                rModel.ReturnCode = (int)ReturnCode.LoginError;
                rModel.ReturnData = null;
                rModel.Token = "";
                var cache = 10;
                if (_cacheManager.Get<int?>(model.Phone + "LoginNO") != null)
                {
                    int loginNo = _cacheManager.Get<int>(model.Phone + "LoginNO") + 1;
                    if (loginNo >= 5)
                    {
                        _cacheManager.Set(model.Phone + "LoginUnavailable", "Unavailable", 180);//密码十分钟内连续输入五次,冻结账号3个小时
                        rModel.Message = "密码错误,账户已冻结！";
                        return rModel;
                    }
                    _cacheManager.Set(model.Phone + "LoginNO", loginNo, cache);//加缓存记录十分钟内，密码输错的次数
                }
                else
                {
                    _cacheManager.Set(model.Phone + "LoginNO", 1, cache);
                }
                rModel.Message = "登录失败，验证码错误或过期！";
                return rModel;
            }

            if (customer.IsAdminAccount)
            {
                rModel.ReturnCode = (int)ReturnCode.LoginError;
                rModel.Message = "后台人员不允许登录！";
                return rModel;
            }

            var userInfo = _cstUserInfoService.GetUserInfo(phone: customer.Phone);
            if (userInfo!=null&&userInfo.CompanyId != null)
            {
                rModel.ReturnCode = (int)ReturnCode.LoginError;
                rModel.Message = "企业户请往PC登录！";
                return rModel;
            }

            TokenModel token = new TokenModel(customer.Id);
            rModel.ReturnCode = (int)ReturnCode.Success;
            rModel.Message = "登录成功";
            RLoginReturnModel loModel = new RLoginReturnModel();
            loModel.UserName = customer.Username;
            loModel.UserId = customer.Id;
            loModel.Phone = customer.Phone;
            loModel.Password = "";
            var cstUserInfo = _cstUserInfoService.GetUserInfo(customer.Id);
            if (cstUserInfo != null)
            {
                loModel.picUrl = FastDFSHelper.GetImageAbsolutePath(cstUserInfo.cst_user_pic);
            }
            rModel.ReturnData = loModel;
            //加Token
            rModel.Token = CryptHelper.Encrypt(JsonConvert.SerializeObject(token));

            #region 清除登录失败缓存
            _cacheManager.Remove(customer.Phone + "LoginUnavailable");
            _cacheManager.Remove(customer.Phone + "LoginNO");
            _cacheManager.Remove(customer.Username + "LoginUnavailable");
            _cacheManager.Remove(customer.Username + "LoginNO");
            #endregion

            #region 登录日志

            LoginLogInfo loginLogModel = new LoginLogInfo()
            {
                Operator = customer.Id,
                OperatorName = customer.Username,
                OperatingTime = DateTime.Now,
                Type = (int)OperateType.LoginIn,
                CreateDate = DateTime.Now,
                Source = (int)UserSource.WeChat,
                BY2 = customer.Phone,
                DeviceAddress = model.IPAddress,
                //DeviceType = model.PhoneInfo,
                //VersionNum = model.VersionNum
            };

            _loginLogInfoService.Add(loginLogModel);

            #endregion

            return rModel;
        }

        #endregion
    }
}