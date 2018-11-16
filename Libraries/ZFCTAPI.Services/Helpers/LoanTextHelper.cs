using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Data.BoHai.SubmitModels;
using ZFCTAPI.Data.BoHai.TransferData;
using ZFCTAPI.Data.CST;

namespace ZFCTAPI.Services.Helpers
{

    public interface ILoanTextHelper
    {
        /// <summary>
        /// 还款文件生成
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        /// <returns>还款文件路径</returns>
        Task<UploadResult> FileRelease(SBHRepaymentFile file, string fileName);

        /// <summary>
        /// 划转文件生成
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<UploadResult> RaiseLoanResultFile(SBHRaiseLoanResultFile file, string fileName);

        /// <summary>
        /// 流标撤销申购文件生成
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<UploadResult> RaiseLoanFailFile(SBHBatInvestCancleFile file, string fileName);
        /// <summary>
        /// TODO：生成用户数据上传文件
        /// </summary>
        /// <param name="file">生成的文件实体</param>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        UploadResult AccountImportFile(ExistUserRegisterModel file, string fileName);

        /// <summary>
        /// TODO：标的数据迁移,生成标的数据文本（包含标的汇总数据及投资明细数据）
        /// </summary>
        /// <param name="loanCountModel">标的汇总数据Model</param>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        Task<UploadResult> LoanImportFile(LoanCountModel loanCountModel, string fileName);
    }

    public class LoanTextHelper : ILoanTextHelper
    {
        public  async Task<UploadResult> FileRelease(SBHRepaymentFile file, string fileName)
        {
            var filePath = LogsHelper.AppPath() + "\\BoHaiFile\\" + "\\FileRelease\\" + DateTime.Now.ToString("yyyyMMdd");
            var sFile = filePath + "\\" + fileName;
            if (!Directory.Exists(filePath)) //验证路径是否存在
            {
                //不存在则创建
                Directory.CreateDirectory(filePath);
            }
            var fs = System.IO.File.Exists(sFile)
                ? new FileStream(sFile, FileMode.Open, FileAccess.Write)
                : new FileStream(sFile, FileMode.Create, FileAccess.Write);
            //清空文本
            fs.SetLength(0);
            var sw = new StreamWriter(fs);
            //写入汇总信息
            var sb = new StringBuilder();
            sb.Append(file.char_set + "|");
            sb.Append(file.partner_id + "|");
            sb.Append(file.MerBillNo + "|");
            sb.Append(file.TransAmt + "|");
            sb.Append(file.FeeAmt + "|");
            sb.Append(file.BorrowId + "|");
            sb.Append(file.BorrowerAmt + "|");
            sb.Append(file.BorrCustId + "|");
            sb.Append(file.MerPriv + "|");
            sb.Append(file.TotalNum);
            sb.Append("|");
            //写入还款明细
            sw.WriteLine(sb.ToString());
            foreach (var sbhRepaymentFilePerson in file.InvestPersons)
            {
                var repaymentInfo = sbhRepaymentFilePerson.ID + "|" + sbhRepaymentFilePerson.PlaCustId + "|"
                                    + sbhRepaymentFilePerson.TransAmt + "|" + sbhRepaymentFilePerson.Interest + "|"
                                    + sbhRepaymentFilePerson.Inves_fee+"|";
                sw.WriteLine(repaymentInfo);
            }


            sw.Close();
            fs.Close();
            #region 上传文件到文件服务器
            //var fileInfo = new FileInfo(sFile);
            //await FastDFSHelper.TextFastDfsAsync(fileInfo);
            #endregion
            #region 上传文件到ftp
            var  tryCount = 0;
            var result = UploadResult.Fail;//由于对方文件服务器不稳定尝试上传3次
            while (tryCount<3&&result==UploadResult.Fail)
            {
                LogsHelper.WriteLog("进入放款上传:" + tryCount);
                tryCount++;
                result = FtpUploadHelper.FtpUploadFile(sFile, fileName);
            }
            return result;
            #endregion
        }

