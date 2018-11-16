using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Data.BoHai.ReturnModels;

namespace ZFCTAPI.Services.Helpers
{
    public interface IBoHaiReconciliationService
    {
        /// <summary>
        /// 下载并处理投资对账文件
        /// </summary>
        void DownloadHandleInvest();
        /// <summary>
        /// 下载并处理充值对账文件
        /// </summary>
        void DownloadHandelRecharge();
        /// <summary>
        /// 下载并处理提现对账文件
        /// </summary>
        void DownloadHandelWithDraw();
        /// <summary>
        /// 下载并处理红包对账文件
        /// </summary>
        void DownloadHandelRed();
        /// <summary>
        /// 测试处理文件
        /// </summary>
        void TestHandleText(string url);
    }

    public class BoHaiReconciliationService:IBoHaiReconciliationService
    {
        public void DownloadHandleInvest()
        {
            var fileByte = FtpUploadHelper.GetFile(UploadFileType.InvestChk, null, out string fileName);
            if (fileByte.Length > 0)
            {
                var sFilePath = LogsHelper.AppPath() + "\\BoHaiFile\\" + "\\Reconciliation\\Invest\\" + DateTime.Now.ToString("yyyyMMdd");
                var path = sFilePath+"\\" + fileName;
                if (!Directory.Exists(sFilePath))//验证路径是否存在
                {
                    //不存在则创建
                    Directory.CreateDirectory(sFilePath);
                }
                //创建新文件若存在则删除
                var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                fs.Seek(0, SeekOrigin.Begin);
                fs.Write(fileByte, 0, fileByte.Length);
                fs.Close();
                #region 处理文件
                HandleTetx(UploadFileType.InvestChk,path);
                #endregion
            }
        }

        public void DownloadHandelRecharge()
        {
            var fileByte = FtpUploadHelper.GetFile(UploadFileType.PpdChk, null, out string fileName);
            if (fileByte.Length > 0)
            {
                var sFilePath = LogsHelper.AppPath() + "\\BoHaiFile\\" + "\\Reconciliation\\Recharge\\" + DateTime.Now.ToString("yyyyMMdd");
                var path = sFilePath+"\\" + fileName;
                if (!Directory.Exists(sFilePath))//验证路径是否存在
                {
                    //不存在则创建
                    Directory.CreateDirectory(sFilePath);
                }
                //创建新文件若存在则删除
                var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                fs.Seek(0, SeekOrigin.Begin);
                fs.Write(fileByte, 0, fileByte.Length);
                fs.Close();
                #region 处理文件
                HandleTetx(UploadFileType.PpdChk, path);
                #endregion
            }
        }

        public void DownloadHandelWithDraw()
        {
            var fileByte = FtpUploadHelper.GetFile(UploadFileType.InvestChk, null, out string fileName);
            if (fileByte.Length > 0)
            {
                var sFilePath = LogsHelper.AppPath() + "\\BoHaiFile\\" + "\\Reconciliation\\Withdraw\\" + DateTime.Now.ToString("yyyyMMdd");
                var path = sFilePath+"\\" + fileName;
                if (!Directory.Exists(sFilePath))//验证路径是否存在
                {
                    //不存在则创建
                    Directory.CreateDirectory(sFilePath);
                }
                //创建新文件若存在则删除
                var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                fs.Seek(0, SeekOrigin.Begin);
                fs.Write(fileByte, 0, fileByte.Length);
                fs.Close();
                #region 处理文件
                HandleTetx(UploadFileType.WdcCHk, path);
                #endregion
            }
        }

