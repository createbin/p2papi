using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Infrastructure;

namespace ZFCTAPI.Core.Helpers
{
    public class FtpUploadHelper
    {
        public static UploadResult FtpUploadFile(string filePath, string fileName)
        {
            var ftpInfos = EngineContext.Current.Resolve<FtpConfig>();

            var dict = DateTime.Now.ToString("yyyyMMdd");
            var zfctUrl = "";
            if (DirectoryIsExist(dict))
            {
                zfctUrl = ftpInfos.FtpAddress + "/" + dict + "/" + fileName;
            }
            else
            {
                zfctUrl = MakeDir(dict) + "/" + fileName;
            }
            LogsHelper.WriteLog(zfctUrl);
            //链接ftp服务器
            try
            {
                var request = (FtpWebRequest)WebRequest.Create(zfctUrl);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                //登录ftp服务器
                request.Credentials = new NetworkCredential(ftpInfos.FtpUser, ftpInfos.FtpPassword);
                request.KeepAlive = false;
                request.UseBinary = true;
                request.UsePassive = false;
                request.Timeout = -1; 
                byte[] fileContents = null;
                //复制文件流至文件
                if (filePath.Contains("zip") || filePath.Contains("rar") || filePath.Contains("txt"))
                {
                    fileContents = File.ReadAllBytes(filePath);
                }
                else
                {
                    fileContents = GetFileData(filePath);
                }
                request.ContentLength = fileContents.Length;
                request.ServicePoint.ConnectionLimit = fileContents.Length;                
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                
                response.Close();
                return UploadResult.Success;
            }
            catch (Exception e)
            {
                LogsHelper.WriteLog("还款文件上传失败" + e.ToString());
                return UploadResult.Fail;
            }
        }

        public static UploadResult FtpUploadFile(byte[] fileContents, string fileName)
        {
            var ftpInfos = EngineContext.Current.Resolve<FtpConfig>();

            var dict = DateTime.Now.ToString("yyyyMMdd");
            var zfctUrl = "";
            if (DirectoryIsExist(dict))
            {
                zfctUrl = ftpInfos.FtpAddress + "/" + dict + "/" + fileName;
            }
            else
            {
                zfctUrl = MakeDir(dict) + "/" + fileName;
            }

            //链接ftp服务器
            var request = (FtpWebRequest)WebRequest.Create(zfctUrl);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            //登录ftp服务器
            request.Credentials = new NetworkCredential(ftpInfos.FtpUser, ftpInfos.FtpPassword);
            request.KeepAlive = false;
            request.UseBinary = true;
            request.UsePassive = false;

            //复制文件流至文件
            request.ContentLength = fileContents.Length;
            try
            {
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                #region 记录日志上传信息
                Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);
                #endregion
                response.Close();
                return UploadResult.Success;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// 在FTP文件服务创建文件夹
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public static string MakeDir(string dirName)
        {
            var ftpInfos = EngineContext.Current.Resolve<FtpConfig>();

            var path = ftpInfos.FtpAddress + "/" + dirName;
            var request = (FtpWebRequest)WebRequest.Create(path);
            request.UsePassive = false;
            //登录ftp服务器
            request.Credentials = new NetworkCredential(ftpInfos.FtpUser, ftpInfos.FtpPassword);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            response.Close();
            request.Abort();
            return path;
        }

        /// <summary>
        /// 判断文件夹是否存在
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public static bool DirectoryIsExist(string dirName)
        {
            var ftpInfos = EngineContext.Current.Resolve<FtpConfig>();
            var path = ftpInfos.FtpAddress + "/" + dirName;
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));
            request.UsePassive = false;
            request.UseBinary = true;
            request.Credentials = new NetworkCredential(ftpInfos.FtpUser, ftpInfos.FtpPassword);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string line = reader.ReadLine();
            reader.Close();
            response.Close();
            request.Abort();
            return line != null;
        }

        public static byte[] GetFile(UploadFileType type, DateTime? time, out string fileName)
        {
            fileName = "";
            var ftpInfos = EngineContext.Current.Resolve<FtpConfig>();
            if (time == null)
                time = DateTime.Now;
            var merId = EngineContext.Current.Resolve<BoHaiApiConfig>().InstId;
            var url = ftpInfos.FtpAddress+"/CHK" + "/" + time.Value.ToString("yyyyMMdd");

            switch (type)
            {
                //投资对账
                case UploadFileType.InvestChk:
                    fileName = merId + "_" + time.Value.AddDays(-1).ToString("yyyyMMdd") + "INVEST_chk.txt";
                    break;
                //充值对账
                case UploadFileType.PpdChk:
                    fileName = merId + "_" + time.Value.AddDays(-1).ToString("yyyyMMdd") + "PPD_chk.txt";
                    break;
                //提现对账
                case UploadFileType.WdcCHk:
                    fileName = merId + "_" + time.Value.AddDays(-1).ToString("yyyyMMdd") + "WDC_chk.txt";
                    break;
                //红包对账
                case UploadFileType.RedCHK:
                    fileName = merId + "_" + time.Value.AddDays(-1).ToString("yyyyMMdd") + "EXP_chk.txt";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            url = url + "/" + fileName;
            //创建链接ftp  
            var request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            //登录ftp服务器
            request.Credentials = new NetworkCredential(ftpInfos.FtpUser, ftpInfos.FtpPassword);
            var ms = new MemoryStream();
            try
            {
                using (var response = (FtpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        stream.CopyTo(ms);
                    }
                }
                return ms.ToArray();
            }
            catch
            {
                //如果文件服务器还不存在改文件返回空结果
                return new byte[0];
            }

        }

        /// <summary>
        /// 获取存量用户注册 结果文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static byte[] GetFileImport(string fileName)
        {
            var ftpInfos = EngineContext.Current.Resolve<FtpConfig>();
            var url = ftpInfos.FtpAddress+ "/CHK/" + DateTime.Now.ToString("yyyyMMdd");//ftp文件夹的路径要修改
            url = url + "/" + fileName;
            //创建链接ftp  
            var request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            //登录ftp服务器
            request.Credentials = new NetworkCredential(ftpInfos.FtpUser, ftpInfos.FtpPassword);
            var ms = new MemoryStream();
            try
            {
                using (var response = (FtpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        stream.CopyTo(ms);
                    }
                }
                return ms.ToArray();
            }
            catch
            {
                //如果文件服务器还不存在改文件返回空结果
                return new byte[0];
            }
        }

        private static byte[] GetFileData(string filePath)
        {
            using (var sr = new StreamReader(filePath))
            {
                return Encoding.GetEncoding("GBK").GetBytes(sr.ReadToEnd());
            }
        }
    }
}