        public async Task<UploadResult> RaiseLoanFailFile(SBHBatInvestCancleFile file, string fileName)
        {
            var filePath = LogsHelper.AppPath() + "\\BoHaiFile\\" + "\\RaiseLoanFail\\" + DateTime.Now.ToString("yyyyMMdd");
            var sFile = filePath + "\\" + fileName;

            if (!Directory.Exists(filePath)) //验证路径是否存在
            {
                //不存在则创建
                Directory.CreateDirectory(filePath);
            }
            var fs = System.IO.File.Exists(sFile)
                ? new FileStream(sFile, FileMode.Open, FileAccess.Write)
                : new FileStream(sFile, FileMode.Create, FileAccess.Write);
            //清空文本
            fs.SetLength(0);
            var sw = new StreamWriter(fs);
            //写入汇总信息
            var sb = new StringBuilder();
            sb.Append(file.char_set + "|");
            sb.Append(file.partner_id + "|");
            sb.Append(file.BatchNo + "|");
            sb.Append(file.TransDate + "|");
            sb.Append(file.TotalNum);

            //写入撤销第一行
            sw.WriteLine(sb.ToString());
            //写入投资人信息
            foreach (var sbhRepaymentFilePerson in file.InvestCancleDetail)
            {
                var repaymentInfo = sbhRepaymentFilePerson.ID + "|" + sbhRepaymentFilePerson.OldTransId + "|"
                                    + sbhRepaymentFilePerson.PlaCustId + "|" + sbhRepaymentFilePerson.TransAmt + "|"
                                    + sbhRepaymentFilePerson.FreezeId;
                sw.WriteLine(repaymentInfo);
            }

            sw.Close();
            fs.Close();

            #region 上传文件到文件服务器
            var fileInfo = new FileInfo(sFile);
            await FastDFSHelper.TextFastDfsAsync(fileInfo);
            #endregion
            #region 上传文件到ftp

            var tryCount = 0;
            var result = UploadResult.Fail;//由于对方文件服务器不稳定尝试上传3次
            while (tryCount < 3 && result == UploadResult.Fail)
            {
                LogsHelper.WriteLog("进入流标上传:" + tryCount);
                tryCount++;
                result = FtpUploadHelper.FtpUploadFile(sFile, fileName);
            }
            return result;


            //var result = FtpUploadHelper.FtpUploadFile(sFile, fileName);
            #endregion

            //return result;
        }

        public async Task<UploadResult> RaiseLoanResultFile(SBHRaiseLoanResultFile file, string fileName)
        {
            var filePath = LogsHelper.AppPath() + "\\BoHaiFile\\" + "\\RaiseLoan\\" + DateTime.Now.ToString("yyyyMMdd");
            var sFile = filePath + "\\" + fileName;
            if (!Directory.Exists(filePath)) //验证路径是否存在
            {
                //不存在则创建
                Directory.CreateDirectory(filePath);
            }
            
            var fs = System.IO.File.Exists(sFile)
                ? new FileStream(sFile, FileMode.Open, FileAccess.Write)
                : new FileStream(sFile, FileMode.Create, FileAccess.Write);
            //清空文本
            fs.SetLength(0);
            var sw = new StreamWriter(fs);
            //写入汇总信息
            var sb = new StringBuilder();
            sb.Append(file.char_set + "|");
            sb.Append(file.partner_id + "|");
            sb.Append(file.MerBillNo + "|");
            sb.Append(file.TransAmt + "|");
            sb.Append(file.FeeAmt + "|");
            sb.Append(file.BorrowId + "|");
            sb.Append(file.BorrowerAmt + "|");
            sb.Append(file.BorrCustId + "|");
            sb.Append(file.ReleaseType + "|");
            sb.Append(file.MerPriv + "|");
            sb.Append(file.TotalNum);
            sb.Append("|");
            //写入投资明细
            sw.WriteLine(sb.ToString());
            foreach (var investDetail in file.InvestDetails)
            {
                var repaymentInfo = investDetail.ID + "|" + investDetail.PlaCustId + "|"
                                    + investDetail.TransAmt + "|" + investDetail.FreezeId+"|";
                sw.WriteLine(repaymentInfo);
            }

            sw.Close();
            fs.Close();

            #region 上传文件到文件服务器
            //var fileInfo = new FileInfo(sFile);
            //await FastDFSHelper.TextFastDfsAsync(fileInfo);
            #endregion

            //#region 上传文件到ftp

            //try
            //{
            //    var result = FtpUploadHelper.FtpUploadFile(sFile, fileName);
            //    return result;
            //}
            //catch (Exception e)
            //{
            //    LogsHelper.WriteLog("满标文件上传失败，错误信息,报错时间："+DateTime.Now+"，错误信息："+e.ToString());
            //    return UploadResult.Fail;
            //}
            //#endregion
            #region 上传文件到ftp
            var tryCount = 0;
            var result = UploadResult.Fail;//由于对方文件服务器不稳定尝试上传3次
            while (tryCount < 3 && result == UploadResult.Fail)
            {
                LogsHelper.WriteLog("进入满标上传:"+tryCount);
                tryCount++;
                result = FtpUploadHelper.FtpUploadFile(sFile, fileName);
            }
            return result;
            #endregion
        }