        public void DownloadHandelRed()
        {
            var fileByte = FtpUploadHelper.GetFile(UploadFileType.RedCHK, null, out string fileName);
            if (fileByte.Length > 0)
            {
                var sFilePath = LogsHelper.AppPath() + "\\BoHaiFile\\" + "\\Reconciliation\\Red\\" + DateTime.Now.ToString("yyyyMMdd");
                var path = sFilePath + "\\" + fileName;
                if (!Directory.Exists(sFilePath))//验证路径是否存在
                {
                    //不存在则创建
                    Directory.CreateDirectory(sFilePath);
                }
                //创建新文件若存在则删除
                var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                fs.Seek(0, SeekOrigin.Begin);
                fs.Write(fileByte, 0, fileByte.Length);
                fs.Close();
                #region 处理文件
                HandleTetx(UploadFileType.RedCHK, path);
                #endregion
            }
        }

        public void TestHandleText(string url)
        {
            HandleTetx(UploadFileType.InvestChk, url);
        }

        private void HandleTetx(UploadFileType type,string filePath)
        {
            var sR = File.OpenText(filePath);
            string nextLine;
            switch (type)
            {
                case UploadFileType.InvestChk:
                    var investInfos = new  List<RBHBidReconciliation>();
                    while ((nextLine=sR.ReadLine())!=null)
                    {
                        var investInfo=new RBHBidReconciliation();
                        var textInfo = nextLine.Split("|");
                        investInfo.TransId = textInfo[0];
                        investInfo.MercId = textInfo[1];
                        investInfo.PlaCustId = textInfo[2];
                        investInfo.TransAmt = textInfo[3];
                        investInfo.BorrowId = textInfo[4];
                        investInfo.CreDt = textInfo[5];
                        investInfo.CreTm = textInfo[6];
                        investInfo.OrdSts = textInfo[7];
                        investInfo.MerBillNo = textInfo[8];
                        investInfos.Add(investInfo);
                    }
                    #region 处理对账文件

                    #endregion

                    break;
                case UploadFileType.PpdChk:
                    var rechargesInfos = new List<RBHRechargeReconciliation>();
                    while ((nextLine = sR.ReadLine()) != null)
                    {
                        var rechargesInfo = new RBHRechargeReconciliation();
                        var textInfo = nextLine.Split("|");
                        rechargesInfo.OrdNo = textInfo[0];
                        rechargesInfo.CreDt = textInfo[1];
                        rechargesInfo.PlaCustId = textInfo[2];
                        rechargesInfo.TransAmt = textInfo[3];
                        rechargesInfo.FeeAmt = textInfo[4];
                        rechargesInfo.MerBillNo = textInfo[5];
                        rechargesInfos.Add(rechargesInfo);
                    }
                    #region 处理对账文件

                    #endregion

                    break;
                case UploadFileType.WdcCHk:
                    var withdrawInfos = new List<RBHWithDrawReconciliation>();
                    while ((nextLine = sR.ReadLine()) != null)
                    {
                        var withdrawInfo = new RBHWithDrawReconciliation();
                        var textInfo = nextLine.Split("|");
                        withdrawInfo.OrdNo = textInfo[0];
                        withdrawInfo.CreDt = textInfo[1];
                        withdrawInfo.PlaCustId = textInfo[2];
                        withdrawInfo.TransAmt = textInfo[3];
                        withdrawInfo.FeeAmt = textInfo[4];
                        withdrawInfo.MerBillNo = textInfo[5];
                        withdrawInfo.WdcSts = textInfo[6];
                        withdrawInfo.FalRsn = textInfo[7];
                        withdrawInfos.Add(withdrawInfo);
                    }
                    #region 处理对账文件

                    #endregion

                    break;
                case UploadFileType.RedCHK:
                    var redInfos = new List<RBHRedReconciliation>();
                    while ((nextLine = sR.ReadLine()) != null)
                    {
                        var redInfo = new RBHRedReconciliation();
                        var textInfo = nextLine.Split("|");
                        redInfo.TransId = textInfo[0];
                        redInfo.MerBillNo = textInfo[1];
                        redInfo.CreDt = textInfo[2];
                        redInfo.TransAmt = textInfo[3];
                        redInfo.PlaCustId = textInfo[4];
                    }
                    #region 处理对账文件

                    #endregion
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
