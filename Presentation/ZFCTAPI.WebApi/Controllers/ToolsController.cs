using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Services.InvestInfo;
using ZFCTAPI.Services.UserInfo;
using ZFCTAPI.WebApi.Validates;
using ZFCTAPI.Data.CST;
using System.Text.RegularExpressions;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Services.Messages;
using ZFCTAPI.Core.Caching;
using ZFCTAPI.Data.PRO;
using Microsoft.AspNetCore.Hosting;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.Infrastructure;
using ZFCTAPI.Services.BoHai;
using ZFCTAPI.Data.BoHai.SubmitModels;
using System.Xml;
using ZFCTAPI.WebApi.RequestAttribute;
using System.IO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZFCTAPI.WebApi.Controllers
{
    /// <summary>
    /// 通用功能
    /// </summary>
    [Route("api/[controller]/[action]")]
    [RequestLog]
    public class ToolsController : Controller
    {
        private static IInvestInfoService _investInfoService;
        private static IInvesterPlanService _investerPlanService;
        private static ICstUserInfoService _userInfoService;
        private readonly IMessageNoticeService _messageNoticeService;
        private readonly ICacheManager _cacheManager;
        private readonly IHostingEnvironment _env;
        private readonly IBHUserService _iBHUserService;
        private readonly IAccountInfoService _accountInfoService;
        private readonly IBHRepaymentService _ibhrepaymentService;

        public ToolsController(IInvestInfoService investInfoService,
            IInvesterPlanService investerPlanService,
            ICstUserInfoService userInfoService,
            IMessageNoticeService messageNoticeService,
            ICacheManager cacheManager,
            IHostingEnvironment env,
            IBHUserService iBHUserService,
            IAccountInfoService accountInfoService,
             IBHRepaymentService ibhrepaymentService)
        {
            _investInfoService = investInfoService;
            _investerPlanService = investerPlanService;
            _userInfoService = userInfoService;
            _messageNoticeService = messageNoticeService;
            _cacheManager = cacheManager;
            _env = env;
            _iBHUserService = iBHUserService;
            _accountInfoService = accountInfoService;
            _ibhrepaymentService = ibhrepaymentService;
        }

        /// <summary>
        /// 版本信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RRetRetVersion,string> RetRetVersion([FromBody]SRetRetVersion model)
        {
            var rModel = new ReturnModel<RRetRetVersion, string>();
            RRetRetVersion reV = new RRetRetVersion();

            #region 校验签名

            var signResult = VerifyBase.Sign<SRetRetVersion>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                rModel.Message = "签名错误";
                rModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                rModel.ReturnData = null;
                rModel.Token = model.Token;
                return rModel;
            }

            #endregion 校验签名

            string path = _env.ContentRootPath + "/App_Data";

            XmlDocument doc = new XmlDocument();
            string nodename = "";
            doc.Load(path + @"\MobileAppVersion.xml");
            XmlElement root = null;
            root = doc.DocumentElement;
            XmlNodeList listNodes = null;

            if (model != null &&!string.IsNullOrEmpty(model.Type) && model.Type.ToLower() == "ios")
            {
                nodename = "/update/ios/item";
            }
            else
            {
                nodename = "/update/android/item";
            }

            listNodes = root.SelectNodes(nodename);
            if (listNodes != null)
            {
                XmlNode node = listNodes.Item(listNodes.Count - 1);
                XmlNodeList xnl0 = node.ChildNodes;
                reV.DownloadUrl = xnl0[2].InnerText;
                reV.VersionCode = xnl0[0].InnerText;
                reV.VersionName = xnl0[1].InnerText;
                reV.UpdateLog = xnl0[3].InnerText;
                reV.Mandatoryupdate = xnl0[4].InnerText;
            }

            rModel.Message = "查询成功！";
            rModel.ReturnCode = (int)ReturnCode.Success;
            rModel.ReturnData = reV;

            return rModel;
        }

        /// <summary>
        /// 首页综合统计数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RHomeStatistics, string> HomeStatistics([FromBody]BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RHomeStatistics, string>();

            #region 校验签名

            var signResult = VerifyBase.Sign<BaseSubmitModel>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            var statistics = new RHomeStatistics
            {
                AllTranscationMoney = _investInfoService.SumInvestMoney(),
                AllProfitMoney = _investerPlanService.SumUserGains(),
                AllUser = _userInfoService.RegisterUserCount(),
                SecureDay = (DateTime.Now - Convert.ToDateTime("2016-4-18")).Days
            };
            retModel.Message = "查询成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = statistics;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 向用户发送手机验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> SendMobileVCodeToUser([FromBody]BaseSubmitModel model)
        {
            var retModel = new ReturnModel<bool, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = false;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            string validateCode = EngineContext.Current.Resolve<CommonConfig>().IsTest == "true" ? "888888" : CommonHelper.GenerateRandomDigitCode(6);
            if (Regex.IsMatch(userInfo.cst_user_phone, RegularExpression.Mobile))
            {
                //发送SMS
                var result = _messageNoticeService.SendValidateCode(validateCode, userInfo.cst_user_phone);
                if (result)
                {
                    _cacheManager.Set(userInfo.cst_user_phone, validateCode, 5);
                    retModel.Message = "成功！";
                    retModel.ReturnCode = (int)ReturnCode.Success;
                    retModel.ReturnData = true;
                    retModel.Token = model.Token;
                    return retModel;
                }
            }
            retModel.Message = "短信发送失败，无效的手机号码或者短信服务器故障！";
            retModel.ReturnCode = (int)ReturnCode.SendMess;
            retModel.ReturnData = false;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 向用户渤海手机发送验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> SendMobileVCodeToBhUser([FromBody]SVCodeToMobile model)
        {
            var retModel = new ReturnModel<bool, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SVCodeToMobile>(model, out int userId);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = false;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名
            var accountInfo = _accountInfoService.GetAccountInfoByUserId(userId);
            if (accountInfo != null && accountInfo.JieSuan && accountInfo.BoHai) {
                if (!accountInfo.act_user_phone.Equals(model.MobileNumber)) {
                    retModel.Message = "原手机号不正确";
                    retModel.ReturnCode = (int)ReturnCode.DataEorr;
                    retModel.ReturnData = false;
                    retModel.Token = model.Token;
                    return retModel;
                }
                string validateCode = EngineContext.Current.Resolve<CommonConfig>().IsTest == "true" ? "888888" : CommonHelper.GenerateRandomDigitCode(6);
                if (Regex.IsMatch(accountInfo.act_user_phone, RegularExpression.Mobile))
                {
                    //发送SMS
                    var result = _messageNoticeService.SendValidateCode(validateCode, accountInfo.act_user_phone);
                    if (result)
                    {
                        _cacheManager.Set(accountInfo.act_user_phone, validateCode, 5);
                        retModel.Message = "成功！";
                        retModel.ReturnCode = (int)ReturnCode.Success;
                        retModel.ReturnData = true;
                        retModel.Token = model.Token;
                        return retModel;
                    }
                }
                retModel.Message = "短信发送失败，无效的手机号码或者短信服务器故障！";
                retModel.ReturnCode = (int)ReturnCode.SendMess;
                retModel.ReturnData = false;
                retModel.Token = model.Token;
                return retModel;
            }
            retModel.Message = "用户尚未完成开户或绑卡！";
            retModel.ReturnCode = (int)ReturnCode.DataEorr;
            retModel.ReturnData = false;
            retModel.Token = model.Token;
            return retModel;


        }

        /// <summary>
        /// 向邮箱发送验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> SendVCodeToEmail([FromBody]SVCodeToEmail model)
        {
            var retModel = new ReturnModel<bool, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<SVCodeToEmail>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = false;
                return retModel;
            }

            string validateCode = EngineContext.Current.Resolve<CommonConfig>().IsTest == "true" ? "888888" : CommonHelper.GenerateRandomDigitCode(6);
            if (CommonHelper.IsValidEmail(model.EmailCode))
            {
                //发送SMS
                userInfo.cst_user_email = model.EmailCode;
                var p2pInfo = new P2PInfo { UserInfo = userInfo };
                var result = _messageNoticeService.SendEmailActiveMsg(p2pInfo, validateCode);
                if (result)
                {
                    _cacheManager.Set(model.EmailCode, validateCode, 5);
                    retModel.Message = "成功！";
                    retModel.ReturnCode = (int)ReturnCode.Success;
                    retModel.ReturnData = true;
                    return retModel;
                }
            }

            #endregion 校验签名

            retModel.Message = "邮箱发送失败，无效的邮箱或者邮箱服务器故障！";
            retModel.ReturnCode = (int)ReturnCode.SendMess;
            retModel.ReturnData = false;
            return retModel;
        }

        /// <summary>
        /// 向手机发送验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> SendVCodeToMobile([FromBody]SVCodeToMobile model)
        {
            var retModel = new ReturnModel<bool, string>();

            #region 校验签名

            var signResult = VerifyBase.Sign<SVCodeToMobile>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = false;
                return retModel;
            }

            #endregion 校验签名

            string validateCode = EngineContext.Current.Resolve<CommonConfig>().IsTest=="true" ? "888888" : CommonHelper.GenerateRandomDigitCode(6);
            if (Regex.IsMatch(model.MobileNumber, RegularExpression.Mobile))
            {
                //发送SMS
                var result = _messageNoticeService.SendValidateCode(validateCode, model.MobileNumber);
                if (result)
                {
                    _cacheManager.Set(model.MobileNumber, validateCode, 5);
                    retModel.Message = "成功！";
                    retModel.ReturnCode = (int)ReturnCode.Success;
                    retModel.ReturnData = true;
                    return retModel;
                }
            }
            retModel.Message = "短信发送失败，无效的手机号码或者短信服务器故障！";
            retModel.ReturnCode = (int)ReturnCode.SendMess;
            retModel.ReturnData = false;
            return retModel;
        }

        /// <summary>
        /// 发送向用户手机发送投资验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool, string> SendInvestVCode([FromBody]BaseSubmitModel model)
        {
            var retModel = new ReturnModel<bool, string>();

            #region 校验签名

            var signResult = VerifyBase.SignAndToken<BaseSubmitModel>(model, out CST_user_info userInfo);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = false;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            var vCode =  _iBHUserService.SendUapMsg(new SBHSendUapMsg
            {
                SvcBody = new SBHSendUapMsgBody
                {
                    mobileNo = userInfo.cst_user_phone
                }
            });

            if (!string.IsNullOrEmpty(vCode)) {
                retModel.Message = "成功！";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = true;
                retModel.Token = model.Token;
                return retModel;
            }
            retModel.Message = "短信发送失败，无效的手机号码或者短信服务器故障！";
            retModel.ReturnCode = (int)ReturnCode.SendMess;
            retModel.ReturnData = false;
            retModel.Token = model.Token;
            return retModel;
        }

        /// <summary>
        /// 渤海银行开户支持的银行卡列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RBankInfos, string> BankInfos([FromBody] BaseSubmitModel model)
        {
            var retModel = new ReturnModel<RBankInfos, string>();

            #region 校验签名

            var signResult = VerifyBase.Sign<SVCodeToMobile>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                retModel.Message = "签名错误";
                retModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                retModel.ReturnData = null;
                retModel.Token = model.Token;
                return retModel;
            }

            #endregion 校验签名

            var banks = new List<BankInfo>
            {
                new BankInfo{BankCode = "ICBC",BankName = "工商银行"},
                new BankInfo{BankCode = "ABC",BankName = "农行"},
                new BankInfo{BankCode = "CMB",BankName = "招行"},
                new BankInfo{BankCode = "CCB",BankName = "建设银行"},
                new BankInfo{BankCode = "BOC",BankName = "中国银行"},
                new BankInfo{BankCode = "BCOM",BankName = "交通银行"},
                new BankInfo{BankCode = "CMBC",BankName = "民生银行"},
                new BankInfo{BankCode = "CIB",BankName = "兴业银行"},
                new BankInfo{BankCode = "CBHB",BankName = "渤海银行"},
                new BankInfo{BankCode = "GDB",BankName = "广发银行"},
                new BankInfo{BankCode = "HXB",BankName = "华夏银行"},
                new BankInfo{BankCode = "PAB",BankName = "平安银行"},
                new BankInfo{BankCode = "PSBC",BankName = "邮储银行"},
                new BankInfo{BankCode = "SPDB",BankName = "上海浦东发展银行"},
                new BankInfo{BankCode = "CEB",BankName = "光大银行"}
            };
            var result = new RBankInfos {BankInfos = banks};
            retModel.Message = "成功";
            retModel.ReturnCode = (int)ReturnCode.Success;
            retModel.ReturnData = result;
            retModel.Token = model.Token;
            return retModel;
        }

        #region 投资合同

        [HttpPost]
        public ReturnModel<bool, string> UploadLoanZipFile([FromBody]SContractFileUploadModel model)
        {
            var rModel = new ReturnModel<bool, string>();

            #region 校验签名

            var signResult = VerifyBase.Sign<SContractFileUploadModel>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                rModel.Message = "签名错误";
                rModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                return rModel;
            }

            #endregion 校验签名

            var instId = BoHaiApiEngineToConfiguration.InstId(); //商户号
            var merBillNo = CommonHelper.GetMchntTxnSsn();//流水号
            var fileName = instId + "_" + DateTime.Now.ToString("yyyyMMdd") + "_ContractFileUpload_" + merBillNo + ".zip";
            //DFS下载文件
            var file = FastDFSHelper.DownFastDFS(model.fileName);
            if (file.Length == 0) {
                rModel.ReturnCode = (int)ReturnCode.DataFormatError;
                rModel.Message = "DFS文件下载失败";
                return rModel;
            }
            //上传文件到FTP
            FtpUploadHelper.FtpUploadFile(file, fileName);


            var rqModel = new SBContractFileUpload
            {
                SvcBody = new SBContractFileUploadModel
                {
                    projectCode = model.projectCode,
                    fileName = fileName,
                    projectType = ProjectType.Creditor,
                    comment = model.comment,
                    extension = model.extension,
                    contractNo = model.contractNo
                }
            };
            rqModel.ReqSvcHeader.tranSeqNo = merBillNo;

            var result = _ibhrepaymentService.ContractFileUpload(rqModel);

            if (result != null && result.RspSvcHeader.returnCode == JSReturnCode.Success.ToString())
            {
                rModel.ReturnCode = (int)ReturnCode.Success;
                rModel.Message = "上传成功";
            }
            else
            {
                rModel.ReturnCode = (int)ReturnCode.DataFormatError;
                rModel.Message = "上传失败";
            }

            return rModel;
        }

        #endregion
    }
}