        public UploadResult AccountImportFile(ExistUserRegisterModel file, string fileName)
        {
            var filePath = LogsHelper.AppPath() + "\\BoHaiFile\\" + "\\RaiseLoanFail\\" + DateTime.Now.ToString("yyyyMMdd");
            var sFile = filePath + "\\" + fileName;

            if (!Directory.Exists(filePath)) //验证路径是否存在
            {
                //不存在则创建
                Directory.CreateDirectory(filePath);
            }
            var fs = System.IO.File.Exists(sFile)
                ? new FileStream(sFile, FileMode.Open, FileAccess.Write)
                : new FileStream(sFile, FileMode.Create, FileAccess.Write);
            //清空文本
            fs.SetLength(0);
            var sw = new StreamWriter(fs, Encoding.GetEncoding("GBK"));
            //写入汇总信息
            var sb = new StringBuilder();
            sb.Append(file.char_set + "|");
            sb.Append(file.partner_id + "|");
            sb.Append(file.BatchNo + "|");
            sb.Append(file.TransDate + "|");
            sb.Append(file.TotalNum);

            //写入撤销第一行
            sw.WriteLine(sb.ToString());
            //写入投资人信息
            foreach (var sbhRepaymentFilePerson in file.AccountImportModels)
            {
                var repaymentInfo = sbhRepaymentFilePerson.IDENTITY_TYPE + "|" + sbhRepaymentFilePerson.IDCARD + "|"
                                    + sbhRepaymentFilePerson.TRUENAME + "|" + sbhRepaymentFilePerson.PHONENUM + "|"
                                    + sbhRepaymentFilePerson.BANKCODE + "|"
                                    + sbhRepaymentFilePerson.BANKCARD + "|"
                                    + sbhRepaymentFilePerson.OPEN_TYPE;
                sw.WriteLine(repaymentInfo);
            }

            sw.Close();
            fs.Close();

            #region 上传文件到ftp

            var tryCount = 0;
            var result = UploadResult.Fail;//由于对方文件服务器不稳定尝试上传3次
            while (tryCount < 3 && result == UploadResult.Fail)
            {
                LogsHelper.WriteLog("导入用户文件上传文件上传:" + tryCount);
                tryCount++;
                result = FtpUploadHelper.FtpUploadFile(sFile, fileName);
            }
            return result;


            //var result = FtpUploadHelper.FtpUploadFile(sFile, fileName);
            #endregion
        }
        
        public async Task<UploadResult> LoanImportFile(LoanCountModel loanCountModel, string fileName)
        {
            var filePath = LogsHelper.AppPath() + "\\BoHaiFile\\" + "\\LoanFile\\" + DateTime.Now.ToString("yyyyMMdd");
            var sFile = filePath + "\\" + fileName;
            //验证路径是否存在
            if (!Directory.Exists(filePath)) 
                Directory.CreateDirectory(filePath);//不存在则创建
            var fs = System.IO.File.Exists(sFile) ? new FileStream(sFile, FileMode.Open, FileAccess.Write) : new FileStream(sFile, FileMode.Create, FileAccess.Write);
            fs.SetLength(0);//清空文本
            var sw = new StreamWriter(fs, Encoding.GetEncoding("GBK"));
            //写入汇总信息
            var sb = new StringBuilder();
            sb.Append(loanCountModel.partner_id + "|");
            sb.Append(loanCountModel.MerBillNo + "|");
            sb.Append(loanCountModel.BorrowId + "|");
            sb.Append(loanCountModel.BorrowTyp + "|");
            sb.Append(loanCountModel.BorrowerAmt * 100 + "|");
            sb.Append(loanCountModel.BorrowerInterestAmt + "|");
            sb.Append(loanCountModel.BorrCustId + "|");
            sb.Append(loanCountModel.AccountName + "|");
            sb.Append(loanCountModel.GuaranteeNo + "|");
            sb.Append(loanCountModel.BorrowerStartDate.ToString("yyyyMMdd") + "|");
            sb.Append(loanCountModel.BorrowerEndDate.ToString("yyyyMMdd") + "|");
            sb.Append(loanCountModel.BorrowerRepayDate.ToString("yyyyMMdd") + "|");
            sb.Append(loanCountModel.ReleaseType + "|");
            sb.Append(loanCountModel.InvestDateType + "|");
            sb.Append(loanCountModel.InvestPeriod + "|");
            sb.Append(loanCountModel.BorrowerDetails + "|");
            sb.Append(loanCountModel.TotalNum + "|");
            sb.Append(loanCountModel.MerPriv1 + "|");
            sb.Append(loanCountModel.MerPriv2);
            sb.Append("|");
            //写入第一行投资明细汇总
            sw.WriteLine(sb.ToString());
            //2-n 行投资数据
            foreach (var item in loanCountModel.LoanInvestDetailsModelList)
            {
                var loanInvestDetails = item.ID + "|" + item.InvestMerNo + "|" + item.PlaCustId + "|" +
                    item.TransAmt * 100 + "|" + item.MerPriv;
                sw.WriteLine(loanInvestDetails);
            }
            sw.Close();
            fs.Close();

            #region 上传文件到ftp

            var tryCount = 0;
            var result = UploadResult.Fail;//由于对方文件服务器不稳定尝试上传3次
            while (tryCount < 3 && result == UploadResult.Fail)
            {
                LogsHelper.WriteLog("导入用户文件上传文件上传:" + tryCount);
                tryCount++;
                result = FtpUploadHelper.FtpUploadFile(sFile, fileName);
            }
            #endregion
            return result;
        }
    }


}
