using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Data.BoHai.ReturnModels;
using ZFCTAPI.Data.BoHai.SubmitModels;
using ZFCTAPI.Data.BoHai.TransferData;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Services.BatchImport;
using ZFCTAPI.Services.Helpers;
using ZFCTAPI.Services.UserInfo;
using ZFCTAPI.Services.Views;
using ZFCTAPI.WebApi.Validates;

namespace ZFCTAPI.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class BatchImportController : Controller
    {
        private readonly IBatchImportService _batchImportService;
        private readonly IAccountInfoService _accountInfoService;
        private readonly IAccountBatchImportService _accountBatchImportService;
        private readonly ILoanTextHelper _loanTextHelper;
        private readonly ZfctWebConfig _zfctWebConfig;

        public BatchImportController(IBatchImportService batchImportService, 
            IAccountInfoService accountInfoService,
            IAccountBatchImportService accountBatchImportService,
            ILoanTextHelper loanTextHelper,
            ZfctWebConfig zfctWebConfig)
        {
            _batchImportService = batchImportService;
            _accountInfoService = accountInfoService;
            _accountBatchImportService = accountBatchImportService;
            _loanTextHelper = loanTextHelper;
            _zfctWebConfig = zfctWebConfig;
        }

        #region 存量用户迁移
        /// <summary>
        /// 上传用户注册文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RequestModel, string> UploadUerRegisterFile([FromBody]BatchAccountData pos)
        {
            var retModel = new ReturnModel<RequestModel, string>();
            //获取视图中数据
            var importData = _accountBatchImportService.GetImportModels(pos.start, pos.end);

            //将视图中数据转化为TXT   将Txt上传到Ftp
            var merId = BoHaiApiEngineToConfiguration.InstId(); //商户号
            var batchNo = CommonHelper.GetBatchNo();//流水号
            var fileName = merId + "_" + DateTime.Now.ToString("yyyyMMdd") + "_ExistUserRegister_" + batchNo + ".txt";//存量用户文件名称
            var fileModel = new ExistUserRegisterModel
            {
                char_set = "00",
                partner_id = merId,
                BatchNo = batchNo,
                TotalNum = importData.Count.ToString(),
                AccountImportModels = importData
            };

            var uploadResult = _loanTextHelper.AccountImportFile(fileModel, fileName);
            if (uploadResult == UploadResult.Fail)
            {
                retModel.Message = "上传文件到结算ftp服务器失败，请稍后重试";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = "";
                return retModel;
            }
            else
            {
                var model = new RequestModel()
                {
                    batchNo = batchNo,
                    partner_id = merId,
                    FileName = fileName
                };
                retModel.Message = "上传文件到结算ftp服务器失败，请稍后重试";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = model;
                retModel.Token = "";
                return retModel;
            }
        }

        /// <summary>
        /// 存量用户数据迁移
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RBHRegisterApplication, string> RegisterApplication([FromBody]RequestModel requestModel)
        {
            var retModel = new ReturnModel<RBHRegisterApplication, string>();
            //获取视图中数据
            //var importData = _accountBatchImportService.GetImportModels(pos.start, pos.end);
            ////将视图中数据转化为TXT   将Txt上传到Ftp
            //var merId = BoHaiApiEngineToConfiguration.InstId(); //商户号
            //var batchNo = CommonHelper.GetBatchNo();//流水号
            //var fileName = merId + "_" + DateTime.Now.ToString("yyyyMMdd") + "_ExistUserRegister_" + batchNo + ".txt";//存量用户文件名称
            //var fileModel = new ExistUserRegisterModel
            //{
            //    char_set = "00",
            //    partner_id = merId,
            //    BatchNo = batchNo,
            //    TotalNum = importData.Count.ToString(),
            //    AccountImportModels = importData
            //};
            //var uploadResult = _loanTextHelper.AccountImportFile(fileModel, fileName);
            //if (uploadResult == UploadResult.Fail)
            //{
            //    retModel.Message = "上传文件到结算ftp服务器失败，请稍后重试";
            //    retModel.ReturnCode = (int)ReturnCode.DataEorr;
            //    retModel.ReturnData = null;
            //    retModel.Token = "";
            //    return retModel;
            //}
            //需要写 从ftp读取数据到将读取数据保存到库的一整套方法
            var bgUrl = _zfctWebConfig.LocalUrl + "AsyncRecieve/AsyncExistUserRegisterNotice";
            var model = new TransferUserRegister
            {
                serviceName = "existUserRegister",
                SvcBody = new SBHRegisterApplication()
                {

                    //BatchNo = batchNo,
                    //FileName = fileName,
                    //BgRetUrl = bgUrl,
                    //MerBillNo = CommonHelper.GetMchntTxnSsn(),
                    //partner_id = merId
                    BatchNo = requestModel.batchNo,
                    FileName = requestModel.FileName,
                    BgRetUrl = bgUrl,
                    MerBillNo = CommonHelper.GetMchntTxnSsn(),
                    partner_id = requestModel.partner_id
                }
            };
            var result = _batchImportService.ExistUserRegister(model);

            if (result != null)
            {
                //将成功的记录保存在数据库 记录结算已经响应数据
                retModel.Message = "结算返回成功等待异步回调";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = result;
                retModel.Token = "";
                return retModel;
            }
            else
            {
                retModel.Message = "结算返回失败请查询日志";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = "";
                return retModel;
            }
        }
        #endregion


        #region 存量标的迁移
        /// <summary>
        /// 上传标的信息文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RequestModel, string> UploadLoadTransferFile([FromBody]BatchAccountData pos)
        {
            var retModel = new ReturnModel<RequestModel, string>();
            //获取数据
            var loaninfo = _accountBatchImportService.GetLoanInfo(pos.loanid);
            loaninfo.LoanInvestDetailsModelList = _accountBatchImportService.GetLoanInvestDetails(pos.loanid);
            loaninfo.TotalNum = loaninfo.LoanInvestDetailsModelList.Count.ToString();
            //将视图中数据转化为TXT   将Txt上传到Ftp
            var merId = BoHaiApiEngineToConfiguration.InstId(); //商户号
            var batchNo = CommonHelper.GetBatchNo();//流水号
            var fileName = merId + "_" + DateTime.Now.ToString("yyyyMMdd") + "_FileLoanTransfer_" + batchNo + ".txt";//存量用户文件名称
            loaninfo.MerBillNo = batchNo;
            //处理上传文件
            var uploadResult = _loanTextHelper.LoanImportFile(loaninfo, fileName);
            if (uploadResult == null || uploadResult.Result != UploadResult.Success)
            {
                retModel.Message = "上传文件到结算ftp服务器失败，请稍后重试";
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = null;
                retModel.Token = "";
                return retModel;
            }
            else
            {
                var model = new RequestModel()
                {
                    batchNo = batchNo,
                    partner_id = merId,
                    FileName = fileName,
                    BorrowId = loaninfo.BorrowId,
                    BorrowerAmt = loaninfo.BorrowerAmt
                };
                //将成功的记录保存在数据库 记录结算已经响应数据
                retModel.Message = "结算返回成功等待异步回调";
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = model;
                retModel.Token = "";
                return retModel;
            }
        }

        /// <summary>
        /// 存量标的迁移
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<LoanReturnModelApplication, string> TransferLoan([FromBody]RequestModel model)
        {
            var retModel = new ReturnModel<LoanReturnModelApplication, string>();
            //处理请求参数
            var bgUrl = _zfctWebConfig.LocalUrl + "AsyncRecieve/AsyncTransferLoan";
            var reqModel = new TransferLoanStock
            {
                serviceName = "fileLoanTransfer",
                ReqSvcHeader = new SBHBaseHeaderModel
                {
                    tranSeqNo = model.batchNo
                },
                SvcBody = new LoanRequestModel()
                {
                    partner_id = model.partner_id,
                    MerBillNo = model.batchNo,
                    FileName = model.FileName,
                    BorrowId = model.BorrowId,
                    BorrowerAmt = (model.BorrowerAmt * 100).ToString(),
                    BgRetUrl = bgUrl
                }
            };
            // 发起请求
            var result = _batchImportService.ExistLoanTransfer(reqModel);
            if (result.RspSvcHeader.returnCode == BHReturnCode.Success.ToString())
            {
                //将成功的记录保存在数据库 记录结算已经响应数据
                retModel.Message = result.RspSvcHeader.returnMsg;
                retModel.ReturnCode = (int)ReturnCode.Success;
                retModel.ReturnData = result;
                retModel.Token = "";
                return retModel;
            }
            else
            {
                retModel.Message = result.RspSvcHeader.returnMsg;
                retModel.ReturnCode = (int)ReturnCode.DataEorr;
                retModel.ReturnData = result;
                retModel.Token = "";
                return retModel;
            }
        }
        #endregion


    }

    
